using System;

namespace Huge.MVVM
{
    public sealed class ViewSettingAttribute : Attribute
    {
        public readonly Type Prefab;

        public ViewSettingAttribute(Type prefab)
        {
            Prefab = prefab;
        }
    }
}