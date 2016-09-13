using System.Collections.Generic;
using OpenPop.Mime;

namespace Xilium.CefGlue.Client {
    public class MailClient {
        public MessagePart html;
        public Dictionary<int, OpenPop.Mime.Message> messages =
            new Dictionary<int, OpenPop.Mime.Message>();
        public string popServer { get; set; }

        public string port { get; set; }

        public bool ssl { get; set; }

        public string username { get; set; }

        public string pwd { get; set; }
        public string GetHrefFormHtml(string htmlbody, string innertext) {
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