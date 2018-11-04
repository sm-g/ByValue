using System.Collections.Generic;

namespace ByValue
{
    public class NoPropsValue : ValueObject
    {
        protected override IEnumerable<object> Reflect()
        {
            yield break;
        }
    }
}
