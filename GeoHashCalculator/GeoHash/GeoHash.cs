using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace GeoHash
{
    public class GeoHash
    {
        private static HttpClient httpClient = new HttpClient();

        public static string[] GetGeoHash(DateTime date, int latitude, int longitude)
        {
            var gdate = GDate.ForLongitude(date, longitude);
            var djia = GetDowJonesAsync(gdate).ConfigureAwait(false).GetAwaiter().GetResult();
            var fractions = CalculateFractions(djia, gdate);
            fractions[0] = (fractions[0] + Math.Abs(latitude)) * Math.Sign(latitude);
            fractions[1] = (fractions[1] + Math.Abs(longitude)) * Math.Sign(longitude);

            var result = from f in fractions
                         select f.ToString("F5");

            return result.ToArray();
        }

        public static string[] GetGlobalHash(DateTime date)
        {
            var gdate = GDate.ForGlobalhash(date);
            var djia = GetDowJonesAsync(gdate).ConfigureAwait(false).GetAwaiter().GetResult();
            var fractions = CalculateFractions(djia, gdate);
            fractions[0] = fractions[0] * 180.0 - 90.0;
            fractions[1] = fractions[1] * 360.0 - 180.0;

            var result = (from f in fractions
                          select f.ToString("F5")).ToArray();

            return result;
        }

        public static async Task<string> GetDowJonesAsync(GDate gdate)
        {
            // http://geo.crox.net/djia/%Y/%m/%d
            // According to the W30 rule, use actual date or date before, depending on date and longitude
            var result = await httpClient.GetAsync($"http://geo.crox.net/djia/{gdate.DowJonesString()}");
            if (!result.IsSuccessStatusCode)
                return "";

            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public static double[] CalculateFractions(string djia, GDate gdate)
        {
            string hashstring = gdate.ToString() + "-" + djia; // In the hash string we always use actual date

            string hash = GetMd5Hash(hashstring);

            var fraction1 = HexFraction(hash.Substring(0, 16));
            var fraction2 = HexFraction(hash.Substring(16, 16));
            double[] result = new double[] { fraction1, fraction2 };

            return result;
        }

        public static string GetMd5Hash(string input)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.md5?view=netcore-3.1

            using (MD5 md5Hash = MD5.Create())
            {

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public static double HexFraction(string hexString)
        {
            double curvalue = 0;
            double base16 = 16;
            hexString = hexString.ToUpper();

            byte[] hex = Encoding.ASCII.GetBytes(hexString);
            for (int i = 0; i < hex.Length; i++)
            {
                int value = hex[i] - '0';
                if (value > 9)
                    value = value - 7; // '9' - 'A'

                double dvalue = (double)value;
                var weight = Math.Pow(base16, i + 1);
                curvalue += dvalue / weight;
            }

            return curvalue;
        }
    }
}