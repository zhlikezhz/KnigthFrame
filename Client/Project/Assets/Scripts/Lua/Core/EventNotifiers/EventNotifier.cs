using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LuaFrame
{
    public abstract class EventNotifier : MonoBehaviour
    {
        public ScriptBehaviour binder;
    }

}