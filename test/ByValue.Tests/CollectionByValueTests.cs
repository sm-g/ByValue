using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class CollectionByValueTests
    {
        private static readonly Options<string> StrictOptions = new Options<string>(true, null);
        private static readonly Options<string> NotStrictOptions = new Options<string>(false, null);

        [Test]
        public void OfNullCollections_ShouldBeEqual()
        {
            var firstCollection = (string[])null;
            var secondCollection = (string[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, StrictOptions);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullCollectionsOfDifferentTypes_ShouldNotBeEqual()
        {
            var firstCollection = (string[])null;
            var secondCollection = (int[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<int>(secondCollection, new Options<int>(true, null));

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullCollection_ShouldNotBeEqualToOfEmptyCollection()
        {
            var firstCollection = (string[])null;
            var secondCollection = new string[] { };
            var firstByValue = new CollectionByValue<string>(firstCollection, NotStrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, NotStrictOptions);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptyCollection_ShouldNotBeEqualToOfNullCollection()
        {
            var firstCollection = new string[] { };
            var secondCollection = (string[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, NotStrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, NotStrictOptions);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsOfDifferentTypes_ShouldNotBeEqual()
        {
            var firstCollection = new byte[] { 1, 2 };
            var secondCollection = new int[] { 1, 2 };
            var firstByValue = new CollectionByValue<byte>(firstCollection, new Options<byte>(true, null));
            var secondByValue = new CollectionByValue<int>(secondCollection, new Options<int>(true, null));

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithStrictOrdering_OfShuffledCollections_ShouldNotBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "2", "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, StrictOptions);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithStrictOrdering_OfCollectionsWithSameItems_ShouldBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "1", "2" };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, StrictOptions);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithNotStrictOrdering_OfShuffledCollections_ShouldBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "2", "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, NotStrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, NotStrictOptions);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithCustomComparer_ShouldUseComparer([Values]bool strictOrdering)
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "1", "222" };
            var comparer = new AlwaysEqualsEqualityComparer<string>();
            var firstByValue = new CollectionByValue<string>(firstCollection, new Options<string>(strictOrdering, comparer));
            var secondByValue = new CollectionByValue<string>(secondCollection, new Options<string>(strictOrdering, comparer));

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsWithDifferentItemsCount_ShouldNotBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, StrictOptions);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsWithDifferentOrdering_WithSameItems_ShouldBeEqual()
        {
            var firstCollection = new[] { "1" };
            var secondCollection = new[] { "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, NotStrictOptions);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfCollectionsWithDifferentOrdering_WithShuffledItems_ShouldNotBeEqual()
        {
            var firstCollection = new[] { "1", "2" };
            var secondCollection = new[] { "2", "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, NotStrictOptions);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfSameCountCollectionsWithSameFirstItem_ShouldHaveSameHashCode()
        {
            var firstCollection = new[] { "1" };
            var secondCollection = new[] { "1" };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);
            var secondByValue = new CollectionByValue<string>(secondCollection, StrictOptions);

            Assert.AreEqual(firstByValue.GetHashCode(), secondByValue.GetHashCode());
        }

        [Test]
        public void OfEmptyCollection_ShouldHaveZeroHashCode()
        {
            var firstCollection = new string[] { };
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfNullCollection_ShouldHaveZeroHashCode()
        {
            var firstCollection = (string[])null;
            var firstByValue = new CollectionByValue<string>(firstCollection, StrictOptions);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        #region ToString

        [Test]
        public void ToString_ShouldReturnCountOfCollectionAndAllItems()
        {
            var byValue = new CollectionByValue<string>(new[] { "qwe", "asd" }, StrictOptions);

            var result = byValue.ToString();

            Assert.AreEqual("2:[qwe,asd]", result);
        }

        [Test]
        public void ToString_OfEmptyCollection_ShouldReturnCountOfCollectionAndAllItems()
        {
            var byValue = new CollectionByValue<int>(new int[] { }, new Options<int>(true, null));

            var result = byValue.ToString();

            Assert.AreEqual("0:[]", result);
        }

        [Test]
        public void ToString_OfNullCollection_ShouldReturnTypeAndNullMark()
        {
            var byValue = new CollectionByValue<int>((int[])null, new Options<int>(true, null));

            var result = byValue.ToString();

            Assert.AreEqual("<null> of Int32", result);
        }

        #endregion ToString
    }
}