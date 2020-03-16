using System.Collections.Generic;

using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class SetByValueTests
    {
        private static readonly Options<string> Options = new Options<string>(false, null);

        [Test]
        public void OfNullSets_ShouldBeEqual()
        {
            var firstSet = (ISet<string>)null;
            var secondSet = (ISet<string>)null;
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<string>(secondSet, Options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullSetsOfDifferentTypes_ShouldNotBeEqual()
        {
            var firstSet = (ISet<string>)null;
            var secondSet = (ISet<int>)null;
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<int>(secondSet, new Options<int>(true, null));

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullSet_ShouldNotBeEqualToOfEmptySet()
        {
            var firstSet = (ISet<string>)null;
            var secondSet = new HashSet<string> { };
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<string>(secondSet, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptySet_ShouldNotBeEqualToOfNullSet()
        {
            var firstSet = new HashSet<string> { };
            var secondSet = (ISet<string>)null;
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<string>(secondSet, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfSetsOfDifferentTypes_ShouldNotBeEqual()
        {
            var firstSet = new HashSet<byte> { 1, 2 };
            var secondSet = new HashSet<int> { 1, 2 };
            var firstByValue = new SetByValue<byte>(firstSet, new Options<byte>(true, null));
            var secondByValue = new SetByValue<int>(secondSet, new Options<int>(true, null));

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfShuffledItems_ShouldBeEqual()
        {
            var firstSet = new HashSet<string> { "1", "2" };
            var secondSet = new HashSet<string> { "2", "1" };
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<string>(secondSet, Options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithCustomComparer_ShouldUseComparer()
        {
            var firstSet = new HashSet<string> { "1", "2" };
            var secondSet = new HashSet<string> { "1", "222" };
            var comparer = new AlwaysEqualsEqualityComparer<string>();
            var firstByValue = new SetByValue<string>(firstSet, new Options<string>(false, comparer));
            var secondByValue = new SetByValue<string>(secondSet, new Options<string>(false, comparer));

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfSetsWithDifferentItemsCount_ShouldNotBeEqual()
        {
            var firstSet = new HashSet<string> { "1", "2" };
            var secondSet = new HashSet<string> { "1" };
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<string>(secondSet, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfSameCountSetsWithSameFirstItem_ShouldHaveSameHashCode()
        {
            var firstSet = new HashSet<string> { "1" };
            var secondSet = new HashSet<string> { "1" };
            var firstByValue = new SetByValue<string>(firstSet, Options);
            var secondByValue = new SetByValue<string>(secondSet, Options);

            Assert.AreEqual(firstByValue.GetHashCode(), secondByValue.GetHashCode());
        }

        [Test]
        public void OfEmptySet_ShouldHaveZeroHashCode()
        {
            var firstSet = new HashSet<string> { };
            var firstByValue = new SetByValue<string>(firstSet, Options);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfNullSet_ShouldHaveZeroHashCode()
        {
            var firstSet = (ISet<string>)null;
            var firstByValue = new SetByValue<string>(firstSet, Options);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfSetWithNotComparableItems_ShouldNotThrowWhenGetHashCode()
        {
            var set = new HashSet<NotComparableClass>(new[]
            {
                new NotComparableClass(),
                new NotComparableClass()
            });

            var byValue = new SetByValue<NotComparableClass>(
                set,
                new Options<NotComparableClass>(false, null));

            Assert.DoesNotThrow(() => byValue.GetHashCode());
        }

        #region ToString

        [Test]
        public void ToString_ShouldReturnCountOfSetAndAllItems()
        {
            var byValue = new SetByValue<string>(new HashSet<string> { "qwe", "asd" }, Options);

            var result = byValue.ToString();

            Assert.AreEqual("2:[qwe,asd]", result);
        }

        [Test]
        public void ToString_OfEmptySet_ShouldReturnCountOfSetAndAllItems()
        {
            var byValue = new SetByValue<int>(new HashSet<int> { }, new Options<int>(false, null));

            var result = byValue.ToString();

            Assert.AreEqual("0:[]", result);
        }

        [Test]
        public void ToString_OfNullSet_ShouldReturnTypeAndNullMark()
        {
            var byValue = new SetByValue<int>(null, new Options<int>(false, null));

            var result = byValue.ToString();

            Assert.AreEqual("<null> of Int32", result);
        }

        #endregion ToString
    }
}