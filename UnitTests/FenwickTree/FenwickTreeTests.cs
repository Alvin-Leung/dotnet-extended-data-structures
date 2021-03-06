﻿using DataStructures;
using NUnit.Framework;

namespace UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FenwickTree"/> class
    /// </summary>
    internal class FenwickTreeTests
    {
        /// <summary>
        /// Checks that <see cref="FenwickTree.GetSum(int, int)"/> behaves as expected
        /// </summary>
        /// <param name="from">The index to start the range sum</param>
        /// <param name="to">The index to do the range sum up to</param>
        /// <param name="expectedSum">The expected range sum</param>
        [TestCase(3, 7, 19)]
        [TestCase(1, 5, 24)]
        [TestCase(0, 7, 24)]
        [TestCase(0, 0, 3)]
        [TestCase(7, 7, -8)]
        [Test]
        public void TestGetSum(int from, int to, int expectedSum)
        {
            var fenwickTree = new FenwickTree(new[] { 3, 4, -2, 7, 4, 11, 5, -8 });
            Assert.That(fenwickTree.GetSum(from, to), Is.EqualTo(expectedSum));
        }

        /// <summary>
        /// Checks that <see cref="FenwickTree.GetValue(int)"/> behaves as expected
        /// </summary>
        /// <param name="index">The index to get a value from</param>
        /// <param name="expectedValue">The expected value at <paramref name="index"/></param>
        [TestCase(0, 4)]
        [TestCase(29, -36)]
        [TestCase(10, 49)]
        public void TestGetValue(int index, int expectedValue)
        {
            var fenwickTree = new FenwickTree(new[] { 4, -47, 42, -39, -12, -4, 37, -34, 30, -5, 49, 0, 25, 13, 19, -40, 21, -25, -22, -29, -35, 23, 10, 20, 11, 7, 45, -7, 38, -36 });
            Assert.That(fenwickTree.GetValue(index), Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Checks that <see cref="FenwickTree.SetValue(int, int)"/> behaves as expected
        /// </summary>
        /// <param name="index">The index to set a value at</param>
        /// <param name="valueToSet">The value to set at <paramref name="index"/></param>
        /// <param name="from">The index to start the range sum</param>
        /// <param name="to">The index to do the range sum up to</param>
        /// <param name="expectedSum">The expected range sum</param>
        [TestCase(0, 10, 0, 4, 23)]
        [TestCase(7, 8, 5, 7, 24)]
        [TestCase(4, 12, 3, 5, 30)]
        public void TestSetValue(int index, int valueToSet, int from, int to, int expectedSum)
        {
            var fenwickTree = new FenwickTree(new[] { 3, 4, -2, 7, 4, 11, 5, -8 });
            fenwickTree.SetValue(index, valueToSet);
            Assert.That(fenwickTree.GetSum(from, to), Is.EqualTo(expectedSum));
        }
    }
}
