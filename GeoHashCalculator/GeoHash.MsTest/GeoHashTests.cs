using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using GeoHash;

namespace GeoHash.MsTest
{
    [TestClass]
    public class GeoHashTests
    {
        [TestMethod]
        public void TestGetGlobalhash()
        {
            // Verifying against the table on https://geohashing.site/geohashing/30W_Time_Zone_Rule

            //DateTime date = new DateTime(2008, 05, 27);
            //string expected_latitude = "-67,43391";
            //string expected_longitude = "27,75993";
            DateTime date = new DateTime(2008, 05, 25);
            string expected_latitude = "79,51947";
            string expected_longitude = "-114,16550";

            var coordinates = GeoHash.GetGlobalHash(date);

            Assert.AreEqual(expected_latitude, coordinates[0]);
            Assert.AreEqual(expected_longitude, coordinates[1]);
        }

        [TestMethod]
        public void TestGetGeoHashWest()
        {
            // Verifying against the table on https://geohashing.site/geohashing/30W_Time_Zone_Rule

            DateTime date = new DateTime(2008, 05, 27);
            string expected_latitude = "68,20968"; // "209677"
            string expected_longitude = "-30,10144"; // "101441"

            var coordinates = GeoHash.GetGeoHash(date, 68, -30);

            Assert.AreEqual(expected_latitude, coordinates[0]);
            Assert.AreEqual(expected_longitude, coordinates[1]);
        }

        [TestMethod]
        public void TestGetGeoHashEast()
        {
            // Verifying against the table on https://geohashing.site/geohashing/30W_Time_Zone_Rule

            DateTime date = new DateTime(2008, 05, 27);
            string expected_latitude = "68,12537"; // "125367"
            string expected_longitude = "-29,57711"; // "577110"

            var coordinates = GeoHash.GetGeoHash(date, 68, -29);

            Assert.AreEqual(expected_latitude, coordinates[0]);
            Assert.AreEqual(expected_longitude, coordinates[1]);
        }

        [TestMethod]
        public void TestGetDJIAWest()
        {
            DateTime date = new DateTime(2008, 05, 27);
            GDate gdate = GDate.ForLongitude(date, -30);
            string expected_djia = "12479.63";

            var djia = GeoHash.GetDowJonesAsync(gdate).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(expected_djia, djia);
        }

        [TestMethod]
        public void TestGetDJIAEast()
        {
            DateTime date = new DateTime(2008, 05, 27);
            GDate gdate = GDate.ForLongitude(date, -29);
            string expected_djia = "12620.90";

            var djia = GeoHash.GetDowJonesAsync(gdate).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(expected_djia, djia);
        }

        [TestMethod]
        public void TestGetDowJonesAsync()
        {
            GDate gdate = new GDate(2005, 05, 26); // https://geohashing.site/images/thumb/5/51/Coordinates.png/600px-Coordinates.png
            string expected_djia = "10458.68";

            var djia = GeoHash.GetDowJonesAsync(gdate).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.AreEqual(expected_djia, djia);
        }

        [TestMethod]
        public void TestCalculateGeoHashManually()
        {
            string year = "2005";
            string month = "05";
            string day = "26";
            string djia = "10458.68";

            string expected_hash = "db9318c2259923d08b672cb305440f97";

            string hashstring = year + "-" + month + "-" + day + "-" + djia;
            var hash = GeoHash.GetMd5Hash(hashstring); 

            Assert.AreEqual(expected_hash, hash);
        }

        [TestMethod]
        public void TestCalculateGeoHash()
        {
            GDate gdate = new GDate(2005, 05, 26);
            string djia = "10458.68";

            string expected_fraction0 = "857713";
            string expected_fraction1 = "544543";

            var fractions = GeoHash.CalculateFractions(djia, gdate);
            var sfraction0 = fractions[0].ToString().Substring(2, 6);
            var sfraction1 = fractions[1].ToString().Substring(2, 6);

            Assert.AreEqual(expected_fraction0, sfraction0);
            Assert.AreEqual(expected_fraction1, sfraction1);
        }

        [TestMethod]
        public void TestHexFraction()
        {
            string hash0 = "db9318c2259923d0";
            string hash1 = "8b672cb305440f97";
            //double expected_fraction = 0.85771326770700229;
            
            string expected_fraction = "857713";

            var fraction = GeoHash.HexFraction(hash0);
            var scurvalue = fraction.ToString().Substring(2, 6);

            Assert.AreEqual(expected_fraction, scurvalue);

            expected_fraction = "544543";

            fraction = GeoHash.HexFraction(hash1);
            scurvalue = fraction.ToString().Substring(2, 6);

            Assert.AreEqual(expected_fraction, scurvalue);
        }
    }
}
