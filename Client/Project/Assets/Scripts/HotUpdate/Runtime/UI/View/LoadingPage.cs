using System;
using UnityEngine;
using UnityEngine.UI;
using Joy.MVVM;
using Joy.MVVM.DataBinding;
using Joy.Asset;
using System.Diagnostics;

// public class ItemLoadingPageListView : ItemLoadingPageListViewGenerate
// {
// 	protected override void OnDataContextChanged(ViewModel model)
// 	{
//         BindViewModel(bindSet, model);
//         Image image = _BtnIcon.GetComponent<Image>();
//         ItemLoadingPageViewModel vm = model as ItemLoadingPageViewModel;
//         bindSet.BindProperty(vm).For(() => vm.Color, nameof(vm.Color)).To((color) => image.color = color);
//         bindSet.BindProperty(vm).For(() => vm.Position, nameof(vm.Position)).To((position) => image.rectTransform.anchoredPosition = position);
// 	}
// }

public class ItemLoadingPageLoopListViewViewModel : ItemLoadingPageLoopListViewViewModelGenerate
{
    // bool isShow = false;
    public ObservableList<ItemLoadingPageLoopListViewViewModelGenerate> List;
    public override void OnClickIcon(bool isOn) 
    { 
        UnityEngine.Debug.LogError($"-----0--{isOn}" );
        if (isOn == true)
        {
            UnityEngine.Debug.LogError("-----1");
            ImgIcon = isOn ? AssetManager.Instance.LoadAsset<Sprite>("Assets/Art/UI/Textures/Common/btn_m04_Normal.png") : AssetManager.Instance.LoadAsset<Sprite>("Assets/Art/UI/Textures/Common/btn_m05_Normal.png");
        }
        else
        {
            // bool isCanSelectFalse = false;
            // foreach(var item in List)
            // {
            //     if (item.TglGrpIcon == true)
            //     {
            //         isCanSelectFalse = true;
            //         break;
            //     }
            // }
            // UnityEngine.Debug.LogError("-----2");
            // if (!isCanSelectFalse)
            // {
            //     TglGrpIcon = true;
            //     return;
            // }
            UnityEngine.Debug.LogError("-----2");
            ImgIcon = isOn ? AssetManager.Instance.LoadAsset<Sprite>("Assets/Art/UI/Textures/Common/btn_m04_Normal.png") : AssetManager.Instance.LoadAsset<Sprite>("Assets/Art/UI/Textures/Common/btn_m05_Normal.png");
        }
    }
}

public class LoadingPageViewModel : LoadingPageViewModelGenerate
{
    public override void OnClickSelect()
    {
        LoopListView[5].TglGrpIcon = true;
    }

    public LoadingPageViewModel()
    {
        var vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "1";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "2";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "3";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "4";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "5";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "6";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "7";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "8";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "9";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "10";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "11";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "12";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.TxtName = "13";
        vm.List = LoopListView;
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.List = LoopListView;
        vm.TxtName = "14";
        LoopListView.Add(vm);
        vm = new ItemLoadingPageLoopListViewViewModel();
        vm.List = LoopListView;
        vm.TxtName = "15";
        LoopListView.Add(vm);
    }
}

[ViewSettingAttribute("Assets/Art/UI/Prefab/Common/LoadingPage.prefab")]
public partial class LoadingPage : LoadingPageGenerate
{
    protected override void OnStart()
    {
        var mainVM = new LoadingPageViewModel();
        DataContext = mainVM;
    }

	protected override void OnDataContextChanged(ViewModel model)
    {
        var vm = model as LoadingPageViewModelGenerate;
        BindViewModel(bindSet, vm);
    }

    protected override void OnDestroy()
    {

    }
}