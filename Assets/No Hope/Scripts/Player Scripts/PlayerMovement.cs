using Rewired;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using NoHope.RunTime.Utilities;

namespace NoHope.RunTime.PlayerScripts
{
	[RequireComponent(typeof(PlayerBase))]
	public class PlayerMovement : MonoBehaviour
	{
		#region Variables and Properties
		private PlayerBase Base;

		public bool IsFacingRight { get; private set; }
		public bool IsJumping { get; private set; }
		public bool IsWallJumping { get; private set; }
		public bool IsDashing { get; private set; }
		public bool IsSliding { get; private set; }
		public bool IsGrounded { get; private set; }

		public float LastOnGroundTime { get; private set; }
		public float LastOnWallTime { get; private set; }
		public float LastOnWallRightTime { get; private set; }
		public float LastOnWallLeftTime { get; private set; }

		//Jump
		private bool _isJumpCut;
		private bool _isJumpFalling;

		//Wall Jump
		private float _wallJumpStartTime;
		private int _lastWallJumpDir;

		//Dash
		private int _dashesLeft;
		private bool _dashRefilling;
		private Vector2 _lastDashDir;
		private bool _isDashAttacking;

		private Vector2 _moveInput;

		public float LastPressedJumpTime { get; private set; }
		public float LastPressedDashTime { get; private set; }

		[BoxGroup("Ground Checks")]
		[SerializeField]
		private Transform _groundCheckPoint;
        [BoxGroup("Ground Checks")]
        [SerializeField]
		private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
        [BoxGroup("Ground Checks")]
        [SerializeField]
		private Transform _frontWallCheckPoint;
        [BoxGroup("Ground Checks")]
        [SerializeField]
		private Transform _backWallCheckPoint;
        [BoxGroup("Ground Checks")]
        [SerializeField]
		private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

        [BoxGroup("Layers and Tags")]
        [SerializeField]
		private LayerMask _groundLayer;
		#endregion

		// -------------------------------------------

		#region Unity Methods
		private void Awake()
		{
			Base = GetComponent<PlayerBase>();
		}

		private void Start()
		{
			SetGravityScale(Base.MyRigidbody.gravityScale);
			IsFacingRight = true;
		}

		private void Update()
		{
			LastOnGroundTime -= Time.deltaTime;
			LastOnWallTime -= Time.deltaTime;
			LastOnWallRightTime -= Time.deltaTime;
			LastOnWallLeftTime -= Time.deltaTime;

			LastPressedJumpTime -= Time.deltaTime;
			LastPressedDashTime -= Time.deltaTime;

			InputHandler();

			CollisionCheck();

			CheckJump();

			DashCheck();

			GravityCalculations();
		}

		private void FixedUpdate()
		{
			if (Base.CurrentState == GameEnums.PlayerState.Attacking)
				return;

			//Handle Run
			if (!IsDashing)
			{
				if (IsWallJumping)
					Run(Base.Data.WallJumpRunLerp);
				else
					Run(1);
			}
			else if (_isDashAttacking)
			{
				Run(Base.Data.DashEndRunLerp);
			}

			//Handle Slide
			if (IsSliding)
				Slide();
		}
		#endregion

		// -------------------------------------------

		#region Input Methods
		private void InputHandler()
		{
			if (Base.CurrentState == GameEnums.PlayerState.Attacking ||
				Base.IsDead)
				return;

			_moveInput.x = Base.MyInput.GetAxisRaw("Horizontal Axis");
			_moveInput.y = Base.MyInput.GetAxisRaw("Vertical Axis");

			if (_moveInput.x != 0)
			{
				CheckDirectionToFace(_moveInput.x > 0);
				Base.MyAnimator.SetBool("isMoving", true);
			}

			if (Base.MyInput.GetButtonDown("Jump"))
			{
				OnJumpInput();
            }

			if (Base.MyInput.GetButtonUp("Jump"))
			{
				OnJumpUpInput();
			}

			if (Base.MyInput.GetButtonDown("Dash"))
			{
				OnDashInput();
            }
		}

		public void OnJumpInput()
		{
			LastPressedJumpTime = Base.Data.JumpInputBufferTime;
		}

		public void OnJumpUpInput()
		{
			if (CanJumpCut() || CanWallJumpCut())
				_isJumpCut = true;
		}

		public void OnDashInput()
		{
			LastPressedDashTime = Base.Data.DashInputBufferTime;
		}
		#endregion

		#region Gravity Methods
		public void SetGravityScale(float scale)
		{
            Base.MyRigidbody.gravityScale = scale;
		}

