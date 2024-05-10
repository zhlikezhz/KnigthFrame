using System;
using Huge.MVVM;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Huge.MVVM.Editor
{
    [CustomEditor(typeof(UIBaseBinder))]
    public class UIBaseBinderInspector : UnityEditor.Editor
    {
        public VisualTreeAsset m_InspectorXML; 

        // public override VisualElement CreateInspectorGUI()
        // {
        //     // Create a new VisualElement to be the root of our inspector UI
        //     VisualElement myInspector = new VisualElement();

        //     // if (m_InspectorXML != null)
        //     //     m_InspectorXML.CloneTree(myInspector);

        //     // Return the finished inspector UI
        //     return myInspector;
        // }
    }
}