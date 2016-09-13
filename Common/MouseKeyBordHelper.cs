using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Common {
    public static class MouseKeyBordHelper {

        public static int ConvertChar2VirtualKeyBord(char c1) {
            // anscii
            //  A - Z  65-90
            // a - z 97 122
            // 0 - 9 48 57
            // key bord
            // A　　　65
            // Z　　　90 　
            // 0　　　48 
            int c2 = (int)c1;
            int res = 0;
            // 0-9
            if (c2 >= 48 && c2 <= 57) {
                res = c2;
                //a - z
            } else if (c2 >= 97 && c2 <= 122) {
                res = c2 - 32; //  res = c2 - 32;
            } else if (c2 >= 65 && c2 <= 90) {//大写
                res = c2;
            } else if (c2 == '.') {
                res = 110;
            } else {
                throw new Exception("该字符未在转换的范围之内");
            }
            return res;
        }

        public static int[] ConvertStrVirtualKeyBord(string str1) {
            //Console.Write(Keys.D);
            List<int> list = new List<int>();
            foreach (char c in str1) {
                list.Add(ConvertChar2VirtualKeyBord(c));
            }
            return list.ToArray();
        }


        /// <summary>
        /// 例子输入A
        /// keybd_event(65, 0, 0, 0);
        /// keybd_event(65, 0, KEYEVENTF_KEYUP, 0);
        /// 模拟按下'shift+2'键=@
        /// keybd_event(16,0,0,0);@@
        /// keybd_event(50,0,0,0);
        /// keybd_event(50,0,KEYEVENTF_KEYUP,0);
        /// keybd_event(16,0,KEYEVENTF_KEYUP,0);
        /// http://msdn.microsoft.com/en-us/library/dd375731(v=vs.85).aspx
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [System.Runtime.InteropServices.DllImport("user32")]
        static extern void keybd_event(
            byte bVk,
            byte bScan,
            uint dwFlags,
            uint dwExtraInfo
            );
        const uint KEYEVENTF_EXTENDEDKEY = 0x1;
        const uint KEYEVENTF_KEYUP = 0x2;
        public static string CurrentIP = "";
        public static void KeyBoardDo2(int[] key) {
            foreach (int k1 in key) {
                keybd_event((byte)k1, 0, 0 | 0, 0);
                keybd_event((byte)k1, 0, KEYEVENTF_KEYUP, 0);
            }
        }


        //public static void KeyBoardDo(int[] key) {
        //    //foreach (int k in key) {
        //    //    keybd_event((byte)k, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);//keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);
        //    //}
        //    //foreach (int k in key) {
        //    //    keybd_event((byte)k, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        //    //}
        //    foreach (int k in key) {
        //        KeyBoardBig(k);
        //    }
        //}
        public static void KeyBoardDo(char[] key) {
            //foreach (int k in key) {
            //    keybd_event((byte)k, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);//keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);
            //}
            //foreach (int k in key) {
            //    keybd_event((byte)k, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            //}
            foreach (char k in key) {
                KeyBoardBig(k);
            }
            }

        public static void KeyBoardDo3(char[] key) {
            foreach (char k in key) {
                KeyBoardBig(k);

            }
        }
        /// <summary>
        /// 输入@
        /// </summary>
        public static void KeyBoardAt() {
            keybd_event(16, 0, 0, 0);
            keybd_event(50, 0, 0, 0);
            keybd_event(50, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(16, 0, KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// 输入tab
        /// </summary>
        public static void KeyBoardTAB() {
            keybd_event(9, 0, 0, 0);
            keybd_event(9, 0, KEYEVENTF_KEYUP, 0);
        }

        internal static void KeyBoardEnter() {
            keybd_event(13, 0, 0, 0);
            keybd_event(13, 0, KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// 输入tab
        /// </summary>
        public static void KeyBoardBig(char key) {
            Thread.Sleep(200);
            if (key >= 65 && key <= 90) {//大写
                LogManager.WriteLog("大写 {0} {1}".With((char)key, key));
                keybd_event(16, 0, 0, 0);
                keybd_event((byte)key, 0, 0, 0);
                keybd_event((byte)key, 0, KEYEVENTF_KEYUP, 0);
                keybd_event(16, 0, KEYEVENTF_KEYUP, 0);
            } else if (key >= 97 && key <= 122 ){
                LogManager.WriteLog("小写 {0} {1}".With((char)key, key));
               int keyint = key - 32;
               keybd_event((byte)keyint, 0, 0, 0);
               keybd_event((byte)keyint, 0, KEYEVENTF_KEYUP, 0);
            } else {


                LogManager.WriteLog("其他 {0} {1}".With((char)key, key));
                keybd_event((byte)key, 0, 0, 0);
                keybd_event((byte)key, 0, KEYEVENTF_KEYUP, 0);


            }
        }

        //keybd_event((byte)Keys.D, 0, 0, 0); //按下D

        [DllImport("user32", EntryPoint = "mouse_event")]
        public static extern int mouse_event(
          int dwFlags,// 下表中标志之一或它们的组合 
          int dx,
          int dy, //指定x，y方向的绝对位置或相对位置 
          int cButtons,//没有使用 
          int dwExtraInfo//没有使用 
          );

        const int MOUSEEVENTF_MOVE = 0x0001;     // 移动鼠标 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;// 模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标
        //  mouse_event(MOUSEEVENTF_LEFTDOWN, 500, 400, 0, 0);






        //调用系统函数 将鼠标移动到相应位置
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public extern static bool SetCursorPos(int x, int y);
        //获取当前鼠标的绝对位置
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;
        }
        [DllImport("User32")]
        public extern static bool GetCursorPos(out POINT p);

        public static void MoveAndLeftClick(int x, int y) {
            LogManager.WriteLog("move  {0},{1} ".With(x, y));
            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, x, y, 0, 0);

            LogManager.WriteLog("MOUSEEVENTF_LEFTDOWN   {0},{1} ".With(x, y));
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE, 0, x, y, 0);
        }





        public static void MoveAndLeftClick(int left, int top, int leftCha, int topCha) {
            MouseKeyBordHelper.POINT p1;
            p1.X = left + leftCha;
            p1.Y = top + topCha;
            Rectangle rect = SystemInformation.VirtualScreen;//
            int width = rect.Width;
            int height = rect.Height;
            LogManager.WriteLog("VirtualScreen width {0} height{1}".With(rect.Width, rect.Height));




            LogManager.WriteLog("PrimaryScreen width {0} height{1}".With(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));


            //Rectangle ScreenArea = System.Windows.Forms.Screen.GetBounds(this);
            p1.X = p1.X * 65535 / width;
            p1.Y = p1.Y * 65535 / height;
            MoveAndLeftClick(p1.X, p1.Y);
        }
    }
}
