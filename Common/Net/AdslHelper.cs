using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Common.ADSL1;
using NUnit.Framework;

namespace Common.Net {
    public class AdslHelper {
        [Test]

        public static void TestReconn() {
            ConfigHelper.InitConfig("config//sysConfig.txt");
            // checkIpisOKBylocal();
            for (int i = 0; i < 50; i++) {
                ReConn();
            }
        }

        [Test]
        public static void ReConn() {
            ////Conn1();
            //var lines = File.ReadAllLines("config//adslconfig.txt");
            //string name = lines[0];
            //string pwd = lines[1];
            //int seconds = Convert.ToInt32(lines[2]); //6
            //int time = Convert.ToInt32(lines[3]); //5

            //RasApi ras = new RasApi();
            ////TODO:: 重新连接
            //LogManager.WriteLog("当前连接状态" + ras.IsConnected());
            //bool ipisok = false;
            //if (ras.IsConnected()) {
            //    ras.Disconnect();
            //    LogManager.WriteLog("Disconnect");
            //}
            //do {
            //    //  AdslConn(cRas, seconds, time);
            //    ipisok = ras.checkIpisOKBylocal(ip);
            //    ras.Conn();
            //} while (!ipisok);

            ////  LogManager.WriteLog(cRas.Duration);


            //string ip = "";
            //bool isok = ras.ReConn(out ip);
            //if (isok) {

            //}

        }
        /// <summary>
        /// 自动重拨，无需密码
        /// </summary>
        public static void ReConn2() {
            var lines = File.ReadAllLines("config//adslconfig.txt");
            string name = lines[0];
            string pwd = lines[1];
            int seconds = Convert.ToInt32(lines[2]); //6
            int time = Convert.ToInt32(lines[3]); //5

            cRASDisplay cRas = new cRASDisplay();
            LogManager.WriteLog("当前连接状态" + cRas.IsConnected);
            bool ipisok = false;
            if (cRas.IsConnected) {
                cRas.Disconnect();
                LogManager.WriteLog("Disconnect");
            }
            do {
                AdslConn(cRas, seconds, time);
                string key = RasApi.GetIP("宽带连接");
                LogManager.WriteLog("ip =" + key);
                ipisok = checkIpisOKBylocal(key);
            } while (!ipisok);

            LogManager.WriteLog(cRas.Duration);
        }

        private static Dictionary<string, DateTime> ipdDictionary = new Dictionary<string, DateTime>();
        private static List<ipModle> ips = new List<ipModle>();
        private static bool checkIpisOK() {
            //read file is ip exist
            string key = NetHostIp.GetAllIPs();
            Console.WriteLine(key);
            bool isok;
            //获取IP
            //   ips = GetipModels();
            if (ContainsKey(key, ips)) {
                Console.WriteLine("exist !");
                DateTime lastTime = ipdDictionary[key];  // ips.FirstOrDefault(_ => _.ip == key).lastTime;
                isok = (DateTime.Now - lastTime).TotalMinutes > int.Parse(ConfigHelper.GetValue("minutesOfsingleIp"));
                if (!isok) {
                    return false;
                }
            }
            ////添加
            //Savekey(key);
            ipdDictionary[key] = DateTime.Now;
            return true;
        }
        public static bool checkIpisOKBylocal(string ip) {
            //read file is ip exist
            if (ip == "") {
                return false;
            }
            var now = DateTime.Now;
            bool isok;
            //获取IP
            var xe = XElement.Load("config//ips.xml");
            var ipList = xe.Elements("ip");
            var ipitem = ipList.Where(_ => _.Attribute("ip").Value == ip);
            if (ipitem.Count() != 0)//存在
            {
                var firstOne = ipitem.FirstOrDefault();
                Console.WriteLine("exist !");
                DateTime lastTime = DateTime.Parse(firstOne.Attribute("lastTime").Value);
                isok = (DateTime.Now - lastTime).TotalMinutes > int.Parse(ConfigHelper.GetValue("minutesOfsingleIp"));
                if (isok) {
                    //时间合适，更新时间返回合适、
                    firstOne.Attribute("lastTime").SetValue(now.ToString());
                    //使用次数加1
                    int count = int.Parse(firstOne.Attribute("count").Value) + 1;
                    firstOne.Attribute("count").SetValue(count);
                } else {
                    return false;
                }
            } else {
                XElement nele = new XElement("ip");
                nele.SetAttributeValue("ip", ip);
                nele.SetAttributeValue("lastTime", now);
                nele.SetAttributeValue("count", 1);
                xe.Add(nele);
            }
            xe.Save("config//ips.xml");
            return true;
        }


        private static bool ContainsKey(string key, List<ipModle> ips) {


            return ipdDictionary.ContainsKey(key);
        }

        private static bool AdslConn(cRASDisplay cRas, int seconds, int time) {
            bool isok = false;
            int res1 = -1;
            for (int i = 0; i < time; i++) {
                Thread.Sleep(seconds * 1000);
                if (isok) {
                    LogManager.WriteLog("Connected");
                    break;
                }

                res1 = cRas.Connect("宽带连接");
                isok = (res1 == 0);
                LogManager.WriteLog("宽带连接重试 第" + (time - i) + " 次 上次拨号返回 " + res1);

            }
            return isok;
        }
    }
    public class ipModle {
        public string ip { get; set; }
        public DateTime lastTime { get; set; }
        public int count { get; set; }
    }



    public class UserAgentHelper {

        private string randomGetOne() {
            return "";
        }
        public string GetOne() {
            bool isok = false;
            var ua = randomGetOne();
            CheckUa(ua);
            return "";
        }

        private bool CheckUa(string ua) {

            //read file is ip exist
            if (ua == "") {
                return false;
            }
            var now = DateTime.Now;
            bool isok;
            //获取IP
            var xe = XElement.Load("config//uas.xml");
            var ipList = xe.Elements("ip");
            var ipitem = ipList.Where(_ => _.Attribute("ip").Value == ua);
            if (ipitem.Count() != 0)//存在
            {
                var firstOne = ipitem.FirstOrDefault();
                Console.WriteLine("exist !");
                DateTime lastTime = DateTime.Parse(firstOne.Attribute("lastTime").Value);
                isok = (DateTime.Now - lastTime).TotalMinutes > int.Parse(ConfigHelper.GetValue("minutesOfsingleUa"));
                if (isok) {
                    //时间合适，更新时间返回合适、
                    firstOne.Attribute("lastTime").SetValue(now.ToString());
                    //使用次数加1
                    int count = int.Parse(firstOne.Attribute("count").Value) + 1;
                    firstOne.Attribute("count").SetValue(count);
                } else {
                    return false;
                }
            } else {
                XElement nele = new XElement("ip");
                nele.SetAttributeValue("ip", ua);
                nele.SetAttributeValue("lastTime", now);
                nele.SetAttributeValue("count", 1);
                xe.Add(nele);
            }
            xe.Save("config//ips.xml");
            return true;

        }
    }
}