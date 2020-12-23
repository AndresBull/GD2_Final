using BoardBase;
using Utils;
using Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts;

public class GameLoop : SingletonMonoBehaviour<GameLoop>
{
    [SerializeField]
    private PositionConverter _positionConverter = null;

    public BlockArray Array { get; } = new BlockArray(8, 8);

    public GameState GameState { get; set; } = GameState.Menu;

    public PositionConverter PositionConverter => _positionConverter;

    private void Awake()
    {
        var combinedBlockViews = FindObjectsOfType<CombinedBlockView>();
        foreach (var combinedBlockView in combinedBlockViews)
        {
            AllignBlockViews(combinedBlockView);
        }
    }


    /// <summary>
    /// Temp function that alligns blocks to the worldposition that their blockposition would be.
    /// </summary>
    public void AllignBlockViews(CombinedBlockView combinedBlockView)
    {
        combinedBlockView.GetAllBlockViews();

        foreach (var blockView in combinedBlockView.BlockViews)
        {
            var boardPosition = _positionConverter.ToBlockPosition(Array, blockView.transform.position);
            blockView.transform.position = _positionConverter.ToWorldPosition(Array, boardPosition);
            blockView.BottomLeftBlockPosition = boardPosition;
        }
    }


    /// <summary>
    /// <br>Needs to be called when a CombinedBlock lands.</br>
    /// <br>Runs over all BlockViews in the children.</br>
    /// <br>Adds the blocks to a dictionary where it can be called again by the "ToWorldPosition".</br>
    /// </summary>
    /// function<param name="array"></param>
    /// <param name="combinedBlockView"></param>
    public void ConnectBlockViews(CombinedBlockView combinedBlockView)
    {
        foreach (var blockView in combinedBlockView.BlockViews)
        {
            var boardPosition = _positionConverter.ToBlockPosition(Array, blockView.transform.position);
            blockView.BottomLeftBlockPosition = boardPosition;
        }

        Array.AddToDictionary(combinedBlockView);
    }

    
}

public enum GameState
{
    Menu,
    CharacterSetup,
    Play,
    RoundOver
}
