using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Asset
{
    /*
    public class BundleObject
    {
        public AssetBundle Bundle;

        int m_iRefCount = 0;
        Dictionary<string, Object> m_Assets;
        List<BundleObject> m_DependBundles = new List<BundleObject>();

        public static async UniTask<BundleObject> LoadFromFile(string path)
        {
            return null;
        }

        public static async UniTask<BundleObject> LoadFromRemote(string url)
        {
            byte[] bytes = await DownloadFile(url);
            await SaveToFile(bytes, path);
            return LoadFromFile(path);
        }

        public async UniTask<Object> LoadAsset(string assetName)
        {
            AddRef();
            List<string> dependBundleNames = GetDependBundleNamesByAssetName(assetName);
            foreach (string dependBundleName in dependBundleNames)
            {
                string path = GetBundlePath(dependBundleName);
                BundleObject bundleObject = await BundleObject.LoadFromFile(path);
                m_DependBundles.Add(bundleObject);
                bundleObject.AddRef();
            }
        }

        public async void UnloadAsset(string assetName)
        {

        }

        public void AddRef()
        {
            m_iRefCount++;
        }

        public void ReduceRef()
        {
            m_iRefCount--;
        }
    }

    public class BundleLoader
    {
        public async UniTask<AssetBundle> LoadBundle(string bundleName)
        {

        }

        public LoadBundle
    }
    */
}