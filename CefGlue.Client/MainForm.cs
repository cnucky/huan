using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Common.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using PwBusiness.Bll;
using PwBusiness.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;
using PwBusiness;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {


    public partial class MainForm : Form {
        private readonly string _mainTitle;

        public MainForm() {
            InitializeComponent();

            _mainTitle = Text;

            NewTab(ConfigHelper.GetValue("StartUrl"));

            //NewTab("https://login.taobao.com/member/login.jhtml");


        }

        public MainForm(bool _autoStart) {
            InitializeComponent();

            _mainTitle = Text;
            this.autoStart = _autoStart;
            NewTab(ConfigHelper.GetValue("StartUrl"));
            // getMainframe = MainCefFrame;
        }

        public List<tb_tbzfb> tbzfbs {
            get { return GlobalVar.AccountList; }
            set { GlobalVar.AccountList = value; }
        }


        public tbModel tbmode = tbModel.shop;

        public int haoziindex = 0;
        public string downFileName = "";
        private bool isautoRun = false;
        private void NewTab(string startUrl) {
            var page = new TabPage("New Tab");
            page.Padding = new Padding(0, 0, 0, 0);

            var browser = new CefWebBrowser();
            browser.StartUrl = startUrl;
            browser.Dock = DockStyle.Fill;
            browser.TitleChanged += (s, e) => {
                BeginInvoke(new Action(() => {
                    var title = browser.Title;
                    if (tabControl.SelectedTab == page) {
                        borrowTitle = browser.Title;
                        Text = browser.Title + " - " + _mainTitle;
                    }
                    page.ToolTipText = title;
                    if (title.Length > 18) {
                        title = title.Substring(0, 18) + "...";
                    }
                    page.Text = title;
                }));
            };
            browser.AddressChanged += (s, e) => {
                BeginInvoke(new Action(() => {
                    addressTextBox.Text = browser.Address;
                }));
            };
            browser.StatusMessage += (s, e) => {
                BeginInvoke(new Action(() => {
                    //    statusLabel.Text = e.Value;
                }));

            };


            browser.LoadEnd += (s, e) => {
                BeginInvoke(new Action(() => {
                    statusLabel.Text = "status:" + e.HttpStatusCode.ToString();

                    //InitZfbSQ();
                    //Init_tb_reg();
                    //tbV3_Init_reg();
                    //IsInit_wapshop();

                    // OnPageLoaded_TB_Shopping(s, e);
                    //   OnPageLoaded_163_Reg(s, e);
                    //  OnPageLoaded_zfb_Reg(s, e);
                    //  OnPageLoaded_pp_Ads(s, e);
                    //
                    OnPageLoaded_JiShi();
                    //   InitAuto();//装载自动的相关步骤
                }));

            };
            //在此注册
            //弹出
            //  browser.BeforePopup += browser_BeforePopup;
            browser.BeforePopup += (s, e) => {
                BeginInvoke(new Action(() => {
                    //if (Common.FileHelper1.read("//config//chromeconfig.txt")[0] == "true") {
                    //    e.Handled = true;
                    //}
                    if (e.TargetUrl != "http://mobile.alipay.com/index.htm") {
                        browser.Browser.GetMainFrame().LoadUrl(e.TargetUrl);
                    }


                    //e.Frame.LoadUrl(e.TargetUrl);


                }));
            };

            //
            page.Controls.Add(browser);

            tabControl.TabPages.Add(page);

            tabControl.SelectedTab = page;





        }


        /// <summary>
        /// 
        /// </summary>
        private void InitAuto() {
            if (isautoRun) {
                return;
            }
            isautoRun = true;

            Application.DoEvents();
            var t1 = new Thread(() => BeginInvoke(new Action(() => {
                if (!autoStart)
                    return;
                //    MessageBox.Show("autoStart");
                if (bfForms == null) {
                    bfForms = new BusinessForms(this);
                    bfForms.Show();
                }
                Application.DoEvents();

                //导入数据
                //bfForms.button1_Click(new object(), new EventArgs());

                LogManager.SetUI(bfForms.TB_logText);


                ClearAndPrepareNext();
            })));
            t1.IsBackground = true;
            t1.Start();


        }

        public void ClearAndPrepareNext() {
            ovtimeTick.Stop();
            try {
                if (haoziindex >= tbzfbs.Count) {
                    bool isok = taskRunner.getNewTask();
                    CefRuntime.Shutdown();
                    if (!isok) {
                        LogManager.WriteLog(" 待注册账号用完 ");
                        Thread.Sleep(1000 * 60 * 2);
                        AppHelper.ExisttApp();
                    }
                    AppHelper.RestartApp("restart.bat");
                    return;
                }
                string oldZfbEmail = currentHaoZi.zfbEmail;
                currentHaoZi = tbzfbs[haoziindex];

                taskRunner.currentAccount = currentHaoZi;
                //1 清理cookie
                //LogManager.WriteLog("clear cookies");
                //CefCookieManager.Global.VisitAllCookies(new CefCookieVisitorImp());
                //1.1 处理cookie
                // string newZfbEmail = currentHaoZi.zfbEmail;
                // ProcessCookies(oldZfbEmail, newZfbEmail);

                //1.5 清理缓存
                //
                int restartintervalTimes = int.Parse(ConfigHelper.GetValue("RestartIntervalTimes"));
                if (haoziindex % restartintervalTimes == restartintervalTimes - 1) {
                    AppHelper.RestartApp("restart.bat");
                }


                //   AppHelper.ClearCache();

                //2 等待间隔时间
                LogManager.WriteLog("RegIntervalSeconds {0}秒".With(GlobalVar.RegIntervalSeconds));
                Thread.Sleep(GlobalVar.RegIntervalSeconds * 1000);

                //  GlobalCefGlue.UserAgent = FileHelper.RandomReadOneLine("Resources//httpHead.txt").ReplaceNum();
                //  LogManager.WriteLog(GlobalCefGlue.UserAgent);

                //3 换IP
                string changeIpModel = "";
                bool changeIpok = false;
                if (GlobalVar.ChangeIp && haoziindex % GlobalVar.ChangeIpNum == GlobalVar.ChangeIpNum - 1) {
                    //  AdslHelper.ReConn2();
                    LogManager.WriteLog(" 开始换IP  ");
                    changeIpok = changeIp();
                    LogManager.WriteLog(" 结束换IP ");
                }
                //5 记录开始执行任务
                taskRunner.LogTaskBegin();
                //if (taskRunner.GetBeginHandler() != null) {
                //    taskRunner.GetBeginHandler().Invoke(this, new EventArgs());
                //} else {
                //    LogManager.WriteLog("taskRunner.BeginHandler");
                //}
            } catch (Exception e1) {
                LogManager.WriteLog(e1.ToString());
                CefRuntime.Shutdown();
                AppHelper.RestartApp("restart.bat");
            }


            var op = new Operation(MainCefFrame);

            op.OperationHandler += (s, e) => {
                //开始操作
                if (GlobalVar.tbmode == tbModel.shopv2) {
                    BT_TB_SHOPV2_Click(this, new EventArgs());
                } else if (GlobalVar.tbmode == tbModel.reg_zfb) {
                    BT_Reg_zfb_Click(this, new EventArgs());
                } else if (GlobalVar.tbmode == tbModel.reg_tbV3) {

                    lead2tbV3reg();
                } else {
                    LogManager.WriteLog("该模式目前不支持。");
                }
            };
            // op.Deadline = GlobalVar.DeadLineOfChangeClearAndPrepareNext;
            ovtimeTick.Next(op, null);//计时器重新计时
            try {
                //开始操作
                op.OperationHandler.Invoke(this, null);

            } catch (Exception e1) {
                LogManager.WriteLog(e1.ToString());
                AppHelper.RestartApp("restart.bat");
            }

            haoziindex++;
        }

        private static bool changeIp() {
            string changeIp = "";
            bool changeIpok = false;
            string changeIpModel;
            var rea = new RasApi();
            changeIpModel = ConfigHelper.GetValue("changeIpModel");
            int index = 0;
            int trytime = int.Parse(ConfigHelper.GetValue("ChangeIpTryTime"));
            while (!changeIpok && index < trytime) {
                if (changeIpModel == "reconn") {
                    AdslHelper.ReConn2();
                } else if (changeIpModel == "account") {
                    LogManager.WriteLog(" 账号模式切换 ");
                    changeIpok = rea.ReConn(out changeIp);
                    LogManager.WriteLog(" 账号模式切换 结束 ");
                    GlobalVar.CurrentIp = changeIp;
                } else {
                    changeIpok = rea.ReConn(out changeIp);
                    if (!changeIpok) {
                        AdslHelper.ReConn2();
                        //    taskRunner.ReportAccountStatus("changeip", "changeipfailed", RasApi.GetIP("宽带连接"));
                    }
                }

                if (!changeIpok) {
                    LogManager.WriteLog("切换IP失败，等待5分钟再试。第{0}次，共{1}次".With(index, trytime));
                    Thread.Sleep(1000 * 60 * 5);
                } else {
                    GlobalVar.CurrentIp = changeIp;
                    taskRunner.LogAccontStatus("|" + changeIp);
                }
                index++;
            }
            return changeIpok;
        }

        private void ProcessCookies(string oldzfbemail, string newzfbemail) {
            if (oldzfbemail == newzfbemail) {
                return;
            }
            string newfilePath = "cookies//" + newzfbemail;
            string oldfilePath = "cookies//" + oldzfbemail;



            LogManager.WriteLog("save cookies");
            //保存cookies
            var cookieVisitorSave = new CefCookieVisitorSave(true);

            CefCookieManager.Global.VisitAllCookies(cookieVisitorSave);
            LogManager.WriteLog("等待5秒");
            GlobalVar.IscookiesAutoResetEvent.WaitOne(TimeSpan.FromSeconds(5));
            LogManager.WriteLog("阻塞");
            GlobalVar.IscookiesAutoResetEvent.Reset();
            //保存老账号的cookie
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var coo in cookieVisitorSave.CefCookies) {
                stringBuilder.Append(JsonConvert.SerializeObject(coo) + Environment.NewLine);
            }
            LogManager.Write2File(oldfilePath, stringBuilder.ToString());
            //  cookieVisitorSave.CefCookies

            //1 检查新账号有无cookie记录
            bool fileExist = File.Exists(newfilePath);
            if (fileExist) {
                //2 装载其cookie记录
                var cookiesLines = File.ReadAllLines(newfilePath);
                List<CefCookie> cefCookies = new List<CefCookie>();
                foreach (var cookiesLine in cookiesLines) {
                    if (string.IsNullOrEmpty(cookiesLine)) {
                        continue;
                    }
                    cefCookies.Add(JsonConvert.DeserializeObject<CefCookie>(cookiesLine));
                }
                //
                foreach (var cefCookie in cefCookies) {
                    //string url = cefCookie.Domain.Remove(0, 1);
                    //string url = cefCookie.Domain;
                    string url = "www.xdlhcg.com";


                    LogManager.WriteLog(url + CefCookieManager.Global.SetCookie(url, cefCookie).ToString());

                    // string setcookieJs = " document.cookie = '{0}={1}';".With(cefCookie.Name, cefCookie.Value, cefCookie.Expires);
                    // LogManager.WriteLog(setcookieJs);
                    //  CefFrameHelper.ExcuteJs(MainCefFrame, setcookieJs)

                    ;

                }
            }

            if (!fileExist) {
                File.Create(newfilePath);
            }
            //

        }

        #region ==============基础函数==============
        private CefWebBrowser GetActiveBrowser() {
            if (tabControl.TabCount > 0) {
                var page = tabControl.TabPages[tabControl.SelectedIndex];
                foreach (var ctl in page.Controls) {
                    if (ctl is CefWebBrowser) {
                        var browser = (CefWebBrowser)ctl;
                        return browser;
                    }
                }
            }

            return null;
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl.TabCount > 0) {
                var page = tabControl.TabPages[tabControl.SelectedIndex];
                foreach (var ctl in page.Controls) {
                    if (ctl is CefWebBrowser) {
                        var browser = (CefWebBrowser)ctl;

                        Text = browser.Title + " - " + _mainTitle;

                        break;
                    }
                }
            } else {
                Text = _mainTitle;
            }
        }

        private void tabControl_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                for (int i = 0; i < tabControl.TabCount; i++) {
                    Rectangle r = tabControl.GetTabRect(i);
                    if (r.Contains(e.Location)) {
                        closeTabContextMenuItem.Tag = tabControl.TabPages[i];
                        tabContextMenu.LostFocus += (s, ev) => { tabContextMenu.Hide(); };
                        tabContextMenu.ChangeUICues += (s, ev) => { tabContextMenu.Hide(); };
                        tabContextMenu.Show(tabControl, e.Location);
                    }
                }
            }
        }

        private void newTabAction(object sender, EventArgs e) {
            NewTab("http://google.com");
        }

        private void goAddressAction(object sender, EventArgs e) {
            var ctl = GetActiveBrowser();
            if (ctl != null) {
                ctl.Browser.GetMainFrame().LoadUrl(addressTextBox.Text);
            }
        }

        public string borrowTitle { get; set; }




        //显示信息
        internal void ShowStatus(string p) {
            statusLabel.Text = p;
        }

        private void closeTabAction(object sender, EventArgs e) {
            var s = (ToolStripMenuItem)sender;
            var page = s.Tag as TabPage;
            if (page != null) {
                page.Dispose();
                page = null;
            }
        }

        private void addressTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                goAddressAction(sender, EventArgs.Empty);
        }
        #endregion

        #region  ==============调试函数==============
        //
        private void BT_EnterVCode_Click(object sender, EventArgs e) {
            if (state == BusinessStatus.tb_login_start
                || state == BusinessStatus.tb_login_ing) {
                string elementid = "J_CodeInput_i";
                string elementvalue = TB_VCode.Text;
                if (!string.IsNullOrEmpty(elementvalue)) {
                    SetElementValueById(elementid, elementvalue);
                }
                MainForm.state = BusinessStatus.tb_login_ing;
                this.ClickById("J_SubmitStatic");
                //   this.state = BusinessStatus.tb_buypage;
            }
        }

        private void BT_RunJs_Click(object sender, EventArgs e) {
            this.GetActiveBrowser().Browser.GetMainFrame().ExecuteJavaScript(TB_Js.Text,

                this.GetActiveBrowser().Browser.GetMainFrame().Url, 0);
        }
        //f 5
        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            this.GetActiveBrowser().Browser.Reload();
        }
        //测试 js 调用c#的方法
        private void toolStripMenuItem4_Click(object sender, EventArgs e) {

            string js1 = "js2csharp('{0}');".With("statue=ready&method=testmethod&result=1");
            MainCefFrame.ExecuteJavaScript(GlobalVar.base_js + js1,
                MainCefFrame.Url, 0);


            //statue=ready&method=testmethod&result=1

        }

        //
        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            var host = this.GetActiveBrowser().Browser.GetHost();
            var wi = CefWindowInfo.Create();
            wi.SetAsPopup(IntPtr.Zero, "DevTools");
            host.ShowDevTools(wi, new DevToolsWebClient(), new CefBrowserSettings());

        }
        private class DevToolsWebClient : CefClient {
        }
        //clear cookies
        public void toolStripMenuItem8_Click(object sender, EventArgs e) {

            CefCookieManager.Global.VisitAllCookies(new CefCookieVisitorImp());


        }

        public CefFrame MainCefFrame {
            get {
                CefFrame cefFrame = null;
                for (int i = 0; i < 5; i++) {
                    try {
                        cefFrame = this.GetActiveBrowser().Browser.GetMainFrame();
                        if (cefFrame != null) {
                            break;
                        }
                    } catch (Exception e) {

                        LogManager.WriteLog(e.ToString());
                    }


                }
                if (cefFrame == null) {
                    LogManager.WriteLog("cefframe is null {0}".With(state));
                }

                return cefFrame;


            }
        }



        private BusinessForms bfForms;
        private void toolStripMenuItem13_Click(object sender, EventArgs e) {
            if (bfForms == null) {
                bfForms = new BusinessForms(this);
                bfForms.Show();
            }
            bfForms.Activate();
        }
        public void BT_login_Click(object sender, EventArgs e) {
            state = BusinessStatus.tb_login_start;
            this.GetActiveBrowser().Browser.GetMainFrame().LoadUrl("https://login.taobao.com/member/login.jhtml");
        }
        #endregion

        #region ==============格式===================


        #region   ==============购物流程==============
        private void toolStripMenuItem5_Click(object sender, EventArgs e) {
            if (sender != null) {
                state = BusinessStatus.tb_buypage;
            }
            BeginInvoke(new Action(() => {
                Thread.Sleep(2000);
                string a1 = "J_LinkBuy tb-iconfont";

                //提交订单
                ClickByClassName(a1);

            }));
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e) {
            if (sender != null) {
                state = BusinessStatus.tb_buy_confirm;
            }
            //添加验证码
            MainCefFrame.ExecuteJavaScript(
                "document.getElementById('J_verifyImageMask').getElementsByTagName('a')[0].click();",
                MainCefFrame.Url, 0);
            //确认支付
            ClickById("J_Go");
            //document.getElementsByClassName("dpl-button")[0].click()
        }
        private void TB_Confirm_VCode_Click(object sender, EventArgs e) {

            //确认支付
            ClickById("J_Go");

        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e) {
            if (sender != null) {
                state = BusinessStatus.tb_buy_zhifu;
            }
            //填写支付密码
            SetElementValueById("payPassword_rsainput", currentHaoZi.zfbPayPwd);
            //提交支付
            ClickById("J_authSubmit");

        }
        #endregion



        #region =============收货流程=============
        //已购买页面 抽取所有的id，分为待付款、待给好评的、待退款、退款中
        private void BT_bought_goods_list_Click(object sender, EventArgs e) {
            if (GlobalVar.ConfirmGoodsUrls.Count > 0) {
                var url = GlobalVar.ConfirmGoodsUrls.Dequeue();
                state = BusinessStatus.tb_nav2confirmUrl1;
                MainCefFrame.LoadUrl(url);
            } else {
                //  MessageBox.Show("没有可收的货了");
                statusLabel.Text += "没有可收的货了";
            }
        }

        [Obsolete]
        private void toolStripMenuItem10_Click(object sender, EventArgs e) {
            //if (sender != null) {
            //    state = BusinessStatus.tb_buy_zhifu_ok;
            //}
            //string bid = "";//
            ////http://trade.taobao.com/trade/pay_success.htm?biz_order_id=631800819552855&out_trade_no=T200P631800819552855

            //GlobalVar.ExtractPar(MainCefFrame.Url).TryGetValue("biz_order_id", out bid);
            ////currentHaoZi.biz_order_id = bid;
            //MainCefFrame.LoadUrl(tb_confirm_goods + "?biz_order_id=" + bid);
        }

        //收货页面跳转至支付页面
        private void toolStripMenuItem9_Click(object sender, EventArgs e) {

            if (sender != null) {
                state = BusinessStatus.tb_confirm_goods;
            }
            MainCefFrame.ExecuteJavaScript(" location.href=document.getElementById('alipay-iframe').src;"
                , MainCefFrame.Url, 0); //      location.href=document.getElementById("alipay-iframe").src;
            //document.getElementById("payPassword_rsainput").value="AQQ6611"
            //_authOnceConfig.warnTxt=""；设置为空之后不会再弹窗。 

        }
        //收货页面的支付页面支付
        private void BT_tb_confirm_goods_redrect2_confirm_Click(object sender, EventArgs e) {
            if (sender != null) {
                state = BusinessStatus.tb_confirm_goods_redrect2_confirm;
            }

            MainCefFrame.ExecuteJavaScript(" _authOnceConfig.warnTxt='';"
            , MainCefFrame.Url, 0);
            SetElementValueById("payPassword_rsainput", currentHaoZi.zfbPayPwd);
            ClickById("J_authSubmit");

        }

        //跳转到好评
        private void BT_nav2remarkSeller_Click(object sender, EventArgs e) {
            if (GlobalVar.RateGoodsUrls.Count > 0) {
                var url = GlobalVar.RateGoodsUrls.Dequeue();
                state = BusinessStatus.tb_rate_nav2goods;
                MainCefFrame.LoadUrl(url);
            } else {
                MessageBox.Show("没有可好评的货了");
            }
        }
        //提交
        private void toolStripMenuItem11_Click(object sender, EventArgs e) {

            state = BusinessStatus.tb_remark_seller;
            //评论间隔一秒钟再提交
            //Clas= submit submit-btn
            string js_1 = @"
                document.getElementsByClassName('good-rate')[0].checked=true;
                document.getElementsByClassName(‘rate-msg’)[0].value=‘好评’;
                var kslist=document.getElementsByClassName('ks-simplestar');for(var i=0;i<kslist.length;i++){ var imglist=kslist[i].getElementsByTagName('img'); imglist[imglist.length-1].click(); }
            document.getElementsByClassName('submit submit-btn')[0].click()";
            MainCefFrame.ExecuteJavaScript(js_1, MainCefFrame.Url, 0);

            ClickByClassName("submit submit-btn");
        }

        //跳转到已购买页面
        private void BT_nav2bought_goods_Click(object sender, EventArgs e) {
            //state = BusinessStatus.tb_nav2bought_list;
            //MainCefFrame.LoadUrl(this.url_bought_list);
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e) {
            MainCefFrame.VisitDom(new domvisit());
            MainCefFrame.GetSource(new Domvisi2T());
        }
        #endregion




        #region =============更换IP地址=============
        public bool NeedChangeIp = false;
        public EventHandler DisconnHandler;
        public EventHandler ConnHandler;
        private bool autoStart;


        #endregion

        private int emailindex_Test = 1;
        private void toolStripMenuItem12_Click_1(object sender, EventArgs e) {
            ProcessCookies(emailindex_Test.ToString(), (emailindex_Test + 1).ToString());
        }



        #region ========搁置========


        #endregion
        #endregion

        //
        private void toolStripMenuItem16_Click(object sender, EventArgs e) {
            Thread.Sleep(2000);
            CefFrameHelper.ExcuteJs(MainCefFrame, "document.getElementById('J-certNo').value='{0}';".With(currentHaoZi.zfbSFZ));

            new Thread(() => {
                #region ---输入支付密码---
                Thread.Sleep(1000);
                MouseKeyBordHelper.POINT p1;
                MouseKeyBordHelper.GetCursorPos(out p1);
                LogManager.WriteLog("鼠标位置 {0},{1}   ".With(p1.X, p1.Y));

                LogManager.WriteLog("程序界面位置 {0},{1}   ".With(this.Left, this.Top));

                LogManager.WriteLog("边距 {0},{1}   ".With(p1.X - this.Left, p1.Y - this.Top));

                int chax = 123;
                int chay = 232;
                Thread.Sleep(1000);
                MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay);

                Thread.Sleep(1000);
                var s1 = currentHaoZi.zfbPayPwd;
                LogManager.WriteLog(s1);
                MouseKeyBordHelper.KeyBoardDo(s1.ToCharArray());
                #endregion

                new Thread(() => {
                    Thread.Sleep(2000);
                    CefFrameHelper.ExcuteJs(MainCefFrame,
                        "document.getElementById('J-selectStrategyForm').submit();");
                    //LogManager.WriteLog("mouse {0},{1}   ".With(p1.X, p1.Y));
                    //MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay + 65);
                }).Start();
            }).Start();

        }


        //public override bool PreProcessMessage(ref Message msg) {
        //    return base.PreProcessMessage(ref msg);
        //}

        //bool PreFilterMessage(ref Message m) {
        //    if (m.Msg == 513) {
        //        LogManager.WriteLog(m.ToString());
        //    }
        //    return false;
        //}

        private void toolStripMenuItem17_Click(object sender, EventArgs e) {
            Control.CheckForIllegalCrossThreadCalls = false;
            state = BusinessStatus.Zfb_reg_shifou_bangding_shouji_before;
            MainCefFrame.LoadUrl("https://my.alipay.com/portal/account/index.htm");
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e) {
            CefFrameHelper.ExcuteJs(MainCefFrame, "document.getElementById('J-input-user').value='{0}';".With(currentHaoZi.zfbEmail));
            new Thread(() => {
                Thread.Sleep(3000);
                MouseKeyBordHelper.POINT p1;
                MouseKeyBordHelper.GetCursorPos(out p1);

                LogManager.WriteLog("鼠标位置 {0},{1}   ".With(p1.X, p1.Y));

                LogManager.WriteLog("程序界面位置 {0},{1}   ".With(this.Left, this.Top));

                LogManager.WriteLog("边距 {0},{1}   ".With(p1.X - this.Left, p1.Y - this.Top));
                //point 596,406   
                //Form 469,172   


                Thread.Sleep(1000);

                int chax = 123;
                int chay = 232;
                MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay);

                Thread.Sleep(3000);

                var s1 = currentHaoZi.zfbPwd;

                LogManager.WriteLog(s1);
                MouseKeyBordHelper.KeyBoardDo(s1.ToCharArray());


                //var ss1 = "abcdtxyzABCDEFGXYZ0123456789";
                //LogManager.WriteLog(ss1);
                //MouseKeyBordHelper.KeyBoardDo(ss1.ToCharArray());



                //点击登录
                Thread.Sleep(3000);
                MouseKeyBordHelper.MoveAndLeftClick(this.Left, this.Top, chax, chay + 80);

                MouseKeyBordHelper.GetCursorPos(out p1);
                LogManager.WriteLog("click point {0},{1} ".With(p1.X, p1.Y));



                //  CefFrameHelper.ExcuteJs(MainCefFrame, "document.getElementById('J-login-btn').click();");
                CefFrameHelper.ExcuteJs(MainCefFrame, "document.getElementById('J-login-btn').click();");


            }).Start();
        }
        //open tb
        private void toolStripMenuItem19_Click(object sender, EventArgs e) {

        }

       
   






    }
}
