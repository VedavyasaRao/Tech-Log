using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SetupLayout;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace InstallerCreator
{
    class MakeMSIHelper
    {
        string layoutfile;
        string stagefolder;
        string outputfolder;
        string zipfile;
        string msifile;
        string mkzipfile;
        public string errmsg = "";

        private void copyfiles(storagenode node, string fullpath)
        {
            System.Windows.Forms.Application.DoEvents();
            string path = fullpath + "\\" + node.nodedata.name;
            if (node.children.Count > 0)
            {
                Program.UpdateStatus("creating dir:"+ path);
                Directory.CreateDirectory(path);
                foreach (storagenode sn in node.children)
                {
                    copyfiles(sn, path);
                }
            }
            else
            {
                Program.UpdateStatus("copying file "+ path);
                File.Copy(node.nodedata.physicalpath, path, true);
            }
        }

        private void Copyfiles()
        {
            DirectoryInfo dir = new DirectoryInfo(stagefolder);
            if (dir.Exists)
                dir.Delete(true);
            dir.Create();

            storagenode node = layout.getstoragenode(layoutfile);
            copyfiles(node, dir.FullName);

            string setupdir = dir.FullName + "\\Root\\setup";
            Directory.CreateDirectory(setupdir);
            File.Copy(layoutfile, setupdir + "\\installfiles.layout", true);
            container co = layout.getcontainer(layoutfile);
            Dictionary<string, string> dirmap = co.dirmapinfo;
            for (int i = 0; i < 4; ++i)
            {
                string sifile = (i % 2 == 0 ? co.preinstall : co.postinstall);
                string suifile = (i % 2 == 0 ? co.preuninstall : co.postuninstall);
                string sfile = (i < 2) ? sifile : suifile;
                if (sfile == "")
                    continue;

                FileInfo fi = new FileInfo(sfile);
                if (!fi.Exists)
                    Program.UpdateStatus(String.Format("File doesnot exist:{0}", fi.FullName));
                else
                    File.Copy(fi.FullName, setupdir + "\\" + fi.Name);
            }


        }
        
        private void Makezip()
        {
            Program.UpdateStatus("Creating zip file. Please wait...");
            File.WriteAllBytes(zipfile, Resource1.dummy);

            File.WriteAllText(mkzipfile, Resource1.makezip);
            Process p = new Process();
            p.StartInfo.FileName = mkzipfile;
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", stagefolder + "\\root", zipfile);
            p.Start();
            p.WaitForExit();
        }

        private void CreateMSI()
        {
            container co = layout.getcontainer(layoutfile);

            Program.UpdateStatus("Creating installer file. Please wait...");
            string dumpfolder = outputfolder + "\\dump";
            string tempmsifile = outputfolder + "\\IntegratedTests.msi";
            string vbsWiMakCabfile = outputfolder + "\\WiMakCab.vbs";
            string vbsWiRunSQLfile = outputfolder + "\\WiRunSQL.vbs";
            string vbsWiSumInffile = outputfolder + "\\WiSumInf.vbs";
            string vbsUpdateDBfile = outputfolder + "\\UpdateDB.vbs";

            Directory.CreateDirectory(dumpfolder);
            File.WriteAllBytes(tempmsifile, Resource1.EasyInstaller);
            Process p = new Process();
            p.StartInfo.FileName = "msiexec.exe";
            p.StartInfo.Arguments = string.Format("/a \"{0}\" TARGETDIR=\"{1}\" /quiet", tempmsifile, dumpfolder);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            tempmsifile = dumpfolder + "\\IntegratedTests.msi";
            File.Copy(zipfile, dumpfolder + "\\setup\\fileinstall.zip", true);
            File.WriteAllText(vbsWiMakCabfile,Resource1.WiMakCab);
            File.WriteAllText(vbsWiRunSQLfile, Resource1.WiRunSQL);
            File.WriteAllText(vbsWiSumInffile, Resource1.WiSumInf);
            File.WriteAllText(vbsUpdateDBfile, Resource1.UpdateDB);
           
            string curdir = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" {4}", dumpfolder, "CScript.exe", vbsWiMakCabfile, tempmsifile, "Data1 /C /U /E /S");
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            string sqlstr = "";
            foreach (var prop in co.propmapinfo.Keys)
            {
                if (string.IsNullOrEmpty(co.propmapinfo[prop]))
                    continue;

                sqlstr = string.Format("update Property set Value = '{0}' WHERE Property = '{1}'", co.propmapinfo[prop], prop);
                p = new Process();
                p.StartInfo.FileName = curdir + "\\runcmd.cmd";
                p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", dumpfolder, "CScript.exe", vbsWiRunSQLfile, tempmsifile, sqlstr);
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();
            }

            sqlstr = string.Format("update Property set Value = '{0}' WHERE Property = 'ProductVersion'", co.propmapinfo["ProductVersion"] + ".0.0");
            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", dumpfolder, "CScript.exe", vbsWiRunSQLfile, tempmsifile, sqlstr);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            sqlstr = string.Format("update Property set Value = '{0}' WHERE Property = 'ProductCode'", co.propmapinfo["ProductCode"]);
            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", dumpfolder, "CScript.exe", vbsWiRunSQLfile, tempmsifile, sqlstr);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            sqlstr = string.Format("update Property set Value = '{0}' WHERE Property = 'UpgradeCode'", co.propmapinfo["UpgradeCode"]);
            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", dumpfolder, "CScript.exe", vbsWiRunSQLfile, tempmsifile, sqlstr);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            if (Program.bnewpackageid)
            {
                co.packageguid = Guid.NewGuid().ToString("B");
                //layout.write(layoutfile,co,layout.getstoragenode(layoutfile));
            }
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", dumpfolder, "CScript.exe", vbsWiSumInffile, tempmsifile, String.Format("9={0}", co.packageguid));
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();


            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", dumpfolder, "CScript.exe", vbsUpdateDBfile, tempmsifile, "C__48D13764016F48BF95D547251F493254", Guid.NewGuid().ToString("B").ToUpper());
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", dumpfolder, "CScript.exe", vbsUpdateDBfile, tempmsifile, "C__A8D9517E27C944FB94D22E938BB512FF", Guid.NewGuid().ToString("B").ToUpper());
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            p = new Process();
            p.StartInfo.FileName = curdir + "\\runcmd.cmd";
            p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", dumpfolder, "CScript.exe", vbsUpdateDBfile, tempmsifile, "C__D757C47478F64A928AD09C7B679AA935", Guid.NewGuid().ToString("B").ToUpper());
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            Program.UpdateStatus("copying installer file. Please wait...");
            File.Copy(tempmsifile, msifile, true);

        }


        public MakeMSIHelper(string layoutfile, string tempfolder, string msifile)
        {
            this.layoutfile = layoutfile;
            stagefolder = tempfolder + "\\stage";
            outputfolder = tempfolder + "\\output";
            this.msifile = msifile;
            Directory.CreateDirectory(outputfolder);
            zipfile = outputfolder + "\\fileinstall.zip";
            mkzipfile = tempfolder + "\\makezip.vbs";
        }

        public void MakeMSI()
        {
            Copyfiles();
            Makezip();
            CreateMSI();
        }

    }

}
