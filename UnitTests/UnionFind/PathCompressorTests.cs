using DataStructures.UnionFind;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.UnionFind
{
    /// <summary>
    /// Unit tests for the <see cref="PathCompressor"/> class
    /// </summary>
    internal class PathCompressorTests
    {
        /// <summary>
        /// Tests that the <see cref="PathCompressor.Compress(int, int)"/> method behaves as expected
        /// </summary>
        /// <param name="parameters">An instance encapsulating the inputs and expected outputs from this test</param>
        [TestCaseSource(nameof(PathCompressorTestCaseSource))]
        public void TestPathCompressor(CompressParameters parameters)
        {
            var elements = parameters.InitialIndices;
            var compressor = new PathCompressor(elements);

            compressor.Compress(parameters.StartIndex, parameters.RootIndex);
            Assert.That(elements, Is.EqualTo(parameters.ExpectedIndices));
        }

        private static IEnumerable<TestCaseData> PathCompressorTestCaseSource()
        {
            yield return new TestCaseData(new CompressParameters
            {
                InitialIndices = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10 },
                StartIndex = 0,
                RootIndex = 10,
                ExpectedIndices = Enumerable.Repeat(10, 11).ToArray()
            }).SetName("Compress elements in series");

            yield return new TestCaseData(new CompressParameters
            {
                InitialIndices = new[] { 1, 10, 3, 6, 5, 2, 8, 9, 7, 9, 4 },
                StartIndex = 0,
                RootIndex = 9,
                ExpectedIndices = Enumerable.Repeat(9, 11).ToArray()
            }).SetName("Compress elements in mixed array order");
        }

        /// <summary>
        /// Parameters for testing the <see cref="PathCompressor.Compress(int, int)"/> method
        /// </summary>
        public class CompressParameters
        {
            /// <summary>
            /// An array of element indices to initialize the <see cref="PathCompressor"/> instance under test with
            /// </summary>
            public int[] InitialIndices { get; set; }

            /// <summary>
            /// The index of the child element where path compression will begin
            /// </summary>
            public int StartIndex { get; set; }

            /// <summary>
            /// The index of the root parent element where path compression will end
            /// </summary>
            public int RootIndex { get; set; }

            /// <summary>
            /// The expected element indices after path compression
            /// </summary>
            public int[] ExpectedIndices { get; set; }
        }
    }
}
