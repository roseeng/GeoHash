using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeoHash;
using System;

namespace GeoHash.MsTest
{
    [TestClass]
    public class GDateTests
    {
        [TestMethod]
        public void TestWestPre20080526()
        {
            DateTime test = new DateTime(2008, 05, 26);
            var result = GDate.ForLongitude(test, 60);

            Assert.AreEqual("2008-05-26", result.ToString());
            Assert.AreEqual("2008-05-26", result.DowJonesString());
        }

        [TestMethod]
        public void TestEastPre20080526()
        {
            DateTime test = new DateTime(2008, 05, 26);
            var result = GDate.ForLongitude(test, 0);

            Assert.AreEqual("2008-05-26", result.ToString());
            Assert.AreEqual("2008-05-26", result.DowJonesString());
        }

        [TestMethod]
        public void TestWestPost20080526()
        {
            DateTime test = new DateTime(2008, 05, 27);
            var result = GDate.ForLongitude(test, -60);

            Assert.AreEqual("2008-05-27", result.ToString());
            Assert.AreEqual("2008-05-27", result.DowJonesString());
        }

        [TestMethod]
        public void TestEastPost20080526()
        {
            DateTime test = new DateTime(2008, 05, 27);
            var result = GDate.ForLongitude(test, 0);

            Assert.AreEqual("2008-05-27", result.ToString());
            Assert.AreEqual("2008-05-26", result.DowJonesString());
        }

    }
}
