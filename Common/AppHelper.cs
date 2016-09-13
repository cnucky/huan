using System;
using System.Diagnostics;

namespace Common {
    public class AppHelper {
        //"restart.bat"
      
        /// <summary>
        ///  //"restart.bat"
        /// </summary>
        /// <param name="file2Run"></param>
        public static void RestartApp(string file2Run) {
            LogManager.WriteLog("重启程序");
            Process.Start(file2Run);
            //Process.Start("Xilium.CefGlue.Client.exe");
            Environment.Exit(System.Environment.ExitCode);
        }

        /// <summary>
        ///  //"restart.bat"
        /// </summary>
        /// <param name="file2Run"></param>
        public static void ClearCache( ) {
            LogManager.WriteLog("清理缓存文件");
            Process.Start("clearCache.bat");
           
        }


        /// <summary>
        ///  //"restart.bat"
        /// </summary>
        /// <param name="file2Run"></param>
        public static void ExisttApp( ) {
            LogManager.WriteLog("退出程序");
        
            //Process.Start("Xilium.CefGlue.Client.exe");
            Environment.Exit(System.Environment.ExitCode);
        }

    }
}