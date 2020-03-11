namespace DataStructures
{
    public class PathCompressor
    {
        private readonly int[] elements;

        public PathCompressor(int[] elements)
        {
            this.elements = elements;
        }

        public void Compress(int from, int to)
        {
            var next = from;

            while (this.elements[next] != to)
            {
                var temp = this.elements[next];
                this.elements[next] = to;
                next = temp;
            }
        }
    }
}
