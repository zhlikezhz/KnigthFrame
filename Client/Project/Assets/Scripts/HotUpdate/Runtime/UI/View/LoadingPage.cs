using System;
using UnityEngine;
using UnityEngine.UI;
using Joy.MVVM;
using Joy.MVVM.DataBinding;

public class ItemLoadingPageListView : ItemLoadingPageListViewGenerate
{
	protected override void ReplaceViewModel(ViewModel model)
	{
        BindViewModel(bindSet, model);
        ItemLoadingPageViewModel vm = model as ItemLoadingPageViewModel;
        Image image = _BtnIcon.GetComponent<Image>();
        bindSet.BindProperty(vm).For(() => vm.Color, nameof(vm.Color)).To((color) => image.color = color);
        bindSet.BindProperty(vm).For(() => vm.Position, nameof(vm.Position)).To((position) => image.rectTransform.anchoredPosition = position);
	}
}

[ViewSettingAttribute("Assets/Art/UI/Prefab/Common/LoadingPage.prefab")]
public partial class LoadingPage : LoadingPageGenerate
{
    Button button;
    InputField input;
    BindingSet bindSet;
    LoadingPageViweModel vm;

    protected override void OnStart()
    {
        vm = new LoadingPageViweModel();
        bindSet = BindingSet.Get();
        BindViewModel(bindSet, vm);
        bindSet.Build();
    }

    protected override void OnDestroy()
    {

    }

	protected override IListView CreateListView()
	{
		return AddSubView<ListView<ItemLoadingPageListView>>(_SclListView.gameObject);
	}
}