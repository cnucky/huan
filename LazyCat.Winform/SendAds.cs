using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
 

namespace Xilium.CefGlue.Client {
    public partial class SendAds : Form {
       
        public SendAds() {
            InitializeComponent();
        }

        WebBrowser wb = new WebBrowser();
       
//        private string ImTalkJs = @" function initImtalk() {
//            window.imTalk = imTalk;
//             function imTalk(uin, tid, sigT, sigP) {
//                var url = (tid) ? ('tencent://message/?uin=' + uin + '&fromuserid=' + tid + '&touserid=' + tid + '&unionid=72000106&WebSiteName=拍拍网&Service=19&sigT=' + sigT + '&sigU=' + sigP) : 'tencent://message/?uin=' + uin + '&fromuserid=no&touserid=no&unionid=72000106&WebSiteName=拍拍网&Service=19&sigT=' + sigT + '&sigU=' + sigP; var ua = window.navigator.userAgent.toLowerCase(); if (ua.indexOf('msie') != -1) { try { window.location.href = url;   } catch (e) {   showError(3); } } else if (/(firefox|safari|opera|chrome)/i.test(ua) || window.opera) { window.location.href = url;  } else {   showError(1); }
//                function showError(type) { switch (type) { case 1: alert('拍拍网温馨提示：\r\n　　您使用的浏览器不支持QQ临时会话功能，请使用IE/TT浏览器访问。'); break; case 2: alert('拍拍网温馨提示：\r\n　　您使用的QQ版本不支持临时会话功能，请您访问http://im.qq.com/下载最新版本QQ。'); window.open('http://im.qq.com/'); break; case 3: alert('拍拍网温馨提示：\r\n　　您没有安装QQ或您的浏览器设置禁止了QQ临时会话功能，请点击查看操作方法。'); window.target = '_top'; window.open('http://help.paipai.com/learn/aqkj/'); break; } }; return false;
//            };
//        }
//       initImtalk();
//        ";
//        /// <summary>
//        /// 修改打开方式
//        /// </summary>
//        private string ImTalkJs2 = @" function initImtalk() {
//            window.imTalk = imTalk;
//             function imTalk(uin, tid, sigT, sigP) {
//                var url = (tid) ? ('tencent://message/?uin=' + uin + '&fromuserid=' + tid + '&touserid=' + tid + '&unionid=72000106&WebSiteName=拍拍网&Service=19&sigT=' + sigT + '&sigU=' + sigP) : 
//              'tencent://message/?uin=' + uin + '&fromuserid=no&touserid=no&unionid=72000106&WebSiteName=拍拍网&Service=19&sigT=' + sigT + '&sigU=' + sigP; var ua = window.navigator.userAgent.toLowerCase(); if (ua.indexOf('msie') != -1) { try { window.open(url,'_blank')  ;   } catch (e) {   showError(3); } } else if (/(firefox|safari|opera|chrome)/i.test(ua) || window.opera) { window.location.href = url;  } else {   showError(1); }
//                function showError(type) { switch (type) { case 1: alert('拍拍网温馨提示：\r\n　　您使用的浏览器不支持QQ临时会话功能，请使用IE/TT浏览器访问。'); break; case 2: alert('拍拍网温馨提示：\r\n　　您使用的QQ版本不支持临时会话功能，请您访问http://im.qq.com/下载最新版本QQ。'); window.open('http://im.qq.com/'); break; case 3: alert('拍拍网温馨提示：\r\n　　您没有安装QQ或您的浏览器设置禁止了QQ临时会话功能，请点击查看操作方法。'); window.target = '_top'; window.open('http://help.paipai.com/learn/aqkj/'); break; } }; return false;
//            };
//        }
//       initImtalk();
//        ";
//        private string ImTalkAddiocn = "var iconlist = document.getElementsByClassName(\"icon-list\");for(var i=1;i<iconlist.length;i++){var alink1=iconlist[i].getElementsByTagName(\"a\");var uin=alink1[0].getAttribute('uin');alink1[0].innerHTML=\"<img cg='6-4' init_src='http://wpa.paipai.com/pa?p=1:\"+uin+\":17' src='http://wpa.paipai.com/pa?p=1:\"+uin+\":17' iguid='1'>\";}";
//        //http://shopsearch.paipai.com/SearchShopAction.xhtml?keyWord=&address=&orderStyle=5&pageSize=40&pageNum=2&totalPage=74&shoptype=20501&properties=0&level=0
        private string PPurl =
            "http://shopsearch.paipai.com/SearchShopAction.xhtml?keyWord=&address=&orderStyle=5&pageSize={2}&pageNum={0}&shoptype={1}&properties=0&level=0";
        private void BT_LoadUrls_Click(object sender, EventArgs e) {
            string pagenum = TB_pageNum.Text.Trim();
            string hycode = TB_HY_Code.Text.Trim();
            string shopCount = TB_ShopCount.Text.Trim();

            
            wb.Navigate(PPurl.With(pagenum, hycode, shopCount));
        }

         

