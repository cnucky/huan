using System.IO;
using Common;
using PwBusiness;
using PwBusiness.Model;
using Xilium.CefGlue.Client.SchemeHandler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace Xilium.CefGlue.Client {


    internal static class Program {

        [STAThread]
        private static int Main(string[] args1) {

            //自定义初始化系统配置
            InitAllinOne();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm.taskRunner = GlobalUI.taskRunner;
            GlobalVar.AccountList = HaoziHelper.importAccounts();
            //导入账号信息

            //每个账号建立一个cache
            string zfbEmail = "";

            zfbEmail = GlobalVar.AccountList.Count == 0 ? "downloadAccount" : GlobalVar.AccountList[0].zfbEmail;
            string MyCachePath = Application.StartupPath + "\\cache\\{0}\\".With(zfbEmail);
            if (!Directory.Exists(MyCachePath)) {
                Directory.CreateDirectory(MyCachePath);
            }

            //每个账号建立一个个性化信息配置文件
            string zfbEmailConfig = MyCachePath + zfbEmail + ".txt";
            string httpHeadAgent = FileHelper.RandomReadOneLine("Resources//httpHead.txt").ReplaceNum();
            if (!File.Exists(zfbEmailConfig)) {
                //  File.Create(zfbEmailConfig); //Directory.CreateDirectory(MyCachePath);
                File.WriteAllText(zfbEmailConfig, httpHeadAgent);
            } else {
                httpHeadAgent = File.ReadAllLines(zfbEmailConfig)[0];
            }
            GlobalCefGlue.UserAgent = httpHeadAgent;
            LogManager.WriteLog(httpHeadAgent + " " + MyCachePath);

            //
            string[] args = { "" };
            int main;
            if (cefruntimeLoad(out main))
                return main;
            var mainArgs = new CefMainArgs(args);
            var app = new DemoApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app);
            if (exitCode != -1)
                return exitCode;

            var settings = new CefSettings {
                // BrowserSubprocessPath = @"D:\fddima\Projects\Xilium\Xilium.CefGlue\CefGlue.Demo\bin\Release\Xilium.CefGlue.Demo.exe",
                SingleProcess = false,
                LogSeverity = CefLogSeverity.Disable,
                LogFile = "CefGlue.log",
                //设置缓存地址
                CachePath = MyCachePath,// Application.StartupPath + "\\cache\\{0}\\".With(zfbEmail),
                RemoteDebuggingPort = 20480,
                //  UserAgent = httpHeadAgent//FileHelper.RandomReadOneLine("Resources//httpHead.txt").ReplaceNum()
            };
            if (ConfigHelper.GetBoolValue("UseUserAgent")) {
                settings.UserAgent = httpHeadAgent;
            }
            settings.UserAgent = httpHeadAgent;

            //   settings.MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;
            settings.MultiThreadedMessageLoop = bool.Parse(ConfigHelper.GetValue("MultiThreadedMessageLoop"));
            ;

            CefRuntime.Initialize(mainArgs, settings, app);
            //注册  register custom scheme handler
            //
            CefRuntime.RegisterSchemeHandlerFactory("http", GlobalVar.Js2CsharpRequestDomain, new Js2CsharpSchemeHandlerFactory());
            CefRuntime.AddCrossOriginWhitelistEntry(
               "http://trade.taobao.com", "http", "https://tbapi.alipay.com", true);
            CefRuntime.AddCrossOriginWhitelistEntry(
               "http://trade.taobao.com", "https", "https://tbapi.alipay.com", true);

            if (!settings.MultiThreadedMessageLoop) {
                Application.Idle += (sender, e) => { CefRuntime.DoMessageLoopWork(); };
            }

            MainForm mainform;

            //通过配置觉得是否自运行
            if (GlobalVar.autoRun) {
                mainform = new MainForm(true);
            } else {
                mainform = new MainForm();
            }

            Application.Run(mainform);

            CefRuntime.Shutdown();

            
            return 0;
        }

        private static bool cefruntimeLoad(out int main) {
            main = 0;
            try {
                CefRuntime.Load();
            } catch (DllNotFoundException ex) {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                {
                    main = 1;
                    return true;
                }
            } catch (CefRuntimeException ex) {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                {
                    main = 2;
                    return true;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                {
                    main = 3;
                    return true;
                }
            }
            return false;
        }

        public static void InitAllinOne() {
            ConfigHelper.InitConfig("config//sysConfig.txt");
            // var strs = FileHelper.readstringAfterEqual("config//sysConfig.txt");
            GlobalVar.ChangeIpNum = Convert.ToInt32(ConfigHelper.GetValue("ChangeIpNum"));
            GlobalVar.autoRun = bool.Parse(ConfigHelper.GetValue("AutoRun"));
            GlobalVar.ChangeIp = bool.Parse(ConfigHelper.GetValue("ChangeIp"));
            GlobalVar.RegIntervalSeconds = int.Parse(ConfigHelper.GetValue("RegIntervalSeconds"));
            // 日志系统

            //切换模式
            GlobalVar.tbmode = (tbModel)tbModel.Parse(typeof(tbModel), ConfigHelper.GetValue("tbModel"));
            if (GlobalVar.tbmode == tbModel.reg_zfb || GlobalVar.tbmode == tbModel.reg_tbV3) {
                GlobalUI.taskRunner = new RegTaskRunner("待注册账号.txt", "已注册账号.txt");
            } else if (GlobalVar.tbmode == tbModel.shopv2) {
                GlobalUI.taskRunner = new ShopTaskRunner("待购物账号.txt", "已购物账号.txt");
            } else {
                LogManager.WriteLog("该模式目前不支持。");
            }

        }

    }
}
