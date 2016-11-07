namespace TravelMate
{
    partial class details
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
            this.ll_close = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtb_Details = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ll_close
            // 
            this.ll_close.AutoSize = true;
            this.ll_close.Location = new System.Drawing.Point(476, 386);
            this.ll_close.Name = "ll_close";
            this.ll_close.Size = new System.Drawing.Size(33, 13);
            this.ll_close.TabIndex = 0;
            this.ll_close.TabStop = true;
            this.ll_close.Text = "Close";
            this.ll_close.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_close_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(220, 199);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(3, 368);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(517, 2);
            this.label1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(253, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 199);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Our partner Hotels";
            // 
            // rtb_Details
            // 
            this.rtb_Details.Location = new System.Drawing.Point(12, 217);
            this.rtb_Details.Name = "rtb_Details";
            this.rtb_Details.ReadOnly = true;
            this.rtb_Details.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_Details.Size = new System.Drawing.Size(497, 137);
            this.rtb_Details.TabIndex = 0;
            this.rtb_Details.Text = "";
            // 
            // details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 408);
            this.Controls.Add(this.rtb_Details);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ll_close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "details";
            this.Text = "details";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel ll_close;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtb_Details;
    }
}