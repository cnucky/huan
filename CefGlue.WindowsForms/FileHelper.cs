using System;
using System.Collections.Generic;
using System.IO;

namespace Xilium.CefGlue.WindowsForms {
    public static class FileHelper1 {
        public static void log(string filename, string content) {
            StreamWriter sw = new StreamWriter(filename, true);
            sw.Write(content);
            sw.Close();
        }
        /// <summary>
        /// 如果一行中有=，取=后面的东西
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<string> readstringAfterEqual(string filename) {
            List<string> strList = new List<string>();
            StreamReader srReader = new StreamReader(filename, true);
            string line = "";
            while ((line = srReader.ReadLine()) != null) {
                if (string.IsNullOrEmpty(line)) {
                    continue;
                }
                if (line.Contains("=")) {
                    line = line.Split('=')[1];
                }
                strList.Add(line);
            }
            srReader.Close();
            return strList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<string> read(string filename) {
            List<string> strList = new List<string>();
            StreamReader srReader = new StreamReader(filename, true);
            string line = "";
            while ((line = srReader.ReadLine()) != null) {
                if (string.IsNullOrEmpty(line)) {
                    continue;
                }

                strList.Add(line);
            }
            srReader.Close();
            return strList;
        }

        public static List<Tuple<string, string>> readstringAfter(string filename, char p2) {
            List<Tuple<string, string>> strList = new List<Tuple<string, string>>();



            StreamReader srReader = new StreamReader(filename, true);
            string line = "";

            int index = 0;
            while ((line = srReader.ReadLine()) != null) {
                index++;
                if (string.IsNullOrEmpty(line)) {
                    continue;
                }
                var ls1 = line.Split(p2);
                if (ls1.Length >= 2) {
                    if (ls1[1].Length != 15 && ls1[1].Length != 18) {
                        Console.WriteLine("error " + index);
                    }
                    strList.Add(new Tuple<string, string>(ls1[0], ls1[1]));
                }

            }
            srReader.Close();
            return strList;
        }

        public static List<Tuple<string, string, string>> readstringAfter3(string filename, char p2) {
            var strList = new List<Tuple<string, string, string>>();


            StreamReader srReader = new StreamReader(filename, true);
            string line = "";

            int index = 0;
            while ((line = srReader.ReadLine()) != null) {
                index++;
                if (string.IsNullOrEmpty(line)) {
                    continue;
                }
                var ls1 = line.Split(p2);
                if (ls1.Length >= 3 && !string.IsNullOrEmpty(ls1[0]) && !string.IsNullOrEmpty(ls1[1])
                    && !string.IsNullOrEmpty(ls1[2])
                    ) {
                    var t1 = new Tuple<string, string, string>(ls1[0].Trim(), ls1[1].Trim(), ls1[2].Trim());

                    bool exist = strList.Exists(_ => _.Item1 == t1.Item1);
                    if (!exist) {
                        strList.Add(t1);
                    }

                }

            }
            srReader.Close();
            return strList;
        }
    }
}
