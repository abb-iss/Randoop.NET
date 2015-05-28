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
using System.IO;
using Common;
using System.Collections;
using System.Collections.ObjectModel;

namespace Randoop
{

    // SELECTED CREATED REDUNDANT BEHAVIOR


    public enum StatKind { Start, Selected, CreatedNewPlan }

    public enum CreationResult { Redundant, NoInputs, New }

    public class StatsManager
    {
        private StreamWriter writer;

        private StatKind state;

        public StatsManager(RandoopConfiguration config)
        {
            this.writer = new StreamWriter(config.statsFile.fileName);
            this.state = StatKind.Start;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Common.RandoopBareExceptions.InternalError.#ctor(System.String)")]
        public void Selected(string t)
        {
            //Console.WriteLine("@@@ selected " + t);

            if (t == null)
                throw new Common.RandoopBareExceptions.InternalError("Bug in Randoop.StatsManager");

            if (this.state != StatKind.Start)
                throw new Common.RandoopBareExceptions.InternalError("Bug in Randoop.StatsManager.");

            writer.Write(t);
            writer.Write("#");

            this.state = StatKind.Selected;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Common.RandoopBareExceptions.InternalError.#ctor(System.String)")]
        public void CreatedNew(CreationResult cr)
        {

            //Console.WriteLine("@@@ created new " + cr);

            if (this.state != StatKind.Selected)
                throw new Common.RandoopBareExceptions.InternalError("Bug in Randoop.StatsManager.");

            if (cr == CreationResult.NoInputs)
            {
                writer.WriteLine(cr.ToString());
                writer.Flush();

                // Go back to the initial state.
                state = StatKind.Start;
            }
            else if (cr == CreationResult.Redundant)
            {
                writer.WriteLine(cr.ToString());
                writer.Flush();

                // Go back to the initial state.
                state = StatKind.Start;
            }
            else
            {
                writer.Write(cr.ToString());
                writer.Write("#");
                writer.Flush();

                state = StatKind.CreatedNewPlan;
            }

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Common.RandoopBareExceptions.InternalError.#ctor(System.String)")]
        public void ExecutionResult(string result)
        {
            //Console.WriteLine("@@@ execution result " + result);

            if (result == null)
                throw new ArgumentNullException("result");

            if (this.state != StatKind.CreatedNewPlan)
                throw new Common.RandoopBareExceptions.InternalError("Bug in Randoop.StatsManager.");

            writer.Write(result);
            writer.WriteLine();

            state = StatKind.Start;
            writer.Flush();
        }

        /// A line in a stats file can be one of the following.
        ///
        /// "action#Redundant"
        /// 
        ///    means that action was selected but randoop was
        ///    not able to create a new, non-redundant plan for the action.
        /// 
        /// "action#NoInputs"
        /// 
        ///    means that action was selected but randoop was
        ///    not able to find input plans to create a new plan for the action.
        /// 
        /// "action#New#result"
        ///
        ///    means that action was selected, plan was successfully
        ///    created, and plan was executed. If result=="normal" execution
        ///    it means that plan executed normally. Otherwise, result
        ///    identifies the failing behavior (e.g. an exception name).
        ///    
        public static void ComputeStats(string resultsFileName,
            System.Collections.ObjectModel.Collection<FileInfo> statsFiles)
        {
            Dictionary<string, OneActionStats> stats = new Dictionary<string, OneActionStats>();

            foreach (FileInfo fi in statsFiles)
            {
                ReadStats(fi, stats);
            }

            StreamWriter writer = new StreamWriter(resultsFileName);
            foreach (OneActionStats s in SortByTimesActionsSelected(stats))
            {
                writer.WriteLine(s.ToString());
            }
            writer.Close();
            Console.WriteLine("Wrote results to file {0}.", resultsFileName);

        }


        /// <summary>
        /// Reverse sort.
        /// </summary>
        private class OneActionStatsComparer : IComparer<OneActionStats>
        {
            public int Compare(OneActionStats x, OneActionStats y)
            {
                if (x == null) throw new ArgumentNullException("x");
                if (y == null) throw new ArgumentNullException("y");
                if (x.timesSelected > y.timesSelected) return -1;
                if (x.timesSelected < y.timesSelected) return 1;
                return 0;
            }
        }

        private static Collection<OneActionStats> SortByTimesActionsSelected(Dictionary<string, OneActionStats> stats)
        {
            List<OneActionStats> statsList = new List<OneActionStats>();
            foreach (OneActionStats s in stats.Values)
                statsList.Add(s);
            statsList.Sort(new OneActionStatsComparer());
            return new Collection<OneActionStats>(statsList);
        }

        private static void ReadStats(FileInfo fi, Dictionary<string, OneActionStats> stats)
        {
            Console.WriteLine("Parsing file " + fi.FullName);

            StreamReader reader = new StreamReader(fi.FullName);

            try
            {
                reader = new StreamReader(fi.FullName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Warning: unable to open a StreamReader for file "
                    + fi.FullName
                    + ". (Will continue.) Exception message: "
                    + e.Message);
            }

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ParseAndAdd(line, stats);
            }

            reader.Close();
        }

        private static void ParseAndAdd(string line, Dictionary<string, OneActionStats> stats)
        {
            string[] elements = line.Trim().Split('#');
            if (elements.Length < 2)
            {
                Console.WriteLine("Warning: line not a valid stats line: {0}. (Will continue.)", line);
                return;
            }
            string action = elements[0].Trim();
            OneActionStats statsForAction;
            if (!stats.TryGetValue(action, out statsForAction))
            {
                statsForAction = new OneActionStats(action);
                stats[action] = statsForAction;
            }
            string creationResultString = elements[1];

            statsForAction.timesSelected++;



            if (creationResultString.Equals(CreationResult.NoInputs.ToString()))
            {
                statsForAction.timesNoInputs++;

                // We're done with this line.
                return;
            }
            if (creationResultString.Equals(CreationResult.Redundant.ToString()))
            {
                statsForAction.timesRedundant++;

                // We're done with this line.
                return;
            }

            if (!creationResultString.Equals(CreationResult.New.ToString()))
                throw new Common.RandoopBareExceptions.InternalError("Bug in Randoop.StatsManager.");

            statsForAction.timesNew++;

            if (elements.Length < 3)
            {
                Console.WriteLine("Warning: line not a valid stats line: {0}. (Will continue.)", line);
                return;
            }

            string result = elements[2];

            int currentCountForResult;
            if (!statsForAction.results.TryGetValue(result, out currentCountForResult))
            {
                currentCountForResult = 0;
            }
            statsForAction.results[result] = currentCountForResult + 1;
        }


        private class OneActionStats
        {
            public readonly string action;

            public int timesSelected = 0;

            public int timesRedundant = 0;

            public int timesNoInputs = 0;

            public int timesNew = 0;

            public readonly Dictionary<string, int> results = new Dictionary<string, int>();

            public OneActionStats(string action)
            {
                if (action == null)
                    throw new ArgumentNullException("Internal Randoop error: action should not be null.");
                this.action = action;
            }

            public override string ToString()
            {
                StringBuilder b = new StringBuilder();
                b.AppendLine("========== " + action);
                b.AppendLine();
                b.AppendLine("Test generation statistics.");
                b.AppendLine(timesSelected.ToString().PadRight(7) + ": TRIED to create test ending with this member");
                b.AppendLine(timesNoInputs.ToString().PadRight(7) + ": FAILED TO CREATE: found no input arguments");
                b.AppendLine(timesRedundant.ToString().PadRight(7) + ": FAILED TO CREATE: found inputs, but new test syntactially the same as an old test");
                b.AppendLine(timesNew.ToString().PadRight(7) + ": SUCCEEDED in creating a new test");
                b.AppendLine();
                b.AppendLine("Test execution statistics.");

                if (results.Keys.Count == 0)
                {
                    b.AppendLine("No tests were executed.");
                }
                else
                {
                    foreach (string key in results.Keys)
                    {
                        b.AppendLine(results[key].ToString().PadRight(7) + ": " + key);
                    }
                }
                return b.ToString();
            }
        }
    }


}
