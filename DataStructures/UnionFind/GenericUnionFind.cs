using System;
using System.Collections.Generic;

namespace DataStructures.Generic
{
    /// <summary>
    /// A generic implementation of the union-find data structure
    /// </summary>
    /// <typeparam name="T">Type of element to process with the <see cref="GenericUnionFind{T}"/></typeparam>
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
        /// Creates an instance of the generic <see cref="GenericUnionFind{T}"/> with default <see cref="FindWithPathCompression"/> strategy
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the inputted <paramref name="elements"/> array is null</exception>"
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="elements"/> array is empty</exception>
        public UnionFind(T[] elements) : this(elements, new FindWithPathCompression())
        {
        }

        /// <summary>
        /// Creates an instance of the generic <see cref="GenericUnionFind{T}"/> with any <see cref="IFindStrategy"/>
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
        /// <exception cref="ArgumentException">Thrown if either the <paramref name="firstElement"/> or <paramref name="secondElement"/> do not exist in the <see cref="GenericUnionFind{T}"/></exception>
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
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="element"/> does not exist in the <see cref="GenericUnionFind{T}"/></exception>
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
        /// <exception cref="ArgumentException">Thrown if the inputted <paramref name="element"/> does not exist in the <see cref="GenericUnionFind{T}"/></exception>
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
        /// <exception cref="ArgumentException">Thrown if either the <paramref name="firstElement"/> or <paramref name="secondElement"/> do not exist in the <see cref="GenericUnionFind{T}"/></exception>
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
}
