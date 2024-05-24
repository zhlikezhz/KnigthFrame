using System;
using UnityEngine;

namespace YooAsset
{
    [CreateAssetMenu(fileName = "HotUpdateSetting", menuName = "YooAsset/Create HotUpdate Settings")]
    public class HotUpdateSetting : ScriptableObject
    {
        /// <summary>
        /// App ID
        /// </summary> 
        public string AppID = "";

        /// <summary>
        /// 不需要热更的Tags,用";"号分割tag
        /// </summary> 
        public string Tags = "";

        /// <summary>
        /// cdn development token
        /// </summary> 
        public string DevToken = "";

        /// <summary>
        /// cdn development bucket id
        /// </summary> 
        public string DevBucketID = "";

        /// <summary>
        /// cdn development project id
        /// </summary> 
        public string DevProjectID = "";

        /// <summary>
        /// cdn development enviroment id
        /// </summary>
        public string DevEnvironmentID = "";

        /// <summary>
        /// cnd production token
        /// </summary> 
        public string ProToken = "";

        /// <summary>
        /// cnd production bucket id
        /// </summary> 
        public string ProBucketID = "";

        /// <summary>
        /// cnd production project id
        /// </summary>
        public string ProProjectID = "";

        /// <summary>
        /// cnd production enviroment id
        /// </summary>
        public string ProEnvironmentID = "";
    }
}