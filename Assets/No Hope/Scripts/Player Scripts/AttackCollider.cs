using NoHope.RunTime.EnemyScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.PlayerScripts
{
    public class AttackCollider : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase MyPlayer;
        private float _damage;
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            MyPlayer = GetComponentInParent<PlayerBase>();
        }
        #endregion

        //-------------------------------------------------------------------

        public void SetDamage(float NewDamage)
        {
            _damage = NewDamage;
        }

        //-------------------------------------------------------------------

        #region Collision Methods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyBase enemy = collision.GetComponent<EnemyBase>();
                enemy.TakeDamage(_damage);
            }
        }
        #endregion

        //-------------------------------------------------------------------
    }
}