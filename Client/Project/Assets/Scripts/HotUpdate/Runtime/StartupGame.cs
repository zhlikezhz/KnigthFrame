using System;
using UnityEngine;
using Joy.MVVM;
using Joy.Asset;
using Joy;
using Cysharp.Threading.Tasks;
using HQ.Launcher;

public class StartupGame : Launcher
{
    void Awake()
    {
        Startup();
    }

    async void Startup()
    {
        TickManager.Init();

        await AssetManager.Instance.InitAsync();
        await UIManager.Instance.InitAsync();
        await LaunchAsync();
        var view = Joy.MVVM.UIManager.Instance.OpenWindow<LoadingPage>(WindowType.Page);
    }

    void Update()
    {
        TickManager.OnTick();
    }
}