using System;
using UnityEngine;
using UnityEngine.UI;
using Joy.MVVM;
using Joy.MVVM.DataBinding;
using System.Collections.Generic;

public class ItemLoadingPageViewModel : ItemLoadingPageListViewViewModelGenerate
{
    ColorVM color = ColorVM.white;
    public ColorVM Color
    {
        get { return color; }
        set { Set (ref color, value); }
    }

    Vector2VM position = Vector2VM.zero;
    public Vector2VM Position
    {
        get { return position; }
        set { Set (ref position, value); }
    }

    public override void OnClickIcon()
    {
        UnityEngine.Debug.LogError("------");
    }
}


public class LoadingPageViweModel : LoadingPageViewModelGenerate
{
    public LoadingPageViweModel()
    {
        SldBar = 0.5f;
        TxtNum = "1000";
        TxtSpeed = "1.0";

        var vm1 = new ItemLoadingPageViewModel();
        vm1.ImgIcon = Joy.Asset.AssetManager.Instance.LoadAsset<Sprite>("Assets/Art/UI/Textures/Common/add02.png");
        vm1.Position = new Vector2VM(50, 50);
        vm1.Color.r = 0f;
        vm1.TxtName = "234";
        ListView.Add(vm1);

        var vm2 = new ItemLoadingPageViewModel();
        vm2.ImgIcon = Joy.Asset.AssetManager.Instance.LoadAsset<Sprite>("Assets/Art/UI/Textures/Common/add05.png");
        vm2.Color = Color.red;
        vm2.TxtName = "1234";
        ListView.Add(vm2);
    }
}