using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using TLI;
using System.Runtime.InteropServices;
using System.ComponentModel;
namespace COMLibHelper
{
    public class Comobjectstorage
    {
        public class paramtr
        {
            public string name;
            public string vartype;
            public bool bref;
            public bool bout;

            public string getpardeco()
            {
                return (bout ? "out" : bref ? "ref" : "");
            }
        }

        public class classmtds
        {
            public string name;
            public string returntype;
            public List<paramtr> pars = new List<paramtr>();

            public string getrettype()
            {
                return (returntype == "Void" ? "void" : returntype);
            }

            public string getcallrettype()
            {
                return (returntype == "Void" ? "" : "return ");
            }

            public string getvbrettype()
            {
                return (returntype == "Void" ? "Sub" : "Function");
            }

            public string getvbgetset()
            {
                return (returntype == "object" ? "set" : "");
            }


        }

        public class classprops
        {
            public string name;
            public string returntype;
            public bool bget = false;
            public bool bset = false;
            public List<paramtr> getpars = new List<paramtr>();
            public List<paramtr> setpars = new List<paramtr>();

        }

        public classprops findproperty(string name)
        {
            foreach (classprops cp in props)
            {
                if (cp.name == name)
                    return cp;
            }
            return null;
        }

        public classmtds findmtd(string name)
        {
            foreach (classmtds mtd in mtds)
            {
                if (mtd.name == name)
                    return mtd;
            }
            return null;
        }

        public string path = "";
        public string progid = "";
        public bool bmanaged = true;
        public string urn = "";
        public bool iscomplus;
        public string namesp;
        public string name;
        public Guid clsid;
        public Guid wcfclsid;
        public List<classmtds> mtds = new List<classmtds>();
        public List<classprops> props = new List<classprops>();
    }


    public class CodeGenHelper
    {
        public static bool bincoptional = false;
        [DllImport("ole32.dll")]
        static extern int ProgIDFromCLSID([In] ref Guid clsid, [MarshalAs(UnmanagedType.LPWStr)] out string lplpszProgID);

        public static void GatherData(List<Comobjectstorage> sellist)
        {
            foreach (var sel in sellist)
            {
                if (sel.bmanaged && sel.urn == "") 
                    GatherDatafromAssembly(sel);
                else if (!sel.bmanaged) 
                    GatherDataFromTLB(sel);

            }
        }

        public static List<KeyValuePair<string, string>> GetclsidsfromAssembly(string filename)
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
            try
            {
                Assembly b = System.Reflection.Assembly.LoadFrom(filename);
                foreach (Type t in b.GetExportedTypes())
                {
                    object[] ats = t.GetCustomAttributes(typeof(System.Runtime.InteropServices.ComVisibleAttribute), false);
                    if (ats == null || ats.Length != 1 || !(((System.Runtime.InteropServices.ComVisibleAttribute)ats[0]).Value) || !t.IsClass)
                        continue;

                    Guid clsid = t.GUID;
                    string progid = "";
                    ProgIDFromCLSID(ref clsid, out progid);
                    if (progid == null)
                        progid = t.Namespace + "." + t.Name;
                    ret.Add(new KeyValuePair<string, string>(clsid.ToString("B"), progid));
                }
            }
            catch
            {

            }
            return ret;
        }

        public static List<string> GetinterfacesforclsidfromAssembly(string filename,string clsid)
        {
            List<string> ret = new List<string>();
            try
            {
                Assembly b = System.Reflection.Assembly.LoadFrom(filename);
                foreach (Type t in b.GetExportedTypes())
                {
                    object[] ats = t.GetCustomAttributes(typeof(System.Runtime.InteropServices.ComVisibleAttribute), false);
                    if (ats == null || ats.Length != 1 || !(((System.Runtime.InteropServices.ComVisibleAttribute)ats[0]).Value) || !t.IsClass)
                        continue;
                    if (clsid.ToLower() != t.GUID.ToString("B").ToLower())
                        continue;

                    foreach (var itf in t.GetInterfaces())
                    {
                        if (itf.Assembly.Location.ToLower().Contains(Path.GetFileName(filename).ToLower()))
                            ret.Add(itf.GUID.ToString("B"));
                    }
                }
            }
            catch
            {

            }
            return ret;
        }

