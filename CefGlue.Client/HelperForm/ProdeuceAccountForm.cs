using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using Common;
using PwBusiness;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {
    public partial class ProdeuceAccountForm : Form {
        public ProdeuceAccountForm() {
            InitializeComponent();
        }

        //生成带email的账户密码 
        private void BT_produce_Click(object sender, EventArgs e) {



            if (TB_tb_pwd.Text.Trim() == TB_paypwd.Text.Trim()) {
                MessageBox.Show("傻×,你的C盘将被格式化，如果你继续将把2个密码设成一样的话？", "傻x", MessageBoxButtons.YesNoCancel);
                return;
            }
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
            List<string> strTBnamelist = Common.FileHelper1.readstringAfterEqual("account//name.txt");
            //ShenFenZheng
            var strIds = FileHelper1.readstringAfter("account//ids.txt", ',');




            //邮箱-2014年5月28日14时6分4秒[aqq]

            var emailList = File.ReadAllLines("account//" + TB_filePath.Text.Trim()).ToList();
            emailList = emailList.Distinct().ToList();
            //读取已经注册好的email
            List<string> lines = new List<string>();
            for (int i = 0; i < emailList.Count; i++) {
                // string res1 = ProduceRandomStr(1, 1, headerList.ToArray()) + ProduceRandomStr(8, 17, bodyList.ToArray());

                string tbname = RandomManager.ProduceRandomStr(2, 5, strTBnamelist.ToArray()) + RandomManager.ProduceRandomStr(4, 9, bodyList.ToArray());

                string tbpwd = "";//= RandomManager.getPwd();
                string paypwd = "";// RandomManager.getPwd();
                if (CB_UseOnePwd.Checked) {
                    tbpwd = RandomManager.getPwd();
                    paypwd = RandomManager.getPwd();
                    while (tbpwd == paypwd) {
                        paypwd = RandomManager.getPwd();
                    }
                } else {
                    tbpwd = TB_tb_pwd.Text.Trim();
                    paypwd = TB_paypwd.Text.Trim();
                }




                List<string> sqsList = new List<string>();
                while (sqsList.Count < 3) {
                    var sq = RandomManager.GetSQ();
                    bool isTheSameQ = sqsList.Where(_ => _.Split('|')[0] == sq.Split('|')[0] || _.Split('|')[2] == sq.Split('|')[2]).Count() > 0;
                    if (isTheSameQ) {
                        continue;
                    }
                    sqsList.Add(sq);
                }

                string s1 = tbname + "|" //
                    + emailList[i].Split('|')[0] + "|"//邮箱名
                    + emailList[i].Split('|')[1] + "|"//邮箱密码
                    + tbpwd + "|"//淘宝密码
                    + paypwd + "|"//支付密码

                    + RandomIdAndName(strIds) + "|"
                    + sqsList[0]
                    + sqsList[1]
                    + sqsList[2]
                    + Environment.NewLine;


                lines.Add(s1);
                Console.Write(s1);
                //   支付宝账户名（邮箱名）|邮箱密码|淘宝账户名|
                //淘宝名|支付宝账户名（邮箱名）|邮箱密码|支付宝登录密码|支付宝支付密码|身份证名|身份证号码
                FileHelper1.log("account//待注册账号.txt", s1);
            }

            new Thread(() => {
                int i = 1;

                foreach (var line in lines) {
                    try {
                        var model = HaoziHelper.InitXiaoHaoFromLine(line);

                        HaoziHelper.UpdateLoadHaozi(ConfigHelper.GetValue("WebSite"), model, "add2origin");
                        Thread.Sleep(300);
                        LogManager.WriteLog("haozi", "oky|" + line);
                    } catch (Exception e2) {
                        LogManager.WriteLog(e2.ToString());
                        LogManager.WriteLog("haozi", "bad|" + line);
                        LogManager.WriteLog("上传失败");

                    }
                    Console.WriteLine(i++);
                }
                LogManager.WriteLog("上传结束");

            }).Start();


        }

        private string RandomIdAndName(List<Tuple<string, string>> strIds) {
            string idAndName = "";
            Tuple<string, string> item = new Tuple<string, string>("", "");
            item = strIds[RandomManager.random.Next(strIds.Count - 2)];
            idAndName = item.Item1 + "|" + ShenFenZheng.ProduceShenFenZheng();
            return idAndName;
        }

        private void button2_Click(object sender, EventArgs e) {
            //排序文本中的账号，使得文本中每个账户只出现一次
            //并且出现的位置是第一次出现的位置，然后输出到新文本
            var strIds = Common.FileHelper1.readstringAfter3("2014-4-18之前的账号.txt", ',');
            foreach (var tuple in strIds) {
                string res1 = tuple.Item1 + " ," + tuple.Item2 + "," + tuple.Item3 + Environment.NewLine;
                LogManager.WriteLog(res1);
                FileHelper1.log("res_account.txt", res1);
            }

        }

        private void button1_Click(object sender, EventArgs e) {
            var strTBnamelist = Common.FileHelper1.readstringAfter3("t1.txt", '|');
            foreach (var tuple in strTBnamelist) {
                var zfbname = tuple.Item3.Split('|')[0].Trim();
                LogManager.WriteLog("document.getElementById('payerInput').value='" + zfbname + "';document.getElementById('addPayer').click();");
            }
        }
    }
}
