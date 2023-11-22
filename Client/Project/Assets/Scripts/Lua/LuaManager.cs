
using System.Collections.Generic;
using LuaFrame;
using UnityEngine;

namespace XLua
{
    public class LuaManager
    {
        private static LuaManager _luaMrg = null;
        public static LuaManager getInstance()
        {
            if (_luaMrg == null)
            {
                _luaMrg = Singleton.GetInstance<LuaManager>();
            }
            return _luaMrg;
        }


        public LuaEnv luaEnv;

        /// <summary>
        /// 返回两个函数：table，和可迭代的事件函数表
        /// </summary>
        const string lua_func__loadBehaviourClass = "load_behaviour_class";

        const string lua_func__loadMiddleclassClass = "load_middleclass_class";
        const string lua_func_analyze_behaviour_class = "analyze_behaviour_class";
        LuaFunction _loadBehaviourClass;
        LuaFunction _loadMiddleclassClass;
        LuaFunction _analyzeMiddleclassClass;
        Dictionary<string, LuaBehaviourClassCache> luaClasses = new Dictionary<string, LuaBehaviourClassCache>();


        private void Init()
        {
            luaEnv = new LuaEnv();
#if LoadFromLocal && UNITY_EDITOR
            bool isLuaLocal = true;
#else
            bool isLuaLocal = !Setting.Get().isLuaZip;
#endif
            if (!isLuaLocal)
                GameDebug.LogYellow("从AssetBundle加载代码");
            else
                GameDebug.LogYellow("从lua本地加载代码");
            LuaHelper.isLocalFile = isLuaLocal;
            luaEnv.AddLoader(LuaHelper.DoFile);
            //   LuaTimer.init();
        }

        public void StartMain()
        {
#if AssetDebug
            var t = GameUtils.Tool.CurrentTimeMilliseconds();
#endif
            Init();
            luaEnv.AddBuildin("rapidjson", LuaDLL.Lua.LoadRapidJson);
            luaEnv.AddBuildin("lpeg", LuaDLL.Lua.LoadLpeg);
            luaEnv.AddBuildin("pb", LuaDLL.Lua.LoadLuaProfobuf);
            luaEnv.AddBuildin("ffi", LuaDLL.Lua.LoadFFI);
            luaEnv.DoString("require 'common.main'");

            GameController.Instance.gameObject.AddComponent<LuaLooper>();
#if AssetDebug
            t = GameUtils.Tool.CurrentTimeMilliseconds() - t;
            GameDebug.LogRed(string.Format("[c#][LuaManager]StartMain  总共花费{0}ms.", t));
#endif

        }

        public bool isLuaStart()
        {
            return luaEnv != null;
        }





        public bool isArabic()
        {
            return InvokeLuaFunction<bool>("Localization.isArabic");
        }

        public LuaFunction GetLuaFunction(string fn)
        {
            if (luaEnv == null) return null;
            LuaFunction func = luaEnv.Global.GetInPath<LuaFunction>(fn);
            if (func == null)
            {
                GameDebug.LogError(" no find lua function with path:" + fn);
            }
            return func;
        }

        public void CallLuaFunction(string fn, string args)
        {
            if (luaEnv == null) return;
            LuaFunction func = luaEnv.Global.GetInPath<LuaFunction>(fn);
            if (func != null)
            {
                func.Action(args);
                func.Dispose();
            }
        }


        public T InvokeLuaFunction<T>(string fn)
        {
            if (luaEnv == null) return default(T);
            LuaFunction func = luaEnv.Global.GetInPath<LuaFunction>(fn);
            if (func != null)
            {
                T res = func.Func<T>();
                func.Dispose();
                return res;
            }
            return default(T);
        }

        public void CallLuaFunction(string fn, bool args)
        {
            if (luaEnv == null) return;
            LuaFunction func = luaEnv.Global.GetInPath<LuaFunction>(fn);
            if (func != null)
            {
                func.Action(args);
                func.Dispose();
            }
        }



        public bool isBehaviourClass(string luaName)
        {
            object[] result;
            return loadLuaBehaviourClass(luaName, out result);
        }

        public bool isBehaviourClass(LuaTable luaClass)
        {
            object[] result;
            return analyzeLuaBehaviourClass(luaClass, out result);

        }

        private bool loadLuaBehaviourClass(string className, out object[] rets)
        {

            try
            {
                if (_loadBehaviourClass == null)
                {
                    _loadBehaviourClass = GetLuaFunction(lua_func__loadBehaviourClass);
                }

                if (_loadBehaviourClass == null)
                {
                    throw new System.NullReferenceException("no find lua funciton:" + lua_func__loadBehaviourClass);
                }

                rets = _loadBehaviourClass.Call(className);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                GameDebug.Log(ex.ToString());
                rets = new object[] { false, -2.0f, "Lua文件未找到或者编译错误！" };
            }
            return (bool)rets[0];
        }