        public void BT_Add_talk_Click(object sender, EventArgs e) {
             



        }
        private void displaylist(List<PpShop> tbzfbs, DataGridView dgv) {
            DataTable dt = new DataTable();
            dt.Columns.Add("index");
            dt.Columns.Add("qqnum");
            dt.Columns.Add("titile");

            dt.Rows.Clear();


            foreach (var tbTbzfb in tbzfbs) {
                DataRow dr = dt.NewRow();
                dr["index"] = tbTbzfb.index;
                dr["titile"] = tbTbzfb.titile;
                dr["qqnum"] = tbTbzfb.qqnum;
                dt.Rows.Add(dr);
            }
            dgv.DataSource = null;
            dgv.Rows.Clear();

            dgv.DataSource = dt;
        }

        private void BT_Analysis_html_Click(object sender, EventArgs e) {
            var f1 = File.ReadAllText(Application.StartupPath + "//tmp//pp.html");


            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(f1);
            //title[@lang='eng']	选取所有 title 元素，且这些元素拥有值为 eng 的 lang 属性。
            var alist = doc.DocumentNode.SelectNodes("//li[@class='item-attr-list']  ");//item-attr-list
            foreach (var htmlNode in alist) {
                Console.WriteLine(htmlNode.ChildAttributes("shop-title").FirstOrDefault().Value);

            }
        }

        private void BT_PP_Chat_Click(object sender, EventArgs e) {

            if (dataGridView1.SelectedRows.Count != 0) {

                string v1 = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                var shop = GlobalVar.shops.First(_ => _.qqnum == v1);
                PPhelper.PopQq(shop);

                if (shop.index + 1 > dataGridView1.Rows.Count) {
                    MessageBox.Show("这页发完了");
                }
                dataGridView1.Rows[shop.index + 1].Selected = true;
            } else {
                MessageBox.Show("选中一行开始");

            }



        }

        private void button1_Click(object sender, EventArgs e) {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(() => {
              
                //后台下载当前页面
              //  _mfForm.MainCefFrame.GetSource(new HtmlVisitDownLoader());
                wb.DocumentCompleted+= (s, e1) => {  //分析出QQ,将其列表显示
                    
                    displaylist(GlobalVar.shops, dataGridView1);//

                    //依次获取其在线状态

                    LB_Load.Text = "加载完成";
                };
                              
               
            
            }).Start();
        }

        private void BT_Qq_f5_Click(object sender, EventArgs e) {
            WebClient wcClient1 = new WebClient();

            //判断在线
            for (int i = 0; i < GlobalVar.shops.Count; i++) {
                GlobalVar.shops[i].isonline =
                    PPhelper.IsShopOnline(GlobalVar.shops[i]);
            }
            //移除不在线
            GlobalVar.shops.RemoveAll(_ => _.isonline == false);
            //重新排序
            for (int i = 0; i < GlobalVar.shops.Count; i++) {
                GlobalVar.shops[i].index = i;
            }
            //表现 分析出QQ,将其列表显示
            displaylist(GlobalVar.shops, dataGridView1);//
            LB_Load.Text = "移除不在线的完成";
        }


    }
}
