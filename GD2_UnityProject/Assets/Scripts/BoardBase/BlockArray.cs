using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoardBase
{
    public class BlockArray
    {
        private Dictionary<BlockPosition, Block> _blocks = new Dictionary<BlockPosition, Block>();

        [SerializeField]
        private int _rows = 8;

        [SerializeField]
        private int _columns = 8;

        public int Rows => _rows;
        public int Columns => _columns;
        public List<Block> Blocks => _blocks.Values.ToList();

        public BlockArray(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;

            AddBlock();
        }

        private void AddBlock()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    _blocks.Add(new BlockPosition { X = x, Y = y }, new Block(x, y));
                }
            }
        }

        public Block BlockAt(BlockPosition blockPosition)
        {
            if (_blocks.TryGetValue(blockPosition, out var foundValue))
                return foundValue;

            return null;
        }
    }
}
