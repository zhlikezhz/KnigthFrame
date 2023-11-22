using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class TriggerNotifier : EventNotifier
    {
        public void OnTriggerEnter(Collider p)
        {
            binder.proxy.OnTriggerEnter(p);
        }
        public void OnTriggerStay(Collider p)
        {
            binder.proxy.OnTriggerStay(p);
        }
        public void OnTriggerExit(Collider p)
        {
            binder.proxy.OnTriggerExit(p);
        }
    }
}