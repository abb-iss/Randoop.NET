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
using System.Xml;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Xml.Serialization;
using System.Diagnostics;
using Randoop;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Common
{
    /// TODO: Move these classes to their own files with more intuitive names.

    public class TempDir
    {
        /// <summary>
        /// creates a temporary directory and returns its name.
        /// </summary>
        /// <returns></returns>
        /// TODO resource error handling.
        public static string CreateTempDir()
        {
            // Create name of TEMP directory.
            string tempDir = Path.GetTempPath() + Path.GetRandomFileName();
            Console.WriteLine("Creating temporary directory: " + tempDir);
            new DirectoryInfo(tempDir).Create();

            return tempDir;
        }
    }

    /// <summary>
    /// RandoopBare.MainMethod (the randoop input generator) is called with
    /// a single argument: the name of a configuration file (see
    /// Common.RandoopConfiguration). The file name can contain occurrences
    /// of the '*' character. If present, a'*' character stands for
    /// a space in the filename.
    /// 
    /// The scheme is a workaround to a problem when calling
    /// the generator with DHandler and paths that contain whitespace.
    /// </summary>
    public class ConfigFileName
    {
        // TODO this should really just take a string.
        public static String ToString(FileInfo fileInfo)
        {
            return fileInfo.FullName.Replace(' ', '*');
        }

        public static String Parse(String s)
        {
            if (s == null) throw new ArgumentNullException("s");
            return s.Replace('*', ' ');
        }
    }

    [Serializable]
    public class AssertionViolation : Exception
    {
        public AssertionViolation()
            : base()
        {
            // No body.
        }

        public AssertionViolation(String message)
            : base(message)
        {
            // No body.
        }
    }


    /// <summary>
    /// We throw this exception when the exceptional
    /// behavior indicates without question a bug in
    /// Randoop's implementation.
    /// </summary>
    [Serializable]
    public class RandoopBug : Exception
    {
        public RandoopBug()
            : base()
        {
            // No body.
        }

        public RandoopBug(string message)
            : base(message)
        {
            // No body.
        }
    }



    /// <summary>
    /// General utility methods.
    /// </summary>
    public class Misc
    {
        public static Collection<Assembly> LoadAssemblies(ICollection<FileName> binariesFileNames)
        {
            Collection<Assembly> assemblies = new Collection<Assembly>();

            foreach (FileName binaryName in binariesFileNames)
            {
                Console.WriteLine("Loading assembly " + binaryName.fileName);

                try
                {
                    Assembly assembly = Assembly.LoadFrom(binaryName.fileName);
                    assemblies.Add(assembly);
                }
                catch (Exception e)
                {
                    throw new Common.RandoopBareExceptions.InvalidUserParamsException("Unable to load assembly " + binaryName.fileName + ": " + e.Message);
                }
            }
            return assemblies;
        }

        /// <summary>
        /// Return a string (of length 3) denoting the given
        /// numerically-specified month.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        public static string Monthstring(int p)
        {
            switch (p)
            {
                case 1:
                    return "jan";
                case 2:
                    return "feb";
                case 3:
                    return "mar";
                case 4:
                    return "apr";
                case 5:
                    return "may";
                case 6:
                    return "jun";
                case 7:
                    return "jul";
                case 8:
                    return "aug";
                case 9:
                    return "sep";
                case 10:
                    return "oct";
                case 11:
                    return "nov";
                case 12:
                    return "dec";
                default:
                    throw new ArgumentException("invalid p");
            }
        }
    }


    // General utility functions.
    public class Util
    {
        /// <summary>
        /// During randoop's execution, debug listeners are removed, effectively
        /// disabling System.Diagnostics.Debug. This is done so an assertion
        /// failure in the tested code will not stop randoop's execution.
        /// To still be able to check for assertion failures in randoop
        /// itself, we use Util.Assert.
        /// </summary>
        /// <param name="condition"></param>
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new AssertionViolation();
            }
        }


        public static void Assert(bool condition, string str)
        {
            if (!condition)
            {
                throw new AssertionViolation(str);
            }
        }

        public static bool SafeEqual(object object1, object object2)
        {
            if (object1 == null)
            {
                if (object2 == null)
                    return true;
                else
                    return false;
            }
            else
            {
                if (object2 == null)
                    return false;
                else
                    try
                    {
                        return object1.Equals(object2);
                    }
                    catch
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// Checks if the object o raises exceptions for o.ToString() and o.HashCode()
        /// and an exception/false for o.Equals(o)
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ViolatesContracts(object o, out int count, out bool toStrViol, out bool hashCodeViol, out bool equalsViol)
        {
            count = 0;
            toStrViol = hashCodeViol = equalsViol = false;

            if (o == null) return false;

            try
            {
                o.ToString();
            }
            catch (Exception)
            {
                toStrViol = true;
                count++;
            }

            try
            {
                o.GetHashCode();
            }
            catch (Exception)
            {
                hashCodeViol = true;
                count++;
            }
            try
            {
                if (!o.Equals(o))
                {
                    equalsViol = true;
                    count++;
                }
            }
            catch (Exception)
            {
                count++;
                equalsViol = true;
            }

            if (count > 0)
                return true;
            return false;

        }


        /// <summary>
        /// Checks if an exception type is guideline violation
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool GuidelineViolation(Type t)
        {
            if (t.Equals(typeof(NullReferenceException)))
                return true;
            if (t.Equals(typeof(IndexOutOfRangeException)))
                return true;
            if (t.Equals(typeof(AccessViolationException)))
                return true;
            return false;
        }

        public static string PrintArray(string[] args)
        {
            StringBuilder b = new StringBuilder();
            foreach (string s in args)
            {
                b.Append(" ");
                b.Append(s == null ? "null" : s);
            }
            return b.ToString();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static string PrintProcess(Process p)
        {
            if (p == null) throw new ArgumentNullException("p");
            StringBuilder b = new StringBuilder();
            b.AppendLine("executable: " + p.StartInfo.FileName);
            b.AppendLine("arguments: " + p.StartInfo.Arguments);
            b.AppendLine("working dir: " + p.StartInfo.WorkingDirectory);
            return b.ToString();
        }

        public static string SummarizeException(Exception e, string title)
        {
            if (e == null) throw new ArgumentNullException("e");

            StringBuilder b = new StringBuilder();

            b.AppendLine(title.PadRight(70, '*'));

            b.AppendLine("EXCEPTION MESSAGE:");
            b.Append(e.Message);
            b.AppendLine();

            b.AppendLine("EXCEPTION STACK TRACE");
            b.Append(e.StackTrace);

            b.AppendLine();
            b.AppendLine("".PadRight(70, '*'));

            return b.ToString();
        }
    }



}
