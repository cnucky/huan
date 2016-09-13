using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using Common;
using PwBusiness;

namespace Xilium.CefGlue.Client.SchemeHandler {
    internal sealed class Js2CsharpRequestResourceHandler : CefResourceHandler {
        private static int _requestNo;

        private byte[] responseData;
        private int pos;


        protected override bool ProcessRequest(CefRequest request, CefCallback callback) {
            var requestNo = Interlocked.Increment(ref _requestNo);

            var response = new StringBuilder();
            var headers = request.GetHeaderMap();
            foreach (string key in headers) {
                foreach (var value in headers.GetValues(key)) {
                    //此处通知某某方法的执行情况
                    //状态，XX方法，执行情况（1 或者 0）
                    //   response.AppendFormat("{0}: {1}\n", key, value);
                }
            }
            response.AppendFormat("ok");

            var d1 = GlobalVar.ExtractPar(request.Url);
            //  GlobalVar.Notifyjs2Csahrp(d1["statue"], d1["method"], d1["result"]);
            LogManager.WriteLog(d1["statue"] + d1["method"] + d1["result"]);
            if (d1.ContainsKey("encoding") &&   d1["encoding"] == "gb2312") {
                GlobalVar.jsTmpValue = HttpUtility.UrlDecode(d1["result"], Encoding.GetEncoding("gb2312"));
            } else {
                GlobalVar.jsTmpValue = HttpUtility.UrlDecode(d1["result"], Encoding.UTF8);
            }


            LogManager.WriteLog("IsloadOkAutoResetEvent 释放");
            GlobalVar.IsloadOkAutoResetEvent.Set();
            /*
            response.AppendFormat("<pre>\n");
            response.AppendFormat("Requests processed by DemoAppResourceHandler: {0}\n", requestNo);

            response.AppendFormat("Method: {0}\n", request.Method);
            response.AppendFormat("URL: {0}\n", request.Url);

            response.AppendLine();
            response.AppendLine("Headers:");
            var headers = request.GetHeaderMap();
            foreach (string key in headers)
            {
                foreach (var value in headers.GetValues(key))
                {
                    response.AppendFormat("{0}: {1}\n", key, value);
                }
            }
            response.AppendLine();

            response.AppendFormat("</pre>\n");*/

            responseData = Encoding.UTF8.GetBytes(response.ToString());

            callback.Continue();
            return true;
        }

        protected override void GetResponseHeaders(CefResponse response,
            out long responseLength, out string redirectUrl) {
            response.MimeType = "text/html";
            response.Status = 200;
            response.StatusText = "OK, hello from handler!";

            var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            headers.Add("Cache-Control", "private");
            headers.Add("Access-Control-Allow-Origin", "*");//允许跨域
            response.SetHeaderMap(headers);

            responseLength = responseData.LongLength;
            redirectUrl = null;
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback) {
            if (bytesToRead == 0 || pos >= responseData.Length) {
                bytesRead = 0;
                return false;
            } else {
                response.Write(responseData, pos, bytesToRead);
                pos += bytesToRead;
                bytesRead = bytesToRead;
                return true;
            }
        }

        protected override bool CanGetCookie(CefCookie cookie) {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie) {
            return false;
        }

        protected override void Cancel() {
        }
    }
}
