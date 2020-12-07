using BoardBase;
using Utils;
using Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionConverter _positionConverter = null;

    public BlockArray Array { get; } = new BlockArray(8,8);

    public string BlockType;



    private void Awake()
    {
        AllignBlockViews();
    }

    private void AllignBlockViews()
    {
        var blockViews = FindObjectsOfType<BlockView>();
        foreach (var blockView in blockViews)
        {
            var boardPosition = _positionConverter.ToBlockPosition(Array, blockView.transform.position);
            blockView.transform.position = _positionConverter.ToWorldPosition(Array, boardPosition);
            ConnectBlockView(Array, blockView);
        }
    }

    public void ConnectBlockView(BlockArray array, BlockView blockView)
    {
            var boardPosition = _positionConverter.ToBlockPosition(array, blockView.transform.position);
            blockView.BottomLeftBlockPosition = boardPosition;
            blockView.GetAllBlockPositions();
            array.AddToDictionary(blockView);
    }
}
