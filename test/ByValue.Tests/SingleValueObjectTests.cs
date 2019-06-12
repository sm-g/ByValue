using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public partial class SingleValueObjectTests
    {
        [Test]
        public void NullSingleValueObjects_ShouldBeEqual()
        {
            var nullVO = (UserId)null;
            var anotherNullVO = (UserId)null;

            Assert.AreEqual(nullVO, anotherNullVO);
            Assert.IsTrue(nullVO == anotherNullVO);
            Assert.IsFalse(nullVO != anotherNullVO);
        }

        [Test]
        public void NullSingleValueObjectOfDifferentTypes_ShouldBeEqual()
        {
            var nullVO = (UserId)null;
            var otherTypeNullVO = (Minutes)null;

            Assert.AreEqual(nullVO, otherTypeNullVO);
        }

        [Test]
        public void NullSingleValueObject_ShouldNotBeEqualToNotNullOne()
        {
            var nullVO = (UserId)null;
            var valueObject = new UserId(1);

            Assert.AreNotEqual(nullVO, valueObject);
            Assert.IsFalse(nullVO == valueObject);
            Assert.IsTrue(nullVO != valueObject);
        }

        [Test]
        public void SingleValueObject_ShouldNotBeEqualToNullOne()
        {
            var valueObject = new UserId(1);
            var nullVO = (UserId)null;

            Assert.AreNotEqual(valueObject, nullVO);
            Assert.IsFalse(valueObject == nullVO);
            Assert.IsTrue(valueObject != nullVO);
        }

        [Test]
        public void SingleValueObjectWithSameValues_ShouldBeEqual()
        {
            var valueObject = new UserId(1);
            var sameValuesVO = new UserId(1);

            Assert.AreEqual(valueObject, sameValuesVO);
            Assert.IsTrue(valueObject == sameValuesVO);
            Assert.IsFalse(valueObject != sameValuesVO);
        }

        [Test]
        public void SingleValueObjectWithDifferentValue_ShouldBeNotEqual()
        {
            var valueObject = new UserId(1);
            var otherValuesVO = new UserId(222);

            Assert.AreNotEqual(valueObject, otherValuesVO);
            Assert.IsFalse(valueObject == otherValuesVO);
            Assert.IsTrue(valueObject != otherValuesVO);
        }

        [Test]
        public void SingleValueObjectsWithSameValues_ShouldHaveSameHashCode()
        {
            var valueObject = new UserId(1);
            var sameValuesVO = new UserId(1);

            Assert.AreEqual(valueObject.GetHashCode(), sameValuesVO.GetHashCode());
            Assert.AreNotEqual(0, valueObject.GetHashCode());
        }

        [Test]
        public void BaseSingleValueObject_ShouldBeNotEqualToDerived()
        {
            var baseId = new UserId(1);
            var derivedId = new DerivedUserId(1, "1");

            Assert.AreNotEqual(baseId, derivedId, "base equals derived");
            Assert.AreNotEqual(derivedId, baseId, "derived equals base");
        }

        [Test]
        public void DerivedSingleValueObjectsWithDifferentValues_ShouldBeNotEqual()
        {
            var derivedId1 = new DerivedUserId(1, "1");
            var derivedId2 = new DerivedUserId(1, "222");

            Assert.AreNotEqual(derivedId1, derivedId2, "derived1 equals derived2");
            Assert.AreNotEqual(derivedId2, derivedId1, "derived2 equals derived1");
            Assert.IsFalse(EqualsAsBase(derivedId1, derivedId2), "equals as base");

            bool EqualsAsBase(UserId x, UserId y) => x.Equals(y) || y.Equals(x);
        }

        [Test]
        public void DerivedSingleValueObjectsWithSameValues_ShouldBeEqual()
        {
            var derivedId1 = new DerivedUserId(1, "1");
            var derivedId2 = new DerivedUserId(1, "1");

            Assert.AreEqual(derivedId1, derivedId2, "derived1 not equals derived2");
            Assert.AreEqual(derivedId2, derivedId1, "derived2 not equals derived1");
            Assert.IsTrue(EqualsAsBase(derivedId1, derivedId2), "not equals as base");

            bool EqualsAsBase(UserId x, UserId y) => x.Equals(y) && y.Equals(x);
        }

        [Test]
        public void DerivedSingleValueObjectsOfDifferentTypesWithSameValues_ShouldBeNotEqual()
        {
            var deletedId = new DeletedUserId(1);
            var vipId = new VipUserId(1);

            Assert.AreNotEqual(deletedId, vipId, "deleted equals vip");
            Assert.AreNotEqual(vipId, deletedId, "vip equals deleted");
        }

        [Test]
        public void ToString_ShouldReturnValue()
        {
            var vo = new UserId(1);

            var result = vo.ToString();

            Assert.AreEqual("1", result);
        }

        [Test]
        public void ToStringWhenValueIsNull_ShouldReturnEmptyString()
        {
            var vo = new StringValueObject(null);

            var result = vo.ToString();

            Assert.AreEqual("", result);
        }

        private class StringValueObject : SingleValueObject<string>
        {
            public StringValueObject(string value)
                : base(value)
            {
            }
        }
    }
}