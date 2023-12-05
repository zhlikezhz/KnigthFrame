using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

using Huge;

namespace Huge.Asset
{
    public class AssetManager : Singleton<AssetManager>
    {
        public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            return null;
        }

        public UnityEngine.Object LoadAsset(string assetName, System.Type type)
        {
            return null;
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
            await UniTask.DelayFrame(1);
            return null;
        }

        public void LoadAssetAsync<T>(string assetName, UnityAction<T> callback) where T : UnityEngine.Object
        {

        }

        public void LoadAssetAsync(string assetName, System.Type assetType, UnityAction<UnityEngine.Object> callback)
        {

        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            await UniTask.DelayFrame(1);
        }

        public void LoadSceneAsync(string sceneName, UnityAction callback)
        {

        }

        public void UnloadAsset(string assetName)
        {

        }
    }
}
