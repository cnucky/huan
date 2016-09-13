namespace Xilium.CefGlue.Client {
    partial class AccountFilter {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.BT_Go = new System.Windows.Forms.Button();
            this.TB_FileName = new System.Windows.Forms.TextBox();
            this.TB_contain = new System.Windows.Forms.TextBox();
            this.TB_Filter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TB_result = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BT_Go
            // 
            this.BT_Go.Location = new System.Drawing.Point(532, 107);
            this.BT_Go.Name = "BT_Go";
            this.BT_Go.Size = new System.Drawing.Size(75, 23);
            this.BT_Go.TabIndex = 0;
            this.BT_Go.Text = "GO";
            this.BT_Go.UseVisualStyleBackColor = true;
            this.BT_Go.Click += new System.EventHandler(this.BT_Go_Click);
            // 
            // TB_FileName
            // 
            this.TB_FileName.Location = new System.Drawing.Point(105, 46);
            this.TB_FileName.Name = "TB_FileName";
            this.TB_FileName.Size = new System.Drawing.Size(250, 21);
            this.TB_FileName.TabIndex = 1;
            // 
            // TB_contain
            // 
            this.TB_contain.Location = new System.Drawing.Point(105, 104);
            this.TB_contain.Name = "TB_contain";
            this.TB_contain.Size = new System.Drawing.Size(100, 21);
            this.TB_contain.TabIndex = 2;
            this.TB_contain.Text = "|";
            // 
            // TB_Filter
            // 
            this.TB_Filter.Location = new System.Drawing.Point(313, 103);
            this.TB_Filter.Name = "TB_Filter";
            this.TB_Filter.Size = new System.Drawing.Size(100, 21);
            this.TB_Filter.TabIndex = 3;
            this.TB_Filter.Text = "#";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "文件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "包含";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(246, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "不包含";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "文件放置account下";
            // 
            // TB_result
            // 
            this.TB_result.Location = new System.Drawing.Point(38, 211);
            this.TB_result.Multiline = true;
            this.TB_result.Name = "TB_result";
            this.TB_result.Size = new System.Drawing.Size(569, 270);
            this.TB_result.TabIndex = 8;
            // 
            // AccountFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 539);
            this.Controls.Add(this.TB_result);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_Filter);
            this.Controls.Add(this.TB_contain);
            this.Controls.Add(this.TB_FileName);
            this.Controls.Add(this.BT_Go);
            this.Name = "AccountFilter";
            this.Text = "AccountFilter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_Go;
        public System.Windows.Forms.TextBox TB_FileName;
        private System.Windows.Forms.TextBox TB_contain;
        private System.Windows.Forms.TextBox TB_Filter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_result;
    }
}