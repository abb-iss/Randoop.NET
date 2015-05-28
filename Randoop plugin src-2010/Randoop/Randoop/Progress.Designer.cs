using System;
namespace Randoop
{
    partial class Progress
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.backgroundWorkerProgress = new System.ComponentModel.BackgroundWorker();
            this.progressText2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressText
            // 
            this.progressText.AutoSize = true;
            this.progressText.Location = new System.Drawing.Point(59, 80);
            this.progressText.Name = "progressText";
            this.progressText.Size = new System.Drawing.Size(35, 13);
            this.progressText.TabIndex = 0;
            this.progressText.Text = "label1";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(62, 111);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(359, 28);
            this.progressBar.TabIndex = 1;
            // 
            // backgroundWorkerProgress
            // 
            this.backgroundWorkerProgress.WorkerReportsProgress = true;
            this.backgroundWorkerProgress.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerProgress_DoWork);
            this.backgroundWorkerProgress.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerProgress_RunWorkerCompleted);
            this.backgroundWorkerProgress.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerProgress_ProgressChanged);
            // 
            // progressText2
            // 
            this.progressText2.AutoSize = true;
            this.progressText2.Location = new System.Drawing.Point(59, 58);
            this.progressText2.Name = "progressText2";
            this.progressText2.Size = new System.Drawing.Size(35, 13);
            this.progressText2.TabIndex = 2;
            this.progressText2.Text = "label1";
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 214);
            this.Controls.Add(this.progressText2);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressText);
            this.Name = "Progress";
            this.Text = "Progress";
            this.Load += new System.EventHandler(this.Progress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label progressText;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorkerProgress;

        private int totalTime;
        private string randoopPath;
        private string randoopArgs;
        private bool normalTerminate;

        private string out_dir;
        private int nTestPfile;
        private string dllTest;
        private System.Windows.Forms.Label progressText2;
        private int en_reducer;
        private bool en_minimizer;
        private bool en_output_plan;
    }
}