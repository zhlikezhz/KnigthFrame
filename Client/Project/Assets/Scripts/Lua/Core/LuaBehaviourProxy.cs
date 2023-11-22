using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;
using UnityEngine.UI;
using System.Diagnostics;

namespace LuaFrame
{
    [LuaCallCSharp]
    public class LuaBehaviourProxy
    {
        ScriptBehaviour _binder;

        LuaBinding _binding;

        public ScriptBehaviour binder
        {
            get
            {
                return _binder;
            }
        }


        public GameObject gameObject
        {
            get
            {
                return _binder.gameObject;
            }
        }
        public Transform transform
        {
            get
            {
                return _binder.transform;
            }
        }

        /// <summary>
        /// Lua脚本名字（类名）
        /// </summary>
        public string luaName
        {
            get
            {
                return _binder.scriptName;
            }
            set
            {
                _binder.scriptName = value;
            }
        }
        private bool isExcudeAwake;
        private bool isExcudeStart;

        public int BindCount
        {
            get
            {
                if (_binding != null)
                {
                    return _binding.mVariables.Length;
                }
                return 0;
            }
        }

        /// <summary>
        /// LuaBehaviour的类型缓存
        /// </summary>
        LuaBehaviourClassCache _luaClass;
        public LuaBehaviourClassCache luaClass
        {
            get
            {

                return _luaClass;
            }
        }


        /// <summary>
        /// LuaBehaviour实例
        /// </summary>
        LuaTable _luaInstance;
        public LuaTable luaInstance
        {
            get
            {
                checkLuaInstance();
                return _luaInstance;
            }
            set
            {
                _luaInstance = value;

                _luaClass = LuaManager.getInstance().LoadLuaBehaviourClass(value);
                luaName = _luaClass.className;
            }
        }
        static bool _WeakAssetEnabled = true;
        public static bool WeakAssetEnabled { get { return _WeakAssetEnabled; } internal set { _WeakAssetEnabled = value; } }

        public object GetScriptHandle()
        {
            return luaInstance;
        }


        /// <summary>
        /// 构造LuaBehaviour，可能会被多线程调用
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="luaName"></param>
        [BlackListAttribute]
        public LuaBehaviourProxy(ScriptBehaviour binder)
        {
            this._binder = binder;
            this._binding = this.gameObject.GetComponent<LuaBinding>();
        }
        /// <summary>
        /// Awake事件，创建Lua实例对象
        /// </summary>
        [BlackListAttribute]
        public virtual void Awake()
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            UnityEngine.Profiling.Profiler.BeginSample(binder.gameObject.name + ":Awake");
            checkLuaClass();
            if (!Application.isPlaying)
            {
                return;
            }
            if (_luaClass != null)
            {
                checkLuaInstance();

                if (isExcudeAwake) return;
                isExcudeAwake = true;

                UnityEngine.Profiling.Profiler.BeginSample("LuaAwake:" + luaName);
                forceDispacthEvent(LuaClassFunc.Awake);
                UnityEngine.Profiling.Profiler.EndSample();
            }
            UnityEngine.Profiling.Profiler.EndSample();
            //sw.Stop();
            //TimeSpan ts2 = sw.Elapsed;
            //GameDebug.Log(string.Format(" {0 } call Awake: 总共花费{1}ms.", this.luaName, ts2.TotalMilliseconds));

        }

        private void registObjectEvent()
        {

            if (this._binding != null)
            {
                this._binding.Init(this.luaInstance);

            }
        }

        private void initNotifier()
        {
            if (_luaClass != null)
            {
                foreach (LuaClassFunc func in _luaClass.functions)
                {
                    initNotifier(func);
                }
            }
        }
        private void initNotifier(LuaClassFunc func)
        {
            var type = func.GetType();
            var fieldName = func.ToString();
            System.Reflection.FieldInfo fieldInfo = type.GetField(fieldName);
            var objs = fieldInfo.GetCustomAttributes(typeof(NotifierAttribute), true);

            if (objs.Length > 0)
            {
                NotifierAttribute notifier = (NotifierAttribute)objs[0];
                Type notifierType = notifier.notifierType;
                var com = this.gameObject.GetComponent(notifierType) as EventNotifier;
                if (com == null)
                {
                    com = this.gameObject.AddComponent(notifierType) as EventNotifier;
                }
                com.binder = this.binder;
            }
        }

