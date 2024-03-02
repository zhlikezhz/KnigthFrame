using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using YooAsset;
using Huge;

namespace Huge.Asset
{
    public class AssetManager : Singleton<AssetManager>
    {
        public readonly static string c_strPackageName = "Main";

        Dictionary<string, AssetHandle> m_dicRefAssets = new Dictionary<string, AssetHandle>();
         
        internal async UniTask InitAsync()
        {
            YooAssets.Initialize();
            var package = YooAssets.CreatePackage(c_strPackageName);
            YooAssets.SetDefaultPackage(package);

#if UNITY_EDITOR
            var initParameters = new EditorSimulateModeParameters();
            var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, c_strPackageName);
            initParameters.SimulateManifestFilePath  = simulateManifestFilePath;
            await package.InitializeAsync(initParameters);
#else
            var initParameters = new OfflinePlayModeParameters();
            await package.InitializeAsync(initParameters);
#endif
        }

        public bool IsExists(string assetName)
        {
            var info = YooAssets.GetAssetInfo(assetName);
            if (string.IsNullOrEmpty(info.AssetPath))
            {
                return false;
            }
            return true;
        }

        public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            AssetHandle handler = YooAssets.LoadAssetSync<T>(assetName);
            return handler.AssetObject as T;
        }

        public UnityEngine.Object LoadAsset(string assetName, System.Type type)
        {
            return YooAssets.LoadAssetSync(assetName, type).AssetObject;
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
            AssetHandle handler = YooAssets.LoadAssetAsync<T>(assetName);
            await handler.ToUniTask();
            return handler.AssetObject as T;
        }

        public void LoadAssetAsync<T>(string assetName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            AssetHandle handler = YooAssets.LoadAssetAsync<T>(assetName);
            handler.Completed += (_handler) => {
                callback?.Invoke(_handler.AssetObject as T);
            };
        }

        public void LoadAssetAsync(string assetName, System.Type assetType, UnityAction<UnityEngine.Object> callback)
        {
            AssetHandle handler = YooAssets.LoadAssetAsync(assetName, assetType);
            handler.Completed += (_handler) => {
                callback?.Invoke(_handler.AssetObject);
            };
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            await YooAssets.LoadSceneAsync(sceneName);
        }

        public void LoadSceneAsync(string sceneName, UnityAction callback)
        {
            SceneHandle handler = YooAssets.LoadSceneAsync(sceneName);
            handler.Completed += (_handler) => {
                callback?.Invoke();
            };
        }

        public void UnloadAsset(string assetName)
        {

        }
    }
}
