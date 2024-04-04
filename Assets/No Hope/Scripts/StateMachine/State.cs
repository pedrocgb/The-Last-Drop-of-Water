using Sirenix.OdinInspector;
using UnityEngine;

namespace NoHope.RunTime.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        protected bool IsCompleted { get; set; } = false;
        protected float _startTime = 0f;
        public float ElapsedTime { get => Time.time - _startTime; }

        public Rigidbody2D MyRigidbody { get; protected set; }
        public Animator MyAnimator { get; protected set; }
        public SpriteRenderer MySpriteRender { get; protected set; }

        public bool IsFacingRight { get; protected set; }
        public bool IsJumping { get; protected set; }
        public bool IsWallJumping { get; protected set; }
        public bool IsDashing { get; protected set; }
        public bool IsSliding { get; protected set; }
        public bool IsGrounded { get; protected set; }


        protected virtual void Start() { }

        protected virtual void EnterState() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
        protected virtual void ExitState() { }

    }
}