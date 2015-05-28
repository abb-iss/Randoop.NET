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
using System.IO;
using Common;
using System.Diagnostics;

namespace Randoop
{
    /// <summary>
    ///  Given the command-line arguments given by the user, and other input
    /// parameters, creates an XML configuration file to be given to RandoopBare.
    /// </summary>
    class ConfigFileCreator
    {
        public static RandoopConfiguration CreateConfigFile(
           CommandLineArguments args,
           string outputDirBase,
           int lastPlanId,
           TimeSpan timeForNextInvocation,
           int round,
           string executionLogFileName,
           int randomseed)
        {
            RandoopConfiguration config = new RandoopConfiguration();

            string statsFileName = outputDirBase + "\\" + Path.GetRandomFileName() + ".stats.txt";
            config.statsFile = new FileName(statsFileName);

            config.singledir = args.SingleDir;
            config.outputnormalinputs = args.OutputNormal;
            config.outputdir = outputDirBase;
            config.planstartid = lastPlanId;
            config.timelimit = Convert.ToInt32(timeForNextInvocation.TotalSeconds);
            config.randomseed = round - 1 + randomseed;

            config.useinternal = args.ExploreInternal;
            config.usestatic = !args.NoStatic; // TODO fix this twisted logic!
            config.forbidnull = !args.AllowNull; // TODO fix this twisted logic!
            config.fairOpt = args.Fair; // execute fair algorithm

            if (args.TrueRandom)
                config.randomSource = RandomSource.Crypto;
            else
                config.randomSource = RandomSource.SystemRandom;

            foreach (string s in args.AssemblyNames)
            {
                config.assemblies.Add(new FileName(new FileInfo(s).FullName));
            }
            config.executionLog = executionLogFileName;

            if (args.DontExecute) config.executionmode = ExecutionMode.DontExecute;

            // Load any configuration files present in the user-specified directory.

            bool userSpecifiedConfigDir = false;
            if (args.ConfigFilesDir != null)
            {
                if (Directory.Exists(args.ConfigFilesDir))
                {
                    userSpecifiedConfigDir = true;
                }
                else
                {
                    Console.WriteLine("*** Warning: randoop could not find directory {0}.", args.ConfigFilesDir);
                    Console.WriteLine("             Will use default configuration files.");
                }
            }

            if (userSpecifiedConfigDir)
            {
                string configFile;

                configFile = args.ConfigFilesDir + @"\seed_sbyte.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.SbytesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(sbyte), configFile));

                configFile = args.ConfigFilesDir + @"\seed_byte.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.BytesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(byte), configFile));

                configFile = args.ConfigFilesDir + @"\seed_short.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.ShortsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(short), configFile));

                configFile = args.ConfigFilesDir + @"\seed_ushort.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.UshortsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(ushort), configFile));

                configFile = args.ConfigFilesDir + @"\seed_int.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.IntsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(int), configFile));

                configFile = args.ConfigFilesDir + @"\seed_uint.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.UintsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(uint), configFile));

                configFile = args.ConfigFilesDir + @"\seed_long.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.LongsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(long), configFile));

                configFile = args.ConfigFilesDir + @"\seed_ulong.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.UlongsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(ulong), configFile));

                configFile = args.ConfigFilesDir + @"\seed_char.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.CharsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(char), configFile));

                configFile = args.ConfigFilesDir + @"\seed_float.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.FloatsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(float), configFile));

                configFile = args.ConfigFilesDir + @"\seed_double.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.DoublesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(double), configFile));

                configFile = args.ConfigFilesDir + @"\seed_bool.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.BoolsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(bool), configFile));

                configFile = args.ConfigFilesDir + @"\seed_decimal.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.DecimalsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(decimal), configFile));

                configFile = args.ConfigFilesDir + @"\seed_string.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.StringsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(string), configFile));

                configFile = args.ConfigFilesDir + @"\require_types.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.RequiretypesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.require_typesFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = args.ConfigFilesDir + @"\require_members.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.RequiremembersDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.require_membersFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = args.ConfigFilesDir + @"\require_fields.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.RequirefieldsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.require_fieldsFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = args.ConfigFilesDir + @"\forbid_types.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.ForbidtypesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.forbid_typesFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = args.ConfigFilesDir + @"\forbid_members.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.ForbidmembersDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.forbid_membersFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = args.ConfigFilesDir + @"\forbid_fields.txt";
                if (!File.Exists(configFile))
                    configFile = Enviroment.ForbidfieldsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.forbid_fieldsFiles.Add(new FileName(new FileInfo(configFile).FullName));


            }
            else
            {
                string configFile;


                configFile = Enviroment.SbytesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(sbyte), configFile));

                configFile = Enviroment.BytesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(byte), configFile));

                configFile = Enviroment.ShortsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(short), configFile));

                configFile = Enviroment.UshortsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(ushort), configFile));

                configFile = Enviroment.IntsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(int), configFile));

                configFile = Enviroment.UintsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(uint), configFile));

                configFile = Enviroment.LongsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(long), configFile));

                configFile = Enviroment.UlongsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(ulong), configFile));

                configFile = Enviroment.CharsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(char), configFile));

                configFile = Enviroment.FloatsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(float), configFile));

                configFile = Enviroment.DoublesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(double), configFile));

                configFile = Enviroment.BoolsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(bool), configFile));

                configFile = Enviroment.DecimalsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(decimal), configFile));

                configFile = Enviroment.StringsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.simpleTypeValues.Add(CreateSimpleValueTypeXmlElement(typeof(string), configFile));

                configFile = Enviroment.RequiretypesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.require_typesFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = Enviroment.RequiremembersDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.require_membersFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = Enviroment.RequirefieldsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.require_fieldsFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = Enviroment.ForbidtypesDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.forbid_typesFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = Enviroment.ForbidmembersDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.forbid_membersFiles.Add(new FileName(new FileInfo(configFile).FullName));

                configFile = Enviroment.ForbidfieldsDefaultConfigFile;
                Console.WriteLine("Will use configuration file {0}.", configFile);
                config.forbid_fieldsFiles.Add(new FileName(new FileInfo(configFile).FullName));

            }

            config.CheckRep();
            return config;
        }

        private static SimpleTypeValues CreateSimpleValueTypeXmlElement(Type t, string configFile)
        {
            SimpleTypeValues vals = new SimpleTypeValues();
            vals.simpleType = t.FullName;
            vals.fileNames = new Collection<FileName>();

            vals.fileNames.Add(new FileName(new FileInfo(configFile).FullName));
            return vals;
        }

        private static void CheckExists(string f)
        {
            if (!File.Exists(f))
            {
                string msg = "Configuration file does not exist: " + f;
                throw new ArgumentException(msg);
            }
        }

    }
}
