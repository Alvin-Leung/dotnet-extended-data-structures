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
        public void TestUnify(TestParameters parameters)
        {
            var unionFind = new UnionFind(parameters.Size);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var pair in parameters.TestPairs)
            {
                Assert.That(unionFind.Find(pair.Input), Is.EqualTo(pair.ExpectedOutput));
            }
        }

        private static IEnumerable<TestCaseData> UnifyTestCaseSource()
        {
            yield return new TestCaseData(new TestParameters
            {
                Size = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                TestPairs = new[] { new TestPair(1, 1), new TestPair(2, 2), new TestPair(3, 3) }
            }).SetName("Merge elements to themselves");

            yield return new TestCaseData(new TestParameters
            {
                Size = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                TestPairs = new[] { new TestPair(1, 1), new TestPair(2, 1), new TestPair(3, 1), new TestPair(4, 1), new TestPair(5, 1) }
            }).SetName("Merge all elements to the same group");

            yield return new TestCaseData(new TestParameters
            {
                Size = 101,
                PairsToMerge = new[] { new IndexPair(5, 100), new IndexPair(3, 50), new IndexPair(2, 50), new IndexPair(50, 6), new IndexPair(100, 6) },
                TestPairs = new[] { new TestPair(5, 3), new TestPair(100, 3), new TestPair(3, 3), new TestPair(50, 3), new TestPair(2, 3), new TestPair(6, 3) }
            }).SetName("Merge two groups together");

            yield return new TestCaseData(new TestParameters
            {
                Size = 501,
                PairsToMerge = new[] { new IndexPair(500, 400), new IndexPair(20, 60), new IndexPair(33, 67), new IndexPair(33, 6), new IndexPair(500, 20), new IndexPair(500, 6) },
                TestPairs = new[] 
                { 
                    new TestPair(500, 500),
                    new TestPair(400, 500),
                    new TestPair(20, 500),
                    new TestPair(60, 500),
                    new TestPair(33, 500),
                    new TestPair(6, 500) 
                }
            }).SetName("Merge three groups together");
        }

        public class TestParameters
        {
            public int Size;

            public IndexPair[] PairsToMerge;

            public TestPair[] TestPairs;
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

        public class TestPair
        {
            public int Input { get; }

            public int ExpectedOutput { get; }

            public TestPair(int inputIndex, int expectedOutput)
            {
                this.Input = inputIndex;
                this.ExpectedOutput = expectedOutput;
            }
        }
    }
}