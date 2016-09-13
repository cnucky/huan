using System;
using System.Net;
using System.Text;
using System.Web;
using Common;
using Newtonsoft.Json;
using NUnit.Framework;
using PwBusiness;
using PwBusiness.Bll;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {
    public class ShopTaskRunner : BaseTaskRunner {

        public ShopTaskRunner()
            : base("", "") {
        }

        public ShopTaskRunner(string infile, string outfile)
            : base(infile, outfile) {
            tmodel1 = tbModel.shopv2.ToString();
        }
        [Test]
        public void ReportAccountTest() {
            var t1 = new tb_tasklog("a220707275@163.com", BusinessStatus.ready.ToString());
            t1.action = "buy";
            t1.actionresult = "1";
            t1.note = "≤‚ ‘";
            MainForm.state = BusinessStatus.ready;
            currentAccount = new tb_tbzfb();
            currentAccount.zfbEmail = "aa@qq.com";
            this.ReportAccount("ok");
        }



        public override void ReportAccount(string isok) {
            tb_tasklog tlog = new tb_tasklog(currentAccount.zfbEmail, MainForm.state.ToString());
            tlog.action = "buy";
            tlog.actionresult = isok;
            var frame = MainForm.getMainframe;
            tlog.note +=( MainForm.shoppingIndex+"|");
            if (frame != null) {
                tlog.note+= CefFrameHelper.ExtractWarnMsg(MainForm.getMainframe).Trim() + "|" + MainForm.getMainframe.Url;
            }
            if (tlog.note == "") {
                tlog.note = "wap";
            }


            //email name and num
            string s1 = JsonConvert.SerializeObject(tlog);
            HaoziHelper.ReportStatus(s1, "tb_tasklog");

        }




    }
}