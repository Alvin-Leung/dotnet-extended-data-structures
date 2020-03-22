namespace DataStructures
{
    /// <summary>
    /// Outlines required methods for any <see cref="UnionFind"/> find strategy
    /// </summary>
    public interface IFindStrategy
    {
        /// <summary>
        /// Finds the index of the root parent to an element
        /// </summary>
        int Find(int index);
    }
}