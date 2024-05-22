using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Windows.Automation;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Media;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using UITesting.Automated.MouseKeyboardActivityMonitor;
using UITesting.Automated.MouseKeyboardActivityMonitor.WinApi;
using System.Reflection;


namespace UITesting.Automated.ControlDBTool
{

    public partial class UISelectionCtl : UserControl
    {
        ContainerDetails cd =  new ContainerDetails("sample","sample");
        string filename = "";
        TreeNode draggednode;
        KeyboardHookListener m_KeyboardHookManager;
        MouseHookListener m_MouseHookManager;
        HighlightRectangle highlight;
        bool dirty = false;
        bool bfindwindow = false;

        private UIARecorder uiarecoder;
        bool brecorduseractions = false;
        Logger log = new Logger();
        public MainWnd HostForm
        {
            set;
            private get;
        }

        public UISelectionCtl()
        {
            try
            {
                Nativemethods.SetProcessDPIAware();
            }
            catch (EntryPointNotFoundException)
            {
                // Not running under Vista.
            }

            InitializeComponent();
            Hooker hooker = new GlobalHooker();
            m_KeyboardHookManager = new KeyboardHookListener(hooker);
            m_KeyboardHookManager.Enabled = true;
            m_KeyboardHookManager.KeyDown += HookManager_KeyDown;
            

            m_MouseHookManager = new MouseHookListener(hooker);
            m_MouseHookManager.Enabled = false;
            m_MouseHookManager.MouseUp += HookManager_MouseUp;
            m_MouseHookManager.MouseDown+= HookManager_MouseClick;
            m_MouseHookManager.MouseDoubleClick += new MouseEventHandler(HookManager_MouseDoubleClick);

            highlight = new HighlightRectangle();
            uiarecoder = new UIARecorder(this);
        }

        public void AppendToLog(string message)
        {
            log.AppendtoLog(message);
        }

        public void UpdateStatus(string message)
        {
            HostForm.UpdateStatus(message);
            AppendToLog(message);
        }

        public bool CanClose()
        {
            bool bret = true;

            if (brecorduseractions)
                uiarecoder.Stop();
            uiarecoder.Close();
            if (dirty)
            {
                bret = (MessageBox.Show("Do you want to abandon changes?", "", MessageBoxButtons.YesNo) == DialogResult.Yes);
            }
            log.Close();
            return bret;
        }

