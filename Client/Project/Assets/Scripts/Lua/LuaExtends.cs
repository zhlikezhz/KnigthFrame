using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

namespace LuaFrame
{

    [LuaCallCSharp]
    // [ReflectionUse]
    public static class UnityEngineObjectExtention
    {
        public static bool IsNull(this UnityEngine.Object o)
        {
            return o == null;
        }
        /// <summary>
        /// Point of this is to change the pivot, without moving the object
        /// </summary>
        public static void SetPivot(this RectTransform rect, float x = 0.5f, float y = 0.5f)
        {
            if (rect == null) return;
            Vector2 pivot = new Vector2(x, y);
            Vector3 deltaPosition = rect.pivot - pivot;    // get change in pivot
            deltaPosition.Scale(rect.rect.size);           // apply sizing
            deltaPosition.Scale(rect.localScale);          // apply scaling
            deltaPosition = rect.transform.localRotation * deltaPosition; // apply rotation

            rect.pivot = pivot;                            // change the pivot
            rect.localPosition -= deltaPosition;           // reverse the position change
        }
    }
}

