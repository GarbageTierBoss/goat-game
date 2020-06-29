using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringFSM
{
    private Dictionary<string, List<string>> _states;

    private string _currentState;

    public StringFSM(Dictionary<string, List<string>> states, string initState)
    {
        if (!states.ContainsKey(initState))
        {
            throw new System.ArgumentException($"Initial state {initState} doesn't exist.");
        }

        _states = states;
        _currentState = initState;
    }

    public string GetCurrentState()
    {
        return _currentState;
    }

    public string ChangeState(string newState)
    {
        if (!_states.ContainsKey(newState))
        {
            throw new System.ArgumentException("The state {newState} does not exist");
        }

        if (_states[_currentState].Contains(newState))
        {
            _currentState = newState;
        }

        return _currentState;
    }
}
