using System;

namespace ByValue
{
    public class Minutes : SingleValueObject<int>
    {
        public Minutes(int value)
            : base(value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        public static implicit operator Minutes(int value)
        {
            return new Minutes(value);
        }

        public static implicit operator Minutes(TimeSpan value)
        {
            return new Minutes(value.Days * 24 * 60 + value.Hours * 60 + value.Minutes);
        }

        public static explicit operator int(Minutes minutes)
        {
            if (minutes == null)
                throw new ArgumentNullException(nameof(minutes));
            return minutes.Value;
        }

        public static explicit operator TimeSpan(Minutes minutes)
        {
            if (minutes == null)
                throw new ArgumentNullException(nameof(minutes));
            return TimeSpan.FromMinutes(minutes.Value);
        }
    }
}