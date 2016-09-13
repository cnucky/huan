using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using aimaInterface;
using NUnit.Framework;

namespace Common.smsapi {

    public class IMySmsFactory {
        public static IMySms Build(string type) {
            if (type == "jike") {
                LogManager.WriteLog("jike");
                return new JikeSms();
            } else if (type == "aima") {
                LogManager.WriteLog("aima");
                return new AIMASms();
            } else {
                return null;
            }
        }
    }

    public interface IMySms {
        //1. 加载配置
        //2.根据服务ID 获取手机号码
        //3. 获取短信
        //4. 释放手机
        void GetConfigOfSms();
        string GetPhone(SmsServer smstype);
        string GetMessage(SmsServer smstype, string phone);
        void ReleasePhone(string phone, SmsServer smstype);
        string ConvertServerId2(SmsServer smstype);
        string getVcodeAndHoldMobilenum(SmsServer smstype, string phone, string nextpid);
        string getOneSpecialMobilenum(SmsServer smstype, string mobile);
        /// <summary>
        /// 获取一个指定的手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="smstype"></param>
        /// <returns></returns>
        string GetPhone(string phone, SmsServer smstype);
    }

    public enum SmsServer {
        zfb_reg_vcode,
        tb_reg_vode,
        zfb_jiebang,
        tb_reg_login_vcode
    }

    public class AIMASms : IMySms {
        private LogonResp logonresp = null;
        private string uid;
        private string author_uid = "";
        private HttpTool send = new HttpTool();
        private static AimaInterface instance = new AimaInterface();
        private String url = "http://api.f02.cn:8888/http.do";

        public string ConvertServerId2(SmsServer smstype) {
            if (smstype == SmsServer.zfb_reg_vcode) {
                return "1245";
            } else if (smstype == SmsServer.tb_reg_vode) {
                return "27";
            } else if (smstype == SmsServer.tb_reg_login_vcode) {
                return "863";


            }
            return "";


        }


        public void GetConfigOfSms() {
            uid = ConfigHelper.GetValue("aimaUid");
            var pwd = ConfigHelper.GetValue("aimaPwd");

            LogManager.WriteLog(uid + "  " + pwd);
            logonresp = instance.loginIn(uid, pwd);
            LogManager.WriteLog(logonresp.Result);

        }
        [Test]
        public void TestGetPhone() {
            GetConfigOfSms();
            Console.WriteLine(GetPhone(SmsServer.zfb_reg_vcode));
            ;
        }
 

        public string getOneSpecialMobilenum(SmsServer smstype, string mobile) {

            GetMobilenumResp resp = instance
                                .getOneSpecialMobilenum(int.Parse(ConvertServerId2(smstype)), uid, logonresp.Token,mobile);

            // 取到一个号码
            if (resp.State) {
                LogManager.WriteLog("获取到一个手机号码：" + resp.Mobile);
                return resp.Mobile;

            } else {
                LogManager.WriteLog("未获取到 号码 " + resp.Result.ToString());
                if ("message|速度过快，请稍后再试".Equals(resp.Result)
                        || "message|please try again later".Equals(resp
                                .Result)) {
                    Thread.Sleep(1000);

                } else if ("max_count_disable".Equals(resp.Result)) {
                    Thread.Sleep(2000);

                }
            }
            return "";
        }

        public string GetPhone(string phone, SmsServer smstype) {
            GetMobilenumResp resp = instance
                                 .getOneSpecialMobilenum(int.Parse(ConvertServerId2(smstype)), uid, logonresp.Token,phone);
    
            // 取到一个号码
            if (resp.State) {
                LogManager.WriteLog("获取到一个手机号码：" + resp.Mobile);
                return resp.Mobile;

            } else {
                LogManager.WriteLog("未获取到 号码 " + resp.Result.ToString());
                if ("message|速度过快，请稍后再试".Equals(resp.Result)
                        || "message|please try again later".Equals(resp
                                .Result)) {
                    Thread.Sleep(1000);

                } else if ("max_count_disable".Equals(resp.Result)) {
                    Thread.Sleep(2000);

                }
            }


            return "";
        }

        public string GetPhone(SmsServer smstype) {

            GetMobilenumResp resp = instance
                                .getMobilenum(int.Parse(ConvertServerId2(smstype)), uid, logonresp.Token);

            // 取到一个号码
            if (resp.State) {
                LogManager.WriteLog("获取到一个手机号码：" + resp.Mobile);
                return resp.Mobile;

            } else {
                LogManager.WriteLog("未获取到 号码 " + resp.Result.ToString());
                if ("message|速度过快，请稍后再试".Equals(resp.Result)
                        || "message|please try again later".Equals(resp
                                .Result)) {
                    Thread.Sleep(1000);

                } else if ("max_count_disable".Equals(resp.Result)) {
                    Thread.Sleep(2000);

                }
            }
            return "";
        }

