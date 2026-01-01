using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using static BackupRestoreTool.ArchiveInfo;

namespace BackupRestoreTool
{
    /// <summary>
    /// Interaction logic for newcollection.xaml
    /// </summary>
    public partial class newcollection : Window
    {
        public ArchiveInfo archinfo;
        public string archloc;
        public bool newarch;

        public newcollection()
        {
            InitializeComponent();
        }

        public void hidecontrols()
        {
            if (!newarch)
            {
                txtname.IsEnabled = false;
                dirbtn.IsEnabled = false;
                desc.IsEnabled = false;
                bkuploc.IsEnabled = false;
            }
            bkupsz.IsEnabled = false;
        }

        int getszindex()
        {
            switch (archinfo.archive_size)
            {
                case 1024:
                    return 0;
                case 512:
                    return 1;
                case 256:
                    return 2;
            }
            return -1;
        }

        public void load(ArchiveInfo ai)
        {
            archinfo = ai;
            txtname.Text = archinfo.name;
            desc.Text = archinfo.desc;
            bkuploc.SelectedIndex = (int)archinfo.arch_loc;
            foreach (var item in archinfo.archiveddirs)
            {
                dirlist.Items.Add(item.Key);
            }
            bkupsz.SelectedIndex = getszindex();
        }

        private void addbtn_Click(object sender, RoutedEventArgs e)
        {
            Form topmostWrapper = new Form { TopMost = true };
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog(topmostWrapper) == System.Windows.Forms.DialogResult.OK)
            {
                if (!dirlist.Items.Contains(dialog.SelectedPath))
                    dirlist.Items.Add(dialog.SelectedPath);
            }
        }
        private void Dir_Click(object sender, RoutedEventArgs e)
        {
            Form topmostWrapper = new Form { TopMost = true };
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog(topmostWrapper) == System.Windows.Forms.DialogResult.OK)
            {
                location.Text = dialog.SelectedPath;
            }
        }


        private void cloudbtn_Click(object sender, RoutedEventArgs e)
        {
            App.arm.Loadcloud();
            if (App.archivedir == null)
                return;
            location.Text = App.archivedir;
            App.archivedir = null;
        }

        private bool validate()
        {

            if (newarch)
            {
                if (txtname.Text == "")
                {
                    System.Windows.MessageBox.Show("name cannot be blank");
                    return false;
                }

                if (location.Text == "")
                {
                    System.Windows.MessageBox.Show("location is empty");
                    return false;
                }
                archloc = location.Text + "\\" + txtname.Text;
                if (DirectoryEx.Exists(archloc))
                {
                    System.Windows.MessageBox.Show("location is not empty");
                    return false;
                }

                if (desc.Text == "")
                {
                    System.Windows.MessageBox.Show("description is empty");
                    return false;
                }

                if (bkuploc.Text == "")
                {
                    System.Windows.MessageBox.Show("storage is empty");
                    return false;
                }

                if (bkuploc.SelectedIndex == 1 && bkupsz.Text == "")
                {
                    System.Windows.MessageBox.Show("storage size is empty");
                    return false;
                }

            }
            if (dirlist.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Directorries are empty");
                return false;
            }
            return true;
        }

        private void okbtn_Click(object sender, RoutedEventArgs e)
        {
            if (!validate())
            {
                return;
            }
            var selitms = from object o in dirlist.SelectedItems select o.ToString();
            var unselitms = (from object o in dirlist.Items select o.ToString()).Except(selitms);
            var fulitms = (from o in selitms select new KeyValuePair<string, bool>(o, true)).Union((from o in unselitms select new KeyValuePair<string, bool>(o, false))).ToList();
            var loc = ((bkuploc.SelectedIndex == 0) ? archive_location.harddisk : archive_location.cloud);
            var sz = (bkupsz.SelectedIndex == -1) ? 4096 : int.Parse(bkupsz.Text);
            archinfo = new ArchiveInfo { name = txtname.Text, desc = desc.Text, archiveddirs = fulitms, arch_loc = loc, archive_size=sz};
            DialogResult = true;

        }


        private void bkuploc_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            bkupsz.IsEnabled = e.AddedItems.Contains(bkuploc.Items[1]);
            if (!bkupsz.IsEnabled )
                bkupsz.SelectedIndex = -1;

        }

    }
}
