using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class VisibleNotifier : EventNotifier
    {
        public void OnBecameInvisible()
        {
            binder.proxy.OnBecameInvisible();
        }
        public void OnBecameVisible()
        {
            binder.proxy.OnBecameVisible();
        }
    }
}