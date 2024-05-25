using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.AI
{
    public abstract class BaseState<T> where T : Enum
    {
        public T StateKey { get; private set; }
        public BaseState(T key)
        {
            StateKey = key;
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
        public abstract T GetNextState();


        public abstract void OnTriggerEnter(Collider2D c);
        public abstract void OnTriggerStay(Collider2D c);
        public abstract void OnTriggerExit(Collider2D c);
    }
}