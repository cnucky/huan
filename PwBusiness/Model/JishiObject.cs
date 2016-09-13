using System;
using System.Collections.Generic;

namespace PwBusiness.Model {

    public class JishiObject {

        public List<pageData> pagedata { get; set; }
        public pageInfo1 pageInfo { get; set; }
        public searchParams1 searchParams { get; set; }
    }
    public class pageInfo1 {
        public int pageId;
        public int pageSize;
        public int totalCount;
        public int totalPages;
    }

    public class searchParams1 {
        public string typeName;
        public string unitPrice;
        public string itemName;
        public string flag;
        public string excludeItemId;
        public string sortField;
        public string typeNameParam;
        public string gameId;
        public string serverId;
        public string sortWay;
    };

 
    public partial class pageData {

        public string closed;
        public string createDate;
        public string curId;
        public string gameId;
        public string gameItemId;
        public string gender;
        public string grade;
        public string gradeName;
        public string guild;
        public string iconPath;
        public string id;
        public int itemAmount;
        public string itemCode;
        public string itemDesc;
        public string itemName;
        public string itemType;
        public string power;
        public string price;
        public string publicityEndDate;
        public string returnDate;
        public string saveTime;
        public string sellerCasId;
        public string sellerGameId;
        public string sellerRole;
        public string serverId;
        public string shelfDate;
        public string shelfDays;
        public string status;
        public string unitPrice;
    }
}