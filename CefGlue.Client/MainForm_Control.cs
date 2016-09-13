using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using PwBusiness;
using PwBusiness.Model;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {
    public partial class MainForm {

        public static ITaskRunner taskRunner;
        public static BusinessStatus state = BusinessStatus.ready;

        public static CefFrame getMainframe;
       
    
        private void OperationTest(object s, LoadEndEventArgs e, string currentUrl) {
            lock (lockobject) {
                LogManager.WriteLog("state={0},currentUrl={1}".With(state, currentUrl));
                foreach (var operation in opsList) {
                    if (operation.PerviousStatus == state && currentUrl.Contains(operation.CurrentUrl)) {
                        LogManager.WriteLog("{2} {0} 跳转到下一个状态 {1} {3} ".With(state, operation.NextStatus, operation.index, operation.note));

                        state = operation.NextStatus;
                        ovtimeTick.Next(operation, e);//计时器重新计时
                        //记录当前注册账号的状态
                        //LogManager.LogAccontStatus("|{0}".With(state.ToString()));
                        taskRunner.LogAccontStatus("|{0}".With(state.ToString()));// MouseKeyBordHelper.CurrentIP 

                        if (operation.OperationHandler != null) {
                            int timeCount = RandomManager.random.Next(2, 5) * 1000;
                            LogManager.WriteLog("随机休息{0}毫秒".With(timeCount));

                            //BeginInvoke(new Action(() => {
                            new Thread(() => {
                                Thread.Sleep(timeCount);
                                try {
                                    operation.OperationHandler.Invoke(operation, e);

                                } catch (Exception e2) {

                                    LogManager.WriteLog(e2.StackTrace + e2.ToString());
                                }

                            }).Start();
                            //}));

                        }
                        break;
                    }
                }
            }
        }

        #region  ==============辅助逻辑==============

        private List<Operation> opsList = new List<Operation>();

        public class OverTimeTick {
            //  private Stack<BusinessStatus> StateStack = new Stack<BusinessStatus>();
            private int badRunTime = 0;

            System.Timers.Timer timersTimer = new System.Timers.Timer();
            private Operation op;
            private MainForm _mfForm;
            public OverTimeTick() {

                timersTimer.Elapsed += (s1, e1) => {
                    LogManager.WriteLog("重新执行该方法 {0} ，{1}秒已经到期".With(op.OperationHandler.Method.Name,op.Deadline));
                    ReflushIfInFrames(op.CefFrame);

                    badRunTime++;
                    if (badRunTime > 2) {
                        taskRunner.ReportAccount("failed");
                        AppHelper.RestartApp("restart.bat");
                    }
                    if (op.OperationHandler != null) {
                        op.OperationHandler.Invoke(op.CefFrame, loadEndEventArgs);
                    }

                };
            }


            private LoadEndEventArgs loadEndEventArgs;
            internal void Next(Operation operation, LoadEndEventArgs e) {
                loadEndEventArgs = e;
                badRunTime = 0;
                op = operation;
                //timersTimer.Close();
                //timersTimer.Enabled = true;
                timersTimer.Stop();
                timersTimer.Interval = operation.Deadline;
                timersTimer.Start();

            }

            internal void Stop() {
                timersTimer.Stop();
            }
        }

        public OverTimeTick ovtimeTick = new OverTimeTick();

        public static tb_tbzfb currentHaoZi = new tb_tbzfb();




        private void ClickByClassName(string elementid) {
            var main = this.GetActiveBrowser().Browser.GetMainFrame();
            main.ExecuteJavaScript("document.getElementsByClassName('{0}')[0].click()".With(elementid), main.Url, 0);
        }

        private void ClickById(string elementid) {
            var main = this.GetActiveBrowser().Browser.GetMainFrame();
            main.ExecuteJavaScript(" document.getElementById('{0}').click();".With(elementid), main.Url, 0);
        }

        private void SetElementValueById(string elementid, string elementvalue) {
            var main = this.GetActiveBrowser().Browser.GetMainFrame();
            main.ExecuteJavaScript(" document.getElementById('{0}').focus();".With(elementid), main.Url, 0);
            main.ExecuteJavaScript(" document.getElementById('{0}').value = \"{1}\";".With(elementid, elementvalue), main.Url, 0);
            main.ExecuteJavaScript(" document.getElementById('{0}').blur();".With(elementid), main.Url, 0);
        }


 
        public bool IsCurrentHaoziFinish = false;


        #endregion
    }
}
