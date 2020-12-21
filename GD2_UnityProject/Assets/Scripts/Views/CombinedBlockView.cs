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

            var startBlock = new BlockPosition(8, 8);
            floodFiller = new FloodFill(Neighbours);

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
            if (upTile == null && !floodFiller.HasBlock(upPosition) && upPosition.X <= 8 && upPosition.Y <= 8)
                neighbours.Add(upPosition);
            
            var downPosition = position;
            downPosition.Y -= 1;
            var downTile = GameLoop.Instance.Array.BlockAt(downPosition);
            if (downTile == null && !floodFiller.HasBlock(downPosition) && downPosition.X <= 8 && downPosition.Y <= 8)                      
                neighbours.Add(downPosition);
                              
            var rightPosition = position;
            rightPosition.X += 1;
            var rightTile = GameLoop.Instance.Array.BlockAt(rightPosition);
            if (rightTile == null && !floodFiller.HasBlock(rightPosition) && rightPosition.X <= 8 && rightPosition.Y <= 8)
                neighbours.Add(rightPosition);

            var leftPosition = position;
            leftPosition.X -= 1;
            var leftTile = GameLoop.Instance.Array.BlockAt(leftPosition);
            if (leftTile == null && !floodFiller.HasBlock(leftPosition) && leftPosition.X <= 8 && leftPosition.Y <= 8)
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
                            var _floodedPositions = floodFiller.Flood(new BlockPosition(8,8));
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
    }
}
