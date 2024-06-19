using NoHope.RunTime.EnemyScripts;
using NoHope.RunTime.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    [BoxGroup("Movement")]
    [SerializeField]
    private GameEnums.MovementAi _aiStyle = GameEnums.MovementAi.Horizontal;
    [BoxGroup("Movement")]
    [SerializeField]
    private float _waitTimer = 2f;
    private float _waitTimeStamp = 0f;



    public EnemyIdleState(EnemyBase enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
