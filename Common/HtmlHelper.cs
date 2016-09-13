using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Common {
    public static class HtmlHelper {
        /// <summary>
        /// 从HTML中找到所有值为innertext的链接
        /// </summary>
        /// <param name="htmlbody"></param>
        /// <param name="innertext"></param>
        /// <returns></returns>
        public static List<string> GetHrefs(string htmlbody, string innertext) {
            List<string> hrefList = new List<string>();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlbody);
            var alist = doc.DocumentNode.Descendants("a");
            foreach (var htmlNode in alist) {
                if (htmlNode.InnerText == innertext) {
                    string activZfbLink1 = htmlNode.Attributes["href"].Value;
                    hrefList.Add(activZfbLink1);
                }
            }
            return hrefList;
        }
        /// <summary>
        /// 从HTML中找到所有值为innertext的链接
        /// |tagname|可以是a,img
        /// </summary> 
        /// <returns></returns>
        public static List<string> GetHrefs(string htmlbody, string ElementId, string tagname, string attribute) {
            List<string> hrefList = new List<string>();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlbody);
            var alist = doc.DocumentNode.Descendants(tagname);
            foreach (var htmlNode in alist) {
                if (htmlNode.Id == ElementId) {
                    string activZfbLink1 = "";
                    var htmlAttribute = htmlNode.Attributes[attribute];
                    if (htmlAttribute != null) { activZfbLink1 = htmlAttribute.Value; }
                    hrefList.Add(activZfbLink1);
                }
            }
            return hrefList;
        }
        /// <summary>
        /// 从|htmlbody|中找到|tagname|标签的|attribute|属性的值为|attributeValue|的innerHTML
        /// </summary>
        /// <param name="htmlbody"></param>
        /// <param name="ClassName"></param>
        /// <param name="tagname"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static List<string> GetValue(string htmlbody, string attributeValue, string tagname, string attribute) {
            List<string> hrefList = new List<string>();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlbody);
            var alist = doc.DocumentNode.Descendants(tagname);
            foreach (var htmlNode in alist) {

                if (htmlNode.Attributes[attribute] != null && htmlNode.Attributes[attribute].Value == attributeValue) {
                    //  var htmlAttribute = htmlNode.Attributes[attribute];
                    //if (htmlAttribute != null) { activZfbLink1 = htmlAttribute.Value; }
                    hrefList.Add(htmlNode.InnerHtml);
                    break;

                }


            }
            return hrefList;
        }
        
    }
}
