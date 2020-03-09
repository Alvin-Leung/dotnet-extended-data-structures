using DataStructures;
using NUnit.Framework;

namespace UnitTests
{
    public class UnionFindTests
    {
        [TestCase(1, 1, 1)]
        [Test]
        public void TestFind(int size, int indexToFind, int expectedParent)
        {
            var unionFind = new UnionFind(size);
            Assert.That(unionFind.Find(indexToFind), Is.EqualTo(expectedParent));
        }
    }
}