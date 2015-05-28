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
using System.Security;
using Common;
using System.Collections.ObjectModel;

namespace Randoop
{

    public class ConstructorCallTransformer : Transformer
    {
        public readonly ConstructorInfo fconstructor;
        public readonly ConstructorInfo coverageInfo;

        public int timesExecuted = 0;
        public double executionTimeAccum = 0;

        private static Dictionary<ConstructorInfo, ConstructorCallTransformer> cachedTransformers =
            new Dictionary<ConstructorInfo, ConstructorCallTransformer>();


        public static ConstructorCallTransformer Get(ConstructorInfo field)
        {
            ConstructorCallTransformer t;
            cachedTransformers.TryGetValue(field, out t);
            if (t == null)
                cachedTransformers[field] = t = new ConstructorCallTransformer(field);
            return t;
        }

        public override string MemberName
        {
            get
            {
                return fconstructor.DeclaringType.FullName;
            }
        }

        public override int TupleIndexOfIthInputParam(int i)
        {
            if (i < 0 && i > resultTypes.Length - 1) throw new ArgumentException("index out of range.");
            return (i + 1); // skip new object.
        }

        private readonly Type[] resultTypes;

        private bool[] defaultActiveResultTypes;

        public override Type[] TupleTypes
        {
            get { return resultTypes; }
        }

        public override bool[] DefaultActiveTupleTypes
        {
            get { return defaultActiveResultTypes; }
        }

        public override Type[] ParameterTypes
        {
            get
            {
                ParameterInfo[] parameters = fconstructor.GetParameters();
                Type[] retval = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    retval[i] = parameters[i].ParameterType;
                return retval;
            }
        }

        public override string ToString()
        {
            return fconstructor.DeclaringType.FullName + " constructor " + fconstructor.ToString();
        }

        public override bool Equals(object obj)
        {
            ConstructorCallTransformer c = obj as ConstructorCallTransformer;
            if (c == null)
                return false;
            return (this.fconstructor.Equals(c.fconstructor));
        }

        public override int GetHashCode()
        {
            return fconstructor.GetHashCode();
        }



        private ConstructorCallTransformer(ConstructorInfo constructor)
        {
            Util.Assert(constructor is ConstructorInfo);
            this.fconstructor = constructor as ConstructorInfo;
            this.coverageInfo = constructor;

            ParameterInfo[] pis = constructor.GetParameters();

            resultTypes = new Type[pis.Length + 1];
            defaultActiveResultTypes = new bool[pis.Length + 1];

            resultTypes[0] = constructor.DeclaringType;
            defaultActiveResultTypes[0] = true;

            for (int i = 0; i < pis.Length; i++)
            {
                resultTypes[i + 1] = pis[i].ParameterType;
                //if (pis[i].IsOut)
                //    defaultActiveResultTypes[i + 1] = true;
                //else
                defaultActiveResultTypes[i + 1] = false;
            }
        }

        public override string ToCSharpCode(ReadOnlyCollection<string> arguments, String newValueName)
        {
            // TODO assert that arguments.Count is correct.
            StringBuilder b = new StringBuilder();
            string retType =
                SourceCodePrinting.ToCodeString(fconstructor.DeclaringType);
            b.Append(retType + " " + newValueName + " = ");
            b.Append(" new " + SourceCodePrinting.ToCodeString(fconstructor.DeclaringType));
            b.Append("(");
            for (int i = 0; i < arguments.Count; i++)
            {
                if (i > 0)
                    b.Append(" , ");

                // Cast.
                b.Append("(");
                b.Append(SourceCodePrinting.ToCodeString(ParameterTypes[i]));
                b.Append(")");
                b.Append(arguments[i]);
            }
            b.Append(");");
            return b.ToString();
        }

