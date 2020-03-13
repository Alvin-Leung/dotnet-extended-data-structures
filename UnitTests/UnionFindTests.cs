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

        [TestCaseSource(nameof(UnionFindTests.GetComponentSizeTestCaseSource))]
        public void TestGetComponentSize(TestParameters parameters)
        {
            var unionFind = new UnionFind(parameters.Size);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var pair in parameters.TestPairs)
            {
                Assert.That(unionFind.GetComponentSize(pair.Input), Is.EqualTo(pair.ExpectedOutput));
            }
        }

        private static IEnumerable<TestCaseData> UnifyTestCaseSource()
        {
            yield return new TestCaseData(new TestParameters
            {
                Size = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                TestPairs = new[] { new TestPair(1, 1), new TestPair(2, 2), new TestPair(3, 3) }
            }).SetName("Unify each element to itself, then ensure each element points to itself");

            yield return new TestCaseData(new TestParameters
            {
                Size = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                TestPairs = new[] { new TestPair(1, 1), new TestPair(2, 1), new TestPair(3, 1), new TestPair(4, 1), new TestPair(5, 1) }
            }).SetName("Unify all elements into the same component, then ensure all elements have the same parent");

            yield return new TestCaseData(new TestParameters
            {
                Size = 101,
                PairsToMerge = new[] { new IndexPair(5, 100), new IndexPair(3, 50), new IndexPair(2, 50), new IndexPair(50, 6), new IndexPair(100, 6) },
                TestPairs = new[] { new TestPair(5, 3), new TestPair(100, 3), new TestPair(3, 3), new TestPair(50, 3), new TestPair(2, 3), new TestPair(6, 3) }
            }).SetName("Unify two components together, then ensure all elements have the same parent");

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
            }).SetName("Unify three components together, then ensure all elements have the same parent");

            yield return new TestCaseData(new TestParameters
            {
                Size = 11,
                PairsToMerge = new[]
                {
                    new IndexPair(10, 0),
                    new IndexPair(1, 9),
                    new IndexPair(2, 8),
                    new IndexPair(3, 7),
                    new IndexPair(4, 6),
                    new IndexPair(5, 10),
                    new IndexPair(5, 1),
                    new IndexPair(8, 5),
                    new IndexPair(3, 4)
                },
                TestPairs = new[]
                {
                    new TestPair(0, 10),
                    new TestPair(1, 10),
                    new TestPair(2, 10),
                    new TestPair(3, 3),
                    new TestPair(4, 3),
                    new TestPair(5, 10),
                    new TestPair(6, 3),
                    new TestPair(7, 3),
                    new TestPair(8, 10),
                    new TestPair(9, 10),
                    new TestPair(10, 10)
                }
            }).SetName("Unify elements into two different compoents, then ensure that they each have the correct parent");
        }

        private static IEnumerable<TestCaseData> GetComponentSizeTestCaseSource()
        {
            yield return new TestCaseData(new TestParameters
            {
                Size = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                TestPairs = new[] { new TestPair(1, 1), new TestPair(2, 1), new TestPair(3, 1) }
            }).SetName("Get the component size of elements that have not yet been grouped");

            yield return new TestCaseData(new TestParameters
            {
                Size = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                TestPairs = new[] { new TestPair(1, 5), new TestPair(2, 5), new TestPair(3, 5), new TestPair(4, 5), new TestPair(5, 5) }
            }).SetName("Get the component size of elements that are all in the same component");

            yield return new TestCaseData(new TestParameters
            {
                Size = 11,
                PairsToMerge = new[]
                { 
                    new IndexPair(10, 0),
                    new IndexPair(1, 9),
                    new IndexPair(2, 8),
                    new IndexPair(3, 7),
                    new IndexPair(4, 6),

                    new IndexPair(5, 10),
                    new IndexPair(5, 1),
                    new IndexPair(8, 5),
                    new IndexPair(3, 4)
                },
                TestPairs = new[]
                {
                    new TestPair(0, 7),
                    new TestPair(1, 7),
                    new TestPair(2, 7),
                    new TestPair(3, 4),
                    new TestPair(4, 4),
                    new TestPair(5, 7),
                    new TestPair(6, 4),
                    new TestPair(7, 4),
                    new TestPair(8, 7),
                    new TestPair(9, 7),
                    new TestPair(10, 7)
                }
            }).SetName("Get the component size of elements that are in different components");
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