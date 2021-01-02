using BoardSystem;
using GameSystem.Characters;
using GameSystem.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSystem.Views
{
    public class BlockView : MonoBehaviour
    {
        private List<Block> _shapeBlocks = new List<Block>();
        private BlockField _field;
        private BlockFieldView _fieldView;
        private BlockPosition _position;
        private float _dropDownDelay = 1f;

        private List<BlockPosition> Neighbours(BlockPosition startBlock)
        {
            List<BlockPosition> neighbours = new List<BlockPosition>();

            BlockPosition position = startBlock;

            BlockPosition northPosition = position;
            northPosition.Y += 1;
            Block northBlock = _field.BlockAt(northPosition);
            if (IsNeighbour(northBlock, northPosition))
            {
                neighbours.Add(northPosition);
            }

            BlockPosition southPosition = position;
            southPosition.Y -= 1;
            Block southBlock = _field.BlockAt(southPosition);
            if (IsNeighbour(southBlock, southPosition))
            {
                neighbours.Add(southPosition);
            }

            BlockPosition eastPosition = position;
            eastPosition.X += 1;
            Block eastBlock = _field.BlockAt(eastPosition);
            if (IsNeighbour(eastBlock, eastPosition))
            {
                neighbours.Add(eastPosition);
            }

            BlockPosition westPosition = position;
            westPosition.X -= 1;
            Block westBlock = _field.BlockAt(westPosition);
            if (IsNeighbour(westBlock, westPosition))
            {
                neighbours.Add(westPosition);
            }

            return neighbours;
        }
        private bool IsNeighbour(Block block, BlockPosition blockPosition)
        {
            if (block == null && !floodFiller.HasBlock(blockPosition)
                && blockPosition.X <= _field.Rows && blockPosition.Y <= _field.Columns
                && blockPosition.X >= 0 && blockPosition.Y >= 0)
                return true;

            return false;
        }
        internal Vector3 Size
        {
            set
            {
                transform.localScale = Vector3.one;

                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                Vector3 meshSize = meshRenderer.bounds.size;

                float ratioX = value.x / meshSize.x;
                float ratioY = value.y / meshSize.y;
                float ratioZ = value.z / meshSize.z;

                transform.localScale = new Vector3(ratioX, ratioY, ratioZ);
            }
        }

        public int BlockWidth { get; internal set; }
        public int BlockHeight { get; internal set; }
        public FloodFill floodFiller;

        private void Awake()
        {
            _field = GameLoop.Instance.Field;
            _fieldView = GameLoop.Instance.FieldView;

            if (_field == null || _fieldView == null)
            {
                StartCoroutine(SetupBlock());
                return;
            }


            //SetSize();
            SetDimensions();
            //SetShape();
            //AllignBlockToGrid();

            //BlockPosition startBlock = new BlockPosition(_field.Rows + 1, _field.Columns);
            //floodFiller = new FloodFill(Neighbours);
            //ClimberBehaviour.floodFiller = floodFiller;


            //StartCoroutine(Drop());
        }

        private IEnumerator SetupBlock()
        {
            while (_field == null || _fieldView == null)
            {
                _field = GameLoop.Instance.Field;
                _fieldView = GameLoop.Instance.FieldView;

                yield return new WaitForSeconds(0.1f);
            }

            //SetSize();
            SetDimensions();
            //SetShape();

            //AllignBlockToGrid();
            
            //BlockPosition startBlock = new BlockPosition(_field.Rows + 1, _field.Columns);
            //floodFiller = new FloodFill(Neighbours);
            //ClimberBehaviour.floodFiller = floodFiller;

            //StartCoroutine(Drop());
        }

        private void SetShape()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform anchor = transform.GetChild(i);
                BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, anchor.position);
                Block block = new Block(blockPosition.X, blockPosition.Y);
                _field.AddToDictionary(block);
                _shapeBlocks.Add(block);

                Destroy(anchor.gameObject);

                //if (i != 0)
                //{
                //    Destroy(anchor.gameObject);
                //}
            }
        }

        private void SetDimensions()
        {
            BlockWidth = 0;
            BlockHeight = 0;
            var baseAnchorBlockPos = _fieldView.PositionConverter.ToBlockPosition(_field, transform.GetChild(0).position);
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform anchor = transform.GetChild(i);
                BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, anchor.position);

                var width = blockPosition.X - baseAnchorBlockPos.X + 1;
                BlockWidth = Mathf.Max(width, BlockWidth);

                var height = blockPosition.Y - baseAnchorBlockPos.Y + 1;
                BlockHeight = Mathf.Max(height, BlockHeight);
            }
        }

        private void SetSize()
        {
            Size = _fieldView.PositionConverter.BlockScale;
        }

        private void AllignBlockToGrid()
        {
            //Transform anchor = transform.GetChild(0);
            //BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, anchor.position);
            //Vector3 worldPosition = _fieldView.PositionConverter.ToWorldPosition(_field, blockPosition);
            //
            //Vector3 offset = anchor.position - worldPosition;
            //transform.position += offset;
            //Destroy(anchor.gameObject);



            _position = _fieldView.PositionConverter.ToBlockPosition(_field, transform.position);
        }

        private void UpdateBlockView(int offsetY)
        {
            UpdateBlocks(offsetY);

            BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, transform.position);
            blockPosition.Y += offsetY;
            transform.position = _fieldView.PositionConverter.ToWorldPosition(_field, blockPosition) 
                + new Vector3((BlockWidth % 2) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0);
        }

        private void UpdateBlocks(int offset)
        {
            for (int i = 0; i < _shapeBlocks.Count; i++)
            {
                Block block = _shapeBlocks[i];
                Block newBlock = new Block(block.Position.X, block.Position.Y + offset);
                _field.AddToDictionary(newBlock);
                _shapeBlocks[i] = newBlock;
            }
        }


        public IEnumerator Drop()
        {
            SetShape();
            //AllignBlockToGrid();

            BlockPosition startBlock = new BlockPosition(_field.Rows + 1, _field.Columns);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.floodFiller = floodFiller;

            while (Application.isPlaying)
            {
                foreach (Block block in _shapeBlocks)
                {
                    _field.RemoveFromDictionary(block);
                    print($"{block.Position.X}, {block.Position.Y}");
                }

                int offsetPosition = -1;
                if (_shapeBlocks.Any(b => _field.BlockAt(new BlockPosition(b.Position.X, b.Position.Y + offsetPosition)) != null)
                    || _shapeBlocks.Any(b => b.Position.Y <= 0))
                {
                    offsetPosition = 0;
                }

                if (offsetPosition != 0)
                {
                    if (_shapeBlocks.Any(b => _field.BlockAt(new BlockPosition(b.Position.X, b.Position.Y + offsetPosition * 2)) != null)
                        || _shapeBlocks.Any(b => b.Position.Y + offsetPosition <= 0))
                    {
                        Debug.Log("Landed");
                        SoundManager.Instance.PlayBlockLanded();
                        floodFiller.FloodedPositions = floodFiller.Flood(new BlockPosition(_field.Rows, _field.Columns));

                        foreach (GameObject climber in PlayerConfigManager.Instance.GetAllClimbers())
                        {
                            climber.GetComponent<ClimberBehaviour>().CheckIfTrapped();
                        }
                        UpdateBlockView(offsetPosition);

                        yield break;
                    }
                    UpdateBlockView(offsetPosition);
                }
                yield return new WaitForSeconds(_dropDownDelay);
            }
        }

        public void MoveAccordingToHand(Vector3 handPosition)
        {
            var handOffset = new Vector3((1 - (BlockWidth % 2)) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0); //if even width, set to half a block wide
            handPosition += handOffset;

            var handBlockPos = _fieldView.PositionConverter.ToBlockPosition(_field, handPosition);
            handBlockPos.Y -= 2;

            var leftLimit = (BlockWidth - (BlockWidth % 2)) / 2;
            var rightLimit = _field.Columns - (BlockWidth + (BlockWidth % 2)) / 2;
            handBlockPos.X = Mathf.Clamp(handBlockPos.X, leftLimit, rightLimit);

            var newPos = _fieldView.PositionConverter.ToWorldPosition(_field, handBlockPos);
            var blockOffset = new Vector3((BlockWidth % 2) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0);
            newPos += blockOffset;
            transform.position = newPos;
        }
    }
}