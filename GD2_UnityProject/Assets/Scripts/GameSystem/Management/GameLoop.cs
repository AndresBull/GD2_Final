using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utils;
using BoardSystem;
using GameSystem.Views;
using System;
using UnityEngine.InputSystem;
using UnityEditorInternal;

namespace GameSystem.Management
{
    public class GameLoop : SingletonMonoBehaviour<GameLoop>
    {
        [SerializeField] private int _fieldRows = 8;
        [SerializeField] private int _fieldColumns = 8;

        private PlayState _playState;
        private StateMachine<BaseState> _stateMachine;

        public BlockField Field { get; private set; }
        public BlockFieldView FieldView { get; internal set; }
        public StateMachine<BaseState> StateMachine => _stateMachine;

        #region Unity Lifecycle
        private void Awake()
        {
            if (Time.time <= Time.deltaTime)
            {
                SetupStateMachine();
            }
        }

        private void OnDestroy()
        {
            _playState.OnPlayStateEntered -= OnPlayStateEntered;
            Application.Quit();
        }
        #endregion

        private void CreateNewField(int rows, int columns)
        {
            Field = new BlockField(rows, columns);
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine<BaseState>();

            MenuState menuState = new MenuState();
            SetupState setupState = new SetupState();
            RulesState rulesState = new RulesState();
            _playState = new PlayState();
            RoundOverState roundOverState = new RoundOverState();
            EndState endState = new EndState();
            GameOverState gameOverState = new GameOverState();

            _stateMachine.RegisterState(GameStates.GameOverState, gameOverState);
            _stateMachine.RegisterState(GameStates.Menu, menuState);
            _stateMachine.RegisterState(GameStates.Setup, setupState);
            _stateMachine.RegisterState(GameStates.Rules, rulesState);
            _stateMachine.RegisterState(GameStates.Play, _playState);
            _stateMachine.RegisterState(GameStates.RoundOver, roundOverState);
            _stateMachine.RegisterState(GameStates.End, endState);

            _stateMachine.MoveTo(GameStates.Menu);

            _playState.OnPlayStateEntered += OnPlayStateEntered;
        }

        #region Events
        private void OnPlayStateEntered(object sender, EventArgs e)
        {
            CreateNewField(_fieldRows, _fieldColumns);
        }
        #endregion
    }
}