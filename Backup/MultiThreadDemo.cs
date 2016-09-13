using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace aimaInterface
{
    /// <summary>
    /// 多线程取号取验证码例子
    /// </summary>
    public class MultiThreadDemo
    {

        String token = "";
        //用记ID
        String uid = "";
        //项目ID
        int pid;

        //取号休眠时间
        int sleeptime = 500;
       
        /**
         * 开发者用户名
         * (可选,传入此参数必须是注册为软件厂商的用户名，
         * 如果当前用户已经有上线则分提成金额的30%给开发者,
         * 如果没有上线则分提成金额的50%归开发者，具体请参考官网的积分说明)      
         */
        private String author_uid = "";

        /**
         * 设置取验证码短信超时时间， 超过这个时间还未获取到验证码短信，将释放号码 单位 ：毫秒	
         */
        long getCodeTimeout = 1000 * 60 * 5; //5分钟 	

        bool isrun = false;
        private Thread thread = null;

        public MultiThreadDemo(String token, String uid, String author_uid, int pid, int sleeptime, long getCodeTimeout)
        {
            this.token = token;
            this.author_uid = author_uid;
            this.uid = uid;
            this.pid = pid;
            this.sleeptime = sleeptime;
            this.getCodeTimeout = getCodeTimeout;
        }


        
        /// <summary>
        /// 启动线程
        /// </summary>
        public void start()
        {
            if (!isrun)
            {
                isrun = true;
                if (thread == null)
                {
                    thread = new Thread(run);
                }

                thread.IsBackground = true;
                thread.Start();
            }

        }


        public void run()
        {
            while (true)
            {
                try
                {
                    GetMobilenumResp resp = AimaInterface.getInstance()
                            .getMobilenum(pid, uid, token);

                    // 取到一个号码
                    if (resp.State)
                    {
                        Console.WriteLine("获取到一个手机号码：" + resp.Mobile);
                        process(resp.Mobile);

                    }
                    else
                    {
                        if ("message|速度过快，请稍后再试".Equals(resp.Result)
                                || "message|please try again later".Equals(resp
                                        .Result))
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        else if ("max_count_disable".Equals(resp.Result))
                        {
                            Thread.Sleep(2000);
                            continue;
                        }
                    }

                    Thread.Sleep(sleeptime);

                }
                catch (Exception e)
                {
                    error(e.ToString());
                }

            }
        }

        /**
         * 成功获到取号码后业务处理
         * @param mobile
         */
        public void process(String mobile)
        {
            ///////////请在这里加上你需要的业务代码/////////////
            //这里加上 从第三方平台申请下发验证码短信 代码		


            ///////////请在这里加上你需要的业务代码/////////////


            // 如果号码已被使用，调用addIgnore方法对号码进行加黑
            // addIgnore(mobile);


            // 确认第三方平台下发短信后，调getVcodeAndReleaseMobile接口获取验证码短信		

            try
            {
                Thread.Sleep(5000); //休眠5秒后开始调用
            }
            catch { }

            long start = DateTimeExtensions.currentTimeMillis(); //开始获取时间		
            GetVcodeAndReleaseMobileResp resp = null;
            while (DateTimeExtensions.currentTimeMillis() - start < getCodeTimeout) //如果获取验证码短信超过等待时间getCodeTimeout，将不再获取，并释放号码	
            {
                resp = getVcodeAndReleaseMobile(mobile);
                if (resp.State) //获取到验证码，跳出
                {
                    break;
                }
                else
                {
                    //Console.WriteLine("未获取到验证码");
                    try
                    {
                        Thread.Sleep(5000);
                    }
                    catch { }
                }

            }
            //成功获取短信验证码
            if (resp != null && resp.State)
            {
                Console.WriteLine("获取验证码成功，号码:" + resp.Mobile + ";验证码短信：" + resp.VerifyCode);
            }
            else //未获取到验证码，释放号码
            {
                Console.WriteLine("获取到验证码短信失败，释放号码" + mobile);
                cancelSMSRecv(mobile);
            }

        }

        /**
         * 加黑一个手机号码
         * @param mobile
         */
        private void addIgnore(String mobile)
        {
            int retry = 0;
            bool isretry = false;
            do
            {
                retry++;
                AddIgnoreListResp resp = AimaInterface.getInstance().addIgnoreList(pid, mobile, uid, token);
                if (!resp.State && ("message|速度过快，请稍后再试".Equals(resp.Result)
                        || "message|please try again later".Equals(resp
                                .Result))
                )
                {
                    isretry = true;
                    try
                    {
                        Thread.Sleep(500);
                    }
                    catch { }
                }
                else
                {
                    isretry = false;
                }

            } while (isretry && retry < 3); //失败重试三次

            Console.WriteLine("加黑一个手机号码:" + mobile);
        }

        /**
         * 释放一个手机号码
         * @param mobile
         */
        private void cancelSMSRecv(String mobile)
        {
            int retry = 0;
            bool isretry = false;
            do
            {
                retry++;
                CancelSMSRecvResp resp = AimaInterface.getInstance().cancelSMSRecv(mobile,
                        uid, token);
                if (!resp.State && ("message|速度过快，请稍后再试".Equals(resp.Result)
                        || "message|please try again later".Equals(resp
                                .Result)))
                {
                    isretry = true;
                    try
                    {
                        Thread.Sleep(500);
                    }
                    catch { }
                }
                else
                {
                    isretry = false;
                }

            } while (isretry && retry < 3); //失败重试三次


        }

        public GetVcodeAndReleaseMobileResp getVcodeAndReleaseMobile(String mobile)
        {
            return AimaInterface.getInstance().getVcodeAndReleaseMobile(mobile, uid, token, author_uid);
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
