using BoardSystem;
using GameSystem.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Views
{
    public class BlockFieldView
    {
        private Dictionary<BlockPosition, BlockView> _blocks = new Dictionary<BlockPosition, BlockView>();

        // TODO: use this positionconverter instead of the one in gameloop, temporarily done like this because fieldview and gameloop need to be rewritten
        //[SerializeField]
        //private PositionConverter _positionConverter = null;

        [SerializeField]
        private int _rows = 8;

        [SerializeField]
        private int _columns = 8;

        public int Rows => _rows;
        public int Columns => _columns;
        public PositionConverter PositionConverter => GameLoop.Instance.PositionConverter;

        public BlockFieldView(int rows, int columns)
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
                _blocks[blockView.Position] = blockView;
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

        /// <summary>
        /// Temp function that alligns blocks to the worldposition that their blockposition would be.
        /// </summary>
        public void AllignBlockViews(CombinedBlockView combinedBlockView)
        {
            combinedBlockView.GetAllBlockViews();

            foreach (var blockView in combinedBlockView.BlockViews)
            {
                var boardPosition = PositionConverter.ToBlockPosition(this, blockView.transform.position);
                blockView.transform.position = PositionConverter.ToWorldPosition(this, boardPosition);
                blockView.Position = boardPosition;
            }
        }

        /// <summary>
        /// <br>Needs to be called when a CombinedBlock lands.</br>
        /// <br>Runs over all BlockViews in the children.</br>
        /// <br>Adds the blocks to a dictionary where it can be called again by the "ToWorldPosition".</br>
        /// </summary>
        /// function<param name="array"></param>
        /// <param name="combinedBlockView"></param>
        public void ConnectBlockViews(CombinedBlockView combinedBlockView)
        {
            foreach (var blockView in combinedBlockView.BlockViews)
            {
                var boardPosition = PositionConverter.ToBlockPosition(this, blockView.transform.position);
                blockView.Position = boardPosition;
            }

            AddToDictionary(combinedBlockView);
        }

        public List<BlockPosition> GetAllArrayPositions()
        {
            var arrayPositions = _blocks.Keys.ToList();
            return arrayPositions;
        }
    }
}
