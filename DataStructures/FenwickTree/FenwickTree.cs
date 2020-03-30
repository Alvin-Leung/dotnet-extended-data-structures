using System;

namespace DataStructures
{
    public class FenwickTree
    {
		private int[] sums;

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

		public int GetValue(int index)
		{
			var shiftedIndex = index + 1;
			return this.GetPrefixSum(shiftedIndex) - this.GetPrefixSum(shiftedIndex - 1);
		}

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
