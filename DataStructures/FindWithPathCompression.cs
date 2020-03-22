namespace DataStructures
{
    /// <summary>
    /// A strategy class that implements path compression during the find operation
    /// </summary>
    public class FindWithPathCompression : IFindStrategy
    {
        private readonly int[] elements;

        /// <summary>
        /// Creates an instance of <see cref="FindWithPathCompression"/>, which performs find operations
        /// </summary>
        /// <param name="elements">The elements which to perform <see cref="Find(int)"/> on</param>
        public FindWithPathCompression(int[] elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// Finds the index of the root parent to an element, with path compression
        /// </summary>
        /// <param name="index">The index of the element to find the root parent index of</param>
        /// <returns>The root parent index</returns>
        /// <remarks>
        /// This method has the side-effect of compressing the path from child to root parent such that all elements between, including the child, directly
        /// point to the root parent after <see cref="Find(int)"/> has executed. As a result of path compression, <see cref="Find(int)"/> runs in amortized 
        /// constant time.
        /// </remarks>
        public int Find(int index)
        {
            int nextIndex = index;

            while (this.HasParent(nextIndex))
            {
                nextIndex = this.elements[nextIndex];
            }

            new PathCompressor(this.elements).Compress(index, nextIndex);

            return nextIndex;
        }

        private bool HasParent(int nextIndex)
        {
            return this.elements[nextIndex] != nextIndex;
        }
    }
}