        public static Dictionary<string, Comobjectstorage> GetinterfacesfromAssembly(string filename)
        {
            Dictionary<string, Comobjectstorage> ret = new Dictionary<string, Comobjectstorage>();
            try
            {
                byte[] a = File.ReadAllBytes(filename);
                Assembly b = Assembly.Load(a);
                foreach (Type t in b.GetExportedTypes())
                {
                    if (!t.IsInterface )
                        continue;
                    
                    string itf = t.Namespace + "." + t.Name;
                    Comobjectstorage comobj = new Comobjectstorage { progid = itf, urn = itf, bmanaged = true };
                    comobj.name = t.Name;
                    comobj.namesp = t.Namespace;
                    comobj.clsid = t.GUID;
                    comobj.wcfclsid = Guid.NewGuid();
                    DescriptionAttribute da = (DescriptionAttribute)TypeDescriptor.GetAttributes(t)[typeof(DescriptionAttribute)];
                    if (da != null && da.Description.Contains("urn:"))
                    {
                        comobj.urn = da.Description;
                    }

                    UpdateComObj(ref comobj, t);
                    ret.Add(itf,comobj);
                }
            }
            catch
            {
            }
            return ret;
        }


        public static List<KeyValuePair<string, string>> GetclsidsfromTLB(string filename)
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
            try
            {
                TLI.TLIApplication tli = new TLI.TLIApplication();
                TypeLibInfo ti = tli.TypeLibInfoFromFile(filename);

                foreach (CoClassInfo cc in ti.CoClasses)
                {
                    Guid clsid = new Guid(cc.GUID);
                    string progid = "";
                    ProgIDFromCLSID(ref clsid, out progid);
                    if (progid == null)
                        progid = ti.Name + "." + cc.Name;
                    ret.Add(new KeyValuePair<string, string>(clsid.ToString("B"), progid));
                }
            }
            catch
            {

            }

