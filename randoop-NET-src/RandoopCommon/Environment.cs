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
using System.Reflection;

namespace Common
{
    /// <summary>
    ///  Global variables.
    /// </summary>
    public class Enviroment
    {
        private static IRandom random;

        public static IRandom Random
        {
            get { return Enviroment.random; }
            set { Enviroment.random = value; }
        }

        public const string RandoopBareInternalErrorMessage = "RANDOOPBARE ABNORMAL TERMINATION";

        public const string RandoopBareInvalidUserParametersErrorMessage = "RANDOOPBARE WAS GIVEN INVALID USER PARAMETERS";

        private static string randoopHome;

        public static string RandoopHome
        {
            get { return Enviroment.randoopHome; }
        }

        // Randoop assembly (the top-level Randoop).
        private static string randoopExe;

        public static string RandoopExe
        {
            get { return Enviroment.randoopExe; }
        }

        // RandoopBare executable.
        private static string randoopBareExe;

        public static string RandoopBareExe
        {
            get { return Enviroment.randoopBareExe; }
        }

        private static string randoopCompileDate;

        public static string RandoopCompileDate
        {
            get { return Enviroment.randoopCompileDate; }
        }
        private static string randoopVersion;

        public static string RandoopVersion
        {
            get { return Enviroment.randoopVersion; }
        }

        private static string miniDHandler;

        public static string MiniDHandler
        {
            get { return Enviroment.miniDHandler; }
        }
        private static string windiff;

        public static string Windiff
        {
            get { return Enviroment.windiff; }
        }
        private static string dHandler;

        public static string DHandler
        {
            get { return Enviroment.dHandler; }
        }
        private static string pageHeap;

        public static string PageHeap
        {
            get { return Enviroment.pageHeap; }
        }
        private static string defaultDhi;

        public static string DefaultDhi
        {
            get { return Enviroment.defaultDhi; }
        }

        // Directory where all default config files reside.
        private static string randoopDefaultConfigFilesDir;

        public static string RandoopDefaultConfigFilesDir
        {
            get { return Enviroment.randoopDefaultConfigFilesDir; }
        }

        // Default config file for sbytes.
        private static string sbytesDefaultConfigFile;

        public static string SbytesDefaultConfigFile
        {
            get { return Enviroment.sbytesDefaultConfigFile; }
        }

        private static string bytesDefaultConfigFile;

        public static string BytesDefaultConfigFile
        {
            get { return Enviroment.bytesDefaultConfigFile; }
        }


        private static string shortsDefaultConfigFile;

        public static string ShortsDefaultConfigFile
        {
            get { return Enviroment.shortsDefaultConfigFile; }
        }
        private static string ushortsDefaultConfigFile;

        public static string UshortsDefaultConfigFile
        {
            get { return Enviroment.ushortsDefaultConfigFile; }
        }
        private static string intsDefaultConfigFile;

        public static string IntsDefaultConfigFile
        {
            get { return Enviroment.intsDefaultConfigFile; }
        }
        private static string uintsDefaultConfigFile;

        public static string UintsDefaultConfigFile
        {
            get { return Enviroment.uintsDefaultConfigFile; }
        }
        private static string longsDefaultConfigFile;

        public static string LongsDefaultConfigFile
        {
            get { return Enviroment.longsDefaultConfigFile; }
        }
        private static string ulongsDefaultConfigFile;

        public static string UlongsDefaultConfigFile
        {
            get { return Enviroment.ulongsDefaultConfigFile; }
        }
        private static string charsDefaultConfigFile;

        public static string CharsDefaultConfigFile
        {
            get { return Enviroment.charsDefaultConfigFile; }
        }
        private static string floatsDefaultConfigFile;

        public static string FloatsDefaultConfigFile
        {
            get { return Enviroment.floatsDefaultConfigFile; }
        }
        private static string doublesDefaultConfigFile;

        public static string DoublesDefaultConfigFile
        {
            get { return Enviroment.doublesDefaultConfigFile; }
        }
        private static string boolsDefaultConfigFile;

        public static string BoolsDefaultConfigFile
        {
            get { return Enviroment.boolsDefaultConfigFile; }
        }
        private static string decimalsDefaultConfigFile;

        public static string DecimalsDefaultConfigFile
        {
            get { return Enviroment.decimalsDefaultConfigFile; }
        }
        private static string stringsDefaultConfigFile;

        public static string StringsDefaultConfigFile
        {
            get { return Enviroment.stringsDefaultConfigFile; }
        }
        private static string requiretypesDefaultConfigFile;

