using System;
using UnityEngine;

namespace Huge.OCS
{
    public class Component
    {
        bool m_bIsDestroied = false;
        internal bool IsDestroied { get {return m_bIsDestroied;} set {m_bIsDestroied = value;} }

        internal Engine World { get; set; }
        public UInt32 EntityID { get; internal set; }

        internal Component() { }
        public T AddComponent<T>() where T : Component => World.AddComponent<T>(EntityID);
        public Component AddComopnent(Type type) => World.AddComponent(EntityID, type);

        internal protected virtual void Start()
        {

        }

        internal protected virtual void OnDestroy()
        {

        }
    }
}