        private void updateposition()
        {
            Point pt = Cursor.Position;
            AutomationElement selae = AutomationElement.FromPoint(new System.Windows.Point(pt.X, pt.Y));
            if (selae.Current.ProcessId == System.Diagnostics.Process.GetCurrentProcess().Id)
                return;
            var br = selae.Current.BoundingRectangle;
            string message = string.Format("Absoute Location: X={0} Y={1}     Relative Location:  X={2} Y={3}", pt.X, pt.Y, pt.X - br.Left, pt.Y - br.Y);
            HostForm.UpdateStatus(message);
            AppendToLog(message);
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                SoundPlayer simpleSound = new SoundPlayer(Resource1.camera_shutter_click_01);
                simpleSound.Play();
                ThreadStart threadDelegate = new ThreadStart(GetAEfromPoint);
                Thread UIAutoThread = new Thread(threadDelegate);
                UIAutoThread.Start();
            }
            if (e.KeyCode == Keys.F11)
            {
                updateposition();
            }
            else if (e.KeyCode == Keys.Escape && bfindwindow)
            {
                m_MouseHookManager.Enabled = false;
                Cursor.Current = Cursors.Arrow;
            }
            else if (brecorduseractions)
            {
                AutomationElement invokedelement = AutomationElement.FocusedElement;
                uiarecoder.AddUserAction(invokedelement, e, null, null, false);
            }

        }

        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            if (bfindwindow)
            {
                bfindwindow = false;
                m_MouseHookManager.Enabled = false;
                Cursor.Current = Cursors.Arrow;
                HookManager_KeyDown(null, new KeyEventArgs(Keys.F12));
            }
        }

        private void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            if (!brecorduseractions)
                return;

            System.Windows.Point point = new System.Windows.Point(Cursor.Position.X, Cursor.Position.Y);
            AutomationElement invokedElement = AutomationElement.FromPoint(point);
            uiarecoder.AddUserAction(invokedElement, null,null, e, false);

        }

        private void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!brecorduseractions)
                return;

            System.Windows.Point point = new System.Windows.Point(Cursor.Position.X, Cursor.Position.Y);
            AutomationElement invokedElement = AutomationElement.FromPoint(point);
            uiarecoder.AddUserAction(invokedElement, null,null, e, true);

        }

        private void UpdateHighlight(object data)
        {
            System.Windows.Rect focusedRect = (System.Windows.Rect)data;
            highlight.Visible = false;

            highlight.Location = new Rectangle(
                (int)focusedRect.Left, (int)focusedRect.Top,
                (int)focusedRect.Width, (int)focusedRect.Height);

            highlight.Visible = btnTglHighlite.Checked;
        }

        private void GetAEfromPoint()
        {
            try
            {
                AutomationElement selae = AutomationElement.FromPoint(new System.Windows.Point(Cursor.Position.X, Cursor.Position.Y));
                if (selae.Current.ProcessId == System.Diagnostics.Process.GetCurrentProcess().Id)
                    return;
                UpdateHighlight(selae.Current.BoundingRectangle);
                Invoke(new Action<AutomationElement,bool>(selectnode), new object[] { selae, true});
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }
        
        private bool SelectUIElementInTree(AutomationElement selae, bool baddtogrid)
        {
            TreeNode selnode = null;
            try
            {
                if (controlTree.Nodes.Count > 0)
                {
                    TreeNode rootnode = controlTree.Nodes[0];
                    string aestr = UIAElementNode.GetAEString(selae);
                    UpdateStatus("finding " + aestr);
                    FindUIElementInTree(aestr, rootnode, ref selnode);
                    UpdateStatus("found " + aestr);
                    if (selnode != null)
                    {
                        controlTree.ExpandAll();
                        controlTree.SelectedNode = selnode;
                        controlTree.Focus();
                        UIAElementNode uianode = (UIAElementNode)controlTree.SelectedNode.Tag;
                        if (baddtogrid)
                        {
                            AddUIElementtoGrid(uianode);
                        }
                    }
                }
                UpdateStatus("");
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
            return (selnode != null);
        }


        private bool FindUIElementInTree(string selaetxt, TreeNode node, ref TreeNode dest)
        {
            try
            {
                UIAElementNode uianode = (UIAElementNode)node.Tag;
                if (uianode.AEString.Contains(selaetxt))
                {
                    dest = node;
                    return true;
                }
                else
                {
                    foreach (TreeNode tn in node.Nodes)
                    {
                        FindUIElementInTree(selaetxt, tn, ref dest);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
            return false;
        }

        private void AddUIElementtoGrid(UIAElementNode uianodeinfo)
        {
            try
            {
                UpdateStatus("checking grid for duplicate entry:" + uianodeinfo.Controlinfo.UserName);
                if (dataGridView1.Rows.Count != 1)
                {
                    for (int x = 0; x < dataGridView1.Rows.Count-1; ++x)
                    {
                        DataGridViewRow dgr = dataGridView1.Rows[x];
                        string aetext = "";
                        if (dgr.Tag is UITesting.Automated.ControlDBTool.UIAElementNode)
                        {
                            aetext = ((UIAElementNode)dgr.Tag).AEString;
                        }
                        else if (dgr.Tag is UITesting.Automated.ControlInf.ControlInfoPair)
                        {
                            aetext = ((UITesting.Automated.ControlInf.ControlInfoPair)dgr.Tag).ci.AEText;
                        }

                        if (aetext == uianodeinfo.AEString)
                        {
                            MessageBox.Show("Cannot add same control");
                            return;
                        }

                    }
                }

                if (controlTree.Nodes.Count == 0)
                {
                    return;
                }

                UpdateStatus("Adding to grid:" + uianodeinfo.AEString);
                UIAElementNode parentwin = ((UIAElementNode)controlTree.Nodes[0].Tag);
                ControlInfo ciparent = parentwin.Controlinfo;
                uianodeinfo.UpdateCenterpoint(parentwin.AE);
                int idx = dataGridView1.Rows.Add(new string[] { uianodeinfo.Controlinfo.UserName, uianodeinfo.Controlinfo.AEType, uianodeinfo.Controlinfo.AEAutomationId, uianodeinfo.Controlinfo.AEText, ciparent.AEText, uianodeinfo.Controlinfo.Patterns, JSONPersister<ControlInfo>.GetJSON(ciparent) });
                dataGridView1.Rows[idx].Tag = uianodeinfo;
                dirty = true;
                UpdateStatus("");
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void selectnode(AutomationElement selae, bool baddtogrid)
        {
            if (SelectUIElementInTree(selae, baddtogrid))
            {
                return;
            }

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            UpdateStatus("Getting selection path please wait...");

            controlTree.Nodes.Clear();
            LinkedList<AutomationElement> selectedpath = new LinkedList<AutomationElement>();
            try
            {
                TreeWalker walker = TreeWalker.RawViewWalker;
                AutomationElement aenode = selae;
                while (aenode != AutomationElement.RootElement)
                {
                    selectedpath.AddFirst(aenode);
                    aenode = TreeWalker.RawViewWalker.GetParent(aenode);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
                selectedpath = null;
            }

            try
            {
                TreeNode trnode = null;
                UpdateStatus("Adding selection path to tree view");
                for (int m = 0; m < selectedpath.Count; ++m)
                {
                    UIAElementNode uiaelement = UIAElementNode.GetUIATreeNode(selectedpath.ElementAt(m));
                    string selaetxt = uiaelement.AEString;

                    if (m == 0)
                    {
                        trnode = controlTree.Nodes.Add(selaetxt);
                    }
                    else
                    {
                        trnode = trnode.Nodes.Add(selaetxt);
                    }
                    trnode.Tag = uiaelement;

                }
                controlTree.ExpandAll();
                SelectUIElementInTree(selae, baddtogrid);
                System.Windows.Forms.Cursor.Current = Cursors.Arrow;
                UpdateStatus("");
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
                selectedpath = null;
            }
        }


        void PopulateTreeCtl(TreeNode trnode)
        {
            try
            {
                TreeWalker walker = TreeWalker.RawViewWalker;
                UIAElementNode uianode = (UIAElementNode)trnode.Tag;

                AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(uianode.AE);
                while (elementNode != null)
                {
                    UIAElementNode uiachildnode = UIAElementNode.GetUIATreeNode(elementNode);
                    UpdateStatus("adding childrens to treee view:" + uiachildnode.AEString);
                    TreeNode childtrnode = trnode.Nodes.Add(uiachildnode.AEString);
                    childtrnode.Tag = uiachildnode;
                    PopulateTreeCtl(childtrnode);
                    elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
                }
                UpdateStatus("");
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void controlTree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

        }

        private void controlTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            draggednode = (TreeNode)e.Item;
            DoDragDrop(e.Item, DragDropEffects.Move);

        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            AddUIElementtoGrid((UIAElementNode)draggednode.Tag);
        }


        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

        }

        private bool validateselection(ref List<ControlInfoPair> selectednodes)
        {
            try
            {
                selectednodes = new List<ControlInfoPair>();
                for (int idx = 0; idx < dataGridView1.Rows.Count - 1; ++idx)
                {

                    if (dataGridView1.Rows[idx].Cells[0].Value == null)
                    {
                        MessageBox.Show("User Name cannot be blank");
                        return false;
                    }

                    ControlInfo ctlinfo =(dataGridView1.Rows[idx].Tag is ControlInfoPair) ?((ControlInfoPair)dataGridView1.Rows[idx].Tag).ci:((UIAElementNode)dataGridView1.Rows[idx].Tag).Controlinfo;
                    ctlinfo.UserName = dataGridView1.Rows[idx].Cells[0].Value.ToString();
                    Regex rx = new Regex("^[a-zA-Z][_a-zA-Z0-9]*?$");
                    if (!rx.IsMatch(ctlinfo.UserName))
                    {
                        MessageBox.Show("User Name is not valid:" + ctlinfo.UserName);
                        return false;
                    }
                    selectednodes.Add((dataGridView1.Rows[idx].Tag is ControlInfoPair) ? ((ControlInfoPair)dataGridView1.Rows[idx].Tag) : (new ControlInfoPair(ctlinfo, JSONPersister<ControlInfo>.SetJSON(dataGridView1.Rows[idx].Cells[6].Value.ToString()))) );
                }

                var temp = (from cip in selectednodes
                            group cip by cip.ci.UserName into names
                            where names.Count() > 1
                            select names.Key);


                if (temp.Count() > 0)
                {
                    MessageBox.Show("User Name should be unique");
                    return false;

                }

            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }

            return true;

        }

        private void savewsc_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 1)
            {
                MessageBox.Show("nothing to save");
                return;
            }

            try
            {
                List<ControlInfoPair> selectednodes = new List<ControlInfoPair>();

                if (!validateselection(ref selectednodes))
                {
                    return;
                }

                SaveAsJS frm = new SaveAsJS(filename, cd, true, selectednodes);
                DialogResult res = frm.ShowDialog();

                if (res == DialogResult.OK)
                {
                    cd = frm.cd;
                    filename = frm.filename;
                    dirty = false;
                    foreach (UseractionData ua in uiarecoder.useractions)
                    {
                        ua.id = cd.id;
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }


        private void controlTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UIAElementNode uianode = (UIAElementNode)e.Node.Tag;
            Thread UIAutoThread = new Thread(UpdateHighlight);
            UIAutoThread.Start(uianode.AE.Current.BoundingRectangle);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (controlTree.Nodes.Count != 0)
                {
                    TreeNode selnode = null;
                    string txt=Microsoft.VisualBasic.Interaction.InputBox("Enter search string", "Search");
                    FindUIElementInTree(txt, controlTree.Nodes[0], ref selnode);
                    if (selnode != null)
                    {
                        controlTree.ExpandAll();
                        controlTree.SelectedNode = selnode;
                        controlTree.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void loadwsc_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 1 &&
                MessageBox.Show("Current selections will be removed. Do you want to continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.CheckFileExists = true;
                dlg.Filter = "JS files(*.JS)|*.JS|WSC files(*.WSC)|*.WSC";
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                List<ControlInfoPair> selectednodes = null;
                if (dlg.FileName.ToLower().IndexOf(".js") != -1)
                {
                    object[] objs = JSONPersister<object[]>.Read(dlg.FileName);
                    string buffer = JSONPersister<object>.GetJSON((object)objs[1]);
                    selectednodes = JSONPersister<List<ControlInfoPair>>.SetJSON(buffer);

                    buffer = JSONPersister<object>.GetJSON((object)objs[0]);
                    cd = JSONPersister<ContainerDetails>.SetJSON(buffer);
                }
                else if (dlg.FileName.ToLower().IndexOf(".wsc") != -1)
                {
                    selectednodes = new List<ControlInfoPair>();
                    string[] lines = File.ReadAllText(dlg.FileName).Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    int p1, p2;
                    cd = new ContainerDetails();
                    foreach (string line in lines)
                    {

                        if (line.IndexOf("description=") == 0)
                        {
                            p1 = line.IndexOf("=\"");
                            p2 = line.IndexOf("\"",p1+2);
                            cd.desc = line.Substring(p1 + 2, p2-p1-2);
                        }

                        if (line.IndexOf("progid=") == 0)
                        {
                            p1 = line.IndexOf("=\"");
                            p2 = line.IndexOf(".", p1 + 2);
                            cd.id = line.Substring(p1 + 2, p2 - p1 - 2);
                        }

                        if (line.IndexOf("version=") == 0)
                        {
                            p1 = line.IndexOf("=\"");
                            p2 = line.IndexOf("\"", p1 + 2);
                            cd.ver = int.Parse(line.Substring(p1 + 2, p2 - p1 - 2));
                        }

                    }

                    foreach (string line in lines)
                    {

                        if ((p1 = line.IndexOf("    get_")) != 0)
                            continue;
                        
                        if ((p1 = line.IndexOf("=\"")) == -1)
                            continue;
                        string token = line.Substring(p1 + 2, line.Length - p1 - 3).Replace("\"\"","\"");
                        selectednodes.Add(JSONPersister<ControlInfoPair>.SetJSON(token));

                    }


                }

                dataGridView1.Rows.Clear();
                foreach (ControlInfoPair cip in selectednodes)
                {
                    ControlInfo ci = cip.ci;
                    int idx = dataGridView1.Rows.Add(new string[] { ci.UserName, ci.AEType, ci.AEAutomationId, ci.AEText, cip.ciroot.AEText, ci.Patterns, JSONPersister<ControlInfo>.GetJSON(cip.ciroot) });
                    dataGridView1.Rows[idx].Tag = UIAElementNode.GetUIATreeNode(ci);
                }
                filename = dlg.FileName;
                dirty = false;
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to delete the row?", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            dirty = true;
        }


        private void btnTglHighlite_CheckStateChanged(object sender, EventArgs e)
        {
            btnTglHighlite.Image = btnTglHighlite.Checked ? Resource1.redrect : Resource1.norect;
            highlight.Visible = btnTglHighlite.Checked;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.Rows.Count - 1)
                return;
            try
            {

                UIAElementNode uianode = (UIAElementNode)dataGridView1.Rows[e.RowIndex].Tag;
                if (uianode.AE != null)
                {
                    Invoke(new Action<AutomationElement, bool>(selectnode), new object[] { uianode.AE, false });
                }
                else
                {
                    controlTree.Nodes.Clear();
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void btnSelUICtl_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Cross;
            bfindwindow = true;
            m_MouseHookManager.Enabled = true;
        }

        private void btnTglTBMode_Click(object sender, EventArgs e)
        {
            btnTglTBMode.Image = btnTglTBMode.Checked ? Resource1.full : Resource1.half;
            splitContainer1.Panel2Collapsed = btnTglTBMode.Checked;
            tableLayoutPanel1.Height =30;
            HostForm.ToggleTBMode(btnTglTBMode.Checked);
        }

        private void btnClearTree_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to clear the controls?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controlTree.Nodes.Clear();
            }

        }

        private void populateChildrenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode trnode = controlTree.SelectedNode;
                trnode.Nodes.Clear();
                PopulateTreeCtl(trnode);
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void btnClearGrid_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to clear the grid?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
            }
            cd = new ContainerDetails("sample", "sample");
            filename = "";

        }


        private void btnExpand_Click(object sender, EventArgs e)
        {
            controlTree.ExpandAll();
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            controlTree.CollapseAll();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRowCancelEventArgs dgve = new DataGridViewRowCancelEventArgs(dataGridView1.SelectedRows[0]);
                    dataGridView1_UserDeletingRow(null, dgve);
                    if (!dgve.Cancel)
                    {
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    }
                }

            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }

        }

        private void addChildrenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                TreeNode trnode = controlTree.SelectedNode;
                foreach (TreeNode childnode in trnode.Nodes)
                {
                    AddUIElementtoGrid((UIAElementNode)childnode.Tag);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void addToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode trnode = controlTree.SelectedNode;
                AddUIElementtoGrid((UIAElementNode)trnode.Tag);
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }

        }

        private void PopulateCtxtMenu_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                UIAElementNode uianode = (UIAElementNode)controlTree.SelectedNode.Tag;
                populateChildrenToolStripMenuItem.Enabled = (TreeWalker.RawViewWalker.GetFirstChild(uianode.AE) != null);
                addChildrenToolStripMenuItem.Enabled = populateChildrenToolStripMenuItem.Enabled;
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
                e.Cancel = true;
            }

        }

        private void PopulateCtxtMenu_Opened(object sender, EventArgs e)
        {
            try
            {
                TreeNode trnode = controlTree.SelectedNode;
                trnode.Nodes.Clear();
                PopulateTreeCtl(trnode);
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void btnCodeGenerator_Click(object sender, EventArgs e)
        {
            try
            {
                List<ControlInfoPair> selectednodes = new List<ControlInfoPair>();
                ContainerDetails cdtls = new ContainerDetails("sample", "sample");
                if (dataGridView1.Rows.Count > 1)
                {
                    if (!validateselection(ref selectednodes))
                    {
                        if (MessageBox.Show("Do you want to use existing selections?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    if (cd.id == "sample")
                    {
                        SaveAsJS frm = new SaveAsJS(filename, cd, false, selectednodes);
                        DialogResult res = frm.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            cd = frm.cd;
                        }
                        else
                        {
                            return;
                        }
                    }
                    cdtls = cd;
                }
                CodeGenearator cgen = new CodeGenearator(cdtls, selectednodes, uiarecoder.useractions);
                cgen.ShowDialog(this);
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }

        }

        private void controlTree_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    TreeNode tn = controlTree.GetNodeAt(e.Location);
                    controlTree.SelectedNode = tn;
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("Exception occured:" + ex.Message);
            }
        }

        private void capturbitmap_Click(object sender, EventArgs e)
        {
            if (controlTree.SelectedNode == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "png files(*.png)|*.png";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string fname = dlg.FileName;
            if (!fname.ToLower().Contains(".png"))
                fname = fname + ".png";
            System.Threading.Thread.Sleep(5000);
            UIAElementNode uianode = (UIAElementNode)controlTree.SelectedNode.Tag;
            AutomationElement selae = uianode.AE;
            System.Windows.Rect rect = selae.Current.BoundingRectangle;

            Bitmap printscreen = new Bitmap((int)rect.Width, (int)rect.Height);
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            highlight.Visible = false;
            graphics.CopyFromScreen((int)rect.Left, (int)rect.Top, 0, 0, new System.Drawing.Size((int)rect.Width, (int)rect.Height));
            highlight.Visible = btnTglHighlite.Checked;
            printscreen.Save(fname, System.Drawing.Imaging.ImageFormat.Png);

        }

        private void btnhelp_Click(object sender, EventArgs e)
        {
            HelpFrm h = new HelpFrm();
            h.ShowDialog();
        }


        private void btnrecordstop_Click(object sender, EventArgs e)
        {
            btnrecordstop.Image = btnrecordstop.Checked ? Resource1.stop: Resource1.record;
            brecorduseractions = btnrecordstop.Checked;
            if (btnrecordstop.Checked)
            {
                m_MouseHookManager.Enabled =true;
                Thread.Sleep(500);
                uiarecoder.Start();
            }
            else
            {
                m_MouseHookManager.Enabled = false;
                Thread.Sleep(500);
                uiarecoder.Stop();
                if (MessageBox.Show("Do you want to Save the recording?", "Record actions", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                uiarecoder.Updateuseractionevent();
                FrmUserActions fua = new FrmUserActions(uiarecoder);
                if (fua.ShowDialog() != DialogResult.OK)
                    return;
                List<string> lstactions = new List<string>();
                dataGridView1.Rows.Clear();
                foreach (UseractionData ua in uiarecoder.useractions)
                {
                    if (!ua.isgood)
                        continue;

                    if (lstactions.Find((s) => s == ua.aelement.ci.UserName) != null)
                        continue;
                    ControlInfo ci = ua.aelement.ci;
                    lstactions.Add(ci.UserName);
                    int idx = dataGridView1.Rows.Add(new string[] { ci.UserName, ci.AEType, ci.AEAutomationId, ci.AEText, ua.aelement.ciroot.AEText, ci.Patterns, JSONPersister<ControlInfo>.GetJSON(ua.aelement.ciroot) });
                    dataGridView1.Rows[idx].Tag = ua.aelement;
                }
            }
        }

    }
}
 