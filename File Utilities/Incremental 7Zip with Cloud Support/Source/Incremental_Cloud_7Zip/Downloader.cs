using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BackupRestoreTool
{

    public class icedrive_folderinfo
    {
        public string path;
        public UInt64 fldrid;
        public UInt64 brinfofid;
        public UInt64 bkupfldrid;
    }

    public class icedrive_fileinfo
    {
        public string filename;
        public UInt64 fileid;
    }

    public class ice_interface
    {
        public string status;


        public icedrive_folderinfo selarchive =null;
        public List<icedrive_folderinfo> archive_folder_list = null;
        public List<icedrive_fileinfo> archive_fileslist = null;

        void getuseridpasswd()
        {

            if (App.bearertoken == "")
            {
                App.bearertoken = login();
            }

        }

        public string login()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                App.loginwindow = new IceDriveLogin();
                App.loginwindow.ShowDialog();
                if (!((IceDriveLogin)App.loginwindow).LoginSuccessful)
                    return;
            });

            return ((IceDriveLogin)App.loginwindow).BearerToken;
        }

        public void create_folder(UInt64 fid, string foldername)
        {
            getuseridpasswd();
            status = "";
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    long newfolderId = ice.CreateFolder((long)fid, foldername);
                    status = (newfolderId == -1)? "ERROR: Failed to create folder":$"Success";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }
            }
        }

        public void delete_filefolder(UInt64 fid)
        {
            getuseridpasswd();

            status = "";
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    var item =  ice.GetItem((long)selarchive.fldrid, (long)fid);
                    if (!(item is null) && item.Count != 0)
                    {

                        bool success = ice.DeleteItem(
                            item[0].id,
                            (item[0].isFolder == 0));

                        status = (!success) ? "ERROR: Failed to delete" : $"Success";
                    }
                    else
                        status = $"ERROR: Item not found";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }
            }
        }

        public void download_file(UInt64 fid, string target_folder)
        {
            getuseridpasswd();
            status = "";
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    var item = ice.GetItem((long)selarchive.fldrid, (long)fid, true);
                    if (!(item is null) && item.Count != 0)
                    {
                        bool success = ice.DownloadFile(
                            item[0].id,
                            target_folder.Replace('\\', '/')+"/"+ item[0].filename);

                        status = (!success) ? "ERROR: Failed to download" : $"Success";
                    }
                    else
                        status = $"ERROR: Item not found";

                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }
            }
        }

        public UInt64 find_filefolder(UInt64 fid, string filefoldername)
        {
            getuseridpasswd();
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                UInt64 ret = 0;
                try
                {
                    
                    var items = ice.GetItem((long)fid, -1); 

                    foreach (var itm in items)
                    {
                        if (itm.filename == filefoldername)
                        {
                            ret = (UInt64)itm.id;
                            break;
                        }
                    }
                    status = $"Success";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }
                return ret;
            }
        }

        public List<icedrive_folderinfo> get_archivefolders(long folderid, string parentfoldername)
        {
            status = "";
            var retlst = new List<icedrive_folderinfo>();
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    long brinfo = 0;
                    var items =  ice.ListItems(folderid);
                    foreach (var itm in items)
                    {
                        if (itm.isFolder == 1)
                        {
                            if (itm.filename == "backups")
                            {
                                retlst.Add (new icedrive_folderinfo { path = $"{parentfoldername}\\{itm.filename}", fldrid = (UInt64)folderid, brinfofid = (UInt64)brinfo, bkupfldrid = (UInt64)itm.id});
                            }
                            else
                            {
                                var templist =  get_archivefolders(itm.id, $"{parentfoldername}\\{itm.filename}");
                                retlst.AddRange(templist);
                            }
                        }
                        else if (itm.isFolder == 0)
                        {
                            if (itm.filename == "brinfoK.dat")
                                brinfo = itm.id;
                        }
                    }
                    status = $"Success";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }

            }
            return retlst;
        }

        public void get_archivefolders()
        {
            getuseridpasswd();

            if (archive_folder_list != null)
            {
                status = "Success";
                return;
            }
            
            archive_folder_list = get_archivefolders(385609617, "ice-drive\\2026");
            archive_folder_list = archive_folder_list.Where(itm => !itm.path.Contains("\\2024\\")).
                Select(itm=>new icedrive_folderinfo { path= itm.path.Replace("ice-drive\\", "").Replace("\\backups", ""),
                    fldrid= itm.fldrid,brinfofid = itm.brinfofid,bkupfldrid = itm.bkupfldrid } ).
                ToList();
        }

        public List<icedrive_fileinfo> get_archivefiles(long folderid, string parentfoldername)
        {
            var retlst = new List<icedrive_fileinfo>();
            status = "";
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    var items =  ice.ListItems(folderid);
                    foreach (var itm in items)
                    {

                        if (itm.isFolder == 1)
                        {
                            var templst =  get_archivefiles(itm.id, $"{parentfoldername}\\{itm.filename}");
                            retlst.AddRange(templst);
                        }
                        else
                            retlst.Add(new icedrive_fileinfo {filename= $"{parentfoldername}\\{itm.filename}", fileid = (ulong)itm.id});
                    }                      
                    status = $"Success";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }

            }
            return retlst;
        }


        public void get_archivefilelist()
        {
            getuseridpasswd();
            if (archive_fileslist != null)
            {
                status = "Success";
                return;
            }

            archive_fileslist = get_archivefiles((long)selarchive.bkupfldrid, "backups");
        }

        public void rename_filefolder(UInt64 fid, string target_filefolder)
        {
            getuseridpasswd();

            status = "";
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    var item = ice.GetItem((long)selarchive.fldrid, (long)fid);
                    if (!(item is null) && item.Count != 0)
                    {
                        bool success = ice.RenameItem(
                            item[0].id,
                            target_filefolder,
                            (item[0].isFolder == 0));

                        status = (!success) ? "ERROR: Failed to rename" : $"Success";
                    }
                    else
                        status = $"ERROR: Item not found";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }
            }
        }

        public void upload_file(UInt64 fid, string filename)
        {
            getuseridpasswd();

            status = "";
            using (IcedriveClient ice =
                new IcedriveClient())
            {
                try
                {
                    //var item = ice.GetItem((long)selarchive.fldrid, (long)fid);
                    //if (!(item is null) && item.Count != 0)
                    {
                        bool success = ice.UploadFile(
                            filename,
                            (long)fid);

                        status = (!success) ? "ERROR: Failed to upload" : $"Success";
                    }
                    //else
                      //  status = $"ERROR: Item not found";
                }
                catch (Exception ex)
                {
                    status = $"ERROR: {ex.ToString()}";
                }
            }
        }

        public void downloadbrinfo(string target_folder)
        {
            download_file(selarchive.brinfofid, target_folder);
        }

        public void updateselection(string foldername)
        {
            selarchive = null;
            foreach (var fldr in archive_folder_list)
            {
                if (foldername.Contains(fldr.path))
                {
                    selarchive = fldr;
                    archive_fileslist = null;
                    break;
                }
            }
        }

        public void uploadfilestocloud(string path)
        {
            var dir = App.archivedir + path;
            var parts = dir.Split('\\');
            var fid = find_filefolder(selarchive.fldrid, "brinfoK.dat.bak");
            if (fid != 0)
                delete_filefolder(fid);
            rename_filefolder(selarchive.brinfofid, "brinfoK.dat.bak");
            upload_file(selarchive.fldrid, App.archivedir + "\\brinfoK.dat"); 
            fid = find_filefolder(selarchive.fldrid, "brinfoK.dat");
            if (fid != 0)
                selarchive.brinfofid = fid;

            create_folder(selarchive.bkupfldrid, Path.GetFileName(dir));
            fid = find_filefolder(selarchive.bkupfldrid, Path.GetFileName(dir));
            foreach (var f in Directory.GetFiles(dir))
            {
                while (true)
                {
                    upload_file(fid, f);
                    var fid2 = find_filefolder(fid, Path.GetFileName(f));
                    if (fid2 != 0)
                        break;
                }

            }
            archive_fileslist = null;
            get_archivefilelist();
        }

    }

    public class Downloader
    {
        int retrycount = 0;
        string success = "Success";

        public ice_interface icedrive = new ice_interface();

        public void create_folder(UInt64 fid, string foldername)
        {
            retrycount = 0;
            do
            {
                App.logit($"create_folder() {foldername}...");
                icedrive.create_folder(fid, foldername);
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
        }

        public void delete_filefolder(UInt64 fid)
        {
            retrycount = 0;
            do
            {
                App.logit($"delete_filefolder() {fid}...");
                icedrive.delete_filefolder(fid);
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
        }

        public void download_file(UInt64 fid, string foldername)
        {
            retrycount = 0;
            do
            {
                App.logit("download_file() {foldername}...");
                icedrive.download_file(fid, foldername);
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
        }

        public List<icedrive_folderinfo> getarchiveinfo()
        {
            retrycount = 0;
            do
            {
                App.logit("get_archivefolders()...");
                icedrive.get_archivefolders();
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
            return icedrive.archive_folder_list;
        }

        public List<icedrive_fileinfo> getfilelist()
        {
            retrycount = 0;
            do
            {
                App.logit("get_archivefilelist()...");
                icedrive.get_archivefilelist();
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
            return icedrive.archive_fileslist;

        }

        public UInt64 find_filefolder(UInt64 fid, string filefoldername)
        {
            UInt64 ret;
            retrycount = 0;
            do
            {
                App.logit($"find_filefolder() {filefoldername}...");
                ret = icedrive.find_filefolder(fid, filefoldername);
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
            return ret;

        }

        public void rename_filefolder(UInt64 fid, string target_filefolder)
        {
            retrycount = 0;
            do
            {
                App.logit($"rename_filefolder() {target_filefolder}...");
                icedrive.upload_file(fid, target_filefolder);
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
        }

        public void upload_file(UInt64 fid, string filename)
        {
            retrycount = 0;
            do
            {
                App.logit($"upload_file()  {filename}...");
                icedrive.upload_file(fid, filename);
            } while (icedrive.status != success && retrycount++ < 3);
            App.logit(icedrive.status);
        }

        public void downloadbrinfo(string foldername)
        {
            icedrive.downloadbrinfo(foldername);
        }
        public void updateselection(string foldername)
        {
            icedrive.updateselection(foldername);
        }

        public void uploadfilestocloud(string path)
        {
            icedrive.uploadfilestocloud(path);
        }

    }
}
