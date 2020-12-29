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
        private StateMachine<BaseState> _stateMachine;

        public BlockField Field { get; private set; }
        public BlockFieldView FieldView { get; internal set; }
        public StateMachine<BaseState> StateMachine => _stateMachine;

        private void Awake()
        {
            SetupStateMachine();
            CreateNewField(8, 8);
        }

        public void CreateNewField(int rows, int columns)
        {
            Field = new BlockField(rows, columns);
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