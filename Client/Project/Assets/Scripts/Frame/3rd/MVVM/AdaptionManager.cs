using System;
using UnityEngine;
using UnityEngine.UI;
using Huge;

namespace HQ.MVVM
{
    //屏幕适配管理
    public class AdaptionManager : Singleton<AdaptionManager>
    {
        public bool IsLandscape {get; set;}
        public bool IsMatchHeight {get; set;}
        public float ScreenLeft {get; set;}
        public float ScreenRight {get; set;}
        public float ScreenUp {get; set;}
        public float ScreenDown {get; set;}

        public AdaptionManager()
        {
            RefreshScreenSafeArea();
            RefreshScreenMatchMode();
        }

        void RefreshScreenMatchMode()
        {
            bool isPad = false;
            float iphoneScreenRate = 9.0f / 16.0f;
            float screenRate = Screen.width * 1.0f / Screen.height;
            if (screenRate > iphoneScreenRate + 0.00001f)
            {
                isPad = true;
            }

            var orientation = Screen.orientation;
            if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight)
            {
                //横屏：以9:16屏幕为分界点，小于等于9:16的屏幕按高适配(iphone)，大于9:16的屏幕按宽适配(ipad)
                if (isPad) IsMatchHeight = false;
                else IsMatchHeight = true;
            }
            else
            {
                //竖屏：以9:16屏幕为分界点，小于等于9:16的屏幕按宽适配(iphone)，大于9:16的屏幕按高适配(ipad)
                if (isPad) IsMatchHeight = true;
                else IsMatchHeight = false;
            }
        }

        void RefreshScreenSafeArea()
        {
            var orientation = Screen.orientation;
            if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight)
            {
                ScreenUp = 0;
                ScreenDown = 0;
                IsLandscape = true;
                if (orientation == ScreenOrientation.LandscapeLeft)
                {
                    ScreenLeft = Screen.safeArea.x;
                    ScreenRight = Screen.width - Screen.safeArea.x - Screen.safeArea.width;
                }
                else
                {
                    ScreenLeft = Screen.width - Screen.safeArea.x - Screen.safeArea.width;
                    ScreenRight = Screen.safeArea.x;
                }
            }
            else if (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown)
            {
                ScreenLeft = 0;
                ScreenRight = 0;
                IsLandscape = false;
                if (Screen.orientation == ScreenOrientation.Portrait)
                {
                    ScreenUp = Screen.height - Screen.safeArea.y - Screen.safeArea.height;
                    ScreenDown = Screen.safeArea.y;
                }
                else
                {
                    ScreenUp = Screen.safeArea.y;
                    ScreenDown = Screen.height - Screen.safeArea.y - Screen.safeArea.height;
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"screen orientation is ScreenOrientation.AutoRotation, ui adapter not supportted.");
            }
        }

        public void AdapteCanvas(CanvasScaler canvasScaler)
        {
            UnityEngine.Debug.Assert(canvasScaler != null, "param canvasScaler is null");
            canvasScaler.matchWidthOrHeight = IsMatchHeight ? 1 : 0;
        }

        public void AdapteSafeArea(RectTransform rt)
        {
            UnityEngine.Debug.Assert(rt != null, "param rt is null");
            rt.offsetMin = new Vector2(ScreenLeft, ScreenDown);
            rt.offsetMax = new Vector2(-ScreenRight, -ScreenUp);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
        }

        public void AdapteBG(RectTransform uiRoot, RectTransform rt)
        {
            var size = uiRoot.sizeDelta;

            float scale     = 1.0f;
            Vector2 sizeDelta = rt.sizeDelta;
            scale = Mathf.Max(scale, size.x / sizeDelta.x);
            scale = Mathf.Max(scale, size.y / sizeDelta.y);
            float width  = sizeDelta.x * scale;
            float height = sizeDelta.y * scale;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(width, height);
            if (IsLandscape)
            {
                float offsetX = -((uiRoot.offsetMin.x + uiRoot.offsetMax.x) * 0.5f);
                float offsetY = -((uiRoot.offsetMin.y + uiRoot.offsetMax.y) * 0.5f);
                rt.anchoredPosition = new Vector2(offsetX, offsetY);
            }
        }

        public void AdapteLeft(RectTransform rt)
        {

        }

        public void AdapteRight(RectTransform rt)
        {

        }

        public void AdapteLeftAndRight(RectTransform rt)
        {

        }

        public void AdapteUp(RectTransform rt)
        {

        }

        public void AdapteDown(RectTransform rt)
        {

        }

        public void AdapteUpAndDown(RectTransform rt)
        {

        }
    }
}