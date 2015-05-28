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
using System.CodeDom.Compiler;

namespace Common
{
    /// <summary>
    /// A collection of static methods that perform operations
    /// on TestCases.
    /// </summary>
    public class TestCaseUtils
    {
        public static Collection<FileInfo> CollectFilesEndingWith(string endString, params string[] fileNames)
        {
            List<FileInfo> retval = new List<FileInfo>();
            foreach (string fileName in fileNames)
            {
                if (Directory.Exists(fileName))
                {
                    retval.AddRange(CollectFilesEndingWith(endString, new DirectoryInfo(fileName)));
                }
                else if (File.Exists(fileName) && fileName.EndsWith(endString))
                {
                    retval.Add(new FileInfo(fileName));
                }
            }
            return new Collection<FileInfo>(retval);
        }


        public static Collection<FileInfo> CollectFilesEndingWith(string endString, DirectoryInfo resultsDir)
        {
            Collection<DirectoryInfo> allDirs = new Collection<DirectoryInfo>();
            allDirs.Add(resultsDir);
            foreach (DirectoryInfo di in resultsDir.GetDirectories("*", SearchOption.AllDirectories))
            {
                allDirs.Add(di);
            }

            Collection<FileInfo> retval = new Collection<FileInfo>();
            foreach (DirectoryInfo di in allDirs)
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Name.EndsWith(endString))
                        retval.Add(fi);
                }
            }
            return retval;
        }


        public static void ReproduceBehavior(Collection<FileInfo> tests)
        {
            foreach (FileInfo oneTest in tests)
            {
                if (!Reproduce(oneTest))
                {
                    Console.WriteLine("Test " + oneTest + " NOT reproducible.");
                }
            }
        }

        private static bool Reproduce(FileInfo oneTest)
        {
            TestCase test = new TestCase(oneTest);
            TestCase.RunResults results = test.RunExternal();
            if (results.behaviorReproduced)
                return true;
            return false;
        }

        public static void Minimize(Collection<FileInfo> testPaths)
        {
            foreach (FileInfo oneTest in testPaths)
            {
                int linesRemoved = Minimize(oneTest);
                if (linesRemoved > 0)
                    Console.WriteLine("Test " + oneTest + ": removed " + linesRemoved + " lines.");
            }
        }


        public static int Minimize(FileInfo testPath)
        {
            TestCase testFile = new TestCase(testPath);

            testFile.WriteToFile(testPath + ".nonmin", false);

            int linesRemoved = 0;

            //            Console.WriteLine(testFile);

            for (int linePos = testFile.NumTestLines - 1; linePos >= 0; linePos--)
            {
                string oldLine = testFile.RemoveLine(linePos);

                if (!testFile.RunExternal().behaviorReproduced)
                {
                    testFile.AddLine(linePos, oldLine);
                }
                else
                {
                    linesRemoved++;
                }
            }

            testFile.WriteToFile(testPath, false);
            return linesRemoved;
        }
        public static Collection<FileInfo> RemoveNonReproducibleErrors(Collection<FileInfo> partitionedTests)
        {
            Collection<FileInfo> reproducibleTests = new Collection<FileInfo>();
            foreach (FileInfo test in partitionedTests)
            {
                TestCase testCase = new TestCase(test);

                // Don't attempt to reproduce non-error-revealing tests (to save time).
                if (!IsErrorRevealing(testCase))
                {
                    reproducibleTests.Add(test);
                    continue;
                }

                TestCase.RunResults runResults = testCase.RunExternal();
                if (runResults.behaviorReproduced)
                    reproducibleTests.Add(test);
                else
                {
                    if (!runResults.compilationSuccessful)
                    {
                        //Console.WriteLine("@@@COMPILATIONFAILED:");
                        //foreach (CompilerError err in runResults.compilerErrors)
                        //    Console.WriteLine("@@@" + err);
                    }
                    test.Delete();
                }
            }
            return reproducibleTests;
        }


        private class EquivClass
        {
            public readonly TestCase representative;

            public EquivClass(TestCase testCase)
            {
                if (testCase == null) throw new ArgumentNullException();
                this.representative = testCase;
            }

            public override bool Equals(object obj)
            {
                EquivClass other = obj as EquivClass;
                if (other == null) return false;
                if (!this.representative.exception.Equals(other.representative.exception))
                    return false;
                if (!this.representative.lastAction.Equals(other.representative.lastAction))
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return this.representative.lastAction.GetHashCode() + 3 ^ this.representative.exception.GetHashCode();
            }

            public override string ToString()
            {
                return "<equivalence class lastAction=\"" + this.representative.lastAction
                + "\" exception=\"" + this.representative.exception + "\">";
            }
        }

        public static Collection<FileInfo> Reduce(Collection<FileInfo> tests)
        {
            Dictionary<EquivClass, TestCase> representatives = new Dictionary<EquivClass, TestCase>();
            Dictionary<EquivClass, FileInfo> representativesFileInfos = new Dictionary<EquivClass, FileInfo>();


            foreach (FileInfo file in tests)
            {
                TestCase testCase;
                try
                {
                    testCase = new TestCase(file);
                }
                catch (Exception)
                {
                    // File does not contain a valid test case, or
                    // test case is malformed.
                    continue;
                }


                EquivClass partition = new EquivClass(testCase);
                // If there are no representatives for this partition,
                // use testCase as the representative.
                if (!representatives.ContainsKey(partition))
                {
                    representatives[partition] = testCase;
                    representativesFileInfos[partition] = file;
                }
                // if testCase is smaller than the current representative,
                // use testCase as the representative.
                // Delete the old representative.
                else if (testCase.NumTestLines < representatives[partition].NumTestLines)
                {
                    representativesFileInfos[partition].Delete();
                    representatives[partition] = testCase;
                    representativesFileInfos[partition] = file;
                }
                // Representative is redundant and larger than current representative.
                // Delete representative.
                else
                {
                    file.Delete();
                }
            }

            List<FileInfo> retval = new List<FileInfo>();
            retval.AddRange(representativesFileInfos.Values);
            return new Collection<FileInfo>(retval);
        }

        public static Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>>
          ClassifyTestsByMessage(Collection<FileInfo> tests)
        {
            Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>> testsByMessage =
                new Dictionary<TestCase.ExceptionDescription, Collection<FileInfo>>();

            foreach (FileInfo t in tests)
            {
                TestCase tc;
                try
                {
                    tc = new TestCase(t);
                }
                catch (TestCase.TestCaseParseException e)
                {
                    Console.WriteLine("Problem parsing test {0}. (Ignoring test.)", t.FullName);
                    Console.WriteLine(Util.SummarizeException(e, ""));
                    continue;
                }

                Collection<FileInfo> l;

                if (!testsByMessage.ContainsKey(tc.exception))
                {
                    l = new Collection<FileInfo>();
                    testsByMessage[tc.exception] = l;
                }
                else
                {
                    l = testsByMessage[tc.exception];
                }

                l.Add(t);
            }
            return testsByMessage;
        }

        private static bool IsErrorRevealing(TestCase tc)
        {
            TestCase.ExceptionDescription d;

            d = TestCase.ExceptionDescription.GetDescription(typeof(System.AccessViolationException));
            if (d.Equals(tc.exception))
                return true;

            d = TestCase.ExceptionDescription.GetDescription(typeof(System.NullReferenceException));
            if (d.Equals(tc.exception))
                return true;

            d = TestCase.ExceptionDescription.GetDescription(typeof(System.NullReferenceException));
            if (d.Equals(tc.exception))
                return true;

            return false;
        }

        public static void WriteTestCasesAsUnitTest(
            TextWriter writer,
            string @namespace, 
            string typeName, 
            string testName,
            IEnumerable<TestCase> tests)
        {
            // collect imports
            var imports = new Dictionary<string, string>();
            imports.Add("Microsoft.VisualStudio.TestTools.UnitTesting", null);
            foreach (var test in tests)
                foreach (var import in test.Imports)
                    imports[import] = null;

            // start emitting file
            var w = new IndentedTextWriter(writer);
            foreach (var import in imports)
                w.WriteLine("using {0};", import);

            w.WriteLine("namespace {0}", @namespace);
            w.WriteLine("{");
            w.Indent++;
            {
                w.WriteLine("[TestClass]");
                w.WriteLine("public partial class {0}", typeName);
                w.WriteLine("{");
                w.Indent++;
                {
                    int testCount = 0;
                    foreach (var test in tests)
                    {
                        string testID = testCount.ToString("000");
                        test.WriteAsUnitTest(w, testName + testID);
                        w.WriteLine();
                    }
                }
                w.Indent--;
            }
            w.Indent--;
            w.WriteLine("}");
        }

    }

}
