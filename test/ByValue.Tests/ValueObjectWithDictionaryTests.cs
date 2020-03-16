using System.Collections.Generic;

using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class ValueObjectWithDictionaryTests
    {
        [Test]
        public void WithEmptyDicts_ShouldBeEqual()
        {
            var stats = new GamesStatistic(new Dictionary<WinCombination, int>());
            var sameStats = new GamesStatistic(new Dictionary<WinCombination, int>());

            ObjectAssert.AreEqual(stats, sameStats);
        }

        [Test]
        public void WithSameValuesInCollections_ShouldBeEqual()
        {
            var stats = new GamesStatistic(new Dictionary<WinCombination, int>()
            {
                [new WinCombination(1, new HashSet<int> { 1, 2 })] = 3,
                [new WinCombination(2, new HashSet<int> { 1, 7 })] = 4
            });
            var sameStats = new GamesStatistic(new Dictionary<WinCombination, int>()
            {
                [new WinCombination(1, new HashSet<int> { 1, 2 })] = 3,
                [new WinCombination(2, new HashSet<int> { 1, 7 })] = 4
            });

            ObjectAssert.AreEqual(stats, sameStats);
        }

        [Test]
        public void ShouldUseCustomComparer()
        {
            var stats = new MathcingAnythingGamesStatistic(new Dictionary<WinCombination, int>()
            {
                [new WinCombination(1, new HashSet<int> { 1, 2 })] = 3,
            });
            var otherStats = new MathcingAnythingGamesStatistic(new Dictionary<WinCombination, int>()
            {
                [new WinCombination(1, new HashSet<int> { 1, 2 })] = 333
            });

            ObjectAssert.AreEqual(stats, otherStats);
        }

        private class MathcingAnythingGamesStatistic : GamesStatistic
        {
            public MathcingAnythingGamesStatistic(IReadOnlyDictionary<WinCombination, int> winnersCount)
                : base(winnersCount)
            {
            }

            protected override IEnumerable<object> Reflect()
            {
                yield return WinnersCount.ByValue(x => x.UseComparer(new AlwaysEqualsEqualityComparer<int>()));
            }
        }
    }
}