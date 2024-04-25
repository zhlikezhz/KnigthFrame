using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Huge.MVVM
{
    public class Popup : Window
    {
        internal override void AfterCreate()
        {
            RefreshPopupMask();
        }

        internal override void BeforeDestroy()
        {
            RefreshPopupMask();
        }

        internal void RefreshPopupMask()
        {
            Popup topPopup = null;
            List<Window> viewStack = UIManager.Instance.GetWindowStack();
            for (int i = 0; i < viewStack.Count; i++)
            {
                View view = viewStack[i];
                Popup popup = view as Popup;
                if (popup != null && popup.IsUseMask())
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
                    button.onClick.AddListener(OnClickMask);
                }
            }
            else
            {
                Button button = maskObj.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                maskObj.SetActive(false);
            }
        }

        protected virtual bool IsUseMask()
        {
            return true;
        }

        protected virtual float GetMaskAlpha()
        {
            return 1.0f;
        }

        protected virtual bool IsClickMask()
        {
            return true;
        }

        protected virtual void OnClickMask()
        {
            Close();
        }
    }
}
