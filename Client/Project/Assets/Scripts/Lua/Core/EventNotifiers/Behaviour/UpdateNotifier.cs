using UnityEngine;
using System.Collections.Generic;
using System;
namespace LuaFrame
{

    public class UpdateNotifier :EventNotifier
    {
        protected void Update()
        {
            binder.proxy.Update();
        }
    }

}