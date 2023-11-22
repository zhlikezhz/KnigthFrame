using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class GizmosNotifier : EventNotifier
    {
#if DebugMod
        public void OnDrawGizmos()
        {
            binder.proxy.OnDrawGizmos();
        }
        public void OnDrawGizmosSelected()
        {
            binder.proxy.OnDrawGizmosSelected();
        }
        public void OnGUI()
        {
            binder.proxy.OnGUI();
        }
#endif
    }
}