using System;
using UnityEngine;

namespace Joy
{
    [System.Serializable]
    public class FrameSettings : ScriptableObject
    {
        public bool IsHotUpdatePackage = false; //是否需要检查更新
        public string ResCDN = "https://a.unity.cn/client_api/v1/buckets/c53717b9-8894-4f43-88d0-518b8fb96241/release_by_badge/V1/content/";
        public string ResTokenCDN = "";
        public string ResPackageName = "Main";
    }
}