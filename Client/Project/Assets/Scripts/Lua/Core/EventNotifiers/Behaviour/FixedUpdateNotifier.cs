using UnityEngine;
using System.Collections.Generic;
using System;

namespace LuaFrame
{
    public class FixedUpdateNotifier : EventNotifier
    {
        protected void FixedUpdate()
        {
            binder.proxy.FixedUpdate();
        }
    }

}