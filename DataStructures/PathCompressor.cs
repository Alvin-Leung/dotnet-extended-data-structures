namespace DataStructures
{
    public class PathCompressor
    {
        private readonly int[] elements;

        public PathCompressor(int[] elements)
        {
            this.elements = elements;
        }

        public void Compress(int startIndex, int rootIndex)
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
