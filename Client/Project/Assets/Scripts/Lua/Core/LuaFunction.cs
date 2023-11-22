#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
using System;
#endif

namespace XLua
{
    public partial class LuaFunction
    {
        public void Action()
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
            var L = luaEnv.L;
            var translator = luaEnv.translator;
            int oldTop = LuaAPI.lua_gettop(L);
            int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
            LuaAPI.lua_getref(L, luaReference);
            int error = LuaAPI.lua_pcall(L, 0, 0, errFunc);
            if (error != 0)
                luaEnv.ThrowExceptionFromError(oldTop);
            LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }


        public void Action<T1, T2, T3>(T1 a1, T2 a2, T3 a3)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
            var L = luaEnv.L;
            var translator = luaEnv.translator;
            int oldTop = LuaAPI.lua_gettop(L);
            int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
            LuaAPI.lua_getref(L, luaReference);
            translator.PushByType(L, a1);
            translator.PushByType(L, a2);
            translator.PushByType(L, a3);
            int error = LuaAPI.lua_pcall(L, 3, 0, errFunc);
            if (error != 0)
                luaEnv.ThrowExceptionFromError(oldTop);
            LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }


        public void Action<T1, T2, T3, T4>(T1 a1, T2 a2, T3 a3, T4 a4)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
            var L = luaEnv.L;
            var translator = luaEnv.translator;
            int oldTop = LuaAPI.lua_gettop(L);
            int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
            LuaAPI.lua_getref(L, luaReference);
            translator.PushByType(L, a1);
            translator.PushByType(L, a2);
            translator.PushByType(L, a3);
            translator.PushByType(L, a4);
            int error = LuaAPI.lua_pcall(L, 4, 0, errFunc);
            if (error != 0)
                luaEnv.ThrowExceptionFromError(oldTop);
            LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }


        public void Action<T1, T2, T3, T4, T5>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
            var L = luaEnv.L;
            var translator = luaEnv.translator;
            int oldTop = LuaAPI.lua_gettop(L);
            int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
            LuaAPI.lua_getref(L, luaReference);
            translator.PushByType(L, a1);
            translator.PushByType(L, a2);
            translator.PushByType(L, a3);
            translator.PushByType(L, a4);
            translator.PushByType(L, a5);
            int error = LuaAPI.lua_pcall(L, 5, 0, errFunc);
            if (error != 0)
                luaEnv.ThrowExceptionFromError(oldTop);
            LuaAPI.lua_settop(L, oldTop);
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }


        public TResult Func<T1, T2, T3, TResult>(T1 a1, T2 a2, T3 a3)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
            var L = luaEnv.L;
            var translator = luaEnv.translator;
            int oldTop = LuaAPI.lua_gettop(L);
            int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
            LuaAPI.lua_getref(L, luaReference);
            translator.PushByType(L, a1);
            translator.PushByType(L, a2);
            translator.PushByType(L, a3);
            int error = LuaAPI.lua_pcall(L, 3, 1, errFunc);
            if (error != 0)
                luaEnv.ThrowExceptionFromError(oldTop);
            TResult ret;
            try
            {
                translator.Get(L, -1, out ret);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                LuaAPI.lua_settop(L, oldTop);
            }
            return ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }


        public TResult Func<T1, T2, T3,T4, TResult>(T1 a1, T2 a2, T3 a3,T4 a4)
        {
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
            var L = luaEnv.L;
            var translator = luaEnv.translator;
            int oldTop = LuaAPI.lua_gettop(L);
            int errFunc = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
            LuaAPI.lua_getref(L, luaReference);
            translator.PushByType(L, a1);
            translator.PushByType(L, a2);
            translator.PushByType(L, a3);
            translator.PushByType(L, a4);
            int error = LuaAPI.lua_pcall(L, 4, 1, errFunc);
            if (error != 0)
                luaEnv.ThrowExceptionFromError(oldTop);
            TResult ret;
            try
            {
                translator.Get(L, -1, out ret);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                LuaAPI.lua_settop(L, oldTop);
            }
            return ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
        }
    }
}
