using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.PropsScripts
{
    public class DragableObject : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase _player;

        [BoxGroup("Components"), SerializeField]
        private FixedJoint2D _fixedJoint = null;

        [BoxGroup("Components"), SerializeField]
        private Rigidbody2D _myRigidbody = null;
        public Rigidbody2D MyRigidbody { get { return _myRigidbody; } }
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            _player = FindObjectOfType<PlayerBase>();
        }
        #endregion

        //-------------------------------------------------------------------

        #region Attachment Methods
        public void AttachToPlayer()
        {
            Debug.Log("Attach");
            _fixedJoint.enabled = true;
            _fixedJoint.connectedBody = _player.MyRigidbody;
        }

        public void DettachFromPlayer()
        {
            Debug.Log("Dettach");
            _fixedJoint.enabled = true;
            _fixedJoint.connectedBody = null;
        }
        #endregion

        //-------------------------------------------------------------------
    }
}