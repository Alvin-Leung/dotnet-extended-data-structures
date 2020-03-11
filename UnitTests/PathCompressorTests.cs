using DataStructures;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    class PathCompressorTests
    {
        [TestCaseSource(nameof(PathCompressorTestCaseSource))]
        public void TestPathCompressor(CompressParameters parameters)
        {
            var elements = parameters.InitialElements;
            var compressor = new PathCompressor(elements);

            compressor.Compress(parameters.StartIndex, parameters.RootIndex);
            Assert.That(elements, Is.EqualTo(parameters.ExpectedElements));
        }

        public static IEnumerable<TestCaseData> PathCompressorTestCaseSource()
        {
            yield return new TestCaseData(new CompressParameters
            {
                InitialElements = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10 },
                StartIndex = 0,
                RootIndex = 10,
                ExpectedElements = Enumerable.Repeat(10, 11).ToArray()
            }).SetName("Compress elements in series");

            yield return new TestCaseData(new CompressParameters
            {
                InitialElements = new[] { 1, 10, 3, 6, 5, 2, 8, 9, 7, 9, 4 },
                StartIndex = 0,
                RootIndex = 9,
                ExpectedElements = Enumerable.Repeat(9, 11).ToArray()
            }).SetName("Compress elements in mixed array order");
        }

        public class CompressParameters
        {
            public int[] InitialElements { get; set; }

            public int StartIndex { get; set; }

            public int RootIndex { get; set; }

            public int[] ExpectedElements { get; set; }
        }
    }
}
