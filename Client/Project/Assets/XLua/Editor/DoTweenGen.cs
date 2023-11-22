using System;
using System.Collections.Generic;
using XLua;

namespace XLuaExtension
{
    public static class DoTweenGen
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
        };
    }
}

