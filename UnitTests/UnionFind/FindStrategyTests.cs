using DataStructures.UnionFind;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.UnionFind
{
    /// <summary>
    /// Tests for implementations of <see cref="IFindStrategy"/>
    /// </summary>
    class FindStrategyTests
    {
        /// <summary>
        /// Checks that <see cref="IFindStrategy.Find(int, int[])"/> returns the index of the root parent
        /// </summary>
        /// <param name="findStrategy">The strategy to perform find with</param>
        /// <param name="indexToFind">The index to find the root parent index of</param>
        /// <param name="expectedOutput">The expected output of <see cref="IFindStrategy.Find(int, int[])"/></param>
        [TestCaseSource(nameof(FindStrategyTests.FindTestCaseSource))]
        public void TestFind(IFindStrategy findStrategy, int indexToFind, int expectedOutput)
        {
            var elements = new[] { 4, 1, 2, 0, 1, 5 };

            Assert.That(findStrategy.Find(indexToFind, elements), Is.EqualTo(expectedOutput));
        }

        private static IEnumerable<TestCaseData> FindTestCaseSource()
        {
            yield return new TestCaseData(new FindWithPathCompression(), 3, 1);
        }
    }
}
