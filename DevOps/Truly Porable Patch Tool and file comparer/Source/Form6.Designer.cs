namespace MigrationHelper
{
    partial class Form6
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.previewlist = new System.Windows.Forms.ListView();
            this.colname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colextn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colmerge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coldir = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkincsf = new System.Windows.Forms.CheckBox();
            this.btnoptions = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnrightvopy = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.txtsort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.previewlist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(591, 380);
            this.panel2.TabIndex = 4;
            // 
            // previewlist
            // 
            this.previewlist.CheckBoxes = true;
            this.previewlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colname,
            this.colextn,
            this.colmerge,
            this.coldir});
            this.previewlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewlist.FullRowSelect = true;
            this.previewlist.GridLines = true;
            this.previewlist.Location = new System.Drawing.Point(0, 0);
            this.previewlist.MultiSelect = false;
            this.previewlist.Name = "previewlist";
            this.previewlist.Size = new System.Drawing.Size(591, 380);
            this.previewlist.TabIndex = 2;
            this.previewlist.UseCompatibleStateImageBehavior = false;
            this.previewlist.View = System.Windows.Forms.View.Details;
            this.previewlist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.previewlist_ColumnClick);
            this.previewlist.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            this.previewlist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.previewlist_MouseDoubleClick);
            // 
            // colname
            // 
            this.colname.Text = "File Name";
            this.colname.Width = 150;
            // 
            // colextn
            // 
            this.colextn.Text = "Extension";
            // 
            // colmerge
            // 
            this.colmerge.Text = "Remark";
            // 
            // coldir
            // 
            this.coldir.Text = "Directory";
            this.coldir.Width = 300;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(597, 511);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.chkincsf);
            this.panel1.Controls.Add(this.btnoptions);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.btnrightvopy);
            this.panel1.Controls.Add(this.btnclear);
            this.panel1.Controls.Add(this.txtsort);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(591, 119);
            this.panel1.TabIndex = 4;
            // 
            // chkincsf
            // 
            this.chkincsf.AutoSize = true;
            this.chkincsf.Location = new System.Drawing.Point(387, 74);
            this.chkincsf.Name = "chkincsf";
            this.chkincsf.Size = new System.Drawing.Size(119, 17);
            this.chkincsf.TabIndex = 21;
            this.chkincsf.Text = "Include Source files";
            this.chkincsf.UseVisualStyleBackColor = true;
            // 
            // btnoptions
            // 
            this.btnoptions.Location = new System.Drawing.Point(16, 41);
            this.btnoptions.Name = "btnoptions";
            this.btnoptions.Size = new System.Drawing.Size(75, 23);
            this.btnoptions.TabIndex = 20;
            this.btnoptions.Text = "Options";
            this.btnoptions.UseVisualStyleBackColor = true;
            this.btnoptions.Click += new System.EventHandler(this.btnoptions_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(387, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(150, 10);
            this.progressBar1.TabIndex = 12;
            // 
            // btnrightvopy
            // 
            this.btnrightvopy.Location = new System.Drawing.Point(432, 45);
            this.btnrightvopy.Name = "btnrightvopy";
            this.btnrightvopy.Size = new System.Drawing.Size(75, 23);
            this.btnrightvopy.TabIndex = 11;
            this.btnrightvopy.Text = "Create";
            this.btnrightvopy.UseVisualStyleBackColor = true;
            this.btnrightvopy.Click += new System.EventHandler(this.btnright_Click);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(216, 10);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(75, 23);
            this.btnclear.TabIndex = 3;
            this.btnclear.Text = "Clear";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // txtsort
            // 
            this.txtsort.Location = new System.Drawing.Point(91, 10);
            this.txtsort.Name = "txtsort";
            this.txtsort.ReadOnly = true;
            this.txtsort.Size = new System.Drawing.Size(110, 20);
            this.txtsort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sort Columns:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(97, 41);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "Selected";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 83);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(106, 17);
            this.checkBox1.TabIndex = 27;
            this.checkBox1.Text = "Toggle Select All";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 511);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form6";
            this.Text = "Create Patch";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView previewlist;
        private System.Windows.Forms.ColumnHeader colname;
        private System.Windows.Forms.ColumnHeader colextn;
        private System.Windows.Forms.ColumnHeader colmerge;
        private System.Windows.Forms.ColumnHeader coldir;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnrightvopy;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.TextBox txtsort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnoptions;
        private System.Windows.Forms.CheckBox chkincsf;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}