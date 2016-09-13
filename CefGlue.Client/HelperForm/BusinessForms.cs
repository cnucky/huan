using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using Common.ADSL1;
using Common.Vcode;
using OpenPop.Common.Logging;
using OpenPop.Mime;
using OpenPop.Pop3;
using PwBusiness;
using PwBusiness.Bll;
using PwBusiness.Model;
using HtmlAgilityPack;
using Message = OpenPop.Mime.Message;

namespace Xilium.CefGlue.Client {
    public partial class BusinessForms : Form {


        public BusinessForms() {
            InitializeComponent();
        }

        private MainForm _mfForm;
        public BusinessForms(MainForm mfForm) {
            _mfForm = mfForm;
            mfForm.bform = this;
            InitializeComponent();
            //
            _mfForm.DisconnHandler += toolStripMenuItem3_Click;

            _mfForm.ConnHandler += toolStripMenuItem4_Click;


            //   _mfForm.SendEmailHandler += BT_test1_Click;

            //  _mfForm.GetMobilePhoneHandler += BT_ZFB_REG_TB_REG_SJ_Click;

            _mfForm.GetMobilePhoneTbHandler += BT_TB_ChangeTel_Click;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void BT_Import_Click(object sender, EventArgs e) {
            toolStripComboBox1.SelectedIndex = 0;
            string path = TB_Path.Text.Trim();

            List<string> pathList =
                FileHelper.read(Application.StartupPath + "//import//" + path, Encoding.UTF8);

            _mfForm.tbzfbs.Clear();
            foreach (var line in pathList) {
                //var l1s = line.Split('|');
                //tb_tbzfb tb = new tb_tbzfb();
                ////淘宝账号|淘宝登陆密码|支付宝登陆密码|邮箱（支付宝）账号|
                ////邮箱密码|支付密码|身份证号 
                //tb.tbName = l1s[0];
                //tb.tbPwd = l1s[1];
                //tb.zfbPwd = l1s[2];
                //tb.zfbEmail = l1s[3];
                //tb.zfbEmailPwd = l1s[4];
                //tb.zfbPayPwd = l1s[5];
                //tb.zfbSFZ = l1s[6];
                //tb.tbStatus = l1s[7];
                //_mfForm.tbzfbs.Add(tb);
                tb_tbzfb tb = new tb_tbzfb();
                //淘宝账号|淘宝登陆密码|支付宝登陆密码|邮箱（支付宝）账号|
                //邮箱密码|支付密码|身份证号 
                tb.tbName = line;
                tb.tbPwd = "aqq6611";
                tb.zfbPwd = "aqq6611";
                tb.zfbEmail = line;
                tb.zfbEmailPwd = "aqq6611";
                tb.zfbPayPwd = "AQQ6611";
                // tb.zfbSFZ = "123";
                tb.tbStatus = "";
                _mfForm.tbzfbs.Add(tb);
            }


            //新建table.
            displaylist(_mfForm.tbzfbs, DGV1);
        }

        private void displaylist(List<tb_tbzfb> tbzfbs, DataGridView dgv) {
            DataTable dt = new DataTable();
            dt.Columns.Add("tbName");
            dt.Columns.Add("zfbemail");
            dt.Rows.Clear();


            foreach (var tbTbzfb in tbzfbs) {
                DataRow dr = dt.NewRow();
                dr["tbName"] = tbTbzfb.tbName;
                dr["zfbemail"] = tbTbzfb.zfbEmail;
                dt.Rows.Add(dr);
            }
            dgv.DataSource = null;
            dgv.Rows.Clear();

            dgv.DataSource = dt;
        }

        private void BT_Start_Click(object sender, EventArgs e) {
            //换IP 清理 cookie
            _mfForm.ClearAndPrepareNext();

        }

        private void BT_import_simple_Click(object sender, EventArgs e) {
            toolStripComboBox1.SelectedIndex = 0;
            string path = TB_Path.Text.Trim();

            List<string> pathList =
                FileHelper.read(Application.StartupPath + "//import//" + path, Encoding.UTF8);

            _mfForm.tbzfbs.Clear();
            foreach (var line in pathList) {

                tb_tbzfb tb = new tb_tbzfb();
                //淘宝账号|淘宝登陆密码|支付宝登陆密码|邮箱（支付宝）账号|
                //邮箱密码|支付密码|身份证号 
                tb.tbName = line;
                tb.tbPwd = "qww6633";
                tb.zfbPwd = "qww6633";
                tb.zfbEmail = line;
                tb.zfbEmailPwd = "qww6633";
                tb.zfbPayPwd = "QWW6633";
                tb.tbStatus = "";
                _mfForm.tbzfbs.Add(tb);

            }

            //新建table.
            displaylist(_mfForm.tbzfbs, DGV1);
            ;
        }

        private void BT_next_Click(object sender, EventArgs e) {
            _mfForm.ClearAndPrepareNext();
        }

        #region ============换IP=============
        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            WB1.Navigate("http://192.168.2.1/userRpm/StatusRpm.htm?Disconnect=断 线&wan=1");

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            WB1.Navigate("http://192.168.2.1/userRpm/StatusRpm.htm?Connect=连 接&wan=1");

        }

