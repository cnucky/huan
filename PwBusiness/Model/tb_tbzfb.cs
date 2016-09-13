using System;

namespace PwBusiness.Model {
    /// <summary>
    /// tb_tbzfb:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class tb_tbzfb {
        public bool IsExtractBids = false;
        public bool isLoadedCompetedFillLink = false;

        public string activeLink;
        public int opentbTime = 0;


        public int telReviceTime { get; set; }
        /// <summary>
        /// auto_increment
        /// </summary>
        public int tbzfbID {
            set;
            get;
        }
    }
}

