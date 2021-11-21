
namespace TASK5
{
    partial class Form3
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WorkingLabel = new System.Windows.Forms.Label();
            this.WaitingLabel = new System.Windows.Forms.Label();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.InputLabel = new System.Windows.Forms.Label();
            this.ResultDataLabel = new System.Windows.Forms.Label();
            this.InputDataLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WorkingLabel
            // 
            this.WorkingLabel.AutoSize = true;
            this.WorkingLabel.Location = new System.Drawing.Point(0, 0);
            this.WorkingLabel.Name = "WorkingLabel";
            this.WorkingLabel.Size = new System.Drawing.Size(73, 20);
            this.WorkingLabel.TabIndex = 0;
            this.WorkingLabel.Text = "Working...";
            this.WorkingLabel.Visible = false;
            // 
            // WaitingLabel
            // 
            this.WaitingLabel.AutoSize = true;
            this.WaitingLabel.Location = new System.Drawing.Point(0, 20);
            this.WaitingLabel.Name = "WaitingLabel";
            this.WaitingLabel.Size = new System.Drawing.Size(69, 20);
            this.WaitingLabel.TabIndex = 1;
            this.WaitingLabel.Text = "Waiting...";
            this.WaitingLabel.Visible = false;
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Location = new System.Drawing.Point(0, 109);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(52, 20);
            this.ResultLabel.TabIndex = 2;
            this.ResultLabel.Text = "Result:";
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Location = new System.Drawing.Point(6, 87);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(46, 20);
            this.InputLabel.TabIndex = 3;
            this.InputLabel.Text = "Input:";
            // 
            // ResultDataLabel
            // 
            this.ResultDataLabel.AutoSize = true;
            this.ResultDataLabel.Location = new System.Drawing.Point(58, 109);
            this.ResultDataLabel.Name = "ResultDataLabel";
            this.ResultDataLabel.Size = new System.Drawing.Size(0, 20);
            this.ResultDataLabel.TabIndex = 4;
            // 
            // InputDataLabel
            // 
            this.InputDataLabel.AutoSize = true;
            this.InputDataLabel.Location = new System.Drawing.Point(58, 87);
            this.InputDataLabel.Name = "InputDataLabel";
            this.InputDataLabel.Size = new System.Drawing.Size(0, 20);
            this.InputDataLabel.TabIndex = 5;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 138);
            this.Controls.Add(this.InputDataLabel);
            this.Controls.Add(this.ResultDataLabel);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.WaitingLabel);
            this.Controls.Add(this.WorkingLabel);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WorkingLabel;
        private System.Windows.Forms.Label WaitingLabel;
        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Label ResultDataLabel;
        private System.Windows.Forms.Label InputDataLabel;
    }
}

