using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSonSerializer;
using System.IO;
using System.Runtime.InteropServices;

namespace SetupLayout
{
    public class Nodedata
    {
        public string name = "";
        public string physicalpath = "";
        public Dictionary<string, Customaction> inscustomactions = new Dictionary<string, Customaction>();
        public Dictionary<string, Customaction> uinscustomactions = new Dictionary<string, Customaction>();
        public Nodedata()
        {
        }

        public Nodedata(string path, bool bphysical)
        {
            if (bphysical)
            {
                physicalpath = path;
                int k = path.LastIndexOf("\\");
                if (k != -1)
                    name = path.Substring(k + 1);
            }
            else
                name = path;
        }
    }


    public class storagenode
    {
        public Nodedata nodedata;
        public List<storagenode> children = new List<storagenode>();
    }

    public class container
    {
        public string preinstall = "";
        public string preinstallargs = "";
        public string postinstall = "";
        public string postinstallargs = "";
        public string preuninstall = "";
        public string preuninstallargs = "";
        public string postuninstall = "";
        public string postuninstallargs = "";
        public Dictionary<string, string> dirmapinfo = new Dictionary<string, string>();
        public Dictionary<string, string> propmapinfo = new Dictionary<string, string>() { { "Manufacturer", "EasyInstaller" }, { "ProductName", "Sample" }, 
        { "ProductVersion", "1" }, { "ProductCode", Guid.NewGuid().ToString("B") }, { "UpgradeCode", Guid.NewGuid().ToString("B") } };
        public string packageguid = Guid.NewGuid().ToString("B");
        public bool bask = true;
    }

    public class Customaction
    {

        public const string CONFIG_EDIT = "Edit Configuration";
        public const string REGSVR_32 = "RegSvr 32 Bit";
        public const string UNREGSVR_32 = "UnregSvr 32 Bit";
        public const string REGSVR_64 = "RegSvr 64 Bit";
        public const string UNREGSVR_64 = "UnregSvr 64 Bit";
        public const string REGEDT_32 = "RegEdit 32 Bit";
        public const string CREATEFOLDER = "Create Folder";
        public const string REMOVEFOLDER = "Delete Folder";
        public const string CSCRIPT_32 = "CScript 32";
        public const string CREATE_COMPLUS_APP = "Create COM+ App";
        public const string DELETE_COMPLUS_APP = "Delete COM+ App";

        public string name="";
        public bool folderlevel=false;
        public bool leaflelvel=false;
        public string executable = "";
        public string args="";
        public string filter = "*";
        public bool grpaction = false;
        public int priority = 0;
        public bool builtin = false;
        public bool wfe = true;
        public bool binstall=true;
        public bool buninstall = true;
        public string tooltip="";

        public Customaction clone()
        {
            Customaction ret = new Customaction();
            ret.name = name;
            ret.folderlevel = folderlevel;
            ret.leaflelvel = leaflelvel;
            ret.executable = executable;
            ret.args = args;
            ret.filter = filter;
            ret.grpaction = grpaction;
            ret.priority = priority;
            ret.builtin = builtin;
            ret.wfe = wfe;
            ret.binstall = binstall;
            ret.buninstall = buninstall;
            ret.tooltip = tooltip;
            return ret;
        }
    }


    public class Customactions
    {
        public static void write(string filename, Dictionary<string, Customaction> custacts)
        {
            JSONPersister<Dictionary<string, Customaction>>.Write(filename, custacts);
        }

        public static Dictionary<string, Customaction> read(string filename)
        {
            return JSONPersister<Dictionary<string, Customaction>>.Read(filename);
        }

        public static Dictionary<string, Customaction> GetAllActions(bool bInstall, Dictionary<string, Customaction> allactions, string ppath)
        {
            Dictionary<string, Customaction> ret = new Dictionary<string, Customaction>();
            foreach (var a in allactions)
            {
                if (bInstall && !a.Value.binstall)
                    continue;
                if (!bInstall && !a.Value.buninstall)
                    continue;

                if (ppath.Length == 0 && a.Value.folderlevel)
                {
                    ret.Add(a.Key,a.Value);
                    continue;
                }
                
                if (ppath.Length > 0 && a.Value.leaflelvel)
                {
                    string ex = Path.GetExtension(ppath).ToLower().Substring(1);
                    string[] extns = a.Value.filter.Split(new char[] { ',' });
                    foreach (var x in extns)
                    {
                        if (x.ToLower() == ex || x == "*")
                        {
                            ret.Add(a.Key, a.Value);
                            break;
                        }
                    }
                }

            }


            return ret;
        }

    }


    public class layout
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        static extern bool PathRelativePathTo(
             [Out] StringBuilder pszPath,
             [In] string pszFrom,
             [In] FileAttributes dwAttrFrom,
             [In] string pszTo,
             [In] FileAttributes dwAttrTo
        );

        public static void write(string filename, container co, storagenode sn)
        {
            JSONPersister<object[]>.Write(filename, new object[] { co, sn });
        }

        public static container getcontainer(string filename)
        {
            object[] objs = JSONPersister<object[]>.Read(filename);
            string buffer = JSONPersister<object>.GetJSON((object)objs[0]);
            container cn =  JSONPersister<container>.SetJSON(buffer);
            mergepath(cn, Path.GetDirectoryName(filename), false);
            return cn;
        }

        public static storagenode getstoragenode(string filename)
        {
            object[] objs = JSONPersister<object[]>.Read(filename);
            string buffer = JSONPersister<object>.GetJSON((object)objs[1]);
            storagenode node = JSONPersister<storagenode>.SetJSON(buffer);
            mergepath(node, Path.GetDirectoryName(filename), false);
            return node;

        }

        private static void mergepath(ref string targetpath, string combinepath, bool brelative)
        {
            if (targetpath != "")
            {
                if (brelative)
                {
                    if (Path.GetPathRoot(targetpath) == Path.GetPathRoot(combinepath))
                    {
                        StringBuilder str = new StringBuilder(260);
                        Boolean bRet = PathRelativePathTo(str, combinepath, FileAttributes.Directory, targetpath, FileAttributes.Normal);
                        targetpath = str.ToString();
                    }
                }
                else
                {
                    targetpath = Path.GetFullPath(Path.Combine(combinepath, targetpath));

                }
            }
        }

        public static void mergepath(container cn, string combinepath, bool brelative)
        {
            mergepath(ref cn.preinstall, combinepath,brelative);
            mergepath(ref cn.postinstall, combinepath,brelative);
            mergepath(ref cn.preuninstall, combinepath,brelative);
            mergepath(ref cn.postuninstall, combinepath,brelative);
        }

        public static void mergepath(storagenode node, string combinepath, bool brelative)
        {
            mergepath(ref node.nodedata.physicalpath, combinepath, brelative);
            foreach (var n in node.children)
                mergepath(n, combinepath, brelative);
        }
    }

}
