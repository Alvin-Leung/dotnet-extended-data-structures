namespace DataStructures
{
    /// <summary>
    /// Outlines the methods any union find implementation should have
    /// </summary>
    /// <typeparam name="T">Type of element that the union find will hold</typeparam>
    public interface IUnionFind<T>
    {
        /// <summary>
        /// The number of components that elements have been grouped into
        /// </summary>
        int ComponentCount { get; }

        /// <summary>
        /// The number of elements
        /// </summary>
        int ElementCount { get; }

        /// <summary>
        /// Checks whether two elements are in the same group
        /// </summary>
        /// <param name="firstElement">The first element to check</param>
        /// <param name="secondElement">The second element to check</param>
        /// <returns>True if the elements are in the same group, otherwise false</returns>
        bool Connected(T firstElement, T secondElement);

        /// <summary>
        /// Finds the root parent to a child element
        /// </summary>
        /// <param name="element">The child element</param>
        /// <returns>The root parent element, or the inputted element if it has no parents</returns>
        T Find(T element);

        /// <summary>
        /// Gets the size of the component an element belongs to
        /// </summary>
        /// <param name="element">The element which belongs to a component</param>
        /// <returns>Size of component that the inputted <paramref name="element"/> belongs to</returns>
        int GetComponentSize(T element);

        /// <summary>
        /// Merges two elements' groups together
        /// </summary>
        /// <param name="firstElement">The index of the first element to merge</param>
        /// <param name="secondElement">The index of the second element to merge</param>
        void Unify(T firstElement, T secondElement);
    }
}