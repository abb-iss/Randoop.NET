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
using System.IO;
using System.Collections.ObjectModel;
using Common;
using System.Diagnostics;

namespace Randoop
{
    /// <summary>
    /// This class is actually a wrapper around the actual input generator
    /// (whose entrypoint is RandoopBare.MainMethod).
    /// 
    /// The wrapper performs the following steps.
    /// 
    /// 1. Parses command-line arguments.
    /// 
    /// 2. Invokes RandoopBare, possibly multiple times.
    /// If RandoopBare fails before the user-specified time limit expires,
    /// the wrapper calls the generator again. The reason for doing this is
    /// that the generator may fail due to an error in the code under test 
    /// (e.g. an access violation). In this situation, the best course of
    /// action is to continue generating test inputs until the time limit
    /// expires.
    /// 
    /// The end result is a set of test cases (C# source files) and
    /// an index.html file summarizing the results of the generation.
    /// </summary>
    public class Randoop
    {
        /// DEFAULTS.
        public static readonly int defaultTotalTimeLimit = 100;
        public static readonly int defaultStartTimeSeconds = 100;

        /// <summary>
        /// Main randoop entrypoint.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Randoop.NET: an API fuzzer for .Net. Version {0} (compiled {1}).",
                Common.Enviroment.RandoopVersion, Common.Enviroment.RandoopCompileDate);

            if (args.Length == 0 || IsHelpCommand(args[0]))
            {
                Console.Error.WriteLine(HelpScreen.Usagestring());
                System.Environment.Exit(1);
            }

            if (ContainsHelp(args))
            {
                Console.WriteLine(HelpScreen.Usagestring());
                System.Environment.Exit(0);
            }

            if (args[0].Equals("/about"))
            {
                WriteAboutMessageToConsole();
                System.Environment.Exit(0);
            }

            if (args[0].Equals("minimize"))
            {
                string[] args2 = new string[args.Length - 1];
                Array.Copy(args, 1, args2, 0, args.Length - 1);
                TestCaseUtils.Minimize(TestCaseUtils.CollectFilesEndingWith(".cs", args2));
                System.Environment.Exit(0);
            }

            if (args[0].Equals("reduce"))
            {
                string[] args2 = new string[args.Length - 1];
                Array.Copy(args, 1, args2, 0, args.Length - 1);
                Collection<FileInfo> oldTests = TestCaseUtils.CollectFilesEndingWith(".cs", args2);
                Console.WriteLine("Number of original tests: " + oldTests.Count);
                Collection<FileInfo> newTests = TestCaseUtils.Reduce(oldTests);
                Console.WriteLine("Number of reduced tests: " + newTests.Count);
                Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>> classifiedTests =
                    TestCaseUtils.ClassifyTestsByMessage(newTests);
                PrintStats(classifiedTests);
                StringBuilder b = new StringBuilder();
                foreach (string s in args) b.Append(" " + s);
                string htmlFileName = "reduced" + Path.GetRandomFileName() + ".html";
                HtmlSummary.CreateIndexHtml(classifiedTests, htmlFileName, b.ToString());
                OpenExplorer(htmlFileName);
                System.Environment.Exit(0);
            }

            if (args[0].Equals("reproduce"))
            {
                string[] args2 = new string[args.Length - 1];
                Array.Copy(args, 1, args2, 0, args.Length - 1);
                TestCaseUtils.ReproduceBehavior(TestCaseUtils.CollectFilesEndingWith(".cs", args2));
                System.Environment.Exit(0);
            }

            if (args[0].StartsWith("stats:"))
            {
                string statsResultsFileName = args[0].Substring("stats:".Length);

                string[] args2 = new string[args.Length - 1];
                Array.Copy(args, 1, args2, 0, args.Length - 1);

                StatsManager.ComputeStats(statsResultsFileName,
                    TestCaseUtils.CollectFilesEndingWith(".stats.txt", args2));
                System.Environment.Exit(0);
            }

