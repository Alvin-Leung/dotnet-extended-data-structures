using System;
using System.Linq;

namespace DataStructures
{
    public class UnionFind
    {
        private int[] elements;
        private int numComponents;

        public int Count => this.numComponents;

        public UnionFind(int size)
        {
            this.elements = Enumerable.Range(0, size).ToArray();
            this.numComponents = size;
        }

        public bool Connected(int firstIndex, int secondIndex)
        {
            return this.Find(firstIndex) == this.Find(secondIndex);
        }

        public int Find(int index)
        {
            if (index < 0 || index > this.Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Resource.IndexMustBeWithinValidRange);
            }

            int nextIndex = index;

            while (this.elements[nextIndex] != nextIndex)
            {
                nextIndex = this.elements[index];
            }

            return nextIndex;
        }

        public void Union(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex > this.Count - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(firstIndex), Resource.IndexMustBeWithinValidRange);
            }

            if (secondIndex < 0 || secondIndex > this.Count - 1)
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

            this.elements[firstParentIndex] = secondParentIndex;
            this.numComponents--;
        }
    }
}
