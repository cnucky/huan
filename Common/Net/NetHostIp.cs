using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

namespace Common.Net {
    public class NetHostIp {


        public static string GetHostName() {
            return Dns.GetHostName();
        }

        public static IPAddress[] GetLocalIP() {
            string name = Dns.GetHostName();
            IPHostEntry me = Dns.GetHostEntry(name);
            return me.AddressList;
        }

        public static IPAddress GetFirstIP() {
            IPAddress[] ips = GetLocalIP();
            foreach (IPAddress ip in ips) {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    continue;
                return ip;
            }
            return ips != null && ips.Length > 0 ? ips[0] : new IPAddress(0x0);
        }
        public static string GetAllIPs() {
            IPAddress[] ips = GetLocalIP();
            List<IPAddress> iplist = new List<IPAddress>();
            string ipsStr = "";
            foreach (IPAddress ip in ips) {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    continue;
                //    iplist.Add(ip);
                ipsStr = ipsStr + "_" + ip.ToString();
            }

            return ipsStr;
        }
        [Test]
        public static void TestGetAllIPs() {
            Console.WriteLine(GetAllIPs());

        }
    }
}