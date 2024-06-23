using NoHope.RunTime.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoHope.RunTime.Utilities.GameEnums;

[CreateAssetMenu(fileName = "Tractor Dash State", menuName = "No Hope/AI/Tractor Dash")]
public class TractorDashState : State
{
    [BoxGroup("Movement")]
    [SerializeField]
    private float _dashSpeed = 0f;
    [BoxGroup("Movement")]
    [SerializeField]
    private float _dashDuration = 0f;
    private float _dashTimestamp = 0f;
    private Vector2 _direction = Vector2.zero;

    [BoxGroup("States")]
    [SerializeField]
    private State _idleState = null;
    [BoxGroup("States")]
    [SerializeField]
    private State _stunnedState = null;

    public override void EnterState(StateMachine StateMachine)
    {
        _dashTimestamp = Time.time + _dashDuration;
        StateMachine.MyEnemy.FacesPlayer = false;
        if (StateMachine.MyEnemy.FacingLeft)
            _direction = Vector2.left;
        else
            _direction = Vector2.right;
    }

    public override void ExitState(StateMachine StateMachine)
    {
        StateMachine.MyRigidbody.velocity = Vector2.zero;
        StateMachine.MyEnemy.FacesPlayer = true;
    }

    public override void InitializeState(StateMachine StateMachine)
    {
        _dashTimestamp = Time.time + _dashDuration;
        if (StateMachine.MyEnemy.FacingLeft)
            _direction = Vector2.left;
        else
            _direction = Vector2.right;
    }

    public override void Logic(StateMachine StateMachine)
    {
        if (_dashTimestamp <= Time.time)
            StateMachine.ChangeState(_idleState);
    }

    public override void Physics(StateMachine StateMachine)
    {
        float targetSpeed = StateMachine.transform.right.x * _dashSpeed;
        targetSpeed = Mathf.Lerp(StateMachine.MyRigidbody.velocity.x, targetSpeed, 1f);

        StateMachine.MyRigidbody.AddForce(targetSpeed * _direction, ForceMode2D.Force);
    }
    public override void OffCollision(StateMachine StateMachine, Collision2D Collision)
    {
        
    }

    public override void OnCollision(StateMachine StateMachine, Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Wall"))
        {
            StateMachine.ChangeState(_stunnedState);
        }
    }
}
