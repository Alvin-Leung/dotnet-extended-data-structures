namespace DataStructures
{
    /// <summary>
    /// Outlines required methods for any <see cref="UnionFind"/> find strategy
    /// </summary>
    public interface IFindStrategy
    {
        /// <summary>
        /// Finds the index of the root parent of an element
        /// </summary>
        /// <param name="index">The index of the element to find the root parent index of</param>
        /// <param name="elements">The elements from which to find a root parent index</param>
        int Find(int index, int[] elements);
    }
}