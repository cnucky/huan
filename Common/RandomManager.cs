using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using NUnit.Framework;

namespace Common {

    public static class RandomManager {

        public static List<string> Zero2Nine {
            get {
                List<string> bodyList = new List<string>();

                //添加数字
                for (int i = 0; i < 10; i++) {
                    bodyList.Add(i.ToString());
                }
                return bodyList;
            }
        }

        public static List<string> zimu24 {
            get {
                List<string> headerList = new List<string>();

                //添加小写字母
                for (int i = Convert.ToInt32('a'); i <= Convert.ToInt32('z'); i++) {
                    headerList.Add(((char)i).ToString());
                }
                return headerList;
            }
        }

        public static List<string> zimuAndNum {
            get {
                List<string> headerList = new List<string>();
                headerList.AddRange(Zero2Nine);
                headerList.AddRange(zimu24);
                return headerList;
            }
        }


        public static Random random = new Random(DateTime.Now.Millisecond);

        public static string GetZero2Nine() {
            return random.Next(0, 10).ToString();
        }

        public static string ProduceRandomStr(int start, int end, string[] strList) {
            string res = "";
            int length = random.Next(start, end);

            //其余部分字符
            for (int i = 0; i < length; i++) {
                int index = random.Next(strList.Length - 1);
                res += strList[index];

            }
            return res;

            //6-18个字符，可使用字母 数字 下划线，需字母开头

        }

        public static string getPwd() {
            return RandomManager.ProduceRandomStr(3, 5, RandomManager.zimu24.ToArray()) +
                   RandomManager.ProduceRandomStr(3, 5, RandomManager.Zero2Nine.ToArray()) +
                   RandomManager.ProduceRandomStr(1, 5, RandomManager.Zero2Nine.ToArray());
        }
        [Test]
        public static string GetDateTimeString() {
            var s1 = DateTime.Now.ToString("yyyyMMddHHmmss");
            Console.WriteLine(s1);
            return s1;
        }

        public static string GetRandomName() {
            List<string> strTBnamelist = Common.FileHelper1.readstringAfterEqual("account//name.txt");
            string randomName = strTBnamelist[random.Next(0, strTBnamelist.Count)] +
                         strTBnamelist[random.Next(0, strTBnamelist.Count)];
            return randomName;

        }

        public static string GetSQ() {
            //1-13|1,3|
            int i2 = random.Next(1, 3) % 2 == 0 ? 1 : 3;
            return random.Next(1, 14) + "|" + i2 + "|" + GetRandomName() + "|";
        }

        public static string[] hanzi {
            get {
                return Common.FileHelper1.readstringAfterEqual("account//name.txt").ToArray();
            }

        }


        public static string RandomReadOneLine(string p) {
            var lines = File.ReadAllLines(p);
            var line1s = lines.Distinct();
            int count = line1s.Count();
            return line1s.ElementAt(random.Next(0, count));
        }


        public static bool randomBool() {
            return random.Next(0, 2) == 1;
        }
           [Test]
        public static void testrandomBool() {
               for (int i = 0; i < 11; i++) {
                   Console.WriteLine(randomBool());
               }
           

        }
    }
}