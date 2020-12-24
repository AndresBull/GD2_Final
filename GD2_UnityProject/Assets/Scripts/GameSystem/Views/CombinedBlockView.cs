using BoardSystem;
using GameSystem.Characters;
using GameSystem.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Views
{
    [SelectionBase]
    public class CombinedBlockView : MonoBehaviour
    {
        public event EventHandler Landed;

        public List<BlockView> BlockViews => _blockViews;

        public FloodFill floodFiller;


        private List<BlockView> _blockViews = new List<BlockView>();
        private BlockFieldView _field;
        private BlockPosition _boardPosition;
        private float _dropDownDelay = 1f;

        void Awake()
        {
            _field = GameLoop.Instance.FieldView;
            _boardPosition = _field.PositionConverter.ToBlockPosition(_field, transform.position);

            var startBlock = new BlockPosition(_field.Rows + 1, _field.Columns);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.floodFiller = floodFiller;

            _field.AllignBlockViews(this);
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
            var upTile = _field.BlockAt(upPosition);
            if (IsNeighbour(upTile, upPosition))
                neighbours.Add(upPosition);
            
            var downPosition = position;
            downPosition.Y -= 1;
            var downTile = _field.BlockAt(downPosition);
            if (IsNeighbour(downTile, downPosition))
                neighbours.Add(downPosition);
                              
            var rightPosition = position;
            rightPosition.X += 1;
            var rightTile = _field.BlockAt(rightPosition);
            if (IsNeighbour(rightTile, rightPosition))
                neighbours.Add(rightPosition);

            var leftPosition = position;
            leftPosition.X -= 1;
            var leftTile = _field.BlockAt(leftPosition);
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
                    if (blockView.Position.Y == _boardPosition.Y)
                    {
                        var nextBlockPosition = blockView.Position;
                        nextBlockPosition.Y -= 1;

                        if(nextBlockPosition.Y < 0 || _field.BlockAt(nextBlockPosition) != null)
                        {
                            //Call event landed
                            //StopCoroutine(Drop());

                            Debug.Log("Landed");

                            _field.ConnectBlockViews(this);
                            floodFiller._floodedPositions = floodFiller.Flood(new BlockPosition(8,8));
                            foreach (var climber in PlayerConfigManager.Instance.GetAllClimbers())
                            {
                                climber.GetComponent<ClimberBehaviour>().KillPlayer();
                            }
                            
                            yield break;
                        }
                    }
                }

                foreach (var blockView in BlockViews)
                {
                    var nextBlockPosition = blockView.Position;
                    nextBlockPosition.Y -= 1;
                    blockView.Position = nextBlockPosition;
                }

                var nextBoardPosition = _boardPosition;
                nextBoardPosition.Y -= 1;

                transform.position = _field.PositionConverter.ToWorldPosition(_field, nextBoardPosition);

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
                && blockPosition.X <= _field.Rows && blockPosition.Y <= _field.Columns
                && blockPosition.X >= 0 && blockPosition.Y >= 0)
                return true;

            return false;
        }
    }
}
