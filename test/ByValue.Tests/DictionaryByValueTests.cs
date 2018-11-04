using System.Collections.Generic;
using NUnit.Framework;
using IDict = System.Collections.Generic.IDictionary<string, int>;

namespace ByValue
{
    [TestFixture]
    public class DictionaryByValueTests
    {
        [Test]
        public void OfNullDicts_ShouldBeEqual()
        {
            IDict firstDict = null;
            IDict secondDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullDictsOfDifferentTypes_ShouldNotBeEqual()
        {
            IDictionary<string, int> firstDict = null;
            IDictionary<string, long> secondDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, long>(secondDict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfNullDict_ShouldNotBeEqualToOfEmptyDict()
        {
            IDict firstDict = null;
            IDict secondDict = new Dictionary<string, int> { };
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptyDict_ShouldNotBeEqualToOfNullDict()
        {
            IDict firstDict = new Dictionary<string, int> { };
            IDict secondDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfDictOfDifferentTypes_ShouldNotBeEqual()
        {
            IDictionary<string, byte> firstDict = new Dictionary<string, byte> { { "1", 1 } };
            IDictionary<string, int> secondDict = new Dictionary<string, int> { { "1", 1 } };
            var firstByValue = new DictionaryByValue<string, byte>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfShuffledDicts_ShouldBeEqual()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "2", 2 }, { "1", 1 } };
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfDictsWithDifferentItemsCount_ShouldBeNotEqual()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "1", 1 } };
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreNotEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfDictsWithSameItems_ShouldBeEqual()
        {
            IDict firstDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            IDict secondDict = new Dictionary<string, int> { { "1", 1 }, { "2", 2 } };
            var firstByValue = new DictionaryByValue<string, int>(firstDict);
            var secondByValue = new DictionaryByValue<string, int>(secondDict);

            CollectionByValueAssert.AreEqual(firstByValue, secondByValue);
        }

        [Test]
        public void OfEmptyDict_ShouldHaveZeroHashCode()
        {
            IDict firstDict = new Dictionary<string, int> { };
            var firstByValue = new DictionaryByValue<string, int>(firstDict);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        [Test]
        public void OfNullDict_ShouldHaveZeroHashCode()
        {
            IDict firstDict = null;
            var firstByValue = new DictionaryByValue<string, int>(firstDict);

            Assert.AreEqual(0, firstByValue.GetHashCode());
        }

        #region ToString

        [Test]
        public void ToString_ShouldReturnCountOfDictAndAllItems()
        {
            IDict dict = new Dictionary<string, int> { { "qwe", 1 }, { "asd", 2 } };
            var byValue = new DictionaryByValue<string, int>(dict);

            var result = byValue.ToString();

            Assert.AreEqual("2:{[qwe, 1],[asd, 2]}", result);
        }

        [Test]
        public void ToString_OfEmptyDict_ShouldReturnCountOfDictAndAllItems()
        {
            IDict dict = new Dictionary<string, int>();
            var byValue = new DictionaryByValue<string, int>(dict);

            var result = byValue.ToString();

            Assert.AreEqual("0:{}", result);
        }

        [Test]
        public void ToString_OfNullDict_ShouldReturnTypeAndNullMark()
        {
            var byValue = new DictionaryByValue<string, int>((IDict)null);

            var result = byValue.ToString();

            Assert.AreEqual("<null> of KeyValuePair<String,Int32>", result);
        }

        #endregion ToString
    }
}