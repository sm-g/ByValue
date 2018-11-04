using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ByValue
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [TestCase(typeof(string), ExpectedResult = "String")]
        [TestCase(typeof(string[]), ExpectedResult = "String[]")]
        [TestCase(typeof(List<string>), ExpectedResult = "List<String>")]
        [TestCase(typeof(ISet<string>), ExpectedResult = "ISet<String>")]
        [TestCase(typeof(Dictionary<string, List<string>>), ExpectedResult = "Dictionary<String,List<String>>")]
        [TestCase(typeof(List<(string, int)>), ExpectedResult = "List<ValueTuple<String,Int32>>")]
        public string GetFriendlyName(Type type)
        {
            return type.GetFriendlyName();
        }

        [TestCase(typeof(string), ExpectedResult = "System.String")]
        [TestCase(typeof(string[]), ExpectedResult = "System.String[]")]
        [TestCase(typeof(List<string>), ExpectedResult = "List<System.String>")]
        [TestCase(typeof(List<(string, int)>), ExpectedResult = "List<ValueTuple<System.String,System.Int32>>")]
        public string GetFriendlyFullName(Type type)
        {
            return type.GetFriendlyFullName();
        }
    }
}
