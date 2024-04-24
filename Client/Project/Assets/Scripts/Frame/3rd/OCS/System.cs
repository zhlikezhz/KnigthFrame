using System;
using UnityEngine;

namespace Huge.OCS
{
    public class System
    {
        internal Engine World {get; set;}
        bool m_bIsDestroied = false;
        internal bool IsDestroied { get {return m_bIsDestroied;} set {m_bIsDestroied = value;} }

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
