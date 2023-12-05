using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huge.Pool;

namespace Huge.MVVM
{
    public class Page : View
    {
        internal override void AfterInit(params object[] args)
        {
            int curViewPagePos = -1;
            List<View> viewStack = UIManager.Instance.GetViewStack();
            for(int i = viewStack.Count - 1; i >= 0; i--)
            {
                if (viewStack[i] == this)
                {
                    curViewPagePos = i;
                    break;
                }
            }

            if (curViewPagePos != -1)
            {
                for(int i = 0; i < curViewPagePos; i++)
                {
                    View view = viewStack[i];
                    LayerType layerType = view.GetLayerType();
                    if (layerType == GetLayerType())
                    {
                        view.SetActive(false);
                    }
                }
            }

            base.AfterInit();
        }

        internal override void BeforeDestroy()
        {
            int curViewPagePos = -1;
            int preViewPagePos = -1;
            int nextViewPagePos = -1;
            List<View> viewStack = UIManager.Instance.GetViewStack();
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
                    if (view is Page)
                    {
                        preViewPagePos = i;
                        break;
                    }
                }
                for (int i = curViewPagePos + 1; i < viewStack.Count; i++)
                {
                    View view = viewStack[i];
                    if (view is Page)
                    {
                        nextViewPagePos = i;
                        break;
                    }
                }

                if (nextViewPagePos != -1)
                {
                    //如果Page是顶层Page，显示其下的Page及其子界面。
                    preViewPagePos = (preViewPagePos == -1) ? 0 : preViewPagePos;
                    for (int i = preViewPagePos; i < curViewPagePos; i++)
                    {
                        View view = viewStack[i];
                        view.SetActive(true);
                    }
                }

                //销毁Page及其在同一层的子Popup
                var viewList = ListPool<View>.Get();
                try
                {
                    nextViewPagePos = (nextViewPagePos == -1) ? viewStack.Count : nextViewPagePos;
                    for (int i = curViewPagePos + 1; i < nextViewPagePos; i++)
                    {
                        viewList.Add(viewStack[i]);
                    }
                    foreach (var view in viewList)
                    {
                        UIManager.Instance.CloseView(view);
                    }
                    ListPool<View>.Release(viewList);
                }
                catch(Exception ex)
                {
                    ListPool<View>.Release(viewList);
                    ViewInfo viewInfo = GetViewInfo();
                    Huge.Debug.LogError($"UI: close page {viewInfo.View.Name} error: {ex.Message}.");
                }
            }
            base.BeforeDestroy();
        }
    }
}
