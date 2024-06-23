using NoHope.RunTime.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tractor Idle State", menuName = "No Hope/AI/Tractor Idle")]
public class TractorIdleState : State
{
    [BoxGroup("Idle")]
    [SerializeField]
    private float _idleDuration = 0f;
    private float _idleTimestamp = 0f;

    [BoxGroup("States")]
    [SerializeField]
    private State _dashState = null;

    public override void EnterState(StateMachine StateMachine)
    {
        _idleTimestamp = Time.time + _idleDuration;
    }

    public override void ExitState(StateMachine StateMachine)
    {
        
    }

    public override void InitializeState(StateMachine StateMachine)
    {
        _idleTimestamp = Time.time + _idleDuration;
    }

    public override void Logic(StateMachine StateMachine)
    {
        if (_idleTimestamp <= Time.time)
            StateMachine.ChangeState(_dashState);
    }
    public override void Physics(StateMachine StateMachine)
    {
        
    }

    public override void OffCollision(StateMachine StateMachine, Collision2D Collision)
    {
        
    }

    public override void OnCollision(StateMachine StateMachine, Collision2D Collision)
    {
        
    }

}
