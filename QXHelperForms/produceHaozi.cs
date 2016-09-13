using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using SYManager;

namespace QXHelperForms {
    public partial class produceHaozi : Form {
        public produceHaozi() {
            InitializeComponent();
        }
        Random random = new Random(DateTime.Now.Millisecond);
        private string ProduceRandomStr(int start, int end, string[] strList) {
            string res = "";
            int length = random.Next(start, end);



            //其余部分字符
            for (int i = 0; i < length; i++) {
                int index = random.Next(strList.Length - 1);

                res += strList[index];

            }
            return res;

            //6-18个字符，可使用字母 数字 下划线，需字母开头

        }
        //生成带email的账户密码 
        private void BT_produce_Click(object sender, EventArgs e) {

            List<string> bodyList = new List<string>();
            List<string> headerList = new List<string>();
            //淘宝名
            List<string> tbnameList = new List<string>();
            //添加数字
            for (int i = 0; i < 10; i++) {
                bodyList.Add(i.ToString());
            }
            //添加小写字母
            for (int i = Convert.ToInt32('a'); i <= Convert.ToInt32('z'); i++) {
                bodyList.Add(((char)i).ToString());
                headerList.Add(((char)i).ToString());
            }
            //添加下划线
            bodyList.Add("_");
            //   bodyList.Add("_");
            //中文账户名
            List<string> strTBnamelist = Common.FileHelper1.readstringAfterEqual("name.txt");
            //ShenFenZheng
            var strIds = FileHelper1.readstringAfter("ids.txt", ',');




            //邮箱-2014年5月28日14时6分4秒[aqq]

            var emailList = File.ReadAllLines(TB_filePath.Text.Trim()).ToList();
            //读取已经注册好的email
            for (int i = 0; i < emailList.Count; i++) {
                // string res1 = ProduceRandomStr(1, 1, headerList.ToArray()) + ProduceRandomStr(8, 17, bodyList.ToArray());

                string tbname = ProduceRandomStr(2, 4, strTBnamelist.ToArray()) + ProduceRandomStr(3, 5, bodyList.ToArray());

                string s1 = tbname + "|" //
                    + emailList[i].Split('|')[0] + "|"//邮箱名
                    + emailList[i].Split('|')[1] + "|"//邮箱密码

                    + TB_tb_pwd.Text.Trim() + "|"
                    + TB_paypwd.Text.Trim() + "|"

                    + RandomIdAndName(strIds) + "|"
                    + Environment.NewLine;

                Console.Write(s1);
                //   支付宝账户名（邮箱名）|邮箱密码|淘宝账户名|
                //淘宝名|支付宝账户名（邮箱名）|邮箱密码|支付宝登录密码|支付宝支付密码|身份证名|身份证号码
                Common.FileHelper1.log("emailname_with_id.txt", s1);
            }



        }

        private string RandomIdAndName(List<Tuple<string, string>> strIds) {
            string idAndName = "";
            Tuple<string, string> item = new Tuple<string, string>("", "");
            item = strIds[random.Next(strIds.Count - 2)];
            idAndName = item.Item1 + "|" + item.Item2;
            return idAndName;
        }

        private void button2_Click(object sender, EventArgs e) {
            //排序文本中的账号，使得文本中每个账户只出现一次
            //并且出现的位置是第一次出现的位置，然后输出到新文本
            var strIds = Common.FileHelper1.readstringAfter3("2014-4-18之前的账号.txt", ',');
            foreach (var tuple in strIds) {
                string res1 = tuple.Item1 + " ," + tuple.Item2 + "," + tuple.Item3 + Environment.NewLine;
                Console.WriteLine(res1);
                FileHelper1.log("res_account.txt", res1);
            }

        }

        private void button1_Click(object sender, EventArgs e) {
            var strTBnamelist = Common.FileHelper1.readstringAfter3("t1.txt", '|');
            foreach (var tuple in strTBnamelist) {
                var zfbname = tuple.Item3.Split('|')[0].Trim();
                Console.WriteLine("document.getElementById('payerInput').value='" + zfbname + "';document.getElementById('addPayer').click();");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BindAccount f1 = new BindAccount();
            f1.Show();
        }
    }
}
