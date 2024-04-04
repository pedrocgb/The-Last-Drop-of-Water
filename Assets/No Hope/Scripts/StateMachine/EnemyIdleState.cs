using NoHope.RunTime.EnemyScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.StateMachine
{
    [RequireComponent(typeof(EnemyBase))]
    public class EnemyIdleState : State
    {
        #region Variables and Properties
        private EnemyBase _base;

        private const string IDLE_ANIMATION = "Idle";
        #endregion

        //-------------------------------------------------------------------

        #region State Methods
        protected override void Start()
        {
            if (_base == null) _base = GetComponent<EnemyBase>();
        }

        protected override void EnterState()
        {
            _base.PlayAnimation(IDLE_ANIMATION);
        }

        protected override void Update()
        {
            
        }

        protected override void FixedUpdate()
        {
            
        }

        protected override void ExitState()
        {
            
        }
        #endregion

        //-------------------------------------------------------------------
    }
}