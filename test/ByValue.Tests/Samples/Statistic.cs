using System;
using System.Collections.Generic;

namespace ByValue
{
    public class Statistic : ValueObject
    {
        public Statistic()
        {
            SetValues(0, 0, 0);
        }

        public Statistic(int done, int total)
        {
            SetValues(done, 0, total);
        }

        public Statistic(int done, int failed, int total)
        {
            SetValues(done, failed, total);
        }

        public int Done { get; private set; }
        public int Failed { get; private set; }
        public int Total { get; private set; }

        public Statistic Complete()
        {
            if (Total == 0)
                throw new InvalidOperationException("Total not set");

            return new Statistic(Total - Failed, Failed, Total);
        }

        protected override IEnumerable<object> Reflect() => new object[] { Done, Failed, Total };

        private void SetValues(int done, int failed, int total)
        {
            if (done < 0)
                throw new ArgumentOutOfRangeException(nameof(done), done, "Value must be positive or zero");
            if (failed < 0)
                throw new ArgumentOutOfRangeException(nameof(failed), failed, "Value must be positive or zero");
            if (total < 0)
                throw new ArgumentOutOfRangeException(nameof(total), total, "Value must be positive or zero");
            if (done + failed > total)
                throw new ArgumentException($"Invariant: done + failed <= total, trying to set: {done} + {failed} > {total}");

            Done = done;
            Failed = failed;
            Total = total;
        }
    }
}
