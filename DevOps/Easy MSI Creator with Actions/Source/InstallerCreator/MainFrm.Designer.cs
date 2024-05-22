namespace InstallerCreator
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnnew = new System.Windows.Forms.ToolStripButton();
            this.btnLoad = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnCustomActions = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCollapse = new System.Windows.Forms.ToolStripButton();
            this.btnExpand = new System.Windows.Forms.ToolStripButton();
            this.btnaddfolder = new System.Windows.Forms.ToolStripDropDownButton();
            this.logicalFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicalFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btndelete = new System.Windows.Forms.ToolStripButton();
            this.btnrename = new System.Windows.Forms.ToolStripButton();
            this.sortby = new System.Windows.Forms.ToolStripDropDownButton();
            this.sortbyname = new System.Windows.Forms.ToolStripMenuItem();
            this.sortbyextn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnmsi = new System.Windows.Forms.ToolStripButton();
            this.btnhelp = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new CodersLab.Windows.Controls.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtOloc = new System.Windows.Forms.TextBox();
            this.btnfile = new System.Windows.Forms.Button();
            this.picfile = new System.Windows.Forms.PictureBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnuninstall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtactions = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statuslbl = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picfile)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnnew,
            this.btnLoad,
            this.btnSave,
            this.btnCustomActions,
            this.toolStripSeparator1,
            this.btnCollapse,
            this.btnExpand,
            this.btnaddfolder,
            this.btndelete,
            this.btnrename,
            this.sortby,
            this.btnmsi,
            this.btnhelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(806, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnnew
            // 
            this.btnnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnnew.Image = ((System.Drawing.Image)(resources.GetObject("btnnew.Image")));
            this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(28, 28);
            this.btnnew.Text = "toolStripButton1";
            this.btnnew.ToolTipText = "New layout";
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(28, 28);
            this.btnLoad.Text = "toolStripButton1";
            this.btnLoad.ToolTipText = "Open layout";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(28, 28);
            this.btnSave.Text = "toolStripButton2";
            this.btnSave.ToolTipText = "Save layout";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCustomActions
            // 
            this.btnCustomActions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCustomActions.Image = ((System.Drawing.Image)(resources.GetObject("btnCustomActions.Image")));
            this.btnCustomActions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCustomActions.Name = "btnCustomActions";
            this.btnCustomActions.Size = new System.Drawing.Size(28, 28);
            this.btnCustomActions.Text = "toolStripButton1";
            this.btnCustomActions.ToolTipText = "Define Custom Actions";
            this.btnCustomActions.Click += new System.EventHandler(this.btnCustomActions_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // btnCollapse
            // 
            this.btnCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCollapse.Image = ((System.Drawing.Image)(resources.GetObject("btnCollapse.Image")));
            this.btnCollapse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(28, 28);
            this.btnCollapse.ToolTipText = "Expand Tree";
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExpand.Image = ((System.Drawing.Image)(resources.GetObject("btnExpand.Image")));
            this.btnExpand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(28, 28);
            this.btnExpand.Text = "toolStripButton4";
            this.btnExpand.ToolTipText = "Collapse Tree";
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnaddfolder
            // 
            this.btnaddfolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnaddfolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logicalFolderToolStripMenuItem,
            this.physicalFolderToolStripMenuItem});
            this.btnaddfolder.Image = ((System.Drawing.Image)(resources.GetObject("btnaddfolder.Image")));
            this.btnaddfolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnaddfolder.Name = "btnaddfolder";
            this.btnaddfolder.Size = new System.Drawing.Size(42, 28);
            this.btnaddfolder.Text = "btnaddFolder";
            this.btnaddfolder.ToolTipText = "Create folder";
            // 
            // logicalFolderToolStripMenuItem
            // 
            this.logicalFolderToolStripMenuItem.Name = "logicalFolderToolStripMenuItem";
            this.logicalFolderToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.logicalFolderToolStripMenuItem.Text = "Logical folder";
            this.logicalFolderToolStripMenuItem.ToolTipText = "Create logical folder";
            this.logicalFolderToolStripMenuItem.Click += new System.EventHandler(this.logicalFolderToolStripMenuItem_Click);
            // 
            // physicalFolderToolStripMenuItem
            // 
            this.physicalFolderToolStripMenuItem.Name = "physicalFolderToolStripMenuItem";
            this.physicalFolderToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.physicalFolderToolStripMenuItem.Text = "Physical folder";
            this.physicalFolderToolStripMenuItem.ToolTipText = "Create physical folder";
            this.physicalFolderToolStripMenuItem.Click += new System.EventHandler(this.physicalFolderToolStripMenuItem_Click);
            // 
            // btndelete
            // 
            this.btndelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btndelete.Image = ((System.Drawing.Image)(resources.GetObject("btndelete.Image")));
            this.btndelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(28, 28);
            this.btndelete.Text = "btnDelete";
            this.btndelete.ToolTipText = "Delete file or folder";
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnrename
            // 
            this.btnrename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnrename.Image = ((System.Drawing.Image)(resources.GetObject("btnrename.Image")));
            this.btnrename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnrename.Name = "btnrename";
            this.btnrename.Size = new System.Drawing.Size(28, 28);
            this.btnrename.Text = "toolStripButton1";
            this.btnrename.ToolTipText = "Rename file or folder";
            this.btnrename.Click += new System.EventHandler(this.btnrename_Click);
            // 
            // sortby
            // 
            this.sortby.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sortby.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortbyname,
            this.sortbyextn});
            this.sortby.Image = ((System.Drawing.Image)(resources.GetObject("sortby.Image")));
            this.sortby.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sortby.Name = "sortby";
            this.sortby.Size = new System.Drawing.Size(42, 28);
            this.sortby.Text = "toolStripDropDownButton1";
            this.sortby.ToolTipText = "Sort files and folders";
            // 
            // sortbyname
            // 
            this.sortbyname.Name = "sortbyname";
            this.sortbyname.Size = new System.Drawing.Size(171, 30);
            this.sortbyname.Text = "Name";
            this.sortbyname.ToolTipText = "Sort by names";
            this.sortbyname.Click += new System.EventHandler(this.sortbyname_Click);
            // 
            // sortbyextn
            // 
            this.sortbyextn.Name = "sortbyextn";
            this.sortbyextn.Size = new System.Drawing.Size(171, 30);
            this.sortbyextn.Text = "Extension";
            this.sortbyextn.ToolTipText = "Sort by Extension";
            this.sortbyextn.Click += new System.EventHandler(this.sortbyextn_Click);
            // 
            // btnmsi
            // 
            this.btnmsi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnmsi.Image = ((System.Drawing.Image)(resources.GetObject("btnmsi.Image")));
            this.btnmsi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnmsi.Name = "btnmsi";
            this.btnmsi.Size = new System.Drawing.Size(28, 28);
            this.btnmsi.Text = "toolStripButton1";
            this.btnmsi.ToolTipText = "Create MSI";
            this.btnmsi.Click += new System.EventHandler(this.btnmsi_Click);
            // 
            // btnhelp
            // 
            this.btnhelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnhelp.Image = ((System.Drawing.Image)(resources.GetObject("btnhelp.Image")));
            this.btnhelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnhelp.Name = "btnhelp";
            this.btnhelp.Size = new System.Drawing.Size(28, 28);
            this.btnhelp.Text = "toolStripButton1";
            this.btnhelp.Click += new System.EventHandler(this.btnhelp_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(806, 786);
            this.splitContainer1.SplitterDistance = 621;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.treeView1.SelectionMode = CodersLab.Windows.Controls.TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch;
            this.treeView1.Size = new System.Drawing.Size(806, 621);
            this.treeView1.TabIndex = 0;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.txtOloc, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnfile, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.picfile, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnInstall, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnuninstall, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtactions, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(806, 120);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtOloc
            // 
            this.txtOloc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOloc.Location = new System.Drawing.Point(154, 5);
            this.txtOloc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOloc.Name = "txtOloc";
            this.txtOloc.ReadOnly = true;
            this.txtOloc.Size = new System.Drawing.Size(498, 26);
            this.txtOloc.TabIndex = 19;
            // 
            // btnfile
            // 
            this.btnfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnfile.Location = new System.Drawing.Point(660, 5);
            this.btnfile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnfile.Name = "btnfile";
            this.btnfile.Size = new System.Drawing.Size(67, 50);
            this.btnfile.TabIndex = 23;
            this.btnfile.Text = "...";
            this.btnfile.UseVisualStyleBackColor = true;
            this.btnfile.Click += new System.EventHandler(this.btnfile_Click);
            // 
            // picfile
            // 
            this.picfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.picfile.Location = new System.Drawing.Point(735, 5);
            this.picfile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picfile.Name = "picfile";
            this.picfile.Size = new System.Drawing.Size(67, 49);
            this.picfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picfile.TabIndex = 24;
            this.picfile.TabStop = false;
            // 
            // btnInstall
            // 
            this.btnInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.ForeColor = System.Drawing.Color.IndianRed;
            this.btnInstall.Location = new System.Drawing.Point(660, 65);
            this.btnInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(67, 50);
            this.btnInstall.TabIndex = 25;
            this.btnInstall.Text = "I";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnaction_Click);
            // 
            // btnuninstall
            // 
            this.btnuninstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnuninstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnuninstall.ForeColor = System.Drawing.Color.Blue;
            this.btnuninstall.Location = new System.Drawing.Point(735, 65);
            this.btnuninstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnuninstall.Name = "btnuninstall";
            this.btnuninstall.Size = new System.Drawing.Size(67, 50);
            this.btnuninstall.TabIndex = 22;
            this.btnuninstall.Text = "U";
            this.btnuninstall.UseVisualStyleBackColor = true;
            this.btnuninstall.Click += new System.EventHandler(this.btnaction_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.label1.Size = new System.Drawing.Size(135, 28);
            this.label1.TabIndex = 20;
            this.label1.Text = "Physical Location:";
            // 
            // txtactions
            // 
            this.txtactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtactions.Location = new System.Drawing.Point(154, 65);
            this.txtactions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtactions.Name = "txtactions";
            this.txtactions.ReadOnly = true;
            this.txtactions.Size = new System.Drawing.Size(498, 26);
            this.txtactions.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.label2.Size = new System.Drawing.Size(125, 28);
            this.label2.TabIndex = 21;
            this.label2.Text = "Custom Actions:";
            // 
            // statuslbl
            // 
            this.statuslbl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statuslbl.Location = new System.Drawing.Point(0, 788);
            this.statuslbl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.statuslbl.Multiline = true;
            this.statuslbl.Name = "statuslbl";
            this.statuslbl.ReadOnly = true;
            this.statuslbl.Size = new System.Drawing.Size(806, 29);
            this.statuslbl.TabIndex = 3;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 817);
            this.Controls.Add(this.statuslbl);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainFrm";
            this.Text = "Easy Installer Creator";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton btnLoad;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnCollapse;
        private System.Windows.Forms.ToolStripButton btnExpand;
        private System.Windows.Forms.ToolStripButton btndelete;
        private System.Windows.Forms.ToolStripDropDownButton sortby;
        private System.Windows.Forms.ToolStripMenuItem sortbyname;
        private System.Windows.Forms.ToolStripMenuItem sortbyextn;
        private CodersLab.Windows.Controls.TreeView treeView1;
        private System.Windows.Forms.ToolStripButton btnrename;
        private System.Windows.Forms.ToolStripDropDownButton btnaddfolder;
        private System.Windows.Forms.ToolStripMenuItem logicalFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem physicalFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnmsi;
        private System.Windows.Forms.ToolStripButton btnCustomActions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnuninstall;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtactions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOloc;
        private System.Windows.Forms.TextBox statuslbl;
        private System.Windows.Forms.Button btnfile;
        private System.Windows.Forms.PictureBox picfile;
        private System.Windows.Forms.ToolStripButton btnhelp;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.ToolStripButton btnnew;

    }
}

