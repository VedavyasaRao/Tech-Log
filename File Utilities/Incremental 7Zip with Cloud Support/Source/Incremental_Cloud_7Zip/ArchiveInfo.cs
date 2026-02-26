using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;

namespace BackupRestoreTool
{
    [Serializable]
    public class ArchiveInfo
    {
        public string name;
        public string desc;
        public List<BackupInfo> backups = new List<BackupInfo>();
        public archive_location arch_loc = archive_location.harddisk;
        public long archive_size = 4096;

        [field: NonSerialized]
        public List<KeyValuePair<string,bool>> archiveddirs = new List<KeyValuePair<string, bool>>();

        [field: NonSerialized]
        public List<fileitem> nodes = new List<fileitem>();

        public enum archive_location { harddisk=-0,cloud=1 };
        public List<string> selecteddirs()
        {
            return (from kv in App.arm.selarchive.archiveddirs where kv.Value == true select kv.Key).ToList();
        }
        public bool cleanup()
        {
            int i = 0;
            bool changed = false;
            while(i < backups.Count)
            {
                if (!DirectoryEx.Exists(App.archivedir + "\\backups\\" + backups[i].name))
                {
                    backups.RemoveAt(i);
                    changed = true;
                    i = 0;
                }
                else
                    ++i;
            }
            return (changed);
        }

        public void PopulateAllFileItemsFromBackup(BackupInfo bkup)
        {
            int k = 0;
            var tempdict = new Dictionary<Guid, basicfileitem>[backups.Count()];
            foreach (var bk in backups)
            {
                tempdict[k++] = bk.backup_data_write.ToDictionary(bfi => bfi._id);
                if (bk.name == bkup.name)
                    break;
            }

            bkup.backup_data = new List<fileitem>();
            foreach (var g in bkup.backup_guid_write)
            {
                for (var i = 0; i < k; ++i)
                {
                    if (tempdict[i].ContainsKey(g))
                    {
                        bkup.backup_data.Add(new fileitem((tempdict[i])[g]));
                        break;
                    }
                }
            }

        }

        public string changepath(string path, bool dir)
        {
            string ret = "";
            var dirs = archiveddirs.Select(d => d.Key).ToList();
            foreach (var d in dirs)
            {
                if (path.StartsWith(d + ((!dir) ? "\\" : "")))
                {
                    var tempdir = d.Replace(':', '_').Replace('\\', '_');
                    ret = path.Replace(d + ((!dir) ? "\\" : ""), tempdir + ((!dir) ? "\\" : ""));
                    break;
                }
            }
            return ret;
        }
        public void preparetounzip(string filelist, bool src)
        {
            var filetext = FileEx.ReadAllText(filelist);
            var dirs = archiveddirs.Select(d => d.Key).ToList();
            foreach (var d in dirs)
            {
                var tempdir = d.Replace(':', '_').Replace('\\', '_');
                if (src)
                    filetext = filetext.Replace(d + "\\", tempdir + "\\");
                else
                    filetext = filetext.Replace(tempdir + "\\", d + "\\");
            }
            FileEx.WriteAllText(filelist, filetext);
        }

        public void PopulateFileItemsFromAllBackups()
        {
            foreach (var bk in backups)
            {
                bk.PopulateFileItemsFromBackup();
            }
        }

        public void UpdateMaps()
        {
            foreach (var bk in backups)
            {
                bk.UpdateMaps();
            }

        }

        public BackupInfo find(string name)
        {
            return (from bk in backups where bk.name == name select bk).ToArray()[0];
        }


        public ArchiveInfo() { }


        void GetFolderInfo(object parentpath)
        {
            try
            {
                App.logit("Collecting data .... please wait");
                App.logit("Source folder" + parentpath);
                var outputfile = App.outputpath + "\\targetfile.txt";

                if (FileEx.Exists(outputfile))
                    FileEx.Delete(outputfile);
                App.arm.statp();
                string args = "";
                args = "/c chcp 65001 & echo getting file info  & (dir /s /b  /A-D  \"" + parentpath + "\" >> \"" + outputfile + "\")";
                var ps = new ProcessStartInfo("cmd", args);
                var p = System.Diagnostics.Process.Start(ps);
                p.WaitForExit();
                var temp = FileEx.ReadAllText(outputfile);
                //FileEx.WriteAllText(outputfile, temp.Replace('?', 'Ø').Replace('"', 'Ø').Replace('\v', 'Ø');
                List<char> invalidPathChars = System.IO.Path.GetInvalidPathChars().ToList();
                invalidPathChars.Add('?');
                invalidPathChars.Remove('\r');
                invalidPathChars.Remove('\n');
                foreach (var ch in invalidPathChars)
                {
                    temp = temp.Replace(ch, 'Ø');
                }
                FileEx.WriteAllText(outputfile, temp);
                App.logit("Collecting data .... done");
                App.arm.stopp();
            }
            catch (Exception ex)
            {
                App.logit(ex.Message);
            }
        }


