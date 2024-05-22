namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Form2</summary>
    partial class SaveAsJS
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
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtLoc = new System.Windows.Forms.TextBox();
            this.lblselection = new System.Windows.Forms.Label();
            this.btnFF = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtid = new System.Windows.Forms.TextBox();
            this.txtVer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.savejs = new System.Windows.Forms.CheckBox();
            this.savewsc = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description:";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(83, 82);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(306, 20);
            this.txtDesc.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtDesc, "Enter a description\r\n");
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(324, 162);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 6;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtLoc
            // 
            this.txtLoc.Location = new System.Drawing.Point(83, 114);
            this.txtLoc.Name = "txtLoc";
            this.txtLoc.ReadOnly = true;
            this.txtLoc.Size = new System.Drawing.Size(280, 20);
            this.txtLoc.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtLoc, "Select a directory name\r\n");
            // 
            // lblselection
            // 
            this.lblselection.AutoSize = true;
            this.lblselection.Location = new System.Drawing.Point(20, 114);
            this.lblselection.Name = "lblselection";
            this.lblselection.Size = new System.Drawing.Size(51, 13);
            this.lblselection.TabIndex = 9;
            this.lblselection.Text = "Location:";
            // 
            // btnFF
            // 
            this.btnFF.Location = new System.Drawing.Point(380, 117);
            this.btnFF.Name = "btnFF";
            this.btnFF.Size = new System.Drawing.Size(19, 18);
            this.btnFF.TabIndex = 5;
            this.btnFF.Text = "...";
            this.btnFF.UseVisualStyleBackColor = true;
            this.btnFF.Click += new System.EventHandler(this.btnFF_Click);
            // 
            // txtid
            // 
            this.txtid.Location = new System.Drawing.Point(83, 18);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(265, 20);
            this.txtid.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtid, "Enter a unique container id. e.g., PatientWindow\r\n");
            // 
            // txtVer
            // 
            this.txtVer.Location = new System.Drawing.Point(83, 50);
            this.txtVer.Name = "txtVer";
            this.txtVer.Size = new System.Drawing.Size(63, 20);
            this.txtVer.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtVer, "Enter a version for this revision");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Container Id:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Version:";
            // 
            // savejs
            // 
            this.savejs.AutoSize = true;
            this.savejs.Checked = true;
            this.savejs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.savejs.Location = new System.Drawing.Point(56, 162);
            this.savejs.Name = "savejs";
            this.savejs.Size = new System.Drawing.Size(41, 17);
            this.savejs.TabIndex = 15;
            this.savejs.Text = ".JS";
            this.savejs.UseVisualStyleBackColor = true;
            // 
            // savewsc
            // 
            this.savewsc.AutoSize = true;
            this.savewsc.Location = new System.Drawing.Point(113, 162);
            this.savewsc.Name = "savewsc";
            this.savewsc.Size = new System.Drawing.Size(54, 17);
            this.savewsc.TabIndex = 16;
            this.savewsc.Text = ".WSC";
            this.savewsc.UseVisualStyleBackColor = true;
            // 
            // SaveAsJS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 195);
            this.Controls.Add(this.savewsc);
            this.Controls.Add(this.savejs);
            this.Controls.Add(this.txtVer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFF);
            this.Controls.Add(this.txtLoc);
            this.Controls.Add(this.lblselection);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveAsJS";
            this.Text = "Save Selection";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtLoc;
        private System.Windows.Forms.Label lblselection;
        private System.Windows.Forms.Button btnFF;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox savejs;
        private System.Windows.Forms.CheckBox savewsc;
    }
}