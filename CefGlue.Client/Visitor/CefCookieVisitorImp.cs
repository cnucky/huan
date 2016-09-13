using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using PwBusiness;
using PwBusiness.PP;

namespace Xilium.CefGlue.Client {

    public class Domvisi2T : CefStringVisitor {
        public string CurrentPageHtml = "";
        protected override void Visit(string value) {
            lock (CurrentPageHtml) {
                CurrentPageHtml = value;
                GlobalVar.CurrentHtml = value;
            }

            LogManager.WriteLog("IsloadOkAutoResetEvent 释放");
            GlobalVar.IsloadOkAutoResetEvent.Set();

        }
    }


    public class HtmlVisitDownLoader : CefStringVisitor {
        protected override void Visit(string value) {

            File.WriteAllText("pplog//" + DateTime.Now.ToString("yyyyMMddHHmmssff"), value);


            GlobalVar.shops = PPhelper.PpShopsExtarct(value);

            GlobalVar.PP_analysis_AutoResetEvent.Set();
            //释放

        }
    }
    public class JISHI_HtmlVisitDownLoader : CefStringVisitor {
        protected override void Visit(string value) {

            File.WriteAllText("jishi_log//" + DateTime.Now.ToString("yyyyMMddHHmmssff"), value);


           

            GlobalVar.PP_analysis_AutoResetEvent.Set();
            //释放

        }
    }

    public class domvisit : CefDomVisitor {

        protected override void Visit(CefDomDocument document) {
            LogManager.WriteLog("===================");
            LogManager.WriteLog(document.Body.ToString());
            LogManager.WriteLog("===================");

        }
    }
    

    public class CefCookieVisitorImp : CefCookieVisitor {
        protected override bool Visit(CefCookie cookie, int count, int total, out bool delete) {
            delete = true;
            
            //LogManager.WriteLog(cookie.Domain + " " + cookie.Name + " " + cookie.Value);

            return true;
        }
    }
    public class CefCookieVisitorSave : CefCookieVisitor {
        private bool deleteCookies { get; set; }

        public CefCookieVisitorSave(bool delete) {
            deleteCookies = delete;
        }

        public List<CefCookie> CefCookies = new List<CefCookie>();
        protected override bool Visit(CefCookie cookie, int count, int total, out bool delete) {
            delete = deleteCookies;// 删除

            CefCookies.Add(cookie);
            //LogManager.WriteLog(cookie.Domain + " " + cookie.Name + " " + cookie.Value);
            if (count +1== total) {
                LogManager.WriteLog("释放");
                GlobalVar.IscookiesAutoResetEvent.Set();
            }
            return true;//继续遍历
        }
    }

    public class MyCookies {
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CefCookieVisitorImpOfGetCookies : CefCookieVisitor {
        private string cookiename = "";
        public CefCookieVisitorImpOfGetCookies(string cname) {
            cookiename = cname;
        }

        //   public List<MyCookies> Cookieses = new List<MyCookies>();
        private MyCookies TradeCookies = new MyCookies();


        private List<string> convertCookie2List(string cookies) {
            List<string> list = new List<string>();
            if (cookies != null) {
                var aList = cookies.Split('|');
                list = aList.Where(_ => !string.IsNullOrEmpty(_)).ToList();


            }
            return list;
        }


        protected override bool Visit(CefCookie cookie, int count, int total, out bool delete) {
            delete = false;
            // LogManager.WriteLog(cookie.Domain + " " + cookie.Value);
            //待付款
            if (cookie.Name == cookiename) {
                TradeCookies.Domain = cookie.Domain;
                TradeCookies.Name = cookie.Name;
                if (!string.IsNullOrEmpty(cookie.Value)) {
                    LogManager.WriteLog("Visit cookies. " + cookiename);
                    if (cookiename == "list_bought_items") {
                        lock (GlobalVar.ConfirmGoodsUrls) {
                            convertCookie2List(cookie.Value).ForEach(_ => GlobalVar.ConfirmGoodsUrls.Enqueue(_));
                        }
                    } else if (cookiename == "list_rate_items") {
                        lock (GlobalVar.RateGoodsUrls) {
                            GlobalVar.RateGoodsUrls.Clear();
                            convertCookie2List(cookie.Value).ForEach(_ => GlobalVar.RateGoodsUrls.Enqueue(_));

                        }
                    } else {

                    }

                }//

            }
            //待评价
            //待退
            //待...
            // LogManager.WriteLog(cookie.Domain + " " + cookie.Name + " " + cookie.Value);
            return true;
        }

    }
}