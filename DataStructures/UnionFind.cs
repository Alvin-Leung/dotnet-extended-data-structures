using System;

namespace DataStructures
{
    /// <summary>
    /// An integer based implementation of the union find data structure
    /// </summary>
    /// <remarks>
    /// It is expected that the integer indices stored internally in this <see cref="UnionFind"/> class are correlated to actual objects via a bijection of integer indices 
    /// to objects
    /// </remarks>
    public class UnionFind
    {
        private int[] elements;
        private int[] componentSizes;

        /// <summary>
        /// The number of components that elements have been grouped into
        /// </summary>
        public int ComponentCount { get; private set; }

        /// <summary>
        /// Creates an instance of an integer based <see cref="UnionFind"/> data structure
        /// </summary>
        /// <param name="size">The number of elements to initialize the <see cref="UnionFind"/> instance with</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="size"/> is less than 0 or above <see cref="int.MaxValue"/></exception>
        public UnionFind(int size)
        {
            if (size < 0 || size > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(size), Resource.SizeMustBeWithinValidRange);
            }

            this.elements = new int[size];
            this.componentSizes = new int[size];

            for (var i = 0; i < size; i++)
            {
                this.elements[i] = i;
                this.componentSizes[i] = 1;
            }

            this.ComponentCount = size;
        }

        /// <summary>
        /// Checks by index whether two elements are in the same group
        /// </summary>
        /// <param name="firstIndex">The index of the first element to check</param>
        /// <param name="secondIndex">The index of the second element to check</param>
        /// <returns>True if the elements are in the same group, otherwise false</returns>
        public bool Connected(int firstIndex, int secondIndex)
        {
            return this.Find(firstIndex) == this.Find(secondIndex);
        }

        /// <summary>
        /// Finds the index of the root parent to an element
        /// </summary>
        /// <param name="index">The index of the child element</param>
        /// <returns>The index of the root parent element, or the inputted index if the element has no parents</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="index"/> is less than 0 or above <see cref="int.MaxValue"/></exception>
        /// <remarks>
        /// This method has the side-effect of compressing the path from child to root parent such that all elements between, including the child, directly
        /// point to the root parent after <see cref="Find(int)"/> has executed. As a result of path compression, <see cref="Find(int)"/> runs in amortized 
        /// constant time.
        /// </remarks>
        public int Find(int index)
        {
            if (index < 0 || index > this.elements.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Resource.IndexMustBeWithinValidRange);
            }

            int nextIndex = index;

            while (this.HasParent(nextIndex))
            {
                nextIndex = this.elements[nextIndex];
            }

            new PathCompressor(this.elements).Compress(index, nextIndex);

            return nextIndex;
        }

        /// <summary>
        /// Gets the size of the component an element belongs to
        /// </summary>
        /// <param name="index">The index of the element</param>
        /// <returns>The size of the component the element belongs to</returns>
        public int GetComponentSize(int index)
        {
            return this.componentSizes[this.Find(index)];
        }

        /// <summary>
        /// Merges two elements' groups together
        /// </summary>
        /// <param name="firstIndex">The index of the first element to merge</param>
        /// <param name="secondIndex">The index of the second element to merge</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if either the inputted <paramref name="firstIndex"/> or <paramref name="secondIndex"/> is less than 0 or above <see cref="int.MaxValue"/>
        /// </exception>
        public void Unify(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex > this.elements.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(firstIndex), Resource.IndexMustBeWithinValidRange);
            }

            if (secondIndex < 0 || secondIndex > this.elements.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(secondIndex), Resource.IndexMustBeWithinValidRange);
            }

            if (firstIndex == secondIndex)
            {
                return;
            }

            var firstParentIndex = this.Find(firstIndex);
            var secondParentIndex = this.Find(secondIndex);

            if (firstParentIndex == secondParentIndex)
            {
                return;
            }

            this.UpdateComponentState(firstParentIndex, secondParentIndex);
        }

        private void UpdateComponentState(int firstParentIndex, int secondParentIndex)
        {
            if (this.componentSizes[firstParentIndex] >= this.componentSizes[secondParentIndex])
            {
                this.elements[secondParentIndex] = firstParentIndex;
                this.componentSizes[firstParentIndex] += this.componentSizes[secondParentIndex];
            }
            else
            {
                this.elements[firstParentIndex] = secondParentIndex;
                this.componentSizes[secondParentIndex] += this.componentSizes[firstParentIndex];
            }

            this.ComponentCount--;
        }

        private bool HasParent(int nextIndex)
        {
            return this.elements[nextIndex] != nextIndex;
        }
    }
}
