using System.Collections;
using UnityEngine;
using XLua;
namespace LuaFrame
{


    [LuaCallCSharp]
    public class ScriptBehaviour : MonoBehaviour
    {

        public string scriptName;

       

        [BlackList]
        public LuaTable Table
        {
            get
            {
                return proxy.luaInstance;
            }
            set
            {
                proxy.luaInstance = value;
            }

        }


        protected LuaBehaviourProxy _proxy;

        public virtual LuaBehaviourProxy proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new LuaBehaviourProxy(this);
                }
                return _proxy;
            }
        }

        public int ReleaseFrame
        {
            get
            {
                return 0;
            }
        }

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(scriptName))
            {
              //  GameDebug.LogWarning("can not notify Awake to lua,because scriptName is empty with gameobject name:" + this.gameObject.name);
                return;
            }
            proxy.Awake();
        }

        protected virtual void Start()
        {
            if (string.IsNullOrEmpty(scriptName))
            {
            //    GameDebug.LogWarning("can not notify Start to lua,because scriptName is empty with gameobject name:"+ this.gameObject.name);
                return;
            }

            proxy.Start();
          
        }

        protected virtual void OnEnable()
        {
            if (string.IsNullOrEmpty(scriptName))
            {
             //   GameDebug.LogWarning("can not notify Awake to lua,because scriptName is empty with gameobject name:" + this.gameObject.name);
                return;
            }
            proxy.OnEnable();
        }

        protected virtual void OnDisable()
        {
            proxy.OnDisable();
        }

        protected virtual void OnDestroy()
        {
            if (_proxy!=null)
            {
                _proxy.OnDestroy();

            }
            _proxy = null;
           
        }

    }
}