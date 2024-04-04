using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoHope.RunTime.Utilities;

namespace NoHope.RunTime.EnemyScripts
{
    [RequireComponent(typeof(EnemyBase))]
    public class EnemyMovement : MonoBehaviour
    {
        #region Variable and Properties
        private EnemyBase Base;

        [BoxGroup("Movement")]
        [SerializeField]
        private GameEnums.MovementAi _aiStyle = GameEnums.MovementAi.Horizontal;
        [BoxGroup("Movement")]
        [HideIf("_aiStyle", GameEnums.MovementAi.Complex)]
        [SerializeField]
        private float _waitTimer = 2f;
        private float _waitTimeStamp = 0f;

        private Vector2 Direction
        {
            get
            {
                if (_aiStyle == GameEnums.MovementAi.Horizontal)
                    return Vector2.right;
                else if (_aiStyle == GameEnums.MovementAi.Vertical)
                    return Vector2.up;
                else
                    return Vector2.zero;
            }
        }
        private float GetCartesianPoint
        {
            get
            {
                if (_aiStyle == GameEnums.MovementAi.Horizontal)
                    return transform.position.x;
                else if (_aiStyle == GameEnums.MovementAi.Vertical)
                    return transform.position.y;
                else
                    return 0;
            }
        }

        [BoxGroup("Movement")]
        [SerializeField]
        private float _movementSpeed = 0f;
        [BoxGroup("Movement")]
        [SerializeField]
        private float _desiredPosition;
        private float _targetPosition;
        private float _startPosition;
        private bool _reachedDesiredPosition = false;
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            Base = GetComponent<EnemyBase>();

            _startPosition = GetCartesianPoint;
            _targetPosition = _startPosition + _desiredPosition;
        }

        private void FixedUpdate()
        {
            MoveEnemy();
        }
        #endregion

        //-------------------------------------------------------------------

        private void MoveEnemy()
        {
            if (_aiStyle == GameEnums.MovementAi.Complex)
            {

            }
            else
            {
                if (!_reachedDesiredPosition && _waitTimeStamp <= Time.time)
                {
                    Base.MyRigidbody.MovePosition(Base.MyRigidbody.position + Direction * _movementSpeed * Time.fixedDeltaTime);
                    if (GetCartesianPoint >= _targetPosition)
                    {
                        _reachedDesiredPosition = true;
                        _waitTimeStamp = Time.time + _waitTimer;
                    }
                }
                else if (_reachedDesiredPosition && _waitTimeStamp <= Time.time)
                {
                    Base.MyRigidbody.MovePosition(Base.MyRigidbody.position + (-Direction) * _movementSpeed * Time.fixedDeltaTime);
                    if (GetCartesianPoint <= _startPosition)
                    {
                        _reachedDesiredPosition = false;
                        _waitTimeStamp = Time.time + _waitTimer;
                    }
                }
            }
        }

        //-------------------------------------------------------------------
    }
}