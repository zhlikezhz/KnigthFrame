using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huge.HotFix
{
    [System.Serializable]
    public class TinkerConfig
    {
        public bool IsOpenHotFix = false;
        public string Version = "0.0.0.0"; //大版本.强更版本.热更版本.GIT版本
    }
}
