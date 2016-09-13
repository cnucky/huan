using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PwBusiness;

namespace Xilium.CefGlue.Client {
    public partial class AccountFilter : Form {
        public AccountFilter() {
            InitializeComponent();
        }

        private void BT_Go_Click(object sender, EventArgs e) {
            var list = FileHelper.read("account//" + TB_FileName.Text);

            string contian = TB_contain.Text.Trim();
            string notContain = TB_Filter.Text.Trim();

            List<string> resList = new List<string>();

            //过滤
            foreach (var item in list) {
                if (item.Contains(contian) && !item.Contains(notContain)) {
                    resList.Add(item);
                }
            }
            //呈现
            StringBuilder sb = new StringBuilder();
            resList.ForEach(_ => sb.AppendLine(_));
            TB_result.Text = sb.ToString();

        }
    }
}
