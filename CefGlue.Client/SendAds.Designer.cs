namespace Xilium.CefGlue.Client {
    partial class SendAds {
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
            this.BT_LoadUrls = new System.Windows.Forms.Button();
            this.BT_Add_talk = new System.Windows.Forms.Button();
            this.TB_pageNum = new System.Windows.Forms.TextBox();
            this.TB_HY_Code = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BT_Analysis_html = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CB_qq = new System.Windows.Forms.ComboBox();
            this.BT_PP_Chat = new System.Windows.Forms.Button();
            this.BT_Qq_f5 = new System.Windows.Forms.Button();
            this.LB_Load = new System.Windows.Forms.ToolStripStatusLabel();
            this.TB_ShopCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BT_LoadUrls
            // 
            this.BT_LoadUrls.Location = new System.Drawing.Point(357, 12);
            this.BT_LoadUrls.Name = "BT_LoadUrls";
            this.BT_LoadUrls.Size = new System.Drawing.Size(75, 23);
            this.BT_LoadUrls.TabIndex = 0;
            this.BT_LoadUrls.Text = "导航至页面";
            this.BT_LoadUrls.UseVisualStyleBackColor = true;
            this.BT_LoadUrls.Click += new System.EventHandler(this.BT_LoadUrls_Click);
            // 
            // BT_Add_talk
            // 
            this.BT_Add_talk.Location = new System.Drawing.Point(557, 90);
            this.BT_Add_talk.Name = "BT_Add_talk";
            this.BT_Add_talk.Size = new System.Drawing.Size(199, 23);
            this.BT_Add_talk.TabIndex = 1;
            this.BT_Add_talk.Text = "添加聊天图标·网页端的";
            this.BT_Add_talk.UseVisualStyleBackColor = true;
            this.BT_Add_talk.Click += new System.EventHandler(this.BT_Add_talk_Click);
            // 
            // TB_pageNum
            // 
            this.TB_pageNum.Location = new System.Drawing.Point(64, 15);
            this.TB_pageNum.Name = "TB_pageNum";
            this.TB_pageNum.Size = new System.Drawing.Size(35, 21);
            this.TB_pageNum.TabIndex = 2;
            this.TB_pageNum.Text = "2";
            // 
            // TB_HY_Code
            // 
            this.TB_HY_Code.Location = new System.Drawing.Point(180, 15);
            this.TB_HY_Code.Name = "TB_HY_Code";
            this.TB_HY_Code.Size = new System.Drawing.Size(67, 21);
            this.TB_HY_Code.TabIndex = 3;
            this.TB_HY_Code.Text = "20501";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "页码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "行业代码";
            // 
            // BT_Analysis_html
            // 
            this.BT_Analysis_html.Enabled = false;
            this.BT_Analysis_html.Location = new System.Drawing.Point(476, 12);
            this.BT_Analysis_html.Name = "BT_Analysis_html";
            this.BT_Analysis_html.Size = new System.Drawing.Size(75, 24);
            this.BT_Analysis_html.TabIndex = 6;
            this.BT_Analysis_html.Text = "调试分析";
            this.BT_Analysis_html.UseVisualStyleBackColor = true;
            this.BT_Analysis_html.Click += new System.EventHandler(this.BT_Analysis_html_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(557, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "下载";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LB_Load});
            this.statusStrip1.Location = new System.Drawing.Point(0, 541);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(761, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 119);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(761, 422);
            this.dataGridView1.TabIndex = 9;
            // 
            // CB_qq
            // 
            this.CB_qq.FormattingEnabled = true;
            this.CB_qq.Location = new System.Drawing.Point(38, 93);
            this.CB_qq.Name = "CB_qq";
            this.CB_qq.Size = new System.Drawing.Size(121, 20);
            this.CB_qq.TabIndex = 10;
            // 
            // BT_PP_Chat
            // 
            this.BT_PP_Chat.Location = new System.Drawing.Point(172, 58);
            this.BT_PP_Chat.Name = "BT_PP_Chat";
            this.BT_PP_Chat.Size = new System.Drawing.Size(75, 23);
            this.BT_PP_Chat.TabIndex = 11;
            this.BT_PP_Chat.Text = "发起聊天";
            this.BT_PP_Chat.UseVisualStyleBackColor = true;
            this.BT_PP_Chat.Click += new System.EventHandler(this.BT_PP_Chat_Click);
            // 
            // BT_Qq_f5
            // 
            this.BT_Qq_f5.Location = new System.Drawing.Point(651, 15);
            this.BT_Qq_f5.Name = "BT_Qq_f5";
            this.BT_Qq_f5.Size = new System.Drawing.Size(75, 23);
            this.BT_Qq_f5.TabIndex = 12;
            this.BT_Qq_f5.Text = "刷新";
            this.BT_Qq_f5.UseVisualStyleBackColor = true;
            this.BT_Qq_f5.Click += new System.EventHandler(this.BT_Qq_f5_Click);
            // 
            // LB_Load
            // 
            this.LB_Load.Name = "LB_Load";
            this.LB_Load.Size = new System.Drawing.Size(32, 17);
            this.LB_Load.Text = "状态";
            // 
            // TB_ShopCount
            // 
            this.TB_ShopCount.Location = new System.Drawing.Point(64, 42);
            this.TB_ShopCount.Name = "TB_ShopCount";
            this.TB_ShopCount.Size = new System.Drawing.Size(68, 21);
            this.TB_ShopCount.TabIndex = 13;
            this.TB_ShopCount.Text = "20";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "店铺数";
            // 
            // SendAds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 563);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_ShopCount);
            this.Controls.Add(this.BT_Qq_f5);
            this.Controls.Add(this.BT_PP_Chat);
            this.Controls.Add(this.CB_qq);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BT_Analysis_html);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_HY_Code);
            this.Controls.Add(this.TB_pageNum);
            this.Controls.Add(this.BT_Add_talk);
            this.Controls.Add(this.BT_LoadUrls);
            this.Name = "SendAds";
            this.Text = "SendAds";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_LoadUrls;
        private System.Windows.Forms.Button BT_Add_talk;
        private System.Windows.Forms.TextBox TB_pageNum;
        private System.Windows.Forms.TextBox TB_HY_Code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BT_Analysis_html;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox CB_qq;
        private System.Windows.Forms.Button BT_PP_Chat;
        private System.Windows.Forms.Button BT_Qq_f5;
        private System.Windows.Forms.ToolStripStatusLabel LB_Load;
        private System.Windows.Forms.TextBox TB_ShopCount;
        private System.Windows.Forms.Label label3;
    }
}