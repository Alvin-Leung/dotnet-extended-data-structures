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
        /// The strategy to use during <see cref="Find(int)"/>. Default strategy is <see cref="FindWithPathCompression"/>.
        /// </summary>
        public IFindStrategy FindStrategy { get; set; }

        /// <summary>
        /// The number of components that elements have been grouped into
        /// </summary>
        public int ComponentCount { get; private set; }

        /// <summary>
        /// The number of elements in this <see cref="UnionFind"/>
        /// </summary>
        public int ElementCount => this.elements.Length;

        /// <summary>
        /// Creates an instance of an integer based <see cref="UnionFind"/> data structure
        /// </summary>
        /// <param name="size">The number of elements to initialize the <see cref="UnionFind"/> instance with</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="size"/> is less than or equal to 0</exception>
        public UnionFind(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), Resource.SizeMustBeGreaterThanZero);
            }

            this.elements = new int[size];
            this.componentSizes = new int[size];

            for (var i = 0; i < size; i++)
            {
                this.elements[i] = i;
                this.componentSizes[i] = 1;
            }

            this.FindStrategy = new FindWithPathCompression(this.elements);
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
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="index"/> is less than 0, or larger than or equal to the size of the <see cref="UnionFind"/></exception>
        public int Find(int index)
        {
            if (index < 0 || index >= this.ElementCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Resource.IndexMustBeWithinValidRange);
            }

            return new FindWithPathCompression(this.elements).Find(index);
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
        /// Thrown if either the inputted <paramref name="firstIndex"/> or <paramref name="secondIndex"/> is less than 0 or larger than the size of the <see cref="UnionFind"/>
        /// </exception>
        public void Unify(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex >= this.ElementCount)
            {
                throw new ArgumentOutOfRangeException(nameof(firstIndex), Resource.IndexMustBeWithinValidRange);
            }

            if (secondIndex < 0 || secondIndex >= this.ElementCount)
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
    }
}
