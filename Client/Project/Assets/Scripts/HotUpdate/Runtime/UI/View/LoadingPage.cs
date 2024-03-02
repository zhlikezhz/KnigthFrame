using System;
using UnityEngine;
using UnityEngine.UI;
using Huge.MVVM;

[ViewSettingAttribute(typeof(LoadingPageGenerate))]
public class LoadingPage : Page
{
    protected override void Start(params object[] args)
    {
        UnityEngine.Debug.LogError("------");
    }
}