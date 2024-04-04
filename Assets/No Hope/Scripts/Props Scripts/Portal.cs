using NoHope.RunTime.Controllers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.PropsScripts
{
    public class Portal : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase _player = null;

        [BoxGroup("Settings")]
        [SerializeField]
        protected Transform _destination = null;


        [BoxGroup("Components")]
        [SerializeField]
        protected SpriteRenderer _myRenderer = null;
        [BoxGroup("Components")]
        [SerializeField]
        protected Animator _myAnimator = null;
        [BoxGroup("Components")]
        [SerializeField]
        protected Collider2D _myCollider = null;
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            _player = FindObjectOfType<PlayerBase>();
        }
        #endregion

        //-------------------------------------------------------------------
        
        public virtual void Teleport()
        {
            CameraController.ChangeActiveCamera(CameraController.GetCamera("Boss Camera"));
            _player.transform.position = _destination.position;
        }

        //-------------------------------------------------------------------

        #region Collision Methods
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            
        }
        #endregion

        //-------------------------------------------------------------------
    }
}