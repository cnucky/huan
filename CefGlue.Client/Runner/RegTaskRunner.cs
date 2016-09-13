using System.Text;
using Common;
using Newtonsoft.Json;
using PwBusiness;
using PwBusiness.Bll;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {
    public class RegTaskRunner : BaseTaskRunner {
        public RegTaskRunner(string infile, string outfile)
            : base(infile, outfile) {
            tmodel1 = tbModel.reg_zfb.ToString();
        }


        public override void ReportAccount(string isok) {
            tb_tasklog tlog = new tb_tasklog(currentAccount.zfbEmail, MainForm.state.ToString());
            tlog.action = "reg";
            tlog.actionresult = isok;
            tlog.note = "website";
            //email name and num
            string s1 = JsonConvert.SerializeObject(tlog);
            HaoziHelper.ReportStatus(s1, "tb_tasklog");

        }
    }
}