using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using GeoHash;

namespace GeoHash.MsTest
{
    [TestClass]
    public class Test30WCompliance
    {
        const int DATA_ROWS_IN_FILE = 11;

        int validated = 0;

        [TestMethod]
        public void TestDataFile()
        {
            using (var reader = new StreamReader("GeoHashing - Testing for 30W compliance.tsv"))
            {
                var row = reader.ReadLine(); // Header Row
                var fields = row.Split('\t');
                Assert.AreEqual(8, fields.Length, "Bad structure in the test data file. Should have 8 fields.");

                validated = 0;
                while (!reader.EndOfStream)
                {
                    row = reader.ReadLine();
                    fields = row.Split('\t');
                    if (fields.Length != 8)
                        break; // Stop when first incomplete row comes

                    TestOneRow(fields);
                    validated++;
                }

                Assert.AreEqual(DATA_ROWS_IN_FILE, validated);
            }
        }

        private void TestOneRow(string[] fields)
        {
            var date = DateTime.Parse(fields[0]);
            var djia = fields[1];
            var hashStringWest = fields[2];
            var hashStringEast = fields[3];
            var hashStringGlobal = fields[4];
            var coord68minus30 = fields[5];
            var coord68minus29 = fields[6];
            var globalCoord = fields[7];

            VerifyDJIA(date, djia);

            var actual_coordWest = GeoHash.GetGeoHash(date, 68, -30);
            VerifyCoord(coord68minus30, actual_coordWest, "West");

            var actual_coordEast = GeoHash.GetGeoHash(date, 68, -29);
            VerifyCoord(coord68minus29, actual_coordEast, "East");

            var actual_coordGlobal = GeoHash.GetGlobalHash(date);
            VerifyCoord(globalCoord, actual_coordGlobal, "Global");
        }

        private void VerifyCoord(string coord, string[] actual_coords, string column)
        {
            // TODO: Deal with localization in a more generic manner
            // For now, just assume swedish locale
            coord = coord.Replace(", ", "; ");
            coord = coord.Replace(".", ",");
            string actual_coord = actual_coords[0] + "; " + actual_coords[1];

            Assert.AreEqual(coord, actual_coord, $"Column {column}, Data line {validated+1}"); // "+1" since we are validating the next line
        }

        private void VerifyDJIA(DateTime date, string djia)
        {
            GDate gdate = new GDate(date, date);
            var actual_djia = GeoHash.GetDowJonesAsync(gdate).ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(djia, actual_djia, $"Data line {validated+1}");
        }
    }
}
