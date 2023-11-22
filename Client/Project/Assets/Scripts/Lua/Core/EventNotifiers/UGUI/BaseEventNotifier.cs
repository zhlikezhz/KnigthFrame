using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class BaseEventNotifier : EventNotifier, 
        ICancelHandler, 
        IDeselectHandler,
        ISelectHandler,
        ISubmitHandler,
        IUpdateSelectedHandler
    {
        public void OnCancel(BaseEventData eventData)
        {
            binder.proxy.OnCancel(eventData);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            binder.proxy.OnDeselect(eventData);
        }
        public void OnSelect(BaseEventData eventData)
        {
            binder.proxy.OnSelect(eventData);
        }
        public void OnSubmit(BaseEventData eventData)
        {
            binder.proxy.OnSubmit(eventData);
        }
        public void OnUpdateSelected(BaseEventData eventData)
        {
            binder.proxy.OnUpdateSelected(eventData);
        }
    }
}
