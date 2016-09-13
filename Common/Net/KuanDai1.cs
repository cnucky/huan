using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace Common {
    public class KuanDai1 {

        //禁用 SetNetworkAdapter(False)   
        //启用 SetNetworkAdapter(True)   
        //添加引用system32/shell32.dll  

        Shell32.FolderItemVerb connection = null;//宽带“连接”对象
        Shell32.FolderItemVerb disconnect = null;//宽带“断开”对象

        string adslName = "宽带连接";
        string adslUserName = "";
        string adslPasswd = "";

        public KuanDai1(string name, string pwd) {
            adslUserName = name;
            adslPasswd = pwd;

        }

        ///<summary>
        /// 得到控制面板中宽带连接对象
        ///</summary>
        public void SetNetworkAdapter(string adslName) {
            string discVerb = "断开(&O)";
            string connVerb = "连接(&O)";
            string network = "网络连接";
            string networkConnection = adslName;



            Shell32.Shell sh = new Shell32.Shell();
            Shell32.Folder folder = sh.NameSpace(3);//Shell32.ShellSpecialFolderConstants.ssfCONTROLS
            try {
                //进入控制面板的所有选项  
                foreach (Shell32.FolderItem myItem in folder.Items()) {
                    Console.WriteLine(myItem.Name);
                    //进入网络和拔号连接  
                    if (myItem.Name == network) {
                        Shell32.Folder fd = (Shell32.Folder)myItem.GetFolder;
                        foreach (Shell32.FolderItem fi in fd.Items()) {
                            //找到本地连接  
                            if (fi.Name.IndexOf(networkConnection) > -1) {
                                //找本地连接的所有右键功能菜单  
                                foreach (Shell32.FolderItemVerb fib in fi.Verbs())//fi.Verbs不能再线程中访问,所以在一开始就要获取连接对象
                                {
                                    if (fib.Name == discVerb) {
                                        disconnect = fib;
                                        return;
                                    } else if (fib.Name == connVerb) {
                                        connection = fib;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            } catch {
            }
        }

        ///<summary>
        /// 程序开始前获取连接对象
        ///</summary>
        public void Start() {
            try {
                SetNetworkAdapter(adslName);//获取“连接”或“断开”对象
                if (disconnect != null) {
                    disconnect.DoIt();
                    Thread.Sleep(2000);
                    SetNetworkAdapter(adslName);//获取“连接”对象
                    while (true) {
                        if (connection != null)
                            break;
                        SetNetworkAdapter(adslName);
                        Thread.Sleep(500);
                    }
                    connection.DoIt();
                    AdslReconnect.DoAdsl(adslUserName, adslPasswd);
                } else {
                    connection.DoIt();
                    AdslReconnect.DoAdsl(adslUserName, adslPasswd);
                    SetNetworkAdapter(adslName);//获取“断开”对象
                }
            } catch {
            }
            Ping pingSender = new Ping();
            while (true) {
                try {
                    PingReply reply = pingSender.Send("www.google.com", 100);
                    if (reply.Status == IPStatus.Success) {
                        Thread.Sleep(500);
                        break;
                    }
                } catch {
                }
            }
        }

        ///<summary>
        /// 得到控制面板中宽带连接对象
        ///</summary>
        public void DoAdslReconnect() {
            try {
                if (disconnect != null && connection != null) {
                    disconnect.DoIt();
                    Thread.Sleep(3000);
                    connection.DoIt();
                    AdslReconnect.DoAdsl(adslUserName, adslPasswd);
                    Ping pingSender = new Ping();
                    while (true) {
                        try {
                            PingReply reply = pingSender.Send("www.baidu.com", 100);
                            if (reply.Status == IPStatus.Success) {
                                Thread.Sleep(500);
                                break;
                            }
                        } catch {
                        }
                    }
                }
            } catch {
            }
        }
    }
}