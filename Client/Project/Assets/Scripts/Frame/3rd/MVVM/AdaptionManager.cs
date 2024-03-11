using System;
using UnityEngine;
using UnityEngine.UI;
using Huge;

namespace Huge.MVVM
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
        public float CanvasWidth {get; set;}
        public float CanvasHeight {get; set;}

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

        /// <summary>
        /// 适配UI Canvas
        /// </summary>
        /// <param name="canvasScaler"></param> <summary>
        public void AdapteCanvas(CanvasScaler canvasScaler)
        {
            UnityEngine.Debug.Assert(canvasScaler != null, "param canvasScaler is null");
            canvasScaler.matchWidthOrHeight = IsMatchHeight ? 1 : 0;
            //调用CanvasScaler.OnEnable才能适配新的matchWidthOrHeight
            canvasScaler.enabled = false;
            canvasScaler.enabled = true;
            Vector2 size = canvasScaler.GetComponent<RectTransform>().sizeDelta;
            CanvasWidth = size.x;
            CanvasHeight = size.y;
        }

        public void AdapteSafeArea(RectTransform rt)
        {
            UnityEngine.Debug.Assert(rt != null, "param rt is null");
            rt.offsetMin = new Vector2(ScreenLeft, ScreenDown);
            rt.offsetMax = new Vector2(-ScreenRight, -ScreenUp);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
        }

        public void AdapteBG(RectTransform rt, CanvasScaler uiScaler = null)
        {
            Vector2 size = Vector2.zero;
            bool isMatchHeight = IsMatchHeight;
            Vector2 uiSize = new Vector2(CanvasWidth, CanvasHeight);
            if (uiScaler != null) {
                uiSize = uiScaler.GetComponent<RectTransform>().sizeDelta;
                isMatchHeight = uiScaler.matchWidthOrHeight > 0.5f ? true : false;
            }

            if (isMatchHeight)
            {
                size.x = (uiSize.y / rt.sizeDelta.y) * rt.sizeDelta.x; 
                size.y = uiSize.y;
                if (size.x < uiSize.x)
                {
                    size.x = uiSize.x;
                    size.y = (uiSize.x / rt.sizeDelta.x) * rt.sizeDelta.y;
                }
            }
            else
            {
                size.x = uiSize.x;
                size.y = (uiSize.x / rt.sizeDelta.x) * rt.sizeDelta.y;
                if (size.y < uiSize.y)
                {
                    size.x = (uiSize.y / rt.sizeDelta.y) * rt.sizeDelta.x; 
                    size.y = uiSize.y;
                }
            }
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
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