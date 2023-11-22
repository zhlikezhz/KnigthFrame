using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class PointerEventNotifier : EventNotifier, 
        IPointerDownHandler, 
        IPointerUpHandler, 
        IPointerClickHandler, 
        IPointerEnterHandler, 
        IPointerExitHandler,
        IScrollHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            binder.proxy.OnPointerClick(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            binder.proxy.OnPointerDown(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            binder.proxy.OnPointerUp(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            binder.proxy.OnPointerEnter(eventData);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            GameDebug.Log("OnPointerExit:" + this.gameObject.name);
            binder.proxy.OnPointerExit(eventData);
        }
        public void OnScroll(PointerEventData eventData)
        {
            binder.proxy.OnScroll(eventData);
        }
        
    }
}
