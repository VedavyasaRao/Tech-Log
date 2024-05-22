namespace PortablePatchTool
{
    partial class Form4
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
            this.txtexlude = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chksource = new System.Windows.Forms.CheckBox();
            this.btnadd = new System.Windows.Forms.Button();
            this.lstfiles = new System.Windows.Forms.ListBox();
            this.btnremove = new System.Windows.Forms.Button();
            this.cmdaddfiles = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtimport = new System.Windows.Forms.TextBox();
            this.btnimport = new System.Windows.Forms.Button();
            this.btnimportarchive = new System.Windows.Forms.Button();
            this.grppatch = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fdargstxt = new System.Windows.Forms.TextBox();
            this.fdexebtn = new System.Windows.Forms.Button();
            this.fdexetxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.filestype = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grppatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtexlude
            // 
            this.txtexlude.Location = new System.Drawing.Point(12, 31);
            this.txtexlude.Name = "txtexlude";
            this.txtexlude.Size = new System.Drawing.Size(519, 20);
            this.txtexlude.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(456, 502);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Exclude All";
            // 
            // chksource
            // 
            this.chksource.AutoSize = true;
            this.chksource.Location = new System.Drawing.Point(15, 273);
            this.chksource.Name = "chksource";
            this.chksource.Size = new System.Drawing.Size(98, 17);
            this.chksource.TabIndex = 0;
            this.chksource.Text = "Include Source";
            this.chksource.UseVisualStyleBackColor = true;
            // 
            // btnadd
            // 
            this.btnadd.Location = new System.Drawing.Point(380, 19);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(45, 23);
            this.btnadd.TabIndex = 7;
            this.btnadd.Text = "+";
            this.btnadd.UseVisualStyleBackColor = true;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // lstfiles
            // 
            this.lstfiles.FormattingEnabled = true;
            this.lstfiles.HorizontalScrollbar = true;
            this.lstfiles.Location = new System.Drawing.Point(6, 50);
            this.lstfiles.Name = "lstfiles";
            this.lstfiles.Size = new System.Drawing.Size(483, 212);
            this.lstfiles.TabIndex = 1;
            // 
            // btnremove
            // 
            this.btnremove.Location = new System.Drawing.Point(431, 19);
            this.btnremove.Name = "btnremove";
            this.btnremove.Size = new System.Drawing.Size(44, 23);
            this.btnremove.TabIndex = 8;
            this.btnremove.Text = "-";
            this.btnremove.UseVisualStyleBackColor = true;
            this.btnremove.Click += new System.EventHandler(this.btnremove_Click);
            // 
            // cmdaddfiles
            // 
            this.cmdaddfiles.Location = new System.Drawing.Point(329, 19);
            this.cmdaddfiles.Name = "cmdaddfiles";
            this.cmdaddfiles.Size = new System.Drawing.Size(45, 23);
            this.cmdaddfiles.TabIndex = 9;
            this.cmdaddfiles.Text = "...";
            this.cmdaddfiles.UseVisualStyleBackColor = true;
            this.cmdaddfiles.Click += new System.EventHandler(this.cmdaddfiles_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 314);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Import File List:";
            // 
            // txtimport
            // 
            this.txtimport.Location = new System.Drawing.Point(96, 311);
            this.txtimport.Name = "txtimport";
            this.txtimport.ReadOnly = true;
            this.txtimport.Size = new System.Drawing.Size(349, 20);
            this.txtimport.TabIndex = 19;
            // 
            // btnimport
            // 
            this.btnimport.Location = new System.Drawing.Point(451, 309);
            this.btnimport.Name = "btnimport";
            this.btnimport.Size = new System.Drawing.Size(27, 23);
            this.btnimport.TabIndex = 20;
            this.btnimport.Text = "...";
            this.btnimport.UseVisualStyleBackColor = true;
            this.btnimport.Click += new System.EventHandler(this.btnimport_Click);
            // 
            // btnimportarchive
            // 
            this.btnimportarchive.Location = new System.Drawing.Point(374, 273);
            this.btnimportarchive.Name = "btnimportarchive";
            this.btnimportarchive.Size = new System.Drawing.Size(115, 23);
            this.btnimportarchive.TabIndex = 21;
            this.btnimportarchive.Text = "Import Patch Archive";
            this.btnimportarchive.UseVisualStyleBackColor = true;
            this.btnimportarchive.Visible = false;
            this.btnimportarchive.Click += new System.EventHandler(this.btnimportarchive_Click);
            // 
            // grppatch
            // 
            this.grppatch.Controls.Add(this.label2);
            this.grppatch.Controls.Add(this.filestype);
            this.grppatch.Controls.Add(this.btnimportarchive);
            this.grppatch.Controls.Add(this.btnimport);
            this.grppatch.Controls.Add(this.txtimport);
            this.grppatch.Controls.Add(this.label5);
            this.grppatch.Controls.Add(this.cmdaddfiles);
            this.grppatch.Controls.Add(this.btnremove);
            this.grppatch.Controls.Add(this.lstfiles);
            this.grppatch.Controls.Add(this.btnadd);
            this.grppatch.Controls.Add(this.chksource);
            this.grppatch.Location = new System.Drawing.Point(15, 68);
            this.grppatch.Name = "grppatch";
            this.grppatch.Size = new System.Drawing.Size(516, 354);
            this.grppatch.TabIndex = 8;
            this.grppatch.TabStop = false;
            this.grppatch.Text = "Create Patch";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 470);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Arguments";
            // 
            // fdargstxt
            // 
            this.fdargstxt.Location = new System.Drawing.Point(133, 463);
            this.fdargstxt.Name = "fdargstxt";
            this.fdargstxt.Size = new System.Drawing.Size(318, 20);
            this.fdargstxt.TabIndex = 26;
            // 
            // fdexebtn
            // 
            this.fdexebtn.Location = new System.Drawing.Point(457, 435);
            this.fdexebtn.Name = "fdexebtn";
            this.fdexebtn.Size = new System.Drawing.Size(27, 23);
            this.fdexebtn.TabIndex = 29;
            this.fdexebtn.Text = "...";
            this.fdexebtn.UseVisualStyleBackColor = true;
            this.fdexebtn.Click += new System.EventHandler(this.fdexebtn_Click);
            // 
            // fdexetxt
            // 
            this.fdexetxt.Location = new System.Drawing.Point(133, 437);
            this.fdexetxt.Name = "fdexetxt";
            this.fdexetxt.ReadOnly = true;
            this.fdexetxt.Size = new System.Drawing.Size(318, 20);
            this.fdexetxt.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 441);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "File difference Viewer";
            // 
            // filestype
            // 
            this.filestype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filestype.FormattingEnabled = true;
            this.filestype.Items.AddRange(new object[] {
            "Additional",
            "Remove"});
            this.filestype.Location = new System.Drawing.Point(6, 23);
            this.filestype.Name = "filestype";
            this.filestype.Size = new System.Drawing.Size(121, 21);
            this.filestype.TabIndex = 22;
            this.filestype.SelectedIndexChanged += new System.EventHandler(this.filestype_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(133, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Files";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 537);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.fdargstxt);
            this.Controls.Add(this.fdexebtn);
            this.Controls.Add(this.fdexetxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grppatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtexlude);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form4";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.grppatch.ResumeLayout(false);
            this.grppatch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtexlude;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chksource;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.ListBox lstfiles;
        private System.Windows.Forms.Button btnremove;
        private System.Windows.Forms.Button cmdaddfiles;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtimport;
        private System.Windows.Forms.Button btnimport;
        private System.Windows.Forms.Button btnimportarchive;
        private System.Windows.Forms.GroupBox grppatch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fdargstxt;
        private System.Windows.Forms.Button fdexebtn;
        private System.Windows.Forms.TextBox fdexetxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox filestype;
    }
}