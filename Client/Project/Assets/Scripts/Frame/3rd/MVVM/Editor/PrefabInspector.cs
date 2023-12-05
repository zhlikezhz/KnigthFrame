using Huge.MVVM;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public enum BindingType
{
    None            = 0,
    UIImage         = 1,
    UIButton        = 2,
    UISlider        = 3, 
    UIToggle        = 4,
    UIScroll        = 5,
    UILoopScroll    = 6,
    UIList          = 7,
    UIRawImage      = 8,
    UIInputField    = 9,
    UIDropdown      = 10,
}

[System.Serializable]
public class BindingData
{
    public GameObject gameObject;
    public BindingType bindingType;
    public BindingBase bindingObject;
}

[System.Serializable]
public class BindingSet
{
    public List<BindingData> BindingList;
}

public class PrefabInspector : Editor
{

}
