using System;
using UnityEngine;
using Huge.MVVM;

public class StartupGame : MonoBehaviour
{
    void Awake()
    {
        Startup();
    }

    async void Startup()
    {
        await UIManager.Instance.InitAsync();
        UIManager.Instance.OpenView<LoadingPage>();
    }
}