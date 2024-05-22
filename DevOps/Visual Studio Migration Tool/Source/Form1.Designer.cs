namespace MigrationHelper
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.listpage = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.list_run = new System.Windows.Forms.Button();
            this.list_output_btn = new System.Windows.Forms.Button();
            this.list_output_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.list_xv_btn = new System.Windows.Forms.Button();
            this.list_xv_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.upgradevs = new System.Windows.Forms.TabPage();
            this.up_rad_prj = new System.Windows.Forms.RadioButton();
            this.up_rad_sln = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.vs_run_btn = new System.Windows.Forms.Button();
            this.vs_output_btn = new System.Windows.Forms.Button();
            this.vs_output_txt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.vs_xv_btn = new System.Windows.Forms.Button();
            this.vs_xv_txt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.updatevs = new System.Windows.Forms.TabPage();
            this.up_run = new System.Windows.Forms.Button();
            this.up_output_btn = new System.Windows.Forms.Button();
            this.up_output_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.up_xv_btn = new System.Windows.Forms.Button();
            this.up_xv_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.run_output = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.listpage.SuspendLayout();
            this.upgradevs.SuspendLayout();
            this.updatevs.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.listpage);
            this.tabControl1.Controls.Add(this.upgradevs);
            this.tabControl1.Controls.Add(this.updatevs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(468, 135);
            this.tabControl1.TabIndex = 0;
            // 
            // listpage
            // 
            this.listpage.Controls.Add(this.button2);
            this.listpage.Controls.Add(this.button1);
            this.listpage.Controls.Add(this.list_run);
            this.listpage.Controls.Add(this.list_output_btn);
            this.listpage.Controls.Add(this.list_output_txt);
            this.listpage.Controls.Add(this.label2);
            this.listpage.Controls.Add(this.list_xv_btn);
            this.listpage.Controls.Add(this.list_xv_txt);
            this.listpage.Controls.Add(this.label1);
            this.listpage.Location = new System.Drawing.Point(4, 22);
            this.listpage.Name = "listpage";
            this.listpage.Padding = new System.Windows.Forms.Padding(3);
            this.listpage.Size = new System.Drawing.Size(460, 109);
            this.listpage.TabIndex = 0;
            this.listpage.Text = "List";
            this.listpage.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(192, 79);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Dependencies";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(107, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Runtimes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // list_run
            // 
            this.list_run.Location = new System.Drawing.Point(11, 79);
            this.list_run.Name = "list_run";
            this.list_run.Size = new System.Drawing.Size(71, 23);
            this.list_run.TabIndex = 2;
            this.list_run.Text = "List";
            this.list_run.UseVisualStyleBackColor = true;
            this.list_run.Click += new System.EventHandler(this.list_run_Click);
            // 
            // list_output_btn
            // 
            this.list_output_btn.Location = new System.Drawing.Point(412, 46);
            this.list_output_btn.Name = "list_output_btn";
            this.list_output_btn.Size = new System.Drawing.Size(27, 23);
            this.list_output_btn.TabIndex = 5;
            this.list_output_btn.Text = "...";
            this.list_output_btn.UseVisualStyleBackColor = true;
            this.list_output_btn.Click += new System.EventHandler(this.list_output_btn_Click);
            // 
            // list_output_txt
            // 
            this.list_output_txt.Location = new System.Drawing.Point(104, 46);
            this.list_output_txt.Name = "list_output_txt";
            this.list_output_txt.ReadOnly = true;
            this.list_output_txt.Size = new System.Drawing.Size(302, 20);
            this.list_output_txt.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Folder:";
            // 
            // list_xv_btn
            // 
            this.list_xv_btn.Location = new System.Drawing.Point(412, 17);
            this.list_xv_btn.Name = "list_xv_btn";
            this.list_xv_btn.Size = new System.Drawing.Size(27, 23);
            this.list_xv_btn.TabIndex = 2;
            this.list_xv_btn.Text = "...";
            this.list_xv_btn.UseVisualStyleBackColor = true;
            this.list_xv_btn.Click += new System.EventHandler(this.list_xv_btn_Click);
            // 
            // list_xv_txt
            // 
            this.list_xv_txt.Location = new System.Drawing.Point(104, 20);
            this.list_xv_txt.Name = "list_xv_txt";
            this.list_xv_txt.ReadOnly = true;
            this.list_xv_txt.Size = new System.Drawing.Size(302, 20);
            this.list_xv_txt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Folder:";
            // 
            // upgradevs
            // 
            this.upgradevs.Controls.Add(this.up_rad_prj);
            this.upgradevs.Controls.Add(this.up_rad_sln);
            this.upgradevs.Controls.Add(this.label6);
            this.upgradevs.Controls.Add(this.label5);
            this.upgradevs.Controls.Add(this.vs_run_btn);
            this.upgradevs.Controls.Add(this.vs_output_btn);
            this.upgradevs.Controls.Add(this.vs_output_txt);
            this.upgradevs.Controls.Add(this.label8);
            this.upgradevs.Controls.Add(this.vs_xv_btn);
            this.upgradevs.Controls.Add(this.vs_xv_txt);
            this.upgradevs.Controls.Add(this.label11);
            this.upgradevs.Location = new System.Drawing.Point(4, 22);
            this.upgradevs.Name = "upgradevs";
            this.upgradevs.Padding = new System.Windows.Forms.Padding(3);
            this.upgradevs.Size = new System.Drawing.Size(460, 109);
            this.upgradevs.TabIndex = 4;
            this.upgradevs.Text = "VS Upgrade";
            this.upgradevs.UseVisualStyleBackColor = true;
            // 
            // up_rad_prj
            // 
            this.up_rad_prj.AutoSize = true;
            this.up_rad_prj.Location = new System.Drawing.Point(209, 68);
            this.up_rad_prj.Name = "up_rad_prj";
            this.up_rad_prj.Size = new System.Drawing.Size(63, 17);
            this.up_rad_prj.TabIndex = 23;
            this.up_rad_prj.Text = "Projects";
            this.up_rad_prj.UseVisualStyleBackColor = true;
            // 
            // up_rad_sln
            // 
            this.up_rad_sln.AutoSize = true;
            this.up_rad_sln.Checked = true;
            this.up_rad_sln.Location = new System.Drawing.Point(103, 68);
            this.up_rad_sln.Name = "up_rad_sln";
            this.up_rad_sln.Size = new System.Drawing.Size(68, 17);
            this.up_rad_sln.TabIndex = 22;
            this.up_rad_sln.TabStop = true;
            this.up_rad_sln.Text = "Solutions";
            this.up_rad_sln.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Output Folder:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Source Folder:";
            // 
            // vs_run_btn
            // 
            this.vs_run_btn.Location = new System.Drawing.Point(384, 31);
            this.vs_run_btn.Name = "vs_run_btn";
            this.vs_run_btn.Size = new System.Drawing.Size(60, 23);
            this.vs_run_btn.TabIndex = 15;
            this.vs_run_btn.Text = "Run";
            this.vs_run_btn.UseVisualStyleBackColor = true;
            this.vs_run_btn.Click += new System.EventHandler(this.vs_run_btn_Click);
            // 
            // vs_output_btn
            // 
            this.vs_output_btn.Location = new System.Drawing.Point(340, 41);
            this.vs_output_btn.Name = "vs_output_btn";
            this.vs_output_btn.Size = new System.Drawing.Size(27, 23);
            this.vs_output_btn.TabIndex = 19;
            this.vs_output_btn.Text = "...";
            this.vs_output_btn.UseVisualStyleBackColor = true;
            this.vs_output_btn.Click += new System.EventHandler(this.vs_output_btn_Click);
            // 
            // vs_output_txt
            // 
            this.vs_output_txt.Location = new System.Drawing.Point(103, 41);
            this.vs_output_txt.Name = "vs_output_txt";
            this.vs_output_txt.ReadOnly = true;
            this.vs_output_txt.Size = new System.Drawing.Size(230, 20);
            this.vs_output_txt.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 17;
            // 
            // vs_xv_btn
            // 
            this.vs_xv_btn.Location = new System.Drawing.Point(340, 10);
            this.vs_xv_btn.Name = "vs_xv_btn";
            this.vs_xv_btn.Size = new System.Drawing.Size(27, 23);
            this.vs_xv_btn.TabIndex = 16;
            this.vs_xv_btn.Text = "...";
            this.vs_xv_btn.UseVisualStyleBackColor = true;
            this.vs_xv_btn.Click += new System.EventHandler(this.vs_xv_btn_Click);
            // 
            // vs_xv_txt
            // 
            this.vs_xv_txt.Location = new System.Drawing.Point(103, 15);
            this.vs_xv_txt.Name = "vs_xv_txt";
            this.vs_xv_txt.ReadOnly = true;
            this.vs_xv_txt.Size = new System.Drawing.Size(230, 20);
            this.vs_xv_txt.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 13);
            this.label11.TabIndex = 13;
            // 
            // updatevs
            // 
            this.updatevs.Controls.Add(this.up_run);
            this.updatevs.Controls.Add(this.up_output_btn);
            this.updatevs.Controls.Add(this.up_output_txt);
            this.updatevs.Controls.Add(this.label3);
            this.updatevs.Controls.Add(this.up_xv_btn);
            this.updatevs.Controls.Add(this.up_xv_txt);
            this.updatevs.Controls.Add(this.label4);
            this.updatevs.Location = new System.Drawing.Point(4, 22);
            this.updatevs.Name = "updatevs";
            this.updatevs.Padding = new System.Windows.Forms.Padding(3);
            this.updatevs.Size = new System.Drawing.Size(460, 109);
            this.updatevs.TabIndex = 1;
            this.updatevs.Text = "VS Update";
            this.updatevs.UseVisualStyleBackColor = true;
            // 
            // up_run
            // 
            this.up_run.Location = new System.Drawing.Point(385, 39);
            this.up_run.Name = "up_run";
            this.up_run.Size = new System.Drawing.Size(60, 23);
            this.up_run.TabIndex = 8;
            this.up_run.Text = "Run";
            this.up_run.UseVisualStyleBackColor = true;
            this.up_run.Click += new System.EventHandler(this.up_run_Click);
            // 
            // up_output_btn
            // 
            this.up_output_btn.Location = new System.Drawing.Point(341, 49);
            this.up_output_btn.Name = "up_output_btn";
            this.up_output_btn.Size = new System.Drawing.Size(27, 23);
            this.up_output_btn.TabIndex = 12;
            this.up_output_btn.Text = "...";
            this.up_output_btn.UseVisualStyleBackColor = true;
            this.up_output_btn.Click += new System.EventHandler(this.up_output_btn_Click);
            // 
            // up_output_txt
            // 
            this.up_output_txt.Location = new System.Drawing.Point(104, 49);
            this.up_output_txt.Name = "up_output_txt";
            this.up_output_txt.ReadOnly = true;
            this.up_output_txt.Size = new System.Drawing.Size(230, 20);
            this.up_output_txt.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Output Folder:";
            // 
            // up_xv_btn
            // 
            this.up_xv_btn.Location = new System.Drawing.Point(341, 18);
            this.up_xv_btn.Name = "up_xv_btn";
            this.up_xv_btn.Size = new System.Drawing.Size(27, 23);
            this.up_xv_btn.TabIndex = 9;
            this.up_xv_btn.Text = "...";
            this.up_xv_btn.UseVisualStyleBackColor = true;
            this.up_xv_btn.Click += new System.EventHandler(this.up_xv_btn_Click);
            // 
            // up_xv_txt
            // 
            this.up_xv_txt.Location = new System.Drawing.Point(104, 23);
            this.up_xv_txt.Name = "up_xv_txt";
            this.up_xv_txt.ReadOnly = true;
            this.up_xv_txt.Size = new System.Drawing.Size(230, 20);
            this.up_xv_txt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Source Folder:";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(459, 24);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 2;
            // 
            // run_output
            // 
            this.run_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.run_output.Location = new System.Drawing.Point(3, 33);
            this.run_output.Multiline = true;
            this.run_output.Name = "run_output";
            this.run_output.ReadOnly = true;
            this.run_output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.run_output.Size = new System.Drawing.Size(459, 237);
            this.run_output.TabIndex = 1;
            this.run_output.WordWrap = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 135);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 273);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.run_output, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 135);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(465, 273);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 408);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Migration Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.listpage.ResumeLayout(false);
            this.listpage.PerformLayout();
            this.upgradevs.ResumeLayout(false);
            this.upgradevs.PerformLayout();
            this.updatevs.ResumeLayout(false);
            this.updatevs.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage listpage;
        private System.Windows.Forms.TabPage updatevs;
        private System.Windows.Forms.Button list_output_btn;
        private System.Windows.Forms.TextBox list_output_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button list_xv_btn;
        private System.Windows.Forms.TextBox list_xv_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button list_run;
        private System.Windows.Forms.Button up_run;
        private System.Windows.Forms.Button up_output_btn;
        private System.Windows.Forms.TextBox up_output_txt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button up_xv_btn;
        private System.Windows.Forms.TextBox up_xv_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage upgradevs;
        private System.Windows.Forms.Button vs_run_btn;
        private System.Windows.Forms.Button vs_output_btn;
        private System.Windows.Forms.TextBox vs_output_txt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button vs_xv_btn;
        private System.Windows.Forms.TextBox vs_xv_txt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton up_rad_prj;
        private System.Windows.Forms.RadioButton up_rad_sln;
        private System.Windows.Forms.TextBox run_output;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

