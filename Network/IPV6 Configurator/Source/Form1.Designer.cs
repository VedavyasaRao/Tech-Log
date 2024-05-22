namespace NICUtilTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtip = new System.Windows.Forms.TextBox();
            this.txtsn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtgw = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radipv6 = new System.Windows.Forms.RadioButton();
            this.radipv4 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkupdaedisflag = new System.Windows.Forms.CheckBox();
            this.radmanual = new System.Windows.Forms.RadioButton();
            this.radDHCP = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmdadapter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP Address";
            // 
            // txtip
            // 
            this.txtip.Location = new System.Drawing.Point(154, 38);
            this.txtip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtip.Name = "txtip";
            this.txtip.Size = new System.Drawing.Size(439, 26);
            this.txtip.TabIndex = 5;
            // 
            // txtsn
            // 
            this.txtsn.Location = new System.Drawing.Point(154, 78);
            this.txtsn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtsn.Name = "txtsn";
            this.txtsn.Size = new System.Drawing.Size(439, 26);
            this.txtsn.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Subnet";
            // 
            // txtgw
            // 
            this.txtgw.Location = new System.Drawing.Point(154, 118);
            this.txtgw.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtgw.Name = "txtgw";
            this.txtgw.Size = new System.Drawing.Size(439, 26);
            this.txtgw.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Gateway";
            // 
            // btnset
            // 
            this.btnset.Location = new System.Drawing.Point(514, 428);
            this.btnset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnset.Name = "btnset";
            this.btnset.Size = new System.Drawing.Size(112, 35);
            this.btnset.TabIndex = 10;
            this.btnset.Text = "Set";
            this.btnset.UseVisualStyleBackColor = true;
            this.btnset.Click += new System.EventHandler(this.btnset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radipv6);
            this.groupBox1.Controls.Add(this.radipv4);
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(300, 77);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // radipv6
            // 
            this.radipv6.AutoSize = true;
            this.radipv6.Location = new System.Drawing.Point(195, 23);
            this.radipv6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radipv6.Name = "radipv6";
            this.radipv6.Size = new System.Drawing.Size(69, 24);
            this.radipv6.TabIndex = 3;
            this.radipv6.Text = "IPV6";
            this.radipv6.UseVisualStyleBackColor = true;
            this.radipv6.CheckedChanged += new System.EventHandler(this.radipv6_CheckedChanged);
            // 
            // radipv4
            // 
            this.radipv4.AutoSize = true;
            this.radipv4.Checked = true;
            this.radipv4.Location = new System.Drawing.Point(34, 23);
            this.radipv4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radipv4.Name = "radipv4";
            this.radipv4.Size = new System.Drawing.Size(69, 24);
            this.radipv4.TabIndex = 2;
            this.radipv4.TabStop = true;
            this.radipv4.Text = "IPV4";
            this.radipv4.UseVisualStyleBackColor = true;
            this.radipv4.CheckedChanged += new System.EventHandler(this.radipv4_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkupdaedisflag);
            this.groupBox2.Controls.Add(this.radmanual);
            this.groupBox2.Controls.Add(this.radDHCP);
            this.groupBox2.Location = new System.Drawing.Point(351, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(300, 112);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // chkupdaedisflag
            // 
            this.chkupdaedisflag.AutoSize = true;
            this.chkupdaedisflag.Checked = true;
            this.chkupdaedisflag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkupdaedisflag.Location = new System.Drawing.Point(51, 71);
            this.chkupdaedisflag.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkupdaedisflag.Name = "chkupdaedisflag";
            this.chkupdaedisflag.Size = new System.Drawing.Size(213, 24);
            this.chkupdaedisflag.TabIndex = 17;
            this.chkupdaedisflag.Text = "Update Router Discovery";
            this.chkupdaedisflag.UseVisualStyleBackColor = true;
            this.chkupdaedisflag.CheckedChanged += new System.EventHandler(this.chkupdaedisflag_CheckedChanged);
            // 
            // radmanual
            // 
            this.radmanual.AutoSize = true;
            this.radmanual.Location = new System.Drawing.Point(186, 32);
            this.radmanual.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radmanual.Name = "radmanual";
            this.radmanual.Size = new System.Drawing.Size(86, 24);
            this.radmanual.TabIndex = 5;
            this.radmanual.Text = "Manual";
            this.radmanual.UseVisualStyleBackColor = true;
            this.radmanual.CheckedChanged += new System.EventHandler(this.radmanual_CheckedChanged);
            // 
            // radDHCP
            // 
            this.radDHCP.AutoSize = true;
            this.radDHCP.Checked = true;
            this.radDHCP.Location = new System.Drawing.Point(26, 32);
            this.radDHCP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radDHCP.Name = "radDHCP";
            this.radDHCP.Size = new System.Drawing.Size(79, 24);
            this.radDHCP.TabIndex = 4;
            this.radDHCP.TabStop = true;
            this.radDHCP.Text = "DHCP";
            this.radDHCP.UseVisualStyleBackColor = true;
            this.radDHCP.CheckedChanged += new System.EventHandler(this.radDHCP_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtip);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtsn);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtgw);
            this.groupBox3.Location = new System.Drawing.Point(32, 240);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(620, 165);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            // 
            // cmdadapter
            // 
            this.cmdadapter.FormattingEnabled = true;
            this.cmdadapter.Location = new System.Drawing.Point(114, 158);
            this.cmdadapter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdadapter.Name = "cmdadapter";
            this.cmdadapter.Size = new System.Drawing.Size(386, 28);
            this.cmdadapter.TabIndex = 15;
            this.cmdadapter.SelectedIndexChanged += new System.EventHandler(this.cmdadapter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 158);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Adapters:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 502);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdadapter);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnset);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "IPv6 Configuration Utility";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtip;
        private System.Windows.Forms.TextBox txtsn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtgw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radipv6;
        private System.Windows.Forms.RadioButton radipv4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radmanual;
        private System.Windows.Forms.RadioButton radDHCP;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmdadapter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkupdaedisflag;
    }
}

