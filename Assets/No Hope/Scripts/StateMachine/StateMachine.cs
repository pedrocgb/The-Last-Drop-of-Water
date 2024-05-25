using NoHope.RunTime.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : Enum
{
    protected Dictionary<T, BaseState<T>> States = new Dictionary<T, BaseState<T>>();
    protected BaseState<T> CurrentState;
    protected bool isTransitioningState = false;

    //-------------------------------------------------------------------

    private void Start()
    {
        CurrentState?.EnterState();
    }

    private void Update()
    {
        if (isTransitioningState)
            return;

        T nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else
        {
            StateTransition(nextStateKey);
        }
    }

    //-------------------------------------------------------------------

    public void StateTransition(T stateKey)
    {
        if (!States.ContainsKey(stateKey))
        {
            Debug.LogWarning($"State {stateKey} does not exist in the state dictionary.");
            return;
        }

        isTransitioningState = true;
        CurrentState?.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        isTransitioningState = false;
    }

    //-------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CurrentState?.OnTriggerEnter(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CurrentState?.OnTriggerStay(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CurrentState?.OnTriggerExit(collision);
    }

}
