using GameSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardSystem
{
    public class BlockField
    {
        private Dictionary<BlockPosition, Block> _blocks = new Dictionary<BlockPosition, Block>();

        public readonly int Rows, Columns;
        private List<BlockPosition> BlockPositions => _blocks.Keys.ToList();
        private List<Block> Blocks => _blocks.Values.ToList();
        private PositionConverter _positionConverter;
        public PositionConverter PositionConverter => _positionConverter;

        public BlockField(int rows, int columns, PositionConverter positionConverter)
        {
            Rows = rows;
            Columns = columns;
            _positionConverter = positionConverter;
            CreateDictionary();
        }

        private void CreateDictionary()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    _blocks.Add(new BlockPosition { X = x, Y = y }, null);
                }
            }
        }

        public Block BlockAt(BlockPosition blockPosition)
        {
            _blocks.TryGetValue(blockPosition, out Block foundBlock);
            return foundBlock;
        }

        public BlockPosition PositionOf(Block block)
        {
            int idx = Blocks.IndexOf(block);
            if (idx == -1)
            {
                return default;
            }
            return BlockPositions[idx];
        }

        public void Move(BlockPosition fromPosition, BlockPosition toPosition)
        {
            Block toBlock = BlockAt(toPosition);
            if (toBlock != null)
            {
                return;
            }
            toBlock = new Block(toPosition.X, toPosition.Y);
            AddToDictionary(toBlock);

            Block fromBlock = BlockAt(fromPosition);
            if (fromBlock == null)
            {
                return;
            }
            RemoveFromDictionary(fromBlock);
        }

        public void AddToDictionary(Block block)
        {
            _blocks.TryGetValue(block.Position, out Block foundBlock);
            if (foundBlock == null)
            {
                _blocks[block.Position] = block;
            }
        }

        public void RemoveFromDictionary(Block block)
        {
            _blocks.TryGetValue(block.Position, out Block foundBlock);
            if (foundBlock != null)
            {
                _blocks[block.Position] = null;
            }
        }

        public List<BlockPosition> GetAllFieldPositions()
        {
            var arrayPositions = _blocks.Keys.ToList();
            return arrayPositions;
        }
    }
}