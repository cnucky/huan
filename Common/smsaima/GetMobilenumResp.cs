using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aimaInterface
{
    public class GetMobilenumResp
    {

        private bool state = false;	 //返回false 可能为未登录、无数据、调用异常等情况

        public bool State
        {
            get { return state; }
            set { state = value; }
        }
        private String mobile;

        public String Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        private String result;

        public String Result
        {
            get { return result; }
            set { result = value; }
        }

    }
}
