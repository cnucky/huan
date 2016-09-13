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
        string AdslName = "�������";
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

                LogManager.WriteLog("�Ͽ���� " + GetIP(AdslName));
                bool disconnOk = HangUp(AdslName, out strError);
                LogManager.WriteLog("{0} {1}".With(disconnOk, strError));
              // cRas.Disconnect();
                LogManager.WriteLog("�ȴ�{0}�� ".With( AdslSeconds));
                Thread.Sleep(AdslSeconds * 1000);


                LogManager.WriteLog("ADSL������� ��ʼ");
                connOk = DialUp(AdslName, AdslUserName, AdslPassWord, out strError);
                //  var res1 = cRas.Connect("�������");
                //connOk =res1 == 0 && connOk;
                LogManager.WriteLog("ADSL������� ������ " + strError);

                if (connOk) {
                    ip = strError;
                    LogManager.WriteLog("��ǰIP:" + strError);

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
                //������ʱʧ��
            }

            return connOk;
        }
        /// <summary>
        /// ��Ҫ�ṩ�˺�����
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

                LogManager.WriteLog("�Ͽ���� " + GetIP(AdslName));
                bool disconnOk = HangUp(AdslName, out strError);
                LogManager.WriteLog("{0} {1}".With(disconnOk, strError));
                // cRas.Disconnect();
                LogManager.WriteLog("�ȴ�{0}�� ".With(AdslSeconds));
                Thread.Sleep(AdslSeconds * 1000);


                LogManager.WriteLog("ADSL������� ��ʼ");
                connOk = DialUp(AdslName, AdslUserName, AdslPassWord, out strError);
                //  var res1 = cRas.Connect("�������");
                //connOk =res1 == 0 && connOk;
                LogManager.WriteLog("ADSL������� ������ " + strError);

                if (connOk) {
                    ip = strError;
                    LogManager.WriteLog("��ǰIP:" + strError);

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
            //    //������ʱʧ��
            //}
            outip = strError;
            return connOk;
        }
        [Test]
        public void test() {
            for (int i = 0; i < 11; i++) {

                string err = "";
                ReConn();
                Console.WriteLine("��IP����");

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
        /// �г����е�Զ�̷��ʵ�����
        /// </summary>
        /// <param name="reserved">����������ΪNULL</param>
        /// <param name="lpszPhonebook">�绰���ļ�������·��������</param>
        /// <param name="lprasentryname">ָ��һ�����壬�û������һ��RASENTRYNAME�ṹ�����顣�ڵ��ú���֮ǰ��һ��Ӧ�ó���������������ṹ�е�dwSize��������ָ����Ҫ���ݽṹ�İ汾��</param>
        /// <param name="lpcb">ָ��һ���������ñ���������lprasentryname����ָ���Ļ���Ĵ�С����λΪ�ֽڣ�����Ϊ���أ��������øñ���Ϊ�ɹ�������Ҫ����ֽ�����</param>
        /// <param name="lpcEntries">ָ��һ���������ñ����ں����ɹ��ǣ�����Ϊ��lprasentryname����ָ���ĵ绰���ĺ��롣</param>
        /// <returns>������ֵ��ʧ�ܷ��ط���ֵ��</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasEnumEntries(string reserved, string lpszPhonebook, [In, Out] RASENTRYNAME[] lprasentryname, ref int lpcb, ref int lpcEntries);
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasEnumEntries(
            string reserved,              // ����, �����
            string lpszPhonebook,         // pointer to full path and file name of phone-book file
            [In, Out]RasEntryName[] lprasentryname, // buffer to receive phone-book entries
            ref int lpcb,                  // size in bytes of buffer
            out int lpcEntries             // number of entries written to buffer
            );
        /// <summary>
        /// ͨ����RasDial�ĳɹ����ã����������ú󷵻�������Ϣ������Ϊ�绰����ڡ�
        /// </summary>
        /// <param name="lpszPhonebook">Windows CE:�����������ӣ���������ΪNULL��ͨ���绰���洢���в�������ע�ᣬ������ͨ���绰���ļ���</param>
        /// <param name="lprasdialparams">һ��ָ��RASDIALPARAMS�ṹ��ָ�룬������ʱ��dwSize��Ա���뱻 RASDIALPARAMS�ṹָ����С������szEntryName ��Ա���뱻ָ��һ����Ч�ص绰����ڣ����ʱ�ṹ����һ���Ѿ���ָ���绰���������������Ӳ����� ע�⣺szPhoneNumber ��Ա���ܽӵ���绰�����������ĵ绰���룬Ҫ���õĵ绰������Ҫ���� RasGetEntryProperties ����</param>
        /// <param name="lpfPassword">һ��BOOL����ָ�룬��ʾ�����Ƿ���ͨ���绰����ڷ��غ��û�������������룬�������øñ�־λΪTRUEʱ���û������뷵�ص�lpRasDialParams ����ָ���RASDIALPARAMS �ṹ��szPassword ��Ա��</param>
        /// <returns>��0��ʾ�ɹ���ERROR_BUFFER_INVALID ��ʾlpRasDialParams ���� lpfPassword ָ������Ч�ġ�����lpRasDialParams �û���������Ч�ġ�ERROR_CANNOT_OPEN_PHONEBOOK ��ʾ�绰�����𻵻��߶�ʧ�����ERROR_CANNOT_FIND_PHONEBOOK_ENTRY ��ʾ�绰������ڲ����ڡ�</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasGetEntryDialParams(string lpszPhonebook, ref RASDIALPARAMS lprasdialparams, ref bool lpfPassword);

        /// <summary>
        /// ����������RAS �ͻ��˺ͷ������˵�һ�����ӡ�������Ӱ����ͷ����û���֤��Ϣ��
        /// </summary>
        /// <param name="lpRasDialExtensions">���������Ա����Բ��ҿ�������ΪNULL,��Windows CE,RasDial ����ʹ��RASDIALEXTENSIONS ����Ĭ��ѡ�</param>
        /// <param name="lpszPhonebook">���������Ա����Բ��ҿ�������ΪNULL��ͨ���绰���洢���в�������ע�ᣬ������ͨ���绰���ļ���</param>
        /// <param name="lpRasDialParams">һ��ָ��RASDIALPARAMS �ṹ��ָ�룬�������� RAS���ӵĵ��ò����������߱�������RASDIALPARAMS �ṹ�� dwSize��Ա(���ṹ��С)����sizeof(RASDIALPARAMS)ȡ�ô�С����ֹ��ͬ�汾��ϵͳȡ�õĴ�С��ͬ</param>
        /// <param name="dwNotifierType">����ͨ�����Ĳ������ʡ����ͨ�����ΪNULL,���������Ժ��ԣ�����ǿգ������ñ�����Ϊ����ֵ��0xFFFFFFFF ͨ��������һ��������Ǵ������ͨ�������Ϣ�õġ���ͨ���������У�wParam����ָʾ RAS���ӽ�Ҫ���������״̬������������ʱlParam��洢������Ϣ��ͨ�������ʱ��ʹ�õ���Ϣ�ǣ�WM_RASDIALEVENT��</param>
        /// <param name="lpvNotifier">һ��ָ�룬ָ����������������RasDial��ͨ���¼�������������ǿգ�RasDialΪÿһ��ͨ���¼�����һ��windows��Ϣ��RasDial�����첽����:�ڽ�������֮ǰRasDial�������أ�ʹ�ô�����н���ͨ�š����������������Ϊ��NULL,RasDial ����ͬ��������RasDial���������أ�ֱ�����ӳɹ���������ʧ�ܡ�����������ǿ�,�ڵ���RasDial֮�󣬴���֪ͨ�����κ�ʱ����֡��������¼�����ʱ֪ͨ���������ӱ����������仰˵��RAS������״̬��RASCS_Connected ����ʧ�ܣ����仰˵��dwError ����</param>
        /// <param name="lphRasConn">һ��HRASCONN���͵�ָ�룬��������HRASCONN ���ͱ���Ϊ���ڵ���RasDialǰ�����RasDial�ɹ����������洢һ��RAS���Ӿ���ڱ������С�</param>
        /// <returns>0��ʾ�ɹ�</returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RasDial(int lpRasDialExtensions, string lpszPhonebook, ref RASDIALPARAMS lpRasDialParams, int dwNotifierType, IntPtr lpvNotifier, ref int lphRasConn);
        /// <summary>
        ///  Զ�̴�ȡ�սắ��
        /// </summary>
        /// <param name="hrasconn">���ս��Զ�̴�ȡ���ӵľ��.��RasDial ���� RasEnumConnections ִ���Ժ�õ��þ��</param>
        /// <returns>0��ʾ�ɹ�</returns>
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
        /// �г����лRAS���ӣ�����ÿһ�����Ӿ���͵绰�������
        /// </summary>
        /// <param name="lprasconn">����һ��RASCONN�ṹ����Ļ���ĳ�ָ�룬����ÿһ��RAS���ӡ��ڵ��ñ�����֮ǰ���������û�����RASCONN�ṹ�ĵ�һ����ԱdwSize��ֵ����RASCONN�Ĵ�С��Ϊ���ڲ�ͬϵͳ�汾��ͨ��������sizeof(RASCONN)ȡ�ô�С</param>
        /// <param name="lpcb">һ����ָ�룬ָ��ı�����lprasconnָ��Ļ����д洢���ֽڸ���������ʱ�����������Ѿ����оٵ�����RAS������Ҫ���ֽڸ�����ֵ��lpcb��</param>
        /// <param name="lpcConnections">��ָ�룬���������û��д��������������ж��ٸ� RASCONN�ṹ��д�뵽 lprasconnָ��Ļ�����</param>
        /// <returns>����0��ɹ� ���ط�0ֵ����󣬷���ֵ�ĺ궨���� Raserrorͷ�ļ��У�����ERROR_BUFFER_TOO_SMALL (����̫С) ERROR_NOT_ENOUGH_MEMORY (�ڴ治��)</returns>
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
        public struct RasEntryName      //����ӿ�����
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
                strError = "����ز���������";
                return false;
            }
            int lphrsaConn = 0;
            nErrorValue = RasDial(0, null, ref structure, 0, IntPtr.Zero, ref lphrsaConn);
            if (nErrorValue != 0) {
                strError = "����ز�����" + nErrorValue.ToString() + "����!";
                //this.ConnectNotify(strError, 3);
                return false;
            }

            strError = "����ز��ɹ���";
            strError = GetIP(lphrsaConn);
            if (strError == "") {
                strError = "��ѯIP����δ֪����!";
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
        /// ͨ��API��ȡ����������� win7�Ѳ���
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
        /// ��ȡһ������������� win7�Ѳ���
        /// û���򷵻�null
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
        /// ͨ��ע����ȡ����������� win7�Ѳ���
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllAdslNameByKey() {
            RegistryKey userKey = Registry.CurrentUser;
            RegistryKey key = userKey.OpenSubKey(@"RemoteAccess\Profile");
            return key.GetSubKeyNames();//��ȡ��ǰ������adsl����б�
        }
        /// <summary>
        /// ͨ��ע����ȡ����������� win7�Ѳ���
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
            //    LogManager.WriteLog("��ǰIP:" + strError);
            //} else {
            //    LogManager.WriteLog(strError);
            //}
        }


    }
}
