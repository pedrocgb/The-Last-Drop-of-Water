using NoHope.RunTime.AI;
using NoHope.RunTime.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Patrol State", menuName = "No Hope/AI/Patrol State")]
public class PatrolState : State
{
    [BoxGroup("Movement")]
    [SerializeField]
    private GameEnums.MovementAi _direction = GameEnums.MovementAi.Horizontal;
    [BoxGroup("Movement")]
    [SerializeField]
    private float _waitTimer = 2f;
    private float _waitTimeStamp = 0f;

    private Vector2 Direction
    {
        get
        {
            switch (_direction)
            {
                default:
                case GameEnums.MovementAi.Horizontal:
                    return Vector2.right;
                case GameEnums.MovementAi.Vertical:
                    return Vector2.up;

            }
        }
    }
    private float _cartesianPoint = 0f;

    [BoxGroup("Movement")]
    [SerializeField]
    private float _movementSpeed = 0f;
    [BoxGroup("Movement")]
    [SerializeField]
    private Vector3 _desiredPosition;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _reachedDesiredPosition = false;

    #region States Methods
    public override void InitializeState(StateMachine StateMachine)
    {
        _waitTimeStamp = Time.time + _waitTimer;
        _startPosition = StateMachine.transform.position;
        _targetPosition = _startPosition + _desiredPosition;
    }
    public override void EnterState(StateMachine StateMachine)
    {
        
    }
    public override void ExitState(StateMachine StateMachine)
    {
        
    }
    public override void Logic(StateMachine StateMachine)
    {
        
    }
    public override void Physics(StateMachine StateMachine)
    {
        MoveEnemy(StateMachine);
    }
    public override void OnCollision(StateMachine StateMachine, Collision2D Collision)
    {
        
    }

    public override void OffCollision(StateMachine StateMachine, Collision2D Collision)
    {
        
    }
    #endregion

    private void MoveEnemy(StateMachine stateMachine)
    {

        if (!_reachedDesiredPosition && _waitTimeStamp <= Time.time)
        {
            stateMachine.MyRigidbody.MovePosition(stateMachine.MyRigidbody.position + Direction * _movementSpeed * Time.fixedDeltaTime); ;

            switch (_direction)
            {
                default:
                case GameEnums.MovementAi.Horizontal:
                    if (stateMachine.transform.position.x >= _targetPosition.x)
                    {
                        _reachedDesiredPosition = true;
                        _waitTimeStamp = Time.time + _waitTimer;
                    }
                    break;
                case GameEnums.MovementAi.Vertical:
                    if (stateMachine.transform.position.y >= _targetPosition.y)
                    {
                        _reachedDesiredPosition = true;
                        _waitTimeStamp = Time.time + _waitTimer;
                    }
                    break;
            }                   
        }
        else if (_reachedDesiredPosition && _waitTimeStamp <= Time.time)
        {
            stateMachine.MyRigidbody.MovePosition(stateMachine.MyRigidbody.position + (-Direction) * _movementSpeed * Time.fixedDeltaTime);
                            
            switch (_direction)
            {
                default:
                case GameEnums.MovementAi.Horizontal:
                    if (stateMachine.transform.position.x <= _startPosition.x)
                    {
                        _reachedDesiredPosition = false;
                        _waitTimeStamp = Time.time + _waitTimer;
                    }
                    break;
                case GameEnums.MovementAi.Vertical:
                    if (stateMachine.transform.position.y <= _startPosition.y)
                    {
                        _reachedDesiredPosition = false;
                        _waitTimeStamp = Time.time + _waitTimer;
                    }
                    break;
            }
        }
    }

}
