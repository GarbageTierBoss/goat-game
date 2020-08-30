using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerStateMachine
    {
        private Dictionary<string, PlayerState> _states;
        private PlayerState _currentState;

        public PlayerStateMachine(Dictionary<string, PlayerState> states, string initState)
        {
            if (!states.ContainsKey(initState)) { throw new System.ArgumentException($"{initState} not in dictionary."); }

            _states = states;
            _currentState = _states[initState];
        }

        public PlayerState GetCurrentState()
        {
            return _currentState;
        }

        public PlayerState ChangeState(string transition)
        {
            string nextStateKey = _currentState.ChangeState(transition);
            if (_states.ContainsKey(nextStateKey))
            {
                _currentState = _states[nextStateKey];
            }

            return _currentState;
        }
    }

    public class PlayerState
    {
        public virtual string ChangeState(string transition) { return null; }
        public virtual void Update() { }
    }

