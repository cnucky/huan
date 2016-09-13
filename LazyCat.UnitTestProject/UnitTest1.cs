using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwBusiness;

namespace LazyCat.UnitTestProject {
    [TestClass]
    public class UnitTest1 {
        /// <summary>
        /// 分割url成键值对 wwww.bai.com/1.aspx?a=1&b=2&c=3
        /// </summary>
        [TestMethod]
        public void TestMethod1() {
            string url1 = "wwww.bai.com/1.aspx?a=1&b=2&c=3";
            var d1 = ext1(url1);

            Console.WriteLine(d1.ToString());
            return;

        }


        [TestMethod]
        public void tt1() {
            string clasName = "clasName";
            string cookieName = "cookieName";

            string jsSetElemets2Cookies = (
                           "var a1=document.getElementsByClassName(" + clasName + ");var b1='';for(i=0;i<a1.length;i++ ){ b1+=a1[i].href+'|'; } document.cookie='" + cookieName + "'+b1;"
                               );
            Console.WriteLine(jsSetElemets2Cookies);
        }




        private static Dictionary<string, string> ext1(string url1) {
            var par = url1.Split('?')[1];
            var par1 = par.Split('&');
            Dictionary<string, string> d1 = new Dictionary<string, string>();
            foreach (var s in par1) {
                d1.Add(s.Split('=')[0], s.Split('=')[1]);
                Console.WriteLine(s.Split('=')[0] + " " + s.Split('=')[1]);
            }
            return d1;
        }

        /// <summary>
        /// 从文本中解析出我想要的文本
        /// </summary>
        [TestMethod]
        public void TestMethod3() {
            HtmlDocument doc = new HtmlDocument();



            doc.Load("html_zfb.txt");

            var alist = doc.DocumentNode.Descendants("a");
            foreach (var htmlNode in alist) {

                if (htmlNode.InnerText == "继续注册") {
                    Console.WriteLine(htmlNode.Attributes["href"].Value);
                    break;
                }

            }
            //  HtmlNode node = doc.;

            //Console.WriteLine(node.OuterHtml);
            //Console.WriteLine(node.InnerHtml);

            return;

        }
    }
}
