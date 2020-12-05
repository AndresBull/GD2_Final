using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionConverter _positionConverter = null;

    public BlockArray Array { get; } = new BlockArray(8,8);



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
            var block = array.BlockAt(boardPosition);
            blockView.Model = block;
        }
    }
}
