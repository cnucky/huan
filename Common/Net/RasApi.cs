using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Common.ADSL1;
using Microsoft.Win32;
using NUnit.Framework;

namespace Common.Net {
    public class RasApi {
        string AdslName = "宽带连接";
        string strError = "";
        string AdslPassWord = "";
        string AdslUserName = "";
        cRASDisplay cRas = new cRASDisplay();
        string ip = "";
        int AdslSeconds = 0;
        int AdslCount = 0;
        public RasApi() {
            AdslName = ConfigHelper.GetValue("AdslName");
            AdslUserName = ConfigHelper.GetValue("UserName");
            AdslPassWord = ConfigHelper.GetValue("AdslPassWord");
            AdslSeconds = int.Parse(ConfigHelper.GetValue("AdslSeconds"));
            AdslCount = int.Parse(ConfigHelper.GetValue("AdslCount"));

        }

        public bool IsConnected() {
            return GetIP(AdslName) != "";
        }

        public bool ReConn() {
            bool isojk = false;
            isojk = AdslHelper.checkIpisOKBylocal(ip);
            if (isojk) {
                return true;
            }


            bool connOk = false;
            int i = 0;
            for (i = 0; i < AdslCount; i++) {

                LogManager.WriteLog("断开宽带 " + GetIP(AdslName));
                bool disconnOk = HangUp(AdslName, out strError);
                LogManager.WriteLog("{0} {1}".With(disconnOk, strError));
              // cRas.Disconnect();
                LogManager.WriteLog("等待{0}秒 ".With( AdslSeconds));
                Thread.Sleep(AdslSeconds * 1000);


                LogManager.WriteLog("ADSL宽带拨号 开始");
                connOk = DialUp(AdslName, AdslUserName, AdslPassWord, out strError);
                //  var res1 = cRas.Connect("宽带连接");
                //connOk =res1 == 0 && connOk;
                LogManager.WriteLog("ADSL宽带拨号 结束： " + strError);

                if (connOk) {
                    ip = strError;
                    LogManager.WriteLog("当前IP:" + strError);

                    isojk = AdslHelper.checkIpisOKBylocal(ip);
                    if (isojk) {
                        MouseKeyBordHelper.CurrentIP = ip;
                        break;
                    }
                    connOk = false;


                    LogManager.WriteLog(strError);

                } else {
                    LogManager.WriteLog(strError);
                }

            }
            if (i == AdslCount) {
                //撤换超时失败
            }

            return connOk;
        }
        /// <summary>
        /// 需要提供账号密码
        /// </summary>
        /// <param name="outip"></param>
        /// <returns></returns>
        public bool ReConn(out string outip) {
            //outip = GetIP(AdslName);

            outip ="";

            bool isojk = false;
            isojk = AdslHelper.checkIpisOKBylocal(ip);
            if (isojk) {
           
                return true;
            }


            bool connOk = false;
            int i = 0;
            for (i = 0; i < AdslCount; i++) {

                LogManager.WriteLog("断开宽带 " + GetIP(AdslName));
                bool disconnOk = HangUp(AdslName, out strError);
                LogManager.WriteLog("{0} {1}".With(disconnOk, strError));
                // cRas.Disconnect();
                LogManager.WriteLog("等待{0}秒 ".With(AdslSeconds));
                Thread.Sleep(AdslSeconds * 1000);


                LogManager.WriteLog("ADSL宽带拨号 开始");
                connOk = DialUp(AdslName, AdslUserName, AdslPassWord, out strError);
                //  var res1 = cRas.Connect("宽带连接");
                //connOk =res1 == 0 && connOk;
                LogManager.WriteLog("ADSL宽带拨号 结束： " + strError);

                if (connOk) {
                    ip = strError;
                    LogManager.WriteLog("当前IP:" + strError);

                    isojk = AdslHelper.checkIpisOKBylocal(ip);
                    if (isojk) {
                        MouseKeyBordHelper.CurrentIP = ip;
                        break;
                    }
                    connOk = false;


                    LogManager.WriteLog(strError);

                } else {
                    LogManager.WriteLog(strError);
                }

            }
            //if (i == AdslCount) {
            //    //撤换超时失败
            //}
            outip = strError;
            return connOk;
        }
        [Test]
        public void test() {
            for (int i = 0; i < 11; i++) {

                string err = "";
                ReConn();
                Console.WriteLine("换IP结束");

            }

        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct RASDIALPARAMS {
            internal int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
            internal string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x81)]
            internal string szPhoneNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x81)]
            internal string szCallbackNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
            internal string szUserName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
            internal string szPassword;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
            internal string szDomain;
        }

        /// <summary>
        /// 列出所有的远程访问的名字
        /// </summary>
        /// <param name="reserved">保留，必须为NULL</param>
        /// <param name="lpszPhonebook">电话本文件的完整路径和名字</param>
        /// <param name="lprasentryname">指向一个缓冲，该缓冲接收一个RASENTRYNAME结构的数组。在调用函数之前，一个应用程序必须设置上述结构中的dwSize参数用以指定所要传递结构的版本。</param>
        /// <param name="lpcb">指向一个变量，该变量包含由lprasentryname参数指定的缓冲的大小（单位为字节）。作为返回，函数设置该变量为成功调用所要求的字节数。</param>
        /// <param name="lpcEntries">指向一个变量，该变量在函数成功是，设置为由lprasentryname参数指定的电话本的号码。</param>
        /// <returns>返回零值，失败返回非零值。</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasEnumEntries(string reserved, string lpszPhonebook, [In, Out] RASENTRYNAME[] lprasentryname, ref int lpcb, ref int lpcEntries);
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasEnumEntries(
            string reserved,              // 保留, 必须空
            string lpszPhonebook,         // pointer to full path and file name of phone-book file
            [In, Out]RasEntryName[] lprasentryname, // buffer to receive phone-book entries
            ref int lpcb,                  // size in bytes of buffer
            out int lpcEntries             // number of entries written to buffer
            );
        /// <summary>
        /// 通过对RasDial的成功调用，本函数调用后返回连接信息被保存为电话簿入口。
        /// </summary>
        /// <param name="lpszPhonebook">Windows CE:本参数被忽视，或者设置为NULL，通过电话簿存储进行拨号上网注册，而不是通过电话簿文件。</param>
        /// <param name="lprasdialparams">一个指向RASDIALPARAMS结构的指针，在输入时，dwSize成员必须被 RASDIALPARAMS结构指定大小。并且szEntryName 成员必须被指定一个有效地电话簿入口，输出时结构接收一个已经被指定电话簿入口相关联的连接参数。 注意：szPhoneNumber 成员不能接到与电话簿入口相关联的电话号码，要想获得的电话号码需要调用 RasGetEntryProperties 函数</param>
        /// <param name="lpfPassword">一个BOOL类型指针，表示函数是否能通过电话簿入口返回和用户名相关联的密码，函数设置该标志位为TRUE时，用户的密码返回到lpRasDialParams 参数指向的RASDIALPARAMS 结构的szPassword 成员中</param>
        /// <returns>　0表示成功。ERROR_BUFFER_INVALID 表示lpRasDialParams 或者 lpfPassword 指针是无效的。或者lpRasDialParams 得缓冲区是无效的。ERROR_CANNOT_OPEN_PHONEBOOK 表示电话簿被损坏或者丢失组件。ERROR_CANNOT_FIND_PHONEBOOK_ENTRY 表示电话簿的入口不存在。</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasGetEntryDialParams(string lpszPhonebook, ref RASDIALPARAMS lprasdialparams, ref bool lpfPassword);

        /// <summary>
        /// 本函数建立RAS 客户端和服务器端的一个连接。这个连接包括和返回用户验证信息。
        /// </summary>
        /// <param name="lpRasDialExtensions">本参数可以被忽略并且可以设置为NULL,在Windows CE,RasDial 总是使用RASDIALEXTENSIONS 当做默认选项。</param>
        /// <param name="lpszPhonebook">本参数可以被忽略并且可以设置为NULL，通过电话簿存储进行拨号上网注册，而不是通过电话簿文件。</param>
        /// <param name="lpRasDialParams">一个指向RASDIALPARAMS 结构的指针，用来描述 RAS连接的调用参数。调用者必须设置RASDIALPARAMS 结构的 dwSize成员(即结构大小)，用sizeof(RASDIALPARAMS)取得大小，防止不同版本的系统取得的大小不同</param>
        /// <param name="dwNotifierType">描述通告程序的参数性质。如果通告程序为NULL,本参数可以忽略，如果非空，则设置本参数为下面值：0xFFFFFFFF 通过程序是一个句柄，是窗体接收通告程序消息用的。在通告程序进行中，wParam参数指示 RAS连接将要进入的连接状态。当发生错误时lParam里存储错误信息。通告程序处理时，使用的消息是：WM_RASDIALEVENT。</param>
        /// <param name="lpvNotifier">一个指针，指向窗体句柄，用来接收RasDial的通告事件。如果本参数非空，RasDial为每一个通告事件发送一个windows消息。RasDial调用异步操作:在建立连接之前RasDial立即返回，使用窗体进行进程通信。如果本参数被设置为：NULL,RasDial 调用同步操作：RasDial不立即返回，直到连接成功或者连接失败。如果本参数非空,在调用RasDial之后，窗体通知会在任何时候出现。当下列事件发生时通知结束：连接被建立，换句话说，RAS的连接状态是RASCS_Connected 连接失败，换句话说，dwError 非零</param>
        /// <param name="lphRasConn">一个HRASCONN类型的指针，必须设置HRASCONN 类型变量为空在调用RasDial前。如果RasDial成功，本函数存储一个RAS连接句柄在本参数中。</param>
        /// <returns>0表示成功</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasDial(int lpRasDialExtensions, string lpszPhonebook, ref RASDIALPARAMS lpRasDialParams, int dwNotifierType, IntPtr lpvNotifier, ref int lphRasConn);
        /// <summary>
        ///  远程存取终结函数
        /// </summary>
        /// <param name="hrasconn">被终结的远程存取连接的句柄.当RasDial 或者 RasEnumConnections 执行以后得到该句柄</param>
        /// <returns>0表示成功</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasHangUp(int hrasconn);
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]

        private extern static uint RasGetProjectionInfo(
        int hrasconn, // handle to a RAS connection
        RASPROJECTION rasprojection, // type of control protocol
        [In, Out] RASPPPIP lpprojection, // pointer to a structure that
        ref uint lpcb // size of projection structure

        );


        /// <summary>
        /// 列出所有活动RAS连接，返回每一个连接句柄和电话簿入口名
        /// </summary>
        /// <param name="lprasconn">返回一个RASCONN结构数组的缓存的长指针，对于每一个RAS连接。在调用本函数之前，必须设置缓存中RASCONN结构的第一个成员dwSize的值，即RASCONN的大小，为了在不同系统版本中通过，请用sizeof(RASCONN)取得大小</param>
        /// <param name="lpcb">一个长指针，指向的变量是lprasconn指向的缓存中存储的字节个数，返回时，本函数将已经被列举的所有RAS连接需要的字节个数赋值到lpcb中</param>
        /// <param name="lpcConnections">长指针，本函数设置会回写这个参数，设置有多少个 RASCONN结构被写入到 lprasconn指向的缓存中</param>
        /// <returns>返回0则成功 返回非0值则错误，返回值的宏定义在 Raserror头文件中，例如ERROR_BUFFER_TOO_SMALL (缓存太小) ERROR_NOT_ENOUGH_MEMORY (内存不足)</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasEnumConnections([In, Out] RASCONN[] lprasconn, ref int lpcb, ref int lpcConnections);
        private delegate void RasDialEvent(uint unMsg, RASCONNSTATE rasconnstate, int dwError);
        private enum RASCONNSTATE {
            RASCS_AllDevicesConnected = 4,
            RASCS_AuthAck = 12,
            RASCS_AuthCallback = 8,
            RASCS_AuthChangePassword = 9,
            RASCS_Authenticate = 5,
            RASCS_Authenticated = 14,
            RASCS_AuthLinkSpeed = 11,
            RASCS_AuthNotify = 6,
            RASCS_AuthProject = 10,
            RASCS_AuthRetry = 7,
            RASCS_CallbackSetByCaller = 0x1002,
            RASCS_ConnectDevice = 2,
            RASCS_Connected = 0x2000,
            RASCS_DeviceConnected = 3,
            RASCS_Disconnected = 0x2001,
            RASCS_Interactive = 0x1000,
            RASCS_OpenPort = 0,
            RASCS_PasswordExpired = 0x1003,
            RASCS_PortOpened = 1,
            RASCS_PrepareForCallback = 15,
            RASCS_Projected = 0x12,
            RASCS_ReAuthenticate = 13,
            RASCS_RetryAuthentication = 0x1001,
            RASCS_SubEntryConnected = 0x13,
            RASCS_SubEntryDisconnected = 20,
            RASCS_WaitForCallback = 0x11,
            RASCS_WaitForModemReset = 0x10
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        private struct RASCONN {
            internal int dwSize;
            internal int hrasconn;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
            internal string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x11)]
            internal string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x81)]
            internal string szDeviceName;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct RASCONNSTATUS {
            internal int dwSize;
            internal RASCONNSTATE rasconnstate;
            internal int dwError;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x11)]
            internal string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x81)]
            internal string szDeviceName;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct RASENTRYNAME {
            internal int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x101)]
            internal string szEntryName;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RasEntryName      //定义接口名称
        {
            public int dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256 + 1)]
            public string szEntryName;
