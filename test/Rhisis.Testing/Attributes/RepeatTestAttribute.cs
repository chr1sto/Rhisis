using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Rhisis.Testing.Attributes
{
    public sealed class RepeatTestAttribute : DataAttribute
    {
        private readonly int _count;

        public RepeatTestAttribute(int repeatCount)
        {
            _count = repeatCount;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            foreach (var iterationNumber in Enumerable.Range(start: 1, count: _count))
            {
                yield return new object[] { iterationNumber };
            }
        }
    }
}
