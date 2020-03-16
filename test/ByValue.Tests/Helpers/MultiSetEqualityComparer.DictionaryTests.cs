using System.Collections.Generic;

using NUnit.Framework;

using IDict = System.Collections.Generic.IDictionary<string, int>;

namespace ByValue
{
    [TestFixture]
    public partial class MultiSetEqualityComparerTests
    {
        [Test]
        public void NullDicts_ShouldBeEqual()
        {
            var first = (IDict)null;
            var second = (IDict)null;

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void EmptyDicts_ShouldBeEqual()
        {
            var first = new Dictionary<string, int>();
            var second = new Dictionary<string, int>();

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void NullDict_ShouldBeNotEqualToNotNullOne()
        {
            var first = (IDict)null;
            var second = new Dictionary<string, int>();

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void NotNullDict_ShouldBeNotEqualToNullOne()
        {
            var first = new Dictionary<string, int>();
            var second = (IDict)null;

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void DictsWithSameKeyValuePairs_ShouldBeEqual()
        {
            var first = new Dictionary<string, int> { { "1", 1 } };
            var second = new Dictionary<string, int> { { "1", 1 } };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void DictsWithDifferentValues_ShouldBeNotEqual()
        {
            var first = new Dictionary<string, int> { { "1", 1 } };
            var second = new Dictionary<string, int> { { "1", 2 } };

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void DictsWithDifferentKeys_ShouldBeNotEqual()
        {
            var first = new Dictionary<string, int> { { "1", 1 } };
            var second = new Dictionary<string, int> { { "2", 1 } };

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void ShouldHandleNullValues()
        {
            var first = new Dictionary<string, int?> { { "1", null } };
            var second = new Dictionary<string, int?> { { "1", null } };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, default, default));
        }

        [Test]
        public void ShouldUseGivenKeysEqualityComparer()
        {
            var first = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            var second = new Dictionary<string, int> { { "222", 1 }, { "3", 2 } };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, new AlwaysEqualsEqualityComparer<string>(), default));
        }

        [Test]
        public void ShouldUseGivenKeysEqualityComparer_ComparingValues()
        {
            var first = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            var second = new Dictionary<string, int> { { "222", 1 }, { "3", 3 } };

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second, new AlwaysEqualsEqualityComparer<string>(), default));
        }

        [Test]
        public void ShouldUseGivenValuesEqualityComparer()
        {
            var first = new Dictionary<string, int> { { "1", 1 } };
            var second = new Dictionary<string, int> { { "1", 222 } };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, default, new AlwaysEqualsEqualityComparer<int>()));
        }

        [Test]
        public void ShouldUseGivenEqualityComparers()
        {
            var first = new Dictionary<string, int> { { "1", 1 } };
            var second = new Dictionary<string, int> { { "2", 222 } };

            var result = MultiSetEqualityComparer.Equals(
                first,
                second,
                new AlwaysEqualsEqualityComparer<string>(),
                new AlwaysEqualsEqualityComparer<int>());

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldHandleNullValuesWithCustomKeysEqualityComparer()
        {
            var first = new Dictionary<string, int?> { { "1", null } };
            var second = new Dictionary<string, int?> { { "2", null } };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, new AlwaysEqualsEqualityComparer<string>(), default));
        }

        [Test]
        public void ShouldHandleNullValuesWithCustomValuesEqualityComparer()
        {
            var first = new Dictionary<string, int?> { { "1", null }, { "2", 2 } };
            var second = new Dictionary<string, int?> { { "1", null }, { "2", 1 } };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, default, new AlwaysEqualsEqualityComparer<int?>()));
        }
    }
}