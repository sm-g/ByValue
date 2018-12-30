using System;

namespace ByValue
{
    public sealed class CompanyCountry : SingleValueObject<string>
    {
        public static readonly CompanyCountry US = new CompanyCountry("US");
        public static readonly CompanyCountry UK = new CompanyCountry("UK");
        public static readonly CompanyCountry CA = new CompanyCountry("CA");

        public CompanyCountry(string value)
            : base(value?.ToUpperInvariant())
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length != 2)
                throw new ArgumentException("Must be 2 char code (like 'US')", nameof(value));
        }

        public static explicit operator CompanyCountry(string value)
        {
            return new CompanyCountry(value);
        }

        public static explicit operator string(CompanyCountry companyCountry)
        {
            if (companyCountry == null)
                throw new ArgumentNullException(nameof(companyCountry));

            return companyCountry.Value;
        }
    }
}