		private void GravityCalculations()
		{
			if (!_isDashAttacking)
			{
				//Higher gravity if we've released the jump input or are falling
				if (IsSliding)
				{
					SetGravityScale(0);
				}
				else if (Base.MyRigidbody.velocity.y < 0 && _moveInput.y < 0)
				{
					//Much higher gravity if holding down
					SetGravityScale(Base.Data.GravityScale * Base.Data.FastFallGravityMult);
                    //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                    Base.MyRigidbody.velocity = new Vector2(Base.MyRigidbody.velocity.x, Mathf.Max(Base.MyRigidbody.velocity.y, -Base.Data.MaxFastFallSpeed));
				}
				else if (_isJumpCut)
				{
					//Higher gravity if jump button released
					SetGravityScale(Base.Data.GravityScale * Base.Data.JumpCutGravityMult);
                    Base.MyRigidbody.velocity = new Vector2(Base.MyRigidbody.velocity.x, Mathf.Max(Base.MyRigidbody.velocity.y, -Base.Data.MaxFallSpeed));
				}
				else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(Base.MyRigidbody.velocity.y) < Base.Data.JumpHangTimeThreshold)
				{
					SetGravityScale(Base.Data.GravityScale * Base.Data.JumpHangGravityMult);
				}
				else if (Base.MyRigidbody.velocity.y < 0)
				{
					//Higher gravity if falling
					SetGravityScale(Base.Data.GravityScale * Base.Data.FallGravityMult);
                    //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                    Base.MyRigidbody.velocity = new Vector2(Base.MyRigidbody.velocity.x, Mathf.Max(Base.MyRigidbody.velocity.y, -Base.Data.MaxFallSpeed));
				}
				else
				{
					//Default gravity if standing on a platform or moving upwards
					SetGravityScale(Base.Data.GravityScale);
				}
			}
			else
			{
				//No gravity when dashing (returns to normal once initial dashAttack phase over)
				SetGravityScale(0);
			}
		}
		#endregion

		#region Sleep Methods
		private void Sleep(float duration)
		{
			StartCoroutine(nameof(PerformSleep), duration);
		}

		private IEnumerator PerformSleep(float duration)
		{
			Time.timeScale = 0;
			yield return new WaitForSecondsRealtime(duration);
			Time.timeScale = 1;
		}
		#endregion

		#region Run Methods
		private void Run(float lerpAmount)
		{
			float targetSpeed = _moveInput.x * Base.Data.RunMaxSpeed;
			targetSpeed = Mathf.Lerp(Base.MyRigidbody.velocity.x, targetSpeed, lerpAmount);

			float accelRate;

			if (LastOnGroundTime > 0)
				accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Base.Data.RunAccelAmount : Base.Data.RunDeccelAmount;
			else
				accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Base.Data.RunAccelAmount * Base.Data.AccelInAir : Base.Data.RunDeccelAmount * Base.Data.DeccelInAir;


			if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(Base.MyRigidbody.velocity.y) < Base.Data.JumpHangTimeThreshold)
			{
				accelRate *= Base.Data.JumpHangAccelerationMult;
				targetSpeed *= Base.Data.JumpHangMaxSpeedMult;
			}


			if (Base.Data.DoConserveMomentum && Mathf.Abs(Base.MyRigidbody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(Base.MyRigidbody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
			{
				accelRate = 0;
			}

			float speedDif = targetSpeed - Base.MyRigidbody.velocity.x;
			float movement = speedDif * accelRate;

            Base.MyRigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);

            Base.MyAnimator.SetBool("isMoving", false);
		}

		private void Turn()
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;

			IsFacingRight = !IsFacingRight;
		}

		public void StopMovement()
		{
			Base.MyRigidbody.velocity = Vector2.zero;
		}
		#endregion

		#region Jump Methods
		private void CheckJump()
		{
			if (IsJumping && Base.MyRigidbody.velocity.y < 0)
			{
				IsJumping = false;

				_isJumpFalling = true;
                //Base.MyAnimator.SetBool("isFalling", _isJumpFalling);
            }

			if (IsWallJumping && Time.time - _wallJumpStartTime > Base.Data.WallJumpTime)
			{
				IsWallJumping = false;
			}

			if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
			{
				_isJumpCut = false;

				_isJumpFalling = false;
                //Base.MyAnimator.SetBool("isFalling", _isJumpFalling);
            }

			if (!IsDashing)
			{
				if (CanJump() && LastPressedJumpTime > 0)
				{
					IsJumping = true;
					IsWallJumping = false;
					_isJumpCut = false;
					_isJumpFalling = false;
					Jump();
				}
				//else if (CanWallJump() && LastPressedJumpTime > 0)
				//{
				//	IsWallJumping = true;
				//	IsJumping = false;
				//	_isJumpCut = false;
				//	_isJumpFalling = false;

				//	_wallJumpStartTime = Time.time;
				//	_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

				//	WallJump(_lastWallJumpDir);
				//}
			}

		}
		private void Jump()
		{
			LastPressedJumpTime = 0;
			LastOnGroundTime = 0;

			float force = Base.Data.JumpForce;
			if (Base.MyRigidbody.velocity.y < 0)
				force -= Base.MyRigidbody.velocity.y;

            Base.MyRigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            //Base.MyAnimator.SetTrigger("jump");
            IsGrounded = false;
            //Base.MyAnimator.SetBool("isGrounded", false);
        }
		private void WallJump(int dir)
		{
			LastPressedJumpTime = 0;
			LastOnGroundTime = 0;
			LastOnWallRightTime = 0;
			LastOnWallLeftTime = 0;

			Vector2 force = new Vector2(Base.Data.WallJumpForce.x, Base.Data.WallJumpForce.y);
			force.x *= dir;

			if (Mathf.Sign(Base.MyRigidbody.velocity.x) != Mathf.Sign(force.x))
				force.x -= Base.MyRigidbody.velocity.x;

			if (Base.MyRigidbody.velocity.y < 0)
				force.y -= Base.MyRigidbody.velocity.y;

            Base.MyRigidbody.AddForce(force, ForceMode2D.Impulse);
		}
		#endregion

