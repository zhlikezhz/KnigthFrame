using System;
using UnityEngine;
using Joy.MVVM;
using Joy.Asset;
using Joy;
using Cysharp.Threading.Tasks;

public class StartupGame : MonoBehaviour
{
    void Awake()
    {
        Startup();
    }

    async void Startup()
    {
        TickManager.Init(1);
        await AssetManager.Instance.InitAsync();
        await UIManager.Instance.InitAsync();

        var view = Joy.MVVM.UIManager.Instance.OpenWindow<LoadingPage>(WindowType.Page);

        Type t = typeof(UIManager);
    }

    void Update()
    {
        TickManager.OnTick(1);
    }
}