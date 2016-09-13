using System;
using System.Collections.Generic;
using Common;
using Newtonsoft.Json;
using PwBusiness.Model;

namespace PwBusiness.Bll {
    public class tbzfbBll {

        public List<tb_tbzfb> GetHaoZi(string website, string name, string pwd, int num) {

            MyWebClient myWebClient = new MyWebClient();
            string url = "http://{0}/home/haozi?name={1}&pwd={2}&num={3}"
                     .With(website, name, pwd, num);

            string str = myWebClient.DownHaoZi(url);
       
            return ConvertStr2ModeList(str);

        }

        public List<tb_tbzfb> ConvertStr2ModeList(string str) {
            List<tb_tbzfb> tbzfbs = new List<tb_tbzfb>();
            try {
                tbzfbs = JsonConvert.DeserializeObject<List<tb_tbzfb>>(str);
            } catch (Exception e1) {

            }
            return tbzfbs;

        }

        public List<tb_tbzfb> GetHaoZi(string website, string name, string pwd, int num, string tbmodel) {
            MyWebClient myWebClient = new MyWebClient();
            string url = "http://{0}/home/haozi?name={1}&pwd={2}&num={3}&tbmodel={4}"
                     .With(website, name, pwd, num, tbmodel); 
            string str = myWebClient.DownHaoZi(url);
            return ConvertStr2ModeList(str);
        }
    }
}