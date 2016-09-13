using System;
using System.IO;
using System.Threading;
using Common.ADSL1;


namespace Common.Net {
    public class AdslHelper {
        public static void ReConn() {
            var lines = File.ReadAllLines("config//adslconfig.txt");
            string name = lines[0];
            string pwd = lines[1];
            int seconds = Convert.ToInt32(lines[2]); //6
            int time = Convert.ToInt32(lines[3]); //5


            Common.ADSL1.cRASDisplay cRas = new cRASDisplay();
         //   LogManager.WriteLog("当前连接状态" + cRas.IsConnected);
            if (cRas.IsConnected) {
                cRas.Disconnect();
          //      LogManager.WriteLog("Disconnect");
                Thread.Sleep(5000);
                cRas.Connect("宽带连接");
                AdslConn(cRas, seconds, time);
            } else {
                AdslConn(cRas, seconds, time);
            }
        }

        private static bool AdslConn(cRASDisplay cRas, int seconds, int time) {
            bool isok = false;
            for (int i = 0; i < time; i++) {
                Thread.Sleep(seconds * 1000);
                if (cRas.IsConnected) {
                   
                    isok = true;
                    break;
                } else {
                    isok = false;
                    cRas.Connect("宽带连接");
                 //   LogManager.WriteLog("宽带连接" + (6 - i) + " 重试 倒计次数");
                }
            }
            return isok;
        }
    }
}