        private bool analyzeLuaBehaviourClass(LuaTable luaClass, out object[] rets)
        {
            try
            {
                if (_analyzeMiddleclassClass == null)
                {
                    _analyzeMiddleclassClass = GetLuaFunction(lua_func_analyze_behaviour_class);

                }
                if (_analyzeMiddleclassClass == null)
                {
                    throw new System.NullReferenceException("no find lua function:" + lua_func_analyze_behaviour_class);
                }
                rets = _analyzeMiddleclassClass.Call(luaClass);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                GameDebug.Log(ex.ToString());
                rets = new object[] { false, -2.0f, "Lua文件未找到或者编译错误！" };
            }
            return (bool)rets[0];
        }

        public LuaBehaviourClassCache LoadLuaBehaviourClass(string luaName)
        {
            if (string.IsNullOrEmpty(luaName))
            {
                return null;
            }
            LuaBehaviourClassCache clazz = null;
            if (!luaClasses.TryGetValue(luaName, out clazz))
            {
                object[] result;
                if (loadLuaBehaviourClass(luaName, out result))
                {
                    var table = result[1] as LuaTable;
                    var desc = result[2] as LuaTable;

                    clazz = new LuaBehaviourClassCache(luaName, table, desc);
                   // GameDebug.LogRed("缓存类:" + luaName);
                    luaClasses.Add(luaName, clazz);
                }
                else
                {
                    //尝试分析错误
                    int error = -1;
                    string msg = "";
                    object errorObj = result[1];
                    if (errorObj is double)
                    {
                        double errorf = (double)errorObj;
                        error = (int)errorf;
                    }
                    if (errorObj is float)
                    {
                        float errorf = (float)errorObj;
                        error = (int)errorf;
                    }
                    if (errorObj is int)
                    {
                        error = (int)errorObj;
                    }
                    if (result.Length > 2)
                    {
                        msg = result[2] as string;
                    }
                    GameDebug.LogError(string.Format("LuaManager 加载LuaClass[{0}]错误:{1}   错误码：{2}", luaName, msg, error));
                    clazz = new LuaBehaviourClassCache(luaName, error, msg);
                    luaClasses.Add(luaName, clazz);
                }
            }
            return clazz;
        }
        public LuaBehaviourClassCache LoadLuaBehaviourClass(LuaTable luaobj)
        {
            LuaTable luaClass = luaobj.Get<LuaTable>("class");
            string luaName = luaClass.Get<string>("_file_path");
            if (string.IsNullOrEmpty(luaName))
            {

                GameDebug.LogRed("lua class 其他异常:");
                foreach (var key in luaobj.GetKeys<string>())
                {
                    GameDebug.LogRed(key + luaobj.Get<string>(key));
                }


                return null;
            }

            LuaBehaviourClassCache clazz = null;
            if (!luaClasses.TryGetValue(luaName, out clazz))
            {
                object[] result;
                if (analyzeLuaBehaviourClass(luaClass, out result))
                {
                    var table = result[1] as LuaTable;
                    var desc = result[2] as LuaTable;
                    luaName = table.Get<string>("_file_path");
                    clazz = new LuaBehaviourClassCache(luaName, table, desc);
                  //  GameDebug.LogRed("缓存类:" + luaName);
                    luaClasses.Add(luaName, clazz);
                }
                else
                {
                    //尝试分析错误
                    int error = -1;
                    string msg = "";
                    object errorObj = result[1];
                    if (errorObj is double)
                    {
                        double errorf = (double)errorObj;
                        error = (int)errorf;
                    }
                    if (errorObj is float)
                    {
                        float errorf = (float)errorObj;
                        error = (int)errorf;
                    }
                    if (errorObj is int)
                    {
                        error = (int)errorObj;
                    }
                    if (result.Length > 2)
                    {
                        msg = result[2] as string;
                    }
                    GameDebug.LogError(string.Format("LuaManager 加载LuaClass[{0}]错误:{1}   错误码：{2}", luaName, msg, error));
                    clazz = new LuaBehaviourClassCache(luaName, error, msg);
                    luaClasses.Add(luaName, clazz);
                }
            }
            return clazz;
        }
        public bool CheckLuaStateValid(LuaEnv state)
        {
            return state != null && luaEnv == state;
        }


        /// <summary>
        /// 获取或者加载一个Lua类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public LuaTable RequireClass(string className)
        {
            object[] result = _loadMiddleclassClass.Call(className);
            if (result.Length == 0)
            {
                GameDebug.Log("LuaManager RequireClass error: no class table return! class:" + className);
                return null;
            }
            return result[0] as LuaTable;
        }



        public void ClearLuaClasses()
        {
            foreach (var kvp in luaClasses)
            {
                kvp.Value.Dispose();
            }
            luaClasses.Clear();
        }

        public void Close()
        {
            if (_loadBehaviourClass != null)
            {
                _loadBehaviourClass.Dispose();
            }
            _loadBehaviourClass = null;
            if (_loadMiddleclassClass != null)
            {
                _loadMiddleclassClass.Dispose();
            }
            _loadMiddleclassClass = null;
            if (_analyzeMiddleclassClass != null)
            {
                _analyzeMiddleclassClass.Dispose();
            }
            _analyzeMiddleclassClass = null;
            ClearLuaClasses();
            if (luaEnv != null)
            {
                luaEnv.Dispose();
            }
            luaEnv = null;

        }
    }
}


