using System;
using System.Globalization;

namespace WPFSample.WPF.Data
{
    public class CustomTime
    {
        private readonly DateTime time;

        public DateTime CurrentTime => time;

        public CustomTime(DateTime date, int hour, int minute)
        {
            time = new DateTime(date.Year, date.Month, date.Day, hour, minute, 00);
        }

        public override string ToString()
        {
            return time.ToString("hh:mm", CultureInfo.InvariantCulture);
        }
    }
}