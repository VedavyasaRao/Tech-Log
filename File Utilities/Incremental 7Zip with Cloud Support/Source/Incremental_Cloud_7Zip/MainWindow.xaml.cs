using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BackupRestoreTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            App.mainwindow = this;
            App.vsb = vsb;
            App.logtxt = log;
            App.pmon.pbar = progress;
            App.disp = System.Windows.Application.Current.Dispatcher;
        }

        private void Dir_Click(object sender, RoutedEventArgs e)
        {
            srcfolder.Text = "";
            App.archivedir = null;
            Form topmostWrapper = new Form { TopMost = true };
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog(topmostWrapper) == System.Windows.Forms.DialogResult.OK)
            {
                srcfolder.Text = dialog.SelectedPath;
                App.archivedir = dialog.SelectedPath;
                if (!App.arm.Validate())
                {
                    App.archivedir = null;
                    return;
                }
                this.IsEnabled = false;
                App.arm.LoadArchive();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            if (config.AppSettings.Settings["archivedir"] != null)
            {
                srcfolder.Text = config.AppSettings.Settings["archivedir"].Value;
                App.archivedir = srcfolder.Text;
                if (!App.arm.Validate())
                {
                    App.archivedir = null;
                    return;
                }
                this.IsEnabled = false;
                App.arm.LoadArchive();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(
                           ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove("archivedir");
            config.AppSettings.Settings.Add("archivedir", srcfolder.Text);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void BackupVW_Click(object sender, RoutedEventArgs e)
        {
            if (!App.arm.Validate())
                return;
            App.backupvw = new BackupVW();
            App.backupvw.ShowDialog();
        }

        private void ManageVW_Click(object sender, RoutedEventArgs e)
        {

            newcollection wnd = new newcollection();
            if (((System.Windows.Controls.Button)sender).Content.ToString() == "New")
            {
                wnd.Title = "New Collection";
                wnd.newarch = true;
                wnd.hidecontrols();
                if (wnd.ShowDialog() ?? false)
                {
                    App.archivedir = wnd.archloc;
                    App.logit("Creating collection..." + wnd.archinfo.name);
                    if (!App.arm.CreateArchive(wnd.archinfo))
                    {
                        App.archivedir = null;
                        App.logit("Creating collection... failed" );
                        return;
                    }
                    App.logit("Creating collection... success");
                    if (App.archivedir != "")
                    {
                        srcfolder.Text = wnd.archloc;
                        App.arm.selarchive = wnd.archinfo;
                    }
                }
            }
            else
            {
                wnd.newarch = false;
                wnd.load(App.arm.selarchive);
                wnd.hidecontrols();
                wnd.Title = "Edit Collection";

                if (wnd.ShowDialog() ?? false)
                {
                    App.arm.selarchive.name = wnd.archinfo.name;
                    App.arm.selarchive.desc = wnd.archinfo.desc;
                    App.arm.selarchive.archiveddirs = wnd.archinfo.archiveddirs;
                    App.arm.Persist(App.arm.selarchive);
                    App.logit("Updated collection..." + wnd.archinfo.name);
                }
            }

        }

        private void RestoreVW_Click(object sender, RoutedEventArgs e)
        {
            if (!App.arm.Validate())
                return;
            App.arm.Load();
            App.restorevw = new RestoreVW();
            App.restorevw.ShowDialog();
        }

        private void UpdateVW_Click(object sender, RoutedEventArgs e)
        {
            if (!App.arm.Validate())
                return;
            App.arm.Load();
            App.editvw = new EditVW();
            App.editvw.ShowDialog();
        }

        private void Cloud_Click(object sender, RoutedEventArgs e)
        {

            App.arm.SelectCloudArchive();

        }
    }
}

