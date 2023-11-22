using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class Collision2DNotifier : EventNotifier
    {
        public void OnCollisionEnter2D(Collision2D p)
        {
            binder.proxy.OnCollisionEnter2D(p);
        }
        public void OnCollisionStay2D(Collision2D p)
        {
            binder.proxy.OnCollisionStay2D(p);
        }
        public void OnCollisionExit2D(Collision2D p)
        {
            binder.proxy.OnCollisionExit2D(p);
        }
    }
}