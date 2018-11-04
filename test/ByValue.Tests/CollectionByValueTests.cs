using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class CollectionByValueTests
    {
        [Test]
        public void OfNullCollections_ShouldBeEqual()
        {
            var firstCollection = (string[])null;
            var secondCollection = (string[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.Strict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullCollectionsOfDifferentTypes_ShouldNotBeEqual()
        {
            var firstCollection = (string[])null;
            var secondCollection = (int[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<int>(secondCollection, Ordering.Strict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullCollection_ShouldNotBeEqualToOfEmptyCollection()
        {
            var firstCollection = (string[])null;
            var secondCollection = new string[] { };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.NotStrict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.NotStrict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptyCollection_ShouldNotBeEqualToOfNullCollection()
        {
            var firstCollection = new string[] { };
            var secondCollection = (string[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.NotStrict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.NotStrict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsOfDifferentTypes_ShouldNotBeEqual()
        {
            var firstCollection = new byte[] { 1, 2 };
            var secondCollection = new int[] { 1, 2 };
            var firstByValue = new CollectionByValue<byte>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<int>(secondCollection, Ordering.Strict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithStrictOrdering_OfShuffledCollections_ShouldNotBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "2", "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.Strict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithStrictOrdering_OfCollectionsWithSameItems_ShouldBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "1", "2" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.Strict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithNotStrictOrdering_OfShuffledCollections_ShouldBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "2", "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.NotStrict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.NotStrict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsWithDifferentItemsCount_ShouldNotBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.Strict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsWithDifferentOrdering_WithSameItems_ShouldBeEqual()
        {
            var firstCollection = new[] { "1" };
            var secondCollection = new[] { "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.NotStrict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsWithDifferentOrdering_WithShuffledItems_ShouldNotBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "2", "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.NotStrict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfSameCountCollectionsWithSameFirstItem_ShouldHaveSameHashCode()
        {
            var firstCollection = new[] { "1" };
            var secondCollection = new[] { "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);
            var secondByValue = new CollectionByValue<string>(secondCollection, Ordering.Strict);

            Assert.AreEqual(firstByValue.GetHashCode(), secondByValue.GetHashCode());
        }

        [Test]
        public void OfEmptyCollection_ShouldHaveZeroHashCode()
        {
            var firstCollection = new string[] { };
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfNullCollection_ShouldHaveZeroHashCode()
        {
            var firstCollection = (string[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, Ordering.Strict);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        #region ToString

        [Test]
        public void ToString_ShouldReturnCountOfCollectionAndAllItems()
        {
            var byValue = new CollectionByValue<string>(new[] { "qwe", "asd" }, Ordering.Strict);

            var result = byValue.ToString();

            Assert.AreEqual("2:[qwe,asd]", result);
        }

        [Test]
        public void ToString_OfEmptyCollection_ShouldReturnCountOfCollectionAndAllItems()
        {
            var byValue = new CollectionByValue<int>(new int[] { }, Ordering.Strict);

            var result = byValue.ToString();

            Assert.AreEqual("0:[]", result);
        }

        [Test]
        public void ToString_OfNullCollection_ShouldReturnTypeAndNullMark()
        {
            var byValue = new CollectionByValue<int>((int[])null, Ordering.Strict);

            var result = byValue.ToString();

            Assert.AreEqual("<null> of Int32", result);
        }

        #endregion ToString
    }
}