        private void BT_ip_Click(object sender, EventArgs e) {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            _mfForm.NeedChangeIp = checkBox1.Checked;

        }
        #endregion

        public CefFrame MainCefFrame {
            get {
                return _mfForm.MainCefFrame;
            }
        }

        #region ============163============
        //淘宝名|支付宝账户名（邮箱名）|邮箱密码|支付宝登录密码|支付宝支付密码|身份证名|身份证号码|ok
        private void BT_Import_WY_Click(object sender, EventArgs e) {
            toolStripComboBox1.SelectedIndex = 2;
            string path = TB_FileName.Text.Trim();

            List<string> pathList =
                FileHelper.read(Application.StartupPath + "//import//" + path, Encoding.UTF8);

            _mfForm.tbzfbs.Clear();
            foreach (var line in pathList) {
                if (line.Contains("ok")) {
                    continue;
                }
                var l1s = line.Split('|');
                var tb = new tb_tbzfb();
                tb.tbName = l1s[1];
                tb.zfbEmail = l1s[0];
                //淘宝账号|淘宝登陆密码|支付宝登陆密码|邮箱（支付宝）账号|
                //邮箱密码|支付密码|身份证号 
                tb.tbPwd = "aqq6611";
                tb.zfbPwd = "aqq6611";
                tb.zfbEmailPwd = "aqq6611";
                tb.zfbPayPwd = "AQQ6611";
                if (l1s.Count() >= 4) {
                    tb.realname = l1s[2];
                    tb.zfbSFZ = l1s[3];

                } else {
                    tb.realname = "沈廷宝";
                    tb.zfbSFZ = "342127197108204134";
                }

                tb.tbStatus = "";
                _mfForm.tbzfbs.Add(tb);
            }


            //新建table.
            displaylist(_mfForm.tbzfbs, DGV2);

        }


        private void BT_WY_Reg_Click(object sender, EventArgs e) {

            //_mfForm.tbmode = tbModel.regEmail;


            _mfForm.ClearAndPrepareNext();
        }



        #endregion
        #region  ============支付宝注============
        public void BT_Zfb_Reg_Click(object sender, EventArgs e) {

        }
        #endregion
        #region ============测试邮件============






        //  private Dictionary<int, OpenPop.Mime.Message> messages = new Dictionary<int, OpenPop.Mime.Message>();
        //收取邮件
        public void BT_test1_Click(object sender, EventArgs e1) {
            //1.收取完所有邮件
            //2.在邮件中找链接
            //3.

            bool iserror = false;
            string htmlbody = "";

            LogManager.WriteLog("收取邮件中，稍等");
            EmailHelper emailHelper = new EmailHelper();
            bool isFindHtml = false;
            for (int i = 0; i < 5; i++) {
                Thread.Sleep(5000);
                Application.DoEvents();

                var messages = emailHelper.GetEmails(MainForm.currentHaoZi, ref iserror);
                if (iserror) {
                    MainForm.taskRunner.LogAccontStatus("|Error_Email");
                    LogManager.WriteLog("邮箱账户有误");
                    break;
                }
                string subject = "请激活您的支付宝账户";
                htmlbody = emailHelper.FindHtmlOfSubject(messages, subject);

                if (string.IsNullOrEmpty(htmlbody)) {
                    LogManager.WriteLog("暂未收到邮件，5秒后重试，第{0}次".With(5 - i));
                } else {
                    isFindHtml = true;
                    break;
                }

            }
            string innertext = "继续注册";
            string activZfbLink1 = "";
            if (isFindHtml) {
                activZfbLink1 = emailHelper.FindLink(innertext, htmlbody);
                MainForm.currentHaoZi.activeLink = activZfbLink1;
                _mfForm.BT_Link_Continue_Reg_Click(sender, e1);
                LogManager.WriteLog(activZfbLink1);
            } else {
                LogManager.WriteLog(" 未收到邮件");
            }
            if (iserror) {
                _mfForm.ClearAndPrepareNext();
            }


        }








        #endregion
        #region ============注册淘宝============


