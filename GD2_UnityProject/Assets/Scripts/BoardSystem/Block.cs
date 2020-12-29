namespace BoardSystem
{
    public class Block
    {
        public BlockPosition Position { get; }

        public Block(int x, int y)
        {
            Position = new BlockPosition { X = x, Y = y };
        }
    }
}