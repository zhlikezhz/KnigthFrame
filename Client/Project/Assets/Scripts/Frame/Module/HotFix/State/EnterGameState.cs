using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huge.FSM;

namespace Huge.HotFix
{
    public class EnterGameState : FSMState
    {
        public override void OnEnter(FSMContent content)
        {

        }

        public override void OnLeave(FSMContent content)
        {
            base.OnLeave(content);
        }
    }
}