		#region Dash Methods
		private void DashCheck()
		{
			if (CanDash() && LastPressedDashTime > 0)
			{
				Sleep(Base.Data.DashSleepTime);

				if (_moveInput != Vector2.zero)
					_lastDashDir = _moveInput;
				else
					_lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

				IsDashing = true;
				IsJumping = false;
				IsWallJumping = false;
				_isJumpCut = false;

				StartCoroutine(nameof(StartDash), _lastDashDir);

				//Base.MyAnimator.SetTrigger("dash");
			}
		}

		private IEnumerator StartDash(Vector2 dir)
		{

			LastOnGroundTime = 0;
			LastPressedDashTime = 0;

			float startTime = Time.time;

			_dashesLeft--;
			_isDashAttacking = true;

			SetGravityScale(0);

			while (Time.time - startTime <= Base.Data.DashAttackTime)
			{
                Base.MyRigidbody.velocity = dir.normalized * Base.Data.DashSpeed;
				yield return null;
			}

			startTime = Time.time;

			_isDashAttacking = false;

			SetGravityScale(Base.Data.GravityScale);
            Base.MyRigidbody.velocity = Base.Data.DashEndSpeed * dir.normalized;

			while (Time.time - startTime <= Base.Data.DashEndTime)
			{
				yield return null;
			}

			IsDashing = false;
		}

		private IEnumerator RefillDash(int amount)
		{
			_dashRefilling = true;
			yield return new WaitForSeconds(Base.Data.DashRefillTime);
			_dashRefilling = false;
			_dashesLeft = Mathf.Min(Base.Data.DashAmount, _dashesLeft + 1);
		}
		#endregion

		#region Slide Methods
		private void SlideCheck()
		{
			if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
				IsSliding = true;
			else
				IsSliding = false;
		}

		private void Slide()
		{
			if (Base.MyRigidbody.velocity.y > 0)
			{
                Base.MyRigidbody.AddForce(-Base.MyRigidbody.velocity.y * Vector2.up, ForceMode2D.Impulse);
			}

			float speedDif = Base.Data.SlideSpeed - Base.MyRigidbody.velocity.y;
			float movement = speedDif * Base.Data.SlideAccel;
			movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

            Base.MyRigidbody.AddForce(movement * Vector2.up);
		}
		#endregion

		#region Check Methods
		private void CollisionCheck()
		{
			if (!IsDashing && !IsJumping)
			{
				if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
				{
					LastOnGroundTime = Base.Data.CoyoteTime;
					IsGrounded = true;
                    //Base.MyAnimator.SetBool("isGrounded",true);
				}

				//if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
				//		|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
				//	LastOnWallRightTime = Base.Data.CoyoteTime;

				//if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
				//	|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
				//	LastOnWallLeftTime = Base.Data.CoyoteTime;

				//LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
			}
		}
		public void CheckDirectionToFace(bool isMovingRight)
		{
			if (isMovingRight != IsFacingRight)
				Turn();
		}

		private bool CanJump()
		{
			return LastOnGroundTime > 0 && !IsJumping;
		}

		private bool CanWallJump()
		{
			return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
				 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
		}

		private bool CanJumpCut()
		{
			return IsJumping && Base.MyRigidbody.velocity.y > 0;
		}

		private bool CanWallJumpCut()
		{
			return IsWallJumping && Base.MyRigidbody.velocity.y > 0;
		}

		private bool CanDash()
		{
			if (!IsDashing && _dashesLeft < Base.Data.DashAmount && LastOnGroundTime > 0 && !_dashRefilling)
			{
				StartCoroutine(nameof(RefillDash), 1);
			}

			return _dashesLeft > 0;
		}

		public bool CanSlide()
		{
			if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
				return true;
			else
				return false;
		}
		#endregion

		#region Editor Methods
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
			Gizmos.color = Color.blue;
			//Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
			//Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
		}
		#endregion
	}
}