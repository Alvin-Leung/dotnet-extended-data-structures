using System;

namespace DataStructures
{
    /// <summary>
    /// A strategy class that implements path compression during the find operation
    /// </summary>
    public class FindWithPathCompression : IFindStrategy
    {
        /// <summary>
        /// Finds the index of the root parent to an element, with path compression
        /// </summary>
        /// <param name="index">The index of the element to find the root of</param>
        /// <param name="elements">All <see cref="UnionFind"/> elements</param>
        /// <returns>The root parent index</returns>
        /// <remarks>
        /// This method has the side-effect of compressing the path from child to root parent such that all elements between, including the child, directly
        /// point to the root parent after <see cref="Find(int)"/> has executed. As a result of path compression, <see cref="Find(int)"/> runs in amortized 
        /// constant time.
        /// </remarks>
        public int Find(int index, int[] elements)
        {
            if (index >= elements.Length)
            {
                throw new ArgumentOutOfRangeException("Index out of range of element array", nameof(index));
            }

            if (elements == null || elements.Length == 0)
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(elements));
            }

            int nextIndex = index;

            while (this.HasParent(nextIndex, elements))
            {
                nextIndex = elements[nextIndex];
            }

            new PathCompressor(elements).Compress(index, nextIndex);

            return nextIndex;
        }

        private bool HasParent(int nextIndex, int[] elements)
        {
            return elements[nextIndex] != nextIndex;
        }
    }
}
