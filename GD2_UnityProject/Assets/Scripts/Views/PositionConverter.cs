using BoardBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Views
{
    [CreateAssetMenu(fileName = "DefaultPositionConverter", menuName = "PositionConverter")]
    public class PositionConverter : ScriptableObject
    {
        // Still relies on center of blockarray being at 0,0;

        [SerializeField]
        private Vector3 _blockScale = Vector3.one;

        public Vector3 BlockScale => _blockScale;


        public BlockPosition ToBlockPosition(BlockArray array, Vector3 worldPosition)
        {
            var boardSize = Vector3.Scale(new Vector3(array.Columns, array.Rows, 1), BlockScale);

            var boardOffset = boardSize / 2;
            boardOffset.z = 0;

            worldPosition = new Vector3(Mathf.Floor(worldPosition.x), Mathf.Floor(worldPosition.y), 0);

            var unevenRowColOffset = new Vector3(array.Columns % 2, array.Rows % 2, 0);
            worldPosition -= unevenRowColOffset / 2;

            var offset = worldPosition + boardOffset;

            var boardPosition = new BlockPosition
            {
                X = (int)(offset.x / BlockScale.x),
                Y = (int)(offset.y / BlockScale.y)
            };

            return boardPosition;
        }

        public Vector3 ToWorldPosition(BlockArray array, BlockPosition blockPosition)
        {
            var boardSize = Vector3.Scale(new Vector3(array.Columns, array.Rows, 1), BlockScale);

            var boardOffset = boardSize / 2;
            boardOffset.z = 0;

            var worldPosition = Vector3.Scale(new Vector3(blockPosition.X,blockPosition.Y, 0), BlockScale) - boardOffset;

            return worldPosition;
        }
    }
}
