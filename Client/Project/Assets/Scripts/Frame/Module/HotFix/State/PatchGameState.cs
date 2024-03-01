using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huge.FSM;
using JetBrains.Annotations;


namespace Huge.HotFix
{
    public class PatchGameState : FSMState
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
