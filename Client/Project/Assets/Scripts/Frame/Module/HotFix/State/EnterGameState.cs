using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joy.FSM;

namespace Joy.HotFix
{
    public class EnterGameState : FSMState
    {
        public override void OnEnter(FSMContent content)
        {
            var tinker = content as TinkerManager;
            tinker.IsCompleted = true;
        }

        public override void OnLeave(FSMContent content)
        {
            base.OnLeave(content);
        }
    }
}
