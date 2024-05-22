namespace InstallerCreator
{
    partial class frmCustomActions
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
            this.cmbActions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnnew = new System.Windows.Forms.Button();
            this.btndel = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txttooltip = new System.Windows.Forms.TextBox();
            this.chkuinstall = new System.Windows.Forms.CheckBox();
            this.chkInstall = new System.Windows.Forms.CheckBox();
            this.chkwfe = new System.Windows.Forms.CheckBox();
            this.cmbpriority = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkgrp = new System.Windows.Forms.CheckBox();
            this.txtfilter = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnupdate = new System.Windows.Forms.Button();
            this.txtargs = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtexename = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkleaf = new System.Windows.Forms.CheckBox();
            this.chkfolder = new System.Windows.Forms.CheckBox();
            this.txtname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbActions
            // 
            this.cmbActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActions.FormattingEnabled = true;
            this.cmbActions.Location = new System.Drawing.Point(81, 12);
            this.cmbActions.Name = "cmbActions";
            this.cmbActions.Size = new System.Drawing.Size(250, 21);
            this.cmbActions.TabIndex = 0;
            this.cmbActions.SelectedIndexChanged += new System.EventHandler(this.cmbActions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Actions:";
            // 
            // btnnew
            // 
            this.btnnew.Location = new System.Drawing.Point(337, 12);
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(53, 23);
            this.btnnew.TabIndex = 2;
            this.btnnew.Text = "New";
            this.btnnew.UseVisualStyleBackColor = true;
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // btndel
            // 
            this.btndel.Location = new System.Drawing.Point(396, 12);
            this.btndel.Name = "btndel";
            this.btndel.Size = new System.Drawing.Size(53, 23);
            this.btndel.TabIndex = 18;
            this.btndel.Text = "Delete";
            this.btndel.UseVisualStyleBackColor = true;
            this.btndel.Click += new System.EventHandler(this.btndel_Click);
            // 
            // btnsave
            // 
            this.btnsave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnsave.Location = new System.Drawing.Point(15, 471);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(86, 23);
            this.btnsave.TabIndex = 19;
            this.btnsave.Text = "Save and Exit";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txttooltip);
            this.groupBox1.Controls.Add(this.chkuinstall);
            this.groupBox1.Controls.Add(this.chkInstall);
            this.groupBox1.Controls.Add(this.chkwfe);
            this.groupBox1.Controls.Add(this.cmbpriority);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.chkgrp);
            this.groupBox1.Controls.Add(this.txtfilter);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnupdate);
            this.groupBox1.Controls.Add(this.txtargs);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtexename);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkleaf);
            this.groupBox1.Controls.Add(this.chkfolder);
            this.groupBox1.Controls.Add(this.txtname);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 416);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "ToolTip:";
            // 
            // txttooltip
            // 
            this.txttooltip.Location = new System.Drawing.Point(18, 263);
            this.txttooltip.Multiline = true;
            this.txttooltip.Name = "txttooltip";
            this.txttooltip.Size = new System.Drawing.Size(407, 117);
            this.txttooltip.TabIndex = 37;
            // 
            // chkuinstall
            // 
            this.chkuinstall.AutoSize = true;
            this.chkuinstall.Location = new System.Drawing.Point(303, 80);
            this.chkuinstall.Name = "chkuinstall";
            this.chkuinstall.Size = new System.Drawing.Size(66, 17);
            this.chkuinstall.TabIndex = 36;
            this.chkuinstall.Text = "Uninstall";
            this.chkuinstall.UseVisualStyleBackColor = true;
            // 
            // chkInstall
            // 
            this.chkInstall.AutoSize = true;
            this.chkInstall.Location = new System.Drawing.Point(112, 80);
            this.chkInstall.Name = "chkInstall";
            this.chkInstall.Size = new System.Drawing.Size(53, 17);
            this.chkInstall.TabIndex = 35;
            this.chkInstall.Text = "Install";
            this.chkInstall.UseVisualStyleBackColor = true;
            // 
            // chkwfe
            // 
            this.chkwfe.AutoSize = true;
            this.chkwfe.Location = new System.Drawing.Point(303, 109);
            this.chkwfe.Name = "chkwfe";
            this.chkwfe.Size = new System.Drawing.Size(83, 17);
            this.chkwfe.TabIndex = 34;
            this.chkwfe.Text = "Wait for Exit";
            this.chkwfe.UseVisualStyleBackColor = true;
            // 
            // cmbpriority
            // 
            this.cmbpriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbpriority.FormattingEnabled = true;
            this.cmbpriority.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbpriority.Location = new System.Drawing.Point(301, 147);
            this.cmbpriority.Name = "cmbpriority";
            this.cmbpriority.Size = new System.Drawing.Size(73, 21);
            this.cmbpriority.TabIndex = 33;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(253, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Priority:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(391, 183);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 22);
            this.button1.TabIndex = 30;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkgrp
            // 
            this.chkgrp.AutoSize = true;
            this.chkgrp.Location = new System.Drawing.Point(112, 109);
            this.chkgrp.Name = "chkgrp";
            this.chkgrp.Size = new System.Drawing.Size(100, 17);
            this.chkgrp.TabIndex = 31;
            this.chkgrp.Text = "Supports Group";
            this.chkgrp.UseVisualStyleBackColor = true;
            // 
            // txtfilter
            // 
            this.txtfilter.Enabled = false;
            this.txtfilter.Location = new System.Drawing.Point(84, 147);
            this.txtfilter.Name = "txtfilter";
            this.txtfilter.Size = new System.Drawing.Size(154, 20);
            this.txtfilter.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Filter:";
            // 
            // btnupdate
            // 
            this.btnupdate.Enabled = false;
            this.btnupdate.Location = new System.Drawing.Point(350, 387);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(75, 23);
            this.btnupdate.TabIndex = 27;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // txtargs
            // 
            this.txtargs.Location = new System.Drawing.Point(84, 218);
            this.txtargs.Name = "txtargs";
            this.txtargs.Size = new System.Drawing.Size(341, 20);
            this.txtargs.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Arguments:";
            // 
            // txtexename
            // 
            this.txtexename.Location = new System.Drawing.Point(84, 183);
            this.txtexename.Name = "txtexename";
            this.txtexename.Size = new System.Drawing.Size(301, 20);
            this.txtexename.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Executable:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Criteria:";
            // 
            // chkleaf
            // 
            this.chkleaf.AutoSize = true;
            this.chkleaf.Location = new System.Drawing.Point(303, 52);
            this.chkleaf.Name = "chkleaf";
            this.chkleaf.Size = new System.Drawing.Size(76, 17);
            this.chkleaf.TabIndex = 21;
            this.chkleaf.Text = "Leaf Level";
            this.chkleaf.UseVisualStyleBackColor = true;
            this.chkleaf.CheckedChanged += new System.EventHandler(this.chkleaf_CheckedChanged);
            // 
            // chkfolder
            // 
            this.chkfolder.AutoSize = true;
            this.chkfolder.Location = new System.Drawing.Point(112, 52);
            this.chkfolder.Name = "chkfolder";
            this.chkfolder.Size = new System.Drawing.Size(84, 17);
            this.chkfolder.TabIndex = 20;
            this.chkfolder.Text = "Folder Level";
            this.chkfolder.UseVisualStyleBackColor = true;
            // 
            // txtname
            // 
            this.txtname.Location = new System.Drawing.Point(84, 19);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(341, 20);
            this.txtname.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Name:";
            // 
            // frmCustomActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 499);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.btndel);
            this.Controls.Add(this.btnnew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbActions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomActions";
            this.Text = "Custom Actions";
            this.Load += new System.EventHandler(this.frmCustomActions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbActions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnnew;
        private System.Windows.Forms.Button btndel;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.TextBox txtargs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtexename;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkleaf;
        private System.Windows.Forms.CheckBox chkfolder;
        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtfilter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbpriority;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkgrp;
        private System.Windows.Forms.CheckBox chkwfe;
        private System.Windows.Forms.CheckBox chkuinstall;
        private System.Windows.Forms.CheckBox chkInstall;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txttooltip;
    }
}