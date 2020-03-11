using System;

namespace DataStructures
{
    public class UnionFind
    {
        private int[] elements;
        private int[] componentSizes;

        public int ComponentCount { get; private set; }

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

        public bool Connected(int firstIndex, int secondIndex)
        {
            return this.Find(firstIndex) == this.Find(secondIndex);
        }

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

            this.CompressPath(index, nextIndex);

            return nextIndex;
        }

        public int GetComponentSize(int index)
        {
            return this.componentSizes[this.Find(index)];
        }

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

        private void CompressPath(int startIndex, int rootIndex)
        {
            var next = startIndex;

            while (this.elements[next] != rootIndex)
            {
                var temp = this.elements[next];
                this.elements[next] = rootIndex;
                next = temp;
            }
        }
    }
}
