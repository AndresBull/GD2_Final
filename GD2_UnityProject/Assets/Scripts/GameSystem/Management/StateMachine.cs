using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.Management
{
    public class StateMachine<TState> where TState : IState<TState>
    {
        private Dictionary<string, TState> _states = new Dictionary<string, TState>();

        public TState CurrentState { get; internal set; }

        public void RegisterState(string name, TState state)
        {
            state.StateMachine = this;
            _states.Add(name, state);
        }

        public void MoveTo(string name)
        {
            var state = _states[name];

            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState?.OnEnter();
        }
    }

    public interface IState<TState> where TState : IState<TState>
    {
        void OnEnter();

        void OnExit();

        StateMachine<TState> StateMachine { set; }
    }

    public static class GameStates
    {
        public const string Start = "Start";
        public const string Menu = "Menu";
        public const string Setup = "Setup";
        public const string Play = "Play";
        public const string RoundOver = "RoundOver";
        public const string End = "End";
    }

    public abstract class BaseState : IState<BaseState>
    {
        public StateMachine<BaseState> StateMachine { protected get; set; }

        public void OnEnter()
        {
            SetupScene();
        }

        public void OnExit()
        {
            CleanUpScene();
        }

        protected abstract void CleanUpScene();

        protected abstract void SetupScene();

    }
}
