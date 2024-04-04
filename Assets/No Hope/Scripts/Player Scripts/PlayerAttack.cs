using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoHope.RunTime.Utilities;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace NoHope.RunTime.PlayerScripts
{
    [RequireComponent(typeof(PlayerBase))]
    public class PlayerAttack : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase MyBase;
        private PlayerMovement MyMovement;

        private float _attackTimeStamp = 0f;

        [BoxGroup("Components")]
        [SerializeField]
        private AttackCollider[] _attackColliders;
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            MyBase = GetComponent<PlayerBase>();
            MyMovement = GetComponent<PlayerMovement>();
            UpdateAttackColliders();
        }

        void Update()
        {
            if (MyBase.MyInput.GetButtonDown("Attack") && MyMovement.IsGrounded)
            {
                ComboAttack();
            }

            CheckAttackAnimation();
        }
        #endregion

        //-------------------------------------------------------------------

        private void UpdateAttackColliders()
        {
            for (int i = 0; i < _attackColliders.Length; i++)
            {
                _attackColliders[i].SetDamage(MyBase.Data.AttackDatas[i].Damage);
            }
        }

        #region Attack Methods
        private void ComboAttack()
        {
            MyMovement.StopMovement();

            MyBase.MyAnimator.SetTrigger("attack");
            MyBase.ChangeState(GameEnums.PlayerState.Attacking);
            _attackTimeStamp = Time.time + MyBase.Data.AttackAnimationTimer;

            Debug.Log(MyBase.MyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        private void CheckAttackAnimation()
        {
            if (MyBase.CurrentState != GameEnums.PlayerState.Attacking)
                return;

            if (_attackTimeStamp <= Time.time)
            {
                MyBase.ChangeState(GameEnums.PlayerState.Idle);
            }
        }
        #endregion

        //-------------------------------------------------------------------
    }
}