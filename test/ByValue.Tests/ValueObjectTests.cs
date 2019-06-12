using System;
using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class ValueObjectTests
    {
        [Test]
        public void NullValueObjects_ShouldBeEqual()
        {
            var nullVO = (DateRange)null;
            var anotherNullVO = (DateRange)null;

            Assert.AreEqual(nullVO, anotherNullVO);
            Assert.IsTrue(nullVO == anotherNullVO);
            Assert.IsFalse(nullVO != anotherNullVO);
        }

        [Test]
        public void NullValueObjectOfDifferentTypes_ShouldBeEqual()
        {
            var nullVO = (DateRange)null;
            var otherTypeNullVO = (Statistic)null;

            Assert.AreEqual(nullVO, otherTypeNullVO);
        }

        [Test]
        public void NullValueObject_ShouldNotBeEqualToNotNullOne()
        {
            var nullVO = (DateRange)null;
            var valueObject = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));

            Assert.AreNotEqual(nullVO, valueObject);
            Assert.IsFalse(nullVO == valueObject);
            Assert.IsTrue(nullVO != valueObject);
        }

        [Test]
        public void ValueObject_ShouldNotBeEqualToNullOne()
        {
            var valueObject = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));
            var nullVO = (DateRange)null;

            Assert.AreNotEqual(valueObject, nullVO);
            Assert.IsFalse(valueObject == nullVO);
            Assert.IsTrue(valueObject != nullVO);
        }

        [Test]
        public void ValueObjectsWithSameValues_ShouldBeEqual()
        {
            var valueObject = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));
            var sameValuesVO = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));

            Assert.AreEqual(valueObject, sameValuesVO);
            Assert.IsTrue(valueObject == sameValuesVO);
            Assert.IsFalse(valueObject != sameValuesVO);
        }

        [Test]
        public void ValueObjectsWithoutReflectedProperties_ShouldBeEqual()
        {
            var valueObject = new NoPropsValue();
            var sameValuesVO = new NoPropsValue();

            Assert.AreEqual(valueObject, sameValuesVO);
            Assert.IsTrue(valueObject == sameValuesVO);
            Assert.IsFalse(valueObject != sameValuesVO);
        }

        [Test]
        public void ValueObjectsWithDifferentValue_ShouldBeNotEqual()
        {
            var valueObject = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));
            var otherValuesVO = new DateRange(DateTime.Parse("2018.01.01"), DateTime.Parse("2018.03.30"));

            Assert.AreNotEqual(valueObject, otherValuesVO);
            Assert.IsFalse(valueObject == otherValuesVO);
            Assert.IsTrue(valueObject != otherValuesVO);
        }

        [Test]
        public void ValueObjectsWithSameValues_ShouldHaveSameHashCode()
        {
            var valueObject = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));
            var sameValuesVO = new DateRange(DateTime.Parse("2018.01.30"), DateTime.Parse("2018.03.30"));

            Assert.AreEqual(valueObject.GetHashCode(), sameValuesVO.GetHashCode());
            Assert.AreNotEqual(0, valueObject.GetHashCode());
        }

        [Test]
        public void BaseValueObject_ShouldBeNotEqualToDerived()
        {
            var baseAddress = new Address("123", "Line 1", "Fooburg");
            var derivedAddress = new DerivedAddress("123", "Line 1", "Fooburg", "Ontario");

            Assert.AreNotEqual(baseAddress, derivedAddress, "base equals derived");
            Assert.AreNotEqual(derivedAddress, baseAddress, "derived equals base");
        }

        [Test]
        public void DerivedValueObjectsWithDifferentValues_ShouldBeNotEqual()
        {
            var derivedAddress1 = new DerivedAddress("123", "Line 1", "Fooburg", "Ontario");
            var derivedAddress2 = new DerivedAddress("123", "Line 1", "Fooburg", "Alberta");

            Assert.AreNotEqual(derivedAddress1, derivedAddress2, "derived1 equals derived2");
            Assert.AreNotEqual(derivedAddress2, derivedAddress1, "derived2 equals derived1");
            Assert.IsFalse(EqualsAsBase(derivedAddress1, derivedAddress2), "equals as base");

            bool EqualsAsBase(Address x, Address y) => x.Equals(y) || y.Equals(x);
        }

        [Test]
        public void DerivedValueObjectsWithSameValues_ShouldBeEqual()
        {
            var derivedAddress1 = new DerivedAddress("123", "Line 1", "Fooburg", "Ontario");
            var derivedAddress2 = new DerivedAddress("123", "Line 1", "Fooburg", "Ontario");

            Assert.AreEqual(derivedAddress1, derivedAddress2, "derived1 not equals derived2");
            Assert.AreEqual(derivedAddress2, derivedAddress1, "derived2 not equals derived1");
            Assert.IsTrue(EqualsAsBase(derivedAddress1, derivedAddress2), "not equals as base");

            bool EqualsAsBase(Address x, Address y) => x.Equals(y) && y.Equals(x);
        }

        [Test]
        public void DerivedValueObjectsOfDifferentTypesWithSameValues_ShouldBeNotEqual()
        {
            var finlandAddress = new FinlandAddress("123", "Line 1", "Helsinburg");
            var germanyAddress = new GermanyAddress("123", "Line 1", "Helsinburg");

            Assert.AreNotEqual(finlandAddress, germanyAddress, "finland equals germany");
            Assert.AreNotEqual(germanyAddress, finlandAddress, "germany equals finland");
        }

        [Test]
        public void ToString_ShouldReturnReflectedProperties()
        {
            var vo = new Statistic(1, 2, 3);

            var result = vo.ToString();

            Assert.AreEqual("{1,2,3}", result);
        }

        [Test]
        public void ToStringWhenReflectedValuesIsEmpty_ShouldWork()
        {
            var vo = new NoPropsValue();

            var result = vo.ToString();

            Assert.AreEqual("{}", result);
        }
    }
}