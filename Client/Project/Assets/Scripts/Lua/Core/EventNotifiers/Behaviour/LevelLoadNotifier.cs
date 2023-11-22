using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace LuaFrame
{
    public class LevelLoadNotifier : EventNotifier
    {
        public void OnLevelWasLoaded()
        {
            binder.proxy.OnLevelWasLoaded();
        }
    }
}