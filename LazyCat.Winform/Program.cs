using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common.ADSL1;
using Common.Net;

namespace LazyCat.Winform {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            KeepAlive();
        }

        private static void KeepAlive() {
            new Thread(
                () => {
                    bool isDisConn = false;
                    int count = 0;
                    Common.ADSL1.cRASDisplay cRas = new cRASDisplay();
                    while (true) {
                        Thread.Sleep(1000 * 10);
                        
                        if (cRas.IsConnected) {
                            count = 0;
                           
                        } else {
                            count++;
                            if (count > 6) {
                                AdslHelper.ReConn();
                                Process.Start("restart.bat");
                            }
                        }

                        
                    }
                }).Start();
        }
    }
}
