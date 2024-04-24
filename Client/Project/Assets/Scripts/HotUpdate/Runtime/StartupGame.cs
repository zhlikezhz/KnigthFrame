using System;
using UnityEngine;
using Huge.MVVM;
using Huge.Asset;
using Huge.OCS;
using Unity.Entities;

public class StartupGame : MonoBehaviour
{
    void Awake()
    {
        Startup();
    }

    async void Startup()
    {
        Global.Asset = AssetManager.Instance;
        await Global.Asset.InitAsync();
        Global.UI = UIManager.Instance;
        await Global.UI.InitAsync();
        Global.World = new Engine();
        await Map.CreateAsync(100, 100, 100);
        Hero.CreateAsync();
    }
}