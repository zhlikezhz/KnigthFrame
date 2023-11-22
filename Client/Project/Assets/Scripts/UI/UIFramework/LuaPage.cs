//using XLua;
//using UnityEngine;
//using System;

//public class LuaPage : Page
//{
//    LuaTable _target;
//    private Action<LuaTable> _luaAwake;
//    private Action<LuaTable> _luaRefresh;
//    private Action<LuaTable> _luaOnEnable;
//    private Action<LuaTable, bool> _luaHide;
//    private Action<LuaTable> _luaOnPushAni;

//    public LuaPage(LuaTable target, string uiPath, UIType type,UIAnim pop) :
//        base(uiPath,type, pop)
//    {
//        _target = target;
//        target.Get("Awake", out _luaAwake);
//        target.Get("Refresh", out _luaRefresh);
//        target.Get("Hide", out _luaHide);
//        target.Get("OnEnable", out _luaOnEnable);
//        target.Get("OnPushAni",out _luaOnPushAni);
//    }

//    public override void Awake(GameObject go)
//    {
//        if (_target == null)
//            return;

//        LuaBinding bind = go.GetComponent<LuaBinding>();
//        if (bind)
//        {
//            bind.Init(_target);
//        }
           
//        _luaAwake?.Invoke(_target);

//        OnEnable();
//    }

//    protected override void OnEnable()
//    {
//        base.OnEnable();
//        _luaOnEnable?.Invoke(_target);
//    }

//    public override void OnPushAnim()
//    {
//        base.OnPushAnim();
//        _luaOnPushAni?.Invoke(_target);
//    }

//    public override void Refresh(object data)
//    {
//        _luaRefresh?.Invoke(_target);
//    }

//    public override void Hide(bool isRemove)
//    {
//        _luaHide?.Invoke(_target, isRemove);
//        base.Hide(isRemove);
//    }

//    protected override void OnDestroy()
//    {
//        base.OnDestroy();
//        _luaAwake = null;
//        _luaRefresh = null;
//        _luaHide = null;
//        _target = null;
//    }
//}