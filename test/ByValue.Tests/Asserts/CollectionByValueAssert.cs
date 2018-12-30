using NUnit.Framework;

namespace ByValue
{
    public static class CollectionByValueAssert
    {
        public static void AreEqual(ICollectionByValue first, ICollectionByValue second)
        {
            Assert.AreEqual(first, second);
            Assert.AreEqual(second, first);
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        public static void AreNotEqual(ICollectionByValue first, ICollectionByValue second)
        {
            Assert.AreNotEqual(first, second);
            Assert.AreNotEqual(second, first);

            // hash code may be same for different object
        }
    }
}