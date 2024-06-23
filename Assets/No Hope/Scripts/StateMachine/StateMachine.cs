using NoHope.RunTime.EnemyScripts;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.AI
{
    public class StateMachine : MonoBehaviour
    {
        [BoxGroup("States")]
        [SerializeField]
        private State _currentState = null;

        public EnemyBase MyEnemy { get; private set; }
        public Rigidbody2D MyRigidbody { get; private set; }
        public Collider2D MyCollider2D { get; private set; }
        public Animator MyAnimator { get; private set; }

        private void Awake()
        {
            MyEnemy = GetComponent<EnemyBase>();
            MyRigidbody = GetComponent<Rigidbody2D>();
            MyCollider2D = GetComponent<Collider2D>();
            MyAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (_currentState != null)
                _currentState.InitializeState(this);
        }

        public void ChangeState(State NewState)
        {
            if (_currentState == null)
                return;

            _currentState.ExitState(this);
            _currentState = NewState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            if (_currentState == null)
                return;

            _currentState.Logic(this);
        }

        private void FixedUpdate()
        {
            if (_currentState == null)
                return;

            _currentState.Physics(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            
        }
    }
}