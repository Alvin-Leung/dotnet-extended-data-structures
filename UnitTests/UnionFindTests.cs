using DataStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class UnionFindTests
    {
        [TestCase(1, 0, 0)]
        [TestCase(2, 1, 1)]
        [TestCase(4, 3, 3)]
        [TestCase(10, 9, 9)]
        [Test]
        public void TestFind(int size, int indexToFind, int expectedParent)
        {
            var unionFind = new UnionFind(size);
            Assert.That(unionFind.Find(indexToFind), Is.EqualTo(expectedParent));
        }

        [TestCaseSource(nameof(UnionFindTests.UnionTestCaseSource))]
        public void TestUnion(Tuple<int, int>[] pairsToMerge, int indexToFind, int expectedParent)
        {
            var unionFind = new UnionFind(1000);

            foreach (var pair in pairsToMerge)
            {
                unionFind.Union(pair.Item1, pair.Item2);
            }

            Assert.That(unionFind.Find(indexToFind), Is.EqualTo(expectedParent));
        }

        private static IEnumerable<TestCaseData> UnionTestCaseSource()
        {
            yield return new TestCaseData(new[] { new Tuple<int, int>(1, 1) }, 1, 1);
            yield return new TestCaseData(new[] { new Tuple<int, int>(1, 2) }, 1, 2);
        }
    }
}