using Rewired;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using NoHope.RunTime.Utilities;
using NoHope.RunTime.Controllers;
using UnityEngine.SceneManagement;

public class PlayerBase : MonoBehaviour, IDamageable
{
    #region Variables and Properties
    [BoxGroup("Data")]
    [SerializeField]
    private PlayerData _myData = null;
    public PlayerData Data { get { return _myData; } }
    [BoxGroup("Data")]
    [SerializeField]
    private PlayerUIController _myUi = null;

    [BoxGroup("Player Health")]
    [SerializeField]
    [ReadOnly]
    private float _currentHealth;
    public float HealthPercentage { get { return _currentHealth / Data.MaxHealth; } }
    private bool _isDead = false;
    public bool IsDead { get { return _isDead; } }

    [BoxGroup("Player State")]
    [SerializeField]
    private GameEnums.PlayerState _currentState;
    public GameEnums.PlayerState CurrentState { get { return _currentState; } }

    [BoxGroup("Components")]
    [SerializeField]
    private Rigidbody2D _myRigidBody = null;
    public Rigidbody2D MyRigidbody { get { return _myRigidBody; } }
    [BoxGroup("Components")]
    [SerializeField]
    private Animator _myAnimator = null;
    public Animator MyAnimator { get { return _myAnimator; } }
    [BoxGroup("Components")]
    [SerializeField]
    private SpriteRenderer _myGraphics = null;
    public SpriteRenderer MyGraphics { get { return _myGraphics; } }

    public Player MyInput { get; private set; }
    #endregion

    //-------------------------------------------------------------------

    #region Unity Methods
    private void Start()
    {
        MyInput = ReInput.players.GetPlayer(0);
        _currentHealth = Data.MaxHealth;
    }
    #endregion

    //-------------------------------------------------------------------

    public void ChangeState(GameEnums.PlayerState NewState)
    {
        _currentState = NewState;
    }

    #region Damage Methods
    public void TakeDamage(float Damage)
    {
        if (IsDead)
            return;

        StartCoroutine(DamageAnimation());
        _currentHealth -= Damage;

        _myUi.UpdateHealthBar(HealthPercentage);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageAnimation()
    {
        MyAnimator.SetLayerWeight(1, 1);
        yield return new WaitForSeconds(.66f);
        MyAnimator.SetLayerWeight(1, 0);
    }

    public void Die()
    {
        StartCoroutine(EDie());
    }

    private IEnumerator EDie()
    {
        _isDead = true;
        MyRigidbody.bodyType = RigidbodyType2D.Static;
        MyAnimator.SetLayerWeight(1, 0);
        _myAnimator.SetTrigger("die");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("Tutorial Level");
    }
    #endregion

    //-------------------------------------------------------------------
}
