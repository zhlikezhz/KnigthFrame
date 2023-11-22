
using UnityEngine;
using System.Collections.Generic;
using System;
namespace LuaFrame
{
    public class LateUpdateNotifier : EventNotifier
    {
        protected void LateUpdate()
        {
            binder.proxy.LateUpdate();
        }
    }
}