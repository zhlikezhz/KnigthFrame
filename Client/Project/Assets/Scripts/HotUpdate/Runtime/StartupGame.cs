using System;
using UnityEngine;
using Huge.MVVM;
using Huge.Asset;

public class StartupGame : MonoBehaviour
{
    void Awake()
    {
        Startup();
    }

    async void Startup()
    {
        await AssetManager.Instance.InitAsync();
        await UIManager.Instance.InitAsync();
        UIManager.Instance.OpenView<LoadingPage>();
    }
}