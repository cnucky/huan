using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwBusiness.PP;

namespace LazyCat.UnitTestProject {
    [TestClass]
    public class UnitTest2 {


        [TestMethod]
        public void TestMethod1() {
            var f1 = File.ReadAllText(
@"D:\01 project\3 FastQQ\xl\xilium-xilium.cefglue-820e4919a0f1\LazyCat.UnitTestProject\bin\Debug\tmp\pp.html",
Encoding.GetEncoding("GB2312"));
            var shops = PPhelper.PpShopsExtarct(f1);
            foreach (var ppShop in shops) {
                Console.WriteLine(ppShop.titile + " " + ppShop.mainseller_sell + " " + ppShop.mainseller_owner + " " + ppShop.adress);
            }
        }



        [TestMethod]
        public void TestM2() {
            var f1 = File.ReadAllText(
@"D:\01 project\3 FastQQ\xl\xilium-xilium.cefglue-820e4919a0f1\LazyCat.UnitTestProject\bin\Debug\tmp\pp.html",
Encoding.GetEncoding("GB2312"));
            File.WriteAllText("pplog//" + DateTime.Now.ToString("yyyyMMddHHmmssff"), f1);

        }

        [TestMethod]
        public void TestExtractImtalkInfo() {
            string s1 =
                "javascript:imTalk('1255462811','','7b354ff3727290e8f2c91fb60d25159900bf99927173bbac538e458beb8e4e4d1af0afbbeeea326f','a58c667797f25181365c7326a127f8384bced1682ee41178')";
            var res = s1.Split('\'');
            string id = res[1];
            string id5 = res[5];
            string id7 = res[7];

            Console.WriteLine(id);
            Console.WriteLine(id5);
            Console.WriteLine(id7);

        }

        [TestMethod]
        public void TestPopQq() {
            var f1 = File.ReadAllText(
                @"D:\01 project\3 FastQQ\xl\xilium-xilium.cefglue-820e4919a0f1\LazyCat.UnitTestProject\bin\Debug\tmp\pp.html",
                Encoding.GetEncoding("GB2312"));
            var shops = PPhelper.PpShopsExtarct(f1);
            foreach (var ppShop in shops) {
                Console.WriteLine(ppShop.titile + " " + ppShop.mainseller_sell + " " + ppShop.mainseller_owner + " " + ppShop.adress);
                break;
            }
            var shop = shops.FirstOrDefault();
            PPhelper.PopQq(shop);
        }
        [TestMethod]
        public void TestCheckIsQqOnLine() {
            var f1 = File.ReadAllText(
                @"D:\01 project\3 FastQQ\xl\xilium-xilium.cefglue-820e4919a0f1\LazyCat.UnitTestProject\bin\Debug\tmp\pp.html",
                Encoding.GetEncoding("GB2312"));
            var shops = PPhelper.PpShopsExtarct(f1);
            foreach (var ppShop in shops) {
                Console.WriteLine(ppShop.titile + " " + ppShop.mainseller_sell + " " + ppShop.mainseller_owner + " " + ppShop.adress);
                break;
            }
            var shop = shops.FirstOrDefault();

            //http://wpa.paipai.com/pa?p=1:2090769694:17

            foreach (PpShop ppShop in shops)
            {
                var online = PPhelper.IsShopOnline(ppShop);
                Console.WriteLine(online);
            }
          

          
            // shop.qqnum
            //PPhelper.PopQq(shop);
        }

       
    }
}
