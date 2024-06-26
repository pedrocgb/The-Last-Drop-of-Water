using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.Events
{
    public class GameEventPlayer : MonoBehaviour
    {
        #region Variables and Properties
        [BoxGroup("Events")]
        [SerializeField]
        private List<EventsToPlay> _events = new List<EventsToPlay>();

        [System.Serializable]
        public class EventsToPlay
        {
            public int Id;
            public string Name;
            public GameEvents Event;
        }
        #endregion

        //-------------------------------------------------------------------

        public GameEvents GetEventByName(string EventName)
        {
            if (_events == null ||
                _events.Count <= 0)
                return null;

            for (int i = 0; i < _events.Count; i++)
            {
                if (_events[i].Name == EventName)
                    return _events[i].Event;
            }

            return null;
        }

        public GameEvents GetEventById(int Id)
        {
            if (_events == null ||
                _events.Count <= 0)
                return null;

            for (int i = 0; i < _events.Count; i++)
            {
                if (_events[i].Id == Id)
                    return _events[i].Event;
            }

            return null;
        }

        //-------------------------------------------------------------------
    }
}