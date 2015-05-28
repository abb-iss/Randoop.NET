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
using System.Collections.ObjectModel;

namespace Randoop
{

    public class HelpScreen
    {
        public static string Usagestring()
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine();
            b.AppendLine("Usage: randoop [option]* [assembly]+");
            b.AppendLine();
            b.AppendLine("By default, Randoop explores the public members declared in the given");
            b.AppendLine("assemblies by creating sequences of API calls and executing them.");
            b.AppendLine("Its output is a set of tests, written as C# source files");
            b.AppendLine("to folder randoop_output in the current directory, by default.");
            b.AppendLine();
            b.AppendLine("Options:");
            b.AppendLine();
            b.AppendLine("   /about           Print Randoop release information and exit.");
            b.AppendLine();
            b.AppendLine("   Controlling members that are used/avoided.");
            b.AppendLine("   -----------------------------------------");
            b.AppendLine();
            b.AppendLine("   The following 8 options control the API entities (types,");
            b.AppendLine("   constructors, methods and fields) that randoop uses to create tests.");
            b.AppendLine();
            b.AppendLine("   /nostatic        Don't explore static members.");
            b.AppendLine("                    Default is to explore both static and instance members. ");
            b.AppendLine();
            b.AppendLine("   /internal        Explore internal members in addition to public members.");
            b.AppendLine("                    Default is to explore only public members.");
            b.AppendLine("                    Note that if this option is given, the generated tests may");
            b.AppendLine("                    not compile.");
            b.AppendLine();
            b.AppendLine("   The resulting set MS of methods, constructors and fields is the set");
            b.AppendLine("   Randoop uses to create tests.");
            b.AppendLine();
            b.AppendLine("   Configuration files.");
            b.AppendLine("   --------------------");
            b.AppendLine();
            b.AppendLine("   /configfiles:dir");
            b.AppendLine("                     Set the configuration-files directory to dir.");
            b.AppendLine("                     Randoop looks in dir for configuration files. If");
            b.AppendLine("                     a file is not found in dir, Randoop uses the default");
            b.AppendLine("                     one located in <randoop>\\default_config_files.");
            b.AppendLine();
            b.AppendLine("   CONFIGURATION FILES. There are 20 possible configuration files.");
            b.AppendLine();
            b.AppendLine("   Lines starting with \"#\" are ignored in configuration files.");
            b.AppendLine();
            b.AppendLine("   For the next 6 files, each line in");
            b.AppendLine("   the file is interpreted as a wildcard pattern. The pattern should not contain");
            b.AppendLine("   whitespace (if it does, it will be removed before applying the pattern). A");
            b.AppendLine("   wildcard pattern can contain '*' characters. A '*' matches any sequece");
            b.AppendLine("   of strings, including the empty string.");
            b.AppendLine();
            b.AppendLine("   Examples of wildcard patterns:");
            b.AppendLine();
            b.AppendLine("   System.Xml*         matches any string that starts with \"System.Xml\"");
            b.AppendLine("   *Reflection*        matches any string that contains \"Reflection\"");
            b.AppendLine();
            b.AppendLine("   For more examples see <randoop>\\default_config_files.");
            b.AppendLine("   For more information on how the patterns are used, see the randoop manual.");
            b.AppendLine();
            b.AppendLine("   require_types.txt");
            b.AppendLine("                    Use only types that match at least one wildcard pattern");
            b.AppendLine("                    in the given file. The pattern is matched against the result");
            b.AppendLine("                    of calling T.ToString() on each type.");
            b.AppendLine();
            b.AppendLine("   require_members.txt");
            b.AppendLine("                    Use only methods and constructors that match at least");
            b.AppendLine("                    one wildcard pattern in the given file. The pattern is");
            b.AppendLine("                    matched against the result of calling M.ToString()/returntype");
            b.AppendLine("                    on each member.");
            b.AppendLine("   require_fields.txt");
            b.AppendLine("                    Use only fields that match at least one wildcard pattern");
            b.AppendLine("                    in the given file. The pattern is matched against the result");
            b.AppendLine("                    of calling F.ToString() on each field.");
            b.AppendLine();
            b.AppendLine("   forbid_types.txt");
            b.AppendLine("                    Avoid types that match at least one wildcard pattern");
            b.AppendLine("                    in the given file. The pattern is matched against the result");
            b.AppendLine("                    of calling T.ToString() on each type.");
            b.AppendLine("   forbid_members.txt");
            b.AppendLine("                    Avoid methods and constructors that match at least one wildcard");
            b.AppendLine("                    pattern in the given file. The pattern is matched against the ");
            b.AppendLine("                    result of calling M.ToString() on each member.");
            b.AppendLine("   forbid_fields.txt");
            b.AppendLine("                    Avoid fields that match at least one wildcard pattern");
            b.AppendLine("                    in the given file. The pattern is matched against the result");
            b.AppendLine("                    of calling F.ToString() on each field.");
            b.AppendLine();
            b.AppendLine("   The remaining 14 files specify simple type values (or strings) to");
            b.AppendLine("   use when calling an API that requires an simple type as input.");
            b.AppendLine("   The values for a given type are specified in a file, and");
            b.AppendLine("   each line in the file should contain a string representation");
            b.AppendLine("   of the value that can be parsed into the given type. For example,");
            b.AppendLine("   a file passed using /ints:file must contain one int per line, such");
            b.AppendLine("   as \"3\" or \"-1\". More specifically, for simple type T, each");
            b.AppendLine("   line should contain a string that can be passed to T.Parse(string).");
            b.AppendLine("   The exception is the option /strings:file, where each line can");
            b.AppendLine("   contain any string.");
            b.AppendLine();
            b.AppendLine("   seed_sbyte.txt.");
            b.AppendLine("   seed_byte.txt.");
            b.AppendLine("   seed_short.txt.");
            b.AppendLine("   seed_ushort.txt.");
            b.AppendLine("   seed_int.txt.");
            b.AppendLine("   seed_uint.txt.");
            b.AppendLine("   seed_long.txt.");
            b.AppendLine("   seed_ulong.txt.");
            b.AppendLine("   seed_char.txt.");
            b.AppendLine("   seed_float.txt.");
            b.AppendLine("   seed_double.txt.");
            b.AppendLine("   seed_bool.txt.");
            b.AppendLine("   seed_decimal.txt.");
            b.AppendLine("   seed_string.txt.");
            b.AppendLine();
            b.AppendLine("   Controlling test generation.");
            b.AppendLine("   ----------------------------");
            b.AppendLine();
            b.AppendLine("   /timelimit:N     Stop after exploring for N seconds.");
            b.AppendLine("                    Default: 100 seconds.");
            b.AppendLine();
            b.AppendLine("   /allownull       Allow the use of null as a parameter.");
            b.AppendLine("                    Default behavior never uses null as a parameter.");
            b.AppendLine();
            b.AppendLine();
            b.AppendLine("   /randomseed:N    Use N as the initial random seed.");
            b.AppendLine("                    Round 1 of generation uses N as seed, round 2 uses N+1, etc.");
            b.AppendLine("                    Default: 0.");
            b.AppendLine();
            b.AppendLine("   /restart:N       Kill the generation process and spawn a new one every N seconds.");
            b.AppendLine("                    Default is N=100 seconds.");
            b.AppendLine();
            b.AppendLine("   /truerandom      Use random seed from System.Security.Cryptography.RandomNumberGenerator.");
            b.AppendLine("                    Note: if you use this option, the /randomseed option has no effect.");
            b.AppendLine("                    Default is to use System.Random.");
            b.AppendLine();
            b.AppendLine("   /fairexploration Uses a fair algorithm for exploring the object space.");
            b.AppendLine("                    Turned off by default.");
            b.AppendLine();
            b.AppendLine();
            b.AppendLine("   Controlling which and where tests are output.");
            b.AppendLine("   ---------------------------------------------");
            b.AppendLine();
            b.AppendLine("   /outputnormal    Output test inputs that lead to non-exceptional");
            b.AppendLine("                    behavior.");
            b.AppendLine("                    Default is to output only exceptional behavior.");
            b.AppendLine();
            b.AppendLine("   /singledir       Write all results directly to the output directory;");
            b.AppendLine("                    do not create any subdirectories.");
            b.AppendLine("                    Default is to create a subdirectory per exception type");
            b.AppendLine("                    and write each test to its corresponding subdirectory.");
            b.AppendLine();
            b.AppendLine("   /outputdir       Write results to the given directory.");
            b.AppendLine("                    Default is <current-directory>\\randoop_output.");
            b.AppendLine();
            b.AppendLine("   /testprefix:P    Resulting tests will be files called Pnnn.cs, where nnn");
            b.AppendLine("                    is a number.");
            b.AppendLine("                    Default is P = \"RandoopTest\"");
            b.AppendLine();
            //THE AUXILIARY TOOLS ARE NOT PART OF THE CODE RELEASE
            b.AppendLine("   Using auxiliary tools DURING test generation.");
            b.AppendLine("   --------------------------------------------");
            b.AppendLine();
            //b.AppendLine("   /dhandler        Run dhanlder to suppress pop-up windows (e.g. assertion");
            //b.AppendLine("                    violations). Default behavior is to intercept them and");
            //b.AppendLine("                    not let them appear.");
            //b.AppendLine();
            b.AppendLine("   /pageheap        Enable heap monitoring via pageheap.");
            b.AppendLine("                    Default is not to use pageheap.");
            b.AppendLine();
            b.AppendLine("   Using auxiliary tools AFTER test generation.");
            b.AppendLine("   --------------------------------------------");
            b.AppendLine();
            b.AppendLine("   The following commands take as input a list of filenames, which can be");
            b.AppendLine("   files or directories. They work on all files ending in a specific suffix");
            b.AppendLine("   by recursively searching directories specified.");
            b.AppendLine();
            b.AppendLine("   stats:F [file]+  Looks for files ending in \".stats.txt\" and summarizes ");
            b.AppendLine("                    the information in the stats files. The results are");
            b.AppendLine("                    written to file F.");
            b.AppendLine("                    Randoop generates stats files as it generates tests, and ");
            b.AppendLine("                    they contain information about the generation process.");
            b.AppendLine();
            b.AppendLine("                    A related option is /keepstatlogs (see \"Other\" section).");
            b.AppendLine();
            b.AppendLine("   minimize [file]+ Invoke randoop's minimizer on the given files.");
            b.AppendLine("                    Files must be tests that were generated by Randoop.");
            b.AppendLine();
            b.AppendLine("   reduce [file]+   Invoke randoop's reducer on the given files.");
            b.AppendLine("                    The reduces deletes tests that it considers");
            b.AppendLine("                    redundant because they throw the same exception");
            b.AppendLine("                    and end with the same method call.");
            b.AppendLine("                    Files must be tests that were generated by Randoop.");
            b.AppendLine();
            b.AppendLine("   Other.");
            b.AppendLine("   ------");
            b.AppendLine();
            b.AppendLine("   /noexplorer      Do not launch Internet Explorer at the end.");
            b.AppendLine("                    Default is to open a web page that summarizes");
            b.AppendLine("                    Randoop's results at the end of execution.");
            b.AppendLine();
            b.AppendLine("   /keepstatlogs    Do not remove stats lots (files ending in \".stats.txt\")).");
            b.AppendLine("                    Default is to remove them at the end of a run.");
            return b.ToString();
        }
    }

    /// <summary>
    /// Represents the top-level command-line arguments that the
    /// user gave to Randoop, and provides methods for accessing them.
    /// </summary>
    public class CommandLineArguments
    {
        private string[] args;

        public int timeLimitSeconds = -1;
        public int restartTimeSeconds = -1;

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            foreach (string s in args)
                b.Append(" " + s);
            return b.ToString();
        }

        private static bool IsDllName(string s)
        {
            return s.ToLower().EndsWith(".dll") || s.ToLower().EndsWith(".exe");
        }

        public Collection<string> AssemblyNames
        {
            get
            {
                Collection<string> retval = new Collection<string>();
                foreach (string s in args)
                {
                    if (IsDllName(s))
                        retval.Add(s);
                }
                return retval;
            }
        }

        public bool TrueRandom
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/truerandom"))
                        return true;
                return false;
            }
        }

        public bool NoExplorer
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/noexplorer"))
                        return true;
                return false;
            }
        }

        public bool KeepStatLogs
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/keepstatlogs"))
                        return true;
                return false;
            }
        }


        public bool Verbose
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/verbose"))
                        return true;
                return false;
            }
        }

        public bool PageHeap
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/pageheap"))
                        return true;
                return false; 
            }
        }

        public string RandomSeed
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().StartsWith("/randomseed:"))
                        return s.Substring("/randomseed:".Length);
                return null;
            }
        }


        public bool SingleDir
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/singledir"))
                        return true;
                return false;
            }
        }

        public string OutputDir
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().StartsWith("/outputdir:"))
                        return s.Substring("/outputdir:".Length);
                return null;
            }
        }

        public string ConfigFilesDir
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().StartsWith("/configfiles:"))
                        return s.Substring("/configfiles:".Length);
                return null;
            }
        }


        public bool OutputNormal
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/outputnormal"))
                        return true;
                return false;
            }
        }

        public bool AllowNull
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/allownull"))
                        return true;
                return false;
            }
        }

        public bool NoStatic
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/nostatic"))
                        return true;
                return false;
            }
        }

        public bool ExploreInternal
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/internal"))
                        return true;
                return false;
            }
        }

        public bool UseDHandler
        {
            get
            {
                //foreach (string s in args)
                //    if (s.ToLower().Equals("/dhandler"))
                //        return true;
                return false; //NOT PART OF THE RELEASE
            }
        }

        public int TimeLimitSeconds
        {
            get
            {
                return this.timeLimitSeconds;
            }
        }

        public TimeSpan TimeLimit
        {
            get
            {
                return new TimeSpan(0, 0, this.TimeLimitSeconds);
            }
        }

        public string TimeLimitString
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().StartsWith("/timelimit:"))
                        return s;
                return null;
            }
        }

        public int RestartTimeSeconds
        {
            get
            {
                return this.restartTimeSeconds;
            }
        }

        public string RestartString
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().StartsWith("/restart:"))
                        return s;
                return null;
            }
        }

        /// <summary>
        /// NOT USER-VISIBLE.
        /// This option is used by RandoopTests project.
        /// </summary>
        public bool DontExecute
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/dontexecute"))
                        return true;
                return false;
            }
        }

        public bool Fair 
        {
            get
            {
                foreach (string s in args)
                    if (s.ToLower().Equals("/fairexploration"))
                        return true;
                return false;
            }
        }


        public CommandLineArguments(string[] args, int defaultTimeLimit, int defaultRestartTimeSeconds, out string error)
        {
            this.args = new string[args.Length];
            Array.Copy(args, this.args, args.Length);

            if (defaultTimeLimit < 0)
                throw new ArgumentException();

            bool atLeastOneDll = false;
            foreach (string s in args)
                if (IsDllName(s))
                    atLeastOneDll = true;

            if (!atLeastOneDll)
            {
                error = "No assembly specified.";
                return;
            }

            // Determine time limit.
            if (TimeLimitString != null)
            {
                if (!int.TryParse(TimeLimitString.Substring("/timelimit:".Length), out this.timeLimitSeconds))
                {
                    error = "Invalid time limit: " + TimeLimitString;
                    return;
                }
                if (this.timeLimitSeconds <= 0)
                {
                    error = "Invalid time limit: " + TimeLimitString;
                    return;
                }
            }
            else
            {
                this.timeLimitSeconds = defaultTimeLimit;
            }

            // Determine restart time.
            if (RestartString != null)
            {
                if (!int.TryParse(RestartString.Substring("/restart:".Length), out this.restartTimeSeconds))
                {
                    error = "Invalid restart time: " + RestartString;
                    return;
                }
                if (this.restartTimeSeconds <= 0)
                {
                    error = "Invalid time limit: " + RestartString;
                    return;
                }
            }
            else
            {
                this.restartTimeSeconds = defaultRestartTimeSeconds;
            }

            if (this.TimeLimitSeconds < 0)
            {
                error = "Negative time limit given (" + this.timeLimitSeconds
                + "). Will use default time limit of " + this.timeLimitSeconds + " seconds.";
                return;
            }

            error = null;

        }
    }
}
