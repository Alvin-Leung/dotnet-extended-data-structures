namespace DataStructures
{
    /// <summary>
    /// Encapsulates path compression logic for the <see cref="UnionFind"/> data structure. 
    /// </summary>
    internal class PathCompressor
    {
        private readonly int[] elements;

        /// <summary>
        /// Creates an instance of a <see cref="PathCompressor"/>
        /// </summary>
        /// <param name="elements">An array of all elements that could undergo path compression</param>
        public PathCompressor(int[] elements)
        {
            this.elements = elements;
        }

        /// <summary>
        /// Compresses the path from a child element to its root parent element.
        /// </summary>
        /// <param name="fromIndex">The index of the child element from which path compression will start. Path compression includes the child element.</param>
        /// <param name="toIndex">The index of the root parent element where path compression will end.</param>
        public void Compress(int fromIndex, int toIndex)
        {
            var next = fromIndex;

            while (this.elements[next] != toIndex)
            {
                var temp = this.elements[next];
                this.elements[next] = toIndex;
                next = temp;
            }
        }
    }
}
