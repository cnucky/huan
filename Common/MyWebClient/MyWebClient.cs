using System;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Common {
    public static class Extends {
        public static string With(this string str, params object[] strparStrings) {
            return string.Format(str, strparStrings);

        }
        public static string ReplaceNum(this string str) {
            if (str.Contains("{两位数字}")) {
                str = str.Replace("{两位数字}", RandomManager.GetZero2Nine() + RandomManager.GetZero2Nine());
            }
            if (str.Contains("{一位数字}")) {
                str = str.Replace("{一位数字}", RandomManager.GetZero2Nine()  );
            }
            return str;

        }
    }
    public class MyWebClient {



        public void UT_DownHaoZi() {

            //  DownHaoZi("hina.com", "name", "pwd", 11);
        }

        public string DownHaoZi(string url) {
            string res = "";
            using (WebClient webClient = new WebClient()) {

                try {
                    var res1 = webClient.DownloadData(url);

                    res = System.Text.Encoding.UTF8.GetString(res1);


                } catch (Exception e1) {
                    LogManager.WriteLog(e1.ToString());
                }

            }
            return res;

        }
    }
}