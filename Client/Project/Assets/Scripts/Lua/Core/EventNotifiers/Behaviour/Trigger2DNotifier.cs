using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class Trigger2DNotifier : EventNotifier
    {
        public void OnTriggerEnter2D(Collider2D p)
        {
            binder.proxy.OnTriggerEnter2D(p);
        }
        public void OnTriggerStay2D(Collider2D p)
        {
            binder.proxy.OnTriggerStay2D(p);
        }
        public void OnTriggerExit2D(Collider2D p)
        {
            binder.proxy.OnTriggerExit2D(p);
        }
    }
}