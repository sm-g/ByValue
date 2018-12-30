using System.Collections.Generic;
using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class ValueObjectWithCollectionTests
    {
        [Test]
        public void WithEmptyCollections_ShouldBeEqual()
        {
            var addressBook = new AddressBook(new MultilineAddress[0]);
            var sameAddressBook = new AddressBook(new MultilineAddress[0]);

            AssertEquals(addressBook, sameAddressBook);
        }

        [Test]
        public void WithSameValuesInCollections_ShouldBeEqual()
        {
            var addressBook = new AddressBook(new[] {
                GetRedSquareAddress(),
                GetEiffelTowerAddress("Paris"),
            });
            var shuffledAddressBook = new AddressBook(new[] {
                GetEiffelTowerAddress("paris"),
                GetRedSquareAddress()
            });

            AssertEquals(addressBook, shuffledAddressBook);
        }

        [Test]
        public void WhenUsedEnhancedComparerForValuesInCollections_ShouldBeNotEqual()
        {
            var addressBook = new EnhancedAddressBook(new[] {
                GetRedSquareAddress(),
                GetEiffelTowerAddress("Paris"),
            });
            var lowercasedAddressBook = new EnhancedAddressBook(new[] {
                GetRedSquareAddress(),
                GetEiffelTowerAddress("paris"),
            });

            AssertNotEquals(addressBook, lowercasedAddressBook);
        }

        private void AssertEquals(object first, object second)
        {
            // cannot use Assert.AreEqual - NUnit will treat object as collections (by base type)

            Assert.True(first.Equals(second));
            Assert.True(second.Equals(first));
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        private void AssertNotEquals(object first, object second)
        {
            Assert.False(first.Equals(second));
            Assert.False(second.Equals(first));
        }

        private MultilineAddress GetRedSquareAddress() => new MultilineAddress(new[] { "Red Square" }, "Moscow", "109012");

        private MultilineAddress GetEiffelTowerAddress(string city) => new MultilineAddress(new[] { "Champ de Mars", "5 Avenue Anatole France" }, city, "75007");

        public class EnhancedAddressBook : AddressBook
        {
            public EnhancedAddressBook(IList<MultilineAddress> list)
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

                var other = obj as EnhancedAddressBook;
                var thisByValue = this.ByValue(x => x.UseComparer(EnhancedAddressComparer.Instance));
                var otherByValue = other.ByValue(x => x.UseComparer(EnhancedAddressComparer.Instance));
                return thisByValue.Equals(otherByValue);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            private class EnhancedAddressComparer : IEqualityComparer<MultilineAddress>
            {
                public static EnhancedAddressComparer Instance => new EnhancedAddressComparer();

                public bool Equals(MultilineAddress x, MultilineAddress y)
                {
                    if (ReferenceEquals(x, y))
                        return true;
                    if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                        return false;

                    return
                        x.AddressLines.ByValue(Ordering.Strict).Equals(y.AddressLines.ByValue(Ordering.Strict)) &&
                        x.City == y.City && // do not ignore case of chars
                        x.PostalCode == y.PostalCode;
                }

                public int GetHashCode(MultilineAddress obj)
                {
                    return 1;
                }
            }
        }
    }
}