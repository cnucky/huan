using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.smsapi;
using PwBusiness.Model;

namespace PwBusiness {
    public static partial class GlobalVar {


        public const string Js2CsharpRequestDomain = "js2csharp.requestdomain";
        public const string base_js = @"function CreateXmlHttpRequest() {var xmlHttp = null;
            if (window.XMLHttpRequest) { xmlHttp = new XMLHttpRequest();}
            else if (window.ActiveXObject) { xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');}
            return xmlHttp;}
        function js2csharp( par) {
            var xmlHttp = CreateXmlHttpRequest();
            var para = '';
            xmlHttp.open('POST', 'http://js2csharp.requestdomain/1.aspx?'+par, false);
            xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xmlHttp.send(para);
            var result = xmlHttp.status;
            if (xmlHttp.readyState == 4) { if (result == 200) {var response = xmlHttp.responseText; }            }
        } ";
        /// <summary>
        /// 店铺动态评分  
        /// </summary>
        public const string haoPing_js =
            @"var kslist=document.getElementsByClassName('ks-simplestar');
            for(var i=0;i<kslist.length;i++){
                var imglist=kslist[i].getElementsByTagName('img'); 
                imglist[imglist.length-1].click();
            }";
        //public event something;
        public delegate void Js2Csahrp(string Statue, string method, string result);
        public static event EventHandler Js2CsahrpMsgEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statue"></param>
        /// <param name="method"></param>
        /// <param name="result"></param>
        public static void Notifyjs2Csahrp(string statue, string method, string result) {
            if (Js2CsahrpMsgEvent != null) {
                Js2CsahrpMsgEvent.Invoke(null, null);
            }

            //js2CsahrpMsgEvent.Invoke(Statue, method, result);
        }


        public static Dictionary<string, string> ExtractPar(string url1) {
            var par = url1.Split('?')[1];
            var par1 = par.Split('&');
            var d1 = new Dictionary<string, string>();
            foreach (var s in par1) {
                d1.Add(s.Split('=')[0], s.Split('=')[1]);
                Console.WriteLine(s.Split('=')[0] + " " + s.Split('=')[1]);
            }
            return d1;
        }



        public static Queue<string> ConfirmGoodsUrls = new Queue<string>();
        public static Queue<string> RateGoodsUrls = new Queue<string>();

        #region 163注册

        public static string Wy163Reg_enter_1 =
            "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163mail";// "http://reg.email.163.com/mailregAll/reg0.jsp?from=163mail";
        public static string Wy163Reg_Vcode_2 = "http://reg.zfbEmail.163.com/unireg/call.do";
        public static string Wy163Reg_Mian_3 = "http://cwebmail.mail.163.com/js5/main.jsp";
        #endregion
        #region 支付宝注册

        #endregion
        //https://memberprod.alipay.com/account/reg/email.htm
        public static string zfb_Reg_enter_1 = "https://memberprod.alipay.com/account/reg/email.htm";//https://memberprod.alipay.com/account/reg/index.htm";
        public static string zfb_Reg_linkcomplete = "https://memberprod.alipay.com/account/reg/regComplete.htm";


        public static string activZfbLink;
        public static string CurrentHtml = "";

        public static AutoResetEvent PP_analysis_AutoResetEvent = new AutoResetEvent(false);
        public static AutoResetEvent IsloadOkAutoResetEvent = new AutoResetEvent(false);
        public static AutoResetEvent IscookiesAutoResetEvent = new AutoResetEvent(false);
        public static string zfb_TB_Reg_Url = "https://my.alipay.com/portal/account/index.htm";
        public static string zfb_Reg_sucess = "https://memberprod.alipay.com/account/reg/success.htm";
        public static string zfb_Reg_paymethod = "https://zht.alipay.com/asset/paymethod/paymethod.htm";
        /// <summary>
        /// 登录淘宝的页面，（必须已经登录了支付宝）
        /// </summary>
        public static string zfb_reg_taobao_open = "https://login.taobao.com/member/login.jhtml";
        /// <summary>
        /// 第八页 进入淘宝-注册页面
        /// </summary>
        public static string zfb_reg_taobao_new_alipay_q = "http://reg.taobao.com/member/new_alipay_q.jhtml";
        public static string ZFB_REG_bindassetcard = "https://benefitprod.alipay.com/asset/paymethod/bindassetcard.htm";
        public static string zfb_reg_skip_bindassetcard = "https://my.alipay.com/portal/account/index.htm";

        public static string ZFB_REG_TB_LOGIN = "https://login.taobao.com/member/login.jhtml";

        public static string zfb_reg_account_reg_success = "https://memberprod.alipay.com/account/reg/success.htm";
        public static string smsName;
        public static string SmsPwd;
        public static string zfb_reg_taobao_open_error = "321aaaaaaa3123";

        #region 添加密保
        public static string zfb_reg_nav2set_SecurityQuestion = "https://accounts.alipay.com/console/selectStrategy.htm?sp=1-addSecurityQuestion-fullpage&strategy=payment_password";
        public static string zfb_reg_add_SecurityQuestion = "https://accounts.alipay.com/console/selectStrategy.htm";

        public static string zfb_reg_setQa = "https://accounts.alipay.com/console/qasecure/setQa.htm";
        public static string zfb_reg_add_SecurityQuestion_setQa_confirm = "https://accounts.alipay.com/console/qasecure/setQa.htm";
        public static string zfb_reg_queryStrategy = "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-addSecurityQuestion-fullpage&scene_code=addSecurityQuestion";
        public static string zfb_reg_nav2set_addSecurityQuestion = "https://accounts.alipay.com/console/dispatch.htm?scene_code=addSecurityQuestion&page_type=fullpage&site=1";
        public static string zfb_reg_queryStrategy2 = "https://accounts.alipay.com/console/queryStrategy.htm?site=1&page_type=fullpage&sp=1-addSecurityQuestion-fullpage&scene_code=addSecurityQuestion";
        public static int ChangeIpNum;
        public static bool autoRun;
        public static bool ChangeIp;
        public static int RegIntervalSeconds;
        public static string DoneFileName;
        public static tbModel tbmode;
        /// <summary>
        /// 1000 * 60 * 5;
        /// </summary>
        public static double DeadLineOfChangeClearAndPrepareNext = 1000 * 60 * 5;
        public static string jsTmpValue="";

        #endregion
        public static List<PP.PpShop> shops { get; set; }


        //

        public static List<tb_tbzfb> AccountList = new List<tb_tbzfb>();

        //###
        public static IMySms sms;
        public static string phoneNum;
        public static string CurrentIp;

        public static bool debug {
            get {
                return ConfigHelper.GetBoolValue("debug");
            }
        }

        public static string availabePhone;
    }


}

