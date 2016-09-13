using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace Common.Vcode {


    public class LianZhongVcode {
        /////命令功能:查询剩余验证码点数 
        /////strVcodeUser：联众账号
        /////strVcodePass：联众密码 
        /////return string 成功返回->剩余验证码点数 
        //[DllImport("FastVerCode.dll")]
        //private static extern string GetUserInfo(string strVcodeUser, string strVcodePass);


        /////命令功能:通过作者的下线注册联众账号 
        /////strUser:注册用户
        /////strPass:注册密码
        /////strEmail:注册邮箱
        /////strQQ:注册qq
        /////strAgentid：开发者软件id
        /////strAgentName:软件开发者账号id
        /////return int  1=成功;-1=网络传输异常;0=未知异常 
        //[DllImport("FastVerCode.dll")]
        //private static extern int Reglz(string strUser, string strPass, string strEmail, string strQQ, string strAgentid, string strAgentName);

        /////命令功能:通过上传验证码图片字节到服务器进行验证码识别，方便多线程发送 
        /////b:上传验证码图片字节集
        /////len:上传验证码图片字节集长度
        /////strVcodeUser：联众账号
        /////strVcodePass：联众密码
        /////成功返回->验证码结果|!|打码工人；后台没点数了返回:No Money! ；未注册返回:No Reg! ；上传验证码失败:Error:Put Fail!  ；识别超时了:Error:TimeOut!  ；上传无效验证码:Error:empty picture!  
        //[DllImport("FastVerCode.dll")]
        //private static extern string RecByte(byte[] b, int len, string strVcodeUser, string strVcodePass);

        /////通过上传验证码图片字节到服务器进行验证码识别，方便多线程发送,这个函数可以保护作者的收入
        /////b:上传验证码图片字节集
        /////len:上传验证码图片字节集长度
        /////strVcodeUser：联众账号
        /////strVcodePass：联众密码
        /////strAgentUser：软件开发者账号
        /////成功返回->验证码结果|!|打码工人；后台没点数了返回:No Money! ；未注册返回:No Reg! ；上传验证码失败:Error:Put Fail!  ；识别超时了:Error:TimeOut!  ；上传无效验证码:Error:empty picture!  
        //[DllImport("FastVerCode.dll")]
        //private static extern string RecByte_A(byte[] b, int len, string strVcodeUser, string strVcodePass, string strAgentUser);

        /////命令功能:通过发送验证码本地图片到服务器识别 
        /////strYZMPath：验证码本地路径，例如（c:\1.jpg)  
        /////strVcodeUser：联众账号
        /////strVcodePass：联众密码
        /////成功返回->验证码结果|!|打码工人；后台没点数了返回:No Money! ；未注册返回:No Reg! ；上传验证码失败:Error:Put Fail!  ；识别超时了:Error:TimeOut!  ；上传无效验证码:Error:empty picture!  
        //[DllImport("FastVerCode.dll")]
        //private static extern string RecYZM(string strYZMPath, string strVcodeUser, string strVcodePass);

        /////命令功能:通过发送验证码本地图片到服务器识别,这个函数可以保护作者的收入
        /////strYZMPath：验证码本地路径，例如（c:\1.jpg)  
        /////strVcodeUser：联众账号
        /////strVcodePass：联众密码
        /////strAgentUser：软件开发者账号
        /////成功返回->验证码结果|!|打码工人；后台没点数了返回:No Money! ；未注册返回:No Reg! ；上传验证码失败:Error:Put Fail!  ；识别超时了:Error:TimeOut!  ；上传无效验证码:Error:empty picture!  
        //[DllImport("FastVerCode.dll")]
        //private static extern string RecYZM_A(string strYZMPath, string strVcodeUser, string strVcodePass, string strAgentUser);

        /////命令功能:对打错的验证码进行报告。
        /////strVcodeUser：联众用户
        /////strDaMaWorker：打码工人
        /////返回值类型:空    无返回值
        //[DllImport("FastVerCode.dll")]
        //private static extern void ReportError(string strVcodeUser, string strDaMaWorker);


        /// <summary>
        /// 设置超时时间未12秒
        /// </summary>
        /// <param name="res"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        public static bool Vcode(out string res, out string tip) {
            //新开一个线程执行验证，如果超过指定时间未完成则 
            var strs = File.ReadAllLines("config//lianzhongyanzhengma.txt");

            string name = strs[0];
            string pwd = strs[1];
            string returnMess = "";
            var t1 = new Thread(
                  () => {
                      string path = Application.StartupPath + "\\tmp\\zhifubaoVcode.jpg";
                      var imageBytes = File.ReadAllBytes(path);
                      try {
                          returnMess = FastVerCode.VerCode.RecByte_A(imageBytes, imageBytes.Length, name, pwd, "809dae10b91d47c72f5a1bc5ffc837ec");
                      }
                      catch (Exception e) {
                          LogManager.WriteLog(e.ToString());
                           
                      }
                      

                  }
                  );
            t1.Start();
            Thread.Sleep(12000);
            if (t1.IsAlive) {
                t1.Abort();
            }

            res = "";

            if (returnMess.Equals("No Money!")) {
                tip = "点数不足";
            } else if (returnMess.Equals("No Reg!")) {
                tip = "没有注册";
            } else if (returnMess.Equals("Error:Put Fail!")) {
                tip = "上传验证码失败";
            } else if (returnMess.Equals("Error:TimeOut!")) {
                tip = "识别超时";
            } else if (returnMess.Equals("Error:empty picture!")) {
                tip = "上传无效验证码";
            } else {
                res = returnMess.Split('|')[0];
                tip = "成功";
                return true;
                // textBox6.Text = returnMess.Split('|')[2];
                //  MessageBox.Show("识别成功", "友情提示");
            }
            return false;
        }
    }
}