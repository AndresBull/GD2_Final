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
        public List<BlockView> Blocks => _blocks.Values.ToList();

        public BlockArray(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;

            CreateDictionary();
        }

        //Creates the dictionary
        //Creates all potential blockpositions and connects them to an empty blockview
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

        //Connect blockviews to their blockpositions
        internal void AddToDictionary(BlockView blockView)
        {
            //Needs way to get all different parts of multi-block building blocks
            //Blockview has a list of blockpositions that need to be run over (need their positions first though)
            _blocks[blockView.BottomLeftBlockPosition] = blockView;
        }

        //Returns either the blockview at a specified blockposition or null when there is none
        public BlockView BlockAt(BlockPosition blockPosition)
        {
            if (_blocks.TryGetValue(blockPosition, out var foundValue))
                return foundValue;

            return null;
        }
    }
}
