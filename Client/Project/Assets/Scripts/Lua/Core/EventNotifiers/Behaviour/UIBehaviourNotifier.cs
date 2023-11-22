using UnityEngine;
using System.Collections.Generic;
using System;

namespace LuaFrame
{
    public class UIBehaviourNotifier : EventNotifier
    {
        protected void OnCanvasGroupChanged()
        {
            binder.proxy.OnCanvasGroupChanged();
        }
        protected void OnCanvasHierarchyChanged()
        {
            binder.proxy.OnCanvasHierarchyChanged();
        }
        protected void OnDidApplyAnimationProperties()
        {
            binder.proxy.OnDidApplyAnimationProperties();
        }
        protected void OnRectTransformDimensionsChange()
        {
            binder.proxy.OnRectTransformDimensionsChange();
        }
    }

}