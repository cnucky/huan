using System;
using System.Threading;
using System.Windows.Forms;
using Common;
using Common.smsapi;
using PwBusiness;

namespace Xilium.CefGlue.Client {
    public class SmsConfigHelper {


        public static void GetConfigOfSms() {
            var configstrlist = PwBusiness.FileHelper.read(Application.StartupPath + "//config//shoujiconfig.txt");
            GlobalVar.smsName = configstrlist[0];
            GlobalVar.SmsPwd = configstrlist[1];
            for (int i = 0; i < 3; i++) {
                if (!SmsApi.logined) {
                    SmsApi.logined = SmsApi.Login(GlobalVar.smsName, GlobalVar.SmsPwd);
                    //  SmsApi.Login
                }
            }
        }
        private static AutoResetEvent msgAutoResetEvent = new AutoResetEvent(true);



        public static bool GetSmsOfPhone(string phone, ref  string Vcode, string serverid) {
            string res = "";
            bool isRevicedSmsCode = false;
            //释放一次
            LogManager.WriteLog("msgAutoResetEvent 释放");
            msgAutoResetEvent.Set();

            LogManager.WriteLog("msgAutoResetEvent 阻塞");
            msgAutoResetEvent.Reset();


            new Thread(() => {
                int ReTryTelCount = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceCount"));
                int ReTryTelCountInterTime = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceInterTime"));
                for (int i = 0; i < ReTryTelCount; i++) {
                    Thread.Sleep(ReTryTelCountInterTime * 1000);
                    res = SmsApi.GetMessage(serverid, phone);
                    //您于2014年05月11日申请了手机验证,校验码是295768。如非本人操作,请拨0571-88158198【淘宝网】
                    if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                        isRevicedSmsCode = true;
                        msgAutoResetEvent.Set();
                        LogManager.WriteLog("msgAutoResetEvent 释放 取到验证码");
                        break;
                    } else {
                        LogManager.WriteLog("没有取到验证码 等待时间5秒钟  剩余等待次数{0} "
                            .With(ReTryTelCount - i));
                    }
                }

                if (!isRevicedSmsCode) {
                    LogManager.WriteLog("msgAutoResetEvent 释放.释放手机{0}".With(phone));
                    SmsApi.ReleasePhone(phone, serverid);
                    msgAutoResetEvent.Set();
                }


            }).Start();


            LogManager.WriteLog("msgAutoResetEvent 等待");
            msgAutoResetEvent.WaitOne();
            LogManager.WriteLog("msgAutoResetEvent 继续 ");
            //SmsApi.ReleasePhone(sender.ToString(), "2");

