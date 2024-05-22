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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.createpage = new System.Windows.Forms.TabPage();
            this.btnarchivefile = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.create_xv_updated_btn = new System.Windows.Forms.Button();
            this.create_xv_updated_txt = new System.Windows.Forms.TextBox();
            this.create_output_txt = new System.Windows.Forms.TextBox();
            this.create_xv_original_txt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.create_run_btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.create_xv_original_btn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.applypage = new System.Windows.Forms.TabPage();
            this.btnanalyze = new System.Windows.Forms.Button();
            this.apply_run = new System.Windows.Forms.Button();
            this.apply_output_btn = new System.Windows.Forms.Button();
            this.apply_output_txt = new System.Windows.Forms.TextBox();
            this.apply_xv_txt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.apply_xv_btn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comparepage = new System.Windows.Forms.TabPage();
            this.cmdoptions = new System.Windows.Forms.Button();
            this.compare_right_btn = new System.Windows.Forms.Button();
            this.compare_right_txt = new System.Windows.Forms.TextBox();
            this.compare_left_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btncompareload = new System.Windows.Forms.Button();
            this.compare_left_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.impexp = new System.Windows.Forms.TabPage();
            this.expimp_output_btn = new System.Windows.Forms.Button();
            this.expimp_output_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btextract = new System.Windows.Forms.Button();
            this.btnimporta = new System.Windows.Forms.Button();
            this.btnexport = new System.Windows.Forms.Button();
            this.helppage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.run_output = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.createpage.SuspendLayout();
            this.applypage.SuspendLayout();
            this.comparepage.SuspendLayout();
            this.impexp.SuspendLayout();
            this.helppage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.run_output, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(780, 560);
            this.tableLayoutPanel1.TabIndex = 3;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.createpage);
            this.tabControl.Controls.Add(this.applypage);
            this.tabControl.Controls.Add(this.comparepage);
            this.tabControl.Controls.Add(this.impexp);
            this.tabControl.Controls.Add(this.helppage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(4, 5);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(772, 221);
            this.tabControl.TabIndex = 1;
            // 
            // createpage
            // 
            this.createpage.Controls.Add(this.btnarchivefile);
            this.createpage.Controls.Add(this.btnAdd);
            this.createpage.Controls.Add(this.create_xv_updated_btn);
            this.createpage.Controls.Add(this.create_xv_updated_txt);
            this.createpage.Controls.Add(this.create_output_txt);
            this.createpage.Controls.Add(this.create_xv_original_txt);
            this.createpage.Controls.Add(this.label7);
            this.createpage.Controls.Add(this.create_run_btn);
            this.createpage.Controls.Add(this.label5);
            this.createpage.Controls.Add(this.create_xv_original_btn);
            this.createpage.Controls.Add(this.label6);
            this.createpage.Location = new System.Drawing.Point(4, 29);
            this.createpage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.createpage.Name = "createpage";
            this.createpage.Size = new System.Drawing.Size(764, 188);
            this.createpage.TabIndex = 2;
            this.createpage.Text = "Create Patch";
            this.createpage.UseVisualStyleBackColor = true;
            // 
            // btnarchivefile
            // 
            this.btnarchivefile.Location = new System.Drawing.Point(512, 114);
            this.btnarchivefile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnarchivefile.Name = "btnarchivefile";
            this.btnarchivefile.Size = new System.Drawing.Size(40, 35);
            this.btnarchivefile.TabIndex = 17;
            this.btnarchivefile.Text = "...";
            this.btnarchivefile.UseVisualStyleBackColor = true;
            this.btnarchivefile.Click += new System.EventHandler(this.btnarchivefile_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(579, 26);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 35);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Options";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // create_xv_updated_btn
            // 
            this.create_xv_updated_btn.Location = new System.Drawing.Point(512, 69);
            this.create_xv_updated_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_xv_updated_btn.Name = "create_xv_updated_btn";
            this.create_xv_updated_btn.Size = new System.Drawing.Size(40, 35);
            this.create_xv_updated_btn.TabIndex = 15;
            this.create_xv_updated_btn.Text = "...";
            this.create_xv_updated_btn.UseVisualStyleBackColor = true;
            this.create_xv_updated_btn.Click += new System.EventHandler(this.create_xv_updated_btn_Click);
            // 
            // create_xv_updated_txt
            // 
            this.create_xv_updated_txt.Location = new System.Drawing.Point(188, 68);
            this.create_xv_updated_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_xv_updated_txt.Name = "create_xv_updated_txt";
            this.create_xv_updated_txt.ReadOnly = true;
            this.create_xv_updated_txt.Size = new System.Drawing.Size(313, 26);
            this.create_xv_updated_txt.TabIndex = 14;
            // 
            // create_output_txt
            // 
            this.create_output_txt.Location = new System.Drawing.Point(188, 114);
            this.create_output_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_output_txt.Name = "create_output_txt";
            this.create_output_txt.ReadOnly = true;
            this.create_output_txt.Size = new System.Drawing.Size(313, 26);
            this.create_output_txt.TabIndex = 11;
            // 
            // create_xv_original_txt
            // 
            this.create_xv_original_txt.Location = new System.Drawing.Point(188, 22);
            this.create_xv_original_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_xv_original_txt.Name = "create_xv_original_txt";
            this.create_xv_original_txt.ReadOnly = true;
            this.create_xv_original_txt.Size = new System.Drawing.Size(313, 26);
            this.create_xv_original_txt.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 68);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "Updated Folder:";
            this.label7.DoubleClick += new System.EventHandler(this.label7_DoubleClick);
            // 
            // create_run_btn
            // 
            this.create_run_btn.Location = new System.Drawing.Point(579, 71);
            this.create_run_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_run_btn.Name = "create_run_btn";
            this.create_run_btn.Size = new System.Drawing.Size(90, 35);
            this.create_run_btn.TabIndex = 8;
            this.create_run_btn.Text = "Create";
            this.create_run_btn.UseVisualStyleBackColor = true;
            this.create_run_btn.Click += new System.EventHandler(this.create_run_btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 118);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Patch Archive:";
            this.label5.DoubleClick += new System.EventHandler(this.label5_DoubleClick);
            // 
            // create_xv_original_btn
            // 
            this.create_xv_original_btn.Location = new System.Drawing.Point(512, 26);
            this.create_xv_original_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.create_xv_original_btn.Name = "create_xv_original_btn";
            this.create_xv_original_btn.Size = new System.Drawing.Size(40, 35);
            this.create_xv_original_btn.TabIndex = 9;
            this.create_xv_original_btn.Text = "...";
            this.create_xv_original_btn.UseVisualStyleBackColor = true;
            this.create_xv_original_btn.Click += new System.EventHandler(this.create_xv_original_btn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 26);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "Source Folder:";
            this.label6.DoubleClick += new System.EventHandler(this.label6_DoubleClick);
            // 
            // applypage
            // 
            this.applypage.Controls.Add(this.btnanalyze);
            this.applypage.Controls.Add(this.apply_run);
            this.applypage.Controls.Add(this.apply_output_btn);
            this.applypage.Controls.Add(this.apply_output_txt);
            this.applypage.Controls.Add(this.apply_xv_txt);
            this.applypage.Controls.Add(this.label9);
            this.applypage.Controls.Add(this.apply_xv_btn);
            this.applypage.Controls.Add(this.label10);
            this.applypage.Location = new System.Drawing.Point(4, 29);
            this.applypage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.applypage.Name = "applypage";
            this.applypage.Size = new System.Drawing.Size(764, 188);
            this.applypage.TabIndex = 3;
            this.applypage.Text = "Apply Patch";
            this.applypage.UseVisualStyleBackColor = true;
            // 
            // btnanalyze
            // 
            this.btnanalyze.Location = new System.Drawing.Point(574, 23);
            this.btnanalyze.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnanalyze.Name = "btnanalyze";
            this.btnanalyze.Size = new System.Drawing.Size(90, 35);
            this.btnanalyze.TabIndex = 23;
            this.btnanalyze.Text = "Preview";
            this.btnanalyze.UseVisualStyleBackColor = true;
            this.btnanalyze.Click += new System.EventHandler(this.btnanalyze_Click);
            // 
            // apply_run
            // 
            this.apply_run.Location = new System.Drawing.Point(574, 68);
            this.apply_run.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apply_run.Name = "apply_run";
            this.apply_run.Size = new System.Drawing.Size(90, 35);
            this.apply_run.TabIndex = 18;
            this.apply_run.Text = "Apply";
            this.apply_run.UseVisualStyleBackColor = true;
            this.apply_run.Click += new System.EventHandler(this.apply_run_Click);
            // 
            // apply_output_btn
            // 
            this.apply_output_btn.Location = new System.Drawing.Point(518, 68);
            this.apply_output_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apply_output_btn.Name = "apply_output_btn";
            this.apply_output_btn.Size = new System.Drawing.Size(40, 35);
            this.apply_output_btn.TabIndex = 22;
            this.apply_output_btn.Text = "...";
            this.apply_output_btn.UseVisualStyleBackColor = true;
            this.apply_output_btn.Click += new System.EventHandler(this.apply_output_btn_Click);
            // 
            // apply_output_txt
            // 
            this.apply_output_txt.Location = new System.Drawing.Point(194, 68);
            this.apply_output_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apply_output_txt.Name = "apply_output_txt";
            this.apply_output_txt.ReadOnly = true;
            this.apply_output_txt.Size = new System.Drawing.Size(313, 26);
            this.apply_output_txt.TabIndex = 21;
            // 
            // apply_xv_txt
            // 
            this.apply_xv_txt.Location = new System.Drawing.Point(194, 28);
            this.apply_xv_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apply_xv_txt.Name = "apply_xv_txt";
            this.apply_xv_txt.ReadOnly = true;
            this.apply_xv_txt.Size = new System.Drawing.Size(313, 26);
            this.apply_xv_txt.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 72);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 20);
            this.label9.TabIndex = 20;
            this.label9.Text = "Patch Archive:";
            this.label9.DoubleClick += new System.EventHandler(this.label9_DoubleClick);
            // 
            // apply_xv_btn
            // 
            this.apply_xv_btn.Location = new System.Drawing.Point(518, 23);
            this.apply_xv_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.apply_xv_btn.Name = "apply_xv_btn";
            this.apply_xv_btn.Size = new System.Drawing.Size(40, 35);
            this.apply_xv_btn.TabIndex = 19;
            this.apply_xv_btn.Text = "...";
            this.apply_xv_btn.UseVisualStyleBackColor = true;
            this.apply_xv_btn.Click += new System.EventHandler(this.apply_xv_btn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 32);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 20);
            this.label10.TabIndex = 16;
            this.label10.Text = "Target Folder:";
            this.label10.DoubleClick += new System.EventHandler(this.label10_DoubleClick);
            // 
            // comparepage
            // 
            this.comparepage.Controls.Add(this.cmdoptions);
            this.comparepage.Controls.Add(this.compare_right_btn);
            this.comparepage.Controls.Add(this.compare_right_txt);
            this.comparepage.Controls.Add(this.compare_left_txt);
            this.comparepage.Controls.Add(this.label1);
            this.comparepage.Controls.Add(this.btncompareload);
            this.comparepage.Controls.Add(this.compare_left_btn);
            this.comparepage.Controls.Add(this.label3);
            this.comparepage.Location = new System.Drawing.Point(4, 29);
            this.comparepage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comparepage.Name = "comparepage";
            this.comparepage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comparepage.Size = new System.Drawing.Size(764, 188);
            this.comparepage.TabIndex = 4;
            this.comparepage.Text = "Compare Folders";
            this.comparepage.UseVisualStyleBackColor = true;
            // 
            // cmdoptions
            // 
            this.cmdoptions.Location = new System.Drawing.Point(588, 22);
            this.cmdoptions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdoptions.Name = "cmdoptions";
            this.cmdoptions.Size = new System.Drawing.Size(90, 35);
            this.cmdoptions.TabIndex = 26;
            this.cmdoptions.Text = "Options";
            this.cmdoptions.UseVisualStyleBackColor = true;
            this.cmdoptions.Click += new System.EventHandler(this.cmdoptions_Click);
            // 
            // compare_right_btn
            // 
            this.compare_right_btn.Location = new System.Drawing.Point(524, 66);
            this.compare_right_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compare_right_btn.Name = "compare_right_btn";
            this.compare_right_btn.Size = new System.Drawing.Size(40, 35);
            this.compare_right_btn.TabIndex = 25;
            this.compare_right_btn.Text = "...";
            this.compare_right_btn.UseVisualStyleBackColor = true;
            this.compare_right_btn.Click += new System.EventHandler(this.compare_right_btn_Click);
            // 
            // compare_right_txt
            // 
            this.compare_right_txt.AllowDrop = true;
            this.compare_right_txt.Location = new System.Drawing.Point(200, 65);
            this.compare_right_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compare_right_txt.Name = "compare_right_txt";
            this.compare_right_txt.ReadOnly = true;
            this.compare_right_txt.Size = new System.Drawing.Size(313, 26);
            this.compare_right_txt.TabIndex = 24;
            this.compare_right_txt.DragDrop += new System.Windows.Forms.DragEventHandler(this.compare_right_txt_DragDrop);
            this.compare_right_txt.DragOver += new System.Windows.Forms.DragEventHandler(this.compare_right_txt_DragOver);
            // 
            // compare_left_txt
            // 
            this.compare_left_txt.AllowDrop = true;
            this.compare_left_txt.Location = new System.Drawing.Point(200, 18);
            this.compare_left_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compare_left_txt.Name = "compare_left_txt";
            this.compare_left_txt.ReadOnly = true;
            this.compare_left_txt.Size = new System.Drawing.Size(313, 26);
            this.compare_left_txt.TabIndex = 17;
            this.compare_left_txt.DragDrop += new System.Windows.Forms.DragEventHandler(this.compare_left_txt_DragDrop);
            this.compare_left_txt.DragOver += new System.Windows.Forms.DragEventHandler(this.compare_left_txt_DragOver);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 23;
            this.label1.Text = "Righht Folder:";
            this.label1.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
            // 
            // btncompareload
            // 
            this.btncompareload.Location = new System.Drawing.Point(588, 66);
            this.btncompareload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btncompareload.Name = "btncompareload";
            this.btncompareload.Size = new System.Drawing.Size(90, 35);
            this.btncompareload.TabIndex = 18;
            this.btncompareload.Text = "Load";
            this.btncompareload.UseVisualStyleBackColor = true;
            this.btncompareload.Click += new System.EventHandler(this.btncompareload_Click);
            // 
            // compare_left_btn
            // 
            this.compare_left_btn.Location = new System.Drawing.Point(524, 23);
            this.compare_left_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compare_left_btn.Name = "compare_left_btn";
            this.compare_left_btn.Size = new System.Drawing.Size(40, 35);
            this.compare_left_btn.TabIndex = 19;
            this.compare_left_btn.Text = "...";
            this.compare_left_btn.UseVisualStyleBackColor = true;
            this.compare_left_btn.Click += new System.EventHandler(this.compare_left_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Left Folder:";
            this.label3.DoubleClick += new System.EventHandler(this.label3_DoubleClick);
            // 
            // impexp
            // 
            this.impexp.Controls.Add(this.expimp_output_btn);
            this.impexp.Controls.Add(this.expimp_output_txt);
            this.impexp.Controls.Add(this.label2);
            this.impexp.Controls.Add(this.btextract);
            this.impexp.Controls.Add(this.btnimporta);
            this.impexp.Controls.Add(this.btnexport);
            this.impexp.Location = new System.Drawing.Point(4, 29);
            this.impexp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.impexp.Name = "impexp";
            this.impexp.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.impexp.Size = new System.Drawing.Size(764, 188);
            this.impexp.TabIndex = 5;
            this.impexp.Text = "Export/Import/Extract";
            this.impexp.UseVisualStyleBackColor = true;
            // 
            // expimp_output_btn
            // 
            this.expimp_output_btn.Location = new System.Drawing.Point(506, 75);
            this.expimp_output_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.expimp_output_btn.Name = "expimp_output_btn";
            this.expimp_output_btn.Size = new System.Drawing.Size(40, 35);
            this.expimp_output_btn.TabIndex = 31;
            this.expimp_output_btn.Text = "...";
            this.expimp_output_btn.UseVisualStyleBackColor = true;
            this.expimp_output_btn.Click += new System.EventHandler(this.expimp_output_btn_Click);
            // 
            // expimp_output_txt
            // 
            this.expimp_output_txt.Location = new System.Drawing.Point(182, 75);
            this.expimp_output_txt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.expimp_output_txt.Name = "expimp_output_txt";
            this.expimp_output_txt.ReadOnly = true;
            this.expimp_output_txt.Size = new System.Drawing.Size(313, 26);
            this.expimp_output_txt.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 20);
            this.label2.TabIndex = 29;
            this.label2.Text = "Patch Archive:";
            // 
            // btextract
            // 
            this.btextract.Location = new System.Drawing.Point(570, 117);
            this.btextract.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btextract.Name = "btextract";
            this.btextract.Size = new System.Drawing.Size(90, 35);
            this.btextract.TabIndex = 28;
            this.btextract.Text = "Extract";
            this.btextract.UseVisualStyleBackColor = true;
            this.btextract.Click += new System.EventHandler(this.btextract_Click);
            // 
            // btnimporta
            // 
            this.btnimporta.Location = new System.Drawing.Point(570, 72);
            this.btnimporta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnimporta.Name = "btnimporta";
            this.btnimporta.Size = new System.Drawing.Size(90, 35);
            this.btnimporta.TabIndex = 25;
            this.btnimporta.Text = "Import";
            this.btnimporta.UseVisualStyleBackColor = true;
            this.btnimporta.Click += new System.EventHandler(this.btnimporta_Click);
            // 
            // btnexport
            // 
            this.btnexport.Location = new System.Drawing.Point(570, 28);
            this.btnexport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnexport.Name = "btnexport";
            this.btnexport.Size = new System.Drawing.Size(90, 35);
            this.btnexport.TabIndex = 19;
            this.btnexport.Text = "Export";
            this.btnexport.UseVisualStyleBackColor = true;
            this.btnexport.Click += new System.EventHandler(this.btnexport_Click);
            // 
            // helppage
            // 
            this.helppage.Controls.Add(this.panel1);
            this.helppage.Location = new System.Drawing.Point(4, 29);
            this.helppage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.helppage.Name = "helppage";
            this.helppage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.helppage.Size = new System.Drawing.Size(764, 188);
            this.helppage.TabIndex = 6;
            this.helppage.Text = "About";
            this.helppage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Snow;
            this.panel1.Controls.Add(this.tableLayoutPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(756, 178);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.linkLabel1, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBox1, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.20354F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.20354F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.38939F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.20354F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(756, 178);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(5, 6);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logoPictureBox.Name = "logoPictureBox";
            this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 4);
            this.logoPictureBox.Size = new System.Drawing.Size(240, 166);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            // 
            // labelProductName
            // 
            this.labelProductName.BackColor = System.Drawing.Color.FloralWhite;
            this.labelProductName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductName.Location = new System.Drawing.Point(259, 1);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(9, 0, 4, 0);
            this.labelProductName.MaximumSize = new System.Drawing.Size(0, 26);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(492, 26);
            this.labelProductName.TabIndex = 19;
            this.labelProductName.Text = "Portable Patch Tool   1.0     © 2022";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(259, 64);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(9, 5, 4, 5);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(492, 78);
            this.textBoxDescription.TabIndex = 23;
            this.textBoxDescription.TabStop = false;
            this.textBoxDescription.Text = "Create and deploy patches containing binary files. \r\nView folder differences hier" +
    "archically and recursively. \r\nMove or delete files. Much more...";
            this.textBoxDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxDescription.TextChanged += new System.EventHandler(this.textBoxDescription_TextChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Orange;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel1.Location = new System.Drawing.Point(254, 148);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(497, 29);
            this.linkLabel1.TabIndex = 24;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Help!";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(254, 35);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(497, 26);
            this.textBox1.TabIndex = 25;
            this.textBox1.Text = "Vedavyas Rao (rvvyas@gmail.com)";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(4, 236);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(772, 36);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            // 
            // run_output
            // 
            this.run_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.run_output.Location = new System.Drawing.Point(4, 282);
            this.run_output.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.run_output.Multiline = true;
            this.run_output.Name = "run_output";
            this.run_output.ReadOnly = true;
            this.run_output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.run_output.Size = new System.Drawing.Size(772, 273);
            this.run_output.TabIndex = 4;
            this.run_output.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 560);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Portable Patch Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.createpage.ResumeLayout(false);
            this.createpage.PerformLayout();
            this.applypage.ResumeLayout(false);
            this.applypage.PerformLayout();
            this.comparepage.ResumeLayout(false);
            this.comparepage.PerformLayout();
            this.impexp.ResumeLayout(false);
            this.impexp.PerformLayout();
            this.helppage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage createpage;
        private System.Windows.Forms.Button btnarchivefile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button create_xv_updated_btn;
        private System.Windows.Forms.TextBox create_xv_updated_txt;
        private System.Windows.Forms.TextBox create_output_txt;
        private System.Windows.Forms.TextBox create_xv_original_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button create_run_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button create_xv_original_btn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage applypage;
        private System.Windows.Forms.Button btnanalyze;
        private System.Windows.Forms.Button apply_run;
        private System.Windows.Forms.Button apply_output_btn;
        private System.Windows.Forms.TextBox apply_output_txt;
        private System.Windows.Forms.TextBox apply_xv_txt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button apply_xv_btn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage comparepage;
        private System.Windows.Forms.Button cmdoptions;
        private System.Windows.Forms.Button compare_right_btn;
        private System.Windows.Forms.TextBox compare_right_txt;
        private System.Windows.Forms.TextBox compare_left_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncompareload;
        private System.Windows.Forms.Button compare_left_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox run_output;
        private System.Windows.Forms.TabPage impexp;
        private System.Windows.Forms.Button btextract;
        private System.Windows.Forms.Button btnimporta;
        private System.Windows.Forms.Button btnexport;
        private System.Windows.Forms.Button expimp_output_btn;
        private System.Windows.Forms.TextBox expimp_output_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage helppage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox textBox1;

    }
}

