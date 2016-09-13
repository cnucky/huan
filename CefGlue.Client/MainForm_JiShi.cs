using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Common;
using JiShi.BLL;
using JiShi.Model;
using Maticsoft.DBUtility;
using Newtonsoft.Json;
using NUnit.Framework;
using PwBusiness;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {

    public partial class MainForm : Form {

        //全自动
        private void toolStripMenuItem21_Click(object sender, EventArgs e) {

        }
        private SmtpClient _smtpClient;//kciph92964208@163.com|aw26506123
        private void init_smtpclient() {
            if (_smtpClient == null) {
                _smtpClient = new SmtpClient();
                _smtpClient.Host = "smtp.163.com";// this.popServerTextBox.Text;
                _smtpClient.Port = 25;
                // 不使用默认凭证，即需要认证登陆
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential("kciph92964208@163.com", "aw26506123");// new NetworkCredential(loginTextBox.Text, passwordTextBox.Text);
                _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            }
        }

        /// <summary>
        /// 之前的准备函数-调度计时器定时运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void old_run_fun(object sender, EventArgs e) {
            // ovtimeTick
            ovtimeTick.Stop();
            var op = new Operation(MainCefFrame);

            op.OperationHandler += (s, e1) => {


                //开始操作
                //if (GlobalVar.tbmode == tbModel.shopv2) {
                //    BT_TB_SHOPV2_Click(this, new EventArgs());
                //} else if (GlobalVar.tbmode == tbModel.reg_zfb) {
                //    BT_Reg_zfb_Click(this, new EventArgs());
                //} else if (GlobalVar.tbmode == tbModel.reg_tbV3) {

                //    lead2tbV3reg();
                //} else {
                //    LogManager.WriteLog("该模式目前不支持。");
                //}

                this.Invoke((EventHandler)delegate {
                    jishi_load_selling_Click(s, e1);//1.抽取
                });

                //2.分析
                NotifyUser(); //3.告警
                ovtimeTick.Next(op, null);//计时器重新计时
                LogManager.WriteLog("又一次定时分析开始");
            };
            op.Deadline = 1000 * 60 * 2;// GlobalVar.DeadLineOfChangeClearAndPrepareNext;
            ovtimeTick.Next(op, null);//计时器重新计时
            LogManager.WriteLog("定时分析开始");
        }

        /// <summary>
        /// 比较并通知
        /// </summary>
        [Test]
        public void NotifyUser() {

            bool needNotify = false;
            JiShi.BLL.js_item Bll = new JiShi.BLL.js_item();
            //更新最低成交价
            var ds0 = DbHelperMySQL.Query(" SELECT itemname,itemprice*par as price  FROM `js_concernitem` ").Tables[0];

            for (int i = 0; i < ds0.Rows.Count; i++) {
                var name = ds0.Rows[i]["itemname"];
                var price = ds0.Rows[i]["price"];

                //2.获取数据库中对应的价格并作比较
                var models1 = Bll.GetModelList("  itemName='{0}'   and status==NULL ".With(name, price));
                if (models1.Count > 0) {
                    // MailBll.SendOneEmail(_smtpClient, "kciph92964208@163.com", "568264099@qq.com", RandomManager.GetRandomName() + " " + name + price, RandomManager.GetRandomName(), "");
                    LogManager.WriteLog("更新最低价{0}={1}".With(name, price));
                }
            }

            //加入各个残卷的低价获取和通知
            //1.抓取指定表格中的物品名及其价格
            var ds1 = DbHelperMySQL.Query(" SELECT itemname,itemprice*par as price  FROM `js_concernitem` ").Tables[0];

            for (int i = 0; i < ds1.Rows.Count; i++) {
                var name = ds1.Rows[i]["itemname"];
                var price = ds1.Rows[i]["price"];

                //2.获取数据库中对应的价格并作比较
                var models1 = Bll.GetModelList("  itemName='{0}' and rice/itemAmount < {1} and gameItemId<>NULL ".With(name, price));
                if (models1.Count > 0) {
                    MailBll.SendOneEmail(_smtpClient, "kciph92964208@163.com", "568264099@qq.com", RandomManager.GetRandomName() + " " + name + price, RandomManager.GetRandomName(), "");
                }
            }


            //

            //银子的通知
            var models = Bll.GetModelList("  itemName='银子' and 1000*price/(itemAmount/1000)<75 and gameItemId<>NULL ");
            if (models.Count > 0) {
                needNotify = true;
            }

            if (needNotify) {
                init_smtpclient();


                MailBll.SendOneEmail(_smtpClient, "kciph92964208@163.com", "568264099@qq.com", RandomManager.GetRandomName() + " 低于75银子", RandomManager.GetRandomName(), "");

            }
            //

        }


        private bool jishi_start = true;
        private string analysisModel = "item";//item ,role

        public int currentpage = 0;
        int modeListIndex = 0;
        List<string> modeList = new List<string>();
        /// <summary>
        /// 加载分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jishi_load_selling_Click(object sender, EventArgs e) {

            jishi_start = true;

            modeList.Clear();
            modeList.Add("findSellingGoods");
            modeList.Add("historicalJsonData");
            modeList.Add("findNoticeGoods");
            //   mode = modeList[0];
            currentpage = 0;
            modeListIndex = 0;
            MainCefFrame.LoadUrl("http://jishi.woniu.com/9yin/{0}.html".With(modeList[modeListIndex]));
            //Thread.Sleep(TimeSpan.FromSeconds(5));
            //MainCefFrame.LoadUrl("http://jishi.woniu.com/9yin/{0}.html".With(modeList[modeListIndex]));

        }
        /// <summary>
        /// 每次都执行
        /// </summary>
        private void OnPageLoaded_JiShi() {

            if (!jishi_start) {
                return;
            }


            var current_url = MainCefFrame.Url;
            if (!current_url.Contains("Goods") && !current_url.Contains("historicalJsonData") && !current_url.Contains("tradeItemDetail")) {
                return;
            }

            Domvisi2T dom1 = new Domvisi2T();
            MainCefFrame.GetSource(dom1);
            var htm = GlobalVar.CurrentHtml;
            if (string.IsNullOrEmpty(htm)) {
                return;
            }

            ////角色分析模式
            //if (analysisModel == "role") {

            //    //数据导出
            //    LogManager.WriteLog("role-" + DateTime.Now.ToFileTime().ToString(), htm);


            //    return;
            //}

            //物品分析模式

            //0 数据过滤
            htm = htm.Replace("<html><head></head><body>", "");
            htm = htm.Replace("</body></html>", "");
            htm = htm.Replace("</div>", "");
            htm = htm.Replace("</font>", "");


            // File.WriteAllText("tempload/temp_download-{0}.txt".With(DateTime.Now.ToFileTime()), htm);
            try {

                string s1 = "";
                s1 = Regex.Replace(htm, "itemDesc\":\"(.*?)\",\"itemName", "itemDesc\":\"nothing\",\"itemName");
                s1 = Regex.Replace(s1, "itemDesc\":\"(.*?)\",\"itemSellerRole", "itemDesc\":\"nothing\",\"itemSellerRole");
                //itemSellerRole
                s1 = s1.Remove(0, 1);
                s1 = s1.Remove(s1.Length - 1, 1);

                //1.抽数据
                JishiObject js1 = JsonConvert.DeserializeObject(s1, typeof(JishiObject)) as JishiObject;
                JiShi.BLL.js_item Bll = new JiShi.BLL.js_item();

                foreach (var item in js1.pagedata) {
                    #region MyRegion

                    JiShi.Model.js_item model = new JiShi.Model.js_item();
                    model.closed = item.closed;
                    model.createDate = item.createDate;
                    model.gameId = item.gameId;
                    model.gameItemId = item.gameItemId;
                    model.gender = item.gender;
                    model.gradeName = item.gradeName;
                    model.guild = item.guild;
                    model.iconPath = item.iconPath;
                    model.itemCode = item.itemCode;
                    model.itemDesc = item.itemDesc;
                    model.itemName = item.itemName;
                    model.itemType = item.itemType;
                    model.power = item.power;
                    model.publicityEndDate = item.publicityEndDate;
                    model.sellerCasId = item.sellerCasId;
                    model.sellerGameId = item.sellerGameId;
                    model.sellerRole = item.sellerRole;
                    model.serverId = item.serverId;
                    //serverid部分数据会丢失
                    model.status = item.status;


                    model.curId = item.curId;
                    model.grade = item.grade;
                    model.id = item.id;
                    model.itemAmount = item.itemAmount.ToString();
                    model.price = item.price;
                    model.returnDate = item.returnDate;
                    model.saveTime = item.saveTime;
                    model.shelfDate = item.shelfDate;
                    model.shelfDays = item.shelfDays;
                    model.closed = item.unitPrice;
                    #endregion
                    //为了对付 最近成交模式
                    if (model.id == null) {
                        model.id = model.gameItemId;
                    }
                    //
                    if (!Bll.Exists(model.id)) {
                        model.status = modeList[modeListIndex];
                        LogManager.WriteLog("保存：" + model.gameId + " " + model.itemName + " " + Bll.Add(model));
                    } else {
                        // LogManager.WriteLog("更新：" + model.gameId + " " + model.itemName + " " + Bll.Update(model));
                    }
                }

                //2.翻页
                //   Thread.Sleep(TimeSpan.FromSeconds(RandomManager.random.Next(2,4)));
                if (modeList[modeListIndex] == "historicalJsonData") {
                    js1.pageInfo.totalPages = 6;//historicalJsonData模式下最多只能抓取6页
                }
                Console.WriteLine("类型:" + modeList[modeListIndex] + " 页码：" + js1.pageInfo.pageId + " 总页数:" + js1.pageInfo.totalPages);
                if (js1.pageInfo.pageId < js1.pageInfo.totalPages) {
                    jishi_next.PerformClick();
                } else {
                    if (modeListIndex + 1 < modeList.Count) {
                        modeListIndex++;
                        currentpage = -1;
                        jishi_next.PerformClick();

                    } else {
                        jishi_start = false;
                    }

                }


            } catch (Exception e) {
                LogManager.WriteLog(e.ToString());
            }


        }

        private void jishi_next_Click(object sender, EventArgs e) {

            currentpage = currentpage + 1;

            //http://jishi.woniu.com/9yin/historicalJsonData.html?pageIndex=1&_=1447230525600
            var url_next = "http://jishi.woniu.com/9yin/{0}.html?pageIndex={1}".With(modeList[modeListIndex], currentpage);

            Console.WriteLine("next :" + currentpage + ": " + url_next);
            MainCefFrame.LoadUrl(url_next);
        }

        private void toolStripMenuItem22_Click(object sender, EventArgs e) {

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_loadOneRole_Click(object sender, EventArgs e) {
            //    string url = "http://jishi.woniu.com/9yin/tradeItemDetail.html?catagory=1&itemId=2768865";
            //analysisModel = "role";
            //MainCefFrame.LoadUrl("http://jishi.woniu.com/9yin/tradeItemDetail.html?catagory=1&itemId={0}".With("2768865"));



        }

        [Test]
        public void t1() {

            //pageData pd = new pageData();
            //pd.closed = true;
            //pd.gameId = "12312";

            //    var a1 = JsonConvert.SerializeObject(pd);
            //     Console.Write(a1);

            //var s1="\"closed\":false,\"createDate\":\",\"curId\":18,"gameId":"","gameItemId":"","gender":"","grade":0,"gradeName":"","guild":"","iconPath":"http://180.153.139.98/res/money/yb.png","id":1518894,"itemAmount":1000000,"itemCode":"CapitalType2","itemDesc":"","itemName":"银子","itemType":"2199","power":"","price":88,"publicityEndDate":"2014-09-12 08:46:09","returnDate":"","saveTime":"2014-09-12 08:46:09","sellerCasId":"","sellerGameId":"B347C67E475948C582DFCE392B02AAA7","sellerRole":"郎纵横四海","serverId":"7100046","shelfDate":"2014-09-12 08:46:09","shelfDays":13,"status":"已上架","unitPrice":0}";
        }

        [Test]
        public void t2() {

            var s1 = File.ReadAllText("download2.txt");

            s1 = Regex.Replace(s1, "itemDesc\":\"(.*?)\",\"itemName", "itemDesc\":\"nothing\",\"itemName");

            s1 = s1.Remove(0, 1);
            s1 = s1.Remove(s1.Length - 1, 1);
            File.WriteAllText("temp_record.txt", s1);

            //
            JishiObject js1 = JsonConvert.DeserializeObject(s1, typeof(JishiObject)) as JishiObject;

            //1.抽数据

            JiShi.BLL.js_item Bll = new JiShi.BLL.js_item();

            foreach (var item in js1.pagedata) {

                JiShi.Model.js_item model = new JiShi.Model.js_item();
                model.closed = item.closed.ToString();
                model.createDate = item.createDate.ToString();

                //if (!Bll.Exists(model.id)) {
                //    LogManager.WriteLog("保存：" + Bll.Add(model));
                //}

            }

            Console.WriteLine(js1.pageInfo.pageId + "    " + js1.pageInfo.totalPages + "    ");
            //Console.Write(js1);
            //Console.Write(JsonConvert.SerializeObject(js1));
            //var s1="\"closed\":false,\"createDate\":\",\"curId\":18,"gameId":"","gameItemId":"","gender":"","grade":0,"gradeName":"","guild":"","iconPath":"http://180.153.139.98/res/money/yb.png","id":1518894,"itemAmount":1000000,"itemCode":"CapitalType2","itemDesc":"","itemName":"银子","itemType":"2199","power":"","price":88,"publicityEndDate":"2014-09-12 08:46:09","returnDate":"","saveTime":"2014-09-12 08:46:09","sellerCasId":"","sellerGameId":"B347C67E475948C582DFCE392B02AAA7","sellerRole":"郎纵横四海","serverId":"7100046","shelfDate":"2014-09-12 08:46:09","shelfDays":13,"status":"已上架","unitPrice":0}";
        }

        [Test]
        public void t3() {
            JishiObject js2 = new JishiObject();
            js2.pageInfo = new pageInfo1() { pageId = 1 };
            js2.searchParams = new searchParams1() { excludeItemId = "1" };
            js2.pagedata = new List<pageData>() { new pageData() { closed = "", itemDesc = "21" }, new pageData() { closed = "", itemDesc = "21" }, };
            var js2_str = JsonConvert.SerializeObject(js2);
            Console.WriteLine(js2_str);
        }
    }
}