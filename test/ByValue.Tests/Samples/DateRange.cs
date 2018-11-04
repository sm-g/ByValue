using System;
using System.Collections.Generic;

namespace ByValue
{
    public class DateRange : ValueObject
    {
        public DateRange(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException("Start date can not be later than End date");

            StartDate = start.Date;
            EndDate = end.Date;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public override string ToString() => $"{StartDate.ToString("yyyy.MM.dd")}-{EndDate.ToString("yyyy.MM.dd")}";

        protected override IEnumerable<object> Reflect() => new object[] { StartDate, EndDate };
    }
}
