using UnityEngine;
using System.Collections.Generic;
using System;

namespace LuaFrame
{
    public class ApplicationNotifier : EventNotifier
    {
        protected void OnApplicationFocus(bool fcs)
        {
            binder.proxy.OnApplicationFocus(fcs);
        }
        protected void OnApplicationPause(bool pause)
        {
            binder.proxy.OnApplicationPause(pause);
        }
        protected void OnApplicationQuit()
        {
            binder.proxy.OnApplicationQuit();
        }
    }

}