using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Randoop
{
    public partial class Progress : Form
    {
        private StringBuilder randoopoutput = new StringBuilder();
        private StringBuilder randooperr = new StringBuilder();
        private bool isgoodinfo = false;
        private bool isgoodinfo2 = false;
        
        public Progress()
        {
            InitializeComponent();
        }

        private void Progress_Load(object sender, System.EventArgs e)
        {
            // Start the BackgroundWorker.
            backgroundWorkerProgress.RunWorkerAsync();            
        }

        private void backgroundWorkerProgress_DoWork(object sender, DoWorkEventArgs e)
        {
            normalTerminate = false;

            using (System.Diagnostics.Process pRandoop = new System.Diagnostics.Process())
            {
                pRandoop.StartInfo.FileName = randoopPath;
                pRandoop.StartInfo.Arguments = randoopArgs;

                pRandoop.StartInfo.CreateNoWindow = true;
                //pRandoop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; 

                //in order to capture exceptions by Randoop.exe
                pRandoop.StartInfo.UseShellExecute = false;
                pRandoop.StartInfo.RedirectStandardError = true; 
                pRandoop.StartInfo.RedirectStandardOutput = true;

                //pRandoop.EnableRaisingEvents = true;                
                pRandoop.OutputDataReceived += process_OutputDataReceived;
                pRandoop.ErrorDataReceived += process_ErrorDataReceived;

                //-------------------------------

                pRandoop.Start();
                DateTime startTime = DateTime.Now;
                
                pRandoop.BeginErrorReadLine();
                pRandoop.BeginOutputReadLine();

                //------------------------------------

                progressText2.Invoke((MethodInvoker) delegate { progressText2.Text = "Randoop is generating test cases ... "; });


                //------------------------------------
                
                int i = 0;
                while (!pRandoop.HasExited)
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan span = endTime.Subtract(startTime);
                    int pastTime = span.Seconds + 60 * span.Minutes;
                    i = Convert.ToInt32(Convert.ToDouble(pastTime) / Convert.ToDouble(totalTime) * Convert.ToDouble(100));
                    System.Threading.Thread.Sleep(10);
                    if (i <= 100)
                        backgroundWorkerProgress.ReportProgress(i);                    
                }


                if (i < 80)  //Randoop.NET terminates at less than 80% given generation time probably due to an internal exception
                    MessageBox.Show("Randoop encounters exceptions. \n" 
                                    + "Randoop std. error: \n\n" + randooperr + "\n"
                                    + "----------------------------------------------------- \n"
                                    + "Randoop std. output: \n\n" + randoopoutput + "\n" 
                                    + "----------------------------------------------------- \n"
                                    + "----------------------------------------------------- \n"
                                    + "Possible solutions: \n"
                                    + "Check if all assemblies are built for the same platform, preferably \"Any CPU\".  \n" // case 1
                                    + "Forget to resign the assembly under test using \"sn -R inputAssembly snkFile\"?  \n" // case 2
                                    + "\n"
                                    + "Otherwise please contact xiao.qu@us.abb.com.", "ERROR");
                else
                    normalTerminate = true;
            }

        }

        private void backgroundWorkerProgress_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("randoop done."); //debug
            //normalTerminate = true;
            doWorkLeft();
            this.Close(); 
        }

        private void backgroundWorkerProgress_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar.Value = e.ProgressPercentage;
            // Set the text.
            progressText.Text = e.ProgressPercentage.ToString()+ "%";
        }

        private void doWorkLeft()
        {
            //convert all test files to one RandoopTest.cs
            progressText2.Text = "Converting and adding test cases to project ... ";

            if (en_reducer > 0)
                doReduceWork(out_dir, en_reducer);

            if (en_minimizer)
                doMinimizeWork(out_dir);

            toMSTest objToMsTest = new toMSTest();
            if (en_output_plan)
                objToMsTest.Convert_out_plan(out_dir, nTestPfile);
            else
                objToMsTest.Convert(out_dir, nTestPfile);
            
            //int j = Convert.ToInt32(Convert.ToDouble(totalTime + 1) / Convert.ToDouble(totalTime + 5) * Convert.ToDouble(100));
            ////backgroundWorkerProgress.ReportProgress(j);
            //progressText.Text = j.ToString() + "%";

            //TODO: add/include RandoopTest.cs in a/the Test Project (p3)
            //TODO: add progress to WorkLeft (p3)
            //CreateTestPrj(out_dir, dllTest);
            ////backgroundWorkerProgress.ReportProgress(100);
            //progressText.Text = "100%";
        }


        private void doReduceWork(string files, int reduceStrategy)
        {
            //avoid Randoop Minimizer throwing "unable to parse test file (due to incorrect format)" exception
            string testfile = files + "\\RandoopTest.cs";
            string tempfile = files + "\\temp.cs";
            string tempfile2 = files + "\\temp2.cs";
            if (File.Exists(testfile))
                //File.Move(testfile, files + "\\RandoopTest.cs.bk");
                File.Delete(testfile);
            if (File.Exists(tempfile))
                //File.Move(tempfile, files + "\\temp.cs.bk");
                File.Delete(tempfile);
            if (File.Exists(tempfile2))
                //File.Move(tempfile2, files + "\\temp2.cs.bk");
                File.Delete(tempfile2);

            using (System.Diagnostics.Process pRandoop = new System.Diagnostics.Process())
            {
                pRandoop.StartInfo.FileName = randoopPath;
                if(reduceStrategy == 1) //default simple strategy
                    pRandoop.StartInfo.Arguments = "reduce \"" + files + "\"";
                else //sequence-based strategy
                    pRandoop.StartInfo.Arguments = "reduce2 \"" + files + "\"";

                pRandoop.StartInfo.CreateNoWindow = true;
                pRandoop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //run p in the background

                pRandoop.StartInfo.UseShellExecute = false;
                pRandoop.StartInfo.RedirectStandardOutput = true;
                //pRandoop.OutputDataReceived += reduce_OutputDataReceived;
                //processOutput.Length = 0; //clear string builder

                pRandoop.Start();

                while (!pRandoop.HasExited)
                {
                    System.Threading.Thread.Sleep(10);
                }

                string output = pRandoop.StandardOutput.ReadToEnd();
                MessageBox.Show(output);

                //MessageBox.Show(processOutput.ToString());               
            }

        }

        private void doMinimizeWork(string files) //TODO: NOT implemented 11/27/2012
        {
            //avoid Randoop Minimizer throwing "test file format incorrect" exception
            string testfile = files + "\\RandoopTest.cs";
            string tempfile = files + "\\temp.cs";
            string tempfile2 = files + "\\temp2.cs";
            if (File.Exists(testfile))
                File.Move(testfile, files + "\\RandoopTest.cs.bk");
            if (File.Exists(tempfile))
                File.Move(tempfile, files + "\\temp.cs.bk");
            if (File.Exists(tempfile2))
                File.Move(tempfile2, files + "\\temp2.cs.bk");

            using (System.Diagnostics.Process pRandoop = new System.Diagnostics.Process())
            {
                pRandoop.StartInfo.FileName = randoopPath;
                pRandoop.StartInfo.Arguments = "minimize \"" + files + "\"";

                pRandoop.StartInfo.CreateNoWindow = true;
                pRandoop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //run p in the background

                pRandoop.Start();

                while (!pRandoop.HasExited)
                {
                    System.Threading.Thread.Sleep(10);
                }


            }

        }

        /// <summary>
        /// Auxiliary functions for redirected outputs and errors
        /// </summary>
        /// 
        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // a line is writen to the out stream. you can use it like:
            string s= e.Data;
            
            //TODO: this is a temporary solution, ultimate solution: separate stdout and stderr in original Randoop.NET (p2)
            if ((s != null) && (s.TrimStart().StartsWith("Loading assembly")))
            {
                isgoodinfo = true;
                return;
            }

            if (isgoodinfo && (s != null))
            {
                randoopoutput.AppendLine(s);
                if (s.StartsWith("*** Randoop"))
                    isgoodinfo = false;
            }
            
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // a line is writen to the out stream. you can use it like:
            string s= e.Data;

            //TODO: this is a temporary solution, ultimate solution: separate stdout and stderr in original Randoop.NET (p2)
            if ((s != null) && (s.TrimStart().StartsWith("Loading assembly")))
            {
                isgoodinfo2 = true;
                return;
            }

            if (isgoodinfo2 && (s != null))
            {
                randooperr.AppendLine(s);
                if (s.StartsWith("*** Randoop"))
                    isgoodinfo2 = false;
            }
        }

        
       /// <summary>
       /// Set path to Randoop executable file 
       /// </summary>       
       public void setRandoopExe(string exepath)
       {
           randoopPath = exepath;
       }

       /// <summary>
       /// Set Randoop execution arguments
       /// </summary>
       public void setRandoopArg(string arg)
       {
           randoopArgs = arg;
       }

       /// <summary>
       /// Get Randoop exeuction time limit (set by user)  
       /// </summary>
       public void getTotalTime(int time)
       {
           totalTime = time;
       }      
  
        public bool isNormal()
        {
            return normalTerminate;
        }

        /// <summary>
        /// Set the output directory for generated Randoop test file 
        /// </summary>
        public void setOutDir(string outdir)
        {
            out_dir = outdir;
        }

        /// <summary>
        /// Set the number of test cases per test file 
        /// </summary>
        public void setTestpFile(int testpfile)
        {
            nTestPfile = testpfile;
        }

        /// <summary>
        /// Set the path to the .NET assembly to be tested 
        /// </summary>
        public void setObjTested(string objtested)
        {
            dllTest = objtested;
        }   
        
        /// <summary>
        /// Set if the minimizer is enabled
        /// </summary>
        public void setMinimizer(bool status)
        {
            en_minimizer = status;
        }

        /// <summary>
        /// Set if the reducer is enabled
        /// </summary>
        public void setReducer(int status)
        {
            en_reducer = status;
        }

        public void setOutPlan(bool status)
        {
            en_output_plan = status;
        }
    }
}
