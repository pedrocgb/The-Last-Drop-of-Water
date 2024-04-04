using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Player Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class PlayerData : ScriptableObject
{
    #region Health
    [FoldoutGroup("Health Methods")]
    [SerializeField]
    private float _maxHealth = 100f;
    public float MaxHealth { get { return _maxHealth; } }
    #endregion

    //-------------------------------------------------------------------

    #region Gravity
    public float GravityStrength { get; private set; } //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
	public float GravityScale { get; private set; } //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
                                                                //Also the value the player's rigidbody2D.gravityScale is set to.
    [FoldoutGroup("Gravity Methods")]
    [SerializeField]
    private float _fallGravityMult = 1.5f;
	public float FallGravityMult { get { return _fallGravityMult; } } //Multiplier to the player's gravityScale when falling.
    [FoldoutGroup("Gravity Methods")]
    [SerializeField]
    private float _maxFallSpeed = 25f;
	public float MaxFallSpeed { get { return _maxFallSpeed; } } //Maximum fall speed (terminal velocity) of the player when falling.
    [FoldoutGroup("Gravity Methods")]
    [SerializeField]
    private float _fastFallGravityMult = 2f;
	public float FastFallGravityMult { get { return _fastFallGravityMult; } } //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
                                                                              //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    [FoldoutGroup("Gravity Methods")]
    [SerializeField]
    private float _maxFastFallSpeed = 30f;
	public float MaxFastFallSpeed { get { return _maxFastFallSpeed; } } //Maximum fall speed(terminal velocity) of the player when performing a faster fall.
    #endregion

    //-------------------------------------------------------------------

    #region Run
    public float RunAccelAmount { get; private set; } //The actual force (multiplied with speedDiff) applied to the player.
	public float RunDeccelAmount { get; private set; } //Actual force (multiplied with speedDiff) applied to the player .

	[FoldoutGroup("Run Methods")]
	[SerializeField]
    private float _runMaxSpeed = 11f;
	public float RunMaxSpeed { get { return _runMaxSpeed; } } //Target speed we want the player to reach.
    [FoldoutGroup("Run Methods")]
    [SerializeField]
    private float _runAcceleration = 2.5f;
    public float RunAcceleration { get { return _runAcceleration; } } //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [FoldoutGroup("Run Methods")]
    [SerializeField]
    private float _runDecceleration = 5f;
    public float RunDecceleration { get { return _runDecceleration; } } //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [FoldoutGroup("Run Methods")]
    [SerializeField]
    [Range(0f,1f)]
    private float _accelInAir = 0.65f;
	public float AccelInAir { get { return _accelInAir; } } //Multipliers applied to acceleration rate when airborne.
    [FoldoutGroup("Run Methods")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _deccelInAir = 0.65f;
    public float DeccelInAir { get { return _deccelInAir; } }
    [FoldoutGroup("Run Methods")]
    [SerializeField]
    private bool _doConserveMomentum = true;
	public bool DoConserveMomentum { get { return _doConserveMomentum; } }
    #endregion

    //-------------------------------------------------------------------

    #region Jump
    public float JumpForce { get; private set; } //The actual force applied (upwards) to the player when they jump.
    [FoldoutGroup("Jump Methods")]
    [SerializeField]
    private float _jumpHeight = 3.5f;
    public float JumpHeight { get { return _jumpHeight; } } //Height of the player's jump
    [FoldoutGroup("Jump Methods")]
    [SerializeField]
    private float _jumpTimeToApex = 0.3f;
    public float JumpTimeToApex { get { return _jumpTimeToApex; } } //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.

    [FoldoutGroup("Jump Methods/Double Jump")]
    [SerializeField]
    private float _jumpCutGravityMult = 2f;
    public float JumpCutGravityMult { get { return _jumpCutGravityMult; } } //Multiplier to increase gravity if the player releases thje jump button while still jumping
    [FoldoutGroup("Jump Methods/Double Jump")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _jumpHangGravityMult = 0.5f;
    public float JumpHangGravityMult { get { return _jumpHangGravityMult; } } //Reduces gravity while close to the apex (desired max height) of the jump
    [FoldoutGroup("Jump Methods/Double Jump")]
    [SerializeField]
    private float _jumpHangTimeThreshold = 1f;
    public float JumpHangTimeThreshold { get { return _jumpHangTimeThreshold; } } //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [FoldoutGroup("Jump Methods/Double Jump")]
    [SerializeField]
    private float _jumpHangAccelerationMult = 1.1f;
    public float JumpHangAccelerationMult { get { return _jumpHangAccelerationMult; } }
    [FoldoutGroup("Jump Methods/Double Jump")]
    [SerializeField]
    private float _jumpHangMaxSpeedMult = 1.3f;
    public float JumpHangMaxSpeedMult { get { return _jumpHangMaxSpeedMult; } }

    [FoldoutGroup("Jump Methods/Wall Jump")]
    [SerializeField]
    private Vector2 _wallJumpForce = new Vector2(15f, 25f);
    public Vector2 WallJumpForce { get { return _wallJumpForce; } } //The actual force (this time set by us) applied to the player when wall jumping.
    [FoldoutGroup("Jump Methods/Wall Jump")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _wallJumpRunLerp = 0.5f;
    public float WallJumpRunLerp { get { return _wallJumpRunLerp; } } //Reduces the effect of player's movement while wall jumping.
    [FoldoutGroup("Jump Methods/Wall Jump")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _wallJumpTime = 0.15f;
    public float WallJumpTime { get { return _wallJumpTime; } } //Time after wall jumping the player's movement is slowed for.
    [FoldoutGroup("Jump Methods/Wall Jump")]
    [SerializeField]
    private bool _doTurnOnWallJump = false;
    public bool DoTurnOnWallJump { get { return _doTurnOnWallJump; } } //Player will rotate to face wall jumping direction
    #endregion

    //-------------------------------------------------------------------

    #region Slide
    [FoldoutGroup("Slide Methods")]
    [SerializeField]
    private float _slideSpeed = 0f;
    public float SlideSpeed { get { return _slideSpeed; } }
    [FoldoutGroup("Slide Methods")]
    [SerializeField]
    private float _slideAccel = 0f;
    public float SlideAccel { get { return _slideAccel; } }
    #endregion

    //-------------------------------------------------------------------

    #region Assist
    [FoldoutGroup("Assist Methods")]
    [SerializeField]
    [Range(0f, .5f)]
    private float _coyoteTime = 0.1f;
    public float CoyoteTime { get { return _coyoteTime; } } //Grace period after falling off a platform, where you can still jump
    [FoldoutGroup("Assist Methods")]
    [SerializeField]
    [Range(0f, .5f)]
    private float _jumpInputBufferTime = 0.1f;
    public float JumpInputBufferTime { get { return _jumpInputBufferTime; } } //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.
    #endregion

    //-------------------------------------------------------------------

    #region Dash
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private int _dashAmount = 1;
    public int DashAmount { get { return _dashAmount; } }
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private float _dashSpeed = 20f;
    public float DashSpeed { get { return _dashSpeed; } }
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private float _dashSleepTime = 0.05f;
    public float DashSleepTime { get { return _dashSleepTime; } } //Duration for which the game freezes when we press dash but before we read directional input and apply a force
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private float _dashAttackTime = 0.15f;
    public float DashAttackTime { get { return _dashAttackTime; } }
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private float _dashEndTime = 0.15f;
    public float DashEndTime { get { return _dashEndTime; } } //Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private Vector2 _dashEndSpeed = new Vector2(15f, 15f);
    public Vector2 DashEndSpeed { get { return _dashEndSpeed; } } //Slows down player, makes dash feel more responsive (used in Celeste)
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _dashEndRunLerp = .5f;
    public float DashEndRunLerp { get { return _dashEndRunLerp; } } //Slows the affect of player movement while dashing
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    private float _dashRefillTime = 0.1f;
    public float DashRefillTime { get { return _dashRefillTime; } }
    [FoldoutGroup("Dash Methods")]
    [SerializeField]
    [Range(0f, .5f)]
    private float _dashInputBufferTime = .1f;
	public float DashInputBufferTime { get { return _dashInputBufferTime; } }
    #endregion

    //-------------------------------------------------------------------

    #region Attack Data
    [FoldoutGroup("Attack Methods")]
    [SerializeField]
    private float _attackAnimationTimer = 0.75f;
	public float AttackAnimationTimer { get { return _attackAnimationTimer; } }
    [FoldoutGroup("Attack Methods")]
    [SerializeField]
    private List<AttackData> _attackDatas = new List<AttackData>();
    public List<AttackData> AttackDatas { get { return _attackDatas; } }

    [System.Serializable]
    public class AttackData
    {
        public float Damage;
    }
    
    #endregion

    //-------------------------------------------------------------------

    private void OnValidate()
    {
		//Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
		GravityStrength = -(2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);
		
		//Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
		GravityScale = GravityStrength / Physics2D.gravity.y;

		//Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
		RunAccelAmount = (50 * RunAcceleration) / RunMaxSpeed;
		RunDeccelAmount = (50 * RunDecceleration) / RunMaxSpeed;

		//Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
		JumpForce = Mathf.Abs(GravityStrength) * JumpTimeToApex;

		#region Variable Ranges
		_runAcceleration = Mathf.Clamp(RunAcceleration, 0.01f, RunMaxSpeed);
		_runDecceleration = Mathf.Clamp(RunDecceleration, 0.01f, RunMaxSpeed);
		#endregion
	}

    //-------------------------------------------------------------------
}