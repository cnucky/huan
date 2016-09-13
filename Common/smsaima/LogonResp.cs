using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aimaInterface
{
    public class LogonResp
    {
        /**
         * 调用状态
         */
        private bool state = false;

        public bool State
        {
            get { return state; }
            set { state = value; }
        }
        private String uid;

        public String Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private String token;

        public String Token
        {
            get { return token; }
            set { token = value; }
        }

        /**
         * 接口响应结果
         */
        private String result;

        public String Result
        {
            get { return result; }
            set { result = value; }
        }

	
    }
}
