namespace MigrationHelper
{
    partial class Form3
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
            this.previewlist = new System.Windows.Forms.ListView();
            this.colname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colextn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colmerge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coldir = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colnew = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtfname = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnoptios = new System.Windows.Forms.Button();
            this.chkselonly = new System.Windows.Forms.CheckBox();
            this.chkall = new System.Windows.Forms.CheckBox();
            this.btnapply = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.txtsort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewlist
            // 
            this.previewlist.CheckBoxes = true;
            this.previewlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colname,
            this.colextn,
            this.colmerge,
            this.coldir,
            this.colnew});
            this.previewlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewlist.FullRowSelect = true;
            this.previewlist.GridLines = true;
            this.previewlist.Location = new System.Drawing.Point(0, 0);
            this.previewlist.MultiSelect = false;
            this.previewlist.Name = "previewlist";
            this.previewlist.Size = new System.Drawing.Size(704, 392);
            this.previewlist.TabIndex = 0;
            this.previewlist.UseCompatibleStateImageBehavior = false;
            this.previewlist.View = System.Windows.Forms.View.Details;
            this.previewlist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.previewlist_ColumnClick);
            this.previewlist.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.previewlist_ItemChecked);
            this.previewlist.DoubleClick += new System.EventHandler(this.previewlist_DoubleClick);
            this.previewlist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.previewlist_KeyDown);
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
            this.colmerge.Text = "Auto Merge";
            // 
            // coldir
            // 
            this.coldir.Text = "Directory";
            this.coldir.Width = 330;
            // 
            // colnew
            // 
            this.colnew.Text = "Remark";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtfname);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.btnoptios);
            this.panel1.Controls.Add(this.chkselonly);
            this.panel1.Controls.Add(this.chkall);
            this.panel1.Controls.Add(this.btnapply);
            this.panel1.Controls.Add(this.btnclear);
            this.panel1.Controls.Add(this.txtsort);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 72);
            this.panel1.TabIndex = 1;
            // 
            // txtfname
            // 
            this.txtfname.Location = new System.Drawing.Point(361, 40);
            this.txtfname.Name = "txtfname";
            this.txtfname.ReadOnly = true;
            this.txtfname.Size = new System.Drawing.Size(331, 20);
            this.txtfname.TabIndex = 9;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(437, 16);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(159, 10);
            this.progressBar1.TabIndex = 8;
            // 
            // btnoptios
            // 
            this.btnoptios.Location = new System.Drawing.Point(235, 41);
            this.btnoptios.Name = "btnoptios";
            this.btnoptios.Size = new System.Drawing.Size(75, 23);
            this.btnoptios.TabIndex = 7;
            this.btnoptios.Text = "Filter";
            this.btnoptios.UseVisualStyleBackColor = true;
            this.btnoptios.Click += new System.EventHandler(this.btnoptios_Click);
            // 
            // chkselonly
            // 
            this.chkselonly.AutoSize = true;
            this.chkselonly.Location = new System.Drawing.Point(107, 41);
            this.chkselonly.Name = "chkselonly";
            this.chkselonly.Size = new System.Drawing.Size(122, 17);
            this.chkselonly.TabIndex = 6;
            this.chkselonly.Text = "Show Selected Only";
            this.chkselonly.UseVisualStyleBackColor = true;
            this.chkselonly.CheckedChanged += new System.EventHandler(this.chkselonly_CheckedChanged);
            // 
            // chkall
            // 
            this.chkall.AutoSize = true;
            this.chkall.Location = new System.Drawing.Point(15, 41);
            this.chkall.Name = "chkall";
            this.chkall.Size = new System.Drawing.Size(70, 17);
            this.chkall.TabIndex = 5;
            this.chkall.Text = "Select All";
            this.chkall.UseVisualStyleBackColor = true;
            this.chkall.CheckedChanged += new System.EventHandler(this.chkall_CheckedChanged);
            // 
            // btnapply
            // 
            this.btnapply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnapply.Location = new System.Drawing.Point(617, 10);
            this.btnapply.Name = "btnapply";
            this.btnapply.Size = new System.Drawing.Size(75, 23);
            this.btnapply.TabIndex = 4;
            this.btnapply.Text = "Apply Patch";
            this.btnapply.UseVisualStyleBackColor = true;
            this.btnapply.Click += new System.EventHandler(this.btnapply_Click);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(272, 8);
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
            this.txtsort.Size = new System.Drawing.Size(175, 20);
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
            // panel2
            // 
            this.panel2.Controls.Add(this.previewlist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(704, 392);
            this.panel2.TabIndex = 2;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 464);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form3";
            this.Text = "Apply Patch";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView previewlist;
        private System.Windows.Forms.ColumnHeader colname;
        private System.Windows.Forms.ColumnHeader colextn;
        private System.Windows.Forms.ColumnHeader colmerge;
        private System.Windows.Forms.ColumnHeader coldir;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.TextBox txtsort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnapply;
        private System.Windows.Forms.CheckBox chkall;
        private System.Windows.Forms.Button btnoptios;
        private System.Windows.Forms.CheckBox chkselonly;
        private System.Windows.Forms.ColumnHeader colnew;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtfname;
    }
}