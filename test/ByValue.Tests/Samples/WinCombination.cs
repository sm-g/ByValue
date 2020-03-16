using System;
using System.Collections.Generic;

namespace ByValue
{
    public class WinCombination : ValueObject
    {
        public WinCombination(int gameId, ISet<int> numbers)
        {
            GameId = gameId;
            Numbers = numbers ?? throw new ArgumentNullException(nameof(numbers));
        }

        public int GameId { get; }
        public ISet<int> Numbers { get; }

        protected override IEnumerable<object> Reflect()
        {
            yield return GameId;
            yield return Numbers.ByValue();
        }
    }

    public class GamesStatistic : ValueObject
    {
        public GamesStatistic(IReadOnlyDictionary<WinCombination, int> winnersCount)
        {
            WinnersCount = winnersCount ?? throw new ArgumentNullException(nameof(winnersCount));
        }

        public IReadOnlyDictionary<WinCombination, int> WinnersCount { get; }

        protected override IEnumerable<object> Reflect()
        {
            yield return WinnersCount.ByValue();
        }
    }
}