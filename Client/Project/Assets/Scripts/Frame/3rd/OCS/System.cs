using System;
using HybridCLR.Editor.ABI;
using Unity.Android.Types;
using UnityEngine;

namespace Huge.OCS
{
    public class System
    {
        bool m_bIsDestroied = false;
        internal bool IsDestroied { get {return m_bIsDestroied;} set {m_bIsDestroied = value;} }
        internal Engine World {get; set;}

        public T AddSystem<T>() where T : System => World.AddSystem<T>();
        public System AddSystem(Type type) => World.AddSystem(type);

        internal virtual void Start()
        {

        }

        internal virtual void OnDestroy()
        {

        }
    }
}
