namespace InstallerCreator
{
    partial class savefrm
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
            this.btnloc = new System.Windows.Forms.Button();
            this.btnok = new System.Windows.Forms.Button();
            this.txtloc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkrelative = new System.Windows.Forms.CheckBox();
            this.btnSetDir = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.installpage = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtpostinstallargs = new System.Windows.Forms.TextBox();
            this.txtpreinstallargs = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtpreinstall = new System.Windows.Forms.TextBox();
            this.btnpostinstall = new System.Windows.Forms.Button();
            this.txtpostinstall = new System.Windows.Forms.TextBox();
            this.btnpreinstall = new System.Windows.Forms.Button();
            this.uninstallpage = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtpostuninstallargs = new System.Windows.Forms.TextBox();
            this.txtpreuninstallargs = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtpreuninstall = new System.Windows.Forms.TextBox();
            this.btnpostuninstall = new System.Windows.Forms.Button();
            this.txtpostuninstall = new System.Windows.Forms.TextBox();
            this.btnpreuninstall = new System.Windows.Forms.Button();
            this.chkfolders = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.installpage.SuspendLayout();
            this.uninstallpage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnloc
            // 
            this.btnloc.Location = new System.Drawing.Point(356, 59);
            this.btnloc.Margin = new System.Windows.Forms.Padding(2);
            this.btnloc.Name = "btnloc";
            this.btnloc.Size = new System.Drawing.Size(26, 19);
            this.btnloc.TabIndex = 23;
            this.btnloc.Text = "...";
            this.btnloc.UseVisualStyleBackColor = true;
            this.btnloc.Click += new System.EventHandler(this.btnloc_Click);
            // 
            // btnok
            // 
            this.btnok.Location = new System.Drawing.Point(348, 378);
            this.btnok.Margin = new System.Windows.Forms.Padding(2);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(56, 19);
            this.btnok.TabIndex = 22;
            this.btnok.Text = "Save";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // txtloc
            // 
            this.txtloc.Location = new System.Drawing.Point(57, 57);
            this.txtloc.Margin = new System.Windows.Forms.Padding(2);
            this.txtloc.Name = "txtloc";
            this.txtloc.ReadOnly = true;
            this.txtloc.Size = new System.Drawing.Size(295, 20);
            this.txtloc.TabIndex = 21;
            this.txtloc.DoubleClick += new System.EventHandler(this.txtloc_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Location:";
            // 
            // txtname
            // 
            this.txtname.Location = new System.Drawing.Point(57, 25);
            this.txtname.Margin = new System.Windows.Forms.Padding(2);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(228, 20);
            this.txtname.TabIndex = 17;
            this.txtname.Text = "sample";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.chkrelative);
            this.groupBox1.Controls.Add(this.txtname);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtloc);
            this.groupBox1.Controls.Add(this.btnloc);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(395, 123);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Package Information";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 28);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 19);
            this.button1.TabIndex = 34;
            this.button1.Text = "Customize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkrelative
            // 
            this.chkrelative.AutoSize = true;
            this.chkrelative.Checked = true;
            this.chkrelative.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkrelative.Location = new System.Drawing.Point(7, 93);
            this.chkrelative.Name = "chkrelative";
            this.chkrelative.Size = new System.Drawing.Size(112, 17);
            this.chkrelative.TabIndex = 25;
            this.chkrelative.Text = "Use Relative Path";
            this.chkrelative.UseVisualStyleBackColor = true;
            // 
            // btnSetDir
            // 
            this.btnSetDir.Location = new System.Drawing.Point(9, 368);
            this.btnSetDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetDir.Name = "btnSetDir";
            this.btnSetDir.Size = new System.Drawing.Size(92, 19);
            this.btnSetDir.TabIndex = 33;
            this.btnSetDir.Text = "Target Folders";
            this.btnSetDir.UseVisualStyleBackColor = true;
            this.btnSetDir.Click += new System.EventHandler(this.btnSetDir_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.installpage);
            this.tabControl1.Controls.Add(this.uninstallpage);
            this.tabControl1.Location = new System.Drawing.Point(9, 138);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(395, 219);
            this.tabControl1.TabIndex = 34;
            // 
            // installpage
            // 
            this.installpage.Controls.Add(this.label7);
            this.installpage.Controls.Add(this.label6);
            this.installpage.Controls.Add(this.txtpostinstallargs);
            this.installpage.Controls.Add(this.txtpreinstallargs);
            this.installpage.Controls.Add(this.label5);
            this.installpage.Controls.Add(this.label4);
            this.installpage.Controls.Add(this.txtpreinstall);
            this.installpage.Controls.Add(this.btnpostinstall);
            this.installpage.Controls.Add(this.txtpostinstall);
            this.installpage.Controls.Add(this.btnpreinstall);
            this.installpage.Location = new System.Drawing.Point(4, 22);
            this.installpage.Name = "installpage";
            this.installpage.Padding = new System.Windows.Forms.Padding(3);
            this.installpage.Size = new System.Drawing.Size(387, 193);
            this.installpage.TabIndex = 0;
            this.installpage.Text = "Install";
            this.installpage.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 141);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 46;
            this.label7.Text = "Arguments:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 50);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 44;
            this.label6.Text = "Arguments:";
            // 
            // txtpostinstallargs
            // 
            this.txtpostinstallargs.Enabled = false;
            this.txtpostinstallargs.Location = new System.Drawing.Point(7, 159);
            this.txtpostinstallargs.Margin = new System.Windows.Forms.Padding(2);
            this.txtpostinstallargs.Name = "txtpostinstallargs";
            this.txtpostinstallargs.Size = new System.Drawing.Size(342, 20);
            this.txtpostinstallargs.TabIndex = 45;
            // 
            // txtpreinstallargs
            // 
            this.txtpreinstallargs.Enabled = false;
            this.txtpreinstallargs.Location = new System.Drawing.Point(7, 68);
            this.txtpreinstallargs.Margin = new System.Windows.Forms.Padding(2);
            this.txtpreinstallargs.Name = "txtpreinstallargs";
            this.txtpreinstallargs.Size = new System.Drawing.Size(342, 20);
            this.txtpreinstallargs.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 104);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Execute after install";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Execute before install";
            // 
            // txtpreinstall
            // 
            this.txtpreinstall.Location = new System.Drawing.Point(7, 28);
            this.txtpreinstall.Margin = new System.Windows.Forms.Padding(2);
            this.txtpreinstall.Name = "txtpreinstall";
            this.txtpreinstall.ReadOnly = true;
            this.txtpreinstall.Size = new System.Drawing.Size(342, 20);
            this.txtpreinstall.TabIndex = 38;
            this.txtpreinstall.DoubleClick += new System.EventHandler(this.txtloc_DoubleClick);
            // 
            // btnpostinstall
            // 
            this.btnpostinstall.Location = new System.Drawing.Point(353, 120);
            this.btnpostinstall.Margin = new System.Windows.Forms.Padding(2);
            this.btnpostinstall.Name = "btnpostinstall";
            this.btnpostinstall.Size = new System.Drawing.Size(26, 19);
            this.btnpostinstall.TabIndex = 42;
            this.btnpostinstall.Text = "...";
            this.btnpostinstall.UseVisualStyleBackColor = true;
            this.btnpostinstall.Click += new System.EventHandler(this.btnpostinstall_Click);
            // 
            // txtpostinstall
            // 
            this.txtpostinstall.Location = new System.Drawing.Point(7, 120);
            this.txtpostinstall.Margin = new System.Windows.Forms.Padding(2);
            this.txtpostinstall.Name = "txtpostinstall";
            this.txtpostinstall.ReadOnly = true;
            this.txtpostinstall.Size = new System.Drawing.Size(342, 20);
            this.txtpostinstall.TabIndex = 40;
            this.txtpostinstall.DoubleClick += new System.EventHandler(this.txtloc_DoubleClick);
            // 
            // btnpreinstall
            // 
            this.btnpreinstall.Location = new System.Drawing.Point(353, 28);
            this.btnpreinstall.Margin = new System.Windows.Forms.Padding(2);
            this.btnpreinstall.Name = "btnpreinstall";
            this.btnpreinstall.Size = new System.Drawing.Size(26, 19);
            this.btnpreinstall.TabIndex = 41;
            this.btnpreinstall.Text = "...";
            this.btnpreinstall.UseVisualStyleBackColor = true;
            this.btnpreinstall.Click += new System.EventHandler(this.btnpreinstall_Click);
            // 
            // uninstallpage
            // 
            this.uninstallpage.Controls.Add(this.label12);
            this.uninstallpage.Controls.Add(this.label13);
            this.uninstallpage.Controls.Add(this.txtpostuninstallargs);
            this.uninstallpage.Controls.Add(this.txtpreuninstallargs);
            this.uninstallpage.Controls.Add(this.label14);
            this.uninstallpage.Controls.Add(this.label15);
            this.uninstallpage.Controls.Add(this.txtpreuninstall);
            this.uninstallpage.Controls.Add(this.btnpostuninstall);
            this.uninstallpage.Controls.Add(this.txtpostuninstall);
            this.uninstallpage.Controls.Add(this.btnpreuninstall);
            this.uninstallpage.Location = new System.Drawing.Point(4, 22);
            this.uninstallpage.Name = "uninstallpage";
            this.uninstallpage.Padding = new System.Windows.Forms.Padding(3);
            this.uninstallpage.Size = new System.Drawing.Size(387, 193);
            this.uninstallpage.TabIndex = 1;
            this.uninstallpage.Text = "Uninstall";
            this.uninstallpage.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 141);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 46;
            this.label12.Text = "Arguments:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 50);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 44;
            this.label13.Text = "Arguments:";
            // 
            // txtpostuninstallargs
            // 
            this.txtpostuninstallargs.Enabled = false;
            this.txtpostuninstallargs.Location = new System.Drawing.Point(7, 159);
            this.txtpostuninstallargs.Margin = new System.Windows.Forms.Padding(2);
            this.txtpostuninstallargs.Name = "txtpostuninstallargs";
            this.txtpostuninstallargs.Size = new System.Drawing.Size(342, 20);
            this.txtpostuninstallargs.TabIndex = 45;
            // 
            // txtpreuninstallargs
            // 
            this.txtpreuninstallargs.Enabled = false;
            this.txtpreuninstallargs.Location = new System.Drawing.Point(7, 68);
            this.txtpreuninstallargs.Margin = new System.Windows.Forms.Padding(2);
            this.txtpreuninstallargs.Name = "txtpreuninstallargs";
            this.txtpreuninstallargs.Size = new System.Drawing.Size(342, 20);
            this.txtpreuninstallargs.TabIndex = 43;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 104);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(111, 13);
            this.label14.TabIndex = 39;
            this.label14.Text = "Execute after uninstall";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 13);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 13);
            this.label15.TabIndex = 37;
            this.label15.Text = "Execute before uninstall";
            // 
            // txtpreuninstall
            // 
            this.txtpreuninstall.Location = new System.Drawing.Point(7, 28);
            this.txtpreuninstall.Margin = new System.Windows.Forms.Padding(2);
            this.txtpreuninstall.Name = "txtpreuninstall";
            this.txtpreuninstall.ReadOnly = true;
            this.txtpreuninstall.Size = new System.Drawing.Size(342, 20);
            this.txtpreuninstall.TabIndex = 38;
            this.txtpreuninstall.DoubleClick += new System.EventHandler(this.txtloc_DoubleClick);
            // 
            // btnpostuninstall
            // 
            this.btnpostuninstall.Location = new System.Drawing.Point(353, 120);
            this.btnpostuninstall.Margin = new System.Windows.Forms.Padding(2);
            this.btnpostuninstall.Name = "btnpostuninstall";
            this.btnpostuninstall.Size = new System.Drawing.Size(26, 19);
            this.btnpostuninstall.TabIndex = 42;
            this.btnpostuninstall.Text = "...";
            this.btnpostuninstall.UseVisualStyleBackColor = true;
            this.btnpostuninstall.Click += new System.EventHandler(this.btnpostuninstall_Click);
            // 
            // txtpostuninstall
            // 
            this.txtpostuninstall.Location = new System.Drawing.Point(7, 120);
            this.txtpostuninstall.Margin = new System.Windows.Forms.Padding(2);
            this.txtpostuninstall.Name = "txtpostuninstall";
            this.txtpostuninstall.ReadOnly = true;
            this.txtpostuninstall.Size = new System.Drawing.Size(342, 20);
            this.txtpostuninstall.TabIndex = 40;
            this.txtpostuninstall.DoubleClick += new System.EventHandler(this.txtloc_DoubleClick);
            // 
            // btnpreuninstall
            // 
            this.btnpreuninstall.Location = new System.Drawing.Point(353, 28);
            this.btnpreuninstall.Margin = new System.Windows.Forms.Padding(2);
            this.btnpreuninstall.Name = "btnpreuninstall";
            this.btnpreuninstall.Size = new System.Drawing.Size(26, 19);
            this.btnpreuninstall.TabIndex = 41;
            this.btnpreuninstall.Text = "...";
            this.btnpreuninstall.UseVisualStyleBackColor = true;
            this.btnpreuninstall.Click += new System.EventHandler(this.btnpreuninstall_Click);
            // 
            // chkfolders
            // 
            this.chkfolders.AutoSize = true;
            this.chkfolders.Checked = true;
            this.chkfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkfolders.Location = new System.Drawing.Point(24, 392);
            this.chkfolders.Name = "chkfolders";
            this.chkfolders.Size = new System.Drawing.Size(128, 17);
            this.chkfolders.TabIndex = 35;
            this.chkfolders.Text = "Ask during installation";
            this.chkfolders.UseVisualStyleBackColor = true;
            // 
            // savefrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 411);
            this.Controls.Add(this.chkfolders);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSetDir);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "savefrm";
            this.Text = "Save";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.installpage.ResumeLayout(false);
            this.installpage.PerformLayout();
            this.uninstallpage.ResumeLayout(false);
            this.uninstallpage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnloc;
        private System.Windows.Forms.Button btnok;
        private System.Windows.Forms.TextBox txtloc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSetDir;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage installpage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtpostinstallargs;
        private System.Windows.Forms.TextBox txtpreinstallargs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtpreinstall;
        private System.Windows.Forms.Button btnpostinstall;
        private System.Windows.Forms.TextBox txtpostinstall;
        private System.Windows.Forms.Button btnpreinstall;
        private System.Windows.Forms.TabPage uninstallpage;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtpostuninstallargs;
        private System.Windows.Forms.TextBox txtpreuninstallargs;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtpreuninstall;
        private System.Windows.Forms.Button btnpostuninstall;
        private System.Windows.Forms.TextBox txtpostuninstall;
        private System.Windows.Forms.Button btnpreuninstall;
        private System.Windows.Forms.CheckBox chkrelative;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkfolders;

    }
}