        public object GetBindProperty(int index, out string propertyName, out string propertyType)
        {
            propertyName = "";
            propertyType = "";
            int count = this.BindCount;
            if (index < 0 || index >= count)
            {
                string errorMsg = "index:" + index + " bindItems count:" + count + " binder:" + this._binding;

                throw new ArgumentOutOfRangeException(errorMsg);
            }


            var item = _binding.mVariables[index];
            propertyName = item.name;
            propertyType = item.type;
            if (propertyType == "ScriptBehaviour")
            {
                ScriptBehaviour target = item.val as ScriptBehaviour;
                if (target != null)
                {
                    return target.Table;
                }
            }
            if (propertyType == "ObjectArray")
            {
                ObjectArray list = (ObjectArray)item.val;
                // TODO 可能有对象到脚本层丢失的风险
                object[] result = new object[list.array.Length];
                LuaTable table = LuaManager.getInstance().luaEnv.NewTable();

                for (int i = 0; i < result.Length; i++)
                {
                    object objitem = list.array[i];
                    if (objitem == null)
                    {
                        string errorMsg = "index:" + index + "数组引用丢失=> 数组下标:" + i + "; binder:" + this._binding;

                        throw new ArgumentOutOfRangeException(errorMsg);
                    }
                    if (objitem is ScriptBehaviour)
                    {
                        ScriptBehaviour binderObj = (ScriptBehaviour)objitem;
                        objitem = ((LuaBehaviourProxy)binderObj.proxy).luaInstance;
                    }
                    // result[i] = objitem;
                    table.Set(i + 1, objitem);
                }
                return result;
            }
            return item.val;


        }

