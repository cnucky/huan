using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aimaInterface
{
    public class AddIgnoreListResp
    {


        /**
         * 调用状态
         * 返回false 可能为未登录、无数据、调用异常等情况
         */
        private bool state = false;

        public bool State
        {
            get { return state; }
            set { state = value; }
        }
        private int row;

        public int Row
        {
            get { return row; }
            set { row = value; }
        }
        private String result;

        public String Result
        {
            get { return result; }
            set { result = value; }
        }

	

    }
}
