namespace Xilium.CefGlue.Client
{
    partial class ProdeuceAccountForm
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
            this.BT_produce = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TB_filePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_tb_pwd = new System.Windows.Forms.TextBox();
            this.TB_paypwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CB_UseOnePwd = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BT_produce
            // 
            this.BT_produce.Location = new System.Drawing.Point(54, 291);
            this.BT_produce.Name = "BT_produce";
            this.BT_produce.Size = new System.Drawing.Size(75, 23);
            this.BT_produce.TabIndex = 0;
            this.BT_produce.Text = "随机生成";
            this.BT_produce.UseVisualStyleBackColor = true;
            this.BT_produce.Click += new System.EventHandler(this.BT_produce_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(66, 346);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "测试解析";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(254, 346);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "排序";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TB_filePath
            // 
            this.TB_filePath.Location = new System.Drawing.Point(114, 60);
            this.TB_filePath.Name = "TB_filePath";
            this.TB_filePath.Size = new System.Drawing.Size(183, 21);
            this.TB_filePath.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "email文件名";
            // 
            // TB_tb_pwd
            // 
            this.TB_tb_pwd.Location = new System.Drawing.Point(114, 123);
            this.TB_tb_pwd.Name = "TB_tb_pwd";
            this.TB_tb_pwd.Size = new System.Drawing.Size(100, 21);
            this.TB_tb_pwd.TabIndex = 5;
            this.TB_tb_pwd.Text = "azz6611";
            // 
            // TB_paypwd
            // 
            this.TB_paypwd.Location = new System.Drawing.Point(114, 175);
            this.TB_paypwd.Name = "TB_paypwd";
            this.TB_paypwd.Size = new System.Drawing.Size(100, 21);
            this.TB_paypwd.TabIndex = 6;
            this.TB_paypwd.Text = "AZZ6611";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "淘宝密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "支付密码";
            // 
            // CB_UseOnePwd
            // 
            this.CB_UseOnePwd.AutoSize = true;
            this.CB_UseOnePwd.Location = new System.Drawing.Point(41, 90);
            this.CB_UseOnePwd.Name = "CB_UseOnePwd";
            this.CB_UseOnePwd.Size = new System.Drawing.Size(72, 16);
            this.CB_UseOnePwd.TabIndex = 9;
            this.CB_UseOnePwd.Text = "使用随机";
            this.CB_UseOnePwd.UseVisualStyleBackColor = true;
            // 
            // ProdeuceAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 407);
            this.Controls.Add(this.CB_UseOnePwd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_paypwd);
            this.Controls.Add(this.TB_tb_pwd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_filePath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BT_produce);
            this.Name = "ProdeuceAccountForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_produce;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TB_filePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_tb_pwd;
        private System.Windows.Forms.TextBox TB_paypwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CB_UseOnePwd;
    }
}

