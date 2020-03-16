using System.Collections.Generic;

using NUnit.Framework;

using IDict = System.Collections.Generic.IDictionary<string, int>;

namespace ByValue
{
    [TestFixture]
    public class DictionaryByValueTests
    {
        private static readonly DictionaryOptions<string, int> Options = new DictionaryOptions<string, int>(default, default);

        [Test]
        public void OfNullDicts_ShouldBeEqual()
        {
            IDict firstDict = null;
            IDict secondDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullDictsOfDifferentTypes_ShouldNotBeEqual()
        {
            IDictionary<string, int> firstDict = null;
            IDictionary<string, long> secondDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, long>(secondDict, new DictionaryOptions<string, long>(default, default));

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullDict_ShouldNotBeEqualToOfEmptyDict()
        {
            IDict firstDict = null;
            IDict secondDict = new Dictionary<string, int> { };
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptyDict_ShouldNotBeEqualToOfNullDict()
        {
            IDict firstDict = new Dictionary<string, int> { };
            IDict secondDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfDictOfDifferentTypes_ShouldNotBeEqual()
        {
            IDictionary<string, byte> firstDict = new Dictionary<string, byte> { { "1", 1 } };
            IDictionary<string, int> secondDict = new Dictionary<string, int> { { "1", 1 } };
            var firstByValue = new DictionaryByValue<string, byte>(firstDict, new DictionaryOptions<string, byte>(default, default));
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfShuffledDicts_ShouldBeEqual()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "2", 2 }, { "1", 1 } };
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfDictsWithDifferentItemsCount_ShouldBeNotEqual()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "1", 1 } };
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfDictsWithSameItems_ShouldBeEqual()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, Options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithCustomKeysComparer_ShouldUseComparer()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "1", 1 }, { "222", 2 } };
            var keysComparer = new AlwaysEqualsEqualityComparer<string>();
            var options = new DictionaryOptions<string, int>(keysComparer, default);
            var firstByValue = new DictionaryByValue<string, int>(firstDict, options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void WithCustomValuesComparer_ShouldUseComparer()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "1", 1 }, { "2", 222 } };
            var valuesComparer = new AlwaysEqualsEqualityComparer<int>();
            var options = new DictionaryOptions<string, int>(default, valuesComparer);
            var firstByValue = new DictionaryByValue<string, int>(firstDict, options);
            var secondByValue = new DictionaryByValue<string, int>(secondDict, options);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptyDict_ShouldHaveZeroHashCode()
        {
            IDict firstDict = new Dictionary<string, int> { };
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfNullDict_ShouldHaveZeroHashCode()
        {
            IDict firstDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict, Options);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfDictWithNotComparableKeys_ShouldNotThrowWhenGetHashCode()
        {
            IDictionary<NotComparableClass, int> dict = new Dictionary<NotComparableClass, int>()
            {
                [new NotComparableClass()] = 1,
                [new NotComparableClass()] = 2
            };

            var byValue = new DictionaryByValue<NotComparableClass, int>(
                dict,
                new DictionaryOptions<NotComparableClass, int>(null, null));

            Assert.DoesNotThrow(() => byValue.GetHashCode());
        }

        #region ToString

        [Test]
        public void ToString_ShouldReturnCountOfDictAndAllItems()
        {
            IDict dict = new Dictionary<string, int> { { "qwe", 1 }, { "asd", 2 } };
            var byValue = new DictionaryByValue<string, int>(dict, Options);

            var result = byValue.ToString();

            Assert.AreEqual("2:{[qwe, 1],[asd, 2]}", result);
        }

        [Test]
        public void ToString_OfEmptyDict_ShouldReturnCountOfDictAndAllItems()
        {
            IDict dict = new Dictionary<string, int>();
            var byValue = new DictionaryByValue<string, int>(dict, Options);

            var result = byValue.ToString();

            Assert.AreEqual("0:{}", result);
        }

        [Test]
        public void ToString_OfNullDict_ShouldReturnTypeAndNullMark()
        {
            var byValue = new DictionaryByValue<string, int>((IDict)null, default);

            var result = byValue.ToString();

            Assert.AreEqual("<null> of KeyValuePair<String,Int32>", result);
        }

        #endregion ToString
    }
}