        public static string RequiretypesDefaultConfigFile
        {
            get { return Enviroment.requiretypesDefaultConfigFile; }
        }
        private static string requiremembersDefaultConfigFile;

        public static string RequiremembersDefaultConfigFile
        {
            get { return Enviroment.requiremembersDefaultConfigFile; }
        }
        private static string requirefieldsDefaultConfigFile;

        public static string RequirefieldsDefaultConfigFile
        {
            get { return Enviroment.requirefieldsDefaultConfigFile; }
        }
        private static string forbidtypesDefaultConfigFile;

        public static string ForbidtypesDefaultConfigFile
        {
            get { return Enviroment.forbidtypesDefaultConfigFile; }
        }
        private static string forbidmembersDefaultConfigFile;

        public static string ForbidmembersDefaultConfigFile
        {
            get { return Enviroment.forbidmembersDefaultConfigFile; }
        }
        private static string forbidfieldsDefaultConfigFile;

        public static string ForbidfieldsDefaultConfigFile
        {
            get { return Enviroment.forbidfieldsDefaultConfigFile; }
        }

        static Enviroment()
        {
            random = new Common.SystemRandom();

            randoopVersion = "Beta 1";

            FileInfo commonDllLocation = new FileInfo(Assembly.GetExecutingAssembly().Location);

            if (commonDllLocation.Directory.Name.Equals("Debug"))
            {
                // We're running the "visual studio" release version, where VS puts
                // each binary under a directory <project>\bin\Debug.

                // Move three up from current directory "<project>\bin\Debug".
                randoopHome = commonDllLocation.Directory.Parent.Parent.Parent.FullName;

                SetRandoopBinariesVarsVS(Configuration.Debug);
            }
            else if (commonDllLocation.Directory.Name.Equals("Release"))
            {
                // We're running the "visual studio" debug version, where VS puts
                // each binary under a directory <project>\bin\Release.

                // Move three up from current directory "<project>\bin\Release".
                randoopHome = commonDllLocation.Directory.Parent.Parent.Parent.FullName;

                SetRandoopBinariesVarsVS(Configuration.Release);
            }
            else
            {
                // We're running the "release" version, where all the randoop
                // assemblies reside under a "bin" directory.

                // Move one up from current directory "bin".
                randoopHome = commonDllLocation.Directory.Parent.FullName;

                SetRandoopBinariesVarsRelease();
            }

            randoopCompileDate = GetCompileDate();
            SetAuxToolsVars();

            
            SetDefaultConfigFileVars();
        }

        private enum Configuration { Debug, Release }

        private static void SetRandoopBinariesVarsVS(Configuration config)
        {
            string dir = (config == Configuration.Debug ? "Debug" : "Release");

            randoopExe = randoopHome + @"\Randoop\bin\" + dir + @"\Randoop.exe";
            if (!File.Exists(randoopExe))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", randoopExe);
                System.Environment.Exit(1);
            }


