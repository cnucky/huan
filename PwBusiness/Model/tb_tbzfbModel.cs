/**  版本信息模板在安装目录下，可自行修改。
* tb_tbzfb.cs
*
* 功 能： N/A
* 类 名： tb_tbzfb
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/12 16:01:18   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;

namespace PwBusiness.Model
{
    /// <summary>
    /// tb_tbzfb:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
     
    public partial class tb_tbzfb
    {
        public tb_tbzfb()
        {
            _accountid = -1;
            _tbname = "";
            _tbpwd = "";
            _zfbemail = "";
            _zfbemailpwd = "";
            _zfbpaypwd = "";
            _zfbsfz = "";
            _zfbpwd = "";
            _tbstatus = "";
            _regstatus = "";
            _zfbstatus = "";
            _realname = "";
            _mb_1_1 = "";
            _mb_1_2 = "";
            _mb_1_3 = "";
            _mb_2_1 = "";
            _mb_2_2 = "";
            _mb_2_3 = "";
            _mb_3_1 = "";
            _mb_3_2 = "";
            _mb_3_3 = "";
        }
        #region Model
        private int _accountid;
        private string _tbname;
        private string _tbpwd;
        private string _zfbemail;
        private string _zfbemailpwd;
        private string _zfbpaypwd;
        private string _zfbsfz;
        private string _zfbpwd;
        private string _tbstatus;
        private string _regstatus;
        private string _zfbstatus;
        private string _realname;
        private string _mb_1_1;
        private string _mb_1_2;
        private string _mb_1_3;
        private string _mb_2_1;
        private string _mb_2_2;
        private string _mb_2_3;
        private string _mb_3_1;
        private string _mb_3_2;
        private string _mb_3_3;
        /// <summary>
        /// 
        /// </summary>
        public int AccountId
        {
            set { _accountid = value; }
            get { return _accountid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tbName
        {
            set { _tbname = value; }
            get { return _tbname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tbPwd
        {
            set { _tbpwd = value; }
            get { return _tbpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbEmail
        {
            set { _zfbemail = value; }
            get { return _zfbemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbEmailPwd
        {
            set { _zfbemailpwd = value; }
            get { return _zfbemailpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbPayPwd
        {
            set { _zfbpaypwd = value; }
            get { return _zfbpaypwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbSFZ
        {
            set { _zfbsfz = value; }
            get { return _zfbsfz; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbPwd
        {
            set { _zfbpwd = value; }
            get { return _zfbpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tbStatus
        {
            set { _tbstatus = value; }
            get { return _tbstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string regStatus
        {
            set { _regstatus = value; }
            get { return _regstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbStatus
        {
            set { _zfbstatus = value; }
            get { return _zfbstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string realname
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_1_1
        {
            set { _mb_1_1 = value; }
            get { return _mb_1_1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_1_2
        {
            set { _mb_1_2 = value; }
            get { return _mb_1_2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_1_3
        {
            set { _mb_1_3 = value; }
            get { return _mb_1_3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_2_1
        {
            set { _mb_2_1 = value; }
            get { return _mb_2_1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_2_2
        {
            set { _mb_2_2 = value; }
            get { return _mb_2_2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_2_3
        {
            set { _mb_2_3 = value; }
            get { return _mb_2_3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_3_1
        {
            set { _mb_3_1 = value; }
            get { return _mb_3_1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_3_2
        {
            set { _mb_3_2 = value; }
            get { return _mb_3_2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mb_3_3
        {
            set { _mb_3_3 = value; }
            get { return _mb_3_3; }
        }
        #endregion Model

    }
}

