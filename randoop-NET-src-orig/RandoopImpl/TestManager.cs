//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Common;
using System.Collections.ObjectModel;
using System.Security;

namespace Randoop
{

    public class PlanManager
    {
        RandoopConfiguration config;

        // Counter used to print out a dot for every 1000 plans.
        private int addCounter = 0;

        private int redundantAdds = 0;

        public int Plans
        {
            get
            {
                return addCounter;
            }
        }

        public int RedundantAdds
        {
            get
            {
                return redundantAdds;
            }
        }

        public readonly PlanDataBase exceptionPlans;
        public readonly PlanDataBase builderPlans;
        public readonly PlanDataBase observerPlans;

        private ITestFileWriter testFileWriter;

        public static int numContractViolatingPlans = 0; //to count number of exception + contract violating plans
        public static int numDistinctContractViolPlans = 0; // counts the # of plans that violates contract (may violate same contract)

        public PlanManager(RandoopConfiguration config)
        {
            this.config = config;
            this.builderPlans = new PlanDataBase("builderPlans", config.typematchingmode);
            this.exceptionPlans = new PlanDataBase("exceptionThrowingPlans", config.typematchingmode);
            this.observerPlans = new PlanDataBase("observerPlans", config.typematchingmode);

            Plan.uniqueIdCounter = config.planstartid;

            if (config.singledir)
            {
                this.testFileWriter = new SingleDirTestWriter(new DirectoryInfo(config.outputdir), config.testPrefix);
            }
            else
            {
                DirectoryInfo outputDir = new DirectoryInfo(config.outputdir);

                this.testFileWriter = new ClassifyingTestFileWriter(outputDir, config.testPrefix);
            }


        }

        /// <summary>
        /// Adds (if not already present) p to either fplanDB or fexceptionThrowingPlanDB,
        /// by executing the plan to determine if it throws exceptions.
        /// </summary>
        /// <param name="v"></param>
        public void AddMaybeExecutingIfNeeded(Plan p, StatsManager stats)
        {

            //foreach (string s in p.Codestring)
            //    Console.WriteLine(s);


            if (builderPlans.Containsplan(p))
            {
                redundantAdds++;
                stats.CreatedNew(CreationResult.Redundant);
            }
            else if (exceptionPlans.Containsplan(p))
            {
                redundantAdds++;
                stats.CreatedNew(CreationResult.Redundant);
            }
            else
            {
                addCounter++;
                if (addCounter % 1000 == 0)
                {
                    Console.Write(".");
                    PrintPercentageExecuted();
                }

                stats.CreatedNew(CreationResult.New);
                ResultTuple execResult;
                TextWriter writer = new StringWriter();
                Exception exceptionThrown;
                bool contractViolated;

                this.testFileWriter.WriteTest(p);

                if (config.executionmode == ExecutionMode.DontExecute)
                {
                    builderPlans.AddPlan(p);
                    stats.ExecutionResult("normal");
                }
                else
                {
                    Util.Assert(config.executionmode == ExecutionMode.Reflection);

                    TextWriter executionLog = new StreamWriter(config.executionLog);
                    executionLog.WriteLine("LASTPLANID:" + p.uniqueId);

                    long startTime = 0;
                    Timer.QueryPerformanceCounter(ref startTime);

                    bool execSucceeded = p.Execute(out execResult,
                        executionLog, writer, out exceptionThrown,
                        out contractViolated, this.config.forbidnull,
                        config.monkey);

                    long endTime = 0;
                    Timer.QueryPerformanceCounter(ref endTime);
                    TimeTracking.timeSpentExecutingTestedCode += (endTime - startTime);

                    executionLog.Close();

                    /*
                     * New: Now the execution of plan might fail (recursively) if any of the output tuple
                     * objects violate the contract for ToString(), HashCode(), Equals(o)
                     */
                    if (!execSucceeded)
                    {
                        stats.ExecutionResult(exceptionThrown == null ? "other" : exceptionThrown.GetType().FullName);

                        // TODO This should alway be true...
                        if (exceptionThrown != null)
                        {
                            p.exceptionThrown = exceptionThrown;
                            this.testFileWriter.Move(p, exceptionThrown);

                            if (exceptionThrown is AccessViolationException)
                            {
                                Console.WriteLine("SECOND-CHANCE ACCESS VIOLATION EXCEPTION.");
                                System.Environment.Exit(1);
                            }
                        }


                        string exceptionMessage = writer.ToString();

                        Util.Assert(p.exceptionThrown != null || contractViolated);

                        if (config.monkey)
                        {
                            builderPlans.AddPlan(p);
                            //exceptionPlans.AddPlan(p); //new: also add it to exceptions for monkey
                        }
                        else if (exceptionThrown != null)
                        {
                            exceptionPlans.AddPlan(p);
                        }
                    }
                    else
                    {
                        stats.ExecutionResult("normal");

                        if (config.outputnormalinputs)
                        {
                            this.testFileWriter.MoveNormalTermination(p);
                        }
                        else
                        {
                            this.testFileWriter.Remove(p);
                        }

                        // If forbidNull, then make inactive any result tuple elements that are null.
                        if (this.config.forbidnull)
                        {
                            Util.Assert(p.NumTupleElements == execResult.tuple.Length);
                            for (int i = 0; i < p.NumTupleElements; i++)
                                if (execResult.tuple[i] == null)
                                    p.SetActiveTupleElement(i, false);
                            //Util.Assert(!allNull); What is the motivation behind this assertion?
                        }


                        //only allow the receivers to be arguments to future methods
                        if (config.forbidparamobj)
                        {
                            Util.Assert(p.NumTupleElements == execResult.tuple.Length);
                            for (int i = 1; i < p.NumTupleElements; i++)
                                p.SetActiveTupleElement(i, false);

                        }

                        builderPlans.AddPlan(p, execResult);
                    }
                }
            }

        }

        private void PrintPercentageExecuted()
        {
            long endTime = 0;
            Timer.QueryPerformanceCounter(ref endTime);

            long totalGenerationTime = endTime - TimeTracking.generationStartTime;

            double ratioExecToGen =
                (double)TimeTracking.timeSpentExecutingTestedCode / (double)totalGenerationTime;
        }
    }
}
