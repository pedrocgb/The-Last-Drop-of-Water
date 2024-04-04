using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.Utilities
{
    public class GameEnums
    {
        public enum PlayerState
        {
            Idle,
            Moving,
            Jumping,
            Dashing,
            Attacking
        }

        public enum MovementAi
        {
            Horizontal,
            Vertical,
            Complex
        }
    }
}