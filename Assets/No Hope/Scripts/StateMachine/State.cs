using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public abstract void EnterState(StateMachine stateMachine);
    public abstract void Logic(StateMachine stateMachine);
    public abstract void Physics(StateMachine stateMachine);
    public abstract void ExitState(StateMachine stateMachine);
}
