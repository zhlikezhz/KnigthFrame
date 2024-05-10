using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Huge.MVVM
{
    public struct UIItemDataBinder
    {
        GameObject m_SelectedGameObject;

        int m_SelectedComponentIndex;
        Component m_SelectedComponent;
        Component[] m_ComponentsInGameObject;
        int m_SelectedPropertyIndex;
        string m_SelectedPropertyName;
        string[] m_PropertiesInComponent;
    }

    public class UIBaseBinder : MonoBehaviour
    {
        public string Name;
        public UIVariableArray VariableArray = new UIVariableArray();
    }
}