        void Loadfileitms(string parentpath, ref fileitem treenode)
        {
            string aline = "";
            if (App.arm.isfile(parentpath))
            {
                treenode = new fileitem { _title = System.IO.Path.GetFileName(parentpath), _fullPath = parentpath, isfile = true, _parent = null };
                return;
            }

            fileitem dummy = new fileitem { _title = "dummy" };
            treenode = new fileitem { _title = parentpath, _fullPath = parentpath, _parent = null };
            treenode.Items.Add(dummy);
            string outputfile = App.outputpath + "\\targetfile.txt";

            App.logit("Creating file items .... please wait");
            Dictionary<string, fileitem> parentnodes = new Dictionary<string, fileitem>();
            String[] lines = FileEx.ReadAllLines(outputfile, System.Text.Encoding.GetEncoding(65001/*437*/));
            lines = lines.Distinct().ToArray();
            App.pmon.initpbar(lines.Length);
            for (long k = 0; k < lines.Length; ++k)
            {
                try
                {
                    aline = lines[k].Replace(parentpath+"\\","");

                    var fic = new fileitem { _title = System.IO.Path.GetFileName(aline), _fullPath = aline, isfile = true };
                    var parts = fic._fullPath.Replace(aline + "\\", "").Split(new char[] { '\\' });
                    var ppn = treenode;
                    var pp = treenode._fullPath;
                    for (var i = 0; i < parts.Length - 1; ++i)
                    {
                        pp += "\\" + parts[i];
                        if (!parentnodes.ContainsKey(pp))
                        {
                            var tempnode = new fileitem { _title = parts[i], _fullPath = pp, _parent = ppn, isfile=false };
                            tempnode.Items.Add(dummy);
                            ppn._Items.Add(tempnode);
                            parentnodes.Add(pp, tempnode);
                        }
                        ppn = parentnodes[pp];
                    }
                    ppn._Items.Add(fic);
                    fic._fullPath = lines[k];
                    fic._parent = ppn;
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message+"  "+aline);
                }
                App.pmon.updatepbar();
            }
            App.pmon.closebar();
        }

        void Updateizedom(List<fileitem> leaves)
        {
            App.logit("Updating file items size, dom .... please wait");
            App.pmon.initpbar(leaves.Count);
            foreach (var fi in leaves)
            {
                double sz;
                try
                {
                    System.IO.FileInfo finf = FileInfoEx.FileInfo(fi._fullPath);
                    sz = finf.Length;
                    fi._dateupdated = finf.LastWriteTime.ToFileTime();
                    fileitem fic = fi;
                    while (fic != null)
                    {
                        fic._status = "New";
                        fic._size += sz;
                        if (!fic.isfile)
                            ++fic._count;
                        fic = fic._parent;
                    }
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(fi.FullPath);
                }

                App.pmon.updatepbar();
            }
            App.logit("Updating file items size, dom .... done");

        }
        void Updateparentssize(List<fileitem> leaves)
        {
            App.logit("Updating parent file items size .... please wait");
            App.pmon.initpbar(leaves.Count);
            foreach (var fi in leaves)
            {
                double sz;
                try
                {
                    sz = fi._size;
                    fileitem fic = fi._parent;
                    while (fic != null)
                    {
                        fic._size += sz;
                        ++fic._count;
                        fic = fic._parent;
                    }
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(fi.FullPath);
                }

                App.pmon.updatepbar();
            }
            App.logit("Updating parent file items size .... done");

        }


        void updatemd2(List<fileitem> leaves)
        {
            try
            {
                MD5Util md5util = new MD5Util();
                App.logit("Updating md2 of leaves  .... please wait");
                App.pmon.initpbar(leaves.Count);
                var ficlist = (from fic in leaves where fic._crc == "" select fic).ToList();
                md5util.threadpool(new object[] { ficlist }, md5util.CalculateMD5);
                App.logit("Updating md5 of leaves  .... done");
            }
            catch (Exception ex)
            {
                App.logit(ex.Message);
            }
        }

