using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utils;
using BoardSystem;
using GameSystem.Views;

namespace GameSystem.Management
{
    public class GameLoop : SingletonMonoBehaviour<GameLoop>
    {
        public BlockFieldView FieldView { get; } = new BlockFieldView(8, 8);

        public GameState GameState { get; internal set; }
    }

    public enum GameState
    {
        Menu,
        CharacterSetup,
        Play,
        RoundOver
    }
}
