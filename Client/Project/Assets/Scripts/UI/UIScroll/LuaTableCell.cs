using UnityEngine;
using XLua;
using System;
using LuaFrame;
using GameUtils;

[RequireComponent(typeof(LuaBinding))]
public class LuaTableCell : ScrollableCell
{
    LuaTable _target;
    private Action<LuaTable> _luaInit;
    private Action<LuaTable, int> _luaUpdate;
    private Action<LuaTable> _luaClean;

    public void Init(LuaTable cls)
    {
        Debug.Assert(cls != null, "LuaTableCell.Init target can't be null");
        try
        {

            if (LuaManager.getInstance().isBehaviourClass(cls))
            {
                _target = Tool.AddLuaBehaviourWithClass(this.gameObject, cls);

            }
            else
            {
                LuaFunction _new;
                cls.Get("new", out _new);
                _target = _new.Func<LuaTable, LuaTable>(cls);

                LuaBinding bind = GetComponent<LuaBinding>();
                if (bind)
                {
                    bind.Init(_target);
                }
                ActiveBinding activeBinding = gameObject.AddComponent<ActiveBinding>();
                activeBinding.Init(_target);

            }
            _target.Get("init", out _luaInit);
            _target.Get("updateData", out _luaUpdate);
            _target.Get("clean", out _luaClean);

        }
        catch (Exception e)
        {
            GameDebug.LogError(e.Message);
            GameDebug.LogError(e.StackTrace);
        }
        _luaInit?.Invoke(_target);
    }

    public override void ConfigureCellData()
    {
        //lua table begin 1
        _luaUpdate?.Invoke(_target, DataIndex + 1);
    }

    public override void CleanData()
    {
        _luaClean?.Invoke(_target);

        _luaInit = null;
        _luaUpdate = null;
        _luaClean = null;
        _target = null;
    }


}