#if WINVER5
                public int dwFlags;
                [MarshalAs(UnmanagedType.ByValTStr,SizeConst=260+1)]
                public string szPhonebookPath;
#endif
        }
        /// <summary>

        /// RASPROJECTION

        /// </summary>

        private enum RASPROJECTION : int {
            RASP_Amb = 0x10000,
            RASP_PppNbf = 0x803F,
            RASP_PppIpx = 0x802B,
            RASP_PppIp = 0x8021,
            RASP_PppCcp = 0x80FD,
            RASP_PppLcp = 0xC021,
            RASP_Slip = 0x20000
        }
        /// <summary>
        /// RASPPPIP
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class RASPPPIP {
            public readonly int dwSize = Marshal.SizeOf(typeof(RASPPPIP));
            public uint dwError = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)16)]
            public string szIpAddress = null;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)16)]
            public string szServerIpAddress = null;
        }

        /*
         m_connected = true;

            RAS lpras = new RAS();

            RASCONN lprasConn = new RASCONN();

            lprasConn.dwSize = Marshal.SizeOf(typeof(RASCONN));
            lprasConn.hrasconn = IntPtr.Zero;

            int lpcb = 0;
            int lpcConnections = 0;
            int nRet = 0;
            lpcb = Marshal.SizeOf(typeof(RASCONN));


            nRet = RAS.RasEnumConnections(ref lprasConn, ref lpcb, ref lpcConnections);


            if (nRet != 0)
            {
                m_connected = false;
                return;

            }

            if (lpcConnections > 0)
            {


                //for (int i = 0; i < lpcConnections; i++)

                //{
                RasStats stats = new RasStats();

                m_ConnectedRasHandle = lprasConn.hrasconn;
                RAS.RasGetConnectionStatistics(lprasConn.hrasconn, stats);


                m_ConnectionName = lprasConn.szEntryName;

                int Hours = 0;
                int Minutes = 0;
                int Seconds = 0;

                Hours = ((stats.dwConnectionDuration / 1000) / 3600);
                Minutes = ((stats.dwConnectionDuration / 1000) / 60) - (Hours * 60);
                Seconds = ((stats.dwConnectionDuration / 1000)) - (Minutes * 60) - (Hours * 3600);


                m_duration = Hours + " hours " + Minutes + " minutes " + Seconds + " secs";
                m_TX = stats.dwBytesXmited;
                m_RX = stats.dwBytesRcved;


                //}


            }
            else
            {
                m_connected = false;

            }
         */

        public bool HangUp(string strEntryName, out string strError) {
            strError = "";
            RASCONN[] Rasconn = new RASCONN[1];
            Rasconn[0].dwSize = Marshal.SizeOf(Rasconn[0]);
            RASCONNSTATUS structure = new RASCONNSTATUS();
            int lpcb = 0;
            int lpcConnections = 0;
            structure.dwSize = Marshal.SizeOf(structure);
            lpcb = 0x500;
            int nErrorValue = RasEnumConnections(Rasconn, ref lpcb, ref lpcConnections);
            switch (nErrorValue) {
                case 0:
                    break;
                case 0x25b:
                    Rasconn = new RASCONN[lpcConnections];
                    lpcb = Rasconn[0].dwSize = Marshal.SizeOf(Rasconn[0]);
                    nErrorValue = RasEnumConnections(Rasconn, ref lpcb, ref lpcConnections);
                    break;
                //default:
                //ConnectNotify(this.GetErrorString(nErrorValue), 3);
                //return;
            }
            foreach (RASCONN rasconn in Rasconn.Where(rasconn => rasconn.szEntryName == strEntryName)) {
                if (rasconn.hrasconn != 0) {
                    int num2 = RasHangUp(rasconn.hrasconn);
                    if (num2 != 0) {
                        //strError = this.GetErrorString(num2);
                        //this.ConnectNotify(strError, 0);
                        return false;
                    }
                }
            }
            strError = null;
            return true;
        }

        public bool DialUp(string strEntryName, string szUserName, string szPassWord, out string strError) {
            bool lpfPassword = false;
            strError = null;
            RASDIALPARAMS structure = new RASDIALPARAMS();
            structure.dwSize = Marshal.SizeOf(structure);
            structure.szEntryName = strEntryName;
            int nErrorValue = RasGetEntryDialParams(null, ref structure, ref lpfPassword);
            structure.szUserName = szUserName;
            structure.szPassword = szPassWord;
            if (nErrorValue != 0) {
                strError = "宽带重拨发生错误！";
                return false;
            }
            int lphrsaConn = 0;
            nErrorValue = RasDial(0, null, ref structure, 0, IntPtr.Zero, ref lphrsaConn);
            if (nErrorValue != 0) {
                strError = "宽带重拨返回" + nErrorValue.ToString() + "错误!";
                //this.ConnectNotify(strError, 3);
                return false;
            }

            strError = "宽带重拨成功！";
            strError = GetIP(lphrsaConn);
            if (strError == "") {
                strError = "查询IP发生未知错误!";
                return false;
            }
            return true;
        }

        public string GetIP(int lphrsaConn) {
            RASPPPIP structProjection = new RASPPPIP();
            uint uiProjSize = (uint)structProjection.dwSize;
            try {
                uint errorCode = RasGetProjectionInfo(lphrsaConn, RASPROJECTION.RASP_PppIp, structProjection, ref uiProjSize);
                return structProjection.szIpAddress;
            } catch {
                return "";
            }
        }

        public static string GetIP(string strEntryName) {
            RASCONN[] Rasconn = new RASCONN[1];
            Rasconn[0].dwSize = Marshal.SizeOf(Rasconn[0]);
            RASCONNSTATUS structure = new RASCONNSTATUS();
            int lpcb = 0;
            int lpcConnections = 0;
            structure.dwSize = Marshal.SizeOf(structure);
            int nErrorValue = RasEnumConnections(Rasconn, ref lpcb, ref lpcConnections);
            switch (nErrorValue) {
                case 0:
                    break;
                case 0x25b:
                    Rasconn = new RASCONN[lpcConnections];
                    lpcb = Rasconn[0].dwSize = Marshal.SizeOf(Rasconn[0]);
                    nErrorValue = RasEnumConnections(Rasconn, ref lpcb, ref lpcConnections);
                    break;
                //default:
                //ConnectNotify(this.GetErrorString(nErrorValue), 3);
                //return;
            }
            int lphrsaConn = 0;
            foreach (RASCONN rasconn in Rasconn.Where(rasconn => rasconn.szEntryName == strEntryName)) {
                if (rasconn.hrasconn != 0) {
                    lphrsaConn = rasconn.hrasconn;
                }
            }
            if (lphrsaConn == 0) {
                return "";
            }
            RASPPPIP structProjection = new RASPPPIP();
            uint uiProjSize = (uint)structProjection.dwSize;
            try {
                uint errorCode = RasGetProjectionInfo(lphrsaConn, RASPROJECTION.RASP_PppIp, structProjection, ref uiProjSize);
                return structProjection.szIpAddress;
            } catch {
                return "";
            }
        }

        public bool RasDialUp(string strEntryName, string szUserName, string szPassWord, int SleepMS, out string strError) {
            HangUp(strEntryName, out strError);
            System.Threading.Thread.Sleep(SleepMS);
            bool fh = DialUp(strEntryName, szUserName, szPassWord, out strError);
            System.Threading.Thread.Sleep(200);
            return fh;
        }

        public bool GetEntries(out string[] strEntryName, out string strError) {
            strError = "";
            RASENTRYNAME[] lprasentryname = new RASENTRYNAME[1];
            lprasentryname[0].dwSize = Marshal.SizeOf(lprasentryname[0]);
            int lpcb = 0;
            int lpcEntries = 0;
            int nErrorValue = RasEnumEntries(null, null, lprasentryname, ref lpcb, ref lpcEntries);
            switch (nErrorValue) {
                case 0:
                    break;
                case 0x25b:
                    lprasentryname = new RASENTRYNAME[lpcEntries];
                    lprasentryname[0].dwSize = Marshal.SizeOf(lprasentryname[0]);
                    break;
                default:
                    //strError = this.GetErrorString(nErrorValue);
                    strEntryName = null;
                    return false;
            }
            nErrorValue = RasEnumEntries(null, null, lprasentryname, ref lpcb, ref lpcEntries);
            if (nErrorValue != 0) {
                //strError = this.GetErrorString(nErrorValue);
                strEntryName = null;
                return false;
            }
            strEntryName = new string[lpcEntries];
            for (int i = 0; i < lpcEntries; i++) {
                strEntryName[i] = lprasentryname[i].szEntryName;
            }
            strError = null;
            return true;
        }
        /// <summary>
        /// 通过API获取宽带连接名称 win7已测试
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllAdslName() {
            List<string> list = new List<string>();
            int lpNames = 1;
            int entryNameSize = 0;
            int lpSize = 0;
            RasEntryName[] names = null;
            entryNameSize = Marshal.SizeOf(typeof(RasEntryName));
            lpSize = lpNames * entryNameSize;
            names = new RasEntryName[lpNames];
            names[0].dwSize = entryNameSize;
            uint retval = RasEnumEntries(null, null, names, ref lpSize, out lpNames);
            //if we have more than one connection, we need to do it again
            if (lpNames > 1) {
                names = new RasEntryName[lpNames];
                for (int i = 0; i < names.Length; i++) {
                    names[i].dwSize = entryNameSize;
                }
                retval = RasEnumEntries(null, null, names, ref lpSize, out lpNames);
            }
            if (lpNames > 0) {
                for (int i = 0; i < names.Length; i++) {
                    list.Add(names[i].szEntryName);
                }
            }
            return list;
        }
        /// <summary>
        /// 获取一个宽带连接名称 win7已测试
        /// 没有则返回null
        /// </summary>
        /// <returns></returns>
        public static string GetOneAdslName() {
            List<string> list = GetAllAdslName();
            if (list != null) {
                return list[0];
            }
            return null;
        }
        /// <summary>
        /// 通过注册表获取宽带连接名称 win7已测试
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllAdslNameByKey() {
            RegistryKey userKey = Registry.CurrentUser;
            RegistryKey key = userKey.OpenSubKey(@"RemoteAccess\Profile");
            return key.GetSubKeyNames();//获取当前创建的adsl宽带列表
        }
        /// <summary>
        /// 通过注册表获取宽带连接名称 win7已测试
        /// </summary>
        /// <returns></returns>
        public static string GetOneAdslNameByKey() {
            string[] keysList = GetAllAdslNameByKey();
            if (keysList != null && keysList.Length > 0) {
                return keysList[0];
            }
            return null;
        }

        internal string Disconnect() {
            HangUp(AdslName, out strError);
            return strError;
        }

        internal void Conn() {
            //LogManager.WriteLog("DialUp " + strError);
            //bool fh = ras.DialUp(AdslName, AdslUserName, AdslPassWord, out strError);
            //LogManager.WriteLog("DialUp end " + strError);
            //if (fh) {
            //    ip = strError;
            //    LogManager.WriteLog("当前IP:" + strError);
            //} else {
            //    LogManager.WriteLog(strError);
            //}
        }


    }
}
