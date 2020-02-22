using System;

namespace GeoHash
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 3)
                Usage();

            DateTime date;
            string[] coords;
            if (args[0] == "-g")
            {
                if (args.Length == 1)
                    date = DateTime.Now;
                else
                    date = DateTime.Parse(args[1]);
                
                coords = GeoHash.GetGlobalHash(date);
                Console.WriteLine($"Globalhash: {coords[0]} {coords[1]}");
            }
            else
            {
                if (args.Length == 2)
                    date = DateTime.Now;
                else
                    date = DateTime.Parse(args[2]);

                int latitude = int.Parse(args[0]);
                int longitude = int.Parse(args[1]);

                coords = GeoHash.GetGeoHash(date, latitude, longitude);
                Console.WriteLine($"Geohash: {coords[0]} {coords[1]}");
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage:  geohash lat long <yyyy-mm-dd>");
            Console.WriteLine("where   lat, long is integer e.g 59 12");
            Console.WriteLine("        long is positive east of Greenwich");
            Console.WriteLine("        if date is omitted, use current");
            Console.WriteLine("or ");
            Console.WriteLine("        geohash -g <yyyy-mm-dd> for globalhash");
            Console.WriteLine("        if date is omitted, use current");
            Environment.Exit(0);
        }
    }
}
