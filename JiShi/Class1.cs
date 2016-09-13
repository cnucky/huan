using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiShi {

    public class goods {
        public List<item> pageData { get; set; }

        public searchParams sParam { get; set; }
        public pageInfo pinfo { get; set; }


    }

    public class item {

        public string closed { get; set; }

        public string createDate { get; set; }
        public string curId { get; set; }
        public string deliveryDate { get; set; }
        public string gameId { get; set; }
        public string gameItemId { get; set; }
        public string gender { get; set; }
        public string grade { get; set; }
        public string gradeName { get; set; }
        public string guild { get; set; }
        public string iconPath { get; set; }
        public string id { get; set; }
        public string itemAmount { get; set; }
        public string itemCode { get; set; }
        public string itemDesc { get; set; }
        public string itemName { get; set; }
        public string itemType { get; set; }

        public string power { get; set; }
        public string price { get; set; }
        public string publicityEndDate { get; set; }
        public string returnDate { get; set; }
        public string saveTime { get; set; }
        public string sellerCasId { get; set; }
        public string sellerGameId { get; set; }
        public string sellerRole { get; set; }
        public string serverId { get; set; }
        public string shelfDate { get; set; }
        public string shelfDays { get; set; }
        public string status { get; set; }
        public string unitPrice { get; set; }



    }
    public class searchParams {
        public string itemName { get; set; }
        public string flag { get; set; }
        public string excludeItemId { get; set; }
        public string sortField { get; set; }
        public string typeNameParam { get; set; }
        public string gameId { get; set; }
        public string serverId { get; set; }
        public string sortWay { get; set; }
    }
    public class pageInfo {
        public int pageId { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; }

    }
}
