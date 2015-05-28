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
using System.Diagnostics;
using Common;
using Randoop;
using System.IO;

namespace RandoopTests
{
    public class Util
    {

        public static Process RunExecutable(string executable, string args, string workingDirectory)
        {
            if (args == null) throw new ArgumentNullException();
            if (workingDirectory == null) throw new ArgumentNullException();

            Process p = new Process();
            p.StartInfo.FileName = executable;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.ErrorDialog = true;
            p.StartInfo.WorkingDirectory = workingDirectory;
            Console.WriteLine("Starting process:");
            Console.WriteLine(Common.Util.PrintProcess(p));
            p.Start();
            p.WaitForExit();
            return p;
        }

        // TODO replace with RunExecutable
        public static Process RunRandoop(string args, string workingDirectory)
        {
            if (args == null) throw new ArgumentNullException();
            if (workingDirectory == null) throw new ArgumentNullException();

            Process p = new Process();
            p.StartInfo.FileName = Common.Enviroment.RandoopExe;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.ErrorDialog = true;
            p.StartInfo.WorkingDirectory = workingDirectory;
            Console.WriteLine("Starting process:");
            Console.WriteLine(Common.Util.PrintProcess(p));
            p.Start();
            p.WaitForExit();
            return p;
        }

        private static readonly string CSC = "C:\\windows\\microsoft.net\\framework\\v2.0.50727\\csc.exe";

        /// <summary>
        /// Compiles all files ending in ".cs" under the given directory.
        /// calls csc compiler. Current directory is set to dir.
        /// Instructs csc to omit deprecation warnings.
        /// </summary>
        public static Process CompileTests(DirectoryInfo dir)
        {
            StringBuilder args = new StringBuilder();
            args.Append("/target:library /nowarn:0618 /recurse:*.cs");

            Process p = new Process();
            p.StartInfo.FileName = CSC;
            p.StartInfo.Arguments = args.ToString();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.ErrorDialog = true;
            p.StartInfo.WorkingDirectory = dir.FullName;

            Console.WriteLine("Starting process:");
            Console.WriteLine(Common.Util.PrintProcess(p));
            p.Start();
            p.WaitForExit();
            return p;
        }

    }
}
