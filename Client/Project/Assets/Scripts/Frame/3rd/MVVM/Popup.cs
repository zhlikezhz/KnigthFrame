using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Huge.MVVM
{
    // public interface IPopup
    // {
    //     bool IsUseMask();
    //     void OnClickMask();
    //     float GetMaskAlpha();
    // }

    internal static class Popup
    {
        internal static void AfterCreate(Window self)
        {
            RefreshPopupMask(self);
        }

        internal static void BeforeDestroy(Window self)
        {
            RefreshPopupMask(self);
        }

        internal static void RefreshPopupMask(Window self)
        {
            Window topPopup = null;
            List<Window> viewStack = UIManager.Instance.GetWindowStack();
            for (int i = 0; i < viewStack.Count; i++)
            {
                Window popup = viewStack[i];
                WindowType window = popup.GetWindowLayer();
                if (window == WindowType.Popup && popup.IsUseMask())
                {
                    if (topPopup == null)
                    {
                        topPopup = popup;
                    }
                    else
                    {
                        if (topPopup.GetLayerType() <= popup.GetLayerType())
                        {
                            topPopup = popup;
                        }
                    }
                }
            }

            GameObject maskObj = UIManager.Instance.GetMaskObject();
            if (topPopup != null)
            {
                maskObj.SetActive(true);
                Image image = maskObj.GetComponent<Image>();
                Button button = maskObj.GetComponent<Button>();
                image.color = new Color(1, 1, 1, topPopup.GetMaskAlpha());
                button.interactable = topPopup.IsClickMask();
                if (topPopup.IsClickMask())
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(topPopup.OnClickMask);
                }
            }
            else
            {
                Button button = maskObj.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                maskObj.SetActive(false);
            }
        }
    }
}
