using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

    public class ActiveBinding:MonoBehaviour
    {
    private Action<LuaTable> _OnEnable;
    private Action<LuaTable> _OnDisable;
    private Action<LuaTable> _OnDestroy;

    private LuaTable target;
  [SerializeField]  private string luaName;
    public void Init(LuaTable target)
    {
        this.target = target;
        target.Get("OnEnable", out _OnEnable);
        target.Get("OnDisable",out _OnDisable);
        target.Get("OnDestroy", out _OnDestroy);
        luaName = target.Get<string>("NAME");
    }

    private void OnEnable()
    {
            _OnEnable?.Invoke(this.target);
    }
    private void OnDisable()
    {
        _OnDisable?.Invoke(this.target);
    }

    private void OnDestroy()
    {
        _OnDestroy?.Invoke(this.target);
        Destroy();

    }

    private void Destroy()
    {
        this._OnEnable=null;
        this._OnDisable=null;
        this._OnDestroy = null;
        if (this.target != null)
        {
            this.target.Dispose();
        }
        this.target=null;
    }
}

