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
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using UITesting.Automated.MouseKeyboardActivityMonitor;
using UITesting.Automated.MouseKeyboardActivityMonitor.WinApi;

//TICS -3@102  -- not relevant here
//TICS -3@107  -- not relevant here
namespace UITesting.Automated.ControlDBTool
{
    /// <summary>Form1</summary>
    public partial class Form1 : Form
    {
        string filename = "";
        List<ControlInfo> controlst = new List<ControlInfo>();
        TreeNode draggednode;
        KeyboardHookListener m_KeyboardHookManager;
        MouseHookListener m_MouseHookManager;
        HighlightRectangle highlight;
        System.Windows.Rect focusedRect;
        AutomationElement selae;
        int mouseX, mouseY;
        bool dirty = false;
        AutomationElement rootele;
        int curcmdtreesel = 0;
        LinkedList<AutomationElement> selectedpath = new LinkedList<AutomationElement>();
        /// <summary>Form1</summary>
        public Form1()
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
            m_KeyboardHookManager = new KeyboardHookListener(new GlobalHooker());
            m_KeyboardHookManager.Enabled = true;
            m_KeyboardHookManager.KeyDown += HookManager_KeyDown;

            m_MouseHookManager = new MouseHookListener(new GlobalHooker());
            m_MouseHookManager.Enabled = true;
            m_MouseHookManager.MouseMove += HookManager_MouseMove;

            cmbTreeSel.SelectedIndex = 1;
            
            // Create highlight rectangle.
            highlight = new HighlightRectangle();
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                ThreadStart threadDelegate = new ThreadStart(GetAEfromPoint);
                Thread UIAutoThread = new Thread(threadDelegate);
                UIAutoThread.Start();
            }
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private ControlInfo AddControlinfo(AutomationElement ae, TreeNode tn)
        {
            ControlInfo actl = new ControlInfo();
            string s="";
            object obj = ae.GetCurrentPropertyValue(AutomationElement.RuntimeIdProperty);
            int[] ar = (int[])obj;
            foreach (int x in ar)
            {
                s += x.ToString();
                s += " ";
            }
            actl.AEType = ae.Current.ControlType.LocalizedControlType;
            actl.AERuntimeId = s;
            actl.AEText = ae.Current.Name;
            actl.AEAutomationId = ae.Current.AutomationId;
            System.Windows.Rect rect = ae.Current.BoundingRectangle;
            actl.AEBoundingRectangle = string.Format("{0} {1} {2} {3}",rect.X,rect.Y,rect.Width,rect.Height);
            actl.UserName = "";
            actl.Path = "";
            TreeNode tempnode = tn;
            while (tempnode.Parent != null)
            {
                actl.Path = tempnode.Index.ToString() + " " + actl.Path;
                tempnode = tempnode.Parent;

            }

            return actl;
        }

        private string GetAEString(AutomationElement ae)
        {
            string s = string.Format("{0} {1} [", ae.Current.ControlType.LocalizedControlType, ae.Current.Name);
            object obj = ae.GetCurrentPropertyValue(AutomationElement.RuntimeIdProperty);
            int[] ar = (int[])obj;
            foreach (int x in ar)
            {
                s += x.ToString();
                s += " ";
            }
            s += "]";

            return s;
        }

        private void selectnode()
        {
            TreeNode tn = null;
            bool bdone = false;
            for (int i = 0; i < 2; ++i)
            {
                if (controlTree.Nodes.Count > 0)
                {
                    string ss = GetAEString(selae);
                    TreeNode nd = controlTree.Nodes[0];
                    findnode(ss, nd, ref tn);
                    if (tn != null)
                    {
                        controlTree.SelectedNode = tn;
                        controlTree.ExpandAll();
                        controlTree.Focus();
                        bdone = true;
                        break;
                    }
                }

                if (bdone)
                {
                    return;
                }
                controlTree.Nodes.Clear();
                TreeNode trnode = controlTree.Nodes.Add(GetAEString(rootele));
                ControlInfo actl = AddControlinfo(rootele, trnode);
                actl.UserName = "root";
                trnode.Tag = actl;
                if (curcmdtreesel == 0)
                {
                    WalkControlElements(rootele, trnode);
                }
                else
                {
                    for (int m = 1; m < selectedpath.Count; ++m)
                    {
                        AutomationElement ae = selectedpath.ElementAt(m);

                        trnode = trnode.Nodes.Add(GetAEString(ae));
                        actl = AddControlinfo(ae, trnode);
                        trnode.Tag = actl;
                    }
                }
                controlTree.ExpandAll();
            }
        }

