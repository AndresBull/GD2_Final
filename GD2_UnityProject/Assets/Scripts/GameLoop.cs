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



    private void Awake()
    {
        AllignBlockViews();
    }

    //Temp function that alligns blocks to the worldposition that their blockposition would be
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

    //Needs to be called when a block lands
    //Gives said block a blockposition (and extra depending on the size of the block)
    //Adds the block to a dictionary where it can be called again by the "ToWorldPosition" function
    public void ConnectBlockView(BlockArray array, BlockView blockView)
    {
            var boardPosition = _positionConverter.ToBlockPosition(array, blockView.transform.position);
            blockView.BottomLeftBlockPosition = boardPosition;
            blockView.GetAllBlockPositions();
            array.AddToDictionary(blockView);
    }
}
