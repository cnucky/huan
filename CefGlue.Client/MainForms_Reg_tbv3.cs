using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using Common;
using Common.smsapi;
using Common.Vcode;
using Newtonsoft.Json;
using OpenPop.Common.Logging;
using OpenPop.Pop3;
using PwBusiness;
using PwBusiness.Model;
using Xilium.CefGlue.Client.VCode;
using Xilium.CefGlue.WindowsForms;
using NUnit.Framework;
namespace Xilium.CefGlue.Client {
    [TestFixture]
    public partial class MainForm : Form {

        private bool IsInit_tbV3_loaded = false;

        public void lead2tbV3reg() {

            state = BusinessStatus.Tb_reg_v3_member_reg_fill_mobile_before;
            MainCefFrame.LoadUrl("http://reg.taobao.com/member/new_register.jhtml");
            this.Activate();
        }


        private AutoResetEvent checkVcodeOkEvent = new AutoResetEvent(false);

        private void tbV3_Init_reg() {
            if (IsInit_tbV3_loaded) {
                return;
            }
            IsInit_tbV3_loaded = true;

            Operation fill_mobile = new Operation(MainCefFrame) {
                CurrentUrl = "http://reg.taobao.com/member/reg/fill_mobile.htm",
                index = 1.1,
                PerviousStatus = BusinessStatus.Tb_reg_v3_member_reg_fill_mobile_before,
                NextStatus = BusinessStatus.Tb_reg_v3_member_reg_fill_mobile,

            };

            fill_mobile.OperationHandler += (s, e) => {
                string js2run = "document.getElementsByClassName('f12')[1].getElementsByTagName('a')[0].click()";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
            };


            //填会员信息
            Operation new_register = new Operation(MainCefFrame) {
                CurrentUrl = "http://reg.taobao.com/member/reg/fill_email.htm",
                index = 1.2,
                PerviousStatus = BusinessStatus.Tb_reg_v3_member_reg_fill_mobile,
                NextStatus = BusinessStatus.Tb_reg_v3_member_reg_fill_email,
                Deadline = ConfigHelper.GetIntValue("TBV3注册第一步耗时") * 1000 * 60,
            };

            new_register.OperationHandler += (s, e) => {


                #region ==========填会员信息&过手机验证==========

                //获取ID=J_CheckCode 的data-imgurl属性的值url
                //识别url中的图片

                string js3run = "document.getElementById('J_CheckCodeInput').focus();";

                CefFrameHelper.ExcuteJs(MainCefFrame, js3run);
                new Thread(() => {
                    Thread.Sleep(ConfigHelper.GetIntValue("等待验证码时间(秒)")*1000);
                    var isVCodeOk = false;
                    for (int j = 0; !isVCodeOk && j < 3; j++) {
                        bool vcodeFrom51 = false;
                        string srcs = "";
                        for (int i = 0; i < 3; i++) {
                            //srcs = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J_CheckCodeContainer", "div", "data-imgurl").FirstOrDefault();


                            srcs = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J_CheckCodeImg1", "img", "src").FirstOrDefault();
                            srcs = srcs.Replace("&amp;", "&");
                            if (!string.IsNullOrEmpty(srcs)) {
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(srcs)) {
                            LogManager.WriteLog("重复3次后仍然无法获取图片。");
                            return;
                        }

                        string returnMess;
                        vcodeFrom51 = Vcode.GetVcodeFormImageUrl(srcs, out returnMess);
                        if (!vcodeFrom51) {
                            continue;
                        }

                        string js2run = "document.getElementById('J_Email').value='{0}';".With(currentHaoZi.zfbEmail) +
                                        "document.getElementById('J_CheckCodeInput').value='{0}';".With(returnMess) +
                                        "document.getElementById('J_BtnEmailForm').click();";

                        CefFrameHelper.ExcuteJs(MainCefFrame, js2run);

                        #region    ==================过验证码====================



                        new Thread(() => {
                            Thread.Sleep(3000);
                            var res = "";

                            for (int i = 0; i < 3; i++) {
                                res = CefFrameHelper.GetMsgByJs(MainCefFrame, "document.getElementById('J_MobileCheck').getAttribute('style')");
                                if (!string.IsNullOrEmpty(res)) {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            if (res.Contains("block")) {
                                isVCodeOk = true;
                            }
                            checkVcodeOkEvent.Set();
                        }).Start();
                        checkVcodeOkEvent.Reset();
                        checkVcodeOkEvent.WaitOne(9000);
                        #endregion

                        if (isVCodeOk) {
                            break;
                        }


                    }

                    //手机验证

                    new Thread(() => {

                        try {
                            phoneVailed(state);
                        } catch (Exception e2) {
                            LogManager.WriteLog(e2.StackTrace + e2.ToString());
                        }

                    }).Start();
                    //
                }).Start();
                #endregion
            };

            //账户注册-1.设置登录名-接收邮件
            Operation email_sent = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_member_reg_fill_email,
                NextStatus = BusinessStatus.Tb_reg_v3_member_reg_email_sent,
                CurrentUrl = "http://reg.taobao.com/member/reg/email_sent.htm",
                index = 2,
                note = "账户注册-1.设置登录名-接收邮件"

            };
            email_sent.OperationHandler += (s, e) => {
                //  BeginInvoke(new Action(() => {
                Thread.Sleep(5000);
                var url = EmailCheck("新用户确认通知信", "request_dispatcher.htm");
                MainCefFrame.LoadUrl(url);
                //   }));
            };

            Operation fill_user_info = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_member_reg_email_sent,
                NextStatus = BusinessStatus.Tb_reg_v3_Member_reg_fill_user_info,
                CurrentUrl = "http://reg.taobao.com/member/reg/fill_user_info.htm",
                index = 3,
                note = "账户注册-2.填写账户信息"
            };
            fill_user_info.OperationHandler += (s, e) => {

                string js2runS1 =
                            "document.getElementById('J_Password').value='{0}';".With(currentHaoZi.tbPwd) +
                            "document.getElementById('J_RePassword').value='{0}';".With(currentHaoZi.tbPwd) +
                            "document.getElementById('J_Nick').value='{0}';".With(currentHaoZi.tbName) +
                            "document.getElementById('J_BtnInfoForm').click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2runS1);
            };


            Operation login_unusual = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_Member_reg_fill_user_info,
                NextStatus = BusinessStatus.Tb_reg_v3_member_reg_reg_success,
                CurrentUrl = "http://login.taobao.com/member/login_unusual.htm",
                //http://login.taobao.com/member/login_unusual.htm
                index = 4.1,
                note = "登录身份验证 ",
            };
            login_unusual.OperationHandler += (s, e) => {
                toolStripMenuItem20_Click(this, new EventArgs());//过登录验证
            };


            Operation member_reg_reg_success = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_Member_reg_fill_user_info,
                NextStatus = BusinessStatus.Tb_reg_v3_member_reg_reg_success,
                CurrentUrl = "http://reg.taobao.com/member/reg/reg_success.htm",
                index = 4.2,
                note = "淘宝注册成功 填写收货地址",
            };
            member_reg_reg_success.OperationHandler += (s, e) => {
                //var url = "http://member1.taobao.com/member/fresh/account_management.htm";

                var url = "http://member1.taobao.com/member/fresh/deliver_address.htm";
                MainCefFrame.LoadUrl(url);
            };