        public string GetMessage(SmsServer smstype, string phone) {

            GetVcodeAndReleaseMobileResp GetVRresp = new GetVcodeAndReleaseMobileResp();
            String result = "";
            try {
                result = send.HttpPost(url,
                    "action=getVcodeAndReleaseMobile&uid=" + uid + "&token="
                    + logonresp.Token + "&mobile=" + phone + "&author_uid="
                    + author_uid);

                String[] reset = result.Split('|');
                if (reset.Length >= 2 && isNumber(reset[0])) {
                    GetVRresp.State = true;
                    GetVRresp.Mobile = reset[0];
                    GetVRresp.VerifyCode = reset[1];
                    GetVRresp.Result = result;

                } else {
                    GetVRresp.State = false;
                    GetVRresp.Result = result;
                }
            } catch (Exception e1) {
                LogManager.WriteLog(e1.ToString());
            }

            if (GetVRresp.State) //获取到验证码，跳出
            {
                return GetVRresp.VerifyCode;
                //  break;
            } else {
                LogManager.WriteLog(GetVRresp.Result);
            }
            return "";
        }
        public string getVcodeAndHoldMobilenum(SmsServer smstype, string phone, string nextpid) {

            GetVcodeAndReleaseMobileResp GetVRresp = new GetVcodeAndReleaseMobileResp();
            String result = "";
            try {
                result = send.HttpPost(url,
                    "action=getVcodeAndHoldMobilenum&uid=" + uid + "&token="
                    + logonresp.Token + "&mobile=" + phone + "&author_uid="
                    + author_uid + "&next_pid=" + nextpid);



                String[] reset = result.Split('|');
                if (reset.Length >= 2 && isNumber(reset[0])) {
                    GetVRresp.State = true;
                    GetVRresp.Mobile = reset[0];
                    GetVRresp.VerifyCode = reset[1];
                    GetVRresp.Result = result;

                } else {
                    GetVRresp.State = false;
                    GetVRresp.Result = result;
                }
            } catch (Exception e1) {
                LogManager.WriteLog(e1.ToString());
            }

            if (GetVRresp.State) //获取到验证码，跳出
            {
                return GetVRresp.VerifyCode;
                //  break;
            } else {
                LogManager.WriteLog(GetVRresp.Result);
            }
            return "";
        }
        /// <summary>
        ///  获取验证码并继续使用这个手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <param name="next_pid"></param>
        /// <param name="author_uid"></param>
        /// <returns></returns>
        public GetVcodeAndHoldMobilenumResp getVcodeAndHoldMobilenum(String mobile,
                String uid, String token, String next_pid, String author_uid) {
            GetVcodeAndHoldMobilenumResp resp = new GetVcodeAndHoldMobilenumResp();
            String result = "";
            try {
                result = send.HttpPost(url,
                        "action=getVcodeAndHoldMobilenum&uid=" + uid + "&token="
                                + token + "&mobile=" + mobile + "&next_pid="
                                + next_pid + "&author_uid=" + author_uid);
                info("获取验证码并继续使用这个手机号，mobile:" + mobile + ",uid：" + uid
                + ",token：" + token +
                ",next_pid:" + next_pid + ",author_uid:" + author_uid + ",返回:"
                + result);

                // 返回值：发送号码|验证码|下次获取验证码的token(暂时无用)
                String[] reset = result.Split('|');
                if (reset.Length >= 2 && isNumber(reset[0])) {
                    resp.State = true;
                    resp.Mobile = reset[0];
                    resp.VerifyCode = reset[1];
                    resp.Result = result;

                } else {
                    resp.State = false;
                    resp.Result = result;
                }

            } catch (Exception e) {
                info("获取验证码并继续使用这个手机号，mobile:" + mobile + ",uid：" + uid
                + ",token：" + token +
                ",next_pid:" + next_pid + ",author_uid:" + author_uid + ",e:" +
                e);
                resp.State = false;
            }
            return resp;
        }
        public void ReleasePhone(string phone, SmsServer smstype) {
            var res = instance.addIgnoreList(int.Parse(ConvertServerId2(smstype)), phone, uid, logonresp.Token);
            LogManager.WriteLog(res.Result);
        }


        /// <summary>
        /// 获取验证码并不再使用这个手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <param name="author_uid"></param>
        /// <returns></returns>
        public GetVcodeAndReleaseMobileResp getVcodeAndReleaseMobile(String mobile,
                String uid, String token, String author_uid) {
            GetVcodeAndReleaseMobileResp resp = new GetVcodeAndReleaseMobileResp();
            String result = "";
            try {
                result = send.HttpPost(url,
                        "action=getVcodeAndReleaseMobile&uid=" + uid + "&token="
                                + token + "&mobile=" + mobile + "&author_uid="
                                + author_uid);

                String[] reset = result.Split('|');
                if (reset.Length >= 2 && isNumber(reset[0])) {
                    resp.State = true;
                    resp.Mobile = reset[0];
                    resp.VerifyCode = reset[1];
                    resp.Result = result;

                } else {
                    resp.State = false;
                    resp.Result = result;
                }

                info(
                "获取验证码并不再使用这个手机号，mobile:" + mobile + ",uid："
                + uid + ",token：" + token + ",author_uid:" + author_uid + ",返回:"
                + result + ",state:" + resp.State + ",mobile:" + resp.Mobile);

            } catch (Exception e) {
                info(
                "获取验证码并不再使用这个手机号，mobile:" + mobile + ",uid："
                + uid + ",token：" + token + ",author_uid:" + author_uid + ",e:" +
                e);
                resp.State = false;
            }
            return resp;
        }
        private void info(string msg) {
            LogManager.WriteLog(msg);
        }
        Regex regex = new Regex("\\d");

        private bool isNumber(string str) {
            return regex.Match(str).Success;
        }
    }
}