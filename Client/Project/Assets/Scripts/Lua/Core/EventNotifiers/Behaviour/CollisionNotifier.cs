using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class CollisionNotifier : EventNotifier
    {
        public void OnCollisionEnter(Collision p)
        {
            binder.proxy.OnCollisionEnter(p);
        }
        public void OnCollisionStay(Collision p)
        {
            binder.proxy.OnCollisionStay(p);
        }
        public void OnCollisionExit(Collision p)
        {
            binder.proxy.OnCollisionExit(p);
        }
    }
}