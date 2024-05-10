using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huge.Pool;

namespace Huge.MVVM
{
    /// <summary>
    /// Page全屏窗口
    /// Page会管理在其上所有的Popup、Panel、Dialog
    /// 被打开时会隐藏同LayerType中其他窗口，关闭时会显示同LayerType中层级最高的Page
    /// </summary> <summary>
    /// 
    /// </summary>
    public class Page : Window
    {
        internal override void AfterCreate()
        {
            //找到当前Page的位置
            //找到相同层(和当前Page相同层)或者更高层(比当前Page更高层)中位置最高的Page
            int curViewPagePos = -1;
            int topViewPagePos = -1;
            List<Window> viewStack = UIManager.Instance.GetWindowStack();
            for(int i = viewStack.Count - 1; i >= 0; i--)
            {
                Window view = viewStack[i];
                if (topViewPagePos != -1 && view.GetLayerType() >= GetLayerType() && (view as Page == null))
                {
                    topViewPagePos = i;
                }

                if (view == this)
                {
                    curViewPagePos = i;
                    break;
                }
            }

            if (curViewPagePos != -1 && curViewPagePos >= topViewPagePos)
            {
                for(int i = 0; i < curViewPagePos; i++)
                {
                    Window view = viewStack[i];
                    LayerType layerType = view.GetLayerType();
                    if (layerType == GetLayerType())
                    {
                        view.SetActive(false);
                    }
                }
                SetActive(true);
            }
            else
            {
                SetActive(false);
            }
        }

        internal override void BeforeDestroy()
        {
            int curViewPagePos = -1;
            int preViewPagePos = -1;
            int nextViewPagePos = -1;
            List<Window> viewStack = UIManager.Instance.GetWindowStack();
            for (int i = viewStack.Count - 1; i >= 0; i--)
            {
                if (viewStack[i] == this)
                {
                    curViewPagePos = i;
                    break;
                }
            }

            if (curViewPagePos != -1)
            {
                for (int i = curViewPagePos - 1; i >= 0; i--)
                {
                    View view = viewStack[i];
                    if ((view as Page) != null)
                    {
                        preViewPagePos = i;
                        break;
                    }
                }
                for (int i = curViewPagePos + 1; i < viewStack.Count; i++)
                {
                    View view = viewStack[i];
                    if ((view as Page) != null)
                    {
                        nextViewPagePos = i;
                        break;
                    }
                }

                UnityEngine.Debug.LogError($"pre = {preViewPagePos}, nex = {nextViewPagePos}, cur = {curViewPagePos}");
                if (nextViewPagePos == -1)
                {
                    //如果Page是顶层Page，显示其下的Page及其子界面
                    preViewPagePos = (preViewPagePos == -1) ? 0 : preViewPagePos;
                    for (int i = preViewPagePos; i < curViewPagePos; i++)
                    {
                        View view = viewStack[i];
                        view.SetActive(true);
                    }
                }

                //销毁Page及其在同一层的子Popup
                var viewList = ListPool<Window>.Get();
                try
                {
                    nextViewPagePos = (nextViewPagePos == -1) ? viewStack.Count : nextViewPagePos;
                    for (int i = curViewPagePos + 1; i < nextViewPagePos; i++)
                    {
                        viewList.Add(viewStack[i]);
                    }
                    foreach (var view in viewList)
                    {
                        UIManager.Instance.CloseWindow(view);
                    }
                }
                catch(Exception ex)
                {
                    Huge.Debug.LogError($"UI: close page {GetType().Name} error: {ex.Message}.");
                }
                finally
                {
                    ListPool<Window>.Release(viewList);
                }
            }
        }
    }
}
