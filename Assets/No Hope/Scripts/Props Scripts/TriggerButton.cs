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

        [BoxGroup("Collision")]
        [SerializeField]
        private LayerMask _overlapLayermask = 0;
        [BoxGroup("Collision")]
        [SerializeField]
        private float _overlapRadius = 5f;

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

            _isPressed = true;
            _triggerEvent.Raise();
            _myRenderer.color = Color.green;
        }
        public void UntriggerEffect()
        {
            if (!_isPressed)
                return;

            _isPressed = false;
            _untriggerEvent.Raise();
            _myRenderer.color = Color.red;
        }
        #endregion

        //-------------------------------------------------------------------

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") ||
                collision.CompareTag("Dragable Object"))
            {
                TriggerEffect();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") ||
                collision.CompareTag("Dragable Object"))
            {
                Debug.Log("Work");
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _overlapRadius, _overlapLayermask);
                Debug.Log(cols.Length);
                if (cols == null ||
                    cols.Length <= 0)
                    UntriggerEffect();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _overlapRadius);
        }
    }
}