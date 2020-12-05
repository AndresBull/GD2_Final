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
        ConnectBlockViews(Array);
    }

    private void ConnectBlockViews(BlockArray array)
    {
        var blockViews = FindObjectsOfType<BlockView>();
        foreach (var blockView in blockViews)
        {
            var boardPosition = _positionConverter.ToBlockPosition(array, blockView.transform.position);
            blockView.BottomLeftBlockPosition = boardPosition;
            blockView.GetAllBlockPositions();
            array.AddToDictionary(blockView);
        }
    }
}
