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

        private bool IsInit_tb_loaded = false;

        private void Init_tb_reg() {
            if (IsInit_tb_loaded) {
                return;
            }
            IsInit_tb_loaded = true;

            //填会员信息
            Operation new_register = new Operation(MainCefFrame) {
                CurrentUrl = "http://reg.taobao.com/member/new_register.jhtml",
                index = 1,
                PerviousStatus = BusinessStatus.new_register,
                NextStatus = BusinessStatus.new_cellphone_reg_two,
            };
            new_register.OperationHandler += (s, e) => {


                #region ==========填会员信息==========

                //获取ID=J_CheckCode 的data-imgurl属性的值url
                //识别url中的图片
                new Thread(() => {
                    string srcs = "";
                    for (int i = 0; i < 3; i++) {
                        srcs = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J_CheckCode", "div", "data-imgurl").FirstOrDefault();
                        if (!string.IsNullOrEmpty(srcs)) {
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(srcs)) {
                        LogManager.WriteLog("重复3次后仍然无法获取图片。");
                        return;
                    }

                    srcs = srcs.Replace("&amp;", "&");
                    string returnMess;
                    var isVCodeOk = Vcode.GetVcodeFormImageUrl(srcs, out returnMess);


                    Console.WriteLine(isVCodeOk);
                    //
                    string js2run = "document.getElementById('J_Nick').value='{0}';".With(currentHaoZi.tbName) +
                                    "document.getElementById('J_Pwd').value='{0}';".With(currentHaoZi.tbPwd) +
                                    "document.getElementById('J_RePwd').value='{0}';".With(currentHaoZi.tbPwd) +
                                    "document.getElementById('J_Code').value='{0}';".With(returnMess) +
                                    "document.getElementById('J_BtnBasicInfoForm').click();";

                    CefFrameHelper.ExcuteJs(MainCefFrame, js2run);


                }).Start();
                #endregion
            };

            //验证账户信息-选择方式（手机或邮箱）
            Operation new_cellphone_reg_two = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.new_cellphone_reg_two,
                NextStatus = BusinessStatus.new_email_reg_two,
                CurrentUrl = "http://reg.taobao.com/member/new_cellphone_reg_two.jhtml",
                index = 2
            };
            new_cellphone_reg_two.OperationHandler += (s, e) => {
                //  BeginInvoke(new Action(() => {
                FindTitleAndLoadHref("使用邮箱验证");
                //   }));

            };

            //验证账户信息-选择方式（手机或邮箱）-输入邮箱手机打码
            Operation new_email_reg_three = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.new_email_reg_two,
                NextStatus = BusinessStatus.new_email_reg_three,
                CurrentUrl = "http://reg.taobao.com/member/new_email_reg_two.jhtml",
                index = 3

            };
            new_email_reg_three.OperationHandler += (s, e) => {
                string js2runS1 = "document.getElementById('J_Email').value='{0}';".With(currentHaoZi.zfbEmail) +
                                      "document.getElementsByClassName('btn-b tsl')[0].click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2runS1);
                this.bfForms.BT_TB_ChangeTel_Click(s, e);


            };

            //验证账户信息-选择方式（手机或邮箱）-获取激活链接
            Operation register_confirm = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.new_email_reg_three,
                NextStatus = BusinessStatus.regitster_confirm,
                CurrentUrl = "http://reg.taobao.com/member/new_email_reg_three.jhtml",
                index = 4
            };
            register_confirm.OperationHandler += BT_FindActiveUrl_Click;

            //验证用户信息-完成
            Operation account_management = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.regitster_confirm,
                NextStatus = BusinessStatus.account_management,
                CurrentUrl = "http://reg.taobao.com/member/register_confirm.jhtml",
                //https://lab.alipay.com/user/reg/complete/completeTaobao.htm
                index = 5

            };
            account_management.OperationHandler += (s, e) => {
                MainCefFrame.LoadUrl("http://member1.taobao.com/member/fresh/account_management.htm");
            };

            Operation completeTaobao = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.account_management,
                NextStatus = BusinessStatus.paymethod,
                CurrentUrl = "http://member1.taobao.com/member/fresh/account_management.htm",
                index = 6
            };
            completeTaobao.OperationHandler += (s, e) => {
                FindTitleAndLoadHref("立即补全");
            };

            Operation paymethod = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.paymethod,
                NextStatus = BusinessStatus.zfb_Reg_paymethod,
                index = 7,
                CurrentUrl = "https://memberprod.alipay.com/account/reg/complete/complete.htm?scene=havanaComplete"
            };
            paymethod.OperationHandler += BT_TB_FILLZFB_Click;

            //===================支付宝注册流程

            Operation Zfb_Fill_Account = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_begin,
                NextStatus = BusinessStatus.emailCheck,
                CurrentUrl = GlobalVar.zfb_Reg_enter_1,
                index = 1, Deadline = 1000 * 60 * 5
            };
            Zfb_Fill_Account.OperationHandler += BT_Zfb_Fill_Account_Click;


            Operation emailCheck = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.emailCheck,
                NextStatus = BusinessStatus.zfb_reg_link_complete,
                CurrentUrl = "https://memberprod.alipay.com/account/reg/emailCheck.htm",
                index = 1.1
            };
            emailCheck.OperationHandler += (s, e) => {
                BT_Zfb_Reg_Sumbit_s1_Click(this, new EventArgs());

            };

            Operation zfb_reg_link_complete = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_link_complete,
                NextStatus = BusinessStatus.zfb_Reg_paymethod,
                CurrentUrl = GlobalVar.zfb_Reg_linkcomplete,
                index = 2
            };
            zfb_reg_link_complete.OperationHandler += BT_Zfb_reg_fill_Click;

            Operation zfb_Reg_paymethod = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_Reg_paymethod,
                NextStatus = BusinessStatus.zfb_reg_account_reg_success,
                CurrentUrl = GlobalVar.zfb_Reg_paymethod,
                index = 3
            };
            zfb_Reg_paymethod.OperationHandler += BT_ZFB_TB_Reg_Click;


            Operation zfb_reg_account_reg_success = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_account_reg_success,
                NextStatus = BusinessStatus.zfb_reg_account_reg_success,
                CurrentUrl = GlobalVar.zfb_reg_account_reg_success,
                index = 4,
                note = "  导航至添加密保，跳到 开通淘宝"
            };
            //zfb_reg_account_reg_success.OperationHandler += BT_ZFB_REG_nav2addsq_Click;
            // BT_ZFB_REG_Account_Sucess_Click
            zfb_reg_account_reg_success.OperationHandler += BT_ZFB_REG_Account_Sucess_Click;

            Operation zfb_reg_skip_bindassetcard = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_account_reg_success,
                //NextStatus = BusinessStatus.zfb_reg_taobao_open,//换
                NextStatus = BusinessStatus.zfb_reg_nav2set_add_SecurityQuestion_before,
                CurrentUrl = GlobalVar.zfb_reg_skip_bindassetcard,
                index = 5
            };
            zfb_reg_skip_bindassetcard.OperationHandler += BT_ZFB_REG_TB_open_Click;

            Operation zfb_reg_taobao_new_alipay_q = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.zfb_reg_taobao_new_alipay_q,
                NextStatus = BusinessStatus.nav2deliver_address,
                CurrentUrl = GlobalVar.zfb_reg_taobao_new_alipay_q,
                index = 31,
                note = "开通tb",
                Deadline = int.Parse(ConfigHelper.GetValue("zfb_reg_taobao_new_alipay_RegDeadline")) * 60 * 1000
            };

            zfb_reg_taobao_new_alipay_q.OperationHandler += BT_ZFB_REG_TB_new_alipay_q_Click;

            Operation nav2deliver_address = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.nav2deliver_address,
                NextStatus = BusinessStatus.deliver_address,
                CurrentUrl = "http://reg.taobao.com/member/register_confirm.jhtml",
                index = 32,
                note = "注册完成-跳转->添加收货地址的页面"

            };
            nav2deliver_address.OperationHandler += (s, e) => {
                MainCefFrame.LoadUrl(
                    "http://member1.taobao.com/member/fresh/deliver_address.htm");
            };


            Operation add_deliver_address = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.deliver_address,
                NextStatus = BusinessStatus.ready,
                CurrentUrl = "http://member1.taobao.com/member/fresh/deliver_address.htm",
                index = 33,
                note = "添加收货地址",
            };
            add_deliver_address.Deadline = GlobalVar.DeadLineOfChangeClearAndPrepareNext;
            add_deliver_address.OperationHandler += (s, e) => {

                var js2run = RandomAddressPlace();
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);

                new Thread(() => {
                    currentHaoZi.regStatus = DateTime.Now.ToString();
                    currentHaoZi.tbStatus = GlobalVar.CurrentIp;
                    HaoziHelper.UpdateLoadHaozi(ConfigHelper.GetValue("WebSite"), currentHaoZi, "add");
                    //        Thread.Sleep(6000);
                    LogManager.WriteLog("开始下一个");

                    ClearAndPrepareNext();
                }).Start();
            };

            this.opsList.Add(Zfb_Fill_Account);
            this.opsList.Add(emailCheck);
            this.opsList.Add(zfb_reg_link_complete);
            this.opsList.Add(zfb_Reg_paymethod);
            this.opsList.Add(zfb_reg_account_reg_success);
            this.opsList.Add(zfb_reg_skip_bindassetcard);
            this.opsList.Add(zfb_reg_taobao_new_alipay_q);

            this.opsList.Add(nav2deliver_address);
            this.opsList.Add(add_deliver_address);


            this.opsList.Add(new_register);
            this.opsList.Add(new_cellphone_reg_two);
            this.opsList.Add(new_email_reg_three);
            this.opsList.Add(register_confirm);
            this.opsList.Add(account_management);
            this.opsList.Add(completeTaobao);
            this.opsList.Add(paymethod);




        }
        [Test]
        public string RandomAddressPlace() {
            string address = RandomManager.RandomReadOneLine("config//address.txt");
            string J_City = "110100";
            string J_Area = "110101";
            string J_Town = "110101001";
            string J_PostCode = "100050";
            var ads = address.Split('|');
            try {
                J_City = ads[0];
                J_Area = ads[1];
                J_Town = ads[2];
                J_PostCode = ads[3];
            } catch (Exception e1) {

                LogManager.WriteLog(e1.ToString());
            }

            string J_Street = RandomManager.ProduceRandomStr(4, 7, RandomManager.hanzi) + "路" + RandomManager.ProduceRandomStr(1, 3, RandomManager.Zero2Nine.ToArray()) + "号"; //5744 4440;
            string J_Name = RandomManager.ProduceRandomStr(2, 3, RandomManager.hanzi) + currentHaoZi.realname;
            string J_Mobile = "138" + RandomManager.ProduceRandomStr(8, 8, RandomManager.Zero2Nine.ToArray()); //5744 4440
            string js2run = "var J_DeliverIframeFrame=window.frames['J_DeliverIframe'].document;" +
                            "J_DeliverIframeFrame.getElementById('J_Province').selectedIndex=1;" +
                            "J_DeliverIframeFrame.getElementById('J_City').add(new Option('北京1市','{0}'));".With(J_City) +
                            "J_DeliverIframeFrame.getElementById('J_City').selectedIndex=1;" +
                            "J_DeliverIframeFrame.getElementById('J_Area').add(new Option('东城2区','{0}'));".With(J_Area) +
                            "J_DeliverIframeFrame.getElementById('J_Area').selectedIndex=1;" +
                            "J_DeliverIframeFrame.getElementById('J_Town').add(new Option('东华门3街道','{0}'));".With(J_Town) +
                            "J_DeliverIframeFrame.getElementById('J_Town').selectedIndex=1;" +
                            "J_DeliverIframeFrame.getElementById('J_PostCode').value='{0}';".With(J_PostCode) +
                            "J_DeliverIframeFrame.getElementById('J_Street').value='{0}';".With(J_Street) +
                            "J_DeliverIframeFrame.getElementById('J_Name').value='{0}';".With(J_Name) +
                            "J_DeliverIframeFrame.getElementById('J_Mobile').value='{0}';".With(J_Mobile) +
                            "J_DeliverIframeFrame.getElementById('J_SetDefault').checked=true;" +
                            "createOrUpdate();window.setTimeout(function(){ window.frames['J_DeliverIframe'].document.getElementsByClassName('xdialog-btn xdialog-btn-ok')[0].click();},2000);";
            Console.Write(js2run);
            return js2run;
        }



        public EventHandler GetMobilePhoneTbHandler;

        private void FindTitleAndLoadHref(string linkTitle) {

            Application.DoEvents();
            for (int i = 0; i < 3; i++) {
                var linkList = CefFrameHelper.GetUrlListByHap(MainCefFrame, linkTitle);
                if (linkList.Count > 0) {
                    MainCefFrame.LoadUrl(linkList[0]);
                    break;
                } else {
                    LB_Debug_info.Text = linkTitle;
                    LogManager.WriteLog("没有找到 {0}".With(linkTitle));
                    Application.DoEvents();
                }

            }

        }

        private string FindTitleHref(string linkTitle) {

            Application.DoEvents();
            for (int i = 0; i < 3; i++) {
                var linkList = CefFrameHelper.GetUrlListByHap(MainCefFrame, linkTitle);
                if (linkList.Count > 0) {
                    MainCefFrame.LoadUrl(linkList[0]);
                    return linkList[0];
                } else {
                    LB_Debug_info.Text = linkTitle;
                    LogManager.WriteLog("没有找到 {0}".With(linkTitle));
                    Application.DoEvents();
                }

            }
            return "";
        }

        #region =======通过邮箱找激活的链接========

        private void BT_FindActiveUrl_Click(object sender, EventArgs e1) {
            string htmlbody = "";
            ShowStatus("收取邮件中，稍等");
            Application.DoEvents();

            for (int i = 0; i < 5; i++) {
                Thread.Sleep(5000);
                List<string> titles = new List<string>();
                titles.Add("新用户确认通知信");
                titles.Add("请激活您的支付宝账户");
                htmlbody = GetMailFromZfb(titles);
                if (string.IsNullOrEmpty(htmlbody)) {
                    ShowStatus("暂未收到邮件，5秒后重试，第{0}次".With(i));
                    Application.DoEvents();

                } else {
                    break;
                }

            }

            //ap 找到激活码

            string innertext = "register_confirm";
            string activZfbLink1 = mcClient.GetHrefFormHtml(htmlbody, innertext);
            if (activZfbLink1.Contains(innertext)) {
                //  throw new Exception("未能从邮件中找到链接！");
                //GlobalVar.activZfbLink = activZfbLink1;
                //继续注册
                // BT_Link_Continue_Reg_Click(sender, e1);
                MainCefFrame.LoadUrl(activZfbLink1);

            } else {
                activZfbLink1 = "没找到链接哎。。囧了。别急";

            }
            ShowStatus(activZfbLink1);

        }
        // private Pop3Client pop3Client = new Pop3Client();
        MailClient mcClient = new MailClient();
        public string GetMailFromZfb(List<string> titles) {

            string file = Application.StartupPath + "//import//popserver.txt";
            if (File.Exists(file)) {
                using (StreamReader reader = new StreamReader(File.OpenRead(file))) {
                    // This describes how the OpenPOPLogin.txt file should look like
                    mcClient.popServer = reader.ReadLine(); //  
                    mcClient.port = reader.ReadLine(); // Port
                    mcClient.ssl = bool.Parse(reader.ReadLine() ?? "true"); // Whether to use SSL or not
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

        public void TB_Reg_TB2_Click(object sender, EventArgs e) {
            state = BusinessStatus.new_register;
            MainCefFrame.LoadUrl("http://reg.taobao.com/member/new_register.jhtml");
            //
        }

        private void BT_TB_FILLZFB_Click(object sender, EventArgs e) {
            //new Thread(() => {
            //    Thread.Sleep(2000);
            string js2run = "document.getElementById('payPwd').value='{0}';".With(currentHaoZi.zfbPayPwd) +
                               "document.getElementById('payPwdConfirm').value='{0}';".With(currentHaoZi.zfbPayPwd) +
                               "document.getElementById('realName').value='{0}';".With(currentHaoZi.realname) +
                               "document.getElementById('IDCardNo').value='{0}';".With(currentHaoZi.zfbSFZ) +
                               "document.getElementById('J-complete-form').submit();";
            CefFrameHelper.ExcuteJs(MainCefFrame, js2run);


            //}).Start();

        }

    }

}