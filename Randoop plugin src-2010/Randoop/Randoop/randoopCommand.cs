using System.Collections.Generic;
using System.IO;
using EnvDTE;
using EnvDTE80;
using System.Reflection;
using System;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Text;
using VSLangProj;


namespace Randoop
{
    /// <summary>
    /// This command opens Windows Explorer in the folder corresponding to the specified
    /// selected item in the Solution Explorer Window.
    /// </summary>
    public class RandoopCommand : CommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenFolderCommand"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public RandoopCommand(DTE2 application)
            : base(application, "RandoopMenuCaption", "RandoopExec")
        {
        }

        /// <summary>
        /// Opens the selected item folder.
        /// </summary>
        public override void Perform()
        {
            randoopExe(application);
        }

        private void randoopExe(DTE2 application)
        {
            {
                //////////////////////////////////////////////////////////////////////////////////
                //step 1. when load randoop_net_addin, the path of "randoop" is defined 
                //////////////////////////////////////////////////////////////////////////////////
                string installPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var randoop_path =
                    installPath + "\\Randoop-NET-release";

                //////////////////////////////////////////////////////////////////////////////////
                // step 2. create win form (if an item is or is not selected in solution explorer)
                //////////////////////////////////////////////////////////////////////////////////

                UIHierarchy solutionExplorer = application.ToolWindows.SolutionExplorer;
                var items = solutionExplorer.SelectedItems as Array;
                var arg = new Arguments(randoop_path);

                //if (items.Length >= 1)
                if (items.Length == 1)
                {
                    /*
                    if (items.Length > 1)
                    {
                        MessageBox.Show("Select only one item.", "ERROR");
                        return;
                    }*/

                    UIHierarchyItem item1 = items.GetValue(0) as UIHierarchyItem;
                    var prJItem = item1.Object as ProjectItem;

                    if (prJItem != null)
                    {
                        string prjPath = prJItem.Properties.Item("FullPath").Value.ToString();
                        if (prjPath.EndsWith(".dll") || prjPath.EndsWith(".exe"))
                            arg.SetDllToTest(prjPath);

                    }
                }

                //////////////////////////////////////////////////////////////////////////////////
                // step 3. show the win form
                //////////////////////////////////////////////////////////////////////////////////

                arg.ShowDialog();

                if (arg.ifContinue() == false)
                {
                    //MessageBox.Show("not going to execute Randoop."); 
                    return;
                }

                //////////////////////////////////////////////////////////////////////////////////
                // step 4. run Randoop.exe while reporting progress
                //////////////////////////////////////////////////////////////////////////////////

                string exepath = randoop_path + "\\bin\\Randoop.exe";

                if (!File.Exists(exepath))
                {
                    MessageBox.Show("Can't find Randoop.exe!", "ERROR");
                    return;
                }

                var prg = new Progress();
                int totalTime = arg.GetTimeLimit();

                prg.getTotalTime(totalTime);
                prg.setRandoopExe(exepath);
                prg.setRandoopArg(arg.GetRandoopArg());

                /*
                prg.ShowDialog();

                if (prg.isNormal() == false)
                {
                    return;
                }
                */


                //////////////////////////////////////////////////////////////////////////////////
                // step 5. convert all test files to one RandoopTest.cs
                //////////////////////////////////////////////////////////////////////////////////

                //MessageBox.Show("Randoop finishes generating test cases.", "Progress"); // [progress tracking]
                string out_dir = arg.GetTestFilepath();
                int nTestPfile = arg.GetTestNoPerFile();

                prg.setOutDir(out_dir);
                prg.setTestpFile(nTestPfile);

                //toMSTest objToMsTest = new toMSTest();
                //MessageBox.Show("Converting test cases to MSTest format.", "Progress"); // [progress tracking]
                //objToMsTest.Convert(out_dir, nTestPfile);


                //////////////////////////////////////////////////////////////////////////////////
                // step 6. add/include RandoopTest.cs in a/the Test Project  
                //////////////////////////////////////////////////////////////////////////////////

                //MessageBox.Show("Creating a Test Project and add test files ...", "Progress"); // [progress tracking]
                string dllTest = arg.GetDllToTest();
                prg.setObjTested(dllTest);
                //CreateTestPrj(out_dir, dllTest);
                //MessageBox.Show("Task Completes!", "Progress"); // [progress tracking]

                //////////////////////////////////////////////////////////////////////////////////
                // final. progress traking complete with the progress bar 
                //////////////////////////////////////////////////////////////////////////////////

                prg.ShowDialog();

                if (prg.isNormal() == false)
                {
                    return;
                }

                string pathToTestPrj = CreateTestPrj(out_dir, dllTest, application);
                MessageBox.Show("Test file is created in project: " + pathToTestPrj);
                //invoke ie to open index.html generated by Randoop
                System.Diagnostics.Process.Start("IEXPLORE.EXE", pathToTestPrj + "\\index.html");
            }

            return;
        }


