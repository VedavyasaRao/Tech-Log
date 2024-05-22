using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using JSonSerializer;
using SetupLayout;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic;

namespace FileDeploymentWizard
{
    class Deployer
    {
        container co = null;
        storagenode node = null;
        string logfile = Program.workfolder+"\\"+"deployer.log";
        string errormsg = "success";



        class actiondata
        {
            public Customaction action;
            public List<Nodedata> nodes = new List<Nodedata>();
        }

        public Deployer(bool binstall)
        {
            logfile = Program.workfolder + "\\" + (binstall ? "deployer.log" : "undeployer.log");
            File.WriteAllText(logfile, "*********************************Starting*********************************\r\n");
        }

        private void AppendToLog(string msg)
        {
            File.AppendAllText(logfile, msg+"\r\n");
        }

        private void updatephysicalpath(storagenode node, string sourcepath)
        {
            AppendToLog(String.Format("updatephysicalpath({0},{1})", node.nodedata.name,sourcepath));
            try
            {
                if (node.children.Count > 0)
                {
                    foreach (storagenode sn in node.children)
                    {
                        string srcpath = sourcepath + sn.nodedata.name;
                        if (sn.children.Count == 0)
                        {
                            sn.nodedata.physicalpath = srcpath;
                        }
                        else
                        {
                            sn.nodedata.physicalpath = srcpath;
                            updatephysicalpath(sn, srcpath + "\\");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendToLog("Exception:" + ex.Message);
                errormsg = ex.Message;
                throw;
            }

        }

        private void copyfiles(storagenode node, string sourcepath, string targetpath)
        {
            try
            {
                AppendToLog(String.Format("copyfiles({0},{1},{2})", node.nodedata.name,sourcepath, targetpath));
                if (node.children.Count > 0)
                {
                    foreach (storagenode sn in node.children)
                    {
                        if (targetpath == "" && sn.nodedata.name == "Setup")
                            continue;

                        string srcpath = sourcepath + sn.nodedata.name;
                        string destpath = targetpath + Environment.ExpandEnvironmentVariables(sn.nodedata.name);

                        string dirs = destpath;
                        if (sn.children.Count == 0)
                        {
                            dirs = Path.GetDirectoryName(destpath);
                            Directory.CreateDirectory(dirs);
                            sn.nodedata.physicalpath = destpath;
                            AppendToLog(String.Format("copying {0} {1}", srcpath, destpath));
                            File.Copy(srcpath, destpath, true);
                        }
                        else
                        {
                            if (sn.nodedata.inscustomactions.ContainsKey(Customaction.CREATEFOLDER) && Directory.Exists(dirs))
                            {
                                AppendToLog(string.Format("deleting {0}", dirs));
                                Directory.Delete(dirs, true);
                            }
                            AppendToLog(string.Format("creating {0}", dirs));
                            Directory.CreateDirectory(dirs);
                            sn.nodedata.physicalpath = dirs;
                            copyfiles(sn, srcpath + "\\", destpath + "\\");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendToLog("Exception:" + ex.Message);
                errormsg = ex.Message;
                throw;
            }
        }

        private void getfilesforop(storagenode node, bool binstall, ref Dictionary<string, actiondata> actionsmap)
        {
            AppendToLog(String.Format("getfilesforop({0},{1})", node.nodedata.name, binstall));

            if (node.children.Count > 0)
            {
                foreach (storagenode sn in node.children)
                {
                    foreach (var ca in (binstall ? sn.nodedata.inscustomactions : sn.nodedata.uinscustomactions))
                    {
                        if (!actionsmap.ContainsKey(ca.Key))
                        {
                            actiondata ad = new actiondata();
                            ad.action = ca.Value;
                            ad.nodes.Add(sn.nodedata);
                            actionsmap.Add(ca.Key, ad);
                        }
                        else
                        {
                            actionsmap[ca.Key].nodes.Add(sn.nodedata);
                        }
                    }
                    if (sn.children.Count > 0)
                    {
                        getfilesforop(sn, binstall, ref actionsmap);
                    }
                }
            }
        }



        private string getexeforaction(Customaction action)
        {
            string exename = "";
            int osver = System.Environment.OSVersion.Version.Major;

            if (action.name == Customaction.REGSVR_32 || action.name == Customaction.UNREGSVR_32)
            {
                exename = Environment.ExpandEnvironmentVariables((osver > 5) ? @"%windir%\SysWOW64\regsvr32.exe" : @"%Windir%\system32\regsvr32.exe");
            }
            else if (action.name == Customaction.REGSVR_64 || action.name == Customaction.UNREGSVR_64)
            {
                exename = Environment.ExpandEnvironmentVariables((osver > 5) ? @"%windir%\system32\regsvr32.exe" : @"");
            }
            else if (action.name == Customaction.REGEDT_32)
            {
                exename = Environment.ExpandEnvironmentVariables((osver > 5) ? @"%windir%\SysWOW64\regedit.exe" : @"%Windir%\regedit.exe");
            }
            else if (action.name == Customaction.CSCRIPT_32)
            {
                exename = Environment.ExpandEnvironmentVariables((osver > 5) ? @"%windir%\SysWOW64\cscript.exe" : @"%Windir%\system32\cscript.exe");
            }
            else
                exename = Environment.ExpandEnvironmentVariables(action.executable);

            return exename;
        }


        public void GetSettings()
        {
            Process p = new Process();
            try
            {
                string layoutfile = Program.workfolder + "\\setup\\installfiles.layout";
                co = layout.getcontainer(layoutfile);
                if (co.bask && Program.uilevel == 5)
                {
                    string tempfile = Program.workfolder + "\\temp.settings";
                    AppendToLog("GetSettings()");
                    JSONPersister<Dictionary<string, string>>.Write(tempfile, co.dirmapinfo);
                    p.StartInfo.FileName = Program.workfolder + "\\setup\\ConfigEditor.exe";
                    p.StartInfo.Arguments = "-f:\"" + tempfile + "\" -t:\"Update Target Directories\"";
                    p.Start();
                    p.WaitForExit();
                    co.dirmapinfo = JSONPersister<Dictionary<string, string>>.Read(tempfile);
                    layout.write(layoutfile, co, layout.getstoragenode(layoutfile));
                }
            }
            catch (Exception ex)
            {
                AppendToLog("Exception:"+ex.Message);
                errormsg = ex.Message;
                throw;
            }
        }


        public void Createenvvars()
        {
            if (co == null)
            {
                string layoutfile = Program.workfolder + "\\setup\\installfiles.layout";
                co = layout.getcontainer(layoutfile);
            }

            var settings = co.dirmapinfo;
            Environment.SetEnvironmentVariable("InstallDir", Program.workfolder);
            Environment.SetEnvironmentVariable("InstallAppDir", Directory.GetParent(Program.workfolder).Parent.FullName);
            foreach (var s in settings)
                Environment.SetEnvironmentVariable(s.Key, Environment.ExpandEnvironmentVariables(s.Value));
        }


        public void runprepostrunexe(bool bpre, bool binstall)
        {
            try
            {
                AppendToLog(string.Format("runprepostrunexe({0})",bpre));
                string layoutfile = Program.workfolder + "\\setup\\installfiles.layout";
                co = layout.getcontainer(layoutfile);
                string si = bpre ? co.preinstall : co.postinstall;
                string su = bpre ? co.preuninstall : co.postuninstall;
                string s = binstall ? si : su;
                if (s == "")
                    return;
                StringBuilder sb = new StringBuilder();
                FileInfo fi = new FileInfo(s);
                Process p = new Process();
                p.StartInfo.FileName = '"' + Program.workfolder + "\\setup\\" + fi.Name + '"';
                p.StartInfo.Arguments = Environment.ExpandEnvironmentVariables((bpre ? co.preinstallargs : co.postinstallargs));
                p.StartInfo.WorkingDirectory = '"' + Program.workfolder + "\\setup" + '"';

                sb.AppendFormat("Filename:{0}", p.StartInfo.FileName);
                sb.AppendLine();
                sb.AppendFormat("Args:{0}", p.StartInfo.Arguments);
                sb.AppendLine();
                sb.AppendFormat("WorkingDirectory:{0}", p.StartInfo.WorkingDirectory);
                sb.AppendLine();
                AppendToLog(sb.ToString());
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                AppendToLog("Exception:" + ex.Message);
                errormsg = ex.Message;
                throw;
            }

        }

        public void copyfiles()
        {
            try
            {
                AppendToLog("copyfiles()");
                string layoutfile = Program.workfolder + "\\setup\\installfiles.layout";
                node = layout.getstoragenode(layoutfile);
                copyfiles(node, Program.workfolder + "\\", "");
                foreach (storagenode sn in node.children)
                {
                    if (sn.nodedata.name == "Setup")
                        updatephysicalpath(sn, Program.workfolder + "\\Setup\\");
                }
                layout.write(layoutfile,co, node);
            }
            catch (Exception ex)
            {
                AppendToLog("Exception:" + ex.Message);
                errormsg = ex.Message;
                throw;
            }

        }



        private void performfileop(actiondata ad, bool binstall)
        {
            try
            {
                AppendToLog(string.Format("performfileop({0})", ad.action.name));
                if (ad.action.name == Customaction.CREATEFOLDER)
                    return;

                if (ad.action.name == Customaction.REMOVEFOLDER)
                {
                    string dirpath=ad.nodes[0].physicalpath;
                    if (Directory.Exists(dirpath))
                    {
                        Directory.Delete(dirpath, true);
                        return;
                    }
                }   

                Process p = new Process();
                p.StartInfo.FileName = getexeforaction(ad.action);
                AppendToLog(string.Format("{0}", p.StartInfo.FileName));

                if (ad.action.grpaction)
                {
                    string files = "";
                    foreach (var n in ad.nodes)
                    {
                        files = files + " " + '"' + n.physicalpath + '"';
                    }

                    if (Program.uilevel != 5 && ad.action.name == Customaction.CONFIG_EDIT)
                        ad.action.args = "-s " + ad.action.args;
                    p.StartInfo.Arguments = ad.action.args +"  "+files;
                    AppendToLog(string.Format("Args:{0}", p.StartInfo.Arguments));
                    if (ad.action.name != Customaction.CONFIG_EDIT)
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                    if (ad.action.wfe)
                        p.WaitForExit();
                }
                else
                {
                    foreach (var n in ad.nodes)
                    {
                        foreach (var a in binstall ? n.inscustomactions : n.uinscustomactions)
                        {
                            if (a.Key != ad.action.name)
                                continue;
                            p.StartInfo.Arguments = a.Value.args + "  "+'"' + n.physicalpath + '"';
                            AppendToLog(string.Format("Args:{0}", p.StartInfo.Arguments));
                            if (ad.action.name != Customaction.CONFIG_EDIT)
                                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            p.Start();
                            if (ad.action.wfe)
                                p.WaitForExit();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendToLog("Exception:" + ex.Message);
                errormsg = ex.Message;
                throw;
            }
        }


        public void performfileops(bool binstall)
        {
            AppendToLog(string.Format("performfileops({0})",binstall));
            string layoutfile = Program.workfolder + "\\setup\\installfiles.layout";
            node = layout.getstoragenode(layoutfile);

            Dictionary<string, actiondata> actionsmap = new Dictionary<string, actiondata>();
            getfilesforop(node, binstall, ref actionsmap);
            for (int x = 0; x < 5; ++x)
            {
                foreach (var kv in actionsmap)
                {
                    if (kv.Value.action.priority == x)
                    {
                        performfileop(kv.Value, binstall);
                    }

                }
            }
        }
    }
}
