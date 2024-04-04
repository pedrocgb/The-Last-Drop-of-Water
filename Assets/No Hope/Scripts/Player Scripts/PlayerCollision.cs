using NoHope.RunTime.PropsScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.PlayerScripts
{
    [RequireComponent(typeof(PlayerBase))]
    public class PlayerCollision : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase Base;

        private bool _canAttach = false;
        private bool _isAttached = false;
        private DragableObject _currentProp = null;

        private bool _canTeleport = false;
        private Portal _currentPortal = null;
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            Base = GetComponent<PlayerBase>();
        }

        private void Update()
        {
            DragableObjectInputHandler();
            PortalObjectInputHandler();
        }
        #endregion

        //-------------------------------------------------------------------


        #region Input Methods
        private void DragableObjectInputHandler()
        {
            if (Base.MyInput.GetButtonDown("Interact") && _isAttached)
            {
                Debug.Log("Dettach Interaction");
                _currentProp.DettachFromPlayer();
                _isAttached = false;
            }
            else if (Base.MyInput.GetButtonDown("Interact") && _canAttach && !_isAttached)
            {
                Debug.Log("Attach Interaction");
                _currentProp.AttachToPlayer();
                _isAttached = true;
            }
        }

        private void PortalObjectInputHandler()
        {
            if (Base.MyInput.GetButtonDown("Interact") && _canTeleport)
            {
                _currentPortal.Teleport();

                _canTeleport = false;
                _currentPortal = null;
            }
        }
        #endregion

        //-------------------------------------------------------------------

        #region Collision Methods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Dragable Trigger") && !_isAttached)
            {
                _canAttach = true;
                _currentProp = collision.GetComponent<DragableObject>();
            }

            if (collision.CompareTag("Portal"))
            {
                _canTeleport = true;
                _currentPortal = collision.GetComponent<Portal>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Dragable Trigger") && !_isAttached)
            {
                _canAttach = false;
                _currentProp = null;
            }

            if (collision.CompareTag("Portal"))
            {
                _canTeleport = false;
                _currentPortal = null;
            }
        }
        #endregion

        //-------------------------------------------------------------------
    }
}