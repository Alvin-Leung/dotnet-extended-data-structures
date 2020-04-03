using System;

namespace DataStructures
{
    // <summary>
    /// Implementation of a Fenwick/Binary-Indexed Tree for performing range sums and point insertions in O(log(n)) time
    /// </summary>
    public class FenwickTree
    {
        private int[] originalElements;
        private int[] sums;

        /// <summary>
        /// Initializes a new instance of the <see cref="FenwickTree"/> class.
        /// </summary>
        /// <param name="elements">The element array which range sums will be calculated on. Expected to be zero indexed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="elements"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="elements"/> is an empty array</exception>
        public FenwickTree(int[] elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (elements.Length == 0)
            {
                throw new ArgumentException("Length of input array cannot be null", nameof(elements));
            }

            this.originalElements = new int[elements.Length];
            Array.Copy(sourceArray: elements, destinationArray: this.originalElements, elements.Length);

            this.sums = new int[elements.Length + 1];
            Array.Copy(sourceArray: elements, sourceIndex: 0, destinationArray: this.sums, destinationIndex: 1, elements.Length);

            for (var i = 1; i <= elements.Length; i++)
            {
                var currentSum = this.sums[i];
                var parentIndex = i + GetLeastSignificantBit(i);

                if (parentIndex >= this.sums.Length)
                {
                    continue;
                }

                this.sums[parentIndex] += currentSum;
            }
        }

        /// <summary>
        /// Gets the value of the element at the inputted <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index to get a value at</param>
        /// <returns>The value of the original element at <paramref name="index"/></returns>
        public int GetValue(int index)
        {
            return this.originalElements[index];
        }

        /// <summary>
        /// Get the range sum from one index to another (both inclusive)
        /// </summary>
        /// <param name="from">The index from which to do the range sum. The range sum will include the value at this index.</param>
        /// <param name="to">The index to which to do the range sum. The range sum will include the value at this index.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="to"/> is less than <paramref name="from"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when either the <paramref name="from"/> or <paramref name="to"/> are less than 0</exception>
        public int GetSum(int from, int to)
        {
            if (to < from)
            {
                throw new ArgumentException("To value must be greater than or equal to from value");
            }

            if (from < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(from));
            }

            if (to < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(to));
            }

            var shiftedFrom = from + 1;
            var shiftedTo = to + 1;

            return this.GetPrefixSum(shiftedTo) - this.GetPrefixSum(shiftedFrom - 1);
        }

        /// <summary>
        /// Sets the value of the element at the inputted <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index to set a value at</param>
        /// <param name="newValue">The new value to set at <paramref name="index"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than zero</exception>
        public void SetValue(int index, int newValue)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var delta = newValue - this.originalElements[index];
            var currentIndex = index + 1;

            while (currentIndex < this.sums.Length)
            {
                this.sums[currentIndex] += delta;
                currentIndex += this.GetLeastSignificantBit(currentIndex);
            }

            this.originalElements[index] = newValue;
        }

        private int GetPrefixSum(int index)
        {
            var currentIndex = index;
            var prefixSum = 0;

            while (currentIndex != 0)
            {
                prefixSum += this.sums[currentIndex];
                currentIndex -= this.GetLeastSignificantBit(currentIndex);
            }

            return prefixSum;
        }

        private int GetLeastSignificantBit(int index)
        {
            return index & -index;
        }
    }
}
