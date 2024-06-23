using NoHope.RunTime.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public abstract void InitializeState(StateMachine StateMachine);
    public abstract void EnterState(StateMachine StateMachine);
    public abstract void Logic(StateMachine StateMachine);
    public abstract void Physics(StateMachine StateMachine);
    public abstract void ExitState(StateMachine StateMachine);
    public abstract void OnCollision(StateMachine StateMachine, Collision2D Collision);
    public abstract void OffCollision(StateMachine StateMachine, Collision2D Collision);
}