            return ret;
        }

        public static List<KeyValuePair<string, string>> GetclsidsfromWSC(string filename)
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
            try
            {
                string alltxt = File.ReadAllText(filename);
                int p=0;
                while (true)
                {
                    string progid = "";
                    p = alltxt.IndexOf("progid=", p);
                    if (p == -1)
                        break;
                    int m=p+8;
                    int n = alltxt.IndexOf('"', m);
                    if (n == -1)
                        break;

                    progid = alltxt.Substring(m, n - m);
                    
                    p = n;
                    string clsid = "";
                    p = alltxt.IndexOf("classid=", p);
                    if (p == -1)
                        break;
                    m = p + 9;
                    n = alltxt.IndexOf('"', m);
                    if (n == -1)
                        break;
                    clsid = alltxt.Substring(m, n - m);
                    p = n;

                    ret.Add(new KeyValuePair<string, string>(clsid, progid));

                }
            }
            catch
            {

            }

            return ret;
        }

        static string VarianttoManaged(VarTypeInfo v)
        {

            switch (v.VarType)
            {
                case TliVarType.VT_BOOL:
                    return "bool";

                case TliVarType.VT_I1:
                    return "sbyte";

                case TliVarType.VT_UI1:
                    return "byte";

                case TliVarType.VT_I2:
                    return "short";

                case TliVarType.VT_UI2:
                    return "ushort";

                case TliVarType.VT_I4:
                    return "int";

                case TliVarType.VT_UI4:
                    return "uint";

                case TliVarType.VT_I8:
                    return "long";

                case TliVarType.VT_UI8:
                    return "ulong";

                case TliVarType.VT_R4:
                    return "float";

                case TliVarType.VT_R8:
                    return "double";

                case TliVarType.VT_DATE:
                    return "DateTime";

                case TliVarType.VT_BSTR:
                    return "string";

                case TliVarType.VT_VOID:
                    return "Void";

                default:
                    return "object";

            }
        }


        static void GatherDataFromTLB(Comobjectstorage comobj)
        {

            TLI.TLIApplication tli = new TLI.TLIApplication();
            TypeLibInfo ti = tli.TypeLibInfoFromFile(comobj.path);

            foreach (CoClassInfo cc in ti.CoClasses)
            {
                string progid = comobj.progid;
                if (progid != "")
                {
                    int k = progid.IndexOf('.');
                    if (k != -1)
                    {
                        comobj.namesp = progid.Substring(0, k);
                        comobj.name = progid.Substring(k + 1).Replace('.','_');
                    }
                }
                if (comobj.clsid.ToString("B").ToLower() != cc.GUID.ToLower())
                    continue;

                comobj.clsid = new Guid(cc.GUID);
                comobj.wcfclsid = Guid.NewGuid();

                foreach (TLI.MemberInfo minf in cc.DefaultInterface.Members)
                {
                    if (minf.AttributeMask != 0)
                        continue;

                    InvokeKinds functype = minf.InvokeKind;
                    if (functype != TLI.InvokeKinds.INVOKE_FUNC && functype != TLI.InvokeKinds.INVOKE_PROPERTYGET &&
                        functype != TLI.InvokeKinds.INVOKE_PROPERTYPUT && functype != TLI.InvokeKinds.INVOKE_PROPERTYPUTREF)
                        continue;

                    if (functype != TLI.InvokeKinds.INVOKE_FUNC)
                    {
                        Comobjectstorage.classprops prop = comobj.findproperty(minf.Name);
                        if (prop == null)
                        {
                            prop = new Comobjectstorage.classprops();
                            prop.name = minf.Name;
                            prop.returntype = VarianttoManaged(minf.ReturnType);
                            comobj.props.Add(prop);
                        }
                        if (functype == TLI.InvokeKinds.INVOKE_PROPERTYGET)
                            prop.bget = true;
                        else if (functype == TLI.InvokeKinds.INVOKE_PROPERTYPUT || functype == TLI.InvokeKinds.INVOKE_PROPERTYPUTREF)
                            prop.bset = true;

                        foreach (TLI.ParameterInfo pmi in minf.Parameters)
                        {
                            Comobjectstorage.paramtr par = new Comobjectstorage.paramtr();
                            par.name = pmi.Name;
                            par.vartype = VarianttoManaged(pmi.VarTypeInfo);
                            par.bref = false;
                            if (pmi.VarTypeInfo.PointerLevel != 0)
                                par.bref = true;
                            (functype == TLI.InvokeKinds.INVOKE_PROPERTYGET ? prop.getpars : prop.setpars).Add(par);
                        }

                    }
                    else
                    {
                        Comobjectstorage.classmtds mtd = new Comobjectstorage.classmtds();
                        mtd.name = minf.Name;

                        mtd.returntype = VarianttoManaged(minf.ReturnType);
                        foreach (TLI.ParameterInfo pmi in minf.Parameters)
                        {
                            if (pmi.Name == "OptionalArgs" && !bincoptional)
                                continue;
                            Comobjectstorage.paramtr par = new Comobjectstorage.paramtr();
                            par.name = pmi.Name;
                            par.vartype = VarianttoManaged(pmi.VarTypeInfo);
                            par.bref = false;
                            if (pmi.VarTypeInfo.PointerLevel != 0)
                                par.bref = true;
                            mtd.pars.Add(par);
                        }
                        comobj.mtds.Add(mtd);
                    }
                }
            }
        }

        static void UpdateComObj(ref Comobjectstorage comobj, Type t)
        {

            List<string> props = new List<string>();
            var properties = t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propinfo in properties)
            {
                if (propinfo.DeclaringType != t)
                    continue;

                Comobjectstorage.classprops comprop = new Comobjectstorage.classprops();
                comprop.returntype = propinfo.PropertyType.Name;
                comprop.name = propinfo.Name;
                comprop.bget = propinfo.CanRead;
                comprop.bset = propinfo.CanWrite;
                comobj.props.Add(comprop);
                props.Add("get_" + propinfo.Name);
                props.Add("set_" + propinfo.Name);
            }

            var methods = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
            foreach (MethodInfo mtdinfo in methods)
            {
                if (mtdinfo.DeclaringType != t)
                    continue;

                if (props.Contains(mtdinfo.Name))
                    continue;

                Comobjectstorage.classmtds mtd = new Comobjectstorage.classmtds();
                mtd.name = mtdinfo.Name;
                mtd.returntype = mtdinfo.ReturnType.Name;
                foreach (System.Reflection.ParameterInfo pi in mtdinfo.GetParameters())
                {
                    Comobjectstorage.paramtr para = new Comobjectstorage.paramtr();
                    para.name = pi.Name;
                    int len = pi.ParameterType.Name.Length;
                    para.vartype = pi.ParameterType.Name;
                    if (pi.ParameterType.Name[len - 1] == '&')
                    {
                        para.vartype = pi.ParameterType.Name.Substring(0, len - 1);
                        para.bout = pi.IsOut;
                        para.bref = !para.bout;
                    }
                    mtd.pars.Add(para);
                }
                comobj.mtds.Add(mtd);
            }

        }

        static void GatherDatafromAssembly(Comobjectstorage comobj)
        {
            Assembly b = null;
            b = System.Reflection.Assembly.LoadFrom(comobj.path);

            foreach (Type t in b.GetExportedTypes())
            {
                object[] ats = t.GetCustomAttributes(typeof(System.Runtime.InteropServices.ComVisibleAttribute), false);
                if (ats == null || ats.Length != 1 || !(((System.Runtime.InteropServices.ComVisibleAttribute)ats[0]).Value))
                    continue;

                comobj.name = t.Name;
                comobj.namesp = t.Namespace;
                string progid = comobj.progid;
                if (progid != "")
                {
                    int k = progid.IndexOf('.');
                    if (k != -1)
                    {
                        comobj.namesp = progid.Substring(0, k);
                        comobj.name = progid.Substring(k + 1);
                    }

                }

                if (comobj.clsid != t.GUID)
                    continue;
                comobj.clsid = t.GUID;
                comobj.wcfclsid = Guid.NewGuid();
                UpdateComObj(ref comobj, t);
            }

        }

    }
}