        private void checkLuaClass()
        {
            if (_luaClass == null)
            {
                loadClass();
            }
        }
        public bool IsAssetObject()
        {
            if (WeakAssetEnabled && binder)
            {
                return binder.gameObject.scene.rootCount == 0;
            }
            return false;
        }
        private void checkLuaInstance()
        {
            checkLuaClass();
            if (_luaInstance == null && luaClass != null)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Profiling.Profiler.BeginSample("LoadLuaInstance:" + luaName);
                    UnityEngine.Profiling.Profiler.BeginSample("LoadLuaInstance.New:" + luaName);
                    _luaInstance = _luaClass.New();
                    UnityEngine.Profiling.Profiler.EndSample();
                    UnityEngine.Profiling.Profiler.BeginSample("LoadLuaInstance.initLuaBehaviour:" + luaName);
                    initLuaBehaviour();
                    UnityEngine.Profiling.Profiler.EndSample();
                    UnityEngine.Profiling.Profiler.EndSample();

                }
            }
        }

        [BlackList]
        public void initLuaBehaviour()
        {
            if (_luaClass == null)
            {
                throw new Exception("lua class is null:" + this.luaName);
            }

            _luaClass.FuncCache.Call(LuaClassFunc.initLuaBehaviour, _luaInstance, this, _binder, _binder.gameObject, _binder.transform);

            // registObjectEvent();
        }

        [BlackList]
        public void loadClass()
        {
            if (string.IsNullOrEmpty(luaName))
            {
                _luaClass = null;
                return;
            }
            UnityEngine.Profiling.Profiler.BeginSample("LoadLuaClass:" + luaName);
            _luaClass = LuaManager.getInstance().LoadLuaBehaviourClass(luaName);
            UnityEngine.Profiling.Profiler.EndSample();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [BlackList]
        public ScriptBehaviour GetBinder()
        {
            return _binder;
        }



        /// <summary>
        /// 转发事件到luaInstance，只在LuaComponent enabled时候有效
        /// </summary>
        /// <param name="f"></param>
        /// <param name="argsSetter"></param>
        [BlackList]
        public void dispacthEvent(LuaClassFunc f)
        {
            if (_binder.enabled && _luaInstance != null)
            {
                _luaClass.CallFunc(f, _luaInstance);

            }
        }


        /// <summary>
        /// 强制转发事件，用于enabled无关的事件
        /// </summary>
        /// <param name="f"></param>
        /// <param name="argsSetter"></param>
        [BlackList]
        public void forceDispacthEvent(LuaClassFunc f)
        {
            if (_luaInstance != null)
            {
                _luaClass.CallFunc(f, _luaInstance);
            }
            else
            {
                // GameDebug.LogRed(string.Format("can not notify {0} to lua,because luaInstance is null with gameobject name:{1}", f, this.gameObject.name));

            }
        }
        #region 基础事件
        [BlackList]
        public void Start()
        {
            if (isExcudeStart) return;
            isExcudeStart = true;
            UnityEngine.Profiling.Profiler.BeginSample("LuaStart:" + luaName);
            forceDispacthEvent(LuaClassFunc.Start);
            UnityEngine.Profiling.Profiler.EndSample();
            initNotifier();
        }

        [BlackList]
        public void OnEnable()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Lua OnEnable:" + luaName);
            forceDispacthEvent(LuaClassFunc.OnEnable);
            UnityEngine.Profiling.Profiler.EndSample();
        }
        [BlackList]
        public void OnDisable()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Lua OnDisable:" + luaName);
            forceDispacthEvent(LuaClassFunc.OnDisable);
            UnityEngine.Profiling.Profiler.EndSample();
        }





        [BlackList]
        public void OnDestroy()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Lua OnDestroy:" + luaName);
            forceDispacthEvent(LuaClassFunc.OnDestroy);
            forceDispacthEvent(LuaClassFunc.OnDestroyed);
            if (_luaInstance != null)
            {
                _luaInstance.Dispose();
            }
            _luaInstance = null;
            _binder = null;
            _luaClass = null;
            _binding = null;
            UnityEngine.Profiling.Profiler.EndSample();
        }



        [BlackList]
        public void OnPooledGet(int idx)
        {
            UnityEngine.Profiling.Profiler.BeginSample("Lua OnPooledGet:" + luaName);
            _luaClass.FuncCache.Call(LuaClassFunc.OnPooledGet, luaInstance,idx);
            UnityEngine.Profiling.Profiler.EndSample();
        }



        #endregion

        #region Notifier事件
        [BlackList]
        public void Update()
        {

            _luaClass.FuncCache.Call(LuaClassFunc.Update, luaInstance);
        }

        [BlackList]
        public void LateUpdate()
        {
            _luaClass.FuncCache.Call(LuaClassFunc.LateUpdate, luaInstance);
  
        }

        [BlackList]
        public void FixedUpdate()
        {
            _luaClass.FuncCache.Call(LuaClassFunc.FixedUpdate, luaInstance);
        }


        [BlackList]
        public void OnPointerClick(PointerEventData eventData)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnPointerClick, luaInstance, eventData);
        }
        [BlackList]
        public void OnPointerDown(PointerEventData eventData)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnPointerDown, luaInstance, eventData);
        }
        [BlackList]
        public void OnPointerUp(PointerEventData eventData)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnPointerUp, luaInstance, eventData);
        }

        [BlackList]
        public void OnPointerEnter(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnPointerEnter, luaInstance, p);
           
        }
        [BlackList]
        public void OnPointerExit(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnPointerExit, luaInstance, p);
            
        }
        [BlackList]
        public void OnScroll(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnScroll, luaInstance, p);
            
        }
        [BlackList]
        public void OnInitializePotentialDrag(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnInitializePotentialDrag, luaInstance, p);
           
        }
        [BlackList]
        public void OnBeginDrag(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnBeginDrag, luaInstance, p);
            
        }
        [BlackList]
        public void OnDrag(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnDrag, luaInstance, p);
            
        }
        [BlackList]
        public void OnDrop(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnDrop, luaInstance, p);
           
        }
        [BlackList]
        public void OnEndDrag(PointerEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnEndDrag, luaInstance, p);
            
        }

        [BlackList]
        public void OnCancel(BaseEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCancel, luaInstance, p);
            
        }
        [BlackList]
        public void OnDeselect(BaseEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnDeselect, luaInstance, p);
            
        }
        [BlackList]
        public void OnSelect(BaseEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnSelect, luaInstance, p);
            
        }
        [BlackList]
        public void OnSubmit(BaseEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnSubmit, luaInstance, p);
            
        }
        [BlackList]
        public void OnUpdateSelected(BaseEventData p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnUpdateSelected, luaInstance, p);
            
        }

        [BlackList]
        public void OnMouseDown()
        {
            
            dispacthEvent(LuaClassFunc.OnMouseDown);
        }
        [BlackList]
        public void OnMouseDrag()
        {
            dispacthEvent(LuaClassFunc.OnMouseDrag);
        }
        [BlackList]
        public void OnMouseEnter()
        {
            dispacthEvent(LuaClassFunc.OnMouseEnter);
        }
        [BlackList]
        public void OnMouseExit()
        {
            dispacthEvent(LuaClassFunc.OnMouseExit);
        }
        [BlackList]
        public void OnMouseOver()
        {
            dispacthEvent(LuaClassFunc.OnMouseOver);
        }
        [BlackList]
        public void OnMouseUp()
        {
            dispacthEvent(LuaClassFunc.OnMouseUp);
        }
        [BlackList]
        public void OnMouseUpAsButton()
        {
            dispacthEvent(LuaClassFunc.OnMouseUpAsButton);
        }



        [BlackList]
        public void OnApplicationFocus(bool focus)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnApplicationFocus, luaInstance, focus);
        }
        [BlackList]
        public void OnApplicationPause(bool pause)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnApplicationPause, luaInstance, pause);
        }
        [BlackList]
        public void OnApplicationQuit()
        {
            dispacthEvent(LuaClassFunc.OnApplicationQuit);
        }
        

        [BlackList]
        public void OnCollisionEnter(Collision p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCollisionEnter, luaInstance, p);
            
        }
        [BlackList]
        public void OnCollisionExit(Collision p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCollisionExit, luaInstance, p);
           
        }
        [BlackList]
        public void OnCollisionStay(Collision p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCollisionStay, luaInstance, p);
            
        }
        [BlackList]
        public void OnTriggerEnter(Collider p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnTriggerEnter, luaInstance, p);
            
        }
        [BlackList]
        public void OnTriggerExit(Collider p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnTriggerExit, luaInstance, p);
            
        }
        [BlackList]
        public void OnTriggerStay(Collider p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnTriggerStay, luaInstance, p);
           
        }
        [BlackList]
        public void OnCollisionStay2D(Collision2D p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCollisionStay2D, luaInstance, p);
            
        }
        [BlackList]
        public void OnCollisionEnter2D(Collision2D p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCollisionEnter2D, luaInstance, p);
            
        }
        [BlackList]
        public void OnCollisionExit2D(Collision2D p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnCollisionExit2D, luaInstance, p);
            
        }
        [BlackList]
        public void OnTriggerEnter2D(Collider2D p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnTriggerEnter2D, luaInstance, p);
           
        }
        [BlackList]
        public void OnTriggerExit2D(Collider2D p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnTriggerExit2D, luaInstance, p);
            
        }
        [BlackList]
        public void OnTriggerStay2D(Collider2D p)
        {
            _luaClass.FuncCache.Call(LuaClassFunc.OnTriggerStay2D, luaInstance, p);
            
        }
        [BlackList]
        public void OnTransformChildrenChanged()
        {

            dispacthEvent(LuaClassFunc.OnTransformChildrenChanged);
        }
        [BlackList]
        public void OnTransformParentChanged()
        {

            dispacthEvent(LuaClassFunc.OnTransformParentChanged);
        }
        [BlackList]
        public void OnLevelWasLoaded()
        {

            dispacthEvent(LuaClassFunc.OnLevelWasLoaded);
        }
       
        [BlackList]
        public void OnBeforeTransformParentChanged()
        {
            dispacthEvent(LuaClassFunc.OnBeforeTransformParentChanged);
        }
        [BlackList]
        public void OnCanvasGroupChanged()
        {
            dispacthEvent(LuaClassFunc.OnCanvasGroupChanged);
        }
        [BlackList]
        public void OnCanvasHierarchyChanged()
        {
            dispacthEvent(LuaClassFunc.OnCanvasHierarchyChanged);
        }
        [BlackList]
        public void OnDidApplyAnimationProperties()
        {
            dispacthEvent(LuaClassFunc.OnDidApplyAnimationProperties);
        }
        [BlackList]
        public void OnRectTransformDimensionsChange()
        {
            dispacthEvent(LuaClassFunc.OnRectTransformDimensionsChange);
        }

        [BlackList]
        public void OnBecameInvisible()
        {
            dispacthEvent(LuaClassFunc.OnBecameInvisible);
        }
        [BlackList]
        public void OnBecameVisible()
        {
            dispacthEvent(LuaClassFunc.OnBecameVisible);
        }

      


        [BlackList]
        public void OnDrawGizmos()
        {
            dispacthEvent(LuaClassFunc.OnDrawGizmos);
        }
        [BlackList]
        public void OnDrawGizmosSelected()
        {
            dispacthEvent(LuaClassFunc.OnDrawGizmosSelected);
        }
        [BlackList]
        public void OnGUI()
        {
            dispacthEvent(LuaClassFunc.OnGUI);
        }
        

        #endregion

    }
}

