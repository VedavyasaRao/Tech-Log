namespace MigrationHelper
{
    partial class Form2
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
            this.previewTree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.btncollapse = new System.Windows.Forms.Button();
            this.btnexpand = new System.Windows.Forms.Button();
            this.btnshow = new System.Windows.Forms.Button();
            this.txtextensions = new System.Windows.Forms.TextBox();
            this.txtextns = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chksingle = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewTree
            // 
            this.previewTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewTree.Location = new System.Drawing.Point(0, 0);
            this.previewTree.Name = "previewTree";
            this.previewTree.Size = new System.Drawing.Size(537, 301);
            this.previewTree.TabIndex = 0;
            this.previewTree.DoubleClick += new System.EventHandler(this.previewTree_DoubleClick);
            this.previewTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.previewTree_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filter Extensions:";
            // 
            // btncollapse
            // 
            this.btncollapse.Location = new System.Drawing.Point(448, 12);
            this.btncollapse.Name = "btncollapse";
            this.btncollapse.Size = new System.Drawing.Size(24, 23);
            this.btncollapse.TabIndex = 3;
            this.btncollapse.Text = "+";
            this.btncollapse.UseVisualStyleBackColor = true;
            this.btncollapse.Click += new System.EventHandler(this.btncollapse_Click);
            // 
            // btnexpand
            // 
            this.btnexpand.Location = new System.Drawing.Point(478, 12);
            this.btnexpand.Name = "btnexpand";
            this.btnexpand.Size = new System.Drawing.Size(24, 23);
            this.btnexpand.TabIndex = 4;
            this.btnexpand.Text = "-";
            this.btnexpand.UseVisualStyleBackColor = true;
            this.btnexpand.Click += new System.EventHandler(this.btnexpand_Click);
            // 
            // btnshow
            // 
            this.btnshow.Location = new System.Drawing.Point(353, 12);
            this.btnshow.Name = "btnshow";
            this.btnshow.Size = new System.Drawing.Size(52, 23);
            this.btnshow.TabIndex = 5;
            this.btnshow.Text = "Show";
            this.btnshow.UseVisualStyleBackColor = true;
            this.btnshow.Click += new System.EventHandler(this.btnshow_Click);
            // 
            // txtextensions
            // 
            this.txtextensions.Location = new System.Drawing.Point(97, 12);
            this.txtextensions.Name = "txtextensions";
            this.txtextensions.Size = new System.Drawing.Size(250, 20);
            this.txtextensions.TabIndex = 2;
            this.txtextensions.Text = "*";
            // 
            // txtextns
            // 
            this.txtextns.Location = new System.Drawing.Point(97, 39);
            this.txtextns.Name = "txtextns";
            this.txtextns.ReadOnly = true;
            this.txtextns.Size = new System.Drawing.Size(308, 20);
            this.txtextns.TabIndex = 6;
            this.txtextns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chksingle);
            this.panel1.Controls.Add(this.txtextensions);
            this.panel1.Controls.Add(this.txtextns);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnshow);
            this.panel1.Controls.Add(this.btncollapse);
            this.panel1.Controls.Add(this.btnexpand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 71);
            this.panel1.TabIndex = 7;
            // 
            // chksingle
            // 
            this.chksingle.AutoSize = true;
            this.chksingle.Location = new System.Drawing.Point(436, 41);
            this.chksingle.Name = "chksingle";
            this.chksingle.Size = new System.Drawing.Size(88, 17);
            this.chksingle.TabIndex = 7;
            this.chksingle.Text = "Merged View";
            this.chksingle.UseVisualStyleBackColor = true;
            this.chksingle.CheckedChanged += new System.EventHandler(this.chksingle_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.previewTree);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 71);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(537, 301);
            this.panel2.TabIndex = 8;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 372);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.Text = "Apply Patch Preview";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView previewTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncollapse;
        private System.Windows.Forms.Button btnexpand;
        private System.Windows.Forms.Button btnshow;
        private System.Windows.Forms.TextBox txtextensions;
        private System.Windows.Forms.TextBox txtextns;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chksingle;
    }
}