        public void BT_ZFB_REG_TB_REG_SJ_Click(object sender, EventArgs e) {
            //1.login
            SmsConfigHelper.GetConfigOfSms();
            if (!SmsApi.logined) {
                LogManager.WriteLog("登录失败");
                return;
            }
            //2.
            TB_ZFB_REG_TB_ShouJi.Text = SmsApi.GetPhone("2");

            string js2run =
                "document.getElementById('J_PhoneInput').value = '{0}';".With(TB_ZFB_REG_TB_ShouJi.Text.Trim()) +
                "document.getElementsByClassName('btn-b')[0].click();";

            CefFrameHelper.ExcuteJs(_mfForm.MainCefFrame, js2run);

            Application.DoEvents();
            new Thread(() => {
                Thread.Sleep(2000);
                BT_ZFB_REG_TB_SMS_ENTERVCODE_Click(sender, e);
            }).Start();
            //    BT_ZFB_REG_TB_SMS_ENTERVCODE_Click(sender, e);
        }
        private void BT_SMS_Login_Click(object sender, EventArgs e) {


            MessageBox.Show(GlobalVar.smsName + "|" + GlobalVar.SmsPwd);
        }



        private AutoResetEvent msgAutoResetEvent = new AutoResetEvent(true);
        private void BT_ZFB_REG_TB_SMS_ENTERVCODE_Click(object sender, EventArgs e) {
            string res = "";
            bool isRevicedSmsCode = false;
            LogManager.WriteLog("msgAutoResetEvent 阻塞");
            msgAutoResetEvent.Reset();
            new Thread(() => {
                for (int i = 0; i < 5; i++) {
                    Thread.Sleep(3000);
                    res = SmsApi.GetMessage("2", TB_ZFB_REG_TB_ShouJi.Text);
                    //您于2014年05月11日申请了手机验证,校验码是295768。如非本人操作,请拨0571-88158198【淘宝网】
                    if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                        isRevicedSmsCode = true;
                        msgAutoResetEvent.Set();
                        LogManager.WriteLog("msgAutoResetEvent 释放");
                        break;
                    }

                }
                if (!isRevicedSmsCode) {
                    msgAutoResetEvent.Set();
                    LogManager.WriteLog("msgAutoResetEvent 释放.没有收到信息");
                }


            }).Start();


            LogManager.WriteLog("msgAutoResetEvent 等待");
            msgAutoResetEvent.WaitOne();
            LogManager.WriteLog("msgAutoResetEvent 继续");

            TB_SMS_MSG.Text = res;

            if (isRevicedSmsCode) {

                int index1 = res.IndexOf("是") + 1;
                string num = res.Substring(index1, 6);
                LBS_sms.Text = num;
                TB_SMS_vcode.Text = num;
                string js2run =
                    "     document.getElementById('J_PhoneCheckCode').value='{0}';".With(TB_SMS_vcode.Text) +
                    "document.getElementsByClassName('btn-s')[0].click();";
                CefFrameHelper.ExcuteJs(_mfForm.MainCefFrame, js2run);
            } else {
                TB_SMS_MSG.Text = "没有收到验证码，耐心等等";
            }



        }

        private void SendEmail(object sender, EventArgs e) {

        }

        #endregion

        public void BT_ZFB_DM_Click(object sender, EventArgs e) {


        }

        public void BT_ZFB_REG_VCODE_Click(object sender, EventArgs e) {

            pictureBox1.Image = Image.FromFile(
                                Application.StartupPath + "//tmp//zhifubaoVcode.jpg");
        }

        public void BT_LZ_Vcode_Click(object sender, EventArgs e) {

            string tip;
            bool isok = true;
            string returnMess = "";// RecYZM(path, "q2601598871", "q2601598871");
            LogManager.WriteLog("Vcode start");
            //if (checkBox2.Checked) {
            //    _mfForm.BT_Zfb_Reg_Sumbit_s1_Click(sender, e);
            //} else {
            isok = LianZhongVcode.Vcode(out returnMess, out tip);
            LogManager.WriteLog("Vcode end");
            if (isok) {
                LogManager.WriteLog("Vcode ok");
                TB_ZFB_REG_VCODE.Text = returnMess;
                _mfForm.TB_VCode.Text = returnMess;
                _mfForm.BT_Zfb_Reg_Sumbit_s1_Click(sender, e);
            } else {
                LB_VCODE_TIP.Text = "失败，准备重新验证";
            }
        }

        //第二套

