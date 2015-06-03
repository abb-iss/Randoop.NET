using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Randoop
{
    public partial class Arguments : Form
    {

        /// <summary>
        /// Member Declarations
        /// </summary>
        private string randoop_root;
        private string allArg;
        private bool ifCancel = true;

        //TabPage 1: Inputs
        private string dll_to_test = "";
        private bool if_static = true;
        private bool if_internal = false;
        private bool if_mapped = false;
        //TabPage 2: Configuration File
        private string dir_config = "";
        //TabPage 3: Test Generation
        private string time_limit = "100";
        private string rand_seed = "0";
        private string restart_time = "100";
        private bool if_allow_null = false;
        private bool if_true_rand = false;
        private bool if_fair_explore = false;
        //TabPage 4: Test Output
        private bool if_out_normal = true;
        private bool if_out_single = false;
        private bool if_minimize = false;
        private bool if_out_plan = false;
        private int if_reduce = 0;
        private string out_dir = "";
        private string dll_dir = ""; //for copying references
        private bool outDirSet = false;
        private string testPfile = "unlimited";
        //TabPage 5: Auxiliary Tool
        private bool pageHeap = false;

        
        /// <summary>
        /// Constructor and Initialization
        /// </summary>
        public Arguments()
        {
            InitializeComponent();
            SetDefaultValue();
        }


        public Arguments(string root)
        {
            randoop_root = root;
            InitializeComponent();
            SetDefaultValue();
        }


        public void SetDllToTest(string dlltest)
        {
            dll_to_test = dlltest;
            this.InputtextBox.Text = dlltest;
            this.t6_obj_text.Text = dlltest;
            //Set default value of "OutdirtextBox.Text" -- decided by the location of .dll under test
            int nameindex = dll_to_test.LastIndexOf("\\");
            dll_dir = dll_to_test.Substring(0, nameindex);
            this.OutdirtextBox.Text = dll_dir + "\\randoop_output";
            this.t6_outdir_text.Text = this.OutdirtextBox.Text;
            out_dir = this.OutdirtextBox.Text;            
        }

        public void SetDefaultValue()   //Initialize
        {
            //hide tabpage2-6
            this.tabControl1.TabPages.Remove(tabPage2);
            this.tabControl1.TabPages.Remove(tabPage3);
            this.tabControl1.TabPages.Remove(tabPage4);
            this.tabControl1.TabPages.Remove(tabPage5);
            this.tabControl1.TabPages.Remove(tabPage6);

            this.StaticMbcheckBox.Checked = true;
            this.mapEditbutton.Enabled = false;
            this.mapExebutton.Enabled = false;
            this.ConfigtextBox.Text = this.randoop_root + "\\config_files";
            this.RTtextBox.Text = "100";
            this.t6_restart_text.Text = "100";
            this.SeedtextBox.Text = "0";
            this.t6_seed_text.Text = "0";
            this.TLtextBox.Text = "100";
            this.t6_limit_text.Text = "100";
            this.NullcomboBox.SelectedIndex = 1; //default: no
            this.t6_null_combo.SelectedIndex = 1;
            this.RandcomboBox.SelectedIndex = 1; //default: no
            this.t6_rand_combo.SelectedIndex = 1;
            this.FaircomboBox.SelectedIndex = 1; //default: no
            this.t6_fairExp_combo.SelectedIndex = 1;
            this.OutnormalcomboBox.SelectedIndex = 0; //default: yes
            this.t6_normal_combo.SelectedIndex = 0;
            this.OutsinglecomboBox.SelectedIndex = 1; //default: no
            this.t6_single_combo.SelectedIndex = 1;
            this.MinimizeCombo.SelectedIndex = 0; //default: no
            this.ReduceCombo.SelectedIndex = 0; //default: no
            this.out_plan_combo.SelectedIndex = 0; //default: no
            this.OutdirtextBox.Text = "(randoop_output dir)";
            this.testPfiletextBox.Text = "unlimited";
            this.t6_tPf_text.Text = "unlimited";
            this.t6_tPf_text.Enabled = false; //TODO: testPfile (p4)

            this.heapEnable.Checked = false;
            this.heapDisable.Checked = true;

            //changing from preference to wizard [12/22/2011]
            this.backbutton.Enabled = false;
            this.OKbutton.Enabled = false;

            //add config. place [12/23/2011]
            dir_config = randoop_root + "\\config_files";
            string[] filePaths = Directory.GetFiles(dir_config);
            foreach (string filePath in filePaths)
            {
                string fileName = filePath.Substring(filePath.LastIndexOf("\\")+1);
                if (filePath.EndsWith(".txt")) //avoid showing ".txt.bk"
                {
                    if (fileName.Equals("mapped_methods.txt"))
                        continue;
                    
                    if (fileName.StartsWith("seed"))   //Type Seeds
                        configListBox2.Items.Add(fileName);
                    else                               //API Filters
                        configListBox.Items.Add(fileName);
                }
            }

            //use toolTip to show hits when rollover on a control
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(configListBox, "Set API filters.");
            toolTip1.SetToolTip(configListBox2, "Specify simple type values (or strings) to use when calling an API that requires an simple type as input.");
            toolTip1.SetToolTip(label4,"Stop after exploring for N (input) seconds.");
            toolTip1.SetToolTip(t6_limit_text, "Stop after exploring for N seconds.");
            toolTip1.SetToolTip(label12, "Allow the use of null as a parameter.");
            toolTip1.SetToolTip(t6_null_combo, "Allow the use of null as a parameter.");
            toolTip1.SetToolTip(label9,"Use the input as the initial random seed.");
            toolTip1.SetToolTip(t6_seed_text, "Use the input as the initial random seed.");
            toolTip1.SetToolTip(label10, "Kill the generation process and spawn a new one every N (input) seconds.");
            toolTip1.SetToolTip(t6_restart_text, "Kill the generation process and spawn a new one every N (input) seconds.");
            toolTip1.SetToolTip(label13, "Use random seed from System.Security.Cryptography.RandomNumberGenerator.");
            toolTip1.SetToolTip(t6_rand_combo, "Use random seed from System.Security.Cryptography.RandomNumberGenerator.");
            toolTip1.SetToolTip(label14, "Uses a fair algorithm for exploring the object space.");
            toolTip1.SetToolTip(t6_fairExp_combo, "Uses a fair algorithm for exploring the object space.");
            toolTip1.SetToolTip(label8, "Write all results directly to the output directory; do not create any subdirectories.");
            toolTip1.SetToolTip(label7, "Output test cases that lead to non-exceptional.");
            toolTip1.SetToolTip(t6_normal_combo, "Output test cases that lead to non-exceptional.");
        }

        public int GetTimeLimit()
        {
            return Convert.ToInt32(time_limit);
        }


        public string GetTestFilepath()
        {
            return out_dir;
        }

        public string GetDllpath()
        {
            return dll_dir;
        }
        
        public string GetDllToTest()
        {
            return dll_to_test;
        }

        public int GetTestNoPerFile()
        {
            if (this.testPfile != "unlimited")
            {
                if (isNumber(this.testPfile))
                    return Convert.ToInt32(this.testPfile);
                else
                    return 0;
            }

            else
            {
                return 0;
            }
        }

        public string GetRandoopArg()
        {
            return allArg;
        }


        public bool ifContinue()
        {
            return !ifCancel;
        }

        private bool isNumber(string inStr)
        {
            for (int i = 0; i < inStr.Length; i++)
            {
                if (char.IsDigit(inStr[i]) == false)
                    return false;
            }

            return true;
        }

        public int GetReducerStatus()
        {
            return if_reduce;
        }

        public bool GetMinimizerStatus()
        {
            return if_minimize;
        }

        public bool GetOutputPlanStatus()
        {
            return if_out_plan;
        }

        /// <summary>
        /// Event Handler for buttons on the main dialog
        /// </summary>

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            StringBuilder randoopArg = new StringBuilder();

            ///////////////////
            //TabPage 1: Inputs
            ///////////////////
            if (InternalMbcheckBox.Checked)
                if_internal = true;
            if (!StaticMbcheckBox.Checked)
                if_static = false;

            if (if_internal)
                randoopArg.Append(" /internal"); // MessageBox.Show("TRUE INTERNAL");
            if (!if_static)
                randoopArg.Append(" /nostatic"); // MessageBox.Show("FALSE STATIC");


            ///////////////////////////////
            //TabPage 2: Configuration File
            ///////////////////////////////            
            
            if (dir_config != "")
            {
                randoopArg.Append(" /configfiles:\"");
                randoopArg.Append(dir_config);
                randoopArg.Append("\"");
            }
            else
            {
                MessageBox.Show("Configuration files missing!", "WARN");
            }

            ////////////////////////////
            //TabPage 3: Test Generation
            ////////////////////////////
            //if (TLtextBox.Text != "") //else there is default one
            if (t6_limit_text.Text != "")
            {
                //time_limit = TLtextBox.Text;
                time_limit = t6_limit_text.Text;
                if (!isNumber(time_limit))
                {
                    MessageBox.Show("Time Limit is not integer.", "ERROR");
                    return;
                }
            }

            //if ((SeedtextBox.Enabled) && (SeedtextBox.Text != ""))
            if ((this.t6_seed_text.Enabled) && (t6_seed_text.Text != ""))
            {
                rand_seed = t6_seed_text.Text; //SeedtextBox.Text;
                if (!isNumber(rand_seed))
                {
                    MessageBox.Show("Seed is not integer.", "ERROR");
                    return;
                }
            }

            //if (RTtextBox.Text != "")
            if (t6_restart_text.Text != "")
            {
                restart_time = t6_restart_text.Text; // RTtextBox.Text;
                if (!isNumber(restart_time))
                {
                    MessageBox.Show("Restart Time is not integer.", "ERROR");
                    return;
                }
            }
            if(t6_null_combo.SelectedIndex == 0) //if (NullcomboBox.SelectedIndex == 0)
                if_allow_null = true;
            if(t6_rand_combo.SelectedIndex == 0) //if (RandcomboBox.SelectedIndex == 0)
                if_true_rand = true;
            if(t6_fairExp_combo.SelectedIndex == 0) //if (FaircomboBox.SelectedIndex == 0)
                if_fair_explore = true;


            randoopArg.Append(" /noexplorer /timelimit:");
            randoopArg.Append(time_limit);

           // if (SeedtextBox.Enabled)
            if(t6_seed_text.Enabled)
            {
                randoopArg.Append(" /randomseed:");
                randoopArg.Append(rand_seed);
            }

            randoopArg.Append(" /restart:");
            randoopArg.Append(restart_time); // MessageBox.Show(restart_time);
            if (if_allow_null)
                randoopArg.Append(" /allownull"); // MessageBox.Show("Allow Null");
            if (if_true_rand)
                randoopArg.Append(" /truerandom"); // MessageBox.Show("TRUE RAND");
            if (if_fair_explore)
                randoopArg.Append(" /fairexploration"); // MessageBox.Show("Fair Explore");

            ////////////////////////
            //TabPage 4: Test Output
            ////////////////////////

            //if (OutnormalcomboBox.SelectedIndex == 1)
            //    if_out_normal = false;
            if(t6_normal_combo.SelectedIndex == 1)
                if_out_normal = false;

            //if (OutsinglecomboBox.SelectedIndex == 0)
            //    if_out_single = true;
            if (t6_single_combo.SelectedIndex == 0)
                if_out_single = true;

            //testPfile = testPfiletextBox.Text;
            testPfile = t6_tPf_text.Text;

            if (if_out_normal)
                randoopArg.Append(" /outputnormal");
            if (if_out_single)
                randoopArg.Append(" /singledir");

            ///////////////////////////
            //TabPage 5: Auxiliary Tool
            ///////////////////////////
            //TODO: TabPage 5 implementation (p2)

            if (pageHeap)
                randoopArg.Append(" /pageheap");


            if (out_dir != "")
            {
                randoopArg.Append(" /outputdir:\"");
                randoopArg.Append(out_dir);
                randoopArg.Append("\"");
            }
            else
            {
                MessageBox.Show("Output Dir can't be null.", "ERROR");
                return;
            }

            ///////////////////////////
            // All in 1 Command Argument
            ///////////////////////////
            if (dll_to_test == "")
            {
                MessageBox.Show("No .NET assembly (.dll or .exe) under test is selected.", "ERROR");
                return;
            }

            randoopArg.Append(" \"");
            randoopArg.Append(dll_to_test);
            randoopArg.Append("\"");
            allArg = randoopArg.ToString();

            ifCancel = false;

            Close();  //close windows form (MUST close inside to unblock the parent thread)
        }


        /// <summary>
        /// Event Handler for items in each TabPage
        /// </summary>

        //TODO: add immediate validation (p3)

        private void CONFIGbutton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ConfigtextBox.Text = folderBrowserDialog1.SelectedPath;
                dir_config = ConfigtextBox.Text;
            }
        }


        private void Outdirbutton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                OutdirtextBox.Text = folderBrowserDialog2.SelectedPath;
                out_dir = OutdirtextBox.Text;
                outDirSet = true;
            }
        }

        private void t6_browse_button_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                t6_outdir_text.Text = folderBrowserDialog2.SelectedPath;
                out_dir = t6_outdir_text.Text;
                outDirSet = true;
            }

        }


        private void Statsbutton_Click(object sender, EventArgs e)
        {
            openFileDialog2.Multiselect = true;
            openFileDialog2.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string filename in openFileDialog2.FileNames)
                {
                    builder.Append(filename);
                    builder.Append('.');
                }

                StatstextBox.Text = builder.ToString();
            }
        }

        private void Minimizebutton_Click(object sender, EventArgs e)
        {
            openFileDialog3.Multiselect = true;
            openFileDialog3.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string filename in openFileDialog3.FileNames)
                {
                    builder.Append(filename);
                    builder.Append('.');
                }

                MinimizetextBox.Text = builder.ToString();
            }
        }

        private void Reducebutton_Click(object sender, EventArgs e)
        {
            openFileDialog4.Multiselect = true;
            openFileDialog4.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
            if (openFileDialog4.ShowDialog() == DialogResult.OK)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string filename in openFileDialog4.FileNames)
                {
                    builder.Append(filename);
                    builder.Append('.');
                }

                ReducetextBox.Text = builder.ToString();
            }
        }

        private void Inputbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".NET assembilies (*.dll; *.exe)|*.dll;*.exe|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                InputtextBox.Text = openFileDialog1.FileName;
                dll_to_test = InputtextBox.Text;
                t6_obj_text.Text = dll_to_test;

                //Set default value of "OutdirtextBox.Text" -- decided by the location of .dll under test
                int nameindex = dll_to_test.LastIndexOf("\\");
                dll_dir = dll_to_test.Substring(0, nameindex);                
                if (!outDirSet)
                {
                    this.OutdirtextBox.Text = dll_dir + "\\randoop_output";
                    this.t6_outdir_text.Text = this.OutdirtextBox.Text;
                    out_dir = this.OutdirtextBox.Text;
                }

            }

        }

        private void RandcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = RandcomboBox.SelectedIndex;
            if (selectedIndex == 0) //True Random is selected
                SeedtextBox.Enabled = false; // randomseed option has no effect
            else
                SeedtextBox.Enabled = true;
        }

        private void t6_rand_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.t6_rand_combo.SelectedIndex;
            if (selectedIndex == 0) //True Random is selected
                this.t6_seed_text.Enabled = false; // randomseed option has no effect
            else
                this.t6_seed_text.Enabled = true;
        }


        private void InternalMbcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (InternalMbcheckBox.Checked)
                MessageBox.Show("WARNING:if this option is given, the generated tests may not compile.", "WARN");
        }

        private void heapEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.heapDisable.Checked = !this.heapEnable.Checked;
            pageHeap = !this.heapDisable.Checked;
        }

        private void heapDisable_CheckedChanged(object sender, EventArgs e)
        {
            this.heapEnable.Checked = !this.heapDisable.Checked;
            pageHeap = this.heapEnable.Checked;
        }

        private void nextbutton_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.TabPages.Contains(tabPage1))
            {
                tabControl1.TabPages.Remove(tabPage1);
                if (!tabControl1.TabPages.Contains(tabPage6))
                {
                    tabControl1.TabPages.Add(tabPage6);
                }

                this.backbutton.Enabled = true;
                this.nextbutton.Enabled = false;
                this.OKbutton.Enabled = true;

            }
            
        }

        private void backbutton_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.TabPages.Contains(tabPage6))
            {
                tabControl1.TabPages.Remove(tabPage6);
                if (!tabControl1.TabPages.Contains(tabPage1))
                {
                    tabControl1.TabPages.Add(tabPage1);
                }

                this.nextbutton.Enabled = true;
                this.backbutton.Enabled = false;
                this.OKbutton.Enabled = false;
            }
            
        }

        private void configEditbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "";
                string fileName2 = "";
                if (configListBox.SelectedItems.Count == 0)
                {
                    if (configListBox2.SelectedItems.Count == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Select a configuration file first.", "ERROR");
                        return;
                    }
                    else
                        fileName = configListBox2.SelectedItem.ToString();
                }
                else
                {
                    fileName = configListBox.SelectedItem.ToString();
                    if (configListBox2.SelectedItems.Count != 0)
                        fileName2 = configListBox2.SelectedItem.ToString();
                }
                
                string filePath = randoop_root + "\\config_files\\" + fileName;
                string copyPath = filePath + ".bk";
                File.Copy(filePath, copyPath, true);
                System.Diagnostics.Process.Start("notepad.exe", filePath);

                if (fileName2 != "")
                {
                    filePath = randoop_root + "\\config_files\\" + fileName2;
                    copyPath = filePath + ".bk";
                    File.Copy(filePath, copyPath, true);
                    System.Diagnostics.Process.Start("notepad.exe", filePath);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void configListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.configListBox2.SelectedItems.Count != 0)
            {
                configListBox2.ClearSelected();                
            }
        }

        private void configListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.configListBox.SelectedItems.Count != 0)
            {
                configListBox.ClearSelected();
            }
        }

        private void mapEditbutton_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = randoop_root + "\\config_files\\mapped_methods.txt";
                System.Diagnostics.Process.Start("notepad.exe", filePath);                
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "ERROR");
            }

        }

        private void mapExebutton_Click(object sender, EventArgs e)
        {
            if (dll_to_test == "")
            {
                MessageBox.Show("No .NET assembly (.dll or .exe) under test is selected.", "ERROR");
                return;
            }
            
            //call "randoop.exe methodtransformer mapfile assembilies-to-test"
            string mapfile = randoop_root + "\\config_files\\mapped_methods.txt";

            using (System.Diagnostics.Process pRandoop = new System.Diagnostics.Process())
            {
                pRandoop.StartInfo.FileName = randoop_root + "\\bin\\Randoop.exe";
                pRandoop.StartInfo.Arguments = "methodtransformer \"" + mapfile + "\" \"" + dll_to_test + "\"";

                pRandoop.StartInfo.CreateNoWindow = true;
                pRandoop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //run p in the background

                //pRandoop.StartInfo.UseShellExecute = false;
                //pRandoop.StartInfo.RedirectStandardError = true;
                //pRandoop.StartInfo.RedirectStandardOutput = true;

                pRandoop.Start();

                while (!pRandoop.HasExited)
                {
                    System.Threading.Thread.Sleep(10);
                }

                //{ //debug
                //    string output = pRandoop.StandardOutput.ReadToEnd();
                //    MessageBox.Show(output);
                //    string error = pRandoop.StandardError.ReadToEnd();
                //    MessageBox.Show(error); //in order to capture exceptions by Randoop.exe
                //} //debug

                string filename = Path.GetFileNameWithoutExtension(dll_to_test);
                string extension = Path.GetExtension(dll_to_test);
                string path = Path.GetDirectoryName(dll_to_test);

                if ((File.Exists(path+"\\"+filename+"_orig"+extension))
                    && (File.Exists(path+"\\"+filename+"_orig.pdb")))
                {
                    MessageBox.Show("Transformation completed. \n"
                    + "Please don't forget to resign the assembly if strong name was applied (using \"sn -R inputAssembly snkFile\")!");
                }
                else
                {
                    MessageBox.Show("Transformation failed. \n"
                        + "please contact xiao.qu@us.abb.com");
                }
            }
        }

        private void MethodMapMbcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (MethodMapMbcheckBox.Checked)
            {
                this.mapEditbutton.Enabled = true;
                this.mapExebutton.Enabled = true;
                if_mapped = true;
                //MessageBox.Show("Please edit the pairs of mapping methods first!");
            }
            else
            {
                this.mapEditbutton.Enabled = false;
                this.mapExebutton.Enabled = false;
                if_mapped = false;
            }
        }

        private void t6_normal_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO
        }

        private void t6_single_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO
        }

        private void MinimizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MinimizeCombo.SelectedIndex == 1)
                if_minimize = true;
            else
                if_minimize = false;
        }

        private void ReduceCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if_reduce = this.ReduceCombo.SelectedIndex;
        }

        private void out_plan_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.out_plan_combo.SelectedIndex == 1)
                if_out_plan = true;
            else
                if_out_plan = false;
                
        }                                  

    }

 
}
