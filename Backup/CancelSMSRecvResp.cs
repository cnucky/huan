using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aimaInterface
{
    public class CancelSMSRecvResp
    {


        private bool state = false;	 //返回false 可能为未登录、无数据、调用异常等情况

        public bool State
        {
            get { return state; }
            set { state = value; }
        }
        private String flag;

        public String Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        private String result;

        public String Result
        {
            get { return result; }
            set { result = value; }
        }

    }
}
