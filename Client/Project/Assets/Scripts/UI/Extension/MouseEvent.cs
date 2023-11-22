using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class MouseEvent : MonoBehaviour
{
    private Action<LuaTable> _OnMouseDown;
    private Action<LuaTable> _OnMouseDrag;
    private Action<LuaTable> _OnMouseEnter;
    private Action<LuaTable> _OnMouseExit;
    private Action<LuaTable> _OnMouseOver;
    private Action<LuaTable> _OnMouseUp;
    private Action<LuaTable> _OnMouseUpAsButton;
    private LuaTable self;

    public void Bind(LuaTable table)
    {
        self = table;
        self.Get("OnMouseDown", out _OnMouseDown);
        self.Get("OnMouseDrag", out _OnMouseDrag);
        self.Get("OnMouseEnter", out _OnMouseEnter);
        self.Get("OnMouseExit", out _OnMouseExit);
        self.Get("OnMouseOver", out _OnMouseOver);
        self.Get("OnMouseUp", out _OnMouseUp);
        self.Get("OnMouseUpAsButton", out _OnMouseUpAsButton);
    }
    public void OnMouseDown()
    {
        GameDebug.Log("--->OnMouseDown");
        _OnMouseDown?.Invoke(self);
    }
    public void OnMouseDrag()
    {
        GameDebug.Log("--->OnMouseDrag");
        _OnMouseDrag?.Invoke(self);

    }
    public void OnMouseEnter()
    {
        _OnMouseEnter?.Invoke(self);
    }
    public void OnMouseExit()
    {
        _OnMouseExit?.Invoke(self);
    }
    public void OnMouseOver()
    {
        _OnMouseOver?.Invoke(self);
    }
    public void OnMouseUp()
    {
        _OnMouseUp?.Invoke(self);
    }
    public void OnMouseUpAsButton()
    {
        _OnMouseUpAsButton?.Invoke(self);

    }
    void OnDestroy()
    {
        self = null;
        _OnMouseDown = null;
        _OnMouseDrag = null;
        _OnMouseEnter = null;
        _OnMouseExit = null;
        _OnMouseOver = null;
        _OnMouseUp = null;
        _OnMouseUpAsButton = null;

    }
}

