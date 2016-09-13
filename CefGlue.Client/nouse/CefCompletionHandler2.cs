using System;
using Common;

namespace Xilium.CefGlue.Client
{
    public class CefCompletionHandler2 : CefCompletionHandler {
        public static CefCompletionHandler2 GetOne() {
            return new CefCompletionHandler2();
        }

        protected override void OnComplete() {
            //  throw new NotImplementedException();
            LogManager.WriteLog("FlushStore is done ");// MessageBox.Show("piece ok");

        }


    }
}