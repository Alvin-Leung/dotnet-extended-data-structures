using System;

namespace DataStructures
{
    public class UnionFind
    {
        private int[] elements;
        private int[] componentSizes;

        public int Count { get; private set; }

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

            this.Count = size;
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
                nextIndex = this.elements[nextIndex];
            }

            return nextIndex;
        }

        public void Unify(int firstIndex, int secondIndex)
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

            this.Count--;
        }
    }
}
