using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; set; }

    public void Initialize(EnemyState StartingState)
    {
        CurrentEnemyState = StartingState;
        CurrentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState NewState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = NewState;
        CurrentEnemyState.EnterState();
    }
}
