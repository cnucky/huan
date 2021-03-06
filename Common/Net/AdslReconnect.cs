﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Common {
    public class AdslReconnect {
        //找主窗口句柄
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //找子窗口句柄
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        //发送字符串
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        //枚举窗口
        [DllImport("user32.dll", EntryPoint = "EnumChildWindows")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildWindowsProc lpEnumFunc, int lParam);

        //获取指定句柄类名
        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        private static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

        private const int WM_SETTEXT = 0x000C; //发送文本
        private const int WM_CLICK = 0x00F5; //鼠标单击

        private delegate bool EnumChildWindowsProc(IntPtr hwnd, uint lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private static IntPtr reconnection = IntPtr.Zero; //重新连接按钮句柄

        ///<summary>
        /// ADSL拨号
        ///</summary>
        ///<param name="adslUserName">adsl用户名</param>
        ///<param name="adslPasswd">adsl密码</param>
        ///<returns></returns>
        public static bool DoAdsl(string adslUserName, string adslPasswd) {
            string username = adslUserName;
            string passwd = adslPasswd;

            //下面的这些参数都可以用Spy++查到 
            string lpszParentClass = "#32770"; //整个窗口的类名 
            string lpszParentWindow = "连接 宽带连接"; //窗口标题 
            string lpszClass = "Edit"; //需要查找的子窗口的类名，也就是输入框 
            string lpszClass_Submit = "Button"; //需要查找的Button的类名 
            string lpszName_Submit = "连接(&C)"; //需要查找的Button的标题 

            IntPtr parentHWND = new IntPtr(0);
            //查到窗体，得到整个主窗口句柄 
            while (true) {
                parentHWND = FindWindow(lpszParentClass, lpszParentWindow);
                if (parentHWND != IntPtr.Zero)
                    break;
                Thread.Sleep(100);
            }

            if (!parentHWND.Equals(IntPtr.Zero)) {
                IntPtr childHWND = new IntPtr(0);
                childHWND = FindWindowEx(parentHWND, childHWND, lpszClass, string.Empty);
                //得到User Name这个子窗体             
                SendMessage(childHWND, WM_SETTEXT, IntPtr.Zero, username); //调用SendMessage方法设置其内容 
                childHWND = FindWindowEx(parentHWND, childHWND, lpszClass, string.Empty);
                //得到Password这个子窗体                
                SendMessage(childHWND, WM_SETTEXT, IntPtr.Zero, passwd);
                childHWND = FindWindowEx(parentHWND, childHWND, lpszClass_Submit, lpszName_Submit); //得到Button这个子窗体
                SendMessage(childHWND, WM_CLICK, IntPtr.Zero, "0"); //触发Click事件 

                Thread.Sleep(500);
                while (true) {
                    if (FindWindow("#32770", "正在连接 宽带连接...") == IntPtr.Zero) //表示连接成功
                        break;
                    IntPtr errorParent = FindWindow("#32770", "连接到 宽带连接 时出错");
                    if (errorParent != IntPtr.Zero) {
                        //获取'重拨(&R)'按钮句柄
                        EnumChildWindowsProc myEnumChild = new EnumChildWindowsProc(EumWinChiPro);
                        try {
                            EnumChildWindows(errorParent, myEnumChild, 0);
                        } catch (Exception ex) {
                            throw new Exception(ex.Message + "\r\n " + ex.Source + "\r\n\r\n " +
                                                ex.StackTrace.ToString());
                        }
                        if (reconnection != IntPtr.Zero)
                            SendMessage(reconnection, WM_CLICK, (IntPtr)0, "0");
                    }
                    Thread.Sleep(1000);
                }
            } else {
                return false;
            }
            return true;
        }

        private static bool EumWinChiPro(IntPtr hWnd, uint lParam) {
            StringBuilder s = new StringBuilder(50);
            GetClassName(hWnd, s, 50);
            if (s.ToString() == "Button") {
                reconnection = hWnd;
                return false;
            }
            return true;
        }

    }
}