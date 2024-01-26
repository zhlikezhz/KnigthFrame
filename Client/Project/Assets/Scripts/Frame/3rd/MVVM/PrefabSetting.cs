using System;

namespace Huge.MVVM
{
    public class PrefabSettingAttribute : System.Attribute
    {
        public readonly string PrefabPath;

        public PrefabSettingAttribute(string prefabPath)
        {
            PrefabPath = prefabPath;
        }
    }
}