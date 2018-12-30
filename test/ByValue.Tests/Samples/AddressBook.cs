using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ByValue
{
    public class MultilineAddress : ValueObject
    {
        public MultilineAddress(IReadOnlyCollection<string> addressLines, string city, string postalCode)
        {
            AddressLines = addressLines ?? throw new ArgumentNullException(nameof(addressLines));
            City = city ?? throw new ArgumentNullException(nameof(city));
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));

            if (addressLines.Count < 1 || addressLines.Count > 3)
                throw new ArgumentOutOfRangeException(nameof(addressLines), addressLines, "Multiline address should have from 1 to 3 address lines");
        }

        public IReadOnlyCollection<string> AddressLines { get; }
        public string City { get; }
        public string PostalCode { get; }

        protected override IEnumerable<object> Reflect()
        {
            yield return AddressLines.ByValue(Ordering.Strict);
            yield return City.ToUpperInvariant();
            yield return PostalCode;
        }
    }

    public class AddressBook : ReadOnlyCollection<MultilineAddress>
    {
        public AddressBook(IList<MultilineAddress> list)
            : base(list)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;

            var other = obj as AddressBook;
            var thisByValue = this.ByValue();
            var otherByValue = other.ByValue();
            return thisByValue.Equals(otherByValue);
        }

        public override int GetHashCode()
        {
            return Items.Count;
        }
    }
}