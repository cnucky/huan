using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Common;
using OpenPop.Common.Logging;
using OpenPop.Pop3;
using PwBusiness;
using PwBusiness.Model;
using Message = OpenPop.Mime.Message;

namespace Xilium.CefGlue.Client {
    public class EmailHelper {
        private Pop3Client pop3Client = new Pop3Client();
        MailClient mcClient = new MailClient();
        public List<OpenPop.Mime.Message> GetEmails(tb_tbzfb currentHaoZi, ref bool error) {
            var messages = new List<OpenPop.Mime.Message>();
            // We will try to load in default values for the hostname, port, ssl, username and password from a file
            //  string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string file = Application.StartupPath + "//import//popserver.txt";
            if (File.Exists(file)) {
                using (StreamReader reader = new StreamReader(File.OpenRead(file))) {
                    mcClient.popServer = reader.ReadLine(); //  
                    mcClient.port = reader.ReadLine(); // Port
                    mcClient.ssl = bool.Parse(reader.ReadLine() ?? "true"); // Whether to use SSL or not
                    mcClient.username = reader.ReadLine(); // Username
                    mcClient.pwd = currentHaoZi.zfbEmailPwd; // Password
                }
            }
            mcClient.username = currentHaoZi.zfbEmail;
            //
            int count = 0;
            try {
                if (pop3Client.Connected) {
                    pop3Client.Disconnect();
                }
                pop3Client.Connect(mcClient.popServer, int.Parse(mcClient.port), mcClient.ssl);
                pop3Client.Authenticate(mcClient.username, mcClient.pwd);
                count = pop3Client.GetMessageCount();
            } catch (Exception e1) {
                error = true;
                LogManager.WriteLog(e1.ToString());
            }


            messages.Clear();
            for (int i = count; i >= 1; i -= 1) {
                Application.DoEvents();
                try {
                    Message message = pop3Client.GetMessage(i);
                    // Add the message to the dictionary from the messageNumber to the Message
                    messages.Add(message);
                } catch (Exception e) {
                    LogManager.WriteLog(
                        "TestForm: Message fetching failed: " + e.Message + "\r\n" +
                        "Stack trace:\r\n" +
                        e.StackTrace);
                }
            }
            return messages;
        }

        public string FindHtmlOfSubject(List<Message> messages, string subject) {
            string htmlbody = "";


            foreach (var msg in messages) {
                var html = msg.FindFirstHtmlVersion();
                if (html != null) {
                    if (msg.Headers.Subject.Contains(subject)) {
                        htmlbody = html.GetBodyAsText();
                        break;
                    }
                }
            }


            return htmlbody;
        }
        public string FindLink(string innertext, string htmlbody) {
            //  string innertext = "继续注册";
            string activZfbLink1 = ActivZfbLink1(htmlbody, innertext);
            //if (activZfbLink1.Contains("alipay.com")) {
            //    //  throw new Exception("未能从邮件中找到链接！");
            //    //   GlobalVar.activZfbLink = activZfbLink1;
            //    //继续注册

            //} else {
            //    activZfbLink1 = "没找到链接哎。。囧了。别急";
            //}
            return activZfbLink1;
        }
        public string ActivZfbLink1(string htmlbody, string innertext) {
            string activZfbLink1 = "";
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlbody);
            var alist = doc.DocumentNode.Descendants("a");
            foreach (var htmlNode in alist) {
                if (htmlNode.InnerText.Contains(innertext)) {
                    activZfbLink1 = htmlNode.Attributes["href"].Value;
                    break;
                }
            }
            return activZfbLink1;
        }
    }
}