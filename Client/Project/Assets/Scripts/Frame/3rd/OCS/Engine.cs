using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Huge.OCS
{
    public class Engine
    {
        UInt32 m_iEID = 0;
        List<System> m_Systems = new List<System>();
        Dictionary<UInt32, List<Component>> m_dicEntity2Components = new Dictionary<UInt32, List<Component>>();

        UInt32 GenerateEID()
        {
            if (m_iEID < UInt32.MaxValue)
            {
                m_iEID++;
            }
            else
            {
                m_iEID = 1;
                Huge.Debug.LogError($"eid over UInt32.MaxValue: {m_iEID}");
            }
            m_dicEntity2Components[m_iEID] = new List<Component>();
            return m_iEID;
        }

        public UInt32 CreateEntity()
        {
            return GenerateEID();
        }

        public void DeleteEntity(UInt32 eid)
        {
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                m_dicEntity2Components.Remove(eid);
                foreach(var comp in compList)
                {
                    DeleteComponent(comp);
                }
            }
        }

        public T AddComponent<T>(UInt32 eid) where T : Component => AddComponent(eid, typeof(T)) as T;
        public Component AddComponent(UInt32 eid, Type type) => CreateComponent(eid, type);

        public bool HasComponent<T>(UInt32 eid) where T : Component => HasComponent(eid, typeof(T));
        public bool HasComponent(UInt32 eid, Type type)
        {
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                foreach(var comp in compList)
                {
                    if (type == comp.GetType())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public T GetComponent<T>(UInt32 eid) where T : Component => GetComponent(eid, typeof(T)) as T;
        public Component GetComponent(UInt32 eid, Type type)
        {
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                foreach(var comp in compList)
                {
                    if (type == comp.GetType())
                    {
                        return comp;
                    }
                }
            }
            return null;
        }

        public List<T> GetComponents<T>(UInt32 eid) where T : Component => GetComponents(eid, typeof(T)) as List<T>;
        public List<Component> GetComponents(UInt32 eid, Type type)
        {
            List<Component> comps = new List<Component>();
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                foreach(var comp in compList)
                {
                    if (type == comp.GetType())
                    {
                        comps.Add(comp);
                    }
                }
            }
            return comps;
        }

        public void GetComponents<T>(UInt32 eid, ref List<T> comps) where T : Component 
        {
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                foreach(var comp in compList)
                {
                    T t = comp as T;
                    if (t != null)
                    {
                        comps.Add(t);
                    }
                }
            }
        }

        public void GetComponents(UInt32 eid, Type type, ref List<Component> comps)
        {
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                foreach(var comp in compList)
                {
                    if (type == comp.GetType())
                    {
                        comps.Add(comp);
                    }
                }
            }
        }

        public void RemoveComponent(Component inst)
        {
            Huge.Debug.Assert(inst != null, "inst is null");
            if (inst == null) return;

            UInt32 eid = inst.EntityID;
            if (m_dicEntity2Components.TryGetValue(eid, out var compList))
            {
                if (compList.Remove(inst))
                {
                    DeleteComponent(inst);
                }
            }
        }

        public void RemoveComponentByType<T>(UInt32 eid) where T : Component => RemoveComponentByType(eid, typeof(T));
        public void RemoveComponentByType(UInt32 eid, Type type)
        {
            var comp = GetComponent(eid, type);
            RemoveComponent(comp);
        }

        public void RemoveComponentsByType<T>(UInt32 eid) where T : Component => RemoveComponentsByType(eid, typeof(T));
        public void RemoveComponentsByType(UInt32 eid, Type type)
        {
            List<Component> compList = GetComponents(eid, type);
            foreach(var comp in compList)
            {
                RemoveComponent(comp);
            }
        }

        Component CreateComponent(UInt32 eid, Type type)
        {
            Huge.Debug.Assert(type != null, "param type is null");
            Component comp = Activator.CreateInstance(type) as Component;
            if (comp != null)
            {
                comp.World = this;
                comp.EntityID = eid;
                if (m_dicEntity2Components.TryGetValue(eid, out var compList))
                {
                    compList.Add(comp);
                    try
                    {
                        comp.Start();
                    }
                    catch(Exception ex)
                    {
                        Huge.Debug.LogError($"component init error: {ex.ToString()}");
                    }
                }
                else
                {
                    Huge.Debug.LogError($"invaild eid");
                }
            }
            else
            {
                Huge.Debug.LogError("param type need inheirt component");
            }
            return comp;
        }

        void DeleteComponent(Component inst)
        {
            try
            {
                if (!inst.IsDestroied)
                {
                    inst.IsDestroied = true;
                    inst.OnDestroy();
                }
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"delete component fail: {ex.ToString()}");
            }
        }


        public T AddSystem<T>() where T : System
        {
            return AddSystem(typeof(T)) as T;
        }

        public System AddSystem(Type type)
        {
            Huge.Debug.Assert(type != null, "param type is null");
            System sys = Activator.CreateInstance(type) as System;
            if (sys != null)
            {
                m_Systems.Add(sys);
                sys.World = this;
                try
                {
                    sys.Start();
                }
                catch(Exception ex)
                {
                    Huge.Debug.LogError($"system init fail: {type.Name}.Start()\n{ex.ToString()}");
                }
            }
            else
            {
                Huge.Debug.LogError("param type need inheirt component");
            }
            return null;
        }

        public void RemoveSystem<T>() where T : System
        {
            RemoveSystem(typeof(T));
        }

        public void RemoveSystem(Type type)
        {
            foreach(System sys in m_Systems)
            {
                if (sys.GetType() == type)
                {
                    RemoveSystem(sys);
                    return;
                }
            }
        }

        public void RemoveSystems<T>() where T : System
        {
            RemoveSystems(typeof(T));
        }

        public void RemoveSystems(Type type)
        {
            List<System> sysList = new List<System>();
            foreach(System sys in m_Systems)
            {
                if (sys.GetType() == type)
                {
                    sysList.Add(sys);
                }
            }
            foreach(System sys in sysList)
            {
                RemoveSystem(sys);
            }
        }

        public void RemoveSystem(System inst)
        {
            Huge.Debug.Assert(inst != null, "param inst is null");
            if (inst == null) return;
            if (m_Systems.Remove(inst))
            {
                try
                {
                    inst.IsDestroied = true;
                    inst.OnDestroy();
                }
                catch(Exception ex)
                {
                    Huge.Debug.LogError($"system destroy fail: {inst.GetType().Name}.OnDestroy()\n{ex.ToString()}");
                }
            }
        }
    }
}
