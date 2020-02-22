using System;
using System.Collections.Generic;
using System.Text;

namespace GeoHash
{
    // A simple date class, with just the right helper functions
    public class GDate
    {
        private DateTime _date;
        private DateTime _dowJonesDate;

        private static DateTime w30breakdate = new DateTime(2008, 5, 27);

        public GDate(int year, int month, int day)
        {
            _date = new DateTime(year, month, day);
            _dowJonesDate = _date;
        }

        public GDate(DateTime date, DateTime dowJonesDate)
        {
            _date = date;
            _dowJonesDate = dowJonesDate;
        }

        public static GDate ForLongitude(DateTime date, int longitude)
        {
            var dowJonesDate = date;

            // W30 adjustments
            if (date >= w30breakdate && longitude > -30)
                dowJonesDate = date.AddDays(-1);

            GDate inst = new GDate(date, dowJonesDate);

            return inst;
        }
        public static GDate ForGlobalhash(DateTime date)
        {
            // W30 adjustments
            var dowJonesDate = date.AddDays(-1);

            GDate inst = new GDate(date, dowJonesDate);

            return inst;
        }

        public override string ToString()
        {
            return String.Format("{0:D4}-{1:D2}-{2:D2}", _date.Year, _date.Month, _date.Day);
        }

        public string DowJonesString()
        {
            return String.Format("{0:D4}-{1:D2}-{2:D2}", _dowJonesDate.Year, _dowJonesDate.Month, _dowJonesDate.Day);
        }
    }
}
