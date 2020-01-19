using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public partial class MultiSetEqualityComparerTests
    {
        [Test]
        public void NullCollections_ShouldBeEqual()
        {
            var first = (string[])null;
            var second = (string[])null;

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void EmptyCollections_ShouldBeEqual()
        {
            var first = new string[] { };
            var second = new string[] { };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void NullCollection_ShouldBeNotEqualToNotNullOne()
        {
            var first = (string[])null;
            var second = new string[] { };

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void NotNullCollection_ShouldBeNotEqualToNullOne()
        {
            var first = new string[] { };
            var second = (string[])null;

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void CollectionsWithSameItems_ShouldBeEqual()
        {
            var first = new[] { "a", "b" };
            var second = new[] { "a", "b" };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void ShuffledCollectionsWithSameItems_ShouldBeEqual()
        {
            var first = new[] { "a", "b" };
            var second = new[] { "b", "a" };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void MultisetCollectionsWithDifferentCountsOfItem_ShouldBeNotEqual()
        {
            var first = new[] { "a", "b", "b" };
            var second = new[] { "a", "a", "b" };

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void ShouldHandleNullItems()
        {
            var first = new[] { "a", null };
            var second = new[] { "a", null };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void CollectionsWithDifferentNullItemsCount_ShouldBeNotEqual()
        {
            var first = new[] { "a", null };
            var second = new[] { null, "a", null };

            Assert.IsFalse(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void CollectionsWithSameCountOfNulls_ShouldBeEqual()
        {
            var first = new[] { "a", null, null };
            var second = new[] { null, "a", null };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second));
        }

        [Test]
        public void ShouldUseGivenEqualityComparer()
        {
            var first = new[] { 1 };
            var second = new[] { 22 };

            Assert.IsTrue(MultiSetEqualityComparer.Equals(first, second, new AlwaysEqualsEqualityComparer<int>()));
        }
    }
}