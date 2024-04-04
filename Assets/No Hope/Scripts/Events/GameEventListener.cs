using UnityEngine;
using UnityEngine.Events;

namespace NoHope.RunTime.Events
{
    /// <summary>
    /// Add this component to an object who will listen to a specific (or multiple) events.
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {

        private string eventName = string.Empty;

        [Space(20)]
        public GameEvents Event = null;

        public UnityEvent Response = null;


        /// <summary>
        /// Register the Event on enable.
        /// </summary>
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        /// <summary>
        /// Unregister the Event on disable.
        /// </summary>
        private void OnDisable()
        {
            Event.UnRegisterListener(this);
        }

        /// <summary>
        /// Invoke a simple Event.
        /// </summary>
        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}
