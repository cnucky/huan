using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PwBusiness {
    public static class FileHelper {
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<string> read(string filename, Encoding encoding) {
            List<string> strList = new List<string>();
            StreamReader srReader = new StreamReader(filename, encoding, true);
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

        static Random r1 = new Random();
        public static string RandomReadOneLine(string p) {
            var lines = File.ReadAllLines(p);
            //lines = lines.Distinct();
            int count = lines.Length;
            return lines[r1.Next(0, count)];
        }
    }
}
