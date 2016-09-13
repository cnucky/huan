using System;
using System.IO;
using System.Windows.Forms;

namespace Common {
    public class LogManager {

        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath {
            get {
                if (logPath == string.Empty) {
                    if (System.Web.HttpContext.Current == null)
                        // Windows Forms 应用
                        logPath = AppDomain.CurrentDomain.BaseDirectory;
                    else
                        // Web 应用
                        logPath = AppDomain.CurrentDomain.BaseDirectory + @"bin\";
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }
        public static void WriteHaoZi(string msg, string filname) {

            string path = "import//" + filname;
            if (File.Exists(path)) {
                File.Move(path, "import//" + filname + "{0}.txt".With(RandomManager.GetDateTimeString()));
            }

            using (StreamWriter sw = File.CreateText(
                    "import//" + filname
                    )) {
                sw.Write(msg);
                sw.Close();
            }
        }

        //public static void LogHaoZi(string msg) {
        //    using (StreamWriter sw = File.AppendText(
        //            "import//" + "已注册账号.txt"
        //            )) {
        //        sw.Write(msg);
        //        sw.Close();
        //    }
        //}
        public static void WriteLog(string msg) {
            Console.WriteLine(msg);
            WriteLog(LogFile.Normal, msg);
            if (tbBox != null) {
                tbBox.Text += (msg + Environment.NewLine);

            }
        }

        public static void WriteLogIfDebug(string msg) {
            if (!ConfigHelper.GetBoolValue("debug")) {
                return;
            }
            Console.WriteLine(msg);
            WriteLog(LogFile.Normal, msg);
            if (tbBox != null) {
                tbBox.Text += (msg + Environment.NewLine);

            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg) {

            try {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    Application.StartupPath + "//" + LogFielPrefix + logFile +
                    DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") + msg);
                sw.Close();
            } catch { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void Write2File(string path, string msg) {

            try {
                System.IO.StreamWriter sw = System.IO.File.CreateText(path
                    );
                sw.Write(msg);
                sw.Close();
            } catch { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(LogFile logFile, string msg) {
            WriteLog(logFile.ToString(), msg);
        }

        public static void LogHaoZi(string msg, string filename) {
            using (StreamWriter sw = File.AppendText(
                    "import//" + filename
                    )) {
                sw.Write(msg);
                sw.Close();
            }
        }

        private static TextBox tbBox;
        public static void SetUI(TextBox textBox) {
            if (tbBox == null) {
                tbBox = textBox;
            }
        }
    }
}