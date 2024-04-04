using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NoHope.RunTime.PropsScripts
{
    public class Door : Portal
    {
        #region Variables and Properties
        private bool _isOpen = false;
        public bool IsOpen { get { return _isOpen; } }
        #endregion

        //-------------------------------------------------------------------

        #region Door Methods
        public void OpenDoor()
        {
            _isOpen = true;
            _myAnimator.Play("Open", 0, 0f);
            _myCollider.enabled = true;
        }

        public void CloseDoor()
        {
            _isOpen = false;
            _myAnimator.Play("Close", 0, 0f);
            _myCollider.enabled = false;
        }
        #endregion

        //-------------------------------------------------------------------

    }
}