using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Views;

namespace BoardBase
{
    public class BlockArray
    {
        private Dictionary<BlockPosition, BlockView> _blocks = new Dictionary<BlockPosition, BlockView>();

        [SerializeField]
        private int _rows = 8;

        [SerializeField]
        private int _columns = 8;

        public int Rows => _rows;
        public int Columns => _columns;
        //public List<BlockView> Blocks => _blocks.Values.ToList();

        public BlockArray(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;

            CreateDictionary();
        }

        /// <summary>
        /// <br>Creates the dictionary.</br>
        /// <br>Creates all potential blockpositions and connects them to an empty blockview.</br>
        /// </summary>
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

        /// <summary>
        /// Connect blockviews to their blockpositions.
        /// </summary>
        /// <param name="combinedBlockView"></param>
        internal void AddToDictionary(CombinedBlockView combinedBlockView)
        {
            foreach(var blockView in combinedBlockView.BlockViews)
            {
                _blocks[blockView.BottomLeftBlockPosition] = blockView;
            }
        }

        /// <summary>
        /// Returns either the blockview at a specified blockposition or null when there is none.
        /// </summary>
        /// <param name="blockPosition"></param>
        /// <returns></returns>
        public BlockView BlockAt(BlockPosition blockPosition)
        {
            if (_blocks.TryGetValue(blockPosition, out var foundValue))
                return foundValue;

            return null;
        }
    }
}
