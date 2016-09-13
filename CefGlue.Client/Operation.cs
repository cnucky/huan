using System;
using Common;
using PwBusiness;
using Xilium.CefGlue.WindowsForms;

namespace Xilium.CefGlue.Client {
    public class Operation {
        public CefFrame CefFrame { get; set; }

        public Operation(CefFrame frame) {
            CefFrame = frame;
            CurrentUrl = "!!";
            Deadline = ConfigHelper.GetIntValue("默认单页面停留时间") * 60 * 1000;
            note = "";
        }
        public string note { get; set; }
        public string CurrentUrl { get; set; }

        public BusinessStatus PerviousStatus { get; set; }

        public BusinessStatus NextStatus { get; set; }
        public EventHandler<LoadEndEventArgs> OperationHandler;
        public double index;

        public double Deadline { get; set; }

        /// <summary>
        /// 检测是否已经不在当前操作指定的页面
        /// </summary>
        /// <returns></returns>
        public bool CheckOperation() {

            return CefFrame.Url.Contains(CurrentUrl);
        }

        //public void OperationMethod(ref BusinessStatus state) {
        //    state = NextStatus;
        //    if (OperationHandler != null) {
        //        OperationHandler(new object(), new LoadEndEventArgs());
        //    }

        //}


    }
}