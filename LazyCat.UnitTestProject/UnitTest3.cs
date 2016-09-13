using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyCat.UnitTestProject {
    [TestClass]
    public class UnitTest3 {
        [DllImport("User32")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);


        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

        public List<string> GetRunApplicationList(Form appForm) {
            try {
                List<string> appString = new List<string>();
                int handle = (int)appForm.Handle;
                int hwCurr;
                hwCurr = GetWindow(handle, GW_HWNDFIRST);
                while (hwCurr > 0) {
                    int isTask = (WS_VISIBLE | WS_BORDER);
                    int lngStyle = GetWindowLongA(hwCurr, GWL_STYLE);
                    bool taskWindow = ((lngStyle & isTask) == isTask);
                    if (taskWindow) {
                        int length = GetWindowTextLength(new IntPtr(hwCurr));
                        StringBuilder sb = new StringBuilder(2 * length + 1);
                        GetWindowText(hwCurr, sb, sb.Capacity);
                        string strTitle = sb.ToString();
                        if (!string.IsNullOrEmpty(strTitle)) {
                            appString.Add(strTitle);
                        }
                    }
                    hwCurr = GetWindow(hwCurr, GW_HWNDNEXT);
                }
                return appString;
            } catch (Exception ex) {
                throw new Exception("读取应用程序信息时出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 列出所有可访问进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnList_Click() {
            List<string> l1 = new List<string>();
            List<string> l2 = new List<string>();
            Process[] processes;
            processes = Process.GetProcesses();
             
            foreach (Process p in processes) {
                try {
                    //    str = p.ProcessName;
                    l1.Add("名称：" + p.ProcessName + "，启动时间：" + p.StartTime.ToShortTimeString() + "，进程ID：" + p.Id.ToString());
                } catch (Exception ex) {
                    l2.Add(ex.Message.ToString());//某些系统进程禁止访问，所以要加异常处理
                }
            }

            l1.ForEach(_ => Console.WriteLine(_));
            Console.WriteLine("===================");
            l2.ForEach(_ => Console.WriteLine(_));
        }

        // Get a handle to an application window. 
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName,
        string lpWindowName);

        // Activate an application window. 
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static bool BringWindowToTop(string windowName, bool wait) {
            int hWnd = FindWindow2(windowName, wait);
            if (hWnd != 0) {
                return SetForegroundWindow((IntPtr)hWnd);
            }
            return false;
        }

        // THE FOLLOWING METHOD REFERENCES THE FindWindowAPI
        public static int FindWindow2(string windowName, bool wait) {
            int hWnd = (int)FindWindow(null, windowName);
            while (wait && hWnd == 0) {
                System.Threading.Thread.Sleep(500);
                hWnd = (int)FindWindow(null, windowName);
            }

            return hWnd;
        }




        [TestMethod]
        public void TestMethod1()
        {
           
            btnList_Click();
            BringWindowToTop("请选择号码", false);
            return;
             
        }



    }
}
