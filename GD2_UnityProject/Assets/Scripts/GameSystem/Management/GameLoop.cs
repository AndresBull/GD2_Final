using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Utils;
using BoardSystem;
using GameSystem.Views;
using System;

namespace GameSystem.Management
{
    public class GameLoop : SingletonMonoBehaviour<GameLoop>
    {
        private StateMachine<BaseState> _stateMachine;
        private PlayState _playState;

        public BlockField Field { get; private set; }
        public BlockFieldView FieldView { get; internal set; }
        public StateMachine<BaseState> StateMachine => _stateMachine;

        private void Awake()
        {
            if (Time.time <= Time.deltaTime)
            {
                SetupStateMachine();
                CreateNewField(8, 8);
            }
        }

        private void OnDestroy()
        {
            _playState.OnPlayStateEntered -= OnPlayStateEntered;
        }

        public void CreateNewField(int rows, int columns)
        {
            Field = new BlockField(rows, columns);
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine<BaseState>();

            StartState startState = new StartState();
            MenuState menuState = new MenuState();
            SetupState setupState = new SetupState();
            _playState = new PlayState();
            RoundOverState roundOverState = new RoundOverState();

            _stateMachine.RegisterState(GameStates.Start, startState);
            _stateMachine.RegisterState(GameStates.Menu, menuState);
            _stateMachine.RegisterState(GameStates.Setup, setupState);
            _stateMachine.RegisterState(GameStates.Play, _playState);
            _stateMachine.RegisterState(GameStates.RoundOver, roundOverState);

            _stateMachine.MoveTo(GameStates.Start);

            _playState.OnPlayStateEntered += OnPlayStateEntered;
        }           

        private void OnPlayStateEntered(object sender, EventArgs e)
        {
            // TODO: move CreateNewField here
        }
    }
}