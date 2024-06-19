using NoHope.RunTime.EnemyScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyBase _enemy;
    protected EnemyStateMachine _stateMachine;

    public EnemyState (EnemyBase enemy, EnemyStateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
