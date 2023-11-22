using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class DragEventNotifier : EventNotifier, 
        IInitializePotentialDragHandler, 
        IBeginDragHandler,
        IDragHandler,
        IDropHandler,
        IEndDragHandler
    {
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            binder.proxy.OnInitializePotentialDrag(eventData);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            binder.proxy.OnBeginDrag(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            binder.proxy.OnDrag(eventData);
        }
        public void OnDrop(PointerEventData eventData)
        {
            binder.proxy.OnDrop(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            binder.proxy.OnEndDrag(eventData);
        }
       
    }
}
