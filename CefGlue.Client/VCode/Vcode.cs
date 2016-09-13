using System;
using System.Net;
using System.Windows.Forms;
using Common;
using Common.Vcode;
using PwBusiness;

namespace Xilium.CefGlue.Client.VCode
{
    public class Vcode
    {
        public static bool GetVcodeFormImageUrl(string srcs, out string returnMess) {
            //新开一个client下载图片
            using (WebClient webClient = new WebClient()) {
                //保存图片
                webClient.DownloadFile(new Uri(srcs), Application.StartupPath + "//tmp//zhifubaoVcode.jpg");
            }

            //
            string tip;
            bool isok = true;
            bool isVCodeOk = false;
            returnMess = "";
            for (int i = 0; i < 5; i++) {
                LogManager.WriteLog("Vcode start");
                isok = LianZhongVcode.Vcode(out returnMess, out tip);
                LogManager.WriteLog("Vcode end");
                if (isok && !string.IsNullOrEmpty(returnMess) && returnMess.Length == 4) {
                    isVCodeOk = true;
                    LogManager.WriteLog("Vcode ok {0}".With(returnMess));
                    break;
                } else {
                    //  LB_VCODE_TIP.Text = "失败，准备重新验证";
                    //todo:失败，考虑重新开始
                    LogManager.WriteLog("Vcode fail {0},重新验证".With(returnMess));
                }
            }
            return isVCodeOk;
        } 
    }
}