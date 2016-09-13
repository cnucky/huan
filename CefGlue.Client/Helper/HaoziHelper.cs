using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Common;
using Newtonsoft.Json;
using PwBusiness;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {
    public static class HaoziHelper {
        public static tb_tbzfb InitXiaoHaoFromLine(string line) {
            var l1s = line.Split('|');
            var tb = new tb_tbzfb();
            tb.tbName = l1s[0];
            tb.zfbEmail = l1s[1];
            //淘宝账号|淘宝登陆密码|支付宝登陆密码|邮箱（支付宝）账号|
            //邮箱密码|支付密码|身份证号 
            //淘宝名|支付宝账户名（邮箱名）|邮箱密码|支付宝登录密码|支付宝支付密码|身份证名|身份证号码|ok
            //密保问题1-1|密保问题1-2|密保1-答案
            tb.tbPwd = l1s[3];
            tb.zfbPwd = l1s[3];
            tb.zfbEmailPwd = l1s[2];
            tb.zfbPayPwd = l1s[4];
            tb.realname = l1s[5];
            tb.zfbSFZ = l1s[6];
            tb.mb_1_1 = l1s[7];
            tb.mb_1_2 = l1s[8];
            tb.mb_1_3 = l1s[9];

            tb.mb_2_1 = l1s[10];
            tb.mb_2_2 = l1s[11];
            tb.mb_2_3 = l1s[12];

            tb.mb_3_1 = l1s[13];
            tb.mb_3_2 = l1s[14];
            tb.mb_3_3 = l1s[15];
            return tb;
        }
        public static void UpdateLoadHaozi(string website, tb_tbzfb currentHaoZi1, string type) {

            currentHaoZi1.zfbStatus = ConfigHelper.GetValue("Author") + "|" + Environment.MachineName;
            string jsonstr = JsonConvert.SerializeObject(currentHaoZi1);
            WebResponse Response = null;

            try {
                //HttpUtility.UrlDecode(par, Encoding.GetEncoding("utf-8"));
                string url = "http://{0}/home/sql?type={2}&jsonstr={1}"
                    .With(website, HttpUtility.UrlEncode(jsonstr, Encoding.UTF8), type);
                var hwr = HttpWebRequest.Create(url);
                hwr.Timeout = 20000;
                Response = hwr.GetResponse();
            } catch (Exception e) {

                LogManager.WriteLog(e.ToString());
            }
        }

        internal static void ReportStatus(string jsonstr, string type) {
            WebResponse Response = null;
            try {
                //HttpUtility.UrlDecode(par, Encoding.GetEncoding("utf-8"));
                string url = "http://{0}/home/sql2?machinename={3}&type={2}&jsonstr={1}"
                    .With(ConfigHelper.GetValue("WebSite"), HttpUtility.UrlEncode(jsonstr, Encoding.UTF8), type,
                    ConfigHelper.GetValue("Author") + "|" + Environment.MachineName);
                var hwr = HttpWebRequest.Create(url);
                hwr.Timeout = 10000;
                Response = hwr.GetResponse();
            } catch (Exception e) {
                LogManager.WriteLog(e.ToString());
            }
        }

        public static void logAccount(List<tb_tbzfb> list, string infilename, string outfilename) {
            StringBuilder sb = new StringBuilder();

            foreach (var tbTbzfb in list) {
                //娇钟离逯简威7973|enhl7114233@163.com|svroevtmxk275|jol033670|gfp53130|张昊|130105198311201217|13|1|咎提|7|1|申屠鹿|1|3|圣杰小蓉|
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|\n"
                    .With(tbTbzfb.tbName, tbTbzfb.zfbEmail, tbTbzfb.zfbEmailPwd,
                        tbTbzfb.zfbPwd, tbTbzfb.zfbPayPwd, tbTbzfb.realname, tbTbzfb.zfbSFZ
                        , tbTbzfb.mb_1_1, tbTbzfb.mb_1_2, tbTbzfb.mb_1_3
                        , tbTbzfb.mb_2_1, tbTbzfb.mb_2_2, tbTbzfb.mb_2_3
                        , tbTbzfb.mb_3_1, tbTbzfb.mb_2_2, tbTbzfb.mb_3_3
                    ));
            }

            LogManager.WriteHaoZi(sb.ToString(), infilename);
            LogManager.WriteHaoZi("", outfilename);
        }


        public static List<tb_tbzfb> importAccounts() {
            List<tb_tbzfb> accounts = new List<tb_tbzfb>();
            List<string> pathList =
                FileHelper.read(Path.Combine(Application.StartupPath, "import", MainForm.taskRunner.getinfilename()),
                    Encoding.UTF8);
            List<string> pathList2 =
                FileHelper.read(Path.Combine(Application.StartupPath, "import", MainForm.taskRunner.getoutfilename()),
                    Encoding.UTF8);
            List<string> NameOfRed = new List<string>();
            foreach (var name in pathList2) {
                NameOfRed.Add(name.Split('|')[0]);
            }


            foreach (var line in pathList) {
                if (line.Contains("ok")) {
                    continue;
                }

                if (string.IsNullOrEmpty(line.Trim())) {
                    continue;
                }
                var tb = HaoziHelper.InitXiaoHaoFromLine(line);


                //剔除已经注册的
                bool exist = false;
                foreach (var aname in NameOfRed) {
                    if (aname == tb.tbName) {
                        exist = true;
                    }
                }
                if (exist) {
                    continue;
                }


                tb.tbStatus = "";
                accounts.Add(tb);
            }
            return accounts;
        }
    }
}