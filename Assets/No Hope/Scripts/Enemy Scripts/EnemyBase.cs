using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoHope.RunTime.AI;

namespace NoHope.RunTime.EnemyScripts
{
    public class EnemyBase : MonoBehaviour, IDamageable, IAnimator
    {
        #region Variables and Properties
        private PlayerBase _player;

        [BoxGroup("Health")]
        [SerializeField]
        private bool _damageble = true;
        [BoxGroup("Health")]
        [SerializeField]
        [ShowIf("_damageble")]
        private float _maxHealth = 0;
        private float _currentHealth;

        private string _currentAnimState = string.Empty;

        [BoxGroup("Turn")]
        [SerializeField]
        private bool _facesPlayer = false;
        public bool FacesPlayer { get { return _facesPlayer; } set { _facesPlayer = value; } }
        [BoxGroup("Turn")]
        [SerializeField]
        [ShowIf("_facesPlayer")]
        private bool _facingLeft = false;
        public bool FacingLeft { get { return _facingLeft; } }

        [BoxGroup("Attack")]
        [SerializeField]
        private float _collisionDamage = 0;

        [BoxGroup("Components")]
        [SerializeField]
        private Rigidbody2D _myRigidbody = null;
        public Rigidbody2D MyRigidbody { get { return _myRigidbody; } }
        [BoxGroup("Components")]
        [SerializeField]
        private Animator _myAnimator = null;
        public Animator MyAnimator { get { return _myAnimator; } }
        [BoxGroup("Components")]
        [SerializeField]
        private SpriteRenderer _mySpriteRender = null;
        #endregion

        #region State Machine

        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Awake()
        {

        }
        protected virtual void Start()
        {
            _player = FindObjectOfType<PlayerBase>();
            _currentHealth = _maxHealth;
            if (_facesPlayer)
                _mySpriteRender.flipX = _facingLeft;

        }

        private void Update()
        {
            if (_facesPlayer)
                FlipSprite();
        }
        #endregion

        //-------------------------------------------------------------------

        #region Damage Methods
        public void TakeDamage(float Damage)
        {
            if (!_damageble)
                return;

            _currentHealth -= Damage;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }
        #endregion

        //-------------------------------------------------------------------

        public void PlayAnimation(string AnimationState)
        {
            if (_currentAnimState == AnimationState) return;

            MyAnimator.Play(AnimationState);
            _currentAnimState = AnimationState;
        }

        private void FlipSprite()
        {
            if (transform.position.x >= _player.transform.position.x && !_facingLeft)
            {
                _facingLeft = true;
                _mySpriteRender.flipX = true;
            }
            else if (transform.position.x < _player.transform.position.x && _facingLeft)
            {
                _facingLeft = false;
                _mySpriteRender.flipX = false;
            }
        }

        //-------------------------------------------------------------------

        #region Collision Methods
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("Test");
                _player.TakeDamage(_collisionDamage);  
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Test");
                _player.TakeDamage(_collisionDamage);
            }
        }
        #endregion

        //-------------------------------------------------------------------
    }
}