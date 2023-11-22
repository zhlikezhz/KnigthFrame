using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using System.Collections;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace XLuaExtension
{
    public static class CustomGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(ArrayList),
            typeof(List<int>),
            typeof(List<Dropdown.OptionData>),
            typeof(Action<string>),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(UnityAction),
            typeof(UnityAction<bool>),
            typeof(Action),
            typeof(Action<string>),
            typeof(Action<bool,string>),
            //net
            typeof(Action<string,string>),
            typeof(Action<string,string,string>),
            typeof(Action<string,Sprite>),
            typeof(Action<int>),
            typeof(Action<int, byte[]>),
            typeof(Action<int,string,byte[]>),
            typeof(Action<byte[]>),
            typeof(Action<long>),
            typeof(Action<long,long>),
            typeof(Action<string[]>),
            typeof(Action<int>),
            typeof(Action<GameObject>),
            typeof(Action<Sprite>),
            typeof(Action<Texture>),
            typeof(Action<Material>),
            typeof(Action<AudioClip>),
             typeof(Action<GameObject,string>),
            typeof(Action<Sprite,string>),
            typeof(Action<Texture,string>),
            typeof(Action<Material,string>),
            typeof(Action<AudioClip,string>),
            typeof(Action<Transform>),
            //lua ui
            typeof(Action<LuaTable>),
            typeof(Action<LuaTable,bool>),
            typeof(Action<LuaTable,Button,string>),
            typeof(Action<LuaTable,Toggle,string,bool>),
            typeof(Action<LuaTable,InputField,string,string>),
            typeof(Action<LuaTable,int>),
            typeof(Action<LuaTable,float>),
            typeof(Func<LuaTable>),
            typeof(Action<LuaTable,string>),
            typeof(Action<LuaTable,PointerEventData>),
            typeof(Action<LuaTable,BaseEventData>),
            typeof(Action<LuaTable,Collision>),
            typeof(Action<LuaTable,Collision2D>),
            typeof(Action<LuaTable,Collider>),
            typeof(Action<LuaTable,Collider2D>),
            //lua timer
            typeof(Func<int, bool>),
            typeof(Func<int, RectTransform>),
            typeof(UnityAction<Vector2>),
        };
        //[GCOptimize]
        //public static List<Type> GCOptionList = new List<Type>
        //{
        //      typeof(ATRect),
        //};

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>(){ "NetExtension.WebSocket", "Update"},
            new List<string>(){ "NetExtension.WebSocket", "OnDestroy"},
            new List<string>() { "UnityEngine.Input", "IsJoystickPreconfigured", "System.String" },
            new List<string>(){ "UnityEngine.ParticleSystemRenderer", "supportsMeshInstancing" }
        };
    }
}
