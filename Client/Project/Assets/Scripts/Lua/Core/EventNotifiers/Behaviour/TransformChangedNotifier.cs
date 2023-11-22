using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class TransformChangedNotifier : EventNotifier
    {
        public void OnTransformChildrenChanged()
        {
            binder.proxy.OnTransformChildrenChanged();
        }
        public void OnTransformParentChanged()
        {
            binder.proxy.OnTransformParentChanged();
        }
        public void OnBeforeTransformParentChanged()
        {
            binder.proxy.OnBeforeTransformParentChanged();
        }
    }
}