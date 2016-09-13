using System;
using Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using Xilium.CefGlue.Client;

namespace LazyCat.UnitTestProject.Web {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]

        public void TestMethod1() {
            var currentHaoZi = HaoziHelper.InitXiaoHaoFromLine("test1|test1@163.com|1|1|kucc1112|邓文丽|431003198108073241|12|1|要斌|5|1|锋惠|13|1|傲霜大");
            HaoziHelper.UpdateLoadHaozi(ConfigHelper.GetValue("WebSite"), currentHaoZi, "add");
            ;

        }
    }
}
