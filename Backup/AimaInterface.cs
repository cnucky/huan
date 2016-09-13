using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aimaInterface
{
    /// <summary>
    /// 爱码接口类
    /// </summary>
    public class AimaInterface
    {
        private HttpTool send = new HttpTool();
        private static AimaInterface instance = new AimaInterface();
        private String url = "http://api.f02.cn:8888/http.do";

        public static AimaInterface getInstance()
        {
            return instance;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public LogonResp loginIn(String uid, String pwd)
        {
            LogonResp resp = new LogonResp();
            string result = "";
            try
            {
                result = send.HttpPost(url, "action=loginIn&uid=" + uid + "&pwd=" + pwd);
                resp.Result = result;
                String[] reset = result.Split('|');

                if (reset.Length >= 2 && uid.Equals(reset[0]))
                {

                    resp.State = true;
                    resp.Uid = reset[0];
                    resp.Token = reset[1];
                }
                else
                {
                    resp.State = false;
                    resp.Result = result;
                }

                info("登录，账号：" + uid + ",密码：" + pwd + ",返回:"
                + result);
            }
            catch (Exception e)
            {
                error("登录异常，账号：" + uid + ",密码：" + pwd + ",e:"
                + e);
                resp.State = false;
            }
            return resp;
        }



        /// <summary>
        /// 获取一个手机号
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public GetMobilenumResp getMobilenum(int pid, String uid, String token)
        {
            GetMobilenumResp resp = new GetMobilenumResp();
            String result = "";
            try
            {
                result = send.HttpPost(url, "action=getMobilenum&uid="
                        + uid + "&token=" + token + "&pid=" + pid);
                info("获取一个手机号，账号：" + uid + ",token：" + token
                + ",pid:" + pid
                + ",返回:" + result);

                String[] reset = result.Split('|');

                Regex regex = new Regex("\\d");
                if (reset.Length >= 2 && regex.Match(reset[0]).Success)
                {

                    resp.State = true;
                    resp.Mobile = reset[0];
                    resp.Result = result;

                }
                else
                {
                    resp.State = false;
                    resp.Result = result;
                }

            }
            catch (Exception e)
            {
                error("获取一个手机号，账号：" + uid + ",token：" + token
                + ",pid:" + pid + ",e=" + e.ToString());
                resp.State = false;
            }
            return resp;
        }





        /// <summary>
        /// 获取多个手机号
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public GetMultiMobilenumResp getMobilenum(int pid, String uid, String token,
                int size)
        {
            GetMultiMobilenumResp resp = new GetMultiMobilenumResp();
            String result = "";
            try
            {
                result = send.HttpPost(url, "action=getMobilenum&uid="
                        + uid + "&token=" + token + "&pid=" + pid + "&size=" + size);
                info("获取一个手机号，账号：" + uid + ",token：" + token
                + ",pid:" + pid
                + ",返回:" + result);

                String[] reset = result.Split('|');

                Regex regex = new Regex("\\d");
                if (reset.Length >= 2)
                {

                    String[] mobile = reset[0].Split(';');

                    if (!regex.Match(mobile[0]).Success)
                    {
                        resp.State = false;
                    }
                    else
                    {
                        resp.State = true;
                        resp.Mobile = mobile;
                    }

                    resp.Result = result;

                }
                else
                {
                    resp.State = false;
                    resp.Result = result;
                }

            }
            catch (Exception e)
            {
                error("获取一个手机号，账号：" + uid + ",token：" + token
                + ",pid:" + pid + ",e="
                + e);
                resp.State = false;
            }
            return resp;
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
                String uid, String token, String next_pid, String author_uid)
        {
            GetVcodeAndHoldMobilenumResp resp = new GetVcodeAndHoldMobilenumResp();
            String result = "";
            try
            {
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
                if (reset.Length >= 2 && isNumber(reset[0]))
                {
                    resp.State = true;
                    resp.Mobile = reset[0];
                    resp.VerifyCode = reset[1];
                    resp.Result = result;

                }
                else
                {
                    resp.State = false;
                    resp.Result = result;
                }

            }
            catch (Exception e)
            {
                error("获取验证码并继续使用这个手机号，mobile:" + mobile + ",uid：" + uid
                + ",token：" + token +
                ",next_pid:" + next_pid + ",author_uid:" + author_uid + ",e:" +
                e);
                resp.State = false;
            }
            return resp;
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
                String uid, String token, String author_uid)
        {
            GetVcodeAndReleaseMobileResp resp = new GetVcodeAndReleaseMobileResp();
            String result = "";
            try
            {
                result = send.HttpPost(url,
                        "action=getVcodeAndReleaseMobile&uid=" + uid + "&token="
                                + token + "&mobile=" + mobile + "&author_uid="
                                + author_uid);

                String[] reset = result.Split('|');
                if (reset.Length >= 2 && isNumber(reset[0]))
                {
                    resp.State = true;
                    resp.Mobile = reset[0];
                    resp.VerifyCode = reset[1];
                    resp.Result = result;

                }
                else
                {
                    resp.State = false;
                    resp.Result = result;
                }

                info(
                "获取验证码并不再使用这个手机号，mobile:" + mobile + ",uid："
                + uid + ",token：" + token + ",author_uid:" + author_uid + ",返回:"
                + result + ",state:" + resp.State + ",mobile:" + resp.Mobile);

            }
            catch (Exception e)
            {
                error(
                "获取验证码并不再使用这个手机号，mobile:" + mobile + ",uid："
                + uid + ",token：" + token + ",author_uid:" + author_uid + ",e:" +
                e);
                resp.State = false;
            }
            return resp;
        }

        /// <summary>
        /// 添加若干手机号到黑名单,可用于网站对此手机号的使用次数进行了限制
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="mobiles"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public AddIgnoreListResp addIgnoreList(int pid, String mobiles,
                String uid, String token)
        {
            AddIgnoreListResp resp = new AddIgnoreListResp();
            String result = "";

            try
            {
                result = send.HttpPost(url,
                        "action=addIgnoreList&uid=" + uid + "&token=" + token
                                + "&mobiles=" + mobiles + "&pid=" + pid);
                info("添加若干手机号到黑名单，mobiles:" + mobiles + ",uid：" + uid +
                ",token：" + token + ",pid:" + pid + ",返回:"
                + result);

                if (isNumber(result))
                {
                    resp.State = true;
                    resp.Row = int.Parse(result);
                    resp.Result = result;

                }
                else
                {
                    resp.State = false;
                    resp.Result = result;
                }

            }
            catch (Exception e)
            {
                error("添加若干手机号到黑名单，mobiles:" + mobiles + ",uid：" + uid +
                ",token：" + token + ",pid:" + pid + ",e:" + e);
                resp.State = false;
            }
            return resp;
        }

        /// <summary>
        /// 取消一个短信接收，可立即解锁被锁定的金额
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public CancelSMSRecvResp cancelSMSRecv(String mobile, String uid,
                String token)
        {
            CancelSMSRecvResp resp = new CancelSMSRecvResp();
            String result = "";
            try
            {
                result = send.HttpPost(url,
                        "action=cancelSMSRecv&uid=" + uid + "&token=" + token
                                + "&mobile=" + mobile);
                info("取消一个短信接收，可立即解锁被锁定的金额，mobile:" + mobile + ",uid：" +
                uid + ",token：" + token + ",返回:"
                + result);

                if ("1".Equals(result))
                {
                    resp.State = true;
                    resp.Flag = result;
                    resp.Result = result;
                }
                else
                {
                    resp.State = false;
                    resp.Flag = result;
                    resp.Result = result;
                }

            }
            catch (Exception e)
            {
                error("取消一个短信接收，可立即解锁被锁定的金额，mobile:" + mobile + ",uid："
                + uid + ",token：" + token + ",e:" + e);
                resp.State = false;
            }
            return resp;
        }

        /// <summary>
        /// 取消一个短信接收，可立即解锁被锁定的金额
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public CancelSMSRecvResp cancelSMSRecvAll(String uid, String token)
        {
            CancelSMSRecvResp resp = new CancelSMSRecvResp();
            String result = "";

            try
            {
                result = send.HttpPost(url,
                        "action=cancelSMSRecvAll&uid=" + uid + "&token=" + token);
                info("取消所有短信接收，可立即解锁所有被锁定的金额，uid：" + uid + ",token："
                + token + ",返回:"
                + result);

                if ("1".Equals(result))
                {
                    resp.State = true;
                    resp.Flag = result;
                    resp.Result = result;
                }
                else
                {
                    resp.State = false;
                    resp.Flag = result;
                    resp.Result = result;
                }

            }
            catch (Exception e)
            {
                error("取消所有短信接收，可立即解锁所有被锁定的金额，uid：" + uid +
                ",token：" + token + ",e:" + e);
                resp.State = false;
            }
            return resp;
        }

        /// <summary>
        /// 获取当前用户正在使用的号码列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public String getRecvingInfo(String pid, String uid, String token)
        {
            // cancelSMSRecvResp resp = new cancelSMSRecvResp();
            String result = "";
            try
            {
                result = send.HttpPost(url,
                        "action=getRecvingInfo&uid=" + uid + "&token=" + token
                                + "&pid=" + pid);
                info("获取当前用户正在使用的号码列表，pid:" + pid + ",uid：" + uid + ",token：" + token +
                ",返回:"
                + result);

            }
            catch (Exception e)
            {
                error("获取当前用户正在使用的号码列表，pid:" + pid + ",uid：" + uid + ",token：" + token +
                ",e:" + e);
                // resp.setState(false);
            }
            return result;
        }


        Regex regex = new Regex("\\d");

        private bool isNumber(string str)
        {
            return regex.Match(str).Success;
        }


        private void info(string msg)
        {
            //Debug.WriteLine(msg);
        }

        private void error(string msg)
        {
            //Debug.WriteLine(msg);
        }

    }
}
