namespace LBCodeGenerator
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.cmbfiles = new System.Windows.Forms.ComboBox();
            this.btnnew = new System.Windows.Forms.Button();
            this.btnremove = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btngo = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnhelp = new System.Windows.Forms.Button();
            this.txtstatus = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtoutput = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtpath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.dcompage = new System.Windows.Forms.TabPage();
            this.chksupportcomplus = new System.Windows.Forms.CheckBox();
            this.btnupdate = new System.Windows.Forms.Button();
            this.chkincoptional = new System.Windows.Forms.CheckBox();
            this.chkruntime35 = new System.Windows.Forms.CheckBox();
            this.chkregfiles = new System.Windows.Forms.CheckBox();
            this.txtprogid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnedit = new System.Windows.Forms.Button();
            this.chkvbs = new System.Windows.Forms.CheckBox();
            this.chkcs = new System.Windows.Forms.CheckBox();
            this.wcfsvrpage = new System.Windows.Forms.TabPage();
            this.chkwcfruntime35 = new System.Windows.Forms.CheckBox();
            this.txtwcfns = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.dcompage.SuspendLayout();
            this.wcfsvrpage.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbfiles
            // 
            this.cmbfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbfiles.DropDownWidth = 200;
            this.cmbfiles.FormattingEnabled = true;
            this.cmbfiles.Location = new System.Drawing.Point(14, 35);
            this.cmbfiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbfiles.Name = "cmbfiles";
            this.cmbfiles.Size = new System.Drawing.Size(384, 28);
            this.cmbfiles.TabIndex = 0;
            this.cmbfiles.SelectedIndexChanged += new System.EventHandler(this.cmbfiles_SelectedIndexChanged);
            // 
            // btnnew
            // 
            this.btnnew.Location = new System.Drawing.Point(493, 35);
            this.btnnew.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(68, 35);
            this.btnnew.TabIndex = 1;
            this.btnnew.Text = "Load";
            this.btnnew.UseVisualStyleBackColor = true;
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // btnremove
            // 
            this.btnremove.Image = global::LBCodeGenerator.Resource2.erase;
            this.btnremove.Location = new System.Drawing.Point(406, 30);
            this.btnremove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnremove.Name = "btnremove";
            this.btnremove.Size = new System.Drawing.Size(42, 44);
            this.btnremove.TabIndex = 2;
            this.btnremove.UseVisualStyleBackColor = true;
            this.btnremove.Click += new System.EventHandler(this.btnremove_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Output folder:";
            // 
            // btngo
            // 
            this.btngo.Location = new System.Drawing.Point(464, 54);
            this.btngo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btngo.Name = "btngo";
            this.btngo.Size = new System.Drawing.Size(165, 35);
            this.btngo.TabIndex = 5;
            this.btngo.Text = "Generate Code";
            this.btngo.UseVisualStyleBackColor = true;
            this.btngo.Click += new System.EventHandler(this.btngo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "ClsIDs";
            // 
            // btnhelp
            // 
            this.btnhelp.Image = ((System.Drawing.Image)(resources.GetObject("btnhelp.Image")));
            this.btnhelp.Location = new System.Drawing.Point(14, 60);
            this.btnhelp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnhelp.Name = "btnhelp";
            this.btnhelp.Size = new System.Drawing.Size(60, 35);
            this.btnhelp.TabIndex = 8;
            this.btnhelp.UseVisualStyleBackColor = true;
            this.btnhelp.Click += new System.EventHandler(this.btnhelp_Click);
            // 
            // txtstatus
            // 
            this.txtstatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtstatus.Location = new System.Drawing.Point(0, 750);
            this.txtstatus.Multiline = true;
            this.txtstatus.Name = "txtstatus";
            this.txtstatus.ReadOnly = true;
            this.txtstatus.Size = new System.Drawing.Size(652, 30);
            this.txtstatus.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(652, 750);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtoutput);
            this.panel1.Controls.Add(this.btnhelp);
            this.panel1.Controls.Add(this.btngo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 640);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(644, 105);
            this.panel1.TabIndex = 0;
            // 
            // txtoutput
            // 
            this.txtoutput.Location = new System.Drawing.Point(117, 14);
            this.txtoutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtoutput.Name = "txtoutput";
            this.txtoutput.ReadOnly = true;
            this.txtoutput.Size = new System.Drawing.Size(565, 26);
            this.txtoutput.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtpath);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cmbfiles);
            this.panel2.Controls.Add(this.btnnew);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btnremove);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(644, 144);
            this.panel2.TabIndex = 1;
            // 
            // txtpath
            // 
            this.txtpath.Location = new System.Drawing.Point(74, 92);
            this.txtpath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtpath.Name = "txtpath";
            this.txtpath.ReadOnly = true;
            this.txtpath.Size = new System.Drawing.Size(553, 26);
            this.txtpath.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Path:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.dcompage);
            this.tabControl1.Controls.Add(this.wcfsvrpage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 159);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(644, 471);
            this.tabControl1.TabIndex = 2;
            // 
            // dcompage
            // 
            this.dcompage.BackColor = System.Drawing.Color.LemonChiffon;
            this.dcompage.Controls.Add(this.chksupportcomplus);
            this.dcompage.Controls.Add(this.btnupdate);
            this.dcompage.Controls.Add(this.chkincoptional);
            this.dcompage.Controls.Add(this.chkruntime35);
            this.dcompage.Controls.Add(this.chkregfiles);
            this.dcompage.Controls.Add(this.txtprogid);
            this.dcompage.Controls.Add(this.label1);
            this.dcompage.Controls.Add(this.btnedit);
            this.dcompage.Controls.Add(this.chkvbs);
            this.dcompage.Controls.Add(this.chkcs);
            this.dcompage.Location = new System.Drawing.Point(4, 29);
            this.dcompage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dcompage.Name = "dcompage";
            this.dcompage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dcompage.Size = new System.Drawing.Size(636, 438);
            this.dcompage.TabIndex = 0;
            this.dcompage.Text = "DCOM Client";
            // 
            // chksupportcomplus
            // 
            this.chksupportcomplus.AutoSize = true;
            this.chksupportcomplus.Location = new System.Drawing.Point(42, 161);
            this.chksupportcomplus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chksupportcomplus.Name = "chksupportcomplus";
            this.chksupportcomplus.Size = new System.Drawing.Size(357, 24);
            this.chksupportcomplus.TabIndex = 15;
            this.chksupportcomplus.Text = "Generate code to support COM+ Applications";
            this.chksupportcomplus.UseVisualStyleBackColor = true;
            // 
            // btnupdate
            // 
            this.btnupdate.Location = new System.Drawing.Point(534, 18);
            this.btnupdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(88, 35);
            this.btnupdate.TabIndex = 5;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // chkincoptional
            // 
            this.chkincoptional.AutoSize = true;
            this.chkincoptional.Location = new System.Drawing.Point(294, 119);
            this.chkincoptional.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkincoptional.Name = "chkincoptional";
            this.chkincoptional.Size = new System.Drawing.Size(228, 24);
            this.chkincoptional.TabIndex = 14;
            this.chkincoptional.Text = "Include Optional Parameter";
            this.chkincoptional.UseVisualStyleBackColor = true;
            // 
            // chkruntime35
            // 
            this.chkruntime35.AutoSize = true;
            this.chkruntime35.Location = new System.Drawing.Point(69, 120);
            this.chkruntime35.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkruntime35.Name = "chkruntime35";
            this.chkruntime35.Size = new System.Drawing.Size(121, 24);
            this.chkruntime35.TabIndex = 13;
            this.chkruntime35.Text = "3.5 Runtime";
            this.chkruntime35.UseVisualStyleBackColor = true;
            // 
            // chkregfiles
            // 
            this.chkregfiles.AutoSize = true;
            this.chkregfiles.Location = new System.Drawing.Point(42, 195);
            this.chkregfiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkregfiles.Name = "chkregfiles";
            this.chkregfiles.Size = new System.Drawing.Size(281, 24);
            this.chkregfiles.TabIndex = 12;
            this.chkregfiles.Text = "Generate Remote Registation files";
            this.chkregfiles.UseVisualStyleBackColor = true;
            // 
            // txtprogid
            // 
            this.txtprogid.Location = new System.Drawing.Point(96, 22);
            this.txtprogid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtprogid.Name = "txtprogid";
            this.txtprogid.Size = new System.Drawing.Size(348, 26);
            this.txtprogid.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prog Id:";
            // 
            // btnedit
            // 
            this.btnedit.Location = new System.Drawing.Point(459, 18);
            this.btnedit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnedit.Name = "btnedit";
            this.btnedit.Size = new System.Drawing.Size(68, 35);
            this.btnedit.TabIndex = 6;
            this.btnedit.Text = "Edit";
            this.btnedit.UseVisualStyleBackColor = true;
            this.btnedit.Click += new System.EventHandler(this.btnedit_Click);
            // 
            // chkvbs
            // 
            this.chkvbs.AutoSize = true;
            this.chkvbs.Location = new System.Drawing.Point(294, 85);
            this.chkvbs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkvbs.Name = "chkvbs";
            this.chkvbs.Size = new System.Drawing.Size(231, 24);
            this.chkvbs.TabIndex = 11;
            this.chkvbs.Text = "Generate VBScript wrapper";
            this.chkvbs.UseVisualStyleBackColor = true;
            // 
            // chkcs
            // 
            this.chkcs.AutoSize = true;
            this.chkcs.Location = new System.Drawing.Point(42, 85);
            this.chkcs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkcs.Name = "chkcs";
            this.chkcs.Size = new System.Drawing.Size(188, 24);
            this.chkcs.TabIndex = 10;
            this.chkcs.Text = "Generate C# wrapper";
            this.chkcs.UseVisualStyleBackColor = true;
            // 
            // wcfsvrpage
            // 
            this.wcfsvrpage.BackColor = System.Drawing.Color.LavenderBlush;
            this.wcfsvrpage.Controls.Add(this.chkwcfruntime35);
            this.wcfsvrpage.Controls.Add(this.txtwcfns);
            this.wcfsvrpage.Controls.Add(this.label3);
            this.wcfsvrpage.Location = new System.Drawing.Point(4, 29);
            this.wcfsvrpage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wcfsvrpage.Name = "wcfsvrpage";
            this.wcfsvrpage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wcfsvrpage.Size = new System.Drawing.Size(636, 438);
            this.wcfsvrpage.TabIndex = 1;
            this.wcfsvrpage.Text = "WCF Server";
            // 
            // chkwcfruntime35
            // 
            this.chkwcfruntime35.AutoSize = true;
            this.chkwcfruntime35.Location = new System.Drawing.Point(240, 69);
            this.chkwcfruntime35.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkwcfruntime35.Name = "chkwcfruntime35";
            this.chkwcfruntime35.Size = new System.Drawing.Size(121, 24);
            this.chkwcfruntime35.TabIndex = 14;
            this.chkwcfruntime35.Text = "3.5 Runtime";
            this.chkwcfruntime35.UseVisualStyleBackColor = true;
            // 
            // txtwcfns
            // 
            this.txtwcfns.Location = new System.Drawing.Point(261, 29);
            this.txtwcfns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtwcfns.Name = "txtwcfns";
            this.txtwcfns.Size = new System.Drawing.Size(148, 26);
            this.txtwcfns.TabIndex = 6;
            this.txtwcfns.Text = "wcfserver";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 34);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Namespace:";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 780);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.txtstatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Text = "Late Binding Client Code Generator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.dcompage.ResumeLayout(false);
            this.dcompage.PerformLayout();
            this.wcfsvrpage.ResumeLayout(false);
            this.wcfsvrpage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbfiles;
        private System.Windows.Forms.Button btnnew;
        private System.Windows.Forms.Button btnremove;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btngo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnhelp;
        private System.Windows.Forms.TextBox txtstatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtoutput;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtpath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage dcompage;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.CheckBox chkincoptional;
        private System.Windows.Forms.CheckBox chkruntime35;
        private System.Windows.Forms.CheckBox chkregfiles;
        private System.Windows.Forms.TextBox txtprogid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnedit;
        private System.Windows.Forms.CheckBox chkvbs;
        private System.Windows.Forms.CheckBox chkcs;
        private System.Windows.Forms.TabPage wcfsvrpage;
        private System.Windows.Forms.CheckBox chkwcfruntime35;
        private System.Windows.Forms.TextBox txtwcfns;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chksupportcomplus;
    }
}

