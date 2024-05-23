using UnityEngine;

namespace Huge.MVVM
{
    public abstract partial class View
    {
        public Vector3 position => transform.position;
        public Vector3 localPosition => transform.localPosition;
        public Vector3 eulerAngles => transform.eulerAngles;
        public Vector3 localEulerAngles => transform.localEulerAngles;
        public Vector3 right => transform.right;
        public Vector3 up => transform.up;
        public Vector3 forward => transform.forward;
        public Quaternion rotation => transform.rotation;
        public Quaternion localRotation => transform.localRotation;
        public Vector3 localScale => transform.localScale;
        public Vector2 anchorMin => transform.anchorMin;
        public Vector2 anchorMax => transform.anchorMax;
        public Vector2 anchoredPosition => transform.anchoredPosition;
        public Vector2 sizeDelta => transform.sizeDelta;
        public Vector2 pivot => transform.pivot; 
        public Vector2 offsetMin => transform.offsetMin;
        public Vector2 offsetMax => transform.offsetMax;
        public Rect rect => transform.rect;
        public void SetAsFirstSibling() => transform.SetAsFirstSibling();
        public void SetAsLastSibling() => transform.SetAsLastSibling();
        public void SetSiblingIndex(int index) => transform.SetSiblingIndex(index);
        public int GetSiblingIndex() => transform.GetSiblingIndex();
    }
}