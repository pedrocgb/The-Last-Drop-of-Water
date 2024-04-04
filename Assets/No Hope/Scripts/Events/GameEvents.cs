using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.Events
{
    /// <summary>
    /// Create an Event as a scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "New Event", menuName = "Event")]
    public class GameEvents : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        #region Generic Event
        /// <summary>
        /// Invoke this Event on all Listeners.
        /// </summary>
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }
        #endregion

        /// <summary>
        /// Register this Event on a Listener.
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        /// <summary>
        /// Unregister this Event on a Listener.
        /// </summary>
        /// <param name="listener"></param>
        public void UnRegisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}
