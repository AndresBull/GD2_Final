﻿using BoardSystem;
using GameSystem.Characters;
using GameSystem.Management;
using Graphical;
using SoundSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSystem.Views
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _smallSmokePrefab = null, _bigSmokePrefab = null;
        private List<Block> _shapeBlocks = new List<Block>();
        private BlockPosition _blockPosition;
        private BlockField _field;
        private BlockFieldView _fieldView;
        private float _elapsedDelayTime = 0f;
        private float _minTimeDelay = 0.1f;
        private bool _wasThrownHard;

        public FloodFill floodFiller;
        public int BlockWidth { get; internal set; }
        public int BlockHeight { get; internal set; }
        public float DropDownDelay { get; internal set; } = 0.4f;


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
            int lowestX = _field.Columns + 1;
            int lowestY = _field.Rows + 1;
            int highestX = -100;
            int highestY = -100;

            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.name.Contains("Anchor"))
                {
                    BlockPosition blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, child.position);
                    lowestX = Math.Min(lowestX, blockPosition.X);
                    lowestY = Math.Min(lowestY, blockPosition.Y);
                    highestX = Math.Max(highestX, blockPosition.X);
                    highestY = Math.Max(highestY, blockPosition.Y);
                }
            }

            BlockWidth = highestX - lowestX + 1;
            BlockHeight = highestY - lowestY + 1;
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
            if (_elapsedDelayTime >= DropDownDelay)
            {
                _elapsedDelayTime -= DropDownDelay;
                int offsetPosition = -1;

                //Should only happen if the block is put atop the limit
                if (_shapeBlocks.Any(b => _field.BlockAt(new BlockPosition(b.Position.X, b.Position.Y + offsetPosition)) != null)
                    || _shapeBlocks.Any(b => b.Position.Y <= 0))
                {
                    offsetPosition = 0;
                    Debug.Log("Block Above Height Limit");

                    PlayerConfigManager.Instance.TriggerRoundOver(-1);

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

                        //Update all views/blocks/positions and register them in the field
                        gameObject.GetComponent<Collider>().enabled = true;
                        UpdateBlockView(new Vector2Int(0, offsetPosition));
                        foreach (Block block in _shapeBlocks)
                        {
                            if (block.Position.Y >= _field.Rows)
                            {
                                Debug.Log("Block Above Height Limit");
                                PlayerConfigManager.Instance.TriggerRoundOver(-1);
                                return;
                            }

                            _field.AddToDictionary(block);
                        }

                        //Floodfill and check for trapped players
                        FloodFillCheck();

                        //Sound effects
                        if (_wasThrownHard)
                        {
                            Camera.main.gameObject.GetComponent<ScreenShake>().BigShake();
                            SoundManager.Instance.PlayFastBlockLanded();
                            Instantiate(_bigSmokePrefab, transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Camera.main.gameObject.GetComponent<ScreenShake>().SmallShake();
                            SoundManager.Instance.PlaySlowBlockLanded();
                            Instantiate(_smallSmokePrefab, transform.position, Quaternion.identity);
                        }

                        //call when nothing special happenend
                        _fieldView.AddToBlockViewList(this);
                        StopUpdate();
                        return;
                    }

                    //Functions when normally falling
                    UpdateBlockView(new Vector2Int(0, offsetPosition));

                    //Need to make this a smoother function
                    DropDownDelay = Mathf.Max(DropDownDelay * 0.8f, _minTimeDelay);
                }
            }
        }

        private void FloodFillCheck()
        {
            floodFiller.FloodedPositions = floodFiller.Flood(new BlockPosition(0, _field.Rows));
            foreach (GameObject climber in PlayerConfigManager.Instance.GetAllClimbers())
            {
                climber.GetComponent<ClimberBehaviour>().CheckIfTrapped();
            }

            //Check for next round
            if (PlayerConfigManager.Instance.GetAllClimbers().All(c => !c.activeInHierarchy))
            {
                int overlordIndex = PlayerConfigManager.Instance.GetOverlordIndex();
                PlayerConfigManager.Instance.TriggerRoundOver(overlordIndex);
                StopUpdate();
                return;
            }
        }

        private void FirstBlockCheck()
        {
            if (_shapeBlocks.Any(b => _field.BlockAt(new BlockPosition(b.Position.X, b.Position.Y - 1)) != null)
                    || _shapeBlocks.Any(b => b.Position.Y <= 0))
            {
                Debug.Log("Block Above Height Limit");

                PlayerConfigManager.Instance.TriggerRoundOver(-1);

                StopUpdate();
            }
        }

        private void FixedUpdate()
        {
            var blockHeightPercentage = Mathf.Min((_elapsedDelayTime / DropDownDelay), 1);

            transform.position = _fieldView.PositionConverter.ToWorldPosition(_field, _blockPosition)
                + new Vector3((BlockWidth % 2) * (_fieldView.PositionConverter.BlockScale.x / 2), 0, 0)
                - new Vector3(0, blockHeightPercentage * _fieldView.PositionConverter.BlockScale.y, 0);
        }

        private void UpdateBlockView(Vector2Int offset)
        {
            UpdateBlocks(offset);

            BlockPosition blockPosition = _blockPosition;
            blockPosition.X += offset.x;
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
                Block newBlock = new Block(block.Position.X + offset.x, block.Position.Y + offset.y);
                _shapeBlocks[i] = newBlock;
            }
        }

        private void StopUpdate()
        {
            DropDownDelay = 1;
            _elapsedDelayTime = 0;
            _wasThrownHard = false;
            enabled = false;
        }


        public void FastDrop()
        {
            SetShape();
            _blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, transform.position);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.FloodFiller = floodFiller;
            _wasThrownHard = true;
            DropDownDelay = _minTimeDelay;

            enabled = true;
            FirstBlockCheck();
        }

        public void SlowDrop()
        {
            SetShape();
            _blockPosition = _fieldView.PositionConverter.ToBlockPosition(_field, transform.position);
            floodFiller = new FloodFill(Neighbours);
            ClimberBehaviour.FloodFiller = floodFiller;
            _wasThrownHard = false;

            enabled = true;
            FirstBlockCheck();
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

        public bool PushBlock(Vector3 playerPosition, int direction)
        {
            //Check if the player is standing on the block
            var posBelowPlayer = _fieldView.PositionConverter.ToBlockPosition(_field, playerPosition);
            posBelowPlayer.Y -= 1;
            if (_shapeBlocks.Contains(_field.BlockAt(posBelowPlayer)))
                return false;


            //Check for blocks above
            foreach (var block in _shapeBlocks)
            {
                var posAbove = block.Position;
                posAbove.Y += 1;

                var checkBlock = _field.BlockAt(posAbove);
                if (_shapeBlocks.Contains(checkBlock))
                    continue;

                if (checkBlock != null)
                    return false;
            }


            //Check if other blocks are in the direction
            foreach (var block in _shapeBlocks)
            {
                var posInDirection = block.Position;
                posInDirection.X += direction;

                if (posInDirection.X < 0 || posInDirection.X >= _field.Columns)
                    return false;

                var checkBlock = _field.BlockAt(posInDirection);
                if (_shapeBlocks.Contains(checkBlock))
                    continue;

                if (checkBlock != null)
                    return false;
            }


            //Unregister old blocks from field
            foreach (Block b in _shapeBlocks)
            {
                _field.RemoveFromDictionary(b);
            }


            //Move blockView to the Direction
            Instantiate(_smallSmokePrefab, transform.position, Quaternion.identity);
            UpdateBlockView(new Vector2Int(direction, 0));
            SoundManager.Instance.PlayPushBlock();


            //Check for blocks below
            foreach (var block in _shapeBlocks)
            {
                var posBelow = block.Position;
                posBelow.Y -= 1;

                if (posBelow.Y < 0)
                {
                    foreach (Block b in _shapeBlocks)
                    {
                        _field.AddToDictionary(b);
                    }
                    FloodFillCheck();
                    Instantiate(_smallSmokePrefab, transform.position, Quaternion.identity);
                    return true;
                }

                var checkBlock = _field.BlockAt(posBelow);
                if (_shapeBlocks.Contains(checkBlock))
                    continue;

                if (checkBlock != null)
                {
                    foreach (Block b in _shapeBlocks)
                    {
                        _field.AddToDictionary(b);
                    }
                    FloodFillCheck();
                    Instantiate(_smallSmokePrefab, transform.position, Quaternion.identity);
                    return true;
                }
            }


            //Fall again
            SlowDrop();
            return true;
        }

        public bool CheckIfContainsBlock(Block block)
        {
            return _shapeBlocks.Contains(block);
        }
    }
}