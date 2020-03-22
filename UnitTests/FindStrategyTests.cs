using DataStructures;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    /// <summary>
    /// Tests for implementations of <see cref="IFindStrategy"/>
    /// </summary>
    class FindStrategyTests
    {
        /// <summary>
        /// Checks that <see cref="IFindStrategy.Find(int)"/> returns the index of the root parent
        /// </summary>
        /// <param name="findStrategy"></param>
        /// <param name="indexToFind"></param>
        /// <param name="expectedOutput"></param>
        [TestCaseSource(nameof(FindStrategyTests.FindTestCaseSource))]
        public void TestFind(IFindStrategy findStrategy, int indexToFind, int expectedOutput)
        {
            Assert.That(findStrategy.Find(indexToFind), Is.EqualTo(expectedOutput));
        }

        private static IEnumerable<TestCaseData> FindTestCaseSource()
        {
            var elements = new[] { 4, 1, 2, 0, 1, 5 };

            yield return new TestCaseData(new FindWithPathCompression(elements), 3, 1);
        }
    }
}
