using DataStructures;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="UnionFind"/> class
    /// </summary>
    public class UnionFindTests
    {
        /// <summary>
        /// Tests that <see cref="UnionFind.Find(int)"/> finds an elements root parent as expected
        /// </summary>
        /// <param name="initialSize">The size of the <see cref="UnionFind"/> to initialize</param>
        /// <param name="indexToFind">The element index to input into the <see cref="UnionFind.Find(int)"/> method</param>
        /// <param name="expectedRootIndex">The expected output index from <see cref="UnionFind.Find(int)"/></param>
        [TestCase(1, 0, 0)]
        [TestCase(2, 1, 1)]
        [TestCase(4, 3, 3)]
        [TestCase(10, 9, 9)]
        public void TestFind(int initialSize, int indexToFind, int expectedRootIndex)
        {
            var unionFind = new UnionFind(initialSize);
            Assert.That(unionFind.Find(indexToFind), Is.EqualTo(expectedRootIndex));
        }

        /// <summary>
        /// Tests that <see cref="UnionFind.Unify(int, int)"/> merges elements/groups as expected
        /// </summary>
        /// <param name="parameters">An instance encapsulating the inputs and expected outputs from this test</param>
        [TestCaseSource(nameof(UnionFindTests.UnifyTestCaseSource))]
        public void TestUnify(Parameters parameters)
        {
            var unionFind = new UnionFind(parameters.InitialSize);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var pair in parameters.InputOutput)
            {
                Assert.That(unionFind.Find(pair.InputIndex), Is.EqualTo(pair.ExpectedOutputIndex));
            }
        }

        /// <summary>
        /// Tests that the <see cref="UnionFind.GetComponentSize(int)"/> returns the queried component size as expected
        /// </summary>
        /// <param name="parameters"></param>
        [TestCaseSource(nameof(UnionFindTests.GetComponentSizeTestCaseSource))]
        public void TestGetComponentSize(Parameters parameters)
        {
            var unionFind = new UnionFind(parameters.InitialSize);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var pair in parameters.InputOutput)
            {
                Assert.That(unionFind.GetComponentSize(pair.InputIndex), Is.EqualTo(pair.ExpectedOutputIndex));
            }
        }

        private static IEnumerable<TestCaseData> UnifyTestCaseSource()
        {
            yield return new TestCaseData(new Parameters
            {
                InitialSize = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                InputOutput = new[] { new InputOutput(1, 1), new InputOutput(2, 2), new InputOutput(3, 3) }
            }).SetName("Unify each element to itself, then ensure each element points to itself");

            yield return new TestCaseData(new Parameters
            {
                InitialSize = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                InputOutput = new[] { new InputOutput(1, 1), new InputOutput(2, 1), new InputOutput(3, 1), new InputOutput(4, 1), new InputOutput(5, 1) }
            }).SetName("Unify all elements into the same component, then ensure all elements have the same parent");

            yield return new TestCaseData(new Parameters
            {
                InitialSize = 101,
                PairsToMerge = new[] { new IndexPair(5, 100), new IndexPair(3, 50), new IndexPair(2, 50), new IndexPair(50, 6), new IndexPair(100, 6) },
                InputOutput = new[] { new InputOutput(5, 3), new InputOutput(100, 3), new InputOutput(3, 3), new InputOutput(50, 3), new InputOutput(2, 3), new InputOutput(6, 3) }
            }).SetName("Unify two components together, then ensure all elements have the same parent");

            yield return new TestCaseData(new Parameters
            {
                InitialSize = 501,
                PairsToMerge = new[] { new IndexPair(500, 400), new IndexPair(20, 60), new IndexPair(33, 67), new IndexPair(33, 6), new IndexPair(500, 20), new IndexPair(500, 6) },
                InputOutput = new[] 
                { 
                    new InputOutput(500, 500),
                    new InputOutput(400, 500),
                    new InputOutput(20, 500),
                    new InputOutput(60, 500),
                    new InputOutput(33, 500),
                    new InputOutput(6, 500) 
                }
            }).SetName("Unify three components together, then ensure all elements have the same parent");

            yield return new TestCaseData(new Parameters
            {
                InitialSize = 11,
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
                InputOutput = new[]
                {
                    new InputOutput(0, 10),
                    new InputOutput(1, 10),
                    new InputOutput(2, 10),
                    new InputOutput(3, 3),
                    new InputOutput(4, 3),
                    new InputOutput(5, 10),
                    new InputOutput(6, 3),
                    new InputOutput(7, 3),
                    new InputOutput(8, 10),
                    new InputOutput(9, 10),
                    new InputOutput(10, 10)
                }
            }).SetName("Unify elements into two different compoents, then ensure that they each have the correct parent");
        }

        private static IEnumerable<TestCaseData> GetComponentSizeTestCaseSource()
        {
            yield return new TestCaseData(new Parameters
            {
                InitialSize = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                InputOutput = new[] { new InputOutput(1, 1), new InputOutput(2, 1), new InputOutput(3, 1) }
            }).SetName("Get the component size of elements that have not yet been grouped");

            yield return new TestCaseData(new Parameters
            {
                InitialSize = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                InputOutput = new[] { new InputOutput(1, 5), new InputOutput(2, 5), new InputOutput(3, 5), new InputOutput(4, 5), new InputOutput(5, 5) }
            }).SetName("Get the component size of elements that are all in the same component");

            yield return new TestCaseData(new Parameters
            {
                InitialSize = 11,
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
                InputOutput = new[]
                {
                    new InputOutput(0, 7),
                    new InputOutput(1, 7),
                    new InputOutput(2, 7),
                    new InputOutput(3, 4),
                    new InputOutput(4, 4),
                    new InputOutput(5, 7),
                    new InputOutput(6, 4),
                    new InputOutput(7, 4),
                    new InputOutput(8, 7),
                    new InputOutput(9, 7),
                    new InputOutput(10, 7)
                }
            }).SetName("Get the component size of elements that are in different components");
        }

        /// <summary>
        /// Parameters for testing the <see cref="UnionFind"/> data structure
        /// </summary>
        public class Parameters
        {
            /// <summary>
            /// The initial size of the <see cref="UnionFind"/> instance under test
            /// </summary>
            public int InitialSize { get; set; }

            /// <summary>
            /// The index pairs to merge before the target method is tested
            /// </summary>
            public IndexPair[] PairsToMerge { get; set; }

            public InputOutput[] InputOutput { get; set; }
        }

        /// <summary>
        /// A pair of indices that should be processed together, such as for merging
        /// </summary>
        public class IndexPair
        {
            /// <summary>
            /// The index of the first element to process
            /// </summary>
            public int FirstIndex { get; }

            /// <summary>
            /// The index of the second element to process
            /// </summary>
            public int SecondIndex { get; }

            /// <summary>
            /// Creates an instance of an <see cref="IndexPair"/> for grouping pairs of indices together
            /// </summary>
            /// <param name="firstIndex">The index of the first element to process</param>
            /// <param name="secondIndex">The index of the second element to process</param>
            public IndexPair(int firstIndex, int secondIndex)
            {
                this.FirstIndex = firstIndex;
                this.SecondIndex = secondIndex;
            }
        }

        /// <summary>
        /// The input into a method under test and expected output of that method
        /// </summary>
        public class InputOutput
        {
            /// <summary>
            /// The index to input into the method under test
            /// </summary>
            public int InputIndex { get; }

            /// <summary>
            /// The expected output index of the method under test
            /// </summary>
            public int ExpectedOutputIndex { get; }

            /// <summary>
            /// Creates an instance for encapsulating input into a method under test, and the expected output
            /// </summary>
            /// <param name="inputIndex">The index that will be inputted into the method under test</param>
            /// <param name="expectedOutputIndex">The expected output from the method under test</param>
            public InputOutput(int inputIndex, int expectedOutputIndex)
            {
                this.InputIndex = inputIndex;
                this.ExpectedOutputIndex = expectedOutputIndex;
            }
        }
    }
}