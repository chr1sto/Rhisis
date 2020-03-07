using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Rhisis.World.Tests.Attributes
{
    public sealed class RepeatTestAttribute : DataAttribute
    {
        private readonly int _count;

        public RepeatTestAttribute(int repeatCount)
        {
            this._count = repeatCount;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            foreach (int iterationNumber in Enumerable.Range(start: 1, count: this._count))
            {
                yield return new object[] { iterationNumber };
            }
        }
    }
}
