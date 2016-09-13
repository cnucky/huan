using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using Common.smsapi;
using PwBusiness;
using Xilium.CefGlue.Client.VCode;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {
    public partial class MainForm : Form {

        public BusinessForms bform;
        private void OnPageLoaded_zfb_Reg(object s, LoadEndEventArgs e) {
            LogManager.WriteLog("[" + e.Frame.Url + "," + state + "]");

            string currentUrl = e.Frame.Url;

            OperationTest(s, e, currentUrl);

        }

        #region =============注册支付宝=============

        private object lockobject = new object();
        public void BT_Reg_zfb_Click(object sender, EventArgs e) {

            state = BusinessStatus.zfb_reg_begin;
            MainCefFrame.LoadUrl(GlobalVar.zfb_Reg_enter_1);

        }

        private bool isDownLoadImg = false;
        public void BT_Zfb_Fill_Account_Click(object sender, EventArgs e) {
            //  state = BusinessStatus.zfb_reg_link_complete;

            Operation operation = sender as Operation;
            if (operation != null) {
                if (!MainCefFrame.Url.Contains(GlobalVar.zfb_Reg_enter_1)) {
                    LogManager.WriteLog("已经不是当前页了");
                    return;
                }

            }
            for (int i = 0; i < 7; i++) {
                //利用hpk获取到url
                string srcs = "";
                for (int g = 0; g < 3; g++) {
                    srcs = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J-checkcode-img", "img", "src").FirstOrDefault();
                    if (!string.IsNullOrEmpty(srcs)) {
                        break;
                    }
                    Application.DoEvents();
                    Thread.Sleep(2000);
                }
                if (string.IsNullOrEmpty(srcs)) {
                    LogManager.WriteLog("重复3次后仍然无法获取图片的链接。");
                    return;
                }


                //2获取验证码

                string vcode;
                bool isVCodeOk = Vcode.GetVcodeFormImageUrl(srcs, out vcode);
                LogManager.WriteLog("{0}    {1} ".With(isVCodeOk, vcode));

                //3 提交验证码
                if (isVCodeOk) {
                    string js2run = " document.getElementById('J-accName').value = '{0}';".With(currentHaoZi.zfbEmail)
                                    + " document.getElementById('J-checkcode').value = '{0}';".With(vcode)
                                    + "document.getElementById('J-index-form').submit();"
                                    + "document.getElementsByClassName('ui-button-text')[0].click();";
                    CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                } else {
                    LogManager.WriteLog("请手动刷新并输入验证码，再提交。");
                }

                //4 检查验证码是否正确
                //2  检测 是否可用
                //LogManager.WriteLog("check checkcodeIcon");
                //string valueOfStyle = "";
                //bool is_valueOfStyle_ok = false;
                //for (int j = 0; j < 3; j++) {
                //    valueOfStyle =
                //        CefFrameHelper.GetUrlListByHapId(MainCefFrame, "checkcodeIcon", "span", "class")
                //            .FirstOrDefault();
                //    LogManager.WriteLog("checkcodeIcon:" + valueOfStyle);
                //    is_valueOfStyle_ok = !(valueOfStyle == null || valueOfStyle.Contains("checkcodeIcon-wrong"));
                //    if (is_valueOfStyle_ok) {
                //        break;
                //    }
                //    Application.DoEvents();
                //    Thread.Sleep(2000);

                //}

                //if (!is_valueOfStyle_ok) {
                //    continue;
                //}

                //手机过验证码
                mobilephoeOfzfb();


                //收取邮件
                //BT_Zfb_Reg_Sumbit_s1_Click(vcode, new EventArgs());
                break;
            }

        }

        private void mobilephoeOfzfb() {


            new Thread(() => {
                Thread.Sleep(5000);
                try {
                    mobilephoeOfzfb2();
                } catch (Exception e2) {
                    LogManager.WriteLog(e2.StackTrace + e2.ToString());
                }

            }).Start();
        }
        //zfb 过验证
        private void mobilephoeOfzfb2() {


            //1.login
            GlobalVar.sms = IMySmsFactory.Build(ConfigHelper.GetValue("smsConfigzfb"));
            GlobalVar.sms.GetConfigOfSms();//SmsConfigHelper.GetConfigOfSms();
            //if (!SmsApi.logined) {
            //    LogManager.WriteLog("登录失败");
            //    return;
            //}
            //2.
            string phoneNum = "";
            Application.DoEvents();
            //  lock (changeTel1) {
            bool getPhoneOK = false;
            bool isVcodeOk = false;

            int TryPhoneNum = int.Parse(ConfigHelper.GetValue("TryPhoneNum"));
            int TryPhoneNumInterTime = int.Parse(ConfigHelper.GetValue("TryPhoneNumInterTime"));
            for (int i = 0; i < TryPhoneNum; i++) {
                Thread.Sleep(2000);
                //2.1
                if (MainForm.state != BusinessStatus.emailCheck) {
                    LogManager.WriteLog("当前不是获取手机号码 的页面了.");
                    break;
                }

                //2.2 获取手机

                //GetPhone(ref string phoneNum,string serverid) {
              
                //支付宝注册
                getPhoneOK = GetPhone(ref phoneNum, SmsServer.zfb_reg_vcode);
                if (!getPhoneOK) {
                    continue;
                }

                //2.3 填充手机
                string js2run = "var mydoc=window.frames[2].document;";
                js2run +=
        "mydoc.getElementById('J-secure-mobile').value = '{0}';".With(phoneNum) +
        "mydoc.getElementById('J-getCheckcodeBtn').click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                LogManager.WriteLog("{0} is fill ".With(phoneNum));
                #region MyRegion

                // 2.4 检测手机是否可用
                LogManager.WriteLog("check valueOfStyle");
                string valueOfStyle = "";
                bool is_valueOfStyle_ok = false;
                for (int j = 0; j < 3; j++) {
                    // valueOfStyle = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J-resend-check-code", "div", "class").FirstOrDefault();
                    valueOfStyle = CefFrameHelper.GetMsgByJs(MainCefFrame, "mydoc.getElementById('J-resend-check-code').getAttribute('class')");
                    //.GetUrlListByHapId(MainCefFrame, "J-resend-check-code", "div", "class").FirstOrDefault();

                    //
                    LogManager.WriteLog("valueOfStyle:" + valueOfStyle);
                    is_valueOfStyle_ok = (valueOfStyle != null && !valueOfStyle.Contains("fn-hide"));
                    if (is_valueOfStyle_ok) {
                        break;
                    }

                    Application.DoEvents();
                    Thread.Sleep(2000);
                }
                if (!is_valueOfStyle_ok) {
                    LogManager.WriteLog("手机号码 {1} 不可用.重新获取,{0}".With(7 - i, phoneNum));
                    //释放单个手机
                 //   SmsApi.ReleasePhone(phoneNum, "1365");
                    continue;
                }

                #endregion

                //2.5 手机可用则接收短信

                LogManager.WriteLog("检查通过，手机号码 {0} 可用.".With(phoneNum));
                string vcodeNum = "";
                //  isVcodeOk = SmsConfigHelper.GetSmsOfPhone(phoneNum, ref vcodeNum,SmsServer.zfb_reg_vcode);
                //    isVcodeOk = SmsConfigHelper.GetSmsOfPhone(phoneNum, ref vcodeNum, SmsServer.zfb_reg_vcode);
                isVcodeOk = SmsConfigHelper.GetSmsOfPhone(phoneNum, ref vcodeNum, SmsServer.zfb_reg_vcode);

                if (!isVcodeOk) {
                    LogManager.WriteLog("没有收到验证码，返回重新接收");

                    //页面上点击 返回修改手机号码
                    //string jsOfChangePhoneNum = "document.getElementById('J_RewritePhone').click()';";
                    //CefFrameHelper.ExcuteJs(MainCefFrame, jsOfChangePhoneNum);
                    continue;
                }
                //2.6 接收到短信则提交
                //                document.getElementById('J_PhoneCheckCode').value='295768';
                //document.getElementsByClassName('btn-s')[0].click();

                string jsOfSubmitVcode = "var mydoc=window.frames[2].document;";
                jsOfSubmitVcode += "mydoc.getElementById('J-checkcode').value='{0}';".With(vcodeNum) +
         "mydoc.getElementById('J-submit-btn').click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, jsOfSubmitVcode);//提交验证码
                LogManager.WriteLog("tel {0} ".With(phoneNum));
                GlobalVar.phoneNum = phoneNum;

                break;



            }
            //     }).Start();
            //   };
        }


        public void BT_Zfb_Reg_Sumbit_s1_Click(object sender, EventArgs e) {
            new Thread(
                () => {
                    bfForms.BT_test1_Click(this, e);

                }
                ).Start();
        }


        public void BT_Link_Continue_Reg_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_link_complete;
            MainCefFrame.LoadUrl(currentHaoZi.activeLink);
        }

        private void BT_Zfb_reg_fill_Click(object sender, EventArgs e) {
            if (currentHaoZi.isLoadedCompetedFillLink) {
                return;
            }
            currentHaoZi.isLoadedCompetedFillLink = true;

            state = BusinessStatus.zfb_Reg_paymethod;
            string js1 = "document.getElementById('queryPwd').value='{0}';document.getElementById('queryPwdConfirm').value='{0}';".With(currentHaoZi.zfbPwd) +
                         "document.getElementById('payPwd').value='{0}';document.getElementById('payPwdConfirm').value='{0}'; ".With(currentHaoZi.zfbPayPwd) +
                         "document.getElementById('realName').value='{0}';".With(currentHaoZi.realname) +
                         "document.getElementById('IDCardNo').value='{0}';".With(currentHaoZi.zfbSFZ) +
                         "document.getElementById('J-complete-form').submit();";
            MainCefFrame.ExecuteJavaScript(js1, MainCefFrame.Url, 0);

            return;
            #region  废弃
            //document.getElementById('queryPwd').focus(); 
            //document.getElementById('queryPwd').value = "aqq6611"; 
            //document.getElementById('queryPwd').blur(); 
            //密码id  //queryPwd
            //CefFrameHelper.SetElementValueById(MainCefFrame, "queryPwd", currentHaoZi.zfbPwd);




            //var host = MainCefFrame.Browser.GetHost();

            //foreach (var c in Debug_keyBord) {
            //    // little hacky
            //    host.SendKeyEvent(new CefKeyEvent {
            //        EventType = CefKeyEventType.Char,
            //        Modifiers = CefEventFlags.None,
            //        WindowsKeyCode = c,
            //        NativeKeyCode = c,
            //        Character = c,
            //        UnmodifiedCharacter = c,
            //    });
            //}
            //MainCefFrame.ExecuteJavaScript("document.getElementById('queryPwd').blur(); ", MainCefFrame.Url, 0);


            ////确认密码id //queryPwdConfirm
            //CefFrameHelper.SetElementValueById(MainCefFrame, "queryPwdConfirm", currentHaoZi.zfbPwd);
            ////支付密码  //PayPwd
            //CefFrameHelper.SetElementValueById(MainCefFrame, "PayPwd", currentHaoZi.zfbPayPwd);
            ////确认支付密码//payPwdConfirm
            //CefFrameHelper.SetElementValueById(MainCefFrame, "payPwdConfirm", currentHaoZi.zfbPayPwd);
            ////名字//realName
            //CefFrameHelper.SetElementValueById(MainCefFrame, "realName", currentHaoZi.realname);
            ////ShenFenZheng//IDCardNo
            //CefFrameHelper.SetElementValueById(MainCefFrame, "IDCardNo", currentHaoZi.zfbSFZ);

            #endregion
        }

        //   
        private void BT_ZFB_TB_Reg_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_account_reg_success;
            MainCefFrame.LoadUrl(GlobalVar.ZFB_REG_bindassetcard);

            //   var linkList = CefFrameHelper.GetUrlListByHap(MainCefFrame, "先跳过，注册成功");
            //if (linkList.Count > 0) {
            //    MainCefFrame.LoadUrl(linkList[0]);
            //} else
            //{
            //    LB_Debug_info.Text = "没有找到注册的链接";
            //}

            //  state = BusinessStatus.zfb_reg_taobao_open;
            // MainCefFrame.LoadUrl(GlobalVar.zfb_reg_taobao_open);
        }
        //注册成功 
        private void BT_ZFB_REG_Account_Sucess_Click(object sender, EventArgs e) {
            state = BusinessStatus.Zfb_reg_shifou_bangding_shouji_before;
            MainCefFrame.LoadUrl("https://my.alipay.com/portal/account/index.htm");

        }

        //添加密保成功
        private void Add_SQ_SUCESS(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_open_tb_before;
            MainCefFrame.LoadUrl(GlobalVar.zfb_reg_skip_bindassetcard);//开通淘宝


        }
        //开通淘宝1

        private void BT_ZFB_REG_TB_open_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_taobao_new_alipay_q;

            FindTitleAndLoadHrefByJS("开通");
            //var href = FindTitleHref("开通");
            //if (href != "") {
            //    NewTab(href);
            //}

            //if (isThreadStart) {
            //    return;
            //}
            //isThreadStart = true;
            new Thread(() => {

                currentHaoZi.opentbTime++;
                if (currentHaoZi.opentbTime < 12) {

                    Thread.Sleep(1000 * 3);
                    if (MainCefFrame.Url.Contains(GlobalVar.zfb_reg_taobao_new_alipay_q)) {
                        LogManager.WriteLog("已经进入 页面：" + borrowTitle);

                    } else {
                        state = BusinessStatus.zfb_reg_account_reg_success;
                        LogManager.WriteLog("重新试试：{0}".With(currentHaoZi.opentbTime));
                        MainCefFrame.LoadUrl("https://my.alipay.com/portal/account/index.htm");

                        //      break;
                    }

                }

            }).Start();



        }

        private void FindTitleAndLoadHrefByJS(string p) {
            string js2run =
                "var hrefLinks=document.getElementsByTagName('a');" +
                "for(var i=0;i<hrefLinks.length;i++){ " +
                "if(hrefLinks[i].innerHTML=='" + p + "'){" +
                "hrefLinks[i].click();break;}}";
            CefFrameHelper.ExcuteJs(MainCefFrame, js2run);

        }
        //
        private void BT_ZFB_REG_RB_LOGINTB_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_taobao_login2reg;
        }

        //登录淘宝
        private void BT_ZFB_REG_TB_LOGIN_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_taobao_new_alipay_q;
            ClickById("J_SubmitQuick");
            //var linkList = CefFrameHelper.GetUrlListByHap(MainCefFrame, "支付宝登录");
            //if (linkList.Count > 0) {
            //    MainCefFrame.LoadUrl(linkList[0]);
            //}
            //else
            //{
            //    LB_Debug_info.Text = @"没有找到支付宝链接";
            //}


        }
        //手机验证码
        //  public EventHandler GetMobilePhoneHandler;
        //
        private void BT_ZFB_REG_TB_new_alipay_q_Click(object sender, EventArgs e) {
            //1.输入信息
            LogManager.WriteLog(DateTime.Now.ToString() + "   " + currentHaoZi.zfbPayPwd + "    " + currentHaoZi.tbName);
            MainForm.state = BusinessStatus.nav2deliver_address;

            //string js1 = "document.getElementById('J_Nick').value = '{0}';".With(currentHaoZi.tbName) +
            //               "document.getElementsByClassName('btn-long')[0].click();";
            //CefFrameHelper.ExcuteJs(MainCefFrame, js1);


            //"document.getElementById('payPassword_rsainput').value = '{0}';".With(currentHaoZi.zfbPayPwd) +

            //输入手机号码
            int chax = ConfigHelper.GetIntValue("开通淘宝页面左边距");  // 600 - 61;
            int chay = ConfigHelper.GetIntValue("开通淘宝页面上边距");  // 562 - 171;
            

            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay);

            LogManager.WriteLog(currentHaoZi.zfbPayPwd);
            MouseKeyBordHelper.KeyBoardDo(currentHaoZi.zfbPayPwd.ToCharArray());

            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay + ConfigHelper.GetIntValue("开通淘宝页面待移动距离"));
            new Thread(() => {
                Thread.Sleep(2000);
                //2.手机验证
                string js2run =// "document.getElementById('payPassword_rsainput').value = '{0}';".With(currentHaoZi.zfbPayPwd) +
                    // " document.getElementsByClassName('btn-s')[1].click();" +
                              "document.getElementById('J_Nick').value = '{0}';".With(currentHaoZi.tbName) +
                              "document.getElementsByClassName('btn-long')[0].click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);

                new Thread(() => {
                    Thread.Sleep(3000);

                    try {
                        GetPhone(sender, e);
                    } catch (Exception e2) {

                        LogManager.WriteLog(e2.StackTrace + e2.ToString());
                    }

                }).Start();

            }).Start();
        }


        public void GetPhone(object sender, EventArgs e) {
            //1.login


            //if (!SmsApi.logined) {
            //    LogManager.WriteLog("登录失败");
            //    return;
            //}
            //2.
            string phoneNum = "";
            Application.DoEvents();
            //  lock (changeTel1) {
            bool getPhoneOK = false;
            bool isVcodeOk = false;
            //   new Thread(() => {
            //   Thread.Sleep(2000);


            int TryPhoneNumTB = int.Parse(ConfigHelper.GetValue("TryPhoneNumTB"));

            if (ConfigHelper.GetValue("smsConfigtb") == "aima" || ConfigHelper.GetValue("smsConfigtb") == "jike") {

                GlobalVar.sms = IMySmsFactory.Build(ConfigHelper.GetValue("smsConfigtb"));
            }

            for (int i = 0; i < TryPhoneNumTB; i++) {

                if (ConfigHelper.GetValue("smsConfigtb") == "both") {
                    GlobalVar.sms = IMySmsFactory.Build(i % 2 == 1 ? "jike" : "aima");
                }



                GlobalVar.sms.GetConfigOfSms();//SmsConfigHelper.GetConfigOfSms();
                Thread.Sleep(1000);
                //2.1 检测是否当前已经通过手机验证
           
                if (MainForm.state != BusinessStatus.nav2deliver_address) {
                    LogManager.WriteLog("当前不是获取手机号码 的页面了.");

                    break;
                }
                //2.2 获取手机
              
                getPhoneOK = GetPhone(ref phoneNum);//开通淘宝
                if (!getPhoneOK) {
                    continue;
                }
                //getPhoneOK = true;
                //phoneNum = GlobalVar.phoneNum;
                //2.3 填充手机
                string js2run =
        "document.getElementById('J_PhoneInput').value = '{0}';".With(phoneNum) +
        "document.getElementsByClassName('btn-b')[0].click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                LogManager.WriteLog("{0} is fill 填充手机 ".With(phoneNum));

                //2.4 检测手机是否可用
                LogManager.WriteLog("检测手机是否可用");
                string valueOfStyle = "";
                bool is_valueOfStyle_ok = false;
                for (int j = 0; j < 2; j++) {
                    Thread.Sleep(3000);
                    valueOfStyle = CefFrameHelper.GetMsgByJs3(MainCefFrame, "document.getElementById('J_PhoneFormTip').getAttribute('style')");

                    //    valueOfStyle = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J_PhoneFormTip", "div", "style").FirstOrDefault();
                    LogManager.WriteLog("valueOfStyle:" + valueOfStyle);
                    is_valueOfStyle_ok = (valueOfStyle != null && valueOfStyle.Contains("hidden"));
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
                isVcodeOk = SmsConfigHelper.GetSmsOfPhone(phoneNum, ref vcodeNum, SmsServer.tb_reg_vode);

                if (!isVcodeOk) {
                    LogManager.WriteLog("没有收到验证码，返回重新接收");

                    //页面上点击 返回修改手机号码
                    string jsOfChangePhoneNum = "document.getElementById('J_RewritePhone').click()';";
                    CefFrameHelper.ExcuteJs(MainCefFrame, jsOfChangePhoneNum);
                    continue;
                }
                //2.6 接收到短信则提交
                //                document.getElementById('J_PhoneCheckCode').value='295768';
                //document.getElementsByClassName('btn-s')[0].click();

                string jsOfSubmitVcode =
         "document.getElementById('J_PhoneCheckCode').value='{0}';".With(vcodeNum) +
         "document.getElementsByClassName('btn-s')[0].click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, jsOfSubmitVcode);//提交验证码


                break;



            }
            //     }).Start();
            //   };






        }



        /// <summary>
        /// 开通淘宝
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        private bool GetPhone(ref string phoneNum) {

            bool isok = false;
            int waitPhoneTime = int.Parse(ConfigHelper.GetValue("waitPhoneTime"));
            for (int i = 0; i < 5; i++) {
                if (ConfigHelper.GetBoolValue("tbModelTest")) {
                    phoneNum = GlobalVar.phoneNum;
                    phoneNum = GlobalVar.sms.GetPhone(phoneNum, SmsServer.tb_reg_vode); // SmsApi.GetPhone(serverid);
                } else {
                    phoneNum = GlobalVar.sms.GetPhone(SmsServer.tb_reg_vode); // SmsApi.GetPhone(serverid);
                }
               // phoneNum = GlobalVar.sms.GetPhone(SmsServer.tb_reg_vode);// SmsApi.GetPhone("2");//
                if (string.IsNullOrEmpty(phoneNum) || phoneNum == "未获取到号码") {
                    LogManager.WriteLog("未获取到号码,等待{0}秒钟".With(waitPhoneTime));
                    Thread.Sleep(waitPhoneTime * 1000);
                } else {
                    isok = true;
                    break;
                }
            }
            return isok;
        }
        /// <summary>
        /// 开通支付宝
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="serverid"></param>
        /// <returns></returns>
        private bool GetPhone(ref string phoneNum, SmsServer serverid) {

            bool isok = false;
            for (int i = 0; i < 5; i++) {
                if (ConfigHelper.GetBoolValue("tbModelTest")) {
                    phoneNum = GlobalVar.phoneNum;
                    phoneNum = GlobalVar.sms.GetPhone(phoneNum,serverid); // SmsApi.GetPhone(serverid);
                }
                else {
                    phoneNum = GlobalVar.sms.GetPhone(serverid); // SmsApi.GetPhone(serverid);
                }
              
                if (string.IsNullOrEmpty(phoneNum) || phoneNum == "未获取到号码") {
                    LogManager.WriteLog("未获取到号码,等待5秒钟");
                    Thread.Sleep(5000);
                } else {
                    isok = true;
                    break;
                }
            }
            return isok;
        }

        private void BT_Fill_Good_Address_Click(object sender, EventArgs e) {

        }

        #endregion
        #region ==========加密保|解绑手机===========

        //  https://memberprod.alipay.com/account/reg/emailCheck.htm

        private bool IsInitZfbSQloaded = false;
        private void InitZfbSQ() {
            if (IsInitZfbSQloaded) {
                return;
            }


            IsInitZfbSQloaded = true;
            // 导航至添加密保(选择添加密保的方式)
            Operation zfb_reg_queryStrategy = new Operation(MainCefFrame);
            zfb_reg_queryStrategy.index = 21;
            zfb_reg_queryStrategy.PerviousStatus = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion_before;
            zfb_reg_queryStrategy.CurrentUrl = "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-addSecurityQuestion-fullpage&scene_code=addSecurityQuestion";
            zfb_reg_queryStrategy.NextStatus = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion;
            zfb_reg_queryStrategy.OperationHandler += (s, e) => {
                MainCefFrame.LoadUrl(GlobalVar.zfb_reg_nav2set_SecurityQuestion);
            };

            //导航至支付宝密保
            Operation nav2set_SQ = new Operation(MainCefFrame);
            nav2set_SQ.index = 22;
            nav2set_SQ.PerviousStatus = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion;//
            nav2set_SQ.NextStatus = BusinessStatus.zfb_reg_nav2set_SecurityQuestion;
            nav2set_SQ.OperationHandler += BT_zfb_reg_nav2set_SecurityQuestion_Click;
            nav2set_SQ.CurrentUrl = "https://accounts.alipay.com/console/selectStrategy.htm?sp=1-addSecurityQuestion-fullpage&strategy=payment_password";// GlobalVar.zfb_reg_queryStrategy2;

            //填支付密码
            Operation add_SQ = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_nav2set_SecurityQuestion,
                NextStatus = BusinessStatus.zfb_reg_add_SecurityQuestion_Fill_PayPwd,
                CurrentUrl = "https://accounts.alipay.com/console/selectStrategy.htm?sp=1-addSecurityQuestion-fullpage&strategy=payment_password"
                ,
                index = 23
            };
            add_SQ.OperationHandler += BT_zfb_reg_add_SecurityQuestion_Fill_PayPwd_Click;

            //填写安保
            Operation setQa = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_add_SecurityQuestion_Fill_PayPwd,
                NextStatus = BusinessStatus.zfb_reg_setQa,
                CurrentUrl = GlobalVar.zfb_reg_setQa
                  ,
                index = 24
            };
            setQa.OperationHandler += BT_zfb_reg_add_SecurityQuestion_Click;

            Operation setQa_confirm = new Operation(MainCefFrame) {
                index = 25,
                PerviousStatus = BusinessStatus.zfb_reg_setQa,
                NextStatus = BusinessStatus.zfb_reg_add_SecurityQuestion_setQa_confirm,
                CurrentUrl = GlobalVar.zfb_reg_add_SecurityQuestion_setQa_confirm
            };
            setQa_confirm.OperationHandler += BT_zfb_reg_add_SecurityQuestion_setQa_confirm_Click;

            Operation add_sq_sucess = new Operation(MainCefFrame) {
                index = 26,
                PerviousStatus = BusinessStatus.zfb_reg_add_SecurityQuestion_setQa_confirm,
                NextStatus = BusinessStatus.zfb_reg_open_tb_before,
                CurrentUrl = "https://accounts.alipay.com/console/common/success.htm"
            };
            add_sq_sucess.OperationHandler += Add_SQ_SUCESS;
            //

            //=========解绑手机
            Operation nav2jiebang = new Operation(MainCefFrame) {
                index = 3.1,
                PerviousStatus = BusinessStatus.Zfb_reg_shifou_bangding_shouji_before,
                NextStatus = BusinessStatus.Zfb_reg_shifou_bangding_shouji,
                CurrentUrl = "https://my.alipay.com/portal/account/index.htm",
                note = "主页至解绑页"
            };
            nav2jiebang.OperationHandler += (s, e) => {
                var res = CefFrameHelper.GetMsgByJs2(MainCefFrame,
                     "document.getElementsByClassName('account-status-det')[2].getElementsByClassName('fn-left')[1].innerText","gb2312");
                LogManager.WriteLog(res);
                if (!string.IsNullOrEmpty(res) && res.Trim() == "未绑定") {//未绑定
                    // state= tb_ TODO:
                    BT_ZFB_REG_nav2addsq_Click(s, e);//导航至添加密保
                } else {
                    MainCefFrame.LoadUrl("https://accounts.alipay.com/console/dispatch.htm?scene_code=removeMobile&site=1&page_type=fullpage");
                }
            };

            Operation removeMobile0 = new Operation(MainCefFrame) {
                index = 3.2,
                PerviousStatus = BusinessStatus.Zfb_reg_shifou_bangding_shouji,
                NextStatus = BusinessStatus.Zfb_reg_removeMobile0,
                CurrentUrl = "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-removeMobile-fullpage&scene_code=removeMobile",
                note = "选择解绑方式1-身份证"
            };
            removeMobile0.OperationHandler += (s, e) => {
                MainCefFrame.LoadUrl("https://accounts.alipay.com/console/selectStrategy.htm?sp=1-removeMobile-fullpage&strategy=email-id_card_raw-payment_password");
            };

            //Operation removeMobile0 = new Operation(MainCefFrame) {
            //    index = 3.2,
            //    PerviousStatus = BusinessStatus.Zfb_reg_shifou_bangding_shouji,
            //    NextStatus = BusinessStatus.Zfb_reg_removeMobile0,
            //    CurrentUrl = "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-removeMobile-fullpage&scene_code=removeMobile",
            //    note = "选择解绑方式2-手机"
            //};
            //removeMobile0.OperationHandler += (s, e) => {
            //    MainCefFrame.LoadUrl("https://accounts.alipay.com/console/selectStrategy.htm?sp=1-removeMobile-fullpage&strategy=sms-payment_password");
            //};

            Operation removeMobile1 = new Operation(MainCefFrame) {
                index = 3.2,
                PerviousStatus = BusinessStatus.Zfb_reg_removeMobile0,
                NextStatus = BusinessStatus.Zfb_reg_removeMobile1,
                CurrentUrl = "https://accounts.alipay.com/console/selectStrategy.htm?sp=1-removeMobile-fullpage&strategy=email-id_card_raw-payment_password",
                note = "输入身份证信息和支付密码"
            };
            removeMobile1.OperationHandler += (s, e) => {
                Thread.Sleep(2000);
                CefFrameHelper.ExcuteJs(MainCefFrame, "document.getElementById('J-certNo').value='{0}';".With(currentHaoZi.zfbSFZ));

                new Thread(() => {
                    #region ---输入支付密码---
                    Thread.Sleep(1000);
                    MouseKeyBordHelper.POINT p1;
                    MouseKeyBordHelper.GetCursorPos(out p1);
                    LogManager.WriteLog("mouse position {0},{1}   ".With(p1.X, p1.Y));
                    LogManager.WriteLog("Form position {0},{1}   ".With(this.Left, this.Top));


                    //int chax = 582 - 110;
                    //int chay = 557 - 110;
                    int chax = ConfigHelper.GetIntValue("取消手机页面左边距");  // 600 - 61;
                    int chay = ConfigHelper.GetIntValue("取消手机页面上边距");  // 562 - 171;

                    Thread.Sleep(1000);
                    MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay);

                    Thread.Sleep(1000);
                    var s1 = currentHaoZi.zfbPayPwd;
                    LogManager.WriteLog(s1);
                    MouseKeyBordHelper.KeyBoardDo(s1.ToCharArray());
                    #endregion

                    //new Thread(() => {
                    //    Thread.Sleep(3000);
                    //CefFrameHelper.ExcuteJs(MainCefFrame,
                    //    "document.getElementById('J-selectStrategyForm').submit();");
                    LogManager.WriteLog("mouse {0},{1}   ".With(p1.X, p1.Y));
                    MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay + ConfigHelper.GetIntValue("取消手机页面待移动距离"));
                    //  }).Start();
                }).Start();


            };

            #region MyRegion
            //Operation removeMobile1 = new Operation(MainCefFrame) {
            //    index = 3.2,
            //    PerviousStatus = BusinessStatus.Zfb_reg_removeMobile0,
            //    NextStatus = BusinessStatus.Zfb_reg_removeMobile1,
            //    CurrentUrl = "https://accounts.alipay.com/console/selectStrategy.htm?sp=1-removeMobile-fullpage&strategy=email-id_card_raw-payment_password",
            //    note = "输入手机验证码和支付密码"
            //};
            //removeMobile1.OperationHandler += (s, e) => {
            //    #region MyRegion
            //    Thread.Sleep(2000);
            //    string js1 = "document.getElementsByClassName('ui-button-text')[0].click();";
            //    CefFrameHelper.ExcuteJs(MainCefFrame,
            //        js1);

            //    new Thread(() => {
            //        #region ---输入支付密码---
            //        Thread.Sleep(1000);
            //        MouseKeyBordHelper.POINT p1;
            //        MouseKeyBordHelper.GetCursorPos(out p1);
            //        LogManager.WriteLog("mouse position {0},{1}   ".With(p1.X, p1.Y));
            //        LogManager.WriteLog("Form position {0},{1}   ".With(this.Left, this.Top));

            //        int chax = 367;
            //        int chay = 403;
            //        Thread.Sleep(1000);
            //        MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay);

            //        Thread.Sleep(1000);
            //        var s1 = currentHaoZi.zfbPayPwd;
            //        LogManager.WriteLog(s1);
            //        MouseKeyBordHelper.KeyBoardDo(s1.ToCharArray());
            //        #endregion

            //        //收验证码
            //        LogManager.WriteLog("之前的号码是 {0} 继续获取".With(GlobalVar.phoneNum));

            //        string vcodeNum = "";
            //        bool isVcodeOk = SmsConfigHelper.GetSmsOfPhone(GlobalVar.phoneNum, ref vcodeNum, SmsServer.zfb_jiebang);

            //        string js2 = "document.getElementById('J-inputCode').value='{0}';".With(vcodeNum);
            //        CefFrameHelper.ExcuteJs(MainCefFrame, js2);


            //        //new Thread(() => {
            //        //    Thread.Sleep(3000);
            //        //CefFrameHelper.ExcuteJs(MainCefFrame,
            //        //    "document.getElementById('J-inputCode').click();");
            //        //    "document.getElementById('J-selectStrategyForm').submit();");  
            //        LogManager.WriteLog("mouse {0},{1}   ".With(p1.X, p1.Y));
            //        MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay + 65);
            //        //  }).Start();
            //    }).Start(); 
            //    #endregion
            //}; 
            #endregion

            Operation removeMobile2 = new Operation(MainCefFrame) {
                index = 3.3,
                PerviousStatus = BusinessStatus.Zfb_reg_removeMobile1,
                NextStatus = BusinessStatus.Zfb_reg_removeMobile2,
                CurrentUrl = "https://accounts.alipay.com/console/common/email/sendSuccess.htm?emailType=securityCheckEmail&emailLogonId=",
                note = "去邮件完成验证"
            };
            removeMobile2.OperationHandler += (s, e) => {
                //#3.3
                var url = EmailCheck("请完成邮箱验证", "立即解除手机绑定");
                if (!string.IsNullOrEmpty(url)) {
                    MainCefFrame.LoadUrl(url);
                }
            };
            Operation removeMobile3 = new Operation(MainCefFrame) {
                index = 3.4,
                PerviousStatus = BusinessStatus.Zfb_reg_removeMobile2,
                NextStatus = BusinessStatus.Zfb_reg_removeMobile3,
                CurrentUrl = "https://accounts.alipay.com/console/emailSecurityConfirm.htm?activeKey=",
                note = "点击链接,继续解绑手机"
            };
            removeMobile3.OperationHandler += (s, e) => {

                var js = "window.location=window.frames[0].document.getElementsByClassName('linklist')[0].getElementsByTagName('a')[0].href;";
                CefFrameHelper.ExcuteJs(MainCefFrame, js);

            };
            Operation removeMobile4 = new Operation(MainCefFrame) {
                index = 3.5,
                PerviousStatus = BusinessStatus.Zfb_reg_removeMobile3,
                NextStatus = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion_before,
                CurrentUrl = "https://accounts.alipay.com/console/common/success.htm?sp=1-removeMobile-fullpage&isCancelCard=false&sp=1-removeMobile-fullpage&site=1&scene_code=removeMobile&page_type=fullpage",
                note = "解绑成功，继续添加密保"

            };
            removeMobile4.OperationHandler += (s, e) => {

                //  var    CurrentUrl = "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-addSecurityQuestion-fullpage&scene_code=addSecurityQuestion";

                BT_ZFB_REG_nav2addsq_Click(s, e);
                //state = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion_before;
                //MainCefFrame.LoadUrl(
                //    "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-addSecurityQuestion-fullpage&scene_code=addSecurityQuestion"

                //    );
                //CurrentUrl = "https://my.alipay.com/portal/account/index.htm",
            };
            //
            Operation taobaoopen = new Operation(MainCefFrame) {
                index = 27,
                PerviousStatus = BusinessStatus.zfb_reg_open_tb_before,
                NextStatus = BusinessStatus.zfb_reg_taobao_new_alipay_q,
                CurrentUrl = "https://my.alipay.com/portal/account/index.htm",
                note = "开通淘宝"
            };

            // BusinessStatus.zfb_reg_taobao_new_alipay_q;
            taobaoopen.OperationHandler += BT_ZFB_REG_TB_open_Click;

            //zfb注册 手机验证


            opsList.Add(nav2jiebang);
            opsList.Add(removeMobile0);
            opsList.Add(removeMobile1);
            opsList.Add(removeMobile2);
            opsList.Add(removeMobile3);
            opsList.Add(removeMobile4);

            //=================

            opsList.Add(zfb_reg_queryStrategy);
            opsList.Add(nav2set_SQ);
            opsList.Add(add_SQ);
            opsList.Add(setQa);
            opsList.Add(setQa_confirm);
            opsList.Add(add_sq_sucess);
            opsList.Add(taobaoopen);
            //
            //添加收货地址

        }
        //1 导航至添加密保
        private void BT_ZFB_REG_nav2addsq_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion_before;
            MainCefFrame.LoadUrl(GlobalVar.zfb_reg_nav2set_addSecurityQuestion);
        }

        //2 选择支付宝密保
        private void BT_zfb_reg_nav2set_SecurityQuestion_Click(object sender, EventArgs e) {

            MainCefFrame.LoadUrl(GlobalVar.zfb_reg_nav2set_SecurityQuestion);
        }

        //3   设置支付宝密保-填支付密码
        private void BT_zfb_reg_add_SecurityQuestion_Fill_PayPwd_Click(object sender, EventArgs e) {
            state = BusinessStatus.zfb_reg_add_SecurityQuestion_Fill_PayPwd;

            int chax = ConfigHelper.GetIntValue("添加密保页面左边距");  // 600 - 61;
            int chay = ConfigHelper.GetIntValue("添加密保页面上边距");  // 562 - 171;

            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay);
            MouseKeyBordHelper.KeyBoardDo(currentHaoZi.zfbPayPwd.ToCharArray());
            MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay + ConfigHelper.GetIntValue("添加密保页面待移动距离"));

            //string js2run = "document.getElementById('payPassword_rsainput').value = '{0}';" +
            //                "document.getElementsByClassName('ui-button-text')[0].click();"
            //    .With(currentHaoZi.zfbPayPwd);
            //CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
        }

        //4   设置支付宝密保-填安保问题
        private void BT_zfb_reg_add_SecurityQuestion_Click(object sender, EventArgs e) {

            string js2run = "" +
            "document.getElementById('J-question11').selectedIndex = {0};  ".With(currentHaoZi.mb_1_1) +
         "document.getElementById('J-question12').selectedIndex = {0};     ".With(currentHaoZi.mb_1_2) +
         "document.getElementById('J-question21').selectedIndex = {0};     ".With(currentHaoZi.mb_2_1) +
         "document.getElementById('J-question22').selectedIndex = {0};     ".With(currentHaoZi.mb_2_2) +
         "document.getElementById('J-question31').selectedIndex = {0};     ".With(currentHaoZi.mb_3_1) +
         "document.getElementById('J-question32').selectedIndex = {0};     ".With(currentHaoZi.mb_3_2) +
         "document.getElementById('J-answer1').value = '{0}';           ".With(currentHaoZi.mb_1_3) +
         "document.getElementById('J-answer2').value = '{0}';           ".With(currentHaoZi.mb_2_3) +
         "document.getElementById('J-answer3').value = '{0}';           ".With(currentHaoZi.mb_3_3) +
         "document.getElementsByClassName('ui-button-text')[0].click(); ";
            CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
        }

        //5   设置支付宝密保-确认
        private void BT_zfb_reg_add_SecurityQuestion_setQa_confirm_Click(object sender, EventArgs e) {

            string js2run = "document.getElementsByClassName('ui-button-text')[0].click();";

            CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
        }



        public string EmailCheck(string title, string linktext) {
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
                if (iserror && messages.Count==0) {
                    MainForm.taskRunner.LogAccontStatus("|Error_Email");
                    LogManager.WriteLog("邮箱账户有误");
                    break;
                }
                string subject = title;//"请激活您的支付宝账户";
                htmlbody = emailHelper.FindHtmlOfSubject(messages, subject);

                if (string.IsNullOrEmpty(htmlbody)) {
                    LogManager.WriteLog("暂未收到邮件，5秒后重试，第{0}次".With(5 - i));
                } else {
                    LogManager.WriteLog("找到邮件 ");
                    isFindHtml = true;
                    break;
                }

            }
            string innertext = linktext;// "继续注册";
            string activZfbLink1 = "";
            if (isFindHtml) {
                activZfbLink1 = emailHelper.FindLink(innertext, htmlbody);
                //MainForm.currentHaoZi.activeLink = activZfbLink1;
                // _mfForm.BT_Link_Continue_Reg_Click(sender, e1);
                LogManager.WriteLog(activZfbLink1);
            } else {
                LogManager.WriteLog("未找到包含该文字的邮件 未收到邮件");
            }


            return activZfbLink1;
        }


        #endregion

    }
}
