using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;
using System.Windows.Forms;
using System.Text;

using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace Randoop
{
    //convert .cs test files to format of MSTest file
    public class toMSTest 
    {        
        public void Convert(string path, int testPfile) 
        {            
            //TODO: testPfile (p4)

            // keep track of all "using ... (name space)" to avoid duplication
            List<string> namespaces = new List<string>();

            var sw = new StreamWriter(path + "\\temp.cs");
            var sw_1 = new StreamWriter(path + "\\temp2.cs");
            
            namespaces.Add("System");
            namespaces.Add("System.Text");
            namespaces.Add("System.Collections.Generic");
            namespaces.Add("System.Linq");
            namespaces.Add("Microsoft.VisualStudio.TestTools.UnitTesting");

            sw.WriteLine("\n[TestClass]");
            sw.WriteLine("public class RandoopTest");
            sw.WriteLine("{");

            sw_1.WriteLine("public class RandoopTest");
            sw_1.WriteLine("{");

            sw.WriteLine("\t[TestInitialize]");
            sw.WriteLine("\tpublic void Initialize()");
            sw.WriteLine("\t{");
            sw.WriteLine("\t\t//put your own test method initialization code.");
            sw.WriteLine("\t}\n");

            sw.WriteLine("\t[TestCleanup]");
            sw.WriteLine("\tpublic void Cleanup()");
            sw.WriteLine("\t{");
            sw.WriteLine("\t\t//put your own test method cleanup code.");
            sw.WriteLine("\t}\n");

            int numTest = 0;
            List<string> dirs = FileHelper.GetFilesRecursive(path);

            foreach (string testfile in dirs)
            {
                if (!(testfile.Contains("RandoopTest") && testfile.EndsWith(".cs")) 
                    || (testfile.Contains("RandoopTest.cs")))
              //TODO: (t_cov) used for OpenCover (opt.)
             //       || (testfile.Contains("RandoopTestGeneral.cs"))) 
                    continue;

                if (testfile.Substring(path.Length + 1).Contains("temp")) //faulty(contract violation) test case, discard
                    continue;

                //if (isNonDeterministic(testfile))
                //{
                //    RemoveAssertion(testfile);                    
                //}                

                //determine the type of test: normal, exceptions, or ...
                string testfileRelPath = testfile.Substring(path.Length + 1);
                string testType = testfileRelPath.Substring(0, testfileRelPath.LastIndexOf("\\"));

                numTest++;
                var sr = new StreamReader(testfile);
                
                string line;
                bool isStart = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("using "))
                    {
                        line = line.Trim();
                        string name = line.Substring(6,line.Length - (6+1)); //len("using ")=6, len(";")=1
                        name = name.Trim();
                        if (namespaces.IndexOf(name) == -1)
                        {
                        //    MessageBox.Show(name); //debug
                            namespaces.Add(name);
                        }
                        else
                            continue; //this namespace has been added
                    }  
                        
                    //if (line.Contains("public static int Main"))
                    if(line.Contains("BEGIN TEST"))
                    {
                        //specify the type of test: normal, exceptions, or ...
                        sw.WriteLine("\r\n\t// Test Case Type: " + testType);
                        sw.WriteLine("\t[TestMethod]");   
                        if(testType.ToLower().Contains("exception"))
                            sw.WriteLine("\t[ExpectedException(typeof("+ testType + "))]");  
                        
                        sw.WriteLine("\tpublic void TestMethod" + numTest.ToString() + "()");
                        sw.WriteLine("\t{");
                        isStart = true;

                        sw_1.WriteLine("\tpublic void TestMethod" + numTest.ToString() + "()");
                        sw_1.WriteLine("\t{");
                        sw_1.WriteLine("\t try{");
                    }              
                    
                    if (line.StartsWith("//") && !line.Contains("Regression assertion")) //lines with space before "//" are not counted
                        continue;                                       

                    //if (line.Contains("END TEST"))
                    if(line.Trim().StartsWith("/*")) //skip the internal output of Randoop about test plans [11/06/2012]
                    {
                        //sw.WriteLine(line);
                        sw.WriteLine("      //END TEST");
                        sw.WriteLine("      return;");
                        sw.WriteLine("\t}\r\n");

                        //sw_1.WriteLine(line);
                        sw_1.WriteLine("      //END TEST");
                        sw_1.WriteLine("      return;");
                        sw_1.WriteLine("\t }");
                        sw_1.WriteLine("\t catch (System.Exception e)");
                        sw_1.WriteLine("\t {");
                        sw_1.WriteLine("\t  return;");
                        sw_1.WriteLine("\t }");
                        sw_1.WriteLine("\t}\r\n");

                        break;
                    }

                    if (isStart)
                    {
                        sw.WriteLine(line);
                        sw_1.WriteLine(line);
                    }
                } //end of "while" (parsing one test file)

                sr.Close(); //close the test file being read

                // backup(rename) original test files created by Randoop
                try
                {
                    //File.Move(testfile, testfile+".bk"); // Try to move
                    //Console.WriteLine("Moved"); // Success
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Exception occurs when backing up " + testfile + ": " + ex.Message, "EXCEPTION"); // Write error
                }
                
            } //end of all test files

            sw.WriteLine("}");
            sw_1.WriteLine("}");

            sw.Close();
            sw_1.Close();

            var sw2 = new StreamWriter(path + "\\RandoopTest.cs");
            var sr2 = new StreamReader(path + "\\temp.cs");
            //TODO: (t_cov) used for OpenCover (opt.)
            //var sw3 = new StreamWriter(path + "\\RandoopTestGeneral.cs");            
            //var sr3 = new StreamReader(path + "\\temp2.cs"); 
            foreach (string name in namespaces)
            {
                sw2.WriteLine("using " + name + ";");
                //sw3.WriteLine("using " + name + ";"); //used for OpenCover
            }
            sw2.WriteLine("");
            //sw3.WriteLine(""); //used for OpenCover
            string line2 = "";
            while ((line2 = sr2.ReadLine()) != null)
            {
                sw2.WriteLine(line2);
                
            }
            sr2.Close();
            sw2.Close();

            //TODO: (t_cov) used for OpenCover (opt.)
            /********************** used for OpenCover [start] **********************/
            //line2 = "";
            //while ((line2 = sr3.ReadLine()) != null)
            //{
            //    if (line2 == "public class RandoopTest")
            //    {
            //        sw3.WriteLine("public class RandoopTestGeneral");
            //        continue;
            //    }
            //    //if (line2.Contains("[Test") || line2.Contains("[ExpectedException(typeof"))
            //    //    continue;
            //    sw3.WriteLine(line2);
            //}
            //sr3.Close();
            //sw3.Close();
            
            //var sw4 = new StreamWriter(path + "\\TestDriver.cs");
            //sw4.WriteLine("using System;");
            //sw4.WriteLine("using System.Collections.Generic;");
            //sw4.WriteLine("using System.Linq;");
            //sw4.WriteLine("using System.Text;");
            //sw4.WriteLine("");
            //sw4.WriteLine("class TestDriver");
            //sw4.WriteLine("{");
            //sw4.WriteLine("    [STAThread]");
            //sw4.WriteLine("    static void Main()");
            //sw4.WriteLine("    {");
            //sw4.WriteLine("        RandoopTestGeneral tests = new RandoopTestGeneral();");
            //int id = 1;
            //while(id < numTest)
            //    sw4.WriteLine("        tests.TestMethod" + id++.ToString() + "();");
            //sw4.WriteLine("    }");
            //sw4.WriteLine("}");
            
            //sw4.Close();
            /*********************** used for OpenCover [end] ***********************/                         

         } // end of method Convert()


        public void Convert_out_plan(string path, int testPfile)
        {
            //TODO: testPfile (p4)

            // keep track of all "using ... (name space)" to avoid duplication
            List<string> namespaces = new List<string>();

            var sw = new StreamWriter(path + "\\temp.cs");
            var sw_1 = new StreamWriter(path + "\\temp2.cs");

            namespaces.Add("System");
            namespaces.Add("System.Text");
            namespaces.Add("System.Collections.Generic");
            namespaces.Add("System.Linq");
            namespaces.Add("Microsoft.VisualStudio.TestTools.UnitTesting");

            sw.WriteLine("\n[TestClass]");
            sw.WriteLine("public class RandoopTest");
            sw.WriteLine("{");

            sw_1.WriteLine("public class RandoopTest");
            sw_1.WriteLine("{");

            sw.WriteLine("\t[TestInitialize]");
            sw.WriteLine("\tpublic void Initialize()");
            sw.WriteLine("\t{");
            sw.WriteLine("\t\t//put your own test method initialization code.");
            sw.WriteLine("\t}\n");

            sw.WriteLine("\t[TestCleanup]");
            sw.WriteLine("\tpublic void Cleanup()");
            sw.WriteLine("\t{");
            sw.WriteLine("\t\t//put your own test method cleanup code.");
            sw.WriteLine("\t}\n");

            int numTest = 0;
            List<string> dirs = FileHelper.GetFilesRecursive(path);

            foreach (string testfile in dirs)
            {
                if (!(testfile.Contains("RandoopTest") && testfile.EndsWith(".cs"))
                    || (testfile.Contains("RandoopTest.cs")))
                    //TODO: (t_cov) used for OpenCover (opt.)
                    //       || (testfile.Contains("RandoopTestGeneral.cs"))) 
                    continue;

                if (testfile.Substring(path.Length + 1).Contains("temp")) //faulty(contract violation) test case, discard
                    continue;

                //if (isNonDeterministic(testfile)) 
                //{
                //    RemoveAssertion(testfile);
                //}

                //determine the type of test: normal, exceptions, or ...
                string testfileRelPath = testfile.Substring(path.Length + 1);
                string testType = testfileRelPath.Substring(0, testfileRelPath.LastIndexOf("\\"));

                numTest++;
                var sr = new StreamReader(testfile);

                string line;
                bool isStart = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("using "))
                    {
                        line = line.Trim();
                        string name = line.Substring(6, line.Length - (6 + 1)); //len("using ")=6, len(";")=1
                        name = name.Trim();
                        if (namespaces.IndexOf(name) == -1)
                        {
                            //    MessageBox.Show(name); //debug
                            namespaces.Add(name);
                        }
                        else
                            continue; //this namespace has been added
                    }

                    //if (line.Contains("public static int Main"))
                    if (line.Contains("BEGIN TEST"))
                    {
                        //specify the type of test: normal, exceptions, or ...
                        sw.WriteLine("\r\n\t// Test Case Type: " + testType);
                        sw.WriteLine("\t[TestMethod]");
                        if (testType.ToLower().Contains("exception"))
                            sw.WriteLine("\t[ExpectedException(typeof(" + testType + "))]");

                        sw.WriteLine("\tpublic void TestMethod" + numTest.ToString() + "()");
                        sw.WriteLine("\t{");
                        isStart = true;

                        sw_1.WriteLine("\tpublic void TestMethod" + numTest.ToString() + "()");
                        sw_1.WriteLine("\t{");
                        sw_1.WriteLine("\t try{");
                    }

                    if (line.StartsWith("//") && !line.Contains("Regression assertion")) //lines with space before "//" are not counted
                        continue;

                    if (line.Contains("END TEST"))
                    {
                        sw.WriteLine(line);
                        sw.WriteLine("      return;");
                        sw.WriteLine("\t}\r\n");

                        sw_1.WriteLine(line);
                        sw_1.WriteLine("      return;");
                        sw_1.WriteLine("\t }");
                        sw_1.WriteLine("\t catch (System.Exception e)");
                        sw_1.WriteLine("\t {");
                        sw_1.WriteLine("\t  return;");
                        sw_1.WriteLine("\t }");
                        sw_1.WriteLine("\t}\r\n");

                        break;
                    }

                    if (isStart)
                    {
                        if (line.Contains("transformer="))
                        {
                            int indx1 = line.IndexOf("=");
                            int indx2 = line.IndexOf("parent");
                            line = "      " + line.Substring(indx1+1, indx2-indx1-1);
                        }

                        sw.WriteLine(line);
                        sw_1.WriteLine(line);
                    }
                } //end of "while" (parsing one test file)

                sr.Close(); //close the test file being read

                // backup(rename) original test files created by Randoop
                try
                {
                    //File.Move(testfile, testfile+".bk"); // Try to move
                    //Console.WriteLine("Moved"); // Success
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Exception occurs when backing up " + testfile + ": " + ex.Message, "EXCEPTION"); // Write error
                }

            } //end of all test files

            sw.WriteLine("}");
            sw_1.WriteLine("}");

            sw.Close();
            sw_1.Close();

            var sw2 = new StreamWriter(path + "\\RandoopTest.cs");
            var sr2 = new StreamReader(path + "\\temp.cs");
            //TODO: (t_cov) used for OpenCover (opt.)
            //var sw3 = new StreamWriter(path + "\\RandoopTestGeneral.cs");            
            //var sr3 = new StreamReader(path + "\\temp2.cs"); 
            foreach (string name in namespaces)
            {
                sw2.WriteLine("using " + name + ";");
                //sw3.WriteLine("using " + name + ";"); //used for OpenCover
            }
            sw2.WriteLine("");
            //sw3.WriteLine(""); //used for OpenCover
            string line2 = "";
            while ((line2 = sr2.ReadLine()) != null)
            {
                sw2.WriteLine(line2);

            }
            sr2.Close();
            sw2.Close();

            //TODO: (t_cov) used for OpenCover (opt.)
            /********************** used for OpenCover [start] **********************/
            //line2 = "";
            //while ((line2 = sr3.ReadLine()) != null)
            //{
            //    if (line2 == "public class RandoopTest")
            //    {
            //        sw3.WriteLine("public class RandoopTestGeneral");
            //        continue;
            //    }
            //    //if (line2.Contains("[Test") || line2.Contains("[ExpectedException(typeof"))
            //    //    continue;
            //    sw3.WriteLine(line2);
            //}
            //sr3.Close();
            //sw3.Close();

            //var sw4 = new StreamWriter(path + "\\TestDriver.cs");
            //sw4.WriteLine("using System;");
            //sw4.WriteLine("using System.Collections.Generic;");
            //sw4.WriteLine("using System.Linq;");
            //sw4.WriteLine("using System.Text;");
            //sw4.WriteLine("");
            //sw4.WriteLine("class TestDriver");
            //sw4.WriteLine("{");
            //sw4.WriteLine("    [STAThread]");
            //sw4.WriteLine("    static void Main()");
            //sw4.WriteLine("    {");
            //sw4.WriteLine("        RandoopTestGeneral tests = new RandoopTestGeneral();");
            //int id = 1;
            //while(id < numTest)
            //    sw4.WriteLine("        tests.TestMethod" + id++.ToString() + "();");
            //sw4.WriteLine("    }");
            //sw4.WriteLine("}");

            //sw4.Close();
            /*********************** used for OpenCover [end] ***********************/

        } // end of method Convert_out_plan()

        bool isNonDeterministic(string testfile) //determine if a test case (file) is non-deterministic
        {            
            string[] code = File.ReadAllLines(testfile);
            if (CompileAndRun(code, Path.GetDirectoryName(testfile)))
                return false;
            else
                return true;
        }

        void RemoveAssertion(string testfile) //remove the regression assertions if a test case is non-deterministic
        {
            StringBuilder newFile = new StringBuilder();
            string temp = "";
            string[] file = File.ReadAllLines(testfile);
            
            foreach (string line in file)
            {

                if (line.Contains("Assert") && line.Contains("Regression Failure?"))
                {
                    temp = line.Insert(0, "//"); 
                    newFile.Append(temp + "\r\n");
                    continue;
                }

                newFile.Append(line + "\r\n");
            }

            File.WriteAllText(testfile, newFile.ToString());
        }

        bool CompileAndRun(string[] code, string dir)        
        {
            //[TODO] copy all assemblies referred otherwise won't compile

            //credit: http:/simeonpilgrim.com/blog/2007/12/04/compiling-and-running-code-at-runtime/
            CompilerParameters CompilerParams = new CompilerParameters();
            string outputDirectory = dir;

            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";

            string[] references = { "System.dll" };
            CompilerParams.ReferencedAssemblies.AddRange(references);
            //[TODO] add more references?

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, code);

            if (compile.Errors.HasErrors)
            {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors)
                {
                    text += "\r\n" + ce.ToString();
                }
                throw new Exception(text);                
            }

            //ExpoloreAssembly(compile.CompiledAssembly);

            Module module = compile.CompiledAssembly.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;

            if (module != null)
            {
                mt = module.GetType("DynaCore.DynaCore");
            }
            else 
                throw new Exception("moude is null");

            if (mt != null)
            {
                methInfo = mt.GetMethod("Main");
            }
            else
                throw new Exception("DynaCore.DynaCore is null");

            if (methInfo != null)
            {
                //Console.WriteLine(methInfo.Invoke(null, new object[] { "here in dyna code" }));
                try
                {
                    var rt = methInfo.Invoke(null, null);
                    if((rt.Equals(99)) || (rt.Equals(100)))
                        return true;
                    else
                        throw new Exception("return value must be 99 or 100, or it throws exception");
                }
                catch
                {
                    return false;
                }

            }
            throw new Exception("method info is null");
        }
        
    } // end of class RandoopTest
    
    static public class FileHelper 
    // [Author] http: //www.dotnetperls.com/recursively-find-files
    {
        public static List<string> GetFilesRecursive(string b)
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            stack.Push(b);

            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.*"));

                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }
    } // end of class FileHelper
}
