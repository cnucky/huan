using System;
using System.Runtime.InteropServices;

namespace Common.ADSL1 {
    //用途：实现ADSL拨号操作 
    internal enum RasFieldSizeConstants {
        RAS_MaxDeviceType = 16,
        RAS_MaxPhoneNumber = 128,
        RAS_MaxIpAddress = 15,
        RAS_MaxIpxAddress = 21,
#if WINVER4
RAS_MaxEntryName      =256,
RAS_MaxDeviceName     =128,
RAS_MaxCallbackNumber =RAS_MaxPhoneNumber,
#else
        RAS_MaxEntryName = 20,
        RAS_MaxDeviceName = 32,
        RAS_MaxCallbackNumber = 48,
#endif

        RAS_MaxAreaCode = 10,
        RAS_MaxPadType = 32,
        RAS_MaxX25Address = 200,
        RAS_MaxFacilities = 200,
        RAS_MaxUserData = 200,
        RAS_MaxReplyMessage = 1024,
        RAS_MaxDnsSuffix = 256,
        UNLEN = 256,
        PWLEN = 256,
        DNLEN = 15
    }

    /// <summary>
    /// GUID
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct GUID {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data4;
    }

    /// <summary>
    /// RASCONN
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct RASCONN {
        public int dwSize;
        public IntPtr hrasconn;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)RasFieldSizeConstants.RAS_MaxEntryName + 1)]
        public string szEntryName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)RasFieldSizeConstants.RAS_MaxDeviceType + 1)]
        public string szDeviceType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)RasFieldSizeConstants.RAS_MaxDeviceName + 1)]
        public string szDeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]//MAX_PAPTH=260
        public string szPhonebook;
        public int dwSubEntry;
        public GUID guidEntry;
#if (WINVER501)
int     dwFlags;
public LUID      luid;
#endif
    }
    /// <summary>
    /// LUID
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct LUID {
        int LowPart;
        int HighPart;
    }

    /// <summary>
    /// RasEntryName
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct RasEntryName {
        public int dwSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)RasFieldSizeConstants.RAS_MaxEntryName + 1)]
        public string szEntryName;
