using System;
using System.Threading.Tasks;

namespace PwBusiness.Model {
    /// <summary>
    /// tb_tasklog:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class tb_tasklog {
        public tb_tasklog(string email, string state) {
            zfbEmail = email;
            //  BusinessStatus.ready.ToString()
            taskstatus = state;

        }
        #region Model
        private int _itemid;
        private string _zfbemail;
        private string _action;
        private string _actionresult;
        private string _taskstatus;
        private DateTime _actiontime;
        private string _note;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int itemId {
            set { _itemid = value; }
            get { return _itemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string zfbEmail {
            set { _zfbemail = value; }
            get { return _zfbemail; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string action {
            set { _action = value; }
            get { return _action; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string actionresult {
            set { _actionresult = value; }
            get { return _actionresult; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string taskstatus {
            set { _taskstatus = value; }
            get { return _taskstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime actiontime {
            set { _actiontime = value; }
            get { return _actiontime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string note {
            set { _note = value; }
            get { return _note; }
        }
        #endregion Model

    }
}

