using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLua;
namespace LuaFrame
{
    internal class LuaLooper : MonoBehaviour
    {
        internal const float GCInterval = 1;
        private float lastGCTime = 0;
        private LuaEnv luaEnv;

        Action m_update;
        Action m_fixedUpdate;
        Action m_lateUpdate;
        Action<int> m_mouseDown;
        Action<bool> m_OnApplicationPauseCallBack;
        Action m_OnApplicationQuit;
        Action<string> _luaChatCallback;

        SafeQueue<string> chatMsgQueue = new SafeQueue<string>();
        private void Start()
        {
            luaEnv = LuaManager.getInstance().luaEnv;

            luaEnv.Global.Get("Update", out m_update);           
            luaEnv.Global.Get("FixedUpdate", out m_fixedUpdate);           
            luaEnv.Global.Get("LateUpdate", out m_lateUpdate);
            m_mouseDown = luaEnv.Global.GetInPath<Action<int>>("App.OnMouseButtonDown");
            m_OnApplicationPauseCallBack = luaEnv.Global.GetInPath<Action<bool>>("App.OnApplicationPauseCallBack");
            m_OnApplicationQuit = luaEnv.Global.GetInPath<Action>("App.OnApplicationQuit");
            _luaChatCallback = luaEnv.Global.GetInPath<Action<string>>("ChatUtil.ChatCallback");


            Debug.Assert(m_update != null, "no find lua function :Update");
            Debug.Assert(m_fixedUpdate != null, "no find lua function :FixedUpdate");
            Debug.Assert(m_lateUpdate != null, "no find lua function :LateUpdate");
            Debug.Assert(m_mouseDown != null, "no find lua function :App.OnMouseButtonDown");
            Debug.Assert(m_OnApplicationPauseCallBack != null, "no find lua function :App.OnApplicationPauseCallBack");
            Debug.Assert(m_OnApplicationQuit != null, "no find lua function :App.OnApplicationQuit");
            Debug.Assert(_luaChatCallback != null, "no find lua function :ChatUtil.ChatCallback");
        }

        private void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                luaEnv?.Tick();
                lastGCTime = Time.time;
            }
            if (Input.GetMouseButtonDown(0))
            {
                m_mouseDown?.Invoke(0);
            }
            m_update?.Invoke();
           
        }

        private void FixedUpdate()
        {
            m_fixedUpdate?.Invoke();
           
        }

        private void LateUpdate()
        {
            m_lateUpdate?.Invoke();
            if (chatMsgQueue.Count > 0)
            {
                var msg = chatMsgQueue.Dequeue();
                _luaChatCallback?.Invoke(msg);
            }
        }
        void OnApplicationPause(bool pause)
        {
            m_OnApplicationPauseCallBack?.Invoke(pause);
        }

        private void OnApplicationQuit()
        {
            m_OnApplicationQuit?.Invoke();
#if UNITY_EDITOR
            GameDebug.Quit();
#endif
            AssetMgr.Dispose();
        }




        public void ChatCallback(string msg)
        {

            if (msg.Contains("setMessageReceivedStatus"))
            {
                return;
            }
          //  GameDebug.LogGreen("消息进栈 ChatCallback:" + msg);
            chatMsgQueue.Enqueue(msg);
        }

        public void ClearChatMsg()
        {
#if AssetDebug
            GameDebug.LogRed("清空融云消息队列");
#endif
            chatMsgQueue.Clear();
        } 


    }
}
