using System;
using Common.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LazyCat.UnitTestProject {
    [TestClass]
    public class UnitTest4 {
        [TestMethod]
        public void TestMethod1() {
            Console.WriteLine(NetHostIp.GetFirstIP());
        }
    }
}
