using NUnit.Framework;

namespace ByValue
{
    public static class ObjectAssert
    {
        public static void AreEqual(object first, object second)
        {
            // cannot use Assert.AreEqual - NUnit will treat object, implementing IEnumerable, as collections

            Assert.True(first.Equals(second));
            Assert.True(second.Equals(first));
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        public static void AreNotEqual(object first, object second)
        {
            Assert.False(first.Equals(second));
            Assert.False(second.Equals(first));

            // hash code may be same for different object
        }
    }
}