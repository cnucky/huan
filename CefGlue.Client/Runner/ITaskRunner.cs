using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PwBusiness.Model;

namespace Xilium.CefGlue.Client {
    /// <summary>
    /// 任务的执行者
    /// </summary>
    public interface ITaskRunner {
        void LogAccontStatus(string state);
        EventHandler GetBeginHandler();

        tb_tbzfb currentAccount { get; set; }
        void LogTaskBegin();

        void ReportAccount(string isok);
       // void ReportAccount(tb_tasklog tasklog);
        bool getNewTask();
        string getinfilename();
        string getoutfilename();



        void ReportAccountStatus(string action, string status, string note);
    }
}
