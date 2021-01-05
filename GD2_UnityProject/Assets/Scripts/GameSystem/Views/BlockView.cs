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
        private BlockPosition _blockPosition;
        private BlockField _field;
        private BlockFieldView _fieldView;
        private float _elapsedDelayTime = 0f;
        private float _minTimeDelay = 0.1f;
        private bool _wasThrownHard;

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


        public FloodFill floodFiller;
        public int BlockWidth { get; internal set; }
        public int BlockHeight { get; internal set; }
        public float _dropDownDelay { get; internal set; } = 0.4f;


        private void Awake()
        {
            _field = GameLoop.Instance.Field;
            _fieldView = GameLoop.Instance.FieldView;

            if (_field == null || _fieldView == null)
            {
                StartCoroutine(SetupBlock());
                return;
            }

            SetDimensions();
            enabled = false;
        }

        private IEnumerator SetupBlock()
        {
            while (_field == null || _fieldView == null)
            {
                _field = GameLoop.Instance.Field;
                _fieldView = GameLoop.Instance.FieldView;

                yield return new WaitForSeconds(0.1f);
            }

            SetDimensions();
            enabled = false;
        }

        private void SetSize()
        {
            Size = _fieldView.PositionConverter.BlockScale;
        }

        private void SetShape()
        {
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.name.Contains("Anchor"))
                {
                    BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, child.position);
                    Block block = new Block(blockPosition.X, blockPosition.Y);
                    _shapeBlocks.Add(block);

                    Destroy(child.gameObject);
                }
            }
        }

        private void SetDimensions()
        {
            BlockWidth = 0;
            BlockHeight = 0;
            BlockPosition baseAnchorBlockPos = _fieldView.PositionConverter.ToBlockPosition(_field, transform.GetChild(0).position);
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform anchor = transform.GetChild(i);
                BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, anchor.position);

                int width = blockPosition.X - baseAnchorBlockPos.X + 1;
                BlockWidth = Mathf.Max(width, BlockWidth);

                int height = blockPosition.Y - baseAnchorBlockPos.Y + 1;
                BlockHeight = Mathf.Max(height, BlockHeight);
            }
        }

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
                && blockPosition.X < _field.Columns && blockPosition.Y <= _field.Rows
                && blockPosition.X >= 0 && blockPosition.Y >= 0)
                return true;

            return false;
        }

        private void Update()
        {
            _elapsedDelayTime += Time.deltaTime;
            if (_elapsedDelayTime >= _dropDownDelay)
            {
                _elapsedDelayTime -= _dropDownDelay;
                int offsetPosition = -1;

                //Should only happen if the block is put atop the limit
                if (_shapeBlocks.Any(b => _field.BlockAt(new BlockPosition(b.Position.X, b.Position.Y + offsetPosition)) != null)
                    || _shapeBlocks.Any(b => b.Position.Y <= 0))
                {
                    offsetPosition = 0;
                    Debug.Log("Block Above Height Limit");

                    GameLoop.Instance.StateMachine.MoveTo(GameStates.Play);
                    StopUpdate();
                    return;
                }


                //Working like normal
                if (offsetPosition != 0)
                {
                    //Landed check
                    if (_shapeBlocks.Any(b => _field.BlockAt(new BlockPosition(b.Position.X, b.Position.Y + offsetPosition * 2)) != null)
                        || _shapeBlocks.Any(b => b.Position.Y + offsetPosition <= 0))
                    {
                        Debug.Log("Landed");

                        UpdateBlockView(new Vector2Int(0, offsetPosition));

                        foreach (Block block in _shapeBlocks)
                        {
                            _field.AddToDictionary(block);
                        }

                        floodFiller.FloodedPositions = floodFiller.Flood(new BlockPosition(0, _field.Rows));

                        foreach (GameObject climber in PlayerConfigManager.Instance.GetAllClimbers())
                        {
                            climber.GetComponent<ClimberBehaviour>().CheckIfTrapped();
                        }

                        if (PlayerConfigManager.Instance.GetAllClimbers().All(c => !c.activeInHierarchy))
                        {
                            GameLoop.Instance.StateMachine.MoveTo(GameStates.Play);
                            StopUpdate();
                            return;
                        }

                        if (_wasThrownHard)
                        {
                            Camera.main.gameObject.GetComponent<ScreenShake>().BigShake();
                            SoundManager.Instance.PlayFastBlockLanded();
                        }
                        else
                        {
                            Camera.main.gameObject.GetComponent<ScreenShake>().SmallShake();
                            SoundManager.Instance.PlaySlowBlockLanded();
                        }

                        StopUpdate();
                        return;
                    }

                    //Functions when normally falling
                    UpdateBlockView(new Vector2Int(0,offsetPosition));

                    //Need to make this a smoother function
                    _dropDownDelay = Mathf.Max(_dropDownDelay * 0.8f, _minTimeDelay);
                }
            }
        }

        private void FixedUpdate()
        {
            var blockHeightPercentage = Mathf.Min((_elapsedDelayTime / _dropDownDelay), 1);

            transform.position = _fieldView.PositionConverter.ToWorldPosition(_field, _blockPosition)
                + new Vector3((BlockWidth % 2) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0)
                - new Vector3(0, blockHeightPercentage * _fieldView.PositionConverter.BlockScale.y, 0);
        }

        private void UpdateBlockView(Vector2Int offset)
        {
            UpdateBlocks(offset);

            BlockPosition blockPosition = _blockPosition;
            blockPosition.Y += offset.y;
            _blockPosition = blockPosition;

            transform.position = _fieldView.PositionConverter.ToWorldPosition(_field, blockPosition)
                + new Vector3((BlockWidth % 2) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0);
        }

        private void UpdateBlocks(Vector2Int offset)
        {
            for (int i = 0; i < _shapeBlocks.Count; i++)
            {
                Block block = _shapeBlocks[i];
                Block newBlock = new Block(block.Position.X +offset.x, block.Position.Y + offset.y);
                _shapeBlocks[i] = newBlock;
            }
        }

        private void StopUpdate()
        {
            _dropDownDelay = 1;
            _elapsedDelayTime = 0;
            _wasThrownHard = false;
            enabled = false;
        }



        public void FastDrop()
        {
            SetShape();
            _blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, transform.position);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.floodFiller = floodFiller;
            _wasThrownHard = true;
            _dropDownDelay = _minTimeDelay;

            enabled = true;
        }

        public void SlowDrop()
        {
            SetShape();
            _blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, transform.position);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.floodFiller = floodFiller;
            _wasThrownHard = false;

            enabled = true;
        }

        public void MoveAccordingToHand(Vector3 handPosition)
        {
            Vector3 handOffset = new Vector3((1 - (BlockWidth % 2)) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0); //if even width, set to half a block wide
            handPosition += handOffset;

            BlockPosition handBlockPos = _fieldView.PositionConverter.ToBlockPosition(_field, handPosition);
            handBlockPos.Y -= 2;

            int leftLimit = (BlockWidth - (BlockWidth % 2)) / 2;
            int rightLimit = _field.Columns - (BlockWidth + (BlockWidth % 2)) / 2;
            handBlockPos.X = Mathf.Clamp(handBlockPos.X, leftLimit, rightLimit);

            Vector3 newPos = _fieldView.PositionConverter.ToWorldPosition(_field, handBlockPos);
            Vector3 blockOffset = new Vector3((BlockWidth % 2) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0);
            newPos += blockOffset;
            transform.position = newPos;
        }

        public void PushBlock(Vector3 playerPosition)
        {
            //Check if the player is standing on the block
            var posBelowPlayer = _fieldView.PositionConverter.ToBlockPosition(_field, playerPosition);
            posBelowPlayer.Y -= 1;
            if (_shapeBlocks.Contains(_field.BlockAt(posBelowPlayer)))
                return;


            //Check for blocks above
            foreach (var block in _shapeBlocks)
            {
                var posAbove = block.Position;
                posAbove.Y += 1;

                var checkBlock = _field.BlockAt(posAbove);
                if (_shapeBlocks.Contains(checkBlock))
                    continue;

                if (checkBlock != null)
                    return;
            }


            //Checks if player is left or right and set direction (simple, but might give problems with weird shapes)
            int direction;
            if (transform.position.x <= playerPosition.x)
                direction = -1;
            else
                direction = 1;


            //Check if other blocks are in the direction
            foreach (var block in _shapeBlocks)
            {
                var posInDirection = block.Position;
                posInDirection.X += direction;

                var checkBlock = _field.BlockAt(posInDirection);
                if (_shapeBlocks.Contains(checkBlock))
                    continue;

                if (checkBlock != null)
                    return;
            }


            //Move blockView to the right
            UpdateBlockView(new Vector2Int(direction, 0));
            SoundManager.Instance.PlayPushBlock();


            //Check for blocks below
            foreach (var block in _shapeBlocks)
            {
                var posBelow = block.Position;
                posBelow.Y -= 1;

                var checkBlock = _field.BlockAt(posBelow);
                if (_shapeBlocks.Contains(checkBlock))
                    continue;

                if (checkBlock != null)
                    return;
            }


            //Fall again
            SlowDrop();
        }
    }
}