#if WINVER5
public int dwFlags;
[MarshalAs(UnmanagedType.ByValTStr,SizeConst=260+1)]
public string szPhonebookPath;
#endif
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class RasStats {
        public int dwSize = Marshal.SizeOf(typeof(RasStats));
        public int dwBytesXmited;
        public int dwBytesRcved;
        public int dwFramesXmited;
        public int dwFramesRcved;
        public int dwCrcErr;
        public int dwTimeoutErr;
        public int dwAlignmentErr;
        public int dwHardwareOverrunErr;
        public int dwFramingErr;
        public int dwBufferOverrunErr;
        public int dwCompressionRatioIn;
        public int dwCompressionRatioOut;
        public int dwBps;
        public int dwConnectDuration;
    }

    /// <summary>
    /// RASPROJECTION
    /// </summary>
    public enum RASPROJECTION : int {
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



    /// <summary>
    /// cRAS
    /// </summary>
    public class cRAS {

        /// <summary>
        /// RasEnumConnections
        /// </summary>
        /// <param name="lprasconn"></param>
        /// <param name="lpcb"></param>
        /// <param name="lpcConnections"></param>
        /// <returns></returns>
        [DllImport("Rasapi32.dll", EntryPoint = "RasEnumConnectionsA",
        SetLastError = true)]
        internal static extern int RasEnumConnections
        (
        ref RASCONN lprasconn, // buffer to receive connections data
        ref int lpcb, // size in bytes of buffer
        ref int lpcConnections // number of connections written to buffer
        );

        /// <summary>
        /// RasGetConnectionStatistics
        /// </summary>
        /// <param name="hRasConn"></param>
        /// <param name="lpStatistics"></param>
        /// <returns></returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        internal static extern uint RasGetConnectionStatistics(
        IntPtr hRasConn,       // handle to the connection
        [In, Out]RasStats lpStatistics  // buffer to receive statistics
        );
        /// <summary>
        /// RasHangUp
        /// </summary>
        /// <param name="hrasconn"></param>
        /// <returns></returns>
        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasHangUp(
        IntPtr hrasconn  // handle to the RAS connection to hang up
        );

        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasEnumEntries(
        string reserved,              // reserved, must be NULL
        string lpszPhonebook,         // pointer to full path and
            //  file name of phone-book file
        [In, Out]RasEntryName[] lprasentryname, // buffer to receive
            //  phone-book entries
        ref int lpcb,                  // size in bytes of buffer
        out int lpcEntries             // number of entries written
            //  to buffer
        );

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        public extern static int InternetDial(
        IntPtr hwnd,
        [In]string lpszConnectoid,
        uint dwFlags,
        ref int lpdwConnection,
        uint dwReserved
        );

        [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
        public extern static uint RasGetProjectionInfo(
        IntPtr hrasconn, // handle to a RAS connection
        RASPROJECTION rasprojection, // type of control protocol
        [In, Out] RASPPPIP lpprojection, // pointer to a structure that
            // receives the projection info
        ref uint lpcb // size of projection structure
        );

        public cRAS() {

        }
    }
    /// <summary>
    /// 获取拨号连接的信息，并可进行拨号以及断网等操作
    /// </summary>
    public class cRASDisplay {
        private string m_duration;
        private string m_ConnectionName;
        private string[] m_ConnectionNames;
        private double m_TX;
        private double m_RX;
        private bool m_connected;
        private IntPtr m_ConnectedRasHandle;
        private int m_ConnectDuration;
        private string m_ip;

        /// <summary>
        /// cRASDisplay
        /// </summary>
        public cRASDisplay() {
            m_connected = true;

            cRAS lpras = new cRAS();
            RASCONN lprasConn = new RASCONN();

            lprasConn.dwSize = Marshal.SizeOf(typeof(RASCONN));
            lprasConn.hrasconn = IntPtr.Zero;
            int lpcb = 0;
            int lpcConnections = 0;
            int nRet = 0;
            lpcb = Marshal.SizeOf(typeof(RASCONN));
            nRet = cRAS.RasEnumConnections(ref lprasConn, ref lpcb, ref
lpcConnections);
            if (nRet != 0) {
                m_connected = false;
                return;
            }
            if (lpcConnections > 0) {
                RasStats stats = new RasStats();
                m_ConnectedRasHandle = lprasConn.hrasconn;
                cRAS.RasGetConnectionStatistics(lprasConn.hrasconn, stats);
                m_ConnectionName = lprasConn.szEntryName;
                m_ConnectDuration = stats.dwConnectDuration;

                int Hours = 0;
                int Minutes = 0;
                int Seconds = 0;

                Hours = ((stats.dwConnectDuration / 1000) / 3600);
                Minutes = ((stats.dwConnectDuration / 1000) / 60) - (Hours * 60);
                Seconds = ((stats.dwConnectDuration / 1000)) - (Minutes * 60) - (Hours * 3600);
                m_duration = Hours + " 小时 " + Minutes + " 分钟 " + Seconds + " 秒";
                m_TX = stats.dwBytesXmited;
                m_RX = stats.dwBytesRcved;

                //获取IP
                RASPPPIP structProjection = new RASPPPIP();
                uint uiProjSize = (uint)structProjection.dwSize;
                try {
                    uint errorCode = cRAS.RasGetProjectionInfo(m_ConnectedRasHandle, RASPROJECTION.RASP_PppIp, structProjection, ref uiProjSize);
                } catch { }
                m_ip = structProjection.szIpAddress;

            } else {
                m_connected = false;
            }
            int lpNames = 1;
            int entryNameSize = 0;
            int lpSize = 0;
            RasEntryName[] names = null;

            entryNameSize = Marshal.SizeOf(typeof(RasEntryName));
            lpSize = lpNames * entryNameSize;
            names = new RasEntryName[lpNames];
            names[0].dwSize = entryNameSize;

            uint retval = cRAS.RasEnumEntries(null, null, names, ref lpSize, out lpNames);

            //if we have more than one connection, we need to do it again
            if (lpNames > 1) {
                names = new RasEntryName[lpNames];
                for (int i = 0; i < names.Length; i++) {
                    names[i].dwSize = entryNameSize;
                }

                retval = cRAS.RasEnumEntries(null, null, names, ref lpSize, out lpNames);

            }
            m_ConnectionNames = new string[names.Length];
            if (lpNames > 0) {
                for (int i = 0; i < names.Length; i++) {
                    m_ConnectionNames[i] = names[i].szEntryName;
                }
            }
        }
        /// <summary>
        /// 当前拨号连接已持续时间,单位：毫秒
        /// </summary>
        public int ConnectDuration {
            get {
                return m_ConnectDuration;
            }
        }
        /// <summary>
        /// 当前拨号连接已持续时间,已格式化，格式为：X小时Y分钟Z秒,比如3小时4分钟39秒
        /// </summary>
        public string Duration {
            get {
                return m_connected ? m_duration : "";
            }
        }
        /// <summary>
        /// Connections
        /// </summary>
        public string[] Connections {
            get {
                return m_ConnectionNames;
            }
        }
        /// <summary>
        /// 当前拨号连接获取的IP地址
        /// </summary>
        public string IP {
            get {
                return m_ip;
            }
        }
        /// <summary>
        /// 发送字节数
        /// </summary>
        public double BytesTransmitted {
            get {
                return m_connected ? m_TX : 0;
            }
        }
        /// <summary>
        /// 接收字节数
        /// </summary>
        public double BytesReceived {
            get {
                return m_connected ? m_RX : 0;
            }
        }
        /// <summary>
        /// 当前已连接的拨号连接的名称
        /// </summary>
        public string ConnectionName {
            get {
                return m_connected ? m_ConnectionName : "";
            }
        }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected {
            get {
                return m_connected;
            }
        }
        /// <summary>
        /// 开始拨号
        /// </summary>
        /// <param name="Connection">拨号连接的名称</param>
        /// <returns>0-指示成功，其他值指示失败</returns>
        public int Connect(string Connection) {
            int temp = 0;
            uint INTERNET_AUTODIAL_FORCE_UNATTENDED = 2;
            int retVal = cRAS.InternetDial(IntPtr.Zero, Connection, INTERNET_AUTODIAL_FORCE_UNATTENDED, ref temp, 0);
            return retVal;
        }
        /// <summary>
        /// 断开拨号连接
        /// </summary>
        public void Disconnect() {
            cRAS.RasHangUp(m_ConnectedRasHandle);
        }
    }
}