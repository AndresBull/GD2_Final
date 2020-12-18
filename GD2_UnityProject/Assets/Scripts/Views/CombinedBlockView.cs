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

        private GameLoop gameLoop;
        private BlockPosition _boardPosition;
        private float _dropDownDelay = 1f;

        void Awake()
        {
            gameLoop = GameLoop.Instance;
            _boardPosition = gameLoop.PositionConverter.ToBlockPosition(gameLoop.Array, transform.position);

            gameLoop.AllignBlockViews(this);
            StartCoroutine(Drop());
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
