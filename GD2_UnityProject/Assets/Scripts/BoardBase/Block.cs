using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardBase
{
    public class Block
    {
        public BlockPosition BlockPosition { get; }

        public Block (int x, int y)
        {
            BlockPosition = new BlockPosition { X = x, Y = y };
        }
    }
}
