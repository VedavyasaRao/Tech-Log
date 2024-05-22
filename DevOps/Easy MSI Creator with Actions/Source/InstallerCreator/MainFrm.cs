using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using SetupLayout;
using System.Reflection;
using System.Threading;


namespace InstallerCreator
{
    public partial class MainFrm : Form
    {
        delegate void delupdatemsg(string msg);
        bool bdirty = false;
        ConfigHelper confighlpr = new ConfigHelper();
        string filename = "";
        delupdatemsg updmsg;
        Dictionary<string, Customaction> actions = new Dictionary<string, Customaction>();
        private void updatemsg(string msg)
        {
            statuslbl.Text = msg;
            Application.DoEvents();
        }

        public MainFrm()
        {
            InitializeComponent();
            updmsg = new delupdatemsg(updatemsg);
            ImageList myImageList = new ImageList();
            myImageList.Images.Add(Resource1.folder);
            myImageList.Images.Add(Resource1.filw);
            treeView1.ImageList = myImageList;
        }

        public void updatestatus(string msg)
        {
            if (InvokeRequired)
                Invoke(updmsg, new object[] { msg });
            else
                updatemsg(msg);
            Application.DoEvents();
        }

        private void createroot()
        {

            string execdir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            treeView1.Nodes.Clear();
            TreeNode root = treeView1.Nodes.Add("Root");
            root.ImageIndex = 0;
            root.Tag = new Nodedata("Root", false);
            TreeNode setup = root.Nodes.Add("Setup");
            setup.ImageIndex = 0;
            setup.Tag = new Nodedata("Setup", false);

            List<string> filenames = new List<string>  { "jsonparser.wsc", "Addshortcut.vbs","Interop.COMAdmin.dll", "Interop.TLI.dll", "SetupLayout.dll", "ComAppHelper.exe", "COMLibHelper.dll", 
                                     "ConfigEditor.exe", "FileDeploymentWizard.exe", "JSonSerializer.dll", "TLBINF32.DLL"  
                                 };

            TreeNode filenode = null;
            Nodedata nddata=null;
            foreach (string fn in filenames)
            {
                filenode = setup.Nodes.Add(fn);
                filenode.ImageIndex = 1;
                nddata = new Nodedata(execdir + fn, true);
                //if (fn == "jsonparser.wsc" || fn == "TLBINF32.DLL")
                //{
                //    nddata.inscustomactions.Add(Customaction.REGSVR_32, actions[Customaction.REGSVR_32].clone());
                //    nddata.uinscustomactions.Add(Customaction.UNREGSVR_32, actions[Customaction.UNREGSVR_32].clone());
                //}
                filenode.Tag = nddata;
            }

            string compname = "";
            compname = "%Files%";

            TreeNode comps = root.Nodes.Add(compname);
            comps.ImageIndex = 0;
            nddata = new Nodedata(compname, false);
            comps.Tag = nddata;
            nddata.inscustomactions.Add(Customaction.CREATEFOLDER, actions[Customaction.CREATEFOLDER].clone());
            nddata.uinscustomactions.Add(Customaction.REMOVEFOLDER, actions[Customaction.REMOVEFOLDER].clone());
        }


        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            bdirty = true;
            try
            {
                if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy &&
                    e.Data.GetDataPresent("Shell IDList Array", false))
                    e.Effect = DragDropEffects.Copy;
                else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move &&
                    e.Data.GetDataPresent("CodersLab.Windows.Controls.NodesCollection", false))
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void AddFiles(TreeNode node, List<Nodedata> filestoadd)
        {
            try
            {
                foreach (Nodedata s in filestoadd)
                {
                    string ppath = s.physicalpath;
                    TreeNode newnode = node.Nodes.Add(s.name);
                    updatestatus("adding " + s.name);
                    FileAttributes attr = File.GetAttributes(ppath);
                    newnode.Tag = s;
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        newnode.ImageIndex = 0;
                        s.physicalpath = "";
                        List<Nodedata> newfiles = confighlpr.AddDir(ppath);
                        AddFiles(newnode, newfiles);
                    }
                    else
                        newnode.ImageIndex = 1;
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                TreeNode node = treeView1.GetNodeAt(treeView1.PointToClient(new Point(e.X, e.Y)));
                if (node != null)
                {
                    if (e.Data.GetDataPresent("Shell IDList Array", false))
                    {
                        List<Nodedata> filestoadd = confighlpr.GetDDSelections((IDataObject)e.Data);
                        AddFiles(node, filestoadd);
                    }
                    else if (e.Data.GetDataPresent("CodersLab.Windows.Controls.NodesCollection", false))
                    {
                        foreach (TreeNode selnode in treeView1.SelectedNodes)
                        {
                            selnode.Parent.Nodes.Remove(selnode);
                            node.Nodes.Add(selnode);
                        }
                    }
                }
                updatestatus("");
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                Nodedata data = (Nodedata)e.Node.Tag;
                string sel = "";
                foreach (var a in data.inscustomactions)
                    sel = sel + ((sel=="")? "":",") + a.Value.name;
                foreach (var a in data.uinscustomactions)
                    sel = sel + ((sel == "") ? "" : ",") + a.Value.name;
                txtactions.Text = sel;
                txtOloc.Text = data.physicalpath;
                picfile.Image = null;
                btnfile.Enabled = false;
                if (data.physicalpath != "")
                {
                    btnfile.Enabled = true;
                    picfile.Image = (File.Exists(data.physicalpath)) ? Resource1.good : Resource1.bad;
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }

        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            bdirty = true;
            try
            {

                if (treeView1.SelectedNodes.Count > 0 && MessageBox.Show("Are You sure?", "Delete Node", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
                foreach (TreeNode selnode in treeView1.SelectedNodes)
                {
                    TreeNode selparent = selnode.Parent;
                    if (selparent == null)
                    {
                        MessageBox.Show("Cannot delete Root");
                        return;
                    }
                    selparent.Nodes.Remove(selnode);
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void sortbyname_Click(object sender, EventArgs e)
        {
            try
            {
                treeView1.TreeViewNodeSorter = new SortByName();
                treeView1.Sort();
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }

        }

        private void sortbyextn_Click(object sender, EventArgs e)
        {
            try
            {
                treeView1.TreeViewNodeSorter = new SortByExtension();
                treeView1.Sort();
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }


        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }

        }

        private void btnrename_Click(object sender, EventArgs e)
        {
            bdirty = true;
            try
            {
                if (treeView1.SelectedNodes.Count == 1)
                {
                    string oldl = treeView1.SelectedNodes[0].Text;
                    string newl = Microsoft.VisualBasic.Interaction.InputBox("Rename", oldl, oldl);
                    string s = (newl == "") ? oldl : newl;
                    treeView1.SelectedNodes[0].Text = s;
                    ((Nodedata)treeView1.SelectedNodes[0].Tag).name = treeView1.SelectedNodes[0].Text;
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void validatenodes(TreeNode root, ref bool bbad, ref string badmsg)
        {
            try
            {
                if (bbad)
                    return;

                int k = 0;
                bool bprev = false;
                foreach (char c in root.Text)
                {
                    if (c == '%' && bprev)
                    {
                        bbad = true;
                        badmsg = "bad name:" + root.FullPath;
                        return;
                    }
                    bprev = c == '%';
                    k += (bprev ? 1 : 0);
                }

                if ((k % 2) == 1)
                {
                    bbad = true;
                    badmsg = "bad name:" + root.FullPath;
                    return;
                }

                if (root.Nodes.Count == 0)
                {
                    Nodedata nd = (Nodedata)root.Tag;
                    if (!File.Exists(nd.physicalpath))
                    {
                        bbad = true;
                        badmsg = "bad file:" + root.FullPath;
                        return;
                    }
                }
                else
                {
                    List<string> nodenames = new List<string>();
                    foreach (TreeNode tn in root.Nodes)
                        nodenames.Add(tn.Text);

                    bbad = ((from anode in nodenames group anode by anode into grp where grp.Count() > 1 select grp).Count() > 0);
                    if (bbad)
                    {
                        bbad = true;
                        badmsg = "duplicate files or folders:" + root.FullPath;
                        return;
                    }

                    foreach (TreeNode tn in root.Nodes)
                    {
                        validatenodes(tn, ref bbad, ref badmsg);
                    }

                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }

        }

        private bool validate()
        {
            try
            {
                foreach (TreeNode tn in treeView1.Nodes[0].Nodes)
                {
                    if (tn.Text.IndexOf('%') == -1 && tn.Text.IndexOf(':') == -1 && tn.Text.ToLower() != "setup")
                    {
                        MessageBox.Show("top level nodes should have either logical path or physical full path:\r\n" + tn.FullPath);
                        return false;
                    }

                }

                bool bbad = false;
                string badmsg = "";
                validatenodes(treeView1.Nodes[0], ref bbad, ref badmsg);
                if (bbad)
                {
                    MessageBox.Show(badmsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
                return false;
            }

            return true;

        }

        private void addchildnodes(TreeNode tn, ref storagenode node)
        {
            try
            {
                foreach (TreeNode ctn in tn.Nodes)
                {
                    storagenode sn = new storagenode();
                    sn.nodedata = (Nodedata)ctn.Tag;
                    node.children.Add(sn);
                    addchildnodes(ctn, ref sn);
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }

        }

        private bool savedata(ref string filename, bool bfrmmsi)
        {
            try
            {
                if (!validate())
                    return false;

                storagenode sn = new storagenode();
                sn.nodedata = (Nodedata)treeView1.Nodes[0].Tag;
                addchildnodes(treeView1.Nodes[0], ref sn);
                if (!bfrmmsi || filename == "" || bdirty)
                {
                    savefrm sf = new savefrm(sn, filename, bfrmmsi);
                    if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return false;
                    filename = sf.filename;
                    loadlayoutfile();
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
                return false;
            }

            return true;
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            savedata(ref filename, false);
            bdirty = false;

        }

        private void Createnew()
        {
            if (bdirty && MessageBox.Show("do you want to abandon changes?", "new", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;
            filename = "";
            createroot();
            bdirty = false;
        }


        private void populatetree(storagenode node, TreeNodeCollection nodes)
        {
            try
            {
                TreeNode tn = nodes.Add(node.nodedata.name);
                tn.Tag = node.nodedata;
                tn.ImageIndex = node.children.Count == 0 ? 1 : 0;
                foreach (storagenode sn in node.children)
                {
                    populatetree(sn, tn.Nodes);
                }
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }


        private void loadlayoutfile()
        {
            try
            {
                storagenode node = layout.getstoragenode(filename);

                treeView1.Nodes.Clear();
                populatetree(node, treeView1.Nodes);
                bdirty = false;
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.CheckFileExists = true;
                ofd.AddExtension = false;
                ofd.DefaultExt = ".layout";
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                filename = ofd.FileName;
                loadlayoutfile();
                this.Text = "EasyInstallerCreator - " + Path.GetFileName(filename);
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void addfolder(bool blogical)
        {
            try
            {
                if (treeView1.SelectedNodes.Count != 1)
                    return;

                string newl = Microsoft.VisualBasic.Interaction.InputBox("Add Node", "NewNode", "NewNode");
                if (newl == "")
                    return;
                if (blogical)
                    newl = "%" + newl + "%";
                TreeNode selnode = treeView1.SelectedNodes[0];
                TreeNode selparent = selnode.Parent;
                if (selparent == null)
                    selparent = treeView1.Nodes[0];

                Nodedata parentdata = (Nodedata)selparent.Tag;
                TreeNode newnode = selparent.Nodes.Insert(selnode.Index, newl);
                newnode.Tag = new Nodedata(newl, false);
                treeView1.SelectedNodes.Clear();
                treeView1.SelectedNodes.Add(newnode);
                bdirty = true;
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void logicalFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addfolder(true);
        }

        private void physicalFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addfolder(false);

        }

        private void btnmsi_Click(object sender, EventArgs e)
        {
            try
            {
                string tempfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\fileconfigwiz";
                string layoutfile = tempfolder + "\\temp.layout";
                if (filename != "")
                    layoutfile = filename;

                string msifile = Program.msifile;

                if (msifile == "")
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.AddExtension = true;
                    sfd.CheckPathExists = true;
                    sfd.FileName = Path.GetFileNameWithoutExtension(layoutfile);
                    sfd.Filter = "MSI file|*.msi";

                    if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

                    msifile = sfd.FileName;
                }
                if (Directory.Exists(tempfolder))
                    Directory.Delete(tempfolder, true);
                Directory.CreateDirectory(tempfolder);
                if (!savedata(ref layoutfile, true))
                    return;

                MakeMSIHelper mkzip = new MakeMSIHelper(layoutfile, tempfolder, msifile);
                mkzip.MakeMSI();
                updatestatus("MSI is created");
            }
            catch (Exception ex)
            {
                updatestatus(ex.Message);
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            if (Program.layoutfile != "")
            {
                filename = Program.layoutfile;
                loadlayoutfile();
                if (Program.bsilent)
                {
                    btnmsi_Click(null, null);
                    Close();
                }
            }

            string afilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\customactions.json";
            if (!File.Exists(afilename))
                File.WriteAllText(afilename,Resource1.customactions1);
            actions = Customactions.read(afilename);
        }

        private void btnCustomActions_Click(object sender, EventArgs e)
        {
            string actfilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +"\\customactions.json";
            frmCustomActions frmca = new frmCustomActions(actions);
            if (frmca.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                actions = frmca.actions;
                Customactions.write(actfilename, actions);
            }
        }

        private void btnaction_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNodes.Count == 0)
                return;
            Nodedata data = (Nodedata)(treeView1.SelectedNodes[0].Tag);
            Dictionary<string, Customaction> customactions = (sender == btnInstall) ? data.inscustomactions : data.uinscustomactions;
            frmSetActions frm = new frmSetActions(Customactions.GetAllActions((sender == btnInstall), actions, data.physicalpath), customactions, (data.physicalpath.Length != 0));
            frm.ShowDialog();
            string sel = "";
            foreach (var a in data.inscustomactions)
                sel = sel + ((sel == "") ? "" : ",") + a.Value.name;
            foreach (var a in data.uinscustomactions)
                sel = sel + ((sel == "") ? "" : ",") + a.Value.name;
            txtactions.Text = sel;

        }

        private void btnfile_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNodes.Count == 0)
                return;
            Nodedata data = (Nodedata)(treeView1.SelectedNodes[0].Tag);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.FileName = data.physicalpath;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                data.physicalpath = ofd.FileName;
                picfile.Image = (File.Exists(data.physicalpath)) ? Resource1.good : Resource1.bad;
            }
        }

        private void btnhelp_Click(object sender, EventArgs e)
        {
            HelpFrm hf = new HelpFrm();
            hf.ShowDialog();
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            Createnew();

        }
    }

    public class SortByName : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            // If they are the same length, call Compare.
            return string.Compare(tx.Text.ToLower(), ty.Text.ToLower());
        }
    }

    public class SortByExtension : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if (tx.Nodes.Count > 0 || ty.Nodes.Count > 0)
                return 0;
            
            int m = tx.Text.LastIndexOf('.');
            int n = ty.Text.LastIndexOf('.');

            if (m == -1 || n == -1)
                return 0;

            // If they are the same length, call Compare.
            return string.Compare(tx.Text.Substring(m).ToLower(), ty.Text.Substring(n).ToLower());
        }
    }

}

