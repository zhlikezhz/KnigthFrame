using System;

namespace Huge.MVVM
{
    public sealed class ViewSettingAttribute : Attribute
    {
        public readonly string PrefabPath;

        public ViewSettingAttribute(string path)
        {
            PrefabPath = path;
        }
    }
}