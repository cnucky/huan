using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aimaInterface
{
    class Program11
    {
        static void Main(string[] args)
        {
            #region 简单实例

            ////使用封装好的接口工具类调用接口
            //LogonResp rs = AimaInterface.getInstance().loginIn("uid", "pwd");

            //if (rs.State)
            //{
            //    Console.WriteLine("调用成功");
            //    Console.WriteLine(rs.Uid);
            //    Console.WriteLine(rs.Token);
            //    Console.WriteLine(rs.Result);
            //}
            //else
            //{
            //    Console.WriteLine("调用失败");
            //    Console.WriteLine(rs.Result);
            //}

            #endregion
            
            #region 多线程实例
            //要使用该多线程例子，请修改MultiThreadDemo.process方法，加入自己的业务代码

            String uid = "q2601598871";//用记ID		
            String pwd = "q2601598871";//密码		
            int pid = 1245;//项目ID
            String author_uid = "";//开发者用户名
            int sleeptime = 500;//取号休眠时间 				

            /*
             * 设置取验证码短信超时时间， 超过这个时间还未获取到验证码短信，将释放号码 单位 ：毫秒	
             */
            long getCodeTimeout = 1000 * 60 * 5; //5分钟 	

            //取号线程数
            int threadSize = 1;
            String token = "";
            LogonResp resp = AimaInterface.getInstance().loginIn(uid, pwd);

            if (resp.State)
            {
                token = resp.Token;
                Console.WriteLine(uid + "登录成功");

                //启动threadSize个取号码线程
                for (int i = 0; i < threadSize; i++)
                {
                    new MultiThreadDemo(token, uid, author_uid, pid, sleeptime, getCodeTimeout).start();
                }

            }
            else
            {
                Console.WriteLine("启动失败，" + uid + "登录失败！失败原因：" + resp.Result);

            }

            #endregion
            Console.ReadLine();
        }
    }
}
