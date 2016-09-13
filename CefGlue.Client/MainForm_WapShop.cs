using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using NUnit.Framework;
using PwBusiness;
using PwBusiness.Model;
using Xilium.CefGlue.Client.VCode;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {
    public partial class MainForm : Form {
        private bool IsInit_wapshop_loaded = false;
        public  static  int shoppingIndex = 1;
        private string beforejs = "if(window.frames.length!=0){mydoc=window.frames[0].document;}else{mydoc=document;}";
        //开始
        private void BT_TB_SHOPV2_Click(object sender, EventArgs e) {
            state = BusinessStatus.mtb;
            MainCefFrame.LoadUrl("http://login.m.taobao.com/login.htm");
        }

        public object wap_tmp = new object();
        private void IsInit_wapshop() {
            if (IsInit_wapshop_loaded) {
                return;
            }

            Operation mtb = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.mtb,
                NextStatus = BusinessStatus.gerenzhongxin,
                CurrentUrl = "login.m.taobao.com/login.htm",
                index = 1,
                note = "登录"
            };
            mtb.OperationHandler += (s, e) => {


                //  if (GlobalVar.tbmode == tbModel.shopv2) {
                //window.onload=function(){if(window.frames.length!=0){window.location.reload();}}
                //ReflushIfInFrames();
                //  }
                taskRunner.LogAccontStatus("|{0}".With(MouseKeyBordHelper.CurrentIP));//  
                #region ==========识别验证码==========
                //1.识别验证码
                string srcs = "";
                for (int i = 0; i < 3; i++) {
                    //   //Img Id=J_StandardCode  data-url
                    srcs = CefFrameHelper.GetUrlListByHapId(MainCefFrame, "J_StandardCode", "img", "data-url").FirstOrDefault();
                    if (!string.IsNullOrEmpty(srcs)) {
                        break;
                    }
                }
                if (string.IsNullOrEmpty(srcs)) {
                    LogManager.WriteLog("重复3次后仍然无法获取图片。");
                    return;
                }
                string returnMess;
                var isVCodeOk = Vcode.GetVcodeFormImageUrl(srcs, out returnMess);


                //2.提交验证码
                string js2run = beforejs + " mydoc.getElementById('J_UserNameTxt').value='{0}';".With(currentHaoZi.tbName) +
                                "mydoc.getElementById('J_PassWordTxt').value='{0}';".With(currentHaoZi.tbPwd) +
                             "mydoc.getElementById('J_AuthCodeTxt').value='{0}';".With(returnMess) +
                                "mydoc.getElementsByClassName('c-btn-oran-big')[0].click();";
                if (isVCodeOk) {
                    //   LogManager.WriteLog(js2run);

                    CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                } else {


                    LogManager.WriteLog("验证码识别出错。等待2分钟后重新执行");
                }

                new Thread(() => {

                    Thread.Sleep(3000);
                    if (isCurrentPage(s as Operation)) {
                        LogManager.WriteLog("还在当前页");
                        string warnmsg = CefFrameHelper.ExtractWarnMsg(MainCefFrame).Trim();
                        if (warnmsg == "该账户名不存在") {
                            taskRunner.ReportAccountStatus("buy", "failed", warnmsg);
                            ClearAndPrepareNext();
                        }
                        ;
                    }

                }).Start();


                #endregion

            };
            //
            Operation mtbloginEx = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.gerenzhongxin,
                NextStatus = BusinessStatus.wapshop_ex_loginerror,
                CurrentUrl = "login.m.taobao.com/login.htm?_input_charset=utf-8&sid=",
                index = 2,
                note = "登录失败"
            };
            mtbloginEx.OperationHandler += (s, e) => {
                taskRunner.ReportAccount("failed");
                ClearAndPrepareNext();
            };
            //进入搜索页

            //直接跳转
            //Operation gerenzhongxin = new Operation(MainCefFrame) {
            //    PerviousStatus = BusinessStatus.gerenzhongxin,
            //    NextStatus = BusinessStatus.Awp_core_detail,
            //    CurrentUrl = "http://h5.m.taobao.com/awp/mtb/mtb.htm",
            //    index = 2,
            //    note = "跳转到某货"
            //};
            //gerenzhongxin.OperationHandler += (s, e) => {
            //    string shaopaddress = FileHelper.RandomReadOneLine("config//goodaddress.txt");// "http://h5.m.taobao.com/awp/core/detail.htm?id=39552254784&spm=0.0.0.0";
            //    MainCefFrame.LoadUrl(shaopaddress);
            //};

            //跳转到搜索
            Operation act_sale_searchlist = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.gerenzhongxin,
                NextStatus = BusinessStatus.act_sale_searchlist,
                CurrentUrl = "h5.m.taobao.com/awp/mtb/mtb.htm",
                index = 2.1,
                note = "跳转到搜索页面"
            };
            act_sale_searchlist.OperationHandler += (s, e) => {
                string js2run = beforejs +
                    "window.location=mydoc.getElementsByClassName('search')[0].getElementsByTagName('a')[0].getAttribute('dataurl');";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);

            };

            //输入关键字
            Operation search_htm = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.act_sale_searchlist,
                NextStatus = BusinessStatus.search_htm,
                CurrentUrl = "m.taobao.com/channel/act/sale/searchlist.html?pds=search",
                index = 2.21,
                note = "输入搜索的关键字"
            };
            search_htm.OperationHandler += SearchOneItem;

            //输入关键字2
            Operation search_htm2 = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.act_sale_searchlist,
                NextStatus = BusinessStatus.search_htm,
                CurrentUrl = "m.taobao.com/?from=wapp",
                index = 2.22,
                note = "输入搜索的关键字"
            };
            search_htm2.OperationHandler += SearchOneItem;
            //跳转到随机一个搜索结果`1
            Operation search_htm2detail = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.search_htm,
                NextStatus = BusinessStatus.Awp_core_detail,
                index = 2.31,
                CurrentUrl = "s.m.taobao.com/h5?q=",
                note = "跳转到随机一个搜索结果"

            };
            search_htm2detail.OperationHandler += ChooseOneItem;

            //跳转到随机一个搜索结果`2
            Operation search_htm2detail2 = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.search_htm,
                NextStatus = BusinessStatus.Awp_core_detail,
                index = 2.32,
                CurrentUrl = "s.m.taobao.com/search.htm?q=",
                note = "跳转到随机一个搜索结果"

            };
            search_htm2detail2.OperationHandler += ChooseOneItem;
            //end of 2
            Operation Awp_core_detail = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.Awp_core_detail,
                NextStatus = BusinessStatus.awp_base_buy,
                CurrentUrl = "h5.m.taobao.com/awp/core/detail.htm?id=",
                index = 3,
                note = "确认下单"
            };
            Awp_core_detail.OperationHandler += (s, e) => {
                int sleepTime = RandomManager.random.Next(2, 8);
                LogManager.WriteLog("暂停{0}秒".With(sleepTime));
                Thread.Sleep(sleepTime * 1000);

                string js2run = beforejs;
                //随机收藏
                if (RandomManager.randomBool()) {
                    js2run += "mydoc.getElementsByClassName('dts-fav')[0].click();";
                }
                js2run += "window.setTimeout(function(){mydoc.getElementsByClassName('c-btn-orgn dt-immbuy')[0].click();},2000); ";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                ExtractIfCurrentPage(s);
            };

            Operation awp_base_buy = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.awp_base_buy,
                NextStatus = BusinessStatus.exCashier,
                CurrentUrl = "h5.m.taobao.com/awp/base/buy.htm?itemId=",
                index = 4,
                note = "提交订单"
            };
            awp_base_buy.OperationHandler += (s, e) => {

                var str = RandomManager.RandomReadOneLine("import//liuyan.txt");
                str = str.ReplaceNum();
                //返回的随机数的下界（随机数可取该下界值）。返回的随机数的上界（随机数不能取该上界值）。
                bool annoymous = RandomManager.random.Next(1, 3) > 1;
                ;
                bool isCurrentPage = false;

                for (int i = 0; i < 3; i++) {
                    LogManager.WriteLog("#第{0}次购买".With(i));
                    //
                    string js2run = beforejs + "mydoc.getElementsByClassName('c-form-txt-normal')[1].value='{0}';".With(str);
                    if (annoymous) {
                        js2run += "mydoc.getElementsByName('anonymous')[0].checked=true;";
                    }
                    js2run += "mydoc.getElementsByClassName('submit-only c-btn-oran')[0].click();";
                    CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                    //
                    //  new Thread(() => {
                    Thread.Sleep(3000);
                    lock (wap_tmp) {

                        //if (!isCurrentPage(s as Operation)) {
                        isCurrentPage = ExtractAndReloadIfCurrentPage(s);
                        //    //不停留在当前页面,则表示没问题
                        //} else {
                        //    LogManager.WriteLog("刷新并等待4秒");
                        //    MainCefFrame.Browser.Reload();
                        //    Thread.Sleep(4000);
                        //}
                    }
                    if (!isCurrentPage) {
                        break;
                    }

                    //  }).Start();
                }



            };

            Operation exCashier = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.exCashier,
                NextStatus = BusinessStatus.asyn_payment_result,
                CurrentUrl = "wapcashier.alipay.com/cashier/exCashier.htm?orderId=",
                index = 4,
                note = "付款方式1"
            };
            exCashier.OperationHandler += (s, e) => {
                //检查
                //<span class="price">0.01元</span>
                float maxPrice;
                bool isOverPrice;
                var priceFloat = PriceFloat(out maxPrice, out isOverPrice);
                if (isOverPrice) {
                    LogManager.WriteLog("{0}超出maxPrice:{1},停止购买".With(priceFloat, maxPrice));
                    return;
                }
                //
                //付款
                string js2run = beforejs +
                       "mydoc.getElementById('couponPayment').checked=true;" +
                       "mydoc.getElementsByName('paymentPassword')[0].value='{0}';".With(currentHaoZi.zfbPayPwd) +
                       "mydoc.getElementsByClassName('ui-button ui-button-submit')[0].click();";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                ExtractIfCurrentPage(s);
            };


            Operation exCashier2 = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.exCashier,
                NextStatus = BusinessStatus.asyn_payment_result,
                CurrentUrl = "mclient.alipay.com/w/trade_pay.do?alipay_trade_no=",
                index = 5,
                note = "付款方式2"

            };
            exCashier2.OperationHandler += (s, e) => {
                string js2run = beforejs + "mydoc.getElementById('pwd').value='{0}';".With(currentHaoZi.zfbPayPwd) +
                     "mydoc.getElementsByClassName('J-button-submit')[0].click()";
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
                ExtractIfCurrentPage(s);
            };

            #region ==========该次购买结束1、2==========
            Operation payok = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.asyn_payment_result,
                NextStatus = BusinessStatus.TB_shop_V2_Ready,
                CurrentUrl = "wapcashier.alipay.com/cashier/asyn_payment_result.htm",
                index = 6.1,
                note = "该次购买结束1",

            };
            payok.OperationHandler += finishOneShop;
            Operation cashierPay = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.asyn_payment_result,
                NextStatus = BusinessStatus.TB_shop_V2_Ready,
                CurrentUrl = "mclient.alipay.com/cashierPay.htm?awid=",
                index = 6.2,
                note = "该次购买结束2",

            };
            cashierPay.OperationHandler += finishOneShop;

            //购买失败
            Operation accessDenied = new Operation(MainCefFrame) {
                PerviousStatus = BusinessStatus.asyn_payment_result,
                NextStatus = BusinessStatus.ready,
                CurrentUrl = "mclient.alipay.com/accessDenied.htm",
                index = 6.3,
                note = "无权访问，重试pay"
            };

            accessDenied.OperationHandler += (s, e) => {

                lock (lockobject) {
                    MainCefFrame.Browser.GoBack();
                    Thread.Sleep(3000);
                    state = BusinessStatus.exCashier;
                    MainCefFrame.Browser.Reload();

                }


            };
            // mclient.alipay.com/accessDenied.htm

            #endregion




            opsList.Add(mtb);
            //  opsList.Add(gerenzhongxin);
            //搜索进店
            opsList.Add(act_sale_searchlist);
            opsList.Add(search_htm);
            opsList.Add(search_htm2detail);
            opsList.Add(search_htm2detail2);
            //end of 搜索进店

            opsList.Add(Awp_core_detail);
            opsList.Add(awp_base_buy);
            opsList.Add(exCashier);
            opsList.Add(exCashier2);

            //
            opsList.Add(payok);//成功方式一
            opsList.Add(cashierPay);//成功方式二
            opsList.Add(accessDenied);

            //

            opsList.Add(mtbloginEx);
        }

        public static void ReflushIfInFrames(CefFrame Frame1) {
            if (GlobalVar.tbmode == tbModel.shopv2) {
                Frame1.ExecuteJavaScript("if(window.frames.length!=0){window.location.reload();}",
                    Frame1.Url, 0);
                LogManager.WriteLog(" tbModel.shopv2  window.frames.length!=0");
                Thread.Sleep(3000);
            }
        }

        private float PriceFloat(out float maxPrice, out bool isOverPrice) {
            string pricestr = "";
            pricestr = CefFrameHelper.GetInnerHtmlByHap(MainCefFrame, "span", "class", "price").FirstOrDefault();
            pricestr = pricestr.Replace("元", "");
            float priceFloat = float.Parse(pricestr);
            maxPrice = float.Parse(ConfigHelper.GetValue("maxPrice"));
            isOverPrice = priceFloat > maxPrice;
            return priceFloat;
        }

        private AutoResetEvent waitchooseResetEvent = new AutoResetEvent(true);

        private List<string> shoppingUrls = new List<string>();
        [Test]
        public void TestSbAppend() {
            string maxprice = "0.02";
            var arrayStr = ArrayStr();
            StringBuilder sb = new StringBuilder();
            sb.Append(beforejs);
            //  js2run += "  var a1=mydoc.getElementsByClassName('page-container J_PageContainer_1')[0]; var lis=a1.getElementsByTagName('li');var index=Math.round(Math.random()*lis.length); window.location=lis[index].getElementsByTagName('a')[0].href;";
            sb.Append("var  shopping =new Array ( {0} );".With(arrayStr));
            sb.Append(
                "if(window.frames.length!=0){mydoc=window.frames[0].document;}else{mydoc=document;}var a1=mydoc.getElementsByClassName('page-container J_PageContainer_1')[0];var lis=a1.getElementsByTagName('li'); ");
                sb.Append("while(1){var index=Math.floor(Math.random()*lis.length); var li0=lis[index];var li0_h=li0.getElementsByClassName('h')[0]; var li0_h_int=li0_h.innerText.substring(1,li0_h.innerHTML.length);");
                sb.Append("if(li0_h_int< " + maxprice + " ){ " );
                     sb.Append(  "var desturl=li0.getElementsByTagName('a')[0].href;var exist=false;for(var t1=0;t1<t1.length;t1++){if(shopping[t1]==desturl){exist=true;break;}}if(!exist){window.location=desturl; break;} }}");

          
            var js2run = sb.ToString();
            Console.WriteLine(js2run);
        }

        private void ChooseOneItem(object sender, LoadEndEventArgs eventHandler) {

            //ReflushIfInFrames();

            bool isok = false;
            string maxprice = ConfigHelper.GetValue("maxPrice");
            string minNum = ConfigHelper.GetValue("待购宝贝最少销量");
            var arrayStr = ArrayStr();


            LogManager.WriteLog(arrayStr);

            for (int i = 0; i < 3; i++) {
                StringBuilder sb = new StringBuilder();
                sb.Append(beforejs);
                //  js2run += "  var a1=mydoc.getElementsByClassName('page-container J_PageContainer_1')[0]; var lis=a1.getElementsByTagName('li');var index=Math.round(Math.random()*lis.length); window.location=lis[index].getElementsByTagName('a')[0].href;";
                sb.Append("var  shopping =new Array ( {0} );".With(arrayStr));
                sb.Append(
                    "if(window.frames.length!=0){mydoc=window.frames[0].document;}else{mydoc=document;}var a1=mydoc.getElementsByClassName('page-container J_PageContainer_1')[0];var lis=a1.getElementsByTagName('li'); ");
                sb.Append("while(1){var index=Math.floor(Math.random()*lis.length); var li0=lis[index];var li0_h=li0.getElementsByClassName('h')[0]; var li0_h_int=li0_h.innerText.substring(1,li0_h.innerHTML.length);var li0_num=li0.getElementsByClassName('d-num')[0]; var li0_num_int=li0_num.innerText.substring(0,li0_num.innerHTML.length-3); ");
                sb.Append("if(li0_h_int< " + maxprice + " &&  li0_num_int>" + minNum + " ){ ");
                sb.Append("var desturl=li0.getElementsByTagName('a')[0].href;desturl=desturl.substring(0,desturl.indexOf('&'));var exist=false;for(var t1=0;t1<t1.length;t1++){if(shopping[t1]==desturl){exist=true;break;}}if(!exist){window.location=desturl; break;} }}");


                var js2run = sb.ToString();
                CefFrameHelper.ExcuteJs(MainCefFrame, js2run);


                if (ConfigHelper.GetBoolValue("debug")) {
                    LogManager.WriteLog(js2run);
                }

                waitchooseResetEvent.Reset();
                new Thread(() => {
                    Thread.Sleep(2000);
                   var url= MainCefFrame.Url;
                   if (!url.Contains("http://h5.m.taobao.com/awp/mtb/mtb.htm")) {
                        isok = true;
                        return; 
                    }
                    else {
                        shoppingUrls.Add(url.Substring(0,url.IndexOf('&')));
                    }
                    //  LogManager.WriteLog(js2run);

                    LogManager.WriteLog("未能进入下一页 {0}".With(i));

                    waitchooseResetEvent.Set();

                }).Start();

                waitchooseResetEvent.WaitOne();
                if (isok) {
                    LogManager.WriteLog("已经跳转到下一页  ");
                    break;
                }

                LogManager.WriteLog("等待5秒钟");
                Thread.Sleep(1000 * 5);

            }
        }
        [Test]
        public void TestArrayStr() {
            string url =
                "http://h5.m.taobao.com/awp/core/detail.htm?id=40149983089&sid=a1984535fb41ef56&abtest=5&rn=85c2a0ed76f24d9b698a1c89410baef3";
           Console.WriteLine( url.Substring(0, url.IndexOf('&')));

        }

        public string ArrayStr() {
         
            string arrayStr = "";
            if (shoppingUrls.Count==0) {
                return arrayStr;
            }
            for (int i = 0; i < shoppingUrls.Count; i++) {
                arrayStr += "'{0}',".With(shoppingUrls[i]);
            }
            arrayStr = arrayStr.Remove(arrayStr.Length - 1);
            Console.WriteLine(arrayStr);
            return arrayStr;
        }

        //抽取提示消息 <div class="warnMsg">请输入账户名！</div>

        public bool isCurrentPage(Operation op) {
            if (MainCefFrame != null && op != null) {
                LogManager.WriteLog("MainCefFrame.Url:" + MainCefFrame.Url);
                LogManager.WriteLog("op.CurrentUrl:" + op.CurrentUrl);

                return MainCefFrame.Url.Contains(op.CurrentUrl);
            } else {
                return false;
            }
        }

        private bool ExtractAndReloadIfCurrentPage(object s) {
            bool iscurrentPage = false;


            new Thread(() => {
                Thread.Sleep(3000);
                if (isCurrentPage(s as Operation)) {
                    // 停留在当前页面 
                    LogManager.WriteLog("还在当前页面");
                    LogManager.WriteLog("刷新并等待4秒");
                    MainCefFrame.Browser.Reload();
                    Thread.Sleep(4000);
                    iscurrentPage = true;


                    LogManager.WriteLog("  释放");
                    GlobalVar.IsloadOkAutoResetEvent.Set();
                }

            }).Start();
            LogManager.WriteLog("  默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromSeconds(10));
            LogManager.WriteLog("  阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();

            return iscurrentPage;
        }

        private bool ExtractIfCurrentPage(object s) {
            bool iscurrentPage = false;


            new Thread(() => {
                Thread.Sleep(2000);
                if (isCurrentPage(s as Operation)) {
                    // 停留在当前页面 
                    LogManager.WriteLog("还在当前页面");
                    CefFrameHelper.ExtractWarnMsg(MainCefFrame);
                    iscurrentPage = true;


                    LogManager.WriteLog("  释放");
                    GlobalVar.IsloadOkAutoResetEvent.Set();
                }

            }).Start();
            LogManager.WriteLog("  默认阻塞，现在等待");
            GlobalVar.IsloadOkAutoResetEvent.WaitOne(TimeSpan.FromSeconds(10));
            LogManager.WriteLog("  阻塞");
            GlobalVar.IsloadOkAutoResetEvent.Reset();

            return iscurrentPage;
        }

        private void finishOneShop(object sender, LoadEndEventArgs eventHandler) {
            //1.计算是否完成X次购买
            if (shoppingIndex >= int.Parse(ConfigHelper.GetValue("shoppingCount"))) {
                taskRunner.ReportAccount("sucess");
                ClearAndPrepareNext();
            } else {
                state = BusinessStatus.gerenzhongxin;
                MainCefFrame.LoadUrl("http://h5.m.taobao.com/awp/mtb/mtb.htm");
            }
            shoppingIndex++;

            //2.完成则切换下一个，未完成则
        }

        public void SearchOneItem(object sender, LoadEndEventArgs eventHandler) {
            string searchKeys = FileHelper.read("config/searchkeysV2.txt")[shoppingIndex - 1]; //RandomManager.RandomReadOneLine("config/searchkeys.txt");
            var keysArry = searchKeys.Split('|');
            var key = keysArry[RandomManager.random.Next(0, keysArry.Count())];

            LogManager.WriteLog(key);
            string js2run = beforejs +
                "mydoc.getElementById('inp-search-index').value='{0}';".With(key) +
                "mydoc.getElementsByClassName('bton-search')[0].click();";
            CefFrameHelper.ExcuteJs(MainCefFrame, js2run);
        }

        [Test]
        public void TestSearchOneItem() {
            //  SearchOneItem(null, null);
        }
    }
}