        public override bool Execute(out ResultTuple ret, ResultTuple[] results,
            Plan.ParameterChooser[] parameterMap, TextWriter executionLog, TextWriter debugLog, out Exception exceptionThrown, out bool contractViolated, bool forbidNull)
        {
            contractViolated = false;

            this.timesExecuted++;
            long startTime = 0;
            Timer.QueryPerformanceCounter(ref startTime);

            object[] objects = new object[fconstructor.GetParameters().Length];
            // Get the actual objects from the results using parameterIndices;
            for (int i = 0; i < fconstructor.GetParameters().Length; i++)
            {
                Plan.ParameterChooser pair = parameterMap[i];
                objects[i] = results[pair.planIndex].tuple[pair.resultIndex];
            }

            if (forbidNull)
                foreach (object o in objects)
                    Util.Assert(o != null);

            object newObject = null;

            CodeExecutor.CodeToExecute call =
                delegate() { newObject = fconstructor.Invoke(objects); };

            executionLog.WriteLine("execute constructor " + this.fconstructor.DeclaringType);
            executionLog.Flush();

            bool retval = true;
            if (!CodeExecutor.ExecuteReflectionCall(call, debugLog, out exceptionThrown))
            {
                ret = null;

                if (exceptionThrown is AccessViolationException)
                {
                    //Console.WriteLine("SECOND CHANCE AV!" + this.ToString());
                    //Logging.LogLine(Logging.GENERAL, "SECOND CHANCE AV!" + this.ToString());
                }

                //for exns we can ony add the class to faulty classes when its a guideline violation
                if (Util.GuidelineViolation(exceptionThrown.GetType()))
                {
                    PlanManager.numDistinctContractViolPlans++;

                    KeyValuePair<MethodBase, Type> k = new KeyValuePair<MethodBase, Type>(this.fconstructor, exceptionThrown.GetType());
                    if (!exnViolatingMethods.ContainsKey(k))
                    {
                        PlanManager.numContractViolatingPlans++;
                        exnViolatingMethods[k] = true;
                    }

                    //add this class to the faulty classes
                    contractExnViolatingClasses[fconstructor.GetType()] = true;
                    contractExnViolatingMethods[fconstructor] = true;
                }


                return false;
            }
            else
                ret = new ResultTuple(fconstructor, newObject, objects);


            //check if the objects in the output tuple violated basic contracts
            if (ret != null)
            {
                foreach (object o in ret.tuple)
                {
                    if (o == null) continue;

                    bool toStrViol, hashCodeViol, equalsViol;
                    int count;
                    if (Util.ViolatesContracts(o, out count, out toStrViol, out hashCodeViol, out equalsViol))
                    {
                        contractViolated = true;
                        contractExnViolatingMethods[fconstructor] = true;

                        bool newcontractViolation = false;
                        PlanManager.numDistinctContractViolPlans++;

                        if (toStrViol)
                        {

                            if (!toStrViolatingMethods.ContainsKey(fconstructor))
                                newcontractViolation = true;
                            toStrViolatingMethods[fconstructor] = true;
                        }
                        if (hashCodeViol)
                        {
                            if (!hashCodeViolatingMethods.ContainsKey(fconstructor))
                                newcontractViolation = true;

                            hashCodeViolatingMethods[fconstructor] = true;
                        }
                        if (equalsViol)
                        {
                            if (!equalsViolatingMethods.ContainsKey(fconstructor))
                                newcontractViolation = true;

                            equalsViolatingMethods[fconstructor] = true;
                        }

                        if (newcontractViolation)
                            PlanManager.numContractViolatingPlans++;

                        //add this class to the faulty classes
                        contractExnViolatingClasses[fconstructor.DeclaringType] = true;

                        retval = false;
                    }

                }
            }

            long endTime = 0;
            Timer.QueryPerformanceCounter(ref endTime);
            executionTimeAccum += ((double)(endTime - startTime)) / ((double)(Timer.PerfTimerFrequency));

            return retval;
        }

        public override string Namespace
        {
            get
            {
                return this.fconstructor.DeclaringType.Namespace;
            }
        }

        public override ReadOnlyCollection<Assembly> Assemblies
        {
            get
            {
                return ReflectionUtils.GetRelatedAssemblies(this.fconstructor);
            }
        }

    }

}