        void updatearchivestatus(List<fileitem> leaves)
        {
            try
            {
                List<fileitem> archivedlist = new List<fileitem>(leaves.Count * backups.Count);
                for (int i = 0; i < archivedlist.Capacity; ++i) archivedlist.Add(null);

                MD5Util md5util = new MD5Util();
                App.logit("Updating archived status of leaves  .... please wait");
                App.pmon.initpbar(leaves.Count);
                md5util.threadpool(new object[] { leaves, archivedlist , backups }, md5util.updatelave);
                App.logit("Updating archived status of leaves  .... done");

                archivedlist.RemoveRange(md5util.stidx+1, archivedlist.Count - md5util.stidx-1);
                ConcurrentDictionary<string,byte> processedlist = new ConcurrentDictionary<string,byte>(12,leaves.Count);
                md5util = new MD5Util();
                App.logit("Updating archived status of parents  .... please wait");
                App.pmon.initpbar(archivedlist.Count);
                md5util.threadpool(new object[] { archivedlist, processedlist }, md5util.updatefolder);
                App.logit("Updating archived status of parents  .... done");

            }

            catch (Exception ex)
            {
                App.logit(ex.Message);
            }
        }

        void updatecrcloaded(List<fileitem> leaves)
        {
            try
            {
                App.logit("Updating crc of existing leaves  .... please wait");
                List<fileitem> archivedlist = new List<fileitem>();
                App.pmon.initpbar(leaves.Count);
                foreach (var fic in leaves)
                {
                    try 
                    {
                        foreach (var bkup in backups)
                        {
                            fileitem temp;
                            if (bkup.nameficdic.TryGetValue(fic._fullPath, out temp))
                            {
                                if (fic._size == temp._size && fic._dateupdated == temp._dateupdated)
                                {
                                    fic._crc = temp._crc;
                                    fic._id = temp._id;
                                    fic.barchived = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        App.logit(ex.Message);
                        App.logit(fic._fullPath);
                    }
                    App.pmon.updatepbar();

                }
                App.logit("Updating archived crc of existing leaves .... done");
            }

            catch (Exception ex)
            {
                App.logit(ex.Message);
            }
        }

        public void CorrectFilenames(List<fileitem> leaves)
        {
            App.pmon.busycursor();
            App.logit("Correcting filenames  .... please wait");
            List<fileitem> ucodenodes = (from fic in leaves where !FileEx.Exists(fic._fullPath) select fic).ToList();

            foreach (var fic in ucodenodes)
            {
                List<fileitem> chain = new List<fileitem>();
                chain.Add(fic);
                var fic2 = fic._parent;
                while (fic2 != null)
                {
                    chain.Add(fic2);
                    fic2 = fic2._parent;
                }
                chain.Reverse();
                foreach (var fic3 in chain)
                {
                    bool found = (fic3.isfile) ? FileEx.Exists(fic3._fullPath) : DirectoryEx.Exists(fic3._fullPath);
                    if (!found)
                    {
                        var parent = fic3._parent;
                        var fsentreis = DirectoryEx.GetFiles(parent._fullPath).ToList();
                        fsentreis.AddRange(DirectoryEx.GetDirectories(parent._fullPath).ToList());
                        int idx = parent._Items.IndexOf(fic3);
                        if (idx < fsentreis.Count)
                        {
                            fic3._fullPath = fsentreis[idx];
                            fic3._title = (fic3.isfile) ? FileInfoEx.FileInfo(fic3._fullPath).Name : DirectoryInfoEx.DirectoryInfo(fic3._fullPath).Name;
                        }
                    }
                }
            }
            App.logit("Correcting filenames  .... done");
        }

        void removeleavenode(fileitem tnd)
        {
            var parent = tnd._parent;
            if (parent == null)
                return;
            foreach (var fi in parent._Items)
            {
                if (fi._fullPath.ToUpper() == tnd._fullPath.ToUpper())
                {
                    parent._Items.Remove(fi);
                    break;
                }
            }
            if (parent._Items.Count==0)
            {
                removeleavenode(parent);
            }
        }

        bool removetreenode(string parentpath, fileitem tnd)
        {
            if (tnd._fullPath.ToUpper() == parentpath)
            {
                tnd._parent._Items.Remove(tnd);
                return true;
            }
            else
            {
                foreach (var fi in tnd._Items)
                    if (fi._Items.Count > 0)
                        if (parentpath.Contains(fi._fullPath.ToUpper()))
                            if (removetreenode(parentpath, fi))
                                return true;
            }
            return false;
        }
        void removeskippedfolders(string parentpath, List<fileitem> leaves,fileitem tnd)
        {
            string skipdirfile = App.archivedir + "\\exclude.txt";
            if (!FileEx.Exists(skipdirfile))
                return;

            var lines = new List<string>(FileEx.ReadAllLines(skipdirfile, System.Text.Encoding.GetEncoding(65001/*437*/)));
            foreach (var l in lines)
            {
                if (!l.ToUpper().Contains(parentpath))
                    continue;
                leaves.RemoveAll(x => x._fullPath.ToUpper().Contains(l.ToUpper()));
                removetreenode(l.ToUpper(), tnd);
            }
        }
        public void togglearchivednodes(bool bshow, string sts )
        {
            if (!bshow)
            {
                nodes = (List<fileitem>)App.arm.Deserialize(App.arm.Khrishadat,false);
                return;
            }

            foreach (var treenode in nodes)
            {
                List<fileitem> leaves = new List<fileitem>();
                treenode.getleaves(ref leaves);

                var temp = leaves.Where((fi) => fi.Status == sts);
                foreach (var fi in temp)
                {
                    removeleavenode(fi);
                }
            }
        }

        public void LoadArchiveForBackup()
        {
            nodes = new List<fileitem>();
            nodes.Add(new fileitem { _title = "Files", _status = "Status", _size = -1, _count = -1 });
            foreach (var parentpath in selecteddirs())
            {
                if (!DirectoryEx.Exists(parentpath))
                    continue;
                fileitem treenode = null;
                GetFolderInfo(parentpath);
                Loadfileitms(parentpath, ref treenode);
                List<fileitem> leaves = new List<fileitem>();
                treenode.getleaves(ref leaves);
                CorrectFilenames(leaves);
                removeskippedfolders(parentpath.ToUpper(), leaves,treenode);
                Updateizedom(leaves);
                updatecrcloaded(leaves);
                updatemd2(leaves);
                updatearchivestatus(leaves);
                nodes.Add(treenode);
            }
            App.arm.Serialize(nodes, ref App.arm.Khrishadat);
        }
        

        private fileitem populatenode(BackupInfo bkup, string parentpath)
        {
            char[] sep = new char[] { '\\' };
            var treemap = new Dictionary<string, fileitem>();
            List<fileitem> leaves = (from fi in bkup.backup_data where fi._fullPath.Contains(parentpath+"\\") select fi).ToList();

            if (leaves.Count == 0)
                return null;
            List<string> dirnames = (from fi in leaves select System.IO.Path.GetDirectoryName(fi._fullPath)).Distinct().ToList();

            treemap.Add(parentpath, new fileitem { _title = parentpath,_fullPath=  parentpath, _parent = null });
            string parentpath2 = parentpath + (parentpath.EndsWith("\\")?"":"\\");

            foreach (var dn in dirnames)
            {
                if (dn == parentpath)
                    continue;
                var parts = dn.Replace(parentpath2, "").Split(sep);
                string ptrpth = parentpath;
                foreach (var prt in parts)
                {
                    ptrpth = ptrpth+(ptrpth.EndsWith("\\") ? "" : "\\") + prt;
                    if (!treemap.ContainsKey(ptrpth))
                    {
                        treemap.Add(ptrpth, new fileitem { _title = prt, _fullPath=ptrpth, _parent = null });
                    }
                }
            }

            foreach (var dn in treemap.Keys)
            {
                if (dn == parentpath)
                    continue;
                var prt = treemap[System.IO.Path.GetDirectoryName(dn)];
                var chd = treemap[dn];
                chd._parent = prt;
                prt._Items.Add(chd);
                prt.Items.Add(chd);
            }

            foreach(var fi in leaves)
            {
                var prt = treemap[System.IO.Path.GetDirectoryName(fi._fullPath)]; 
                fi._selected = false;
                fi._parent = prt;
                prt._Items.Add(fi);
                prt.Items.Add(fi);

            }

            return treemap[parentpath];
        }

        private void populatestatus(List<fileitem> leaves, List<fileitem> livefis, bool syncfldrs)
        {
            App.logit("Updating archived status of leaves  .... please wait");
            App.pmon.initpbar(leaves.Count);
            foreach (var fi in leaves)
            {
                fi._selected = false;
                if (syncfldrs && !FileEx.Exists(fi._fullPath))
                {
                    fi._status = "Deleted";
                }
                else
                {
                    if (livefis.Exists(fic => ((fi._crc == fic._crc))))
                    {
                        fi._status = "Same";
                    }
                    else 
                    {
                        fi._status = livefis.Count>0?"Changed": "Archived";
                    }
                }
                App.pmon.updatepbar();
            }

            App.pmon.closebar();
            App.logit("Updating archived status of leaves  .... done");

            App.logit("Updating archived status of parents  .... please wait");
            App.pmon.initpbar(leaves.Count);
            foreach (var fi in leaves)
            {
                try
                {
                    fileitem fic = fi._parent;
                    while (fic != null)
                    {
                        if (fic._status != "Same" && fic._status != "Changed" && fic._status != "Deleted" && fic._status != "Archived")
                        {
                            fic._selected = false;
                            List<fileitem> archivedleaves = new List<fileitem>();
                            fic.getleaves(ref archivedleaves);
                            //fic._count = leaves.Count;
                            var cnt = (from fiarch in archivedleaves where fiarch._status == "Same" select fiarch).Count();
                            if (cnt == archivedleaves.Count)
                            {
                                fic._status = "Same";
                            }
                            else
                            {
                                cnt = (from fiarch in archivedleaves where fiarch._status == "Archived" select fiarch).Count();
                                if (cnt == archivedleaves.Count)
                                {
                                    fic._status = "Archived";
                                }
                                else
                                {
                                    cnt = (from fiarch in archivedleaves where fiarch._status == "Deleted" select fiarch).Count();
                                    if (cnt == archivedleaves.Count)
                                        fic._status = "Deleted";
                                    else
                                        fic._status = "Changed";
                                }
                            }
                        }
                        fic = fic._parent;
                    }
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(fi._fullPath);
                }
                App.pmon.updatepbar();
            }
            App.pmon.closebar();
            App.logit("Updating archived status of parents  .... done");
        }


        public void LoadBackupForRestore(BackupInfo bkup, bool syncfldrs, bool changesonly)
        {
            if (changesonly)
            {
                bkup.PopulateFileItemsFromBackup();
            }
            else
            {
                PopulateAllFileItemsFromBackup(bkup);
            }
            UpdateMaps();
            nodes = new List<fileitem>();
            App.logit("Loading nodes  .... please wait");
            nodes.Add(new fileitem { _title = "Files", _status = "Status", _size = -1, _count = -1 });
            foreach (var parentpath in bkup.dirs)
            {
                var treenode = populatenode(bkup, parentpath);
                if (treenode != null)
                    nodes.Add(treenode);
            }
            App.logit("Loading nodes  ....  done");


            foreach (var node in nodes)
            {
                if (node._status == "Status")
                    continue;

                List<fileitem> leaves = new List<fileitem>();
                node.getleaves(ref leaves);

                List<fileitem> livefis = new List<fileitem>();
                if (syncfldrs)
                {
                    App.logit("getting non deleted leaves.... please wait");
                    livefis = leaves.FindAll(fi => (FileEx.Exists(fi._fullPath)));
                    App.logit("getting non deleted leaves.... done");
                }
                else
                {
                    foreach (var l in leaves)
                        l.barchived = true;
                }
                App.arm.Serialize(livefis, ref App.arm.Testdat);
                livefis = (List<fileitem>)App.arm.Deserialize(App.arm.Testdat,true);
                updatecrcloaded(livefis);
                
                App.logit("Updating md5 of leaves  .... please wait");
                App.pmon.initpbar(livefis.Count);
                MD5Util md5util = new MD5Util();
                foreach (var fi in livefis)
                {
                    fi._crc = "";
                }
                md5util.threadpool(new object[] { livefis }, md5util.CalculateMD5);
                App.logit("Updating md5 of leaves  .... done");
                Updateparentssize(leaves);
                populatestatus(leaves, livefis, syncfldrs);
            }
            App.arm.Serialize(nodes, ref App.arm.Khrishadat);

        }
        public fileitem AddNode(string parentpath, string selpath, ref List<Tuple<string,string>> locmap, List<string> subitems)
        {
            fileitem treenode = null;
            if (!App.arm.isfile(parentpath))
                GetFolderInfo(parentpath);
            Loadfileitms(parentpath, ref treenode);
            List<fileitem> leaves = new List<fileitem>();
            treenode.getleaves(ref leaves);
            CorrectFilenames(leaves);
            if (subitems.Count > 0)
            {
                leaves = (from l in leaves where subitems.Contains(l._fullPath) select l).ToList();
                treenode._Items = leaves;
            }
            removeskippedfolders(parentpath.ToUpper(), leaves, treenode);
            Updateizedom(leaves);
            updatecrcloaded(leaves);
            updatemd2(leaves);
            updatearchivestatus(leaves);
            var patha = parentpath.Substring(0, parentpath.LastIndexOf('\\'));
            treenode._title = treenode._title.Replace(patha + "\\", "");
            if (subitems.Count > 0)
            {
                patha = parentpath;
            }
            foreach (var l in leaves)
            {
                var temps = l._fullPath.Replace(patha, selpath);
                locmap.Add(new Tuple<string,string>(temps, l._fullPath));
                l._fullPath = temps;
            }
            return treenode;
        }

        public fileitem AddNode(string[] parentpath, string selpath, ref List<Tuple<string, string>> locmap)
        {
            var parentnode = new fileitem { _title = "temp", _fullPath = "temp", isfile = false, _parent = null };
            var parentdirs = (from pp in parentpath where (App.arm.isfile(pp)) select pp).ToLookup(pp => System.IO.Path.GetDirectoryName(pp));

            foreach (var pth in parentpath)
            {
                if ((App.arm.isfile(pth)))
                    continue;
                var tempnode = AddNode(pth, selpath, ref locmap, new List<String>());
                tempnode._parent = parentnode;
                parentnode._Items.Add(tempnode);
            }

            foreach (var pp in parentdirs.Select(g => g.Key).ToList())
            {
                var tempnode = AddNode(pp, selpath, ref locmap, parentdirs[pp].ToList());
                tempnode._parent = parentnode;
                parentnode._Items.AddRange(tempnode._Items);

            }
            return parentnode;
        }
        public void Zip_files()
        {
            App.logit("Gathering data to save...");
            BackupInfo bkup = new BackupInfo();
            List<fileitem> allfis = new List<fileitem>();
            foreach (var node in nodes)
            {
                try
                {
                    if (node._status == "Status")
                        continue;
                    fileitem parentfi = (fileitem)node;
                    App.logit("Adding leaves from  " + parentfi._fullPath + "  ....Please wait");
                    parentfi.getleaves(ref allfis);
                    bkup.dirs.Add(node._fullPath);
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(((fileitem)node)._fullPath);
                }
            }

            bkup.backup_data = allfis.FindAll(fi => (fi.Selected)).ToList();
            bkup.UpdateMaps();
            
            App.logit("Updating archived status of nodes   ....Please wait");
            try
            {
                foreach (var md2fic in bkup.md2ficdic)
                {
                    foreach (var fi in md2fic)
                    {
                        fi._status = "Archived";
                        if (md2fic.Count()>1)
                            fi.duplicatefile = md2fic.ElementAt(0)._fullPath;
                    }
                }
            }
            catch (Exception ex)
            {
                App.logit(ex.Message);
            }
            App.logit("Updating archived status of nodes   ....done");

            var bret = bkup.Zip_files(name);
            if (bret)
            {
                bkup.backup_data = (allfis.FindAll(fi => (fi._status == "Archived"))).ToList();
                string backuproot = "\\backups\\" + bkup.name;
                backups.Add(bkup);
                foreach (var node in nodes)
                {
                    List<fileitem> leaves = new List<fileitem>();
                    node.getleaves(ref leaves);
                    updatearchivestatus(leaves);
                }
                bkup.UpdateMaps();
                bkup.backup_guid_write = bkup.backup_data.Select(fi => fi._id).ToList();
                bkup.backup_data_write = (from fi in bkup.backup_data where !fi.barchived select new basicfileitem(fi)).ToList();
                App.arm.Persist(this);
                if (App.isclouddrive())
                    App.arm.uploadfilestocloud(backuproot);
            }
        }

        public void Export_files(string filename)
        {
            App.logit("Gathering data to export...");
            List<fileitem> allfis = new List<fileitem>();
            foreach (var node in nodes)
            {
                try
                {
                    if (node._status == "Status")
                        continue;
                    fileitem parentfi = (fileitem)node;
                    App.logit("Adding leaves from  " + parentfi._fullPath + "  ....Please wait");
                    parentfi.getleaves(ref allfis);
                }
                catch (Exception ex)
                {
                    App.logit(ex.Message);
                    App.logit(((fileitem)node)._fullPath);
                }
            }

            System.IO.File.WriteAllLines(filename,allfis.FindAll(fi => (fi.Selected)).ToList().Select(ff=>ff._fullPath));
        }
    }
}
