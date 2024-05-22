namespace MigrationHelper
{
    partial class Form5
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
            this.btnTV = new System.Windows.Forms.Button();
            this.chkselonly = new System.Windows.Forms.CheckBox();
            this.cmdsave = new System.Windows.Forms.Button();
            this.btnoptions = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.chkReadonly = new System.Windows.Forms.CheckBox();
            this.cmdel = new System.Windows.Forms.Button();
            this.txtfn = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnrightvopy = new System.Windows.Forms.Button();
            this.btnleftcopy = new System.Windows.Forms.Button();
            this.chkall = new System.Windows.Forms.CheckBox();
            this.btnclear = new System.Windows.Forms.Button();
            this.txtsort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkdifffldr = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.previewlist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 158);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(788, 437);
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
            this.previewlist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.previewlist.MultiSelect = false;
            this.previewlist.Name = "previewlist";
            this.previewlist.Size = new System.Drawing.Size(788, 437);
            this.previewlist.TabIndex = 2;
            this.previewlist.UseCompatibleStateImageBehavior = false;
            this.previewlist.View = System.Windows.Forms.View.Details;
            this.previewlist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.previewlist_ColumnClick);
            this.previewlist.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            this.previewlist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.previewlist_MouseClick);
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
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(796, 629);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.chkdifffldr);
            this.panel1.Controls.Add(this.btnTV);
            this.panel1.Controls.Add(this.chkselonly);
            this.panel1.Controls.Add(this.cmdsave);
            this.panel1.Controls.Add(this.btnoptions);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.chkReadonly);
            this.panel1.Controls.Add(this.cmdel);
            this.panel1.Controls.Add(this.txtfn);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.btnrightvopy);
            this.panel1.Controls.Add(this.btnleftcopy);
            this.panel1.Controls.Add(this.chkall);
            this.panel1.Controls.Add(this.btnclear);
            this.panel1.Controls.Add(this.txtsort);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 146);
            this.panel1.TabIndex = 4;
            // 
            // btnTV
            // 
            this.btnTV.Location = new System.Drawing.Point(253, 79);
            this.btnTV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTV.Name = "btnTV";
            this.btnTV.Size = new System.Drawing.Size(100, 28);
            this.btnTV.TabIndex = 24;
            this.btnTV.Text = "Preview";
            this.btnTV.UseVisualStyleBackColor = true;
            this.btnTV.Click += new System.EventHandler(this.btnTV_Click);
            // 
            // chkselonly
            // 
            this.chkselonly.AutoSize = true;
            this.chkselonly.Location = new System.Drawing.Point(164, 50);
            this.chkselonly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkselonly.Name = "chkselonly";
            this.chkselonly.Size = new System.Drawing.Size(142, 21);
            this.chkselonly.TabIndex = 23;
            this.chkselonly.Text = "Show All Selected";
            this.chkselonly.UseVisualStyleBackColor = true;
            this.chkselonly.CheckedChanged += new System.EventHandler(this.chkselonly_CheckedChanged);
            // 
            // cmdsave
            // 
            this.cmdsave.Location = new System.Drawing.Point(128, 78);
            this.cmdsave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdsave.Name = "cmdsave";
            this.cmdsave.Size = new System.Drawing.Size(100, 28);
            this.cmdsave.TabIndex = 22;
            this.cmdsave.Text = "Export";
            this.cmdsave.UseVisualStyleBackColor = true;
            this.cmdsave.Click += new System.EventHandler(this.cmdsave_Click);
            // 
            // btnoptions
            // 
            this.btnoptions.Location = new System.Drawing.Point(20, 78);
            this.btnoptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnoptions.Name = "btnoptions";
            this.btnoptions.Size = new System.Drawing.Size(100, 28);
            this.btnoptions.TabIndex = 20;
            this.btnoptions.Text = "Filter";
            this.btnoptions.UseVisualStyleBackColor = true;
            this.btnoptions.Click += new System.EventHandler(this.btnoptions_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "File:";
            // 
            // chkReadonly
            // 
            this.chkReadonly.AutoSize = true;
            this.chkReadonly.Location = new System.Drawing.Point(371, 86);
            this.chkReadonly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkReadonly.Name = "chkReadonly";
            this.chkReadonly.Size = new System.Drawing.Size(206, 21);
            this.chkReadonly.TabIndex = 18;
            this.chkReadonly.Text = "Remove Read only attribute";
            this.chkReadonly.UseVisualStyleBackColor = true;
            // 
            // cmdel
            // 
            this.cmdel.Location = new System.Drawing.Point(457, 37);
            this.cmdel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdel.Name = "cmdel";
            this.cmdel.Size = new System.Drawing.Size(100, 28);
            this.cmdel.TabIndex = 17;
            this.cmdel.Text = "Delete";
            this.cmdel.UseVisualStyleBackColor = true;
            this.cmdel.Click += new System.EventHandler(this.cmdel_Click);
            // 
            // txtfn
            // 
            this.txtfn.Location = new System.Drawing.Point(62, 121);
            this.txtfn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtfn.Name = "txtfn";
            this.txtfn.ReadOnly = true;
            this.txtfn.Size = new System.Drawing.Size(701, 22);
            this.txtfn.TabIndex = 13;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(516, 12);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(200, 12);
            this.progressBar1.TabIndex = 12;
            // 
            // btnrightvopy
            // 
            this.btnrightvopy.Location = new System.Drawing.Point(672, 41);
            this.btnrightvopy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnrightvopy.Name = "btnrightvopy";
            this.btnrightvopy.Size = new System.Drawing.Size(100, 28);
            this.btnrightvopy.TabIndex = 11;
            this.btnrightvopy.Text = "Copy Right";
            this.btnrightvopy.UseVisualStyleBackColor = true;
            this.btnrightvopy.Click += new System.EventHandler(this.btnright_Click);
            // 
            // btnleftcopy
            // 
            this.btnleftcopy.Location = new System.Drawing.Point(565, 39);
            this.btnleftcopy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnleftcopy.Name = "btnleftcopy";
            this.btnleftcopy.Size = new System.Drawing.Size(100, 28);
            this.btnleftcopy.TabIndex = 10;
            this.btnleftcopy.Text = "Copy Left";
            this.btnleftcopy.UseVisualStyleBackColor = true;
            this.btnleftcopy.Click += new System.EventHandler(this.btnleft_Click);
            // 
            // chkall
            // 
            this.chkall.AutoSize = true;
            this.chkall.Location = new System.Drawing.Point(20, 50);
            this.chkall.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkall.Name = "chkall";
            this.chkall.Size = new System.Drawing.Size(88, 21);
            this.chkall.TabIndex = 5;
            this.chkall.Text = "Select All";
            this.chkall.UseVisualStyleBackColor = true;
            this.chkall.Click += new System.EventHandler(this.chkall_CheckedChanged);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(288, 12);
            this.btnclear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(100, 28);
            this.btnclear.TabIndex = 3;
            this.btnclear.Text = "Clear";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // txtsort
            // 
            this.txtsort.Location = new System.Drawing.Point(121, 12);
            this.txtsort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtsort.Name = "txtsort";
            this.txtsort.ReadOnly = true;
            this.txtsort.Size = new System.Drawing.Size(145, 22);
            this.txtsort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sort Columns:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.statusStrip1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 602);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(790, 24);
            this.panel3.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(0, 2);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(790, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 17);
            // 
            // chkdifffldr
            // 
            this.chkdifffldr.AutoSize = true;
            this.chkdifffldr.Location = new System.Drawing.Point(585, 86);
            this.chkdifffldr.Margin = new System.Windows.Forms.Padding(4);
            this.chkdifffldr.Name = "chkdifffldr";
            this.chkdifffldr.Size = new System.Drawing.Size(186, 21);
            this.chkdifffldr.TabIndex = 25;
            this.chkdifffldr.Text = "Copy to a different folder";
            this.chkdifffldr.UseVisualStyleBackColor = true;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 629);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form5";
            this.Text = "Compare Folders";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.Button btnleftcopy;
        private System.Windows.Forms.CheckBox chkall;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.TextBox txtsort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtfn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button cmdel;
        private System.Windows.Forms.CheckBox chkReadonly;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnoptions;
        private System.Windows.Forms.Button cmdsave;
        private System.Windows.Forms.CheckBox chkselonly;
        private System.Windows.Forms.Button btnTV;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.CheckBox chkdifffldr;
    }
}