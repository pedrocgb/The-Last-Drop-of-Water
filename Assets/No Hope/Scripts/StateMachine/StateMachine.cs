using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [BoxGroup("States")]
    [SerializeField]
    private State _currentState = null;

    public void ChangeState(State NewState)
    {
        if (_currentState == null)
            return;

        _currentState.ExitState(this);
        _currentState = NewState;
        _currentState.EnterState(this);
    }
}
