using System;
using System.Collections.Generic;

namespace ByValue
{
    public class Address : ValueObject
    {
        public Address(string postalCode, string addressLine, string city)
        {
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            AddressLine = addressLine ?? throw new ArgumentNullException(nameof(addressLine));
            City = city ?? throw new ArgumentNullException(nameof(city));
        }

        public string AddressLine { get; }
        public string City { get; }
        public string PostalCode { get; }

        protected override IEnumerable<object> Reflect()
        {
            yield return AddressLine;
            yield return City;
            yield return PostalCode;
        }
    }

    public class DerivedAddress : Address
    {
        public DerivedAddress(string postalCode, string addressLine, string city, string province)
            : base(postalCode, addressLine, city)
        {
            Province = province ?? throw new ArgumentNullException(nameof(province));
        }

        public string Province { get; }

        protected override IEnumerable<object> Reflect()
        {
            foreach (var item in base.Reflect())
            {
                yield return item;
            }
            yield return Province;
        }
    }

    public class FinlandAddress : Address
    {
        public FinlandAddress(string postalCode, string addressLine, string city)
            : base(postalCode, addressLine, city)
        {
        }
    }

    public class GermanyAddress : Address
    {
        public GermanyAddress(string postalCode, string addressLine, string city)
            : base(postalCode, addressLine, city)
        {
        }
    }
}