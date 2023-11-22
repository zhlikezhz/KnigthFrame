using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLua;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LuaFrame
{
    public class LuaFuncCache : IDisposable
    {

        static private Dictionary<LuaClassFunc, FuncType> _luaFunc_to_actinType = new Dictionary<LuaClassFunc, FuncType>();

        private Dictionary<LuaClassFunc, Delegate> ActionFunc = new Dictionary<LuaClassFunc, Delegate>();



        static LuaFuncCache()
        {
            for (LuaClassFunc key = LuaClassFunc.initLuaBehaviour; key < LuaClassFunc.COUNT; key++)
            {
                var type = key.GetType();
                var fieldName = key.ToString();
                System.Reflection.FieldInfo fieldInfo = type.GetField(fieldName);
                var objs = fieldInfo.GetCustomAttributes(typeof(FuncAttribute), true);

                if (objs != null && objs.Length > 0)
                {
                    FuncAttribute notifier = null;
                    for (int i = 0; i < objs.Length; i++)
                    {
                        var item = objs[i];
                        if (item is FuncAttribute)
                        {
                            notifier = item as FuncAttribute;
                            break;
                        }
                    }

                    FuncType func = notifier.funcType;
                    _luaFunc_to_actinType.Add(key, func);
                }
            }
        }


        public LuaFuncCache(LuaTable eventFunctionsTable)
        {
            Init(eventFunctionsTable);
        }

        private static Delegate GetAction(LuaTable eventFunctionsTable, LuaClassFunc key)
        {
            FuncType funcType;
            if (_luaFunc_to_actinType.TryGetValue(key, out funcType))
            {
                switch (funcType)
                {
                    case FuncType.BehaviourInit:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, LuaBehaviourProxy, ScriptBehaviour, GameObject, Transform>>(key);
                        };
                    case FuncType.Common:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable>>(key);
                        };
                    case FuncType.BoolEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, bool>>(key);
                        }
                    case FuncType.IntEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, int>>(key);
                        }
                    case FuncType.FloatEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, float>>(key);
                        }
                    case FuncType.StringEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, string>>(key);
                        }
                    case FuncType.UIEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, PointerEventData>>(key);
                        }
                    case FuncType.BaseUIEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, BaseEventData>>(key);
                        }
                    case FuncType.CollisionEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, Collision>>(key);
                        }
                    case FuncType.Collision2DEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, Collision2D>>(key);
                        }
                    case FuncType.Collider2DEvent:
                        {
                            return eventFunctionsTable.Get<LuaClassFunc, Action<LuaTable, Collider2D>>(key);
                        }
                }
            }

            GameDebug.LogWarning("不支持lua 方法:" + key);
            return null;
        }


        public void Init(LuaTable eventFunctionsTable)
        {
            foreach (var kvp in eventFunctionsTable.GetKeys<LuaClassFunc>())
            {
                var func = GetAction(eventFunctionsTable, kvp);
                if (func != null)
                {
                    ActionFunc.Add(kvp, func);
                }
            }
        }
        public IEnumerable<LuaClassFunc> functions
        {
            get
            {

                return this.ActionFunc.Keys;
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable> action = @delegate as Action<LuaTable>;
                if (action != null)
                {
                    action.Invoke(self);
                }
            }
        }

        public void Call(LuaClassFunc luaClassFunc, LuaTable self, bool param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, bool> action = @delegate as Action<LuaTable, bool>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, int param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, int> action = @delegate as Action<LuaTable, int>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, float param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, float> action = @delegate as Action<LuaTable, float>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, string param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, string> action = @delegate as Action<LuaTable, string>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, PointerEventData param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, PointerEventData> action = @delegate as Action<LuaTable, PointerEventData>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, BaseEventData param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, BaseEventData> action = @delegate as Action<LuaTable, BaseEventData>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, Collision param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, Collision> action = @delegate as Action<LuaTable, Collision>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, Collision2D param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, Collision2D> action = @delegate as Action<LuaTable, Collision2D>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }

        public void Call(LuaClassFunc luaClassFunc, LuaTable self, Collider param)//Collision
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, Collider> action = @delegate as Action<LuaTable, Collider>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, Collider2D param)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, Collider2D> action = @delegate as Action<LuaTable, Collider2D>;
                if (action != null)
                {
                    action.Invoke(self, param);
                }
            }
        }
        public void Call(LuaClassFunc luaClassFunc, LuaTable self, LuaBehaviourProxy param1, ScriptBehaviour param2, GameObject param3, Transform param4)
        {
            Delegate @delegate = null;
            if (ActionFunc.TryGetValue(luaClassFunc, out @delegate))
            {
                Action<LuaTable, LuaBehaviourProxy, ScriptBehaviour,  GameObject, Transform> action = @delegate as Action<LuaTable, LuaBehaviourProxy, ScriptBehaviour,  GameObject, Transform>;
                if (action != null)
                {
                    action.Invoke(self, param1, param2, param3, param4);
                }
            }
        }

        public void Dispose()
        {
            if (this.ActionFunc != null)
            {
                this.ActionFunc.Clear();
            }
            this.ActionFunc = null;
        }
    }
}
