namespace DataStructures
{
    public interface IUnionFind<T>
    {
        int ComponentCount { get; }
        int ElementCount { get; }

        bool Connected(T firstElement, T secondElement);
        T Find(T element);
        int GetComponentSize(T element);
        void Unify(T firstElement, T secondElement);
    }
}