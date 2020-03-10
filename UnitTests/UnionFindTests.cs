using DataStructures;
using NUnit.Framework;
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

        [TestCaseSource(nameof(UnionFindTests.UnifyTestCaseSource))]
        public void TestUnify(UnifyTestParameters parameters)
        {
            var unionFind = new UnionFind(1000);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var findResult in parameters.ExpectedFindResult)
            {
                Assert.That(unionFind.Find(findResult.FindIndex), Is.EqualTo(findResult.RootParentIndex));
            }
        }

        private static IEnumerable<TestCaseData> UnifyTestCaseSource()
        {
            yield return new TestCaseData(new UnifyTestParameters
            {
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                ExpectedFindResult = new[] { new FindResult(1, 1), new FindResult(2, 2), new FindResult(3, 3) }
            }).SetName("Merge elements to themselves");

            yield return new TestCaseData(new UnifyTestParameters
            {
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                ExpectedFindResult = new[] { new FindResult(1, 1), new FindResult(2, 1), new FindResult(3, 1), new FindResult(4, 1), new FindResult(5, 1) }
            }).SetName("Merge all elements to the same group");

            yield return new TestCaseData(new UnifyTestParameters
            {
                PairsToMerge = new[] { new IndexPair(5, 100), new IndexPair(3, 50), new IndexPair(2, 50), new IndexPair(50, 6), new IndexPair(100, 6) },
                ExpectedFindResult = new[] { new FindResult(5, 3), new FindResult(100, 3), new FindResult(3, 3), new FindResult(50, 3), new FindResult(2, 3), new FindResult(6, 3) }
            }).SetName("Merge two groups together");

            yield return new TestCaseData(new UnifyTestParameters
            {
                PairsToMerge = new[] { new IndexPair(500, 400), new IndexPair(20, 60), new IndexPair(33, 67), new IndexPair(33, 6), new IndexPair(500, 20), new IndexPair(500, 6) },
                ExpectedFindResult = new[] 
                { 
                    new FindResult(500, 500),
                    new FindResult(400, 500),
                    new FindResult(20, 500),
                    new FindResult(60, 500),
                    new FindResult(33, 500),
                    new FindResult(6, 500) 
                }
            }).SetName("Merge three groups together");
        }

        public class UnifyTestParameters
        {
            public IndexPair[] PairsToMerge;

            public FindResult[] ExpectedFindResult;
        }

        public class IndexPair
        {
            public int FirstIndex { get; }

            public int SecondIndex { get; }

            public IndexPair(int firstIndex, int secondIndex)
            {
                this.FirstIndex = firstIndex;
                this.SecondIndex = secondIndex;
            }
        }

        public class FindResult
        {
            public int FindIndex { get; }

            public int RootParentIndex { get; }

            public FindResult(int findIndex, int rootParentIndex)
            {
                this.FindIndex = findIndex;
                this.RootParentIndex = rootParentIndex;
            }
        }
    }
}