        public void button1_Click(object sender, EventArgs e) {

            MainForm.taskRunner = new RegTaskRunner("待测试账号.txt", "已测试账号.txt");

            _mfForm.tbzfbs = HaoziHelper.importAccounts();
            MainForm.currentHaoZi = _mfForm.tbzfbs[_mfForm.haoziindex];
            //新建table.
            displaylist(_mfForm.tbzfbs, DGV2);
        }




        private string splitResult = "";

        private void BT_SplitLine_Click(object sender, EventArgs e) {

            splitResult = "";
            var strs = File.ReadAllLines("account//" + TB_File.Text);

            foreach (var str in strs) {
                var strSplit = str.Split('|');
                string line = TB_Line.Text;
                var intline = line.Split('|');
                string resLine = "";

                int index = 0;
                int length = intline.Length;
                foreach (var s in intline) {
                    if (string.IsNullOrEmpty(s)) {
                        continue;
                    }
                    resLine = resLine + strSplit[int.Parse(s)];

                    if (index + 1 < length) {
                        resLine += "|";
                    }
                    index++;

                }

                splitResult += resLine + Environment.NewLine;
            }
            TB_Result.Text = splitResult;
            File.WriteAllText("account//" + TB_File.Text + ".res.txt", splitResult);
        }

        //GetMobilePhoneTbHandler
        private object changeTel = new object();
        //淘宝V2 更换手机号码
        //1.登录手机 
        //2.获取可用手机()
        //  2.1 获取手机
        //3.获取可用短信
        public void BT_TB_ChangeTel_Click(object sender, EventArgs e) {
            //1.登录手机 
            SmsConfigHelper.GetConfigOfSms();

            if (!SmsApi.logined) {
                LBS_sms.Text = "登录失败";
                return;
            }
            string phone = "";


            Application.DoEvents();
            lock (changeTel) {
                bool getPhoneOK = false;
                bool isVcodeOk = false;
                new Thread(() => {
                    Thread.Sleep(2000);
                    // bool isTelOK = false;
                    for (int i = 0; i < 7; i++) {

                        //2.1 检测是否当前已经通过手机验证
                        if (MainForm.state == BusinessStatus.regitster_confirm) {
                            LogManager.WriteLog("当前不是获取手机号码{0}的页面了.".With(phone));
                            MainForm.state = BusinessStatus.new_email_reg_two;
                            _mfForm.MainCefFrame.Browser.GoBack();

                            getPhoneOK = true;
                            break;
                        }

                        //2.2 获取手机
                        phone = EnterPhone();

                        //2.3 检测手机是否可用
                        LogManager.WriteLog("check valueOfStyle");
                        string valueOfStyle = "";
                        bool is_valueOfStyle_ok = false;
                        for (int j = 0; j < 3; j++) {
                            valueOfStyle = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J_PhoneFormTip", "div", "style").FirstOrDefault();
                            LogManager.WriteLog("valueOfStyle:" + valueOfStyle);
                            is_valueOfStyle_ok = valueOfStyle != null && valueOfStyle.Contains("hidden");
                            if (is_valueOfStyle_ok) {
                                break;
                            } else {
                                Application.DoEvents();
                                Thread.Sleep(2000);
                            }
                        }


                        if (valueOfStyle != null && valueOfStyle.Contains("hidden")) {
                            //可用
                            LogManager.WriteLog("手机号码 {0} 可用.".With(phone));
                            isVcodeOk = UseThisPhoneNum(phone);
                            if (isVcodeOk) {
                                break;
                            } else {
                                //未获取到验证码的处理
                                TB_SMS_MSG.Text = "没有收到验证码，返回重新接收";
                                LogManager.WriteLog(TB_SMS_MSG.Text);
                                //   new Thread(() => {
                                //页面上点击 返回修改手机号码
                                string js2run = "document.getElementById('J_RewritePhone').click()';";
                                CefFrameHelper.ExcuteJs(_mfForm.MainCefFrame, js2run);

                            }
                        } else {
                            LogManager.WriteLog("手机号码 {1} 不可用.重新获取,{0}".With(5 - i, phone));
                            //释放单个手机
                            SmsApi.ReleasePhone(phone, "2");
                        }
                    }
                    if (!getPhoneOK || !isVcodeOk) {
                        _mfForm.ShowStatus("获取手机失败，重新获取，或者自己去网站获取。");
                    }

                }).Start();
            }

            //    BT_ZFB_REG_TB_SMS_ENTERVCODE_Click(sender, e);
        }

