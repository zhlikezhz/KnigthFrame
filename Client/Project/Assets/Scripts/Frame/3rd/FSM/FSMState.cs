using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huge.FSM
{
    public abstract class FSMState
    {
        public virtual void OnInit(FSMContent content)
        {

        }

        public virtual void OnEnter(FSMContent content)
        {

        }

        public virtual void OnUpdate(FSMContent content)
        {

        }

        public virtual void OnLeave(FSMContent content)
        {

        }

        public virtual void OnDestroy(FSMContent content)
        { 

        }
    }
}
