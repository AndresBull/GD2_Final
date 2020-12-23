using Assets.Scripts;
using BoardBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Views
{
    [SelectionBase]
    public class CombinedBlockView : MonoBehaviour
    {
        private List<BlockView> _blockViews = new List<BlockView>();
        public List<BlockView> BlockViews => _blockViews;

        public event EventHandler Landed;
        public FloodFill floodFiller;


        private GameLoop gameLoop;
        private BlockPosition _boardPosition;
        private float _dropDownDelay = 1f;

        void Awake()
        {
            gameLoop = GameLoop.Instance;
            _boardPosition = gameLoop.PositionConverter.ToBlockPosition(gameLoop.Array, transform.position);

            var startBlock = new BlockPosition(GameLoop.Instance.Array.Rows+1, GameLoop.Instance.Array.Columns);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.floodFiller = floodFiller;

            gameLoop.AllignBlockViews(this);
            StartCoroutine(Drop());
        }
        /// <summary>
        /// Gets all neighbours of the startingblock.
        /// These neighbours should get filtered in floodfill, but it might be better to filter here
        /// </summary>
        /// <param name="startBlock"></param>
        /// <returns></returns>
        private List<BlockPosition> Neighbours(BlockPosition startBlock)
        {
            var neighbours = new List<BlockPosition>();

            var position = startBlock;

            var upPosition = position;
            upPosition.Y += 1;
            var upTile = GameLoop.Instance.Array.BlockAt(upPosition);
            if (IsNeighbour(upTile, upPosition))
                neighbours.Add(upPosition);
            
            var downPosition = position;
            downPosition.Y -= 1;
            var downTile = GameLoop.Instance.Array.BlockAt(downPosition);
            if (IsNeighbour(downTile, downPosition))
                neighbours.Add(downPosition);
                              
            var rightPosition = position;
            rightPosition.X += 1;
            var rightTile = GameLoop.Instance.Array.BlockAt(rightPosition);
            if (IsNeighbour(rightTile, rightPosition))
                neighbours.Add(rightPosition);

            var leftPosition = position;
            leftPosition.X -= 1;
            var leftTile = GameLoop.Instance.Array.BlockAt(leftPosition);
            if (IsNeighbour(leftTile, leftPosition))
                neighbours.Add(leftPosition);

            return neighbours;
        }

        private IEnumerator Drop()
        {
            while (Application.isPlaying)
            {
                foreach(var blockView in BlockViews)
                {
                    if (blockView.BottomLeftBlockPosition.Y == _boardPosition.Y)
                    {
                        var nextBlockPosition = blockView.BottomLeftBlockPosition;
                        nextBlockPosition.Y -= 1;

                        if(nextBlockPosition.Y < 0 || gameLoop.Array.BlockAt(nextBlockPosition) != null)
                        {
                            //Call event landed
                            //StopCoroutine(Drop());

                            Debug.Log("Landed");

                            gameLoop.ConnectBlockViews(this);
                            floodFiller._floodedPositions = floodFiller.Flood(new BlockPosition(8,8));
                            
                            yield break;
                        }
                    }
                }

                foreach (var blockView in BlockViews)
                {
                    var nextBlockPosition = blockView.BottomLeftBlockPosition;
                    nextBlockPosition.Y -= 1;
                    blockView.BottomLeftBlockPosition = nextBlockPosition;
                }

                var nextBoardPosition = _boardPosition;
                nextBoardPosition.Y -= 1;

                transform.position = gameLoop.PositionConverter.ToWorldPosition(gameLoop.Array, nextBoardPosition);

                _boardPosition = nextBoardPosition;

                yield return new WaitForSeconds(_dropDownDelay);
            }
        }


        /// <summary>
        /// Finds all BlockView components in the children
        /// </summary>
        public void GetAllBlockViews()
        {
            _blockViews = GetComponentsInChildren<BlockView>().ToList();
        }

        protected virtual void OnLanded(EventArgs args)
        {
            var handler = Landed;
            handler?.Invoke(this, args);
        }

        protected bool IsNeighbour(BlockView block, BlockPosition blockPosition)
        {
            if (block == null && !floodFiller.HasBlock(blockPosition)
                && blockPosition.X <= GameLoop.Instance.Array.Rows && blockPosition.Y <= GameLoop.Instance.Array.Columns
                && blockPosition.X >= 0 && blockPosition.Y >= 0)
            return true;

            return false;
        }
    }
}