        private bool UseThisPhoneNum(string phone) {
            string Vcode = "";
            bool getPhoneOK;

            getPhoneOK = SmsConfigHelper.GetSmsOfPhone(phone, ref Vcode, "2");
            //////////////////
            if (getPhoneOK) {
                string num = Vcode;
                TB_TB_VCODE.Text = num;
                string js2run =
                    "document.getElementById('J_PhoneCheckCode').value='{0}';".With(num) +
                    "document.getElementsByClassName('btn-s tsl')[0].click();";
                CefFrameHelper.ExcuteJs(_mfForm.MainCefFrame, js2run);//提交验证码
            }
            //////////////////
            return getPhoneOK;
        }

        // 获取手机号码，并填入网页中
        private string EnterPhone() {
            string phoneNum = "";
            for (int i = 0; i < 5; i++) {
                phoneNum = SmsApi.GetPhone("2");
                if (string.IsNullOrEmpty(phoneNum) || phoneNum == "未获取到号码") {
                    LogManager.WriteLog("未获取到号码,等待5秒钟");
                    Thread.Sleep(5000);
                } else {
                    break;
                }
            }
            new Thread(() => {
                string js2run =
                 "document.getElementById('J_PhoneInput').value = '{0}';".With(phoneNum) +
                 "document.getElementsByClassName('btn-b tsl')[1].click();";
                CefFrameHelper.ExcuteJs(_mfForm.MainCefFrame, js2run);
                LogManager.WriteLog("{0} is fill ".With(phoneNum));
            }).Start();
            Thread.Sleep(2000);
            Application.DoEvents();
            TB_TB_TELNUM.Text = phoneNum;
            return phoneNum;
        }
        //TB V2


        public void BT_TB_REG_Click(object sender, EventArgs e) {

            var op = new Operation(MainCefFrame) { };
            op.PerviousStatus = op.NextStatus = BusinessStatus.new_email_reg_two;
            op.OperationHandler += _mfForm.TB_Reg_TB2_Click;
            _mfForm.ovtimeTick.Next(op, null);//计时器重新计时

            _mfForm.ClearAndPrepareNext();
        }

        private void BT_ADSL_RECONN_Click(object sender, EventArgs e) {


        }

        private void BT_ProduceZH_Click(object sender, EventArgs e) {
            new ProdeuceAccountForm().Show();
        }

        private void TB_State_1_Click(object sender, EventArgs e) {
            MainForm.state -= 1;
        }
        #region =========测试=========
        private void BT_Test_HttpHead_Click(object sender, EventArgs e) {
            string line = FileHelper.RandomReadOneLine("Resources//httpHead.txt");
        }
        #endregion

        private AccountFilter afForm;
        private void BT_AccountFilter_Click(object sender, EventArgs e) {
            if (afForm == null) {
                afForm = new AccountFilter();
            }
            afForm.Show();
        }

        private void BT_INIT_HAOZI_Click(object sender, EventArgs e) {
            MainForm.currentHaoZi = _mfForm.tbzfbs[_mfForm.haoziindex];
        }

        private void BT_WY_Change_Click(object sender, EventArgs e) {

        }

        private void BT_InitAndUpload_Click(object sender, EventArgs e) {
            MainForm.currentHaoZi = HaoziHelper.InitXiaoHaoFromLine("宛圣豪阴南荣喻09wt|nesyakdqk25@163.com|bmcbjkgjf7|umx87164|kucc1112|邓文丽|431003198108073241|12|1|要斌|5|1|锋惠|13|1|傲霜大");
            HaoziHelper.UpdateLoadHaozi(ConfigHelper.GetValue("WebSite"), MainForm.currentHaoZi, "add");
            ;
        }

        private void BT_UpLoad_Click(object sender, EventArgs e) {
            //foreach (var xiaohao in _mfForm.tbzfbs) {
            //    HaoziHelper.UpdateLoadHaozi(ConfigHelper.GetValue("WebSite"), xiaohao, "add2origin");
            //}

        }

        private void BT_export_Click(object sender, EventArgs e) {
            string host = ConfigHelper.GetValue("WebSite");
            string name = ConfigHelper.GetValue("name");
            string pwd = ConfigHelper.GetValue("pwd");
            int GetHaoZiNum = int.Parse(ConfigHelper.GetValue("GetHaoZiNum"));

            tbzfbBll bll = new tbzfbBll();
            var list = bll.GetHaoZi(host, name, pwd, GetHaoZiNum, tbModel.shopv2.ToString());

            HaoziHelper.logAccount(list, "主-待购物账号.txt", "主-已购物账号.txt");



        }

        private void TB_logText_TextChanged(object sender, EventArgs e) {
            this.TB_logText.SelectionStart = this.TB_logText.Text.Length;
            this.TB_logText.SelectionLength = 0;
            this.TB_logText.ScrollToCaret();

        }




    }

}
