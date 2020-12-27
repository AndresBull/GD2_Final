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
        // TODO: remove this positoinconverter and use the one in blockfieldview
        [SerializeField]
        private PositionConverter _positionConverter = null;

        private StateMachine<BaseState> _stateMachine;

        public BlockFieldView FieldView { get; } = new BlockFieldView(8, 8);
        public PositionConverter PositionConverter => _positionConverter;
        public StateMachine<BaseState> StateMachine => _stateMachine;

        private void Awake()
        {
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine<BaseState>();

            var menuState = new MenuState();
            var setupState = new SetupState();
            var playState = new PlayState();
            var roundOverState = new RoundOverState();

            _stateMachine.RegisterState(GameStates.Menu, menuState);
            _stateMachine.RegisterState(GameStates.Setup, setupState);
            _stateMachine.RegisterState(GameStates.Play, playState);
            _stateMachine.RegisterState(GameStates.RoundOver, roundOverState);

            _stateMachine.MoveTo(GameStates.Menu);
        }
    }
}
