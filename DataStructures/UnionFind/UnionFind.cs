using System;
using System.Collections.Generic;

namespace DataStructures.UnionFind
{
    /// <summary>
    /// A generic implementation of the union-find data structure
    /// </summary>
    /// <typeparam name="T">Type of element to process with the <see cref="UnionFind{T}"/></typeparam>
    public class UnionFind<T> : IUnionFind<T>
    {
        private UnionFind unionFind;
        private Dictionary<int, T> indexToElementLookup;
        private Dictionary<T, int> elementToIndexLookup;

        /// <inheritdoc />
        public int ComponentCount => this.unionFind.ComponentCount;

        /// <inheritdoc />
        public int ElementCount => this.unionFind.ElementCount;

        /// <summary>
        /// Creates an instance of the generic <see cref="UnionFind{T}"/> with default <see cref="FindWithPathCompression"/> strategy
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the inputted <paramref name="elements"/> array is null</exception>"
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="elements"/> array is empty</exception>
        public UnionFind(T[] elements) : this(elements, new FindWithPathCompression())
        {
        }

        /// <summary>
        /// Creates an instance of the generic <see cref="UnionFind{T}"/> with any <see cref="IFindStrategy"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the inputted <paramref name="elements"/> array is null</exception>"
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="elements"/> array is empty</exception>
        public UnionFind(T[] elements, IFindStrategy findStrategy)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (elements.Length == 0)
            {
                throw new ArgumentException("Argument cannot be an empty array", nameof(elements));
            }

            this.unionFind = new UnionFind(elements.Length, findStrategy);
            this.indexToElementLookup = new Dictionary<int, T>();
            this.elementToIndexLookup = new Dictionary<T, int>();

            for (var i = 0; i < elements.Length; i++)
            {
                this.indexToElementLookup.Add(i, elements[i]);
                this.elementToIndexLookup.Add(elements[i], i);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown if either the <paramref name="firstElement"/> or <paramref name="secondElement"/> do not exist in the <see cref="UnionFind{T}"/></exception>
        public bool Connected(T firstElement, T secondElement)
        {
            if (!this.elementToIndexLookup.ContainsKey(firstElement) || !this.elementToIndexLookup.ContainsKey(secondElement))
            {
                throw new ArgumentException("One or more input elements do not exist in the Union Find");
            }

            var firstIndex = this.elementToIndexLookup[firstElement];
            var secondIndex = this.elementToIndexLookup[secondElement];
            return this.unionFind.Connected(firstIndex, secondIndex);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="element"/> does not exist in the <see cref="UnionFind{T}"/></exception>
        public T Find(T element)
        {
            if (!this.elementToIndexLookup.ContainsKey(element))
            {
                throw new ArgumentException("Inputted element does not exist in the Union Find", nameof(element));
            }

            var index = this.elementToIndexLookup[element];
            var rootIndex = this.unionFind.Find(index);
            return this.indexToElementLookup[rootIndex];
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="element"/> does not exist in the <see cref="UnionFind{T}"/></exception>
        public int GetComponentSize(T element)
        {
            if (!this.elementToIndexLookup.ContainsKey(element))
            {
                throw new ArgumentException("Inputted element does not exist in the Union Find", nameof(element));
            }

            var index = this.elementToIndexLookup[element];
            return this.unionFind.GetComponentSize(index);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown if either the <paramref name="firstElement"/> or <paramref name="secondElement"/> do not exist in the <see cref="UnionFind{T}"/></exception>
        public void Unify(T firstElement, T secondElement)
        {
            if (!this.elementToIndexLookup.ContainsKey(firstElement) || !this.elementToIndexLookup.ContainsKey(secondElement))
            {
                throw new ArgumentException("One or more input elements do not exist in the Union Find");
            }

            var firstIndex = this.elementToIndexLookup[firstElement];
            var secondIndex = this.elementToIndexLookup[secondElement];
            this.unionFind.Unify(firstIndex, secondIndex);
        }
    }

    /// <summary>
    /// An integer based implementation of the union find data structure
    /// </summary>
    /// <remarks>
    /// It is expected that the integer indices stored internally in this <see cref="UnionFind"/> class are correlated to actual objects via a bijection of integer indices 
    /// to objects
    /// </remarks>
    public class UnionFind : IUnionFind<int>
    {
        private int[] elements;
        private int[] componentSizes;
        private IFindStrategy findStrategy;

        /// <inheritdoc />
        public int ComponentCount { get; private set; }

        /// <inheritdoc />
        public int ElementCount => this.elements.Length;

        /// <summary>
        /// Creates an integer based <see cref="UnionFind"/> data structure. Defaults to <see cref="FindWithPathCompression"/> strategy.
        /// </summary>
        /// <param name="size">The number of elements to initialize the <see cref="UnionFind"/> instance with</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="size"/> is less than or equal to 0</exception>
        public UnionFind(int size) : this(size, new FindWithPathCompression())
        {
        }

        /// <summary>
        /// Creates an instance of an integer based <see cref="UnionFind"/> data structure
        /// </summary>
        /// <param name="size">The number of elements to initialize the <see cref="UnionFind"/> instance with</param>
        /// <param name="findStrategy">The find strategy to use when <see cref="Find(int)"/> is called</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="size"/> is less than or equal to 0</exception>
        public UnionFind(int size, IFindStrategy findStrategy)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), Resource.SizeMustBeGreaterThanZero);
            }

            this.elements = new int[size];
            this.componentSizes = new int[size];
            this.findStrategy = findStrategy;

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
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the inputted <paramref name="index"/> is less than 0, or larger than or equal to the size of the <see cref="UnionFind"/></exception>
        public int Find(int index)
        {
            if (index < 0 || index >= this.ElementCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Resource.IndexMustBeWithinValidRange);
            }

            return this.findStrategy.Find(index, this.elements);
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
