using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class MouseNotifiler : EventNotifier
    {
        public void OnMouseDown()
        {
            binder.proxy.OnMouseDown();
        }
        public void OnMouseDrag()
        {
            binder.proxy.OnMouseDrag();
        }
        public void OnMouseEnter()
        {
            binder.proxy.OnMouseEnter();
        }
        public void OnMouseExit()
        {
            binder.proxy.OnMouseExit();
        }
        public void OnMouseOver()
        {
            binder.proxy.OnMouseOver();
        }
        public void OnMouseUp()
        {
            binder.proxy.OnMouseUp();
        }
        public void OnMouseUpAsButton()
        {
            binder.proxy.OnMouseUpAsButton();
        }
    }
}
