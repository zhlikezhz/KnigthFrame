using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLua;

namespace LuaFrame
{
    /// <summary>
    /// LuaBehaviour类型缓存
    /// 为避免重复的Lua类型信息加载(函数，对象事件)，ULuaManager将会在第一次加载类型之后缓存Lua的类信息
    /// 
    /// </summary>
    public class LuaBehaviourClassCache : IDisposable
    {

        LuaFuncCache funcCache;


        /// <summary>
        /// LuaBehaviour构造函数
        /// </summary>
        Func<LuaTable,LuaTable> _new;

        /// <summary>
        /// lua层的classTable
        /// </summary>
        LuaTable _classTable;

        /// <summary>
        /// class name
        /// </summary>
        string _className;
        /// <summary>
        /// 标记当前cache是否已经被释放了
        /// （Lua虚拟机重启之后将释放所有已经加载LuaClassCache）
        /// </summary>
        bool disposed = false;

        public string className { get { return _className; } }
        /// <summary>
        /// 当前LuaClass对应的Lua表
        /// </summary>
        public LuaTable classTable { get { return _classTable; } }

   


        /// <summary>
        /// 加载失败的错误类型
        /// </summary>
        public readonly int errorCode = 0;
        /// <summary>
        /// 错误提示
        /// </summary>
        public readonly string errorMessage = null;

        public bool hasError { get { return errorCode != 0; } }
        /// <summary>
        /// 构造一个加载出错的ClassCache
        /// </summary>
        /// <param name="name"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public LuaBehaviourClassCache(string name, int errorCode, string errorMessage)
        {
            this._className = name;
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }



        /// <summary>
        /// 构造一个加载正常的Lua类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="table"></param>
        /// <param name="eventFunctions"></param>
        /// <param name="objectEvents"></param>
        public LuaBehaviourClassCache(string name, LuaTable table, LuaTable desc)
        {
            this._classTable = table;
            this._className = name;
  

            //GameDebug.Log("create LuaClass:" + name);


            table.Get("new", out _new);
            initEventFunctions(desc);


        }

        public IEnumerable<LuaClassFunc> functions { get { return funcCache.functions; } }

        /// <summary>
        /// 初始化事件LuaBehaviour事件函数缓存
        /// </summary>
        /// <param name="eventFunctions"></param>
        private void initEventFunctions(LuaTable eventFunctionsTable)
        {
            if (eventFunctionsTable == null)
            {
                return;
            }

            this.funcCache = new LuaFuncCache(eventFunctionsTable);
            eventFunctionsTable.Dispose();
        }

        /// <summary>
        /// 获取一个Lua函数
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public LuaFuncCache FuncCache
        {
            get
            {
                if (errorCode != 0)
                {
                    GameDebug.LogError("call a load error class:" + _className);
                    return null;
                }

                return this.funcCache;
            }
            
        }
        // public delegate void ArgsDelegate(LuaFunction func);
        /// <summary>
        /// 快速调用一个无返回值的事件函数，函数在LuaClass的时候已经缓存好。
        /// </summary>
        /// <param name="f"></param>
        /// <param name="self"></param>
        /// <param name="argsSetter"></param>
        public void CallFunc(LuaClassFunc f, LuaTable self)
        {
            if (errorCode != 0)
            {
                GameDebug.LogError("call a load error class:" + _className);
                return;
            }
            this.funcCache.Call(f, self);


        }


        public LuaTable New()
        {
            if (errorCode != 0)
            {
                GameDebug.LogError("Invok a load error class:" + _className);
                return null;
            }
            var func = _new;
            var table = func.Invoke(_classTable);
            return table;
        }
        public void Dispose()
        {
            if (_classTable != null)
            {
                _classTable.Dispose();
                _classTable = null;
            }
            if (funcCache != null)
            {
                funcCache.Dispose();
            }
            funcCache = null;
   
            _new = null;

            disposed = true;
            GameDebug.LogRed("释放lua类缓存:"+ className);
        }
        public bool IsDisposed()
        {
            return disposed;
        }
    }
}



