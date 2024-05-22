using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SetupLayout;

namespace InstallerCreator
{
    public partial class frmSetActions : Form
    {

        public Dictionary<string, Customaction> nodeactions = new Dictionary<string, Customaction>();
        private bool bleaf;
        private Dictionary<string, Customaction> allactions = new Dictionary<string, Customaction>();
        public frmSetActions(Dictionary<string, Customaction> allactions, Dictionary<string, Customaction> nodeactions, bool bleaf)
        {
            InitializeComponent();
            this.nodeactions = nodeactions;
            this.allactions = allactions;
            this.bleaf = bleaf;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nodeactions.Clear();
            for (int x = 0; x < dataGridView1.Rows.Count; ++x)
            {
                if ((bool)dataGridView1.Rows[x].Cells[0].Value)
                {
                    Customaction a = allactions[(string)dataGridView1.Rows[x].Cells[1].Value].clone();
                    if (dataGridView1.Rows[x].Cells[2].Value != null)
                        a.args = (string)dataGridView1.Rows[x].Cells[2].Value;
                    nodeactions.Add(a.name, a);
                }

            }

        }

        private void frmSetActions_Load(object sender, EventArgs e)
        {
            foreach (var allact in allactions)
            {
                if (bleaf && !allact.Value.leaflelvel)
                    continue;
                if (!bleaf && !allact.Value.folderlevel)
                    continue;
                bool bcheck = nodeactions.ContainsKey(allact.Value.name);
                int x  = dataGridView1.Rows.Add(new object[] { bcheck, allact.Value.name, (bcheck)?nodeactions[allact.Value.name].args:allact.Value.args});
                dataGridView1.Rows[x].Cells[1].ToolTipText =  allact.Value.tooltip;
            }
        }
    }
}