        private void GetAEfromPoint()
        {
            try
            {
                selae = AutomationElement.FromPoint(new System.Windows.Point(mouseX, mouseY));
                focusedRect = selae.Current.BoundingRectangle;

                UpdateHighlight();

                TreeWalker walker = TreeWalker.RawViewWalker;

                selectedpath.Clear();
                AutomationElement node = selae;
                while (node != AutomationElement.RootElement)
                {
                    selectedpath.AddFirst(node);
                    node = TreeWalker.RawViewWalker.GetParent(node);
                }
                rootele = selectedpath.ElementAt(0);

                Invoke(new Action(selectnode));
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void WalkControlElements(AutomationElement rootElement, TreeNode treeNode)
        {
            try
            {
                // Conditions for the basic views of the subtree (content, control, and raw) 
                // are available as fields of TreeWalker, and one of these is used in the 
                // following code.
                AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(rootElement);

                while (elementNode != null)
                {
                    TreeNode childTreeNode = treeNode.Nodes.Add(GetAEString(elementNode));
                    childTreeNode.Tag = AddControlinfo(elementNode, childTreeNode);
                    WalkControlElements(elementNode, childTreeNode);
                    elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void findnode(string srch, TreeNode node, ref TreeNode dest)
        {
            if (dest != null)
            {
                return;
            }

            foreach (TreeNode tn in node.Nodes)
            {
                if (tn.Text.Contains(srch))
                {
                    dest = tn;
                    controlTree.SelectedNode = dest;
                    break;
                }
                findnode(srch, tn, ref dest);
            }


        }

        /// <summary>
        /// Hides the old rectangle and creates a new one.
        /// </summary>
        private void UpdateHighlight()
        {
            // Hide old rectangle.
            highlight.Visible = false;

            // Show new rectangle.
            highlight.Location = new Rectangle(
                (int)focusedRect.Left, (int)focusedRect.Top,
                (int)focusedRect.Width, (int)focusedRect.Height);
            highlight.Visible = true;
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
            ControlInfo ctlinfo = (ControlInfo)draggednode.Tag;

            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                if (dgr.Tag == draggednode.Tag)
                {
                    MessageBox.Show("Cannot add same control");
                    return;
                }

            }
            if (dataGridView1.Rows.Count == 1)
            {
                ControlInfo tctl = (ControlInfo)controlTree.Nodes[0].Tag;
                int idx2 = dataGridView1.Rows.Add(new string[] { tctl.UserName, tctl.AERuntimeId, tctl.AEType, tctl.AEAutomationId, tctl.Path, tctl.AEText });
                dataGridView1.Rows[idx2].Tag = tctl;
            }
            else
            {
                ControlInfo tctl = (ControlInfo)controlTree.Nodes[0].Tag;
                if (dataGridView1.Rows[0].Cells[5].Value.ToString() != tctl.AEText)
                {
                    MessageBox.Show("Cannot add control from different window");
                    return;
                }
            }
            int idx = dataGridView1.Rows.Add(new string[] { ctlinfo.UserName, ctlinfo.AERuntimeId, ctlinfo.AEType, ctlinfo.AEAutomationId, ctlinfo.Path, ctlinfo.AEText });
            dataGridView1.Rows[idx].Tag = draggednode.Tag;
            dirty = true;
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

        }

        private void savewsc_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 1)
            {
                MessageBox.Show("nothing to save");
                return;
            }

            List<ControlInfo> selectednodes = new List<ControlInfo>();
            for (int idx=0; idx<dataGridView1.Rows.Count-1; ++idx)
            {

                if (dataGridView1.Rows[idx].Cells[0].Value == null)
                {
                    MessageBox.Show("User Name cannot be blank");
                    return;
                }

                ControlInfo ctlinfo = (ControlInfo)dataGridView1.Rows[idx].Tag;
                ctlinfo.UserName=dataGridView1.Rows[idx].Cells[0].Value.ToString();
                selectednodes.Add(ctlinfo);
            }

            var temp = (from ControlInfo in selectednodes
                        group ControlInfo by ControlInfo.UserName into names
                        where names.Count() > 1
                        select names.Key);

            if (temp.Count() > 0)
            {
                MessageBox.Show("User Name should be unique");
                return;

            }

            Form2 frm = new Form2(filename, selectednodes);
            DialogResult res = frm.ShowDialog();

            if (res == DialogResult.OK)
            {
                filename = frm.filename;
                dirty = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

        }


        private void controlTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ControlInfo actl = (ControlInfo)e.Node.Tag;
            char[] sep = { ' ' };
            string[] bds = actl.AEBoundingRectangle.Split(sep);
            focusedRect = new System.Windows.Rect(double.Parse(bds[0]),double.Parse(bds[1]),double.Parse(bds[2]),double.Parse(bds[3]));

            ThreadStart threadDelegate = new ThreadStart(UpdateHighlight);
            Thread UIAutoThread = new Thread(threadDelegate);
            UIAutoThread.Start();

        }

        private void search_Click(object sender, EventArgs e)
        {
            if (controlTree.Nodes.Count != 0)
            {
                TreeNode dest = null;
                findnode(toolStripTextBox1.Text, controlTree.Nodes[0], ref dest);
            }
        }

        private void loadwsc_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 1 &&
                MessageBox.Show("Current selections will be removed. Do you want to continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "WSC files(*.WSC)|*.WSC";
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string buffer = File.ReadAllText(dlg.FileName);
            int p = buffer.IndexOf("var usersel =");
            p += 14;
            int p2 = buffer.IndexOf("var description =");
            buffer = buffer.Substring(p, p2 - p); ;

            bool bdone = false;
            List<ControlInfo> selectednodes = JSONPersister<List<ControlInfo>>.SetJSON(buffer);
            if (controlTree.Nodes.Count > 0)
            {
                ControlInfo ctlinf = (ControlInfo)controlTree.Nodes[0].Tag;
                foreach (ControlInfo ctlinfo in selectednodes)
                {
                    if ((ctlinf.UserName == ctlinfo.UserName && ctlinf.AEText != ctlinfo.AEText) &&
                        (MessageBox.Show("The top level windows are different.\r\nDo you want to continue?", "",
                                MessageBoxButtons.YesNo) == DialogResult.No))
                    {
                        bdone = true;
                        break;
                    }

                }
            }

            if (bdone)
            {
                return;
            }

            dataGridView1.Rows.Clear();
            foreach (ControlInfo ctlinfo in selectednodes)
            {
                int idx = dataGridView1.Rows.Add(new string[] { ctlinfo.UserName, ctlinfo.AERuntimeId, ctlinfo.AEType, ctlinfo.AEAutomationId, ctlinfo.Path, ctlinfo.AEText });
                dataGridView1.Rows[idx].Tag = ctlinfo;
            }
            filename = dlg.FileName;
            dirty = false;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dirty)
            {
                if (MessageBox.Show("Do you want to abandon changes?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

            }
            m_KeyboardHookManager.Enabled = false;
            m_KeyboardHookManager.KeyDown -= HookManager_KeyDown;

            m_MouseHookManager.Enabled = false;
            m_MouseHookManager.MouseMove -= HookManager_MouseMove;

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[0].Value.ToString() == "root")
            {
                MessageBox.Show("Cannot delete the root node");
                e.Cancel = true;
            }
            dirty = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dirty = true;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to abandon changes?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                filename = "";
                dataGridView1.Rows.Clear();
            }

        }

        private void cmbTreeSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            curcmdtreesel = cmbTreeSel.SelectedIndex;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to clear the controls?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                controlTree.Nodes.Clear();
            }

        }

    }
}
//TICS +3@107
//TICS +3@102
