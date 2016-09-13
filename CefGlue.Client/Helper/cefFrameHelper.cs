using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using PwBusiness;

namespace Xilium.CefGlue.Client {
    public static class CefFrameHelper {
        [Obsolete("过期了，考虑使用GetUrlListByHap()")]
        public static void GetUrlList(CefFrame cefFrame, string clasName, string cookieName) {
            string jsSetElemets2Cookies = (
                           "var a1=document.getElementsByClassName('" + clasName + "');var b1='';for(i=0;i<a1.length;i++ ){ b1+=a1[i].href+'|'; } document.cookie='" + cookieName + "='+b1;"
                               );
            cefFrame.ExecuteJavaScript(jsSetElemets2Cookies, cefFrame.Url, 0);

            var cookieVisitor = new CefCookieVisitorImpOfGetCookies(cookieName);
            CefCookieManager.Global.VisitAllCookies(cookieVisitor);

        }



        /// <summary>
        /// 从HTML中找到当前页面所有值为innertext的链接
        /// </summary>
        /// <param name="cefFrame"></param>
        /// <param name="innerText"></param>
        public static List<string> GetUrlListByHap(CefFrame cefFrame, string innerText) {


            //LogManager.WriteLog("{0} 释放".With(innerText));
            //GlobalVar.IsloadOkAutoResetEvent.Set();

            var domvisit = new Domvisi2T();
            cefFrame.GetSource(domvisit);
            LogManager.WriteLog("{0} 默认阻塞，现在等待".With(innerText));
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromSeconds(10));
            LogManager.WriteLog("{0} 阻塞".With(innerText));
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            var a1 = HtmlHelper.GetHrefs(domvisit.CurrentPageHtml, innerText);
            return a1;

        }

        /// <summary>
        /// 从HTML中找到当前页面所有值为innertext的链接
        /// </summary>
        /// <param name="cefFrame"></param>
        /// <param name="innerText"></param>
        public static List<string> GetUrlListByHapId(CefFrame cefFrame, string ElementId, string tagname, string attribute) {
            List<string> ls1 = new List<string>();
            try {

           
            var domvisit = new Domvisi2T();
            if (cefFrame == null) {
                return new List<string>();
            }
            new Thread(() => {
                Thread.Sleep(1000);
                cefFrame.GetSource(domvisit);
            }).Start();

            LogManager.WriteLog("阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            LogManager.WriteLog("默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne();


              ls1 = HtmlHelper.GetHrefs(domvisit.CurrentPageHtml, ElementId, tagname, attribute);
            } catch (Exception e1) {
                LogManager.WriteLog(e1.ToString());
               
            }
            return ls1;

        }

        public static void SetElementValueById(CefFrame cefFrame, string elementid, string elementvalue) {

            cefFrame.ExecuteJavaScript(" document.getElementById('{0}').focus();".With(elementid), cefFrame.Url, 0);
            cefFrame.ExecuteJavaScript(" document.getElementById('{0}').value = \"{1}\";".With(elementid, elementvalue), cefFrame.Url, 0);
            cefFrame.ExecuteJavaScript(" document.getElementById('{0}').blur();".With(elementid), cefFrame.Url, 0);
        }


        public static void ExcuteJs(CefFrame cefFrame, string js2run) {
            cefFrame.ExecuteJavaScript(js2run, cefFrame.Url, 0);
        }

        public static List<string> GetInnerHtmlByHap(CefFrame cefFrame, string tagname, string attribute, string attributeValue) {

            var domvisit = new Domvisi2T();
            if (cefFrame == null) {
                return new List<string>();
            }
            cefFrame.GetSource(domvisit);
            LogManager.WriteLog("默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne();
            LogManager.WriteLog("阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            var a1 = HtmlHelper.GetValue(domvisit.CurrentPageHtml, attributeValue, tagname, attribute);
            return a1;
        }

        ///  <summary>
        ///  
        ///  </summary>
        ///  <param name="MainCefFrame"></param>
        /// <param name="jscangetvalue">例如 mydoc.getElementsByClassName('warnMsg')[0].innerHTML</param>
        /// <returns></returns>
        public static string GetMsgByJs(CefFrame MainCefFrame, string jscangetvalue) {
            GlobalVar.jsTmpValue = "";
            string js_b1 = "if(window.frames.length!=0){mydoc=window.frames[0].document;}else{mydoc=document;}";
            string js1 = js_b1 + "var dcxbfjhcacfcxz={1};js2csharp('{0}'+dcxbfjhcacfcxz);"
                .With("statue=ready&method=testmethod&result=", jscangetvalue);

            MainCefFrame.ExecuteJavaScript(GlobalVar.base_js + js1,
                MainCefFrame.Url, 0);
            LogManager.WriteLog("默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromMilliseconds(10 * 1000));
            LogManager.WriteLog("阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            //CefGlue.Client\SchemeHandler\Js2CsharpRequestResourceHandler.cs(9)

            return GlobalVar.jsTmpValue;
        }
        ///  <summary>
        ///  
        ///  </summary>
        ///  <param name="MainCefFrame"></param>
        /// <param name="jscangetvalue">例如 mydoc.getElementsByClassName('warnMsg')[0].innerHTML</param>
        /// <returns></returns>
        public static string GetMsgByJs2(CefFrame MainCefFrame, string jscangetvalue, string encoding) {
            GlobalVar.jsTmpValue = "";
            string js_b1 = "if(window.frames.length!=0){mydoc=window.frames[0].document;}else{mydoc=document;}";
            string js1 = js_b1 + "var dcxbfjhcacfcxz={1};js2csharp('{0}'+dcxbfjhcacfcxz);"
                .With("statue=ready&method=testmethod&encoding={0}&result=".With(encoding), jscangetvalue);

            MainCefFrame.ExecuteJavaScript(GlobalVar.base_js + js1,
                MainCefFrame.Url, 0);
            LogManager.WriteLog("默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromMilliseconds(10 * 1000));
            LogManager.WriteLog("阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            //CefGlue.Client\SchemeHandler\Js2CsharpRequestResourceHandler.cs(9)

            return GlobalVar.jsTmpValue;
        }

        ///  <summary>
        ///  
        ///  </summary>
        ///  <param name="MainCefFrame"></param>
        /// <param name="jscangetvalue">例如 mydoc.getElementsByClassName('warnMsg')[0].innerHTML</param>
        /// <returns></returns>
        public static string GetMsgByJs3(CefFrame MainCefFrame, string jscangetvalue) {
            GlobalVar.jsTmpValue = "";

            string js1 = "var dcxbfjhcacfcxz={1};js2csharp('{0}'+dcxbfjhcacfcxz);"
                .With("statue=ready&method=testmethod&result=", jscangetvalue);

            MainCefFrame.ExecuteJavaScript(GlobalVar.base_js + js1,
                MainCefFrame.Url, 0);
            LogManager.WriteLog("默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromMilliseconds(10 * 1000));
            LogManager.WriteLog("阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            //CefGlue.Client\SchemeHandler\Js2CsharpRequestResourceHandler.cs(9)

            return GlobalVar.jsTmpValue;
        }
        public static string ExtractWarnMsg(CefFrame frame) {
            string warnMsg = "";
            if (frame != null) {
                warnMsg = CefFrameHelper.GetMsgByJs(frame, "mydoc.getElementsByClassName('warnMsg')[0].innerHTML");

            }
            LogManager.WriteLog("warnMsg:" + warnMsg);
            return warnMsg;
        }
    }
}