            Operation deliver_address = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_member_reg_reg_success,
                NextStatus = BusinessStatus.Tb_reg_v3_deliver_address,
                CurrentUrl = "http://member1.taobao.com/member/fresh/deliver_address.htm",
                index = 33,
                note = "添加收货地址 判断是否激活支付宝  ",
            };
            deliver_address.OperationHandler += (s, e) => {

                var js2run = RandomAddressPlace();
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                new Thread(() => {
                    Application.DoEvents();
                    Thread.Sleep(3000);

                    bool activeZFB = ConfigHelper.GetBoolValue("是否激活支付宝");
                    if (activeZFB) {
                        var url = "http://member1.taobao.com/member/fresh/account_management.htm";
                        MainCefFrame.LoadUrl(url);
                    } else {
                        FinishOneResgister();
                    }


                }).Start();

            };


            Operation member_fresh_account_management = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_deliver_address,
                NextStatus = BusinessStatus.Tb_reg_v3_member_fresh_account_management,
                CurrentUrl = "http://member1.taobao.com/member/fresh/account_management.htm",
                index = 5,
                note = "找到 包含 立即补全 的链接  前往"
            };

            member_fresh_account_management.OperationHandler += (s, e) => {
                string js2runS1 = "document.getElementsByClassName('kv_item')[2].getElementsByTagName('a')[0].click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2runS1);

            };


            Operation account_reg_complete_complete = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_member_fresh_account_management,
                NextStatus = BusinessStatus.Tb_reg_v3_account_reg_complete_complete,
                CurrentUrl = "https://memberprod.alipay.com/account/reg/complete/complete.htm",
                index = 6,
                note = "开通支付宝 填支付密码"
            };
            account_reg_complete_complete.OperationHandler += (s, e) => {
                string js2runS1 =
                    "         document.getElementById('queryPwd').value='{0}';".With(currentHaoZi.zfbPwd) +
                    "document.getElementById('queryPwdConfirm').value='{0}';".With(currentHaoZi.zfbPwd) +
                    "document.getElementById('payPwd').value='{0}';".With(currentHaoZi.zfbPayPwd) +
                    "document.getElementById('payPwdConfirm').value='{0}';".With(currentHaoZi.zfbPayPwd) +
                    "document.getElementById('realName').value='{0}';".With(currentHaoZi.realname) +
                    "document.getElementById('IDCardNo').value='{0}';".With(currentHaoZi.zfbSFZ) +
                    "document.getElementById('J-complete-form').submit();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2runS1);
            };


            Operation asset_paymethod_paymethod = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_account_reg_complete_complete,
                NextStatus = BusinessStatus.Tb_reg_v3_asset_paymethod_paymethod,
                CurrentUrl = "https://zht.alipay.com/asset/paymethod/paymethod.htm",
                index = 7
            };

            asset_paymethod_paymethod.OperationHandler += (s, e) => {

                MainCefFrame.LoadUrl("https://benefitprod.alipay.com/asset/paymethod/bindassetcard.htm");

            };


            Operation account_reg_success = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Tb_reg_v3_asset_paymethod_paymethod,
                NextStatus = BusinessStatus.Tb_reg_v3_account_reg_success,
                CurrentUrl = "https://memberprod.alipay.com/account/reg/success.htm",
                index = 8,
                note = "开通支付宝成功"
            };

            account_reg_success.OperationHandler += (s, e) => {

                // new Thread(() => {

                FinishOneResgister();
                //  }).Start();

            };



            this.opsList.Add(fill_mobile);
            this.opsList.Add(new_register);
            this.opsList.Add(email_sent);
            this.opsList.Add(login_unusual);

            this.opsList.Add(fill_user_info);
            this.opsList.Add(member_reg_reg_success);
            this.opsList.Add(deliver_address);
            this.opsList.Add(member_fresh_account_management);
            this.opsList.Add(account_reg_complete_complete);
            this.opsList.Add(asset_paymethod_paymethod);
            this.opsList.Add(account_reg_success);

        }

        private void FinishOneResgister() {
            currentHaoZi.regStatus = DateTime.Now.ToString();
            currentHaoZi.tbStatus = GlobalVar.phoneNum + "|" + GlobalVar.CurrentIp;
            HaoziHelper.UpdateLoadHaozi(ConfigHelper.GetValue("WebSite"), currentHaoZi, "add");
            //        Thread.Sleep(6000);
            int time = ConfigHelper.GetIntValue("停留时间(秒)");
            LogManager.WriteLog("开始下一个,停留{0}秒".With(time));
            Thread.Sleep(TimeSpan.FromSeconds(time));
            ClearAndPrepareNext();
        }
        //手机验证
        private void phoneVailed(BusinessStatus nowStatus) {

            //}

            //public void GetPhone2(object sender, EventArgs e) {
            //1.login


            string phoneNum = "";
            Application.DoEvents();
            //  lock (changeTel1) {
            bool getPhoneOK = false;
            bool isVcodeOk = false;



            int TryPhoneNumTB = int.Parse(ConfigHelper.GetValue("TryPhoneNumTB"));
            if (ConfigHelper.GetValue("smsConfigtb") == "aima" || ConfigHelper.GetValue("smsConfigtb") == "jike") {
                GlobalVar.sms = IMySmsFactory.Build(ConfigHelper.GetValue("smsConfigtb"));
                GlobalVar.sms.GetConfigOfSms();//SmsConfigHelper.GetConfigOfSms();
            }

            for (int i = 0; i < TryPhoneNumTB; i++) {
                if (ConfigHelper.GetValue("smsConfigtb") == "both") {
                    GlobalVar.sms = IMySmsFactory.Build(i % 2 == 1 ? "jike" : "aima");
                    GlobalVar.sms.GetConfigOfSms();//SmsConfigHelper.GetConfigOfSms();
                }

                Thread.Sleep(1000);
                //2.1 检测是否当前已经通过手机验证

                if (MainForm.state != nowStatus) {
                    LogManager.WriteLog("当前不是获取手机号码 的页面了.");
                    break;
                }
                //2.2 获取手机
                getPhoneOK = SmsConfigHelper.GetPhone(ref phoneNum);
                if (!getPhoneOK) {
                    continue;
                }
                //getPhoneOK = true;
                //phoneNum = GlobalVar.phoneNum;
                //2.3 填充手机
                string js2run =
        "document.getElementById('J_Mobile').value = '{0}';".With(phoneNum) +
        "document.getElementById('J_BtnMobileCode').click();";

                LogManager.WriteLog(js2run);

                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                LogManager.WriteLog("{0} is fill 填充手机 ".With(phoneNum));

                Application.DoEvents();

                //2.4 检测手机是否可用
                LogManager.WriteLog("检测手机是否可用");
                string valueOfStyle = "";
                bool is_valueOfStyle_ok = false;
                for (int j = 0; j < 2; j++) {
                    Thread.Sleep(3000);
                    valueOfStyle = CefFrameHelper.GetMsgByJs3(MainCefFrame, "document.getElementById('J_MsgMobileCode').getAttribute('class')");

                    LogManager.WriteLog("valueOfStyle:" + valueOfStyle);
                    is_valueOfStyle_ok = (valueOfStyle != null && valueOfStyle.Contains("ok"));
                    if (is_valueOfStyle_ok) {
                        break;
                    }
                    Application.DoEvents();
                }
                if (!is_valueOfStyle_ok) {
                    LogManager.WriteLog("手机号码 {1} 不可用 释放并拉黑.重新获取,{0}".With(TryPhoneNumTB - i, phoneNum));
                    //释放单个手机
                    GlobalVar.sms.ReleasePhone(phoneNum, SmsServer.tb_reg_vode); // SmsApi.ReleasePhone(phoneNum, "2");
                    continue;
                }


                //2.5 手机可用则接收短信
                LogManager.WriteLog("手机号码 {0} 可用.".With(phoneNum));
            
                string vcodeNum = "";
                isVcodeOk = SmsConfigHelper.GetSmsOfPhoneTBREGV3_1(phoneNum, ref vcodeNum, SmsServer.tb_reg_vode);

                if (!isVcodeOk) {
                    LogManager.WriteLog("没有收到验证码，返回重新接收");
                    continue;
                }

                if (ConfigHelper.GetBoolValue("tbModelTest")) {//开启测试模式，记录手机号码并释放手机号码，跳转到支付宝注册
                    GlobalVar.availabePhone = phoneNum;
                    GlobalVar.sms.ReleasePhone(phoneNum, SmsServer.tb_reg_login_vcode);
                    BT_Reg_zfb_Click(this, new EventArgs());
                }


                //2.6 接收到短信则提交
                //                document.getElementById('J_PhoneCheckCode').value='295768';
                //document.getElementsByClassName('btn-s')[0].click();

                string jsOfSubmitVcode =
         "document.getElementById('J_MobileCode').value='{0}';".With(vcodeNum) +
         "document.getElementById('J_BtnMobileCodeForm').click();" +
         "document.getElementById('J_BtnMobileCodeForm').click();";
                //J_BtnMobileCodeForm
                //J_BtnMobileForm 
                CefFrameHelper.ExcuteJs(MainCefFrame, jsOfSubmitVcode);//提交验证码
                if (GlobalVar.debug) {
                    LogManager.WriteLog(vcodeNum);
                    LogManager.WriteLog(jsOfSubmitVcode);
                }
                GlobalVar.phoneNum = phoneNum;
                break;



            }
            //     }).Start();
            //   };






        }

         
        #region =======通过邮箱找激活的链接========


        public string tbV3_GetMailFromZfb(List<string> titles) {

            string file = Application.StartupPath + "//import//popserver.txt";
            if (File.Exists(file)) {
                using (StreamReader reader = new StreamReader(File.OpenRead(file))) {
                    // This describes how the OpenPOPLogin.txt file should look like
                    mcClient.popServer = reader.ReadLine(); //  
                    mcClient.port = reader.ReadLine(); // Port
                    mcClient.ssl = bool.Parse(reader.ReadLine() ?? "true"); // Whether to use SSL or not
                    mcClient.username = reader.ReadLine(); // Username
                    mcClient.pwd = reader.ReadLine(); // Password
                }
            }
            mcClient.username = currentHaoZi.zfbEmail;
            mcClient.pwd = currentHaoZi.zfbEmailPwd; // Password
            int count = 0;
            //using 
            using (Pop3Client pop3Client = new Pop3Client()) {

                try {
                    if (pop3Client.Connected) {
                        pop3Client.Disconnect();
                    }

                    pop3Client.Connect(mcClient.popServer, int.Parse(mcClient.port), mcClient.ssl);
                    pop3Client.Authenticate(mcClient.username, mcClient.pwd);
                    count = pop3Client.GetMessageCount();
                } catch (Exception e1) {
                    LogManager.WriteLog(e1.ToString());
                    ;
                } finally {

                }

                mcClient.messages.Clear();
                for (int i = count; i >= 1; i -= 1) {
                    Application.DoEvents();
                    try {
                        OpenPop.Mime.Message message = pop3Client.GetMessage(i);
                        // Add the message to the dictionary from the messageNumber to the Message
                        mcClient.messages.Add(i, message);
                    } catch (Exception e) {
                        DefaultLogger.Log.LogError(
                            "TestForm: Message fetching failed: " + e.Message + "\r\n" +
                            "Stack trace:\r\n" +
                            e.StackTrace);
                    }
                }

            }

            string htmlbody = "";
            for (int i = count; i >= 1; i -= 1) {
                var msg = mcClient.messages[i];
                mcClient.html = msg.FindFirstHtmlVersion();
                if (mcClient.html != null) {
                    // Save the plain text to a file, database or anything you like
                    bool isFind = false;
                    foreach (var tit in titles) {
                        if (msg.Headers.Subject.Contains(tit)) {
                            //html.Save(new FileInfo("zfb_mail.txt"));
                            htmlbody = mcClient.html.GetBodyAsText();
                            isFind = true;
                            break;
                        }
                    }
                    if (isFind) {
                        break;
                    }

                }
            }
            return htmlbody;


        }
        #endregion

        public void tbV3_TB_Reg_TB2_Click(object sender, EventArgs e) {
            state = BusinessStatus.new_register;
            MainCefFrame.LoadUrl("http://reg.taobao.com/member/new_register.jhtml");
            //
        }


        //过登录验证
        private void toolStripMenuItem20_Click(object sender, EventArgs e) {
            this.Activate();

            //发送密码 
            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top,
                ConfigHelper.GetIntValue("登录验证页面发送按钮左边距"), ConfigHelper.GetIntValue("登录验证页面发送按钮上边距"));
            LogManager.WriteLog("发送按钮");
            Thread.Sleep(1000);
           

            string vcodeNum = "";
            bool isVcodeOk = false;
            LogManager.WriteLog(GlobalVar.phoneNum);

            new Thread(() => {

                try {


                    Thread.Sleep(2000);

                    string phone = GlobalVar.phoneNum;
                   SmsConfigHelper.GetPhone(ref phone,SmsServer.tb_reg_login_vcode);
                    isVcodeOk =
                       SmsConfigHelper.GetSmsOfPhone(GlobalVar.phoneNum, ref vcodeNum, SmsServer.tb_reg_login_vcode);

                    if (!isVcodeOk) {
                        LogManager.WriteLog("没有收到验证码 ");
                    }
                    LogManager.WriteLogIfDebug("释放");
                    GlobalVar.IsloadOkAutoResetEvent.Set();

                } catch (Exception e1) {
                    LogManager.WriteLog(e1.ToString());

                }
            }) { IsBackground = true }.Start();


            LogManager.WriteLogIfDebug("阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();
            LogManager.WriteLogIfDebug("等待2分钟");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromMinutes(2));

            try {
                if (!isVcodeOk) {
                    taskRunner.ReportAccountStatus("reg", "failed", GlobalVar.phoneNum + "|" + GlobalVar.CurrentIp);
                    return;
                }

                //输入验证码
                MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top,
                      ConfigHelper.GetIntValue("登录验证页面输入框左边距"), ConfigHelper.GetIntValue("登录验证页面输入框上边距"));
                LogManager.WriteLog("输入框");
                //Thread.Sleep(3000);
                LogManager.WriteLog("输入");
                MouseKeyBordHelper.KeyBoardDo(vcodeNum.ToCharArray());
                Thread.Sleep(1000);
                //提交
                LogManager.WriteLog("提交");
                MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top,
                      ConfigHelper.GetIntValue("登录验证页面确定按钮左边距"), ConfigHelper.GetIntValue("登录验证页面确定按钮上边距"));
            } catch (Exception e1) {
                LogManager.WriteLog(e1.ToString());

            }

        }

        private void Test1() {
            Thread.Sleep(3000);
            //发送密码 
            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top,
                ConfigHelper.GetIntValue("登录验证页面发送按钮左边距"), ConfigHelper.GetIntValue("登录验证页面发送按钮上边距"));
            LogManager.WriteLog("发送按钮");
            Thread.Sleep(3000);
            //输入验证码
            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top,
                ConfigHelper.GetIntValue("登录验证页面输入框左边距"), ConfigHelper.GetIntValue("登录验证页面输入框上边距"));
            LogManager.WriteLog("输入框");
            Thread.Sleep(3000);
            LogManager.WriteLog("输入");
            string s1 = "1234";
            MouseKeyBordHelper.KeyBoardDo(s1.ToCharArray());
            Thread.Sleep(3000);
            //提交
            LogManager.WriteLog("提交");
            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top,
                ConfigHelper.GetIntValue("登录验证页面确定按钮左边距"), ConfigHelper.GetIntValue("登录验证页面确定按钮上边距"));


            Thread.Sleep(3000);
        }
    }

}