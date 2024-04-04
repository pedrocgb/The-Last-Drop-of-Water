using NoHope.RunTime.Events;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.PropsScripts
{
    public class TriggerButton : MonoBehaviour
    {
        #region Variables and Properties
        [BoxGroup("Effects")]
        [SerializeField]
        private GameEvents _triggerEvent = null;
        [BoxGroup("Effects")]
        [SerializeField]
        private GameEvents _untriggerEvent = null;
        private bool _isPressed = false;

        [BoxGroup("Components")]
        [SerializeField]
        private SpriteRenderer _myRenderer = null;
        #endregion

        //-------------------------------------------------------------------

        #region Trigger Methods
        public void TriggerEffect()
        {
            if (_isPressed)
                return;

            _triggerEvent.Raise();
            _myRenderer.color = Color.green;
            _isPressed = true;
        }
        public void UntriggerEffect()
        {
            if (!_isPressed)
                return;

            _untriggerEvent.Raise();
            _myRenderer.color = Color.red;
            _isPressed = false;
        }
        #endregion
        
        //-------------------------------------------------------------------
    }
}