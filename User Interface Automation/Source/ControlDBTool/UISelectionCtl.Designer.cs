namespace UITesting.Automated.ControlDBTool
{
    partial class UISelectionCtl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UISelectionCtl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.controlTree = new System.Windows.Forms.TreeView();
            this.PopulateCtxtMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.populateChildrenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChildrenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnTglTBMode = new System.Windows.Forms.ToolStripButton();
            this.btnTglHighlite = new System.Windows.Forms.ToolStripButton();
            this.btnSelUICtl = new System.Windows.Forms.ToolStripButton();
            this.capturbitmap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnrecordstop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClearTree = new System.Windows.Forms.ToolStripButton();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnExpand = new System.Windows.Forms.ToolStripButton();
            this.btnCollapse = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutomationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutomationText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Patterns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.root = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnClearGrid = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLoadExisting = new System.Windows.Forms.ToolStripButton();
            this.btnSaveSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCodeGenerator = new System.Windows.Forms.ToolStripButton();
            this.btnhelp = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.PopulateCtxtMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(818, 479);
            this.splitContainer1.SplitterDistance = 394;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.controlTree, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 479);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // controlTree
            // 
            this.controlTree.AllowDrop = true;
            this.controlTree.ContextMenuStrip = this.PopulateCtxtMenu;
            this.controlTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlTree.Location = new System.Drawing.Point(3, 33);
            this.controlTree.Name = "controlTree";
            this.controlTree.ShowNodeToolTips = true;
            this.controlTree.Size = new System.Drawing.Size(388, 443);
            this.controlTree.TabIndex = 5;
            this.controlTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.controlTree_ItemDrag);
            this.controlTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.controlTree_AfterSelect);
            this.controlTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.controlTree_DragEnter);
            this.controlTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.controlTree_MouseClick);
            // 
            // PopulateCtxtMenu
            // 
            this.PopulateCtxtMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.populateChildrenToolStripMenuItem,
            this.addChildrenToolStripMenuItem,
            this.addToGridToolStripMenuItem});
            this.PopulateCtxtMenu.Name = "PopulateCtxtMenu";
            this.PopulateCtxtMenu.Size = new System.Drawing.Size(210, 70);
            this.PopulateCtxtMenu.Opening += new System.ComponentModel.CancelEventHandler(this.PopulateCtxtMenu_Opening);
            // 
            // populateChildrenToolStripMenuItem
            // 
            this.populateChildrenToolStripMenuItem.Name = "populateChildrenToolStripMenuItem";
            this.populateChildrenToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.populateChildrenToolStripMenuItem.Text = "Populate Children to Tree";
            this.populateChildrenToolStripMenuItem.Click += new System.EventHandler(this.populateChildrenToolStripMenuItem_Click);
            // 
            // addChildrenToolStripMenuItem
            // 
            this.addChildrenToolStripMenuItem.Name = "addChildrenToolStripMenuItem";
            this.addChildrenToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.addChildrenToolStripMenuItem.Text = "Add Children to Grid";
            this.addChildrenToolStripMenuItem.Click += new System.EventHandler(this.addChildrenToolStripMenuItem_Click);
            // 
            // addToGridToolStripMenuItem
            // 
            this.addToGridToolStripMenuItem.Name = "addToGridToolStripMenuItem";
            this.addToGridToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.addToGridToolStripMenuItem.Text = "Add to Grid";
            this.addToGridToolStripMenuItem.Click += new System.EventHandler(this.addToGridToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTglTBMode,
            this.btnTglHighlite,
            this.btnSelUICtl,
            this.capturbitmap,
            this.toolStripSeparator2,
            this.btnrecordstop,
            this.toolStripSeparator4,
            this.btnClearTree,
            this.btnSearch,
            this.btnExpand,
            this.btnCollapse});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(394, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnTglTBMode
            // 
            this.btnTglTBMode.CheckOnClick = true;
            this.btnTglTBMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTglTBMode.Image = global::UITesting.Automated.ControlDBTool.Properties.Resources.half;
            this.btnTglTBMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTglTBMode.Name = "btnTglTBMode";
            this.btnTglTBMode.Size = new System.Drawing.Size(23, 22);
            this.btnTglTBMode.Text = "toolStripButton3";
            this.btnTglTBMode.ToolTipText = "Toggle Toolbar mode";
            this.btnTglTBMode.Click += new System.EventHandler(this.btnTglTBMode_Click);
            // 
            // btnTglHighlite
            // 
            this.btnTglHighlite.Checked = true;
            this.btnTglHighlite.CheckOnClick = true;
            this.btnTglHighlite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnTglHighlite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTglHighlite.Image = ((System.Drawing.Image)(resources.GetObject("btnTglHighlite.Image")));
            this.btnTglHighlite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTglHighlite.Name = "btnTglHighlite";
            this.btnTglHighlite.Size = new System.Drawing.Size(23, 22);
            this.btnTglHighlite.Text = "Rectangle";
            this.btnTglHighlite.ToolTipText = "Toggle highliting selected UI element";
            this.btnTglHighlite.Click += new System.EventHandler(this.btnTglHighlite_CheckStateChanged);
            // 
            // btnSelUICtl
            // 
            this.btnSelUICtl.Checked = true;
            this.btnSelUICtl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnSelUICtl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelUICtl.Image = global::UITesting.Automated.ControlDBTool.Properties.Resources.Image1;
            this.btnSelUICtl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelUICtl.Name = "btnSelUICtl";
            this.btnSelUICtl.Size = new System.Drawing.Size(23, 22);
            this.btnSelUICtl.Text = "toolStripButton2";
            this.btnSelUICtl.ToolTipText = "Select UI element by drag and drop using mouse";
            this.btnSelUICtl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSelUICtl_MouseDown);
            // 
            // capturbitmap
            // 
            this.capturbitmap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.capturbitmap.Image = global::UITesting.Automated.ControlDBTool.Properties.Resources.camera;
            this.capturbitmap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.capturbitmap.Name = "capturbitmap";
            this.capturbitmap.Size = new System.Drawing.Size(23, 22);
            this.capturbitmap.Text = "toolStripButton1";
            this.capturbitmap.ToolTipText = "Capture bitmap";
            this.capturbitmap.Click += new System.EventHandler(this.capturbitmap_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnrecordstop
            // 
            this.btnrecordstop.CheckOnClick = true;
            this.btnrecordstop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnrecordstop.Image = ((System.Drawing.Image)(resources.GetObject("btnrecordstop.Image")));
            this.btnrecordstop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnrecordstop.Name = "btnrecordstop";
            this.btnrecordstop.Size = new System.Drawing.Size(23, 22);
            this.btnrecordstop.Text = "recordstop";
            this.btnrecordstop.ToolTipText = "Toggle record and stop of user actions";
            this.btnrecordstop.Click += new System.EventHandler(this.btnrecordstop_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClearTree
            // 
            this.btnClearTree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearTree.Image = global::UITesting.Automated.ControlDBTool.Properties.Resources.newbtn;
            this.btnClearTree.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearTree.Name = "btnClearTree";
            this.btnClearTree.Size = new System.Drawing.Size(23, 22);
            this.btnClearTree.Text = "toolStripButton1";
            this.btnClearTree.ToolTipText = "Clear Tree contents";
            this.btnClearTree.Click += new System.EventHandler(this.btnClearTree_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(23, 22);
            this.btnSearch.Text = "toolStripButton4";
            this.btnSearch.ToolTipText = "Search tree";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExpand.Image = ((System.Drawing.Image)(resources.GetObject("btnExpand.Image")));
            this.btnExpand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(23, 22);
            this.btnExpand.Text = "toolStripButton1";
            this.btnExpand.ToolTipText = "Expand tree";
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnCollapse
            // 
            this.btnCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCollapse.Image = ((System.Drawing.Image)(resources.GetObject("btnCollapse.Image")));
            this.btnCollapse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(23, 22);
            this.btnCollapse.Text = "toolStripButton2";
            this.btnCollapse.ToolTipText = "Collapse tree";
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(420, 479);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.Type,
            this.AutomationId,
            this.AutomationText,
            this.wParent,
            this.Patterns,
            this.Path,
            this.root});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(414, 443);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            // 
            // UserName
            // 
            this.UserName.HeaderText = "User Name";
            this.UserName.Name = "UserName";
            // 
            // Type
            // 
            this.Type.HeaderText = "Control Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // AutomationId
            // 
            this.AutomationId.HeaderText = "Automation Id";
            this.AutomationId.Name = "AutomationId";
            // 
            // AutomationText
            // 
            this.AutomationText.HeaderText = "Text";
            this.AutomationText.Name = "AutomationText";
            this.AutomationText.ReadOnly = true;
            // 
            // wParent
            // 
            this.wParent.HeaderText = "Parent";
            this.wParent.Name = "wParent";
            // 
            // Patterns
            // 
            this.Patterns.HeaderText = "Control Patterns";
            this.Patterns.Name = "Patterns";
            this.Patterns.ReadOnly = true;
            this.Patterns.Visible = false;
            // 
            // Path
            // 
            this.Path.HeaderText = "Path";
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Visible = false;
            // 
            // root
            // 
            this.root.HeaderText = "root";
            this.root.Name = "root";
            this.root.Visible = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClearGrid,
            this.btnDelete,
            this.toolStripSeparator1,
            this.btnLoadExisting,
            this.btnSaveSelection,
            this.toolStripSeparator3,
            this.btnCodeGenerator,
            this.btnhelp});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(420, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnClearGrid
            // 
            this.btnClearGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearGrid.Image = global::UITesting.Automated.ControlDBTool.Properties.Resources.newbtn;
            this.btnClearGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearGrid.Name = "btnClearGrid";
            this.btnClearGrid.Size = new System.Drawing.Size(23, 22);
            this.btnClearGrid.Text = "toolStripButton1";
            this.btnClearGrid.ToolTipText = "Clear grid contents";
            this.btnClearGrid.Click += new System.EventHandler(this.btnClearGrid_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "toolStripButton1";
            this.btnDelete.ToolTipText = "Delete selected row in the grid";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLoadExisting
            // 
            this.btnLoadExisting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadExisting.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadExisting.Image")));
            this.btnLoadExisting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadExisting.Name = "btnLoadExisting";
            this.btnLoadExisting.Size = new System.Drawing.Size(23, 22);
            this.btnLoadExisting.Text = "Load";
            this.btnLoadExisting.ToolTipText = "Load an existing selection of UI elements";
            this.btnLoadExisting.Click += new System.EventHandler(this.loadwsc_Click);
            // 
            // btnSaveSelection
            // 
            this.btnSaveSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveSelection.Image")));
            this.btnSaveSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveSelection.Name = "btnSaveSelection";
            this.btnSaveSelection.Size = new System.Drawing.Size(23, 22);
            this.btnSaveSelection.Text = "Load";
            this.btnSaveSelection.ToolTipText = "Save UI element selections to a file";
            this.btnSaveSelection.Click += new System.EventHandler(this.savewsc_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCodeGenerator
            // 
            this.btnCodeGenerator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCodeGenerator.Image = ((System.Drawing.Image)(resources.GetObject("btnCodeGenerator.Image")));
            this.btnCodeGenerator.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCodeGenerator.Name = "btnCodeGenerator";
            this.btnCodeGenerator.Size = new System.Drawing.Size(23, 22);
            this.btnCodeGenerator.Text = "toolStripButton1";
            this.btnCodeGenerator.ToolTipText = "Generate code";
            this.btnCodeGenerator.Click += new System.EventHandler(this.btnCodeGenerator_Click);
            // 
            // btnhelp
            // 
            this.btnhelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnhelp.Image = ((System.Drawing.Image)(resources.GetObject("btnhelp.Image")));
            this.btnhelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnhelp.Name = "btnhelp";
            this.btnhelp.Size = new System.Drawing.Size(23, 22);
            this.btnhelp.Text = "toolStripButton1";
            this.btnhelp.Click += new System.EventHandler(this.btnhelp_Click);
            // 
            // UISelectionCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UISelectionCtl";
            this.Size = new System.Drawing.Size(818, 479);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.PopulateCtxtMenu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnClearTree;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnTglHighlite;
        private System.Windows.Forms.ToolStripButton btnSelUICtl;
        private System.Windows.Forms.ToolStripButton btnTglTBMode;
        private System.Windows.Forms.TreeView controlTree;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnClearGrid;
        private System.Windows.Forms.ToolStripButton btnLoadExisting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSaveSelection;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip PopulateCtxtMenu;
        private System.Windows.Forms.ToolStripMenuItem populateChildrenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addChildrenToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripButton btnExpand;
        private System.Windows.Forms.ToolStripButton btnCollapse;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripMenuItem addToGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnCodeGenerator;
        private System.Windows.Forms.ToolStripButton capturbitmap;
        private System.Windows.Forms.ToolStripButton btnhelp;
        private System.Windows.Forms.ToolStripButton btnrecordstop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutomationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutomationText;
        private System.Windows.Forms.DataGridViewTextBoxColumn wParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patterns;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.DataGridViewTextBoxColumn root;
    }
}
