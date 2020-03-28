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
        /// Tests that <see cref="UnionFind.Connected(int, int)"/> successfully identifies elements which are in the same group
        /// </summary>
        /// <param name="parameters">An instance encapsulating the inputs and expected outputs from this test</param>
        [TestCaseSource(nameof(UnionFindTests.ConnectedTestCaseSource))]
        public void TestConnected(Parameters<IndexPair, bool> parameters)
        {
            var unionFind = new UnionFind(parameters.InitialSize);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var inputOutput in parameters.InputOutput)
            {
                Assert.That(unionFind.Connected(inputOutput.Input.FirstIndex, inputOutput.Input.SecondIndex), Is.EqualTo(inputOutput.ExpectedOutput));
            }
        }

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
        public void TestUnify(Parameters<int, int> parameters)
        {
            var unionFind = new UnionFind(parameters.InitialSize);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var inputOutput in parameters.InputOutput)
            {
                Assert.That(unionFind.Find(inputOutput.Input), Is.EqualTo(inputOutput.ExpectedOutput));
            }
        }

        /// <summary>
        /// Tests that the <see cref="UnionFind.GetComponentSize(int)"/> returns the queried component size as expected
        /// </summary>
        /// <param name="parameters"></param>
        [TestCaseSource(nameof(UnionFindTests.GetComponentSizeTestCaseSource))]
        public void TestGetComponentSize(Parameters<int, int> parameters)
        {
            var unionFind = new UnionFind(parameters.InitialSize);

            foreach (var pair in parameters.PairsToMerge)
            {
                unionFind.Unify(pair.FirstIndex, pair.SecondIndex);
            }

            foreach (var inputOutput in parameters.InputOutput)
            {
                Assert.That(unionFind.GetComponentSize(inputOutput.Input), Is.EqualTo(inputOutput.ExpectedOutput));
            }
        }

        private static IEnumerable<TestCaseData> ConnectedTestCaseSource()
        {
            yield return new TestCaseData(new Parameters<IndexPair, bool>
            {
                InitialSize = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                InputOutput = new[]
                { 
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 2), false),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 3), false),
                    new InputOutput<IndexPair, bool>(new IndexPair(2, 3), false),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 1), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(2, 2), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(3, 3), true),
                }
            }).SetName("Unify each element to itself, then ensure no elements are in the same group");

            yield return new TestCaseData(new Parameters<IndexPair, bool>
            {
                InitialSize = 11,
                PairsToMerge = new[] { new IndexPair(1, 5), new IndexPair(6, 3), new IndexPair(3, 1), new IndexPair(7, 3) },
                InputOutput = new[]
                {
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 2), false),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 3), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 4), false),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 5), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 6), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(1, 7), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(6, 7), true),
                    new InputOutput<IndexPair, bool>(new IndexPair(6, 10), false)
                }
            }).SetName("Unify some elements, then ensure only these elements are in the same group");
        }

        private static IEnumerable<TestCaseData> UnifyTestCaseSource()
        {
            yield return new TestCaseData(new Parameters<int, int>
            {
                InitialSize = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                InputOutput = new[] { new InputOutput<int, int>(1, 1), new InputOutput<int, int>(2, 2), new InputOutput<int, int>(3, 3) }
            }).SetName("Unify each element to itself, then ensure each element points to itself");

            yield return new TestCaseData(new Parameters<int, int>
            {
                InitialSize = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                InputOutput = new[] 
                { 
                    new InputOutput<int, int>(1, 1),
                    new InputOutput<int, int>(2, 1),
                    new InputOutput<int, int>(3, 1),
                    new InputOutput<int, int>(4, 1),
                    new InputOutput<int, int>(5, 1) 
                }
            }).SetName("Unify all elements into the same component, then ensure all elements have the same parent");

            yield return new TestCaseData(new Parameters<int, int>
            {
                InitialSize = 101,
                PairsToMerge = new[] { new IndexPair(5, 100), new IndexPair(3, 50), new IndexPair(2, 50), new IndexPair(50, 6), new IndexPair(100, 6) },
                InputOutput = new[] 
                { 
                    new InputOutput<int, int>(5, 3),
                    new InputOutput<int, int>(100, 3),
                    new InputOutput<int, int>(3, 3),
                    new InputOutput<int, int>(50, 3),
                    new InputOutput<int, int>(2, 3),
                    new InputOutput<int, int>(6, 3) 
                }
            }).SetName("Unify two components together, then ensure all elements have the same parent");

            yield return new TestCaseData(new Parameters<int, int>
            {
                InitialSize = 501,
                PairsToMerge = new[] { new IndexPair(500, 400), new IndexPair(20, 60), new IndexPair(33, 67), new IndexPair(33, 6), new IndexPair(500, 20), new IndexPair(500, 6) },
                InputOutput = new[] 
                { 
                    new InputOutput<int, int>(500, 500),
                    new InputOutput<int, int>(400, 500),
                    new InputOutput<int, int>(20, 500),
                    new InputOutput<int, int>(60, 500),
                    new InputOutput<int, int>(33, 500),
                    new InputOutput<int, int>(6, 500) 
                }
            }).SetName("Unify three components together, then ensure all elements have the same parent");

            yield return new TestCaseData(new Parameters<int, int>
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
                    new InputOutput<int, int>(0, 10),
                    new InputOutput<int, int>(1, 10),
                    new InputOutput<int, int>(2, 10),
                    new InputOutput<int, int>(3, 3),
                    new InputOutput<int, int>(4, 3),
                    new InputOutput<int, int>(5, 10),
                    new InputOutput<int, int>(6, 3),
                    new InputOutput<int, int>(7, 3),
                    new InputOutput<int, int>(8, 10),
                    new InputOutput<int, int>(9, 10),
                    new InputOutput<int, int>(10, 10)
                }
            }).SetName("Unify elements into two different compoents, then ensure that they each have the correct parent");
        }

        private static IEnumerable<TestCaseData> GetComponentSizeTestCaseSource()
        {
            yield return new TestCaseData(new Parameters<int, int>
            {
                InitialSize = 4,
                PairsToMerge = new[] { new IndexPair(1, 1), new IndexPair(2, 2), new IndexPair(3, 3) },
                InputOutput = new[] { new InputOutput<int, int>(1, 1), new InputOutput<int, int>(2, 1), new InputOutput<int, int>(3, 1) }
            }).SetName("Get the component size of elements that have not yet been grouped");

            yield return new TestCaseData(new Parameters<int, int>
            {
                InitialSize = 6,
                PairsToMerge = new[] { new IndexPair(1, 2), new IndexPair(2, 3), new IndexPair(3, 4), new IndexPair(4, 5) },
                InputOutput = new[] 
                { 
                    new InputOutput<int, int>(1, 5),
                    new InputOutput<int, int>(2, 5),
                    new InputOutput<int, int>(3, 5),
                    new InputOutput<int, int>(4, 5),
                    new InputOutput<int, int>(5, 5) 
                }
            }).SetName("Get the component size of elements that are all in the same component");

            yield return new TestCaseData(new Parameters<int, int>
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
                    new InputOutput<int, int>(0, 7),
                    new InputOutput<int, int>(1, 7),
                    new InputOutput<int, int>(2, 7),
                    new InputOutput<int, int>(3, 4),
                    new InputOutput<int, int>(4, 4),
                    new InputOutput<int, int>(5, 7),
                    new InputOutput<int, int>(6, 4),
                    new InputOutput<int, int>(7, 4),
                    new InputOutput<int, int>(8, 7),
                    new InputOutput<int, int>(9, 7),
                    new InputOutput<int, int>(10, 7)
                }
            }).SetName("Get the component size of elements that are in different components");
        }

        /// <summary>
        /// Parameters for testing the <see cref="UnionFind"/> data structure
        /// </summary>
        public class Parameters<T, U>
        {
            /// <summary>
            /// The initial size of the <see cref="UnionFind"/> instance under test
            /// </summary>
            public int InitialSize { get; set; }

            /// <summary>
            /// The index pairs to merge before the target method is tested
            /// </summary>
            public IndexPair[] PairsToMerge { get; set; }

            public InputOutput<T,U>[] InputOutput { get; set; }
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
        /// Encapsulated input and expected output for the method under test
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Input"/></typeparam>
        /// <typeparam name="U">The type of <see cref="ExpectedOutput"/></typeparam>
        public class InputOutput<T, U>
        {
            /// <summary>
            /// The value to input into the method under test
            /// </summary>
            public T Input { get; }

            /// <summary>
            /// The expected output value of the method under test
            /// </summary>
            public U ExpectedOutput { get; }

            /// <summary>
            /// Creates an instance for encapsulating input into a method under test, and the expected output
            /// </summary>
            /// <param name="input">The value that will be inputted into the method under test</param>
            /// <param name="expectedOutput">The expected output value from the method under test</param>
            public InputOutput(T input, U expectedOutput)
            {
                this.Input = input;
                this.ExpectedOutput = expectedOutput;
            }
        }
    }
}