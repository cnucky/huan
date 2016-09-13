using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NPCMS.Common.Extensions;

namespace SYManager {
    public partial class BindAccount : Form {
        public BindAccount() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            List<string> xing = new List<string>();
            List<string> m1 = new List<string>();
            List<string> m2 = new List<string>();
            List<string> xm = new List<string>();

            List<string> xm2 = new List<string>();

            BaseIni bi_xing = new BaseIni();
            bi_xing.ReadPath(@"E:\Bak\Soft\Game\9y\resources\text.package.files\res\text\chineses\string_player_name\first_name.idres");
            bi_xing.analysisLine();
            xing = bi_xing.getItemValueString();
            // bi_xing.test();

            BaseIni bi_ming = new BaseIni();
            bi_ming.ReadPath(@"E:\Bak\Soft\Game\9y\resources\text.package.files\res\text\chineses\string_player_name\second_name_boy.idres");
            bi_ming.analysisLine();
            m1 = bi_ming.getItemValueString();

            BaseIni bi_ming2 = new BaseIni();
            bi_ming2.ReadPath(@"E:\Bak\Soft\Game\9y\resources\text.package.files\res\text\chineses\string_player_name\second_name_girl.idres");
            bi_ming2.analysisLine();
            m2 = bi_ming2.getItemValueString();


            string xingming = "";
            long qq = 2601598879;
            long mobilephone = 13303150315;
            for (int i = 0; i < m1.Count; i++) {
                if (xing[i].Length > 2) {
                    continue;
                }

                xm.Add(xing[i] + m1[i]);
            }

            for (int i = 0; i < m2.Count; i++) {
                if (xing[i].Length > 2) {
                    continue;
                }
                xm2.Add(xing[i] + m2[i]);
            }
            //truename

            //男
            qq = 2603598879;
            mobilephone = 13303150315;
            for (int i = 0; i < xm2.Count; i++) {
                if (xm[i].Length == 2) {
                    xm[i] += xm[i].Last();

                }
                string _ = xm[i];
                //  Console.WriteLine("$datas['qq']=\"" + qq + "\";$datas['mobilephone']=\"" + mobilephone + "\";$datas['email']=\"" + qq + "@qq.com" + "\";$datas['username']=\"" + _ + "\";$datas['truename']=\"" + _ + "\";$rs = member_base::add($datas);");
                //   Console.WriteLine("update bf_memberfields set isEnsure=1 where uid = (select id from bf_members where username=\""+_+"\");");
                Console.WriteLine(_);
                qq++;
                mobilephone++;
            }
            //女
            qq = 2604598879;
            mobilephone = 13403150315;
            for (int i = 0; i < xm2.Count; i++) {
                if (xm2[i].Length == 2) {
                    xm2[i] += xm2[i].Last();

                }
                string _ = xm2[i];
                //   Console.WriteLine("$datas['qq']=\"" + qq + "\";$datas['mobilephone']=\"" + mobilephone + "\";$datas['email']=\"" + qq + "@qq.com" + "\";$datas['username']=\"" + _ + "\";$datas['truename']=\"" + _ + "\";$rs = member_base::add($datas);");
                //   Console.WriteLine("update bf_memberfields set isEnsure=1 where uid = (select id from bf_members where username=\"" + _ + "\");");
                Console.WriteLine(_);
                qq++;
                mobilephone++;
            }
            //xm.ForEach(_ =>);
            //

            // $datas['username']="掉渣天";
            //
            //$rs = member_base::add($datas);

        }

        List<string> ReadFile2List(string filePath) {
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312"));
            string nextLine;
            List<string> strList = new List<string>();
            while ((nextLine = sr.ReadLine()) != null) {
                strList.Add(nextLine.Trim());
                //    MessageBox.Show(nextLine);

            }
            return strList;
        }

        void LogRes(string filename, List<string> strList) {
            StreamWriter sw = new StreamWriter(filename);
            string nextLine;
            strList.ForEach(_ => sw.WriteLine(_));
            sw.Flush();
            sw.Close();
        }
        /// <summary>
        /// 绑定大号小号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            var l1 = ReadFile2List(TB_PingtaiAccount.Text.Trim());
            var l2 = ReadFile2List(TB_TBORPAIPAI.Text.Trim());
            //INSERT INTO bf_buyers (type,uid,username,nickname,score0,score,maxScore,status,timestamp,pauseTimestamp)
            //VALUES (1,(SELECT id from bf_members where  bf_members.username='乌墨冰'),'乌墨冰','1111111',0,0,999,0,1392609810,0)
            string type = "";
            if (checkBox1.Checked) {//淘宝
                type = "1";
            } else {
                type = "2";
            }

            string s1 = "INSERT INTO bf_buyers (type,uid,username,nickname,score0,score,maxScore,status,timestamp,pauseTimestamp) VALUES ({3},(SELECT id from bf_members where  bf_members.username='{0}'),'{1}','{2}',0,0,999,0,1395136384,0);";
            List<string> reslist_sql = new List<string>();
            List<string> reslist_taobaoname = new List<string>();
            List<string> reslist_email = new List<string>();
            for (int i = 0; i < l1.Count; i++) {
                string s2 = s1;
                s2 = s2.With(l1[i], l1[i], l2[i].splitString('|')[0], type);
                //  Console.WriteLine(l2[i].ToString().splitString('|')[3].Trim());
                reslist_sql.Add(s2);
               
                if (type == "1") {
                    reslist_taobaoname.Add(l2[i].ToString().splitString('|')[0]);
                    reslist_email.Add(l2[i].ToString().splitString('|')[2]);
                }
             
            }
            string r1 = (DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day).ToString();
            LogRes("reslist_sql" + r1 + ".txt", reslist_sql);
            if (type == "1")
            {
                LogRes("reslist_taobaoname{0}.txt".With(r1), reslist_taobaoname);
                LogRes("reslist_email{0}.txt".With(r1), reslist_email);
            }

        }
    }
}