            if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                //  TB_TB_sms_res.Text = res;
                int index1 = res.IndexOf("是") + 1;
                Vcode = res.Substring(index1, 6);

            }
            return isRevicedSmsCode;
        }
        /// <summary>
        /// 获取指定的短信，模式1·获取并释放
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="vcodeNum"></param>
        /// <param name="smsServer"></param>
        /// <returns></returns>
        public static bool GetSmsOfPhone(string phoneNum, ref string vcodeNum, Common.smsapi.SmsServer smsServer) {
            string res = "";
            bool isRevicedSmsCode = false;
            //释放一次
            LogManager.WriteLog("msgAutoResetEvent 释放");
            msgAutoResetEvent.Set();

            LogManager.WriteLog("msgAutoResetEvent 阻塞");
            msgAutoResetEvent.Reset();


            new Thread(() => {
                int ReTryTelCount = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceCount"));
                int ReTryTelCountInterTime = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceInterTime"));
                for (int i = 0; i < ReTryTelCount; i++) {
                    Thread.Sleep(ReTryTelCountInterTime * 1000);
                    res = GlobalVar.sms.GetMessage(smsServer, phoneNum);//res = SmsApi.GetMessage(serverid, phone);
                    //您于2014年05月11日申请了手机验证,校验码是295768。如非本人操作,请拨0571-88158198【淘宝网】
                    if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                        isRevicedSmsCode = true;
                        msgAutoResetEvent.Set();
                        LogManager.WriteLog("msgAutoResetEvent 释放 取到验证码");
                        break;
                    } else {
                        LogManager.WriteLog("没有取到验证码 等待时间{1}秒钟  剩余等待次数{0} "
                            .With(ReTryTelCount - i, ReTryTelCountInterTime));
                    }
                }

                if (!isRevicedSmsCode) {
                    LogManager.WriteLog("msgAutoResetEvent 释放.释放手机{0}".With(phoneNum));
                    GlobalVar.sms.ReleasePhone(phoneNum, smsServer); //SmsApi.ReleasePhone(phone, serverid);
                    msgAutoResetEvent.Set();
                }


            }).Start();


            LogManager.WriteLog("msgAutoResetEvent 等待");
            msgAutoResetEvent.WaitOne();
            LogManager.WriteLog("msgAutoResetEvent 继续 ");
            //SmsApi.ReleasePhone(sender.ToString(), "2");

            if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                //  TB_TB_sms_res.Text = res;
                int index1 = res.IndexOf("是") + 1;
                vcodeNum = res.Substring(index1, 6);

            }
            return isRevicedSmsCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="vcodeNum"></param>
        /// <param name="smsServer"></param>
        /// <returns></returns>
        public static bool GetSmsOfPhoneAndHoldMobilenum(string phoneNum, ref string vcodeNum, Common.smsapi.SmsServer smsServer) {
            string res = "";
            bool isRevicedSmsCode = false;
            //释放一次
            LogManager.WriteLog("msgAutoResetEvent 释放");
            msgAutoResetEvent.Set();

            LogManager.WriteLog("msgAutoResetEvent 阻塞");
            msgAutoResetEvent.Reset();


            new Thread(() => {
                int ReTryTelCount = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceCount"));
                int ReTryTelCountInterTime = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceInterTime"));
                for (int i = 0; i < ReTryTelCount; i++) {
                    Thread.Sleep(ReTryTelCountInterTime * 1000);
                    res = GlobalVar.sms.getVcodeAndHoldMobilenum(smsServer, phoneNum, "3917");//支付宝手机解绑//res = SmsApi.GetMessage(serverid, phone);
                    //您于2014年05月11日申请了手机验证,校验码是295768。如非本人操作,请拨0571-88158198【淘宝网】
                    if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                        isRevicedSmsCode = true;
                        msgAutoResetEvent.Set();
                        LogManager.WriteLog("msgAutoResetEvent 释放 取到验证码");
                        break;
                    } else {
                        LogManager.WriteLog("没有取到验证码 等待时间5秒钟  剩余等待次数{0} "
                            .With(ReTryTelCount - i));
                    }
                }

                if (!isRevicedSmsCode) {
                    LogManager.WriteLog("msgAutoResetEvent 释放.释放手机{0}".With(phoneNum));
                    GlobalVar.sms.ReleasePhone(phoneNum, smsServer); //SmsApi.ReleasePhone(phone, serverid);
                    msgAutoResetEvent.Set();
                }


            }).Start();


            LogManager.WriteLog("msgAutoResetEvent 等待");
            msgAutoResetEvent.WaitOne();
            LogManager.WriteLog("msgAutoResetEvent 继续 ");
            //SmsApi.ReleasePhone(sender.ToString(), "2");

            if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                //  TB_TB_sms_res.Text = res;
                int index1 = res.IndexOf("是") + 1;
                vcodeNum = res.Substring(index1, 6);

            }
            return isRevicedSmsCode;
        }


        /// <summary>
        /// 获取短信验证码·并持有
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="vcodeNum"></param>
        /// <param name="smsServer"></param>
        /// <returns></returns>
        public static bool GetSmsOfPhoneTBREGV3(string phoneNum, ref string vcodeNum, Common.smsapi.SmsServer smsServer) {
            string res = "";
            bool isRevicedSmsCode = false;
            //释放一次
            LogManager.WriteLog("msgAutoResetEvent 释放");
            msgAutoResetEvent.Set();

            LogManager.WriteLog("msgAutoResetEvent 阻塞");
            msgAutoResetEvent.Reset();


            new Thread(() => {
                int ReTryTelCount = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceCount"));
                int ReTryTelCountInterTime = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceInterTime"));
                for (int i = 0; i < ReTryTelCount; i++) {
                    Thread.Sleep(ReTryTelCountInterTime * 1000);
                    res = GlobalVar.sms.getVcodeAndHoldMobilenum(smsServer, phoneNum, "868");//868=AIMA,淘宝|旺旺登录身份安全验证
                    //您于2014年05月11日申请了手机验证,校验码是295768。如非本人操作,请拨0571-88158198【淘宝网】
                    if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                        isRevicedSmsCode = true;
                        msgAutoResetEvent.Set();
                        LogManager.WriteLog("msgAutoResetEvent 释放 取到验证码");
                        break;
                    } else {
                        LogManager.WriteLog("没有取到验证码 等待时间{1}秒钟  剩余等待次数{0} "
                            .With(ReTryTelCount - i, ReTryTelCountInterTime));
                    }
                }

                if (!isRevicedSmsCode) {
                    LogManager.WriteLog("msgAutoResetEvent 释放.释放手机{0}".With(phoneNum));
                    try {
                        GlobalVar.sms.ReleasePhone(phoneNum, smsServer); //SmsApi.ReleasePhone(phone, serverid);
                    } catch (Exception e1) {

                        LogManager.WriteLog(e1.ToString());
                    }



                    msgAutoResetEvent.Set();
                }


            }).Start();


            LogManager.WriteLog("msgAutoResetEvent 等待");
            msgAutoResetEvent.WaitOne();
            LogManager.WriteLog("msgAutoResetEvent 继续 ");
            //SmsApi.ReleasePhone(sender.ToString(), "2");

            if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                //  TB_TB_sms_res.Text = res;
                int index1 = res.IndexOf("是") + 1;
                vcodeNum = res.Substring(index1, 6);

            }
            return isRevicedSmsCode;
        }


        /// <summary>
        /// TB REG V3 获取 并释放 等会重新获取
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="vcodeNum"></param>
        /// <param name="smsServer"></param>
        /// <returns></returns>
        public static bool GetSmsOfPhoneTBREGV3_1(string phoneNum, ref string vcodeNum, Common.smsapi.SmsServer smsServer) {
            string res = "";
            bool isRevicedSmsCode = false;
            //释放一次
            LogManager.WriteLog("msgAutoResetEvent 释放");
            msgAutoResetEvent.Set();

            LogManager.WriteLog("msgAutoResetEvent 阻塞");
            msgAutoResetEvent.Reset();


            new Thread(() => {
                int ReTryTelCount = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceCount"));
                int ReTryTelCountInterTime = int.Parse(ConfigHelper.GetValue("ReTrySingleTelReviceInterTime"));
                for (int i = 0; i < ReTryTelCount; i++) {
                    Thread.Sleep(ReTryTelCountInterTime * 1000);
                    res = GlobalVar.sms.GetMessage(smsServer, phoneNum);//868=AIMA,淘宝|旺旺登录身份安全验证
                    //您于2014年05月11日申请了手机验证,校验码是295768。如非本人操作,请拨0571-88158198【淘宝网】
                    if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                        isRevicedSmsCode = true;
                        msgAutoResetEvent.Set();
                        LogManager.WriteLog("msgAutoResetEvent 释放 取到验证码");
                        break;
                    } else {
                        LogManager.WriteLog("没有取到验证码 等待时间{1}秒钟  剩余等待次数{0} "
                            .With(ReTryTelCount - i, ReTryTelCountInterTime));
                    }
                }

                if (!isRevicedSmsCode) {
                    LogManager.WriteLog("msgAutoResetEvent 释放.释放手机{0}".With(phoneNum));
                    try {
                        GlobalVar.sms.ReleasePhone(phoneNum, smsServer); //SmsApi.ReleasePhone(phone, serverid);
                    } catch (Exception e1) {

                        LogManager.WriteLog(e1.ToString());
                    }



                    msgAutoResetEvent.Set();
                }


            }).Start();


            LogManager.WriteLog("msgAutoResetEvent 等待");
            msgAutoResetEvent.WaitOne();
            LogManager.WriteLog("msgAutoResetEvent 继续 ");
            //SmsApi.ReleasePhone(sender.ToString(), "2");

            if (!string.IsNullOrEmpty(res) && res.Contains("校验码是")) {
                //  TB_TB_sms_res.Text = res;
                int index1 = res.IndexOf("是") + 1;
                vcodeNum = res.Substring(index1, 6);

            }
            return isRevicedSmsCode;
        }

        /// <summary>
        /// 获取 邮箱注册 手机号·不持有
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static bool GetPhone(ref string phoneNum) {

            bool isok = false;
            int waitPhoneTime = int.Parse(ConfigHelper.GetValue("waitPhoneTime"));
            for (int i = 0; i < 5; i++) {
                phoneNum = GlobalVar.sms.GetPhone(SmsServer.tb_reg_vode);// SmsApi.GetPhone("2");
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
        /// 用于tbv3·模式1
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static bool GetPhone(ref string phoneNum,SmsServer ssServer) {

            bool isok = false;
            int waitPhoneTime = int.Parse(ConfigHelper.GetValue("waitPhoneTime"));
            for (int i = 0; i < 5; i++) {
                phoneNum = GlobalVar.sms.getOneSpecialMobilenum(ssServer, phoneNum);// SmsApi.GetPhone("2");
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
    }
}