        private string CreateTestPrj(string outDir, string dll_test, DTE2 application)
        {
            try
            {
                Solution curSolution = application.Solution;

                /////////////////////////////////////////////////////////////////////////////////////////////////
                // [step a] [TODO] decide if there is a test project already existing in current solution (p2) 
                /////////////////////////////////////////////////////////////////////////////////////////////////

                //MessageBox.Show((curSolution.FullName)); //debug
                //MessageBox.Show(curSolution.GetType().ToString()); //debug


                //var prjs = curSolution.Projects;
                //if (prjs == null)
                //{
                //    MessageBox.Show(("no project object?"));
                //    return;
                //}

                //foreach (Project prj in prjs)
                //{
                //    //MessageBox.Show(prj.Kind); //debug
                //   //MessageBox.Show(prj.FullName); //debug
                //    var prjguid = new Guid(prj.Kind);
                //    if (IsVisualCSharpProject(prjguid))
                //        MessageBox.Show(prj.Name+" is C# project.", "type of project", MessageBoxButtons.OKCancel); //debug
                //    else 
                //    {
                //        if(IsTestProject(prjguid))
                //            MessageBox.Show(prj.Name + " is test project.", "type of project", MessageBoxButtons.OKCancel); //debug
                //        else
                //            MessageBox.Show(prj.Name + "is not a type that we care.", "type of project", MessageBoxButtons.OKCancel); //debug
                //    }
                //}

                //////////////////////////////////////////////////////////////////////////////////////////////
                // [step b] automatically create a Test Project (particularly for Randoop generated tests)
                //////////////////////////////////////////////////////////////////////////////////////////////

                int existRandoopTest = 0;
                var prjs = curSolution.Projects;
                if (prjs == null)
                {
                    MessageBox.Show("No project in current solution.", "ERROR");
                    return null;
                }

                foreach (Project prj in prjs)
                {
                    if (prj.Name.Contains("RandoopTestPrj"))
                        existRandoopTest = 1;
                }

                //if "RandoopTestPrj" is not existing, create a new one
                if (existRandoopTest == 0)
                {
                    string testPrjPath = curSolution.FullName;
                    string prjName = "RandoopTestPrj";
                    //MessageBox.Show(testPrjPath); //debug
                    int index = testPrjPath.LastIndexOf("\\");
                    testPrjPath = testPrjPath.Substring(0, index + 1) + prjName;
                    //MessageBox.Show(testPrjPath); //debug
                    Solution2 soln = curSolution as Solution2;
                    string csTemplatePath = soln.GetProjectTemplate("TestProject.zip", "CSharp");
                    //MessageBox.Show(csTemplatePath); //debug
                    curSolution.AddFromTemplate(csTemplatePath, testPrjPath, prjName, false); //IMPORTANT: it always returns NULL
                }

                //locate the Randoop Test Project in current solution
                var allPrjs = curSolution.Projects;
                int idTestPrj = 1;
                foreach (Project prj in allPrjs)
                {
                    if (prj.FullName.Contains("RandoopTestPrj"))
                        break;
                    idTestPrj++;
                }

                ///////////////////////////////////////////////////////////////////////////////////////
                // [step c]  add/included converted RandoopTest.cs file under the project created above
                ///////////////////////////////////////////////////////////////////////////////////////

                string testFilePath = outDir + "\\RandoopTest.cs";
                string testHtmlPath = outDir + "\\index.html";
                string testStatPath = outDir + "\\allstats.txt";
                bool isAdded1 = false;
                bool isAdded2 = false;
                bool isAdded3 = false;

                Project testPrj = curSolution.Projects.Item(idTestPrj);
                if (testPrj != null)
                {
                    foreach (ProjectItem it in testPrj.ProjectItems)
                    {
                        if (it.Name.Contains("RandoopTest.cs") && (isAdded1 == false))
                        {
                            it.Delete();
                            testPrj.ProjectItems.AddFromFileCopy(testFilePath);
                            isAdded1 = true;
                        }
                        else
                        {
                            if (it.Name.Contains("index.html") && (isAdded2 == false))
                            {
                                it.Delete();
                                testPrj.ProjectItems.AddFromFileCopy(testHtmlPath);
                                isAdded2 = true;
                            }
                            else
                            {
                                if (it.Name.Contains("allstats.txt") && (isAdded3 == false))
                                {
                                    it.Delete();
                                    testPrj.ProjectItems.AddFromFileCopy(testStatPath);
                                    isAdded3 = true;
                                }
                            }
                        }

                    }

                    if (!isAdded1)
                        testPrj.ProjectItems.AddFromFileCopy(testFilePath);

                    if (!isAdded2)
                        testPrj.ProjectItems.AddFromFileCopy(testHtmlPath);

                    if (!isAdded3)
                        testPrj.ProjectItems.AddFromFileCopy(testStatPath);


                    //if (testPrj.ProjectItems.Count > 2)
                    {
                        foreach (ProjectItem it in testPrj.ProjectItems)
                        {
                            if (it.Name.Contains("UnitTest1.cs"))
                                it.Delete();
                        }
                    }


                    //delete original randoop outputs
                    Directory.Delete(outDir, true);

                    //Programmatically add references to project under test 
                    if (existRandoopTest == 0)
                    {
                        VSProject selectedVSProject = null;
                        selectedVSProject = (VSProject)testPrj.Object;
                        selectedVSProject.References.Add(dll_test);
                    }

                    return (testPrj.FullName.Replace("\\RandoopTestPrj.csproj", ""));

                }

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ERROR: " + ex.Message);
            }

            return null;

        }


        public static bool IsVisualCSharpProject(Guid projectKind)
        {
            return projectKind.CompareTo(new Guid("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")) == 0;
        }

        public static bool IsTestProject(Guid projectKind)
        {
            return projectKind.CompareTo(new Guid("{3AC096D0-A1C2-E12C-1390-A8335801FDAB}")) == 0;
        }
            
    }
}
