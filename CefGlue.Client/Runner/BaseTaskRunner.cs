using System;
using Common;
using Common.Net;
using Newtonsoft.Json;
using PwBusiness.Bll;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {
    public class BaseTaskRunner : ITaskRunner {
        public string infilename { get; set; }
        public string outfilename { get; set; }
        public string tmodel1 { get; set; }

        public BaseTaskRunner(string infile, string outfile) {
            infilename = infile;
            outfilename = outfile;
        }

        public virtual void LogAccontStatus(string state) {
            LogManager.LogHaoZi(state, outfilename);
        }

        public void LogTaskBegin() {
            LogManager.WriteLog("{0} ip = {1}".With(currentAccount.tbName, NetHostIp.GetAllIPs()));
            LogManager.LogHaoZi("\n{0}|{1}|{2}|{3}|{4}"
                .With(currentAccount.tbName, currentAccount.zfbEmail, currentAccount.zfbEmailPwd,
                    currentAccount.zfbPwd, currentAccount.zfbPayPwd), outfilename);
        }


        public tb_tbzfb currentAccount {
            get;
            set;
        }


        public bool getNewTask() {
            //  string tbmodel = "shopv2";
            bool isGetTaskOK;
            IsGetTaskOk(tmodel1, out isGetTaskOK);
            return isGetTaskOK;
        }

        public EventHandler BeginHandler;
        public EventHandler GetBeginHandler() {
            return BeginHandler;
        }


        public string getinfilename() {
            return infilename;
        }

        public string getoutfilename() {
            return outfilename;
        }


        public virtual void ReportAccount(tb_tasklog tb) {

        }

        private bool IsGetTaskOk(string tbmodel, out bool isGetTaskOK) {

            string host = ConfigHelper.GetValue("WebSite");
            string name = ConfigHelper.GetValue("name");
            string pwd = ConfigHelper.GetValue("pwd");
            int GetHaoZiNum = int.Parse(ConfigHelper.GetValue("GetHaoZiNum"));

            if (tbmodel == tbModel.shopv2.ToString()) {
                isGetTaskOK = false;
                return false;
            }
            tbzfbBll bll = new tbzfbBll();
            var list = bll.GetHaoZi(host, name, pwd, GetHaoZiNum, tbmodel);

            isGetTaskOK = list.Count > 0;

            if (!isGetTaskOK) {
                return true;
            }
            HaoziHelper.logAccount(list, infilename, outfilename);


            return isGetTaskOK;
        }



        public virtual void ReportAccount(string isok) {

        }


        public virtual void ReportAccountStatus(string action, string status, string note) {
            tb_tasklog tlog = new tb_tasklog(currentAccount.zfbEmail, MainForm.state.ToString());
            tlog.action = action;
            tlog.actionresult = status;
            tlog.note = note;
            //email name and num
            string s1 = JsonConvert.SerializeObject(tlog);
            HaoziHelper.ReportStatus(s1, "tb_tasklog");
        }
    }
}