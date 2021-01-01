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
    public class BlockFieldView : MonoBehaviour
    {
        [SerializeField]
        private PositionConverter _positionConverter = null;

        public PositionConverter PositionConverter => _positionConverter;

        private void Awake()
        {
            GameLoop.Instance.FieldView = this;
            GenerateTempWalls();
        }

        private void GenerateTempWalls()
        {
            var leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);

            var wallScale = new Vector3(1, GameLoop.Instance.Field.Rows * PositionConverter.BlockScale.y, 1);
            var wallWidthOffset = new Vector3(wallScale.x / 2f, 0, 0);

            var leftWallPos = new Vector3(-GameLoop.Instance.Field.Columns * PositionConverter.BlockScale.x / 2, 0, 0) - wallWidthOffset;
            var rightWallPos = new Vector3(GameLoop.Instance.Field.Columns * PositionConverter.BlockScale.x / 2, 0, 0) + wallWidthOffset;

            leftWall.transform.position = leftWallPos;
            leftWall.transform.localScale = wallScale;
            rightWall.transform.position = rightWallPos;
            rightWall.transform.localScale = wallScale;
        }
    }
}