            GenerateTests(args);
        }

        private static bool ContainsHelp(string[] args)
        {
            foreach (string s in args)
                if (s.ToLower().Equals("/?"))
                    return true;
            return false;
        }

        /// <summary>
        /// The "real" main entrypoint into test generation.
        /// Generates tests by invoking the test generator, potentially multiple times.
        /// </summary>
        private static void GenerateTests(string[] argArray)
        {
            // Parse command line arguments.
            string errorMessage;
            CommandLineArguments args = new CommandLineArguments(
                argArray,
                defaultTotalTimeLimit,
                defaultStartTimeSeconds,
                out errorMessage);
            if (errorMessage != null)
            {
                Console.WriteLine("Error in command-line arguments: " + errorMessage);
                System.Environment.Exit(1);
            }

            //CheckAssembliesExist(args);

            int randomseed = 0;
            if (args.RandomSeed != null)
            {
                int.TryParse(args.RandomSeed, out randomseed);
            }

            // Invoke the generator, possibly multiple times.
            // If timeLimit > Randoop.TIME_PER_INVOCATION, then the generator
            // is invoked multiple times each time with time limit <= TIME_PER_INVOCATION.
            string tempDir = null;
            try
            {
                // Create temporary directory.
                tempDir = Common.TempDir.CreateTempDir();

                // Determine output directory.
                string outputDir;
                if (args.OutputDir == null)
                {
                    // The output directory's name is calculated based on
                    // the day and time that this invocation of Randoop occurred.
                    outputDir =
                        System.Environment.CurrentDirectory
                        + "\\randoop_output\\"
                        + CalculateOutputDirBase();
                }
                else
                {
                    outputDir = args.OutputDir; // TODO check legal file name.
                }

                // Create output directory, if it doesn't exist.
                if (!Directory.Exists(outputDir))
                    new DirectoryInfo(outputDir).Create(); // TODO resource error checking.

                // Determine execution log file name.
                string executionLog = tempDir + "\\" + "execution.log";

                if (args.PageHeap) PageHeaEnableRandoop();
                if (args.UseDHandler) DHandlerEnable();

                TimeSpan totalTime = new TimeSpan(0);
                int round = 0;

                Collection<string> methodsToOmit = new Collection<string>();
                Collection<string> constructorsToOmit = new Collection<string>();

                while (Convert.ToInt32(totalTime.TotalSeconds) < Convert.ToInt32(args.TimeLimit.TotalSeconds))
                {
                    round++;

                    int exitCode = DoInputGenerationRound(args, outputDir, round,
                        ref totalTime, tempDir, executionLog, randomseed,
                        methodsToOmit, constructorsToOmit);

                    if (exitCode != 0)
                        break;
                }

                if (args.PageHeap) PageHeapDisableRandoop();
                if (args.UseDHandler) DHandlerDisable();

                
                
                // Create allstats.txt and index.html.
                bool testsCreated;
                if (!PostProcessTests(args, outputDir))
                {
                    testsCreated = false;
                    Console.WriteLine("No tests created.");
                }
                else
                {
                    testsCreated = true;
                }

                if (!args.KeepStatLogs)
                    DeleteStatLogs(outputDir);


                // The final step invokes IE and displays a summary (file index.html).
                if (!args.NoExplorer && testsCreated)
                    OpenExplorer(outputDir + "\\index.html");
            }
            catch (Exception e)
            {
                Console.WriteLine("*** Randoop error. "
                    + "Please report this error to Randoop's developers.");
                Console.WriteLine(e);
                System.Environment.Exit(1);
            }
            finally
            {
                // Delete temp dir.
                if (tempDir != null)
                {
                    Console.WriteLine("Removing temporary directory: " + tempDir);
                    try
                    {
                        new DirectoryInfo(tempDir).Delete(true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception raised while deleting temporary directory: " + e.Message);
                    }
                }
            }
        }

        private static void DeleteStatLogs(string outputDir)
        {
            foreach (FileInfo fi in TestCaseUtils.CollectFilesEndingWith(".stats.txt", outputDir))
            {
                fi.Delete();
            }
        }

        private static void CheckAssembliesExist(CommandLineArguments args)
        {
            foreach (string s in args.AssemblyNames)
            {
                if (!File.Exists(s))
                {
                    Console.WriteLine("Assembly file does not exist: " + s);
                    System.Environment.Exit(1);
                }
            }
        }

        /// THE RETURN VALUE MEANS "PARAMETERS WRE OK"!!!!! BAD!!
        private static int DoInputGenerationRound(
            CommandLineArguments args,
            string outputDir,
            int round,
            ref TimeSpan totalTime,
            string tempDir,
            string executionLogFileName,
            int randomseed,
            Collection<string> methodsToOmit,
            Collection<string> constructorsToOmit)
        {
            // Calculate time for next invocation of Randoop.
            TimeSpan timeForNextInvocation = CalculateNextTimeLimit(args.TimeLimit, totalTime,
                new TimeSpan(0, 0, args.restartTimeSeconds));

            // Analyze last execution.
            int lastPlanId;
            
            AnalyzeLastExecution(executionLogFileName, out lastPlanId, outputDir);

            // Create a new randoop configuration.
            RandoopConfiguration config = ConfigFileCreator.CreateConfigFile(args, outputDir, lastPlanId,
                timeForNextInvocation, round, executionLogFileName, randomseed);

            string configFileName = tempDir + "\\config" + round + ".xml";
            config.Save(configFileName);
            FileInfo configFileInfo = new FileInfo(configFileName);

            // Launch new instance of Randoop.
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Spawning new input generator process (round " + round + ")");
            
            Process p = new Process();
            
            p.StartInfo.FileName = Common.Enviroment.RandoopBareExe;
            p.StartInfo.Arguments = ConfigFileName.ToString(configFileInfo);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.ErrorDialog = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.WorkingDirectory = tempDir;

            
            Console.WriteLine("Spawning process: ");
            Console.WriteLine(Util.PrintProcess(p));

            // Give it 30 seconds to load reflection info, etc.
            int waitTime = (Convert.ToInt32(timeForNextInvocation.TotalSeconds) + 30)*1000;

            Console.WriteLine("Will wait " + waitTime + " milliseconds for process to terminate.");

            p.Start();
            
            if (!p.WaitForExit(waitTime))
            {   
                // Process didn't terminate. Kill it forcefully.
                Console.WriteLine("Killing process " + p.Id);
                try
                {
                    p.Kill();
                }
                catch (InvalidOperationException)
                {
                    // p has already terminated. No problem.
                }
                p.WaitForExit();
            }

            // Update total time.
            totalTime += p.ExitTime - p.StartTime;

            string err = p.StandardError.ReadToEnd();

            // Randoop terminated with error.
            if (p.ExitCode != 0)
            {
                if (err.Contains(Common.Enviroment.RandoopBareInternalErrorMessage))
                {
                    Console.WriteLine(err);
                    Console.WriteLine("*** RandoopBare had an internal error. Exiting.");
                    return p.ExitCode;
                }

                if (err.Contains(Common.Enviroment.RandoopBareInvalidUserParametersErrorMessage))
                {
                    Console.WriteLine(err);
                    Console.WriteLine("*** RandoopBare terminated normally. Exiting.");
                    return p.ExitCode;
                }

            }

            return 0;
        }

        private static void OpenExplorer(string file)
        {
            Process explorerProcess = new Process();
            explorerProcess.StartInfo.FileName = "explorer";
            explorerProcess.StartInfo.Arguments = file;
            explorerProcess.Start();
        }

        // false if no tests found.
        private static bool PostProcessTests(CommandLineArguments args, string outputDir)
        {

            //////////////////////////////////////////////////////////////////////
            // Determine directory where generated tests were written.

            DirectoryInfo resultsDir = new DirectoryInfo(outputDir);

            //////////////////////////////////////////////////////////////////////
            // Collect all tests generated.

            Collection<FileInfo> tests = TestCaseUtils.CollectFilesEndingWith(".cs", resultsDir);

            if (tests.Count == 0)
            {
                
                return false;
            }

            Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>> classifiedTests =
                TestCaseUtils.ClassifyTestsByMessage(tests);

            PrintStats(classifiedTests);

            //////////////////////////////////////////////////////////////////////
            // Create stats file.

             StatsManager.ComputeStats(resultsDir + "\\allstats.txt",
                 TestCaseUtils.CollectFilesEndingWith(".stats.txt", resultsDir));

            //////////////////////////////////////////////////////////////////////
            // Create HTML index.

            HtmlSummary.CreateIndexHtml(classifiedTests, resultsDir + "\\index.html", args.ToString());

            Console.WriteLine();
            Console.WriteLine("Results written to " + resultsDir + ".");
            Console.WriteLine("You can browse the results by opening the file "
                + System.Environment.NewLine
                + "   " + resultsDir + "\\index.html");

            return true;
        }

        private static string CalculateOutputDirBase()
        {
            System.DateTime now = System.DateTime.Now;

            return
            "run_"
            + Common.Misc.Monthstring(now.Month)
            + "_"
            + now.Day
            + "_"
            + now.Year
            + "_"
            + now.Hour
            + "h_"
            + now.Minute
            + "m_"
            + now.Second
            + "s";
        }

        private static TimeSpan CalculateNextTimeLimit(TimeSpan timeLimit, TimeSpan totalTime, TimeSpan timePerInvocation)
        {
            if (timeLimit - totalTime < timePerInvocation)
            {
                return timeLimit - totalTime;
            }
            else
            {
                return timePerInvocation;
            }
        }

        private static void AnalyzeLastExecution(
            string executionLogFileName,
            out int lastPlanId,
            string outputDir)
        {
            lastPlanId = 0;
            if (new FileInfo(executionLogFileName).Exists)
            {
                StreamReader r = new StreamReader(executionLogFileName);
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.StartsWith("LASTPLANID:"))
                    {
                        lastPlanId = int.Parse(line.Substring("LASTPLANID:".Length).Trim());
                        continue;
                    }
                    if (r.EndOfStream)
                    {
                        AddMethodToOmit(line, outputDir);
                    }
                }
                r.Close();
            }

        }

        private static string WrapInDHandler(string randoopCommand)
        {
            StringBuilder b = new StringBuilder();
            b.Append("/O:\" "
                + Common.Enviroment.RandoopHome
                + "\\randoopruntime\\pagehandleroutput.txt"
                + "\"");
            b.Append(" /I:"
                + "\""
                + Common.Enviroment.DefaultDhi
                + "\"");
            b.Append(" /App:\"" + randoopCommand + "\"");
            return b.ToString();
        }


       

        private static void WriteAboutMessageToConsole()
        {
            Console.WriteLine("Version "
            + Common.Enviroment.RandoopVersion
            + "."
            + System.Environment.NewLine
            + "Compiled on "
            + Common.Enviroment.RandoopCompileDate
            + "."
            + System.Environment.NewLine
            + System.Environment.NewLine
            + "Credits"
            + System.Environment.NewLine
            + "  Authors: Carlos Pacheco (t-carpac)"
            + System.Environment.NewLine
            + "           Shuvendu Lahiri (shuvendu)"
            + System.Environment.NewLine
            + "           Tom Ball (tball)"
            //+ System.Environment.NewLine
            //+ "  DHandler pop-up blocker by Jeff Schwartz (jeffschw)"
            + System.Environment.NewLine
            + "  Help and guidance from Scott Wadsworth and Eugene Bobukh."
            + System.Environment.NewLine
            );
        }

        private static void PrintStats(Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>> testsByMessage)
        {
            Console.WriteLine("Results statistics:");
            foreach (TestCase.ExceptionDescription message in testsByMessage.Keys)
            {
                String testCount =
                    testsByMessage[message].Count
                    + " test"
                    + (testsByMessage[message].Count > 1 ? "s" : "")
                    + ":";
                testCount = testCount.PadRight(15, ' ');
                Console.WriteLine(testCount + message.ExceptionDescriptionString);
            }
        }

        private static void PageHeapDisableRandoop()
        {
            Process p = new Process();
            p.StartInfo.FileName = Common.Enviroment.PageHeap;
            p.StartInfo.Arguments = "/disable randoopbare.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.ErrorDialog = true;

            Console.WriteLine("Starting process:");
            Console.WriteLine(Util.PrintProcess(p));

            p.Start();
            p.WaitForExit();
        }

        private static void PageHeaEnableRandoop()
        {
            Process p = new Process();
            p.StartInfo.FileName = Common.Enviroment.PageHeap;
            p.StartInfo.Arguments = "/enable randoopbare.exe /full";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.ErrorDialog = true;

            Console.WriteLine("Starting process:");
            Console.WriteLine(Util.PrintProcess(p));

            p.Start();
            p.WaitForExit();
        }


        private static Process dHandlerProcess;

        private static void DHandlerDisable()
        {
            if (dHandlerProcess != null)
            {
                Console.WriteLine("Killing DHandler process.");
                dHandlerProcess.Kill();
            }
        }

        private static void DHandlerEnable()
        {
            dHandlerProcess = new Process();
            dHandlerProcess.StartInfo.FileName = Common.Enviroment.DHandler;
            dHandlerProcess.StartInfo.Arguments = "/I:\"" + Common.Enviroment.DefaultDhi + "\"";
            dHandlerProcess.StartInfo.UseShellExecute = false;
            dHandlerProcess.StartInfo.ErrorDialog = true;
            Console.WriteLine("Starting process:");
            Console.WriteLine(Util.PrintProcess(dHandlerProcess));
            dHandlerProcess.Start();
        }

            private static bool IsHelpCommand(string p)
            {
                return p.ToLower().Equals("/?")
                   ||
                   p.ToLower().Equals("-?")
                   ||
                   p.ToLower().Equals("--?")
                   ||
                   p.ToLower().Equals("/help")
                   ||
                   p.ToLower().Equals("-help")
                   ||
                   p.ToLower().Equals("--help");
            }

        private static void AddMethodToOmit(string line, string outputDir)
        {
            if (line.StartsWith("execute method ") && !line.EndsWith("end execute method"))
            {
                string methodName = line.Substring("execute method ".Length).Trim();
                Createforbid_memberFile(methodName, outputDir);
            }
            else if (line.StartsWith("execute constructor ") && !line.EndsWith("end execute constructor"))
            {
                string constructorName = line.Substring("execute constructor ".Length).Trim();
                Createforbid_memberFile(constructorName, outputDir);
            }

            // TODO: 
            // If execution terminated abruptly, find last test input that led to the termination
            // (will be under "temp" Directory) and add it to an "abrupt-termination" Directory.

        }

        private static void Createforbid_memberFile(string methodName, string outputDir)
        {
            string newConfigFileName = outputDir + "\\" + Path.GetRandomFileName();
            Console.WriteLine("Creating config file {0}.", newConfigFileName);
            StreamWriter w = new StreamWriter(newConfigFileName);
            w.WriteLine(methodName);
            w.Close();

        }

    }


}
