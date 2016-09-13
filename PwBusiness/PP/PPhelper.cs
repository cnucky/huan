using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PwBusiness.PP {
    public class PPhelper {


        public static void PopQq(PpShop shop)
        {
            string p1 = AppDomain.CurrentDomain.BaseDirectory + "\\config\\TimwpPath.txt";
            string path = FileHelper.read(p1)[0].Trim();// @"C:\Program Files (x86)\Tencent\QQ\Bin\Timwp.exe";
            string par =
                "tencent://message/?uin=" + shop.qqnum +
                "&fromuserid=no&touserid=no&unionid=72000106&WebSiteName=拍拍网&Service=19&sigT=" + shop.sigT + "&sigU=" +
                shop.sigP;
            Process.Start(path, par);
        }


        public static List<PpShop> PpShopsExtarct(string f1) {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(f1);
            //title[@lang='eng']	选取所有 title 元素，且这些元素拥有值为 eng 的 lang 属性。
            var alist = doc.DocumentNode.SelectNodes("//li[@class='item-attr-list']  "); //item-attr-list
            if (alist==null)
            {
                return new List<PpShop>();
            }
            Console.WriteLine(alist.Elements().Count());
            List<PpShop> shops = new List<PpShop>();
            int index = 0;
            foreach (var htmlNode in alist) {
                PpShop ppShop = new PpShop();
                ppShop.index = index;
                index++;
                var titile = htmlNode.SelectNodes("ul[1]/li[2]/div/dl/dt/h3/a");
                ppShop.titile = titile[0].InnerText;
                ppShop.mainseller_sell = htmlNode.SelectNodes("ul/li[2]/div/dl/div[1]/p[1]/span[2]")[0].InnerText;
                ppShop.mainseller_owner = htmlNode.SelectNodes("ul/li[2]/div/dl/div[1]/p[2]/a")[0].InnerText;
                //imtalk
                ppShop.imtakStr = htmlNode.SelectNodes("ul/li[2]/div/dl/div[2]/ul/li[1]/q/a")[0].GetAttributeValue("href", "");
                //
                ppShop.como_num = htmlNode.SelectNodes("ul[1]/li[3]/dl/dd")[0].InnerText;
                //legend
                ppShop.legend = htmlNode.SelectNodes("ul[1]/li[4]/dl/dd/label")[0].InnerText;
                //promotions
                ppShop.promotions = htmlNode.SelectNodes("ul[1]/li[5]/dl")[0].InnerText;
                //adress
                ppShop.adress = htmlNode.SelectNodes("ul[1]/li[6]")[0].InnerText;
                ////*[@id="itemList"]/ul[1]/li/ul/li[2]/div/dl/div[2]/ul/li[1]/q/a

                shops.Add(ppShop);
            }
            return shops;
        }
        public static bool IsShopOnline(WebClient webClient, PpShop shop) {

            var hwr = HttpWebRequest.Create("http://wpa.paipai.com/pa?p=1:" + shop.qqnum + ":17");
            hwr.Timeout = 1000;
            var Response = hwr.GetResponse();
            var responseUrl = Response.ResponseUri.ToString();
            bool online = false;
            if (responseUrl.Contains("state_2_3")) {
                online = false;
            } else {
                online = true;
            }
            Console.WriteLine("Response.IsFromCache:" + Response.IsFromCache);

            return online;
        }
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static bool CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies,
            out HttpWebResponse res) {
            if (string.IsNullOrEmpty(url)) {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            // request.KeepAlive = false;//add
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent)) {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue) {
                request.Timeout = timeout.Value;
            }
            if (cookies != null) {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            res = null;
            bool isok = true;
            try {
                res = request.GetResponse() as HttpWebResponse;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                isok = false;
            }

            return isok;

        }

        private static bool GetHTMLTCP(string URL, out string strHTML) {
            bool isok = true;
            strHTML = null;
            //用来保存获得的HTML代码 
            try {

                TcpClient clientSocket = new TcpClient();
                Uri URI = new Uri(URL);
                clientSocket.Connect(URI.Host, URI.Port);
                clientSocket.SendTimeout = 2000;
                clientSocket.SendTimeout = 2000;
                StringBuilder RequestHeaders = new StringBuilder();//用来保存HTML协议头部信息 
                RequestHeaders.AppendFormat("{0} {1} HTTP/1.1\r\n", "GET", URI.PathAndQuery);
                RequestHeaders.AppendFormat("Connection:close\r\n");
                RequestHeaders.AppendFormat("Host:{0}\r\n", URI.Host);
                RequestHeaders.AppendFormat("Accept:*/*\r\n");
                RequestHeaders.AppendFormat("Accept-Language:zh-cn\r\n");
                RequestHeaders.AppendFormat("User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)\r\n\r\n");
                Encoding encoding = Encoding.Default;
                byte[] request = encoding.GetBytes(RequestHeaders.ToString());
                clientSocket.Client.Send(request);
                //获取要保存的网络流 
                Stream readStream = clientSocket.GetStream();
                StreamReader sr = new StreamReader(readStream, Encoding.Default);
                strHTML = sr.ReadToEnd();
                readStream.Close();
                clientSocket.Close();
            } catch (Exception e) {
                isok = false;
                Console.WriteLine(e.ToString());

            }

            return isok;
        }
        /// <summary>
        /// http://wpa.paipai.com/pa?p=1:" + shop.qqnum + ":17
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public static bool IsShopOnline(PpShop shop) {

            string url = "http://wpa.paipai.com/pa?p=1:" + shop.qqnum + ":17";
            


            string res1;
            bool isdownok = GetHTMLTCP(url, out res1);
            //bool isdownok = CreateGetHttpResponse(url, 5000, "", new CookieCollection(), out respone);

            bool online = false;
            if (!isdownok) {
                return false;
            }
            if (res1.Contains("state_2_3")) {
                online = false;
            } else {
                online = true;
            }

            //Console.WriteLine("Response.IsFromCache:" + respone.ResponseUri);
            //Console.WriteLine("Response.IsFromCache:" + respone.IsFromCache);

            return online;
        }
    }
}