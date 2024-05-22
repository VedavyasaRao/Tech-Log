using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;
using System.Windows.Automation;

namespace UITesting.Automated.ControlDBTool
{
    public partial class CodeGenearator : Form
    {
        private ImageList imglst;
        PatternHelper pathelper = new PatternHelper();
        string[] supportedpatterns=null;
        ContainerDetails cd = new ContainerDetails("sample","sample");
        List<ControlInfoPair> selectednodes;
        bool bcodevbscript = true;
        List<UseractionData> useractions= new List<UseractionData>();

        public CodeGenearator(ContainerDetails cd,List<ControlInfoPair> selectednodes,List<UseractionData> useractions)
        {
            InitializeComponent();
            supportedpatterns = pathelper.SupportedPatterns;
            this.cd = cd;
            this.selectednodes = selectednodes;
            this.useractions = useractions;
            if (cd.id != "")
            {
                populatetreeview();
                foreach (var ua in this.useractions)
                {
                    ua.id = cd.id;
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.CheckFileExists = true;
                dlg.Filter = "JS files(*.JS)|*.JS";
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                object[] objs = JSONPersister<object[]>.Read(dlg.FileName);
                string buffer = JSONPersister<object>.GetJSON((object)objs[0]);
                cd = JSONPersister<ContainerDetails>.SetJSON(buffer);

                buffer = JSONPersister<object>.GetJSON((object)objs[1]);
                selectednodes = JSONPersister<List<ControlInfoPair>>.SetJSON(buffer);

                populatetreeview();
                selectednodes = new List<ControlInfoPair>();

            }
            catch
            {
            }
        }

        private void populatetreeview()
        {

            if (treeView1.Nodes.ContainsKey(cd.id))
            {
                MessageBox.Show("Window container is already loaded");
                return;
            }

            TreeNode nd = treeView1.Nodes.Add(cd.id, cd.id);
            nd.ToolTipText = cd.desc;
            nd.ImageIndex = 0;
            nd.SelectedImageIndex = 0;
            nd.Tag = selectednodes;

            foreach (ControlInfoPair cip in selectednodes)
            {
                TreeNode nd2 = nd.Nodes.Add(cip.ci.UserName);
                nd2.ImageIndex = 1;
                nd2.SelectedImageIndex = 1;
            }
            treeView1.ExpandAll();

        }

        private void CodeGenearator_Load(object sender, EventArgs e)
        {
            imglst = new ImageList();
            imglst.Images.AddStrip(Resource1.package);
            imglst.Images.AddStrip(Resource1.Element);
            imglst.Images.AddStrip(Resource1.Pattern);
            imglst.Images.AddStrip(Resource1.method);
            treeView1.ImageList = imglst;

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView1.AfterCheck -= treeView1_AfterCheck;
            update_children(e.Node.Checked, e.Node);
            update_parent(e.Node);
            treeView1.AfterCheck += treeView1_AfterCheck;
        }

        private void update_children(bool bcheck, TreeNode trnode)
        {
            foreach (TreeNode tr in trnode.Nodes)
            {
                tr.Checked = bcheck;
                update_children(bcheck, tr);
            }
        }

        private void update_parent(TreeNode trnode)
        {
            TreeNode trparent = trnode.Parent;
            bool bcheck = false;
            if (trparent != null)
            {
                foreach (TreeNode tr in trparent.Nodes)
                {
                    if (tr.Checked)
                    {
                        bcheck = true;
                        break;
                    }
                }

                trparent.Checked = bcheck;
                update_parent(trparent);
            }
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private Dictionary<string,object> GetSelectedWindows()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (TreeNode win in treeView1.Nodes)
            {
                if (!win.Checked)
                    continue;
                ret.Add(win.Text, win.Tag);
            }

            return ret;
        }

        private List<string> GetSelectedElements(string selwnd)
        {
            List<string> ret = new List<string>();
            foreach (TreeNode win in treeView1.Nodes)
            {
                if (win.Text != selwnd)
                    continue;
                
                foreach (TreeNode uie in win.Nodes)
                {
                    if (!uie.Checked)
                        continue;
                    ret.Add(uie.Text);
                }

            }

            return ret;
        }


        private string GetAEDetails(string elename, List<ControlInfoPair> selectednodes, bool blangjs)
        {
            foreach (ControlInfoPair cip in selectednodes)
            {
                if (cip.ci.UserName != elename )
                    continue;

                if (blangjs)
                    return JSONPersister<ControlInfoPair>.GetJSON(cip).Replace("\"", "\"\"").Replace("\r\n", "");
                else
                {
                    var temps = JSONPersister<ControlInfoPair>.GetJSON(cip);
                    temps = temps.Replace("\"", "\\\"").Replace("\r\n", "");
                    int i = 0, j = 0;
                    var token = "\\\"AEText\\\":\\\"";
                    var token2 = "\\\",        \\\"AEAutomationId\\\"";
                    while (true)
                    {
                        i = temps.IndexOf(token, i);
                        if (i == -1)
                            break;
                        i+=token.Length;
                        j = temps.IndexOf(token2, i);
                        var subs = temps.Substring(i, j - i );
                        var reps = subs.Replace("\\", "\\\\");
                        temps = temps.Replace(token+subs, token+reps);
                        i = j + reps.Length;
                    }
                    return temps;
                    //return JSONPersister<ControlInfoPair>.GetJSON(cip).Replace("\"", "\\\"").Replace("\r\n", ""); ;
                }
            }
            return null;
        }

        private List<string> GetPatterns(string elename, List<ControlInfoPair> selectednodes)
        {
            string[] ret = null;
            foreach (ControlInfoPair cip in selectednodes)
            {
                if (cip.ci.UserName != elename)
                    continue;
                string patrns = "MSAAAccessible,Generic,Navigation";
                if (cip.ci.Patterns != "")
                    patrns = patrns + "," + (cip.ci.Patterns);

                List<string> patterns = new List<string>();
                ret = patrns.Split(new char[] { ',' });
                break;
            }
            return supportedpatterns.Intersect(ret).ToList();
        }


        private void generatecscode()
        {
            Dictionary<string, object> winuiamap = GetSelectedWindows();
            List<string> winlst = winuiamap.Keys.ToList();
            string onetab = new string(' ', 4);
            string twotab = new string(' ', 8);
            string threetab = new string(' ', 12);
            string fourtab = new string(' ', 16);
            string fivetab = new string(' ', 20);
            string sixtab = new string(' ', 24);
            string seventab = new string(' ', 28);
            string eighttab = new string(' ', 32);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using UITesting.Automated.UIADriver;");
            sb.AppendLine();
            sb.AppendLine("namespace autogenerated");
            sb.AppendLine("{");
            sb.AppendLine(onetab + "class UIAAutomationElement");
            sb.AppendLine(onetab + "{");
            sb.AppendLine(twotab + "static private UIAElement uiaelement;");
            sb.AppendLine();
            sb.AppendLine(twotab + "static UIAAutomationElement()");
            sb.AppendLine(twotab + "{");
            sb.AppendLine(threetab + "uiaelement = new UIAElement();");
            sb.AppendLine(twotab + "}");

            sb.AppendLine();
            sb.AppendLine(twotab + "static public UIAElement UIADriver");
            sb.AppendLine(twotab + "{");
            sb.AppendLine(threetab + "get");
            sb.AppendLine(threetab + "{");
            sb.AppendLine(fourtab + "return uiaelement;");
            sb.AppendLine(threetab + "}");
            sb.AppendLine(twotab + "}");

            sb.AppendLine(onetab + "}");
            sb.AppendLine();


            foreach (string win in winlst)
            {
                List<string> uielst = GetSelectedElements(win);
                if (uielst.Count == 0)
                    continue;

                List<ControlInfoPair> selectednodes = (List<ControlInfoPair>)winuiamap[win];
                sb.AppendFormat(onetab + "class {0}", win);
                sb.AppendLine();
                sb.AppendLine(onetab + "{");
                foreach (string uie in uielst)
                {
                    List<string> pats = GetPatterns(uie, selectednodes);
                    string selpats = "";
                    foreach (string pat in pats)
                    {
                        if (selpats != "")
                            selpats += ",";
                        selpats += pat;
                    }

                    sb.AppendLine();
                    sb.AppendFormat(twotab + "//supported patterns:{0}", selpats);
                    sb.AppendLine();

                    sb.AppendFormat(twotab + "public string {0}", uie);
                    sb.AppendLine();
                    sb.AppendLine(twotab + "{");
                    sb.AppendLine(threetab + "get");
                    sb.AppendLine(threetab + "{");
                    sb.AppendFormat(fourtab + "return \"{0}\";", GetAEDetails(uie, selectednodes, false));
                    sb.AppendLine();
                    sb.AppendLine(threetab + "}");
                    sb.AppendLine(twotab + "}");

                }
                sb.AppendLine(onetab + "}");
                sb.AppendLine();
            }

            sb.AppendLine(onetab + "//  ***************Playback Code********************");
            sb.AppendLine(onetab + "class Player");
            sb.AppendLine(onetab + "{");
            sb.AppendLine();
            sb.AppendLine(twotab + "static jsonparser jspobj = new jsonparser();");
            foreach (string win in winlst)
            {
                sb.AppendFormat(twotab + "static {0} obj{0} = new {0}();", win);
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.AppendLine(twotab + "static object ParseObj( object injson,  object field)");
            sb.AppendLine(twotab + "{");
            sb.AppendLine(threetab + "return jspobj.ParseObj(injson, field);");
            sb.AppendLine(twotab + "}");
            sb.AppendLine();
            sb.AppendLine(twotab + "public static void Main()");
            sb.AppendLine(twotab + "{");
            sb.AppendLine(threetab + "string brs = null;");
            string oldname = "";
            foreach (UseractionData ua in useractions)
            {
                if (oldname != ua.aelement.ci.UserName)
                {
                    sb.AppendLine();
                    sb.AppendFormat(threetab + "UIAAutomationElement.UIADriver.SetAutomationElement (obj{0}.{1});", ua.id, ua.aelement.ci.UserName);
                    sb.AppendLine();
                    oldname = ua.aelement.ci.UserName;
                }

                if (ua.keydowndata != null)
                {
                    sb.AppendFormat(threetab + "UIAAutomationElement.UIADriver.ProviderGeneric.SendVirtualKeyStrokes ({0},{1});", ua.keydowndata.KeyValue, Convert.ToInt32(ua.modifier));
                    sb.AppendLine();
                }
                else if (ua.mousedata != null)
                {
                    List<ControlInfoPair> selectednodes = (List<ControlInfoPair>)winuiamap[ua.id];
                    ControlInfoPair cip = selectednodes.Find((p) => p.ci.UserName == ua.aelement.ci.UserName);
                    if (ua.aeventlist.Contains(InvokePattern.InvokedEvent))
                    {
                        sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderInvoke.Click();");
                    }
                    else if (ua.aeventlist.Contains(SelectionItemPattern.ElementSelectedEvent))
                    {
                        sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderSelectionItem.MakeSelection();");
                        sb.AppendLine(threetab + "if (UIAAutomationElement.UIADriver.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_IsOffscreen) == \"false\")");
                        sb.AppendLine(fourtab + "UIAAutomationElement.UIADriver.ProviderGeneric.Click (false,false);");
                    }
                    else if (ua.aeventlist.Contains(AutomationElement.MenuOpenedEvent))
                    {
                        sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderExpandCollapse.Expand();");
                    }
                    else
                    {
                        if (cip.ci.Patterns.Contains("Invoke"))
                        {
                            sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderInvoke.Click();");
                        }
                        else if (cip.ci.Patterns.Contains("SelectionItem"))
                        {
                            sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderSelectionItem.MakeSelection();");
                            sb.AppendLine(threetab + "if (UIAAutomationElement.UIADriver.ProviderGeneric.GetAutomationProperty(objUIAElement.Constants.AutomationProperty_IsOffscreen) == \"false\")");
                            sb.AppendLine(fourtab + "UIAAutomationElement.UIADriver.ProviderGeneric.Click (false,false);");
                        }
                        else if (cip.ci.Patterns.Contains("ExpandCollapse"))
                        {
                            sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderExpandCollapse.Expand();");
                        }
                        else if (cip.ci.Patterns.Contains("Toggle"))
                        {
                            sb.AppendLine(threetab + "UIAAutomationElement.UIADriver.ProviderToggle.Toggle();");
                        }
                        else
                        {
                            sb.AppendLine(threetab + "brs = UIAAutomationElement.UIADriver.ProviderGeneric.GetAutomationProperty(UIAAutomationElement.UIADriver.Constants.AutomationProperty_BoundingRectangle);");
                            sb.AppendFormat(threetab + "UIAAutomationElement.UIADriver.ProviderGeneric.MoveandClick ((int)ParseObj(brs,\"Left\")+{0}, (int)ParseObj(brs,\"Top\")+{1}, false, {2}, {3});",
                                Convert.ToInt32(ua.clickPt.X), Convert.ToInt32(ua.clickPt.Y),
                                (ua.mousedata.Button == System.Windows.Forms.MouseButtons.Right).ToString().ToLower(), ua.isdoubleclick.ToString().ToLower());
                            sb.AppendLine();
                        }
                    }
                }
            }
            sb.AppendLine(twotab + "}");
            sb.AppendLine();
            sb.AppendLine(onetab + "}");
            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();

            txtCode.Text = sb.ToString();
            bcodevbscript = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = (bcodevbscript) ? "VBScript files(*.vbs)|*.vbs" : "CSharp files(*.cs)|*.cs";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fname = dlg.FileName;
                if (bcodevbscript)
                {
                    if (fname.IndexOf(".vbs", StringComparison.OrdinalIgnoreCase) == -1)
                        fname += ".vbs";
                }
                else
                {
                    if (fname.IndexOf(".cs", StringComparison.OrdinalIgnoreCase) == -1)
                        fname += ".cs";

                }
                File.WriteAllText(fname, txtCode.Text);
            }

        }

        private void js_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> winuiamap = GetSelectedWindows();
            List<string> winlst = winuiamap.Keys.ToList();
            string onetab = new string(' ', 4);
            string twotab = new string(' ', 8);
            string threetab = new string(' ', 12);
            StringBuilder sb = new StringBuilder();

            foreach (string win in winlst)
            {
                List<string> uielst = GetSelectedElements(win);
                if (uielst.Count == 0)
                    continue;

                List<ControlInfoPair> selectednodes = (List<ControlInfoPair>)winuiamap[win];
                sb.AppendFormat("Class {0}", win);
                foreach (string uie in uielst)
                {
                    List<string> pats = GetPatterns(uie, selectednodes);
                    string selpats = "";
                    foreach (string pat in pats)
                    {
                        if (selpats != "")
                            selpats += ",";
                        selpats += pat;
                    }

                    sb.AppendLine();
                    sb.AppendFormat(twotab + "'supported patterns:{0}", selpats);
                    sb.AppendLine();
                    sb.AppendFormat(onetab + "Public  Property Get {0}()", uie);
                    sb.AppendLine();
                    sb.AppendFormat(twotab + "{0} = \"{1}\"", uie, GetAEDetails(uie, selectednodes, true));
                    sb.AppendLine();
                    sb.AppendLine(onetab + "End Property");
                }
                sb.AppendLine();
                sb.AppendLine("End Class");
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.AppendLine("dim objUIAElement");
            foreach (string win in winlst)
            {
                sb.AppendFormat("dim obj{0}", win);
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("set objUIAElement = CreateObject(\"uiadriverlib.uiaelement\")");
            foreach (string win in winlst)
            {
                sb.AppendFormat("set obj{0} = new {0}", win);
                sb.AppendLine();
            }

            if (useractions.Count != 0)
            {
                sb.AppendLine();
                sb.AppendLine("'******************Playback Code****************");
                //sb.AppendLine("set jspobj = CreateObject(\"jsonparser.wsc\")");
                sb.AppendLine("static jsonparser jspobj = new jsonparser();");

                string oldname = "";
                foreach (UseractionData ua in useractions)
                {
                    if (oldname != ua.aelement.ci.UserName)
                    {
                        sb.AppendLine();
                        sb.AppendFormat("objUIAElement.SetAutomationElement obj{0}.{1}", ua.id, ua.aelement.ci.UserName);
                        sb.AppendLine();
                        oldname = ua.aelement.ci.UserName;
                    }

                    if (ua.keydowndata != null)
                    {
                        sb.AppendFormat("objUIAElement.ProviderGeneric.SendVirtualKeyStrokes {0},{1}", ua.keydowndata.KeyValue, Convert.ToInt32(ua.modifier));
                        sb.AppendLine();
                    }
                    else if (ua.mousedata != null)
                    {
                        List<ControlInfoPair> selectednodes = (List<ControlInfoPair>)winuiamap[ua.id];
                        ControlInfoPair cip = selectednodes.Find((p) => p.ci.UserName == ua.aelement.ci.UserName);
                        if (ua.aeventlist.Contains(InvokePattern.InvokedEvent))
                        {
                            sb.AppendLine("objUIAElement.ProviderInvoke.Click");
                        }
                        else if (ua.aeventlist.Contains(SelectionItemPattern.ElementSelectedEvent))
                        {
                            sb.AppendLine("objUIAElement.ProviderSelectionItem.MakeSelection");
                            sb.AppendLine("if objUIAElement.ProviderGeneric.GetAutomationProperty(objUIAElement.Constants.AutomationProperty_IsOffscreen) = \"false\" then");
                            sb.AppendLine("\tobjUIAElement.ProviderGeneric.Click false,false");
                            sb.AppendLine("end if");
                        }
                        else if (ua.aeventlist.Contains(AutomationElement.MenuOpenedEvent))
                        {
                            sb.AppendLine("objUIAElement.ProviderExpandCollapse.Expand");
                        }
                        else
                        {
                            if (cip.ci.Patterns.Contains("Invoke"))
                            {
                                sb.AppendLine("objUIAElement.ProviderInvoke.Click");
                            }
                            else if (cip.ci.Patterns.Contains("SelectionItem"))
                            {
                                sb.AppendLine("objUIAElement.ProviderSelectionItem.MakeSelection");
                                sb.AppendLine("if objUIAElement.ProviderGeneric.GetAutomationProperty(objUIAElement.Constants.AutomationProperty_IsOffscreen) = \"false\" then");
                                sb.AppendLine("\tobjUIAElement.ProviderGeneric.Click false,false");
                                sb.AppendLine("end if");
                            }
                            else if (cip.ci.Patterns.Contains("ExpandCollapse"))
                            {
                                sb.AppendLine("objUIAElement.ProviderExpandCollapse.Expand");
                            }
                            else if (cip.ci.Patterns.Contains("Toggle"))
                            {
                                sb.AppendLine("objUIAElement.ProviderToggle.Toggle");
                            }
                            else
                            {
                                sb.AppendLine("brs = objUIAElement.ProviderGeneric.GetAutomationProperty(objUIAElement.Constants.AutomationProperty_BoundingRectangle)");
                                sb.AppendFormat("objUIAElement.ProviderGeneric.MoveandClick jspobj.parseobj(brs,\"Left\")+{0}, jspobj.parseobj(brs,\"Top\")+{1}, false, {2}, {3}",
                                    Convert.ToInt32(ua.clickPt.X), Convert.ToInt32(ua.clickPt.Y),
                                    (ua.mousedata.Button == System.Windows.Forms.MouseButtons.Right), ua.isdoubleclick);
                                sb.AppendLine();
                            }

                        }

                    }

                }

                sb.AppendLine("set jspobj = nothing");
            }

            txtCode.Text = sb.ToString();
            bcodevbscript = true;

        }

        private void cs_Click(object sender, EventArgs e)
        {
            generatecscode();
        }

    }
}
