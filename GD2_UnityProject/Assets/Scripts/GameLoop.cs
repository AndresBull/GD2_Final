using BoardBase;
using Utils;
using Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionConverter _positionConverter = null;

    public BlockArray Array { get; } = new BlockArray(8, 8);

    public PositionConverter PositionConverter => _positionConverter;

    private void Awake()
    {
        AllignBlockViews();
    }

    //Temp function that alligns blocks to the worldposition that their blockposition would be
    private void AllignBlockViews()
    {
        var combinedBlockViews = FindObjectsOfType<CombinedBlockView>();
        foreach (var combinedBlockView in combinedBlockViews)
        {
            combinedBlockView.GetAllBlockViews();

            foreach (var blockView in combinedBlockView.BlockViews)
            {
                var boardPosition = _positionConverter.ToBlockPosition(Array, blockView.transform.position);
                blockView.transform.position = _positionConverter.ToWorldPosition(Array, boardPosition);
            }

            ConnectBlockViews(Array, combinedBlockView);
        }
    }

    //Needs to be called when a block lands
    //Gives said block a blockposition (and extra depending on the size of the block)
    //Adds the block to a dictionary where it can be called again by the "ToWorldPosition" function
    public void ConnectBlockViews(BlockArray array, CombinedBlockView combinedBlockView)
    {
        combinedBlockView.GetAllBlockViews();

        foreach (var blockView in combinedBlockView.BlockViews)
        {
            var boardPosition = _positionConverter.ToBlockPosition(Array, blockView.transform.position);
            blockView.BottomLeftBlockPosition = boardPosition;
        }

        array.AddToDictionary(combinedBlockView);
    }
}