            randoopBareExe = randoopHome + @"\RandoopBare\bin\" + dir + @"\RandoopBare.exe";
            if (!File.Exists(randoopBareExe))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", randoopBareExe);
                System.Environment.Exit(1);
            }
        }

        private static void SetRandoopBinariesVarsRelease()
        {

            randoopExe = randoopHome + @"\bin\randoop.exe";
            if (!File.Exists(randoopExe))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", randoopExe);
                System.Environment.Exit(1);
            }


            randoopBareExe = randoopHome + @"\bin\randoopbare.exe";
            if (!File.Exists(randoopBareExe))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", randoopBareExe);
                System.Environment.Exit(1);
            }
        }

        public static void SetDefaultConfigFileVars()
        {
            randoopDefaultConfigFilesDir = randoopHome + @"\default_config_files";

            if (!Directory.Exists(randoopDefaultConfigFilesDir))
            {
                Console.WriteLine("*** Warning: randoop could not find directory {0}.", randoopDefaultConfigFilesDir);
            }


            sbytesDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_sbyte.txt";
            if (!File.Exists(sbytesDefaultConfigFile))
            {
                Console.WriteLine("*** Warning: randoop could not find file {0}.", sbytesDefaultConfigFile);
                sbytesDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_sbyte.txt";
                Console.WriteLine("             Will use file {0}.", sbytesDefaultConfigFile);
            }


            bytesDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_byte.txt";
            if (!File.Exists(bytesDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", bytesDefaultConfigFile);
                System.Environment.Exit(1);
            }


            shortsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_short.txt";
            if (!File.Exists(shortsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", shortsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            ushortsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_ushort.txt";
            if (!File.Exists(ushortsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", ushortsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            intsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_int.txt";
            if (!File.Exists(intsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", intsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            uintsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_uint.txt";
            if (!File.Exists(uintsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", uintsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            longsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_long.txt";
            if (!File.Exists(longsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", longsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            ulongsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_ulong.txt";
            if (!File.Exists(ulongsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", ulongsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            charsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_char.txt";
            if (!File.Exists(charsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", charsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            floatsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_float.txt";
            if (!File.Exists(floatsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", floatsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            doublesDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_double.txt";
            if (!File.Exists(doublesDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", doublesDefaultConfigFile);
                System.Environment.Exit(1);
            }


            boolsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_bool.txt";
            if (!File.Exists(boolsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", boolsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            decimalsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_decimal.txt";
            if (!File.Exists(decimalsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", decimalsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            stringsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\seed_string.txt";
            if (!File.Exists(stringsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", stringsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            requiretypesDefaultConfigFile = randoopDefaultConfigFilesDir + @"\require_types.txt";
            if (!File.Exists(requiretypesDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", requiretypesDefaultConfigFile);
                System.Environment.Exit(1);
            }


            requiremembersDefaultConfigFile = randoopDefaultConfigFilesDir + @"\require_members.txt";
            if (!File.Exists(requiremembersDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", requiremembersDefaultConfigFile);
                System.Environment.Exit(1);
            }


            requirefieldsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\require_fields.txt";
            if (!File.Exists(requirefieldsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", requirefieldsDefaultConfigFile);
                System.Environment.Exit(1);
            }


            forbidtypesDefaultConfigFile = randoopDefaultConfigFilesDir + @"\forbid_types.txt";
            if (!File.Exists(forbidtypesDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", forbidtypesDefaultConfigFile);
                System.Environment.Exit(1);
            }


            forbidmembersDefaultConfigFile = randoopDefaultConfigFilesDir + @"\forbid_members.txt";
            if (!File.Exists(forbidmembersDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", forbidmembersDefaultConfigFile);
                System.Environment.Exit(1);
            }


            forbidfieldsDefaultConfigFile = randoopDefaultConfigFilesDir + @"\forbid_fields.txt";
            if (!File.Exists(forbidfieldsDefaultConfigFile))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", forbidfieldsDefaultConfigFile);
                System.Environment.Exit(1);
            }
        }

        private static void SetAuxToolsVars()
        {
            //dHandler = randoopHome + @"\auxtools\dhandler\I386\DHandler.exe";
            //if (!File.Exists(dHandler))
            //{
            //    Console.WriteLine("*** Error: randoop could not find file {0}.", dHandler);
            //    System.Environment.Exit(1);
            //}


            //windiff = randoopHome + @"\auxtools\windiff.exe";
            //if (!File.Exists(windiff))
            //{
            //    Console.WriteLine("*** Error: randoop could not find file {0}.", windiff);
            //    System.Environment.Exit(1);
            //}


            //miniDHandler = randoopHome + @"\auxtools\dhandler\I386\Mini-DHandler.exe";
            //if (!File.Exists(miniDHandler))
            //{
            //    Console.WriteLine("*** Error: randoop could not find file {0}.", miniDHandler);
            //    System.Environment.Exit(1);
            //}


            pageHeap = randoopHome + @"\auxtools\pageheap.exe";
            if (!File.Exists(pageHeap))
            {
                Console.WriteLine("*** Error: randoop could not find file {0}.", pageHeap);
                System.Environment.Exit(1);
            }


            //defaultDhi = randoopHome + @"\auxtools\dhandler\I386\default.dhi";
            //if (!File.Exists(defaultDhi))
            //{
            //    Console.WriteLine("*** Error: randoop could not find file {0}.", defaultDhi);
            //    System.Environment.Exit(1);
            //}
        }

        private static string GetCompileDate()
        {
            FileInfo compileDateFile = new FileInfo(randoopHome + @"\compiledate.txt");
            if (!compileDateFile.Exists)
                return "<not available>";
            StreamReader r = null;
            try
            {
                StringBuilder b = new StringBuilder();
                r = new StreamReader(compileDateFile.FullName);
                b.Append(r.ReadLine().Trim());
                b.Append(", ");
                b.Append(r.ReadLine().Trim());
                return b.ToString();
            }
            catch (Exception)
            {
                return "<not available>";
            }
            finally
            {
                if (r != null)
                    r.Close();
            }
        }

        /// <summary>
        /// Print failure message and terminate with error code 0.
        /// </summary>
        /// <param name="failureMessage"></param>
        public static void Fail(String failureMessage)
        {
            Console.WriteLine("Randoop encountered a problem.");
            Console.WriteLine(failureMessage);
            System.Environment.Exit(1);
        }
    }
}
