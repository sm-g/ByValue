using System.Collections.Generic;

using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class ValueObjectWithSetTests
    {
        [Test]
        public void WithEmptySets_ShouldBeEqual()
        {
            var winCombination = new WinCombination(1, new HashSet<int> { });
            var sameWinCombination = new WinCombination(1, new HashSet<int> { });

            ObjectAssert.AreEqual(winCombination, sameWinCombination);
        }

        [Test]
        public void WithSameValuesInCollections_ShouldBeEqual()
        {
            var winCombination = new WinCombination(1, new HashSet<int> { 1, 2, 3 });
            var shuffledWinCombination = new WinCombination(1, new HashSet<int> { 3, 2, 1 });

            ObjectAssert.AreEqual(winCombination, shuffledWinCombination);
        }

        [Test]
        public void ShouldUseCustomComparer()
        {
            var winCombination = new MathcingAnythingWinCombination(1, new HashSet<int> { 1, 2, 3 });
            var otherWinCombination = new MathcingAnythingWinCombination(1, new HashSet<int> { 1, 2, 333 });

            ObjectAssert.AreEqual(winCombination, otherWinCombination);
        }

        private class MathcingAnythingWinCombination : WinCombination
        {
            public MathcingAnythingWinCombination(int gameId, ISet<int> numbers)
                : base(gameId, numbers)
            {
            }

            protected override IEnumerable<object> Reflect()
            {
                yield return GameId;
                yield return Numbers.ByValue(x => x.UseComparer(new AlwaysEqualsEqualityComparer<int>()));
            }
        }
    }
}