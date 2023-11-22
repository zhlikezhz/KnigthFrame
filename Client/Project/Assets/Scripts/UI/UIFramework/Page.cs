
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using DG.Tweening;

//#region define Enum

//public enum UIType
//{
//    None,      //独立的窗口
//    Normal,
//    Fixed,
//    PopUp,
//    Peak,
//}

//public enum UIAnim
//{
//    None,
//    RightToLeft,
//    DownToUp,
//    MiddleAppear,
//    /// <summary>
//    /// 顶端向下动画
//    /// </summary>
//    TopToDown,
//}
//#endregion


///// <summary>
///// Each Page Mean one UI 'window'
///// 3 steps:
///// instance ui > refresh ui by data > show
///// </summary>

//public abstract class Page
//{
//    //this page's name
//    public string uiName = string.Empty;

//    //path to load ui
//    private string uiPath = string.Empty;

//    //this page's type
//    private UIType type = UIType.Normal;

//    //the background collider mode
//    private UIAnim uiAnim = UIAnim.None;

//    //this ui's gameobject
//    protected GameObject gameObject;
//    protected Transform transform;

//    //refresh page 's data.
//    public object data = null;

//    #region virtual api
//    ///When Instance UI Ony Once.
//    public virtual void Awake(GameObject go) { }

//    ///Show UI Refresh Eachtime.
//    public virtual void Refresh(object data) { }

//    public virtual void OnPushAnim() { }


//    protected virtual void ReadyActive()
//    {
//        if (!this.gameObject.activeSelf)
//        {
//            OnEnable();
//            this.gameObject.SetActive(true);
//        }
//        transform.SetAsLastSibling();
//        ReadyActivePostion();
//    }



//    ///Active this UI
//    protected virtual void Active()
//    {

//      //  ActivePosition();

//        TweenCallback callback = () =>
//        {
//            OnPushAnim();
//        };
//        PushAnim(callback);
//    }

//    protected virtual void OnEnable() { }

//    protected virtual void OnDisable() { }

//    protected virtual void OnDestroy()
//    {

//        this.transform = null;
//        this.gameObject = null;
//        this.data = null;
//    }

//    /// <summary>
//    /// Only Deactive UI wont clear Data.
//    /// </summary>
//    public virtual void Hide(bool isRemove = false)
//    {
//        TweenCallback callback = () =>
//        {
//            OnDisable();
//            if (isRemove)
//            {
//                gameObject.DestroySelf();
//                OnDestroy();
//            }
//            else
//            {
//                gameObject.SetActive(false);
//            }

//        };
//        PopAnim(callback);
//    }

//    public static readonly float animTime = 0.3f;
//    private readonly int offsetX = 900;
//    private readonly int offsetY = 1000;
//    private readonly int mainToX = -210;

//    private void ReadyActivePostion()
//    {
//        switch (uiAnim)
//        {
//            case UIAnim.None:
//                {
                 
//                }
//                break;
//            case UIAnim.RightToLeft:
//                {
//                    //当前界面
//                    float fromX = (Screen.width + offsetX)*2;
//                    transform.localPosition = new Vector3(fromX, 0, 0);
                    
//                }
//                break;
//            case UIAnim.DownToUp:
//                {
//                    float fromY = (Screen.height + offsetY) * -2;
//                    transform.localPosition = new Vector3(0, fromY);
//                }
//                break;
//            case UIAnim.MiddleAppear:
//                {
//                    transform.localScale = Vector3.zero;
//                }
//                break;
//            case UIAnim.TopToDown:
//                {
//                    //当前界面
//                    float fromY = (Screen.height + offsetY) * 2;
//                    transform.localPosition = new Vector3(0, fromY, 0);
//                }
//                break;
//            default:
//                break;
//        }
//    }

//    private void ActivePosition()
//    {
//        switch (uiAnim)
//        {
//            case UIAnim.None:
//                {

//                }
//                break;
//            case UIAnim.RightToLeft:
//                {
//                    //当前界面
//                    float fromX = Screen.width + offsetX;
//                    transform.localPosition = new Vector3(fromX, 0, 0);

//                }
//                break;
//            case UIAnim.DownToUp:
//                {
//                    float fromY = (Screen.height + offsetY) * -1;
//                    transform.localPosition = new Vector3(0, fromY);
//                }
//                break;
//            case UIAnim.MiddleAppear:
//                {
//                    transform.localScale = Vector3.one;
//                }
//                break;
//            case UIAnim.TopToDown:
//                {
//                    //当前界面
//                    float fromY = (Screen.height + offsetY) * -1;
//                    transform.localPosition = new Vector3(0, fromY, 0);
//                }
//                break;
//            default:
//                break;
//        }
//    }
//    /// <summary>
//    /// 出现动画
//    /// </summary>
//    private void PushAnim(TweenCallback callback)
//    {
//        switch (uiAnim)
//        {
//            case UIAnim.None:
//                {
//                    callback();
//                }
//                break;
//            case UIAnim.RightToLeft:
//                {
//                    //当前界面
//                    float fromX = Screen.width + offsetX;
//                    transform.localPosition = new Vector3(fromX, 0, 0);
//                    transform.DOLocalMoveX(0, animTime).SetEase(Ease.OutQuad).OnComplete(callback);
//                    //主界面
//                    //Page main = UIManager.GetPage(AppConst.MainUI);
//                    //if (main != null)
//                    //{
//                    //    main.transform.DOLocalMoveX(mainToX, animTime).SetEase(Ease.OutQuad).OnComplete(() =>
//                    //    {
//                    //        main.gameObject.SetActive(false);
//                    //    });
//                    //}
//                }
//                break;
//            case UIAnim.DownToUp:
//                {
//                    float fromY = (Screen.height + offsetY) * -1;
//                    transform.localPosition = new Vector3(0, fromY);
//                    transform.DOLocalMoveY(0, animTime).SetEase(Ease.OutQuart).OnComplete(callback);
//                }
//                break;
//            case UIAnim.MiddleAppear:
//                {
//                    transform.localScale = Vector3.one;
//                    Transform frame = transform.Find("frame");
//                    if (frame != null)
//                    {
//                        frame.localScale = Vector3.one * 0.8f;
//                        frame.DOScale(1, animTime).SetEase(Ease.OutBack).OnComplete(callback);
//                    }
//                }
//                break;
//            case UIAnim.TopToDown:
//                {
//                    transform.localPosition = Vector3.zero;
//                    var frameRect = transform.Find("frame").GetComponent<RectTransform>();
//                    if (frameRect != null)
//                    {
//                        float h = frameRect.sizeDelta.y + 100;
//                        var pos = frameRect.anchoredPosition;
//                        pos.y = h;
//                        frameRect.anchoredPosition = pos;
//                        //top stretch
//                        //pivot (0.5,1)
//                        frameRect.DOAnchorPos(Vector2.zero, animTime).SetEase(Ease.OutQuad).OnComplete(callback);
//                    }
//                    //float fromY = (Screen.height + offsetY) ;
//                    //transform.localPosition = new Vector3(0, fromY);
//                    //transform.DOLocalMoveY(0, animTime).SetEase(Ease.OutQuart).OnComplete(callback);
//                }
//                break;
//            default:
//                break;
//        }
//    }
//    /// <summary>
//    /// 隐藏动画
//    /// </summary>
//    /// <param name="callback"></param>
//    private void PopAnim(TweenCallback callback)
//    {
//        switch (uiAnim)
//        {
//            case UIAnim.RightToLeft:
//                {   //当前界面
//                    float ToX = Screen.width + offsetX;
//                    transform.localPosition = Vector3.zero;
//                    transform.DOLocalMoveX(ToX, animTime).SetEase(Ease.InQuad).OnComplete(callback);
//                    //主界面
//                    //Page main = UIManager.GetPage(AppConst.MainUI);
//                    //if (main != null)
//                    //{
//                    //    main.transform.DOLocalMoveX(0, animTime).SetEase(Ease.InQuad);
//                    //    main.gameObject.SetActive(true);
//                    //}
//                }
//                break;
//            case UIAnim.DownToUp:
//                {
//                    float ToY = (Screen.height + offsetY) * -1;
//                    transform.localPosition = Vector3.zero;
//                    transform.DOLocalMoveY(ToY, animTime).SetEase(Ease.InQuad).OnComplete(callback);
//                }
//                break;
//            case UIAnim.MiddleAppear:
//                {  //Transform frame = transform.Find("frame");
//                   //if (frame != null)
//                   //    frame.DOScale(0.8f, animTime).SetEase(Ease.InBack).OnComplete(callback);
//                    callback();
//                }
//                break;
//            case UIAnim.TopToDown:
//                {
//                    var frameRect = transform.Find("frame").GetComponent<RectTransform>();
//                    float h = frameRect.sizeDelta.y + 100;
//                    if (frameRect != null)
//                    {
//                        //top stretch
//                        //pivot (0.5,1)
//                        frameRect.DOAnchorPos(new Vector2(0, h), animTime).SetEase(Ease.OutQuad).OnComplete(callback);
//                    }

//                    //float toY = (Screen.height + offsetY);
//                    //transform.localPosition = Vector3.zero;
//                    //transform.DOLocalMoveY(toY, animTime).SetEase(Ease.OutQuart).OnComplete(callback);
//                }
//                break;
//            default:
//                {
//                    callback();
//                }
//                break;
//        }
//    }



//    #endregion

//    #region internal api
//    public Page() { }
//    public Page(string uiPath, UIType type, UIAnim uiAnim)
//    {
//        this.uiPath = uiPath;
//        this.type = type;
//        this.uiAnim = uiAnim;
//    }
//    public bool isActive()
//    {
//        bool ret = gameObject != null && gameObject.activeSelf;
//        return ret;
//    }

//    /// <summary>
//    /// Sync Show UI Logic
//    /// </summary>
//    public void Show()
//    {
//        //1:instance UI
//        if (gameObject == null && !string.IsNullOrEmpty(uiPath))
//        {
//            GameObject go = ResourceMgr.GetInstance.LoadUIPrefab(uiPath);
//            //protected.
//            if (go == null)
//            {
//                GameDebug.LogError("[UI] Cant load your ui prefab. " + uiPath);
//                return;
//            }
//            go = Object.Instantiate(go);
//            go.name = go.name.Replace("(Clone)", "");

//            //set parent
//            AnchorUIGameObject(go);

//            //after instance should awake init.
//            Awake(go);
//        }
//        ReadyActive();

//        //:refresh ui component.
//        Refresh(data);

//        Active();

//        //:animation or init when active.

//    }

//    public Coroutine StartCoroutine(IEnumerator routine)
//    {
//        return Root.Instance.StartCoroutine(routine);
//    }
//    public Coroutine StartCoroutine(string methodName)
//    {
//        return Root.Instance.StartCoroutine(methodName);
//    }
//    public Coroutine StartCoroutine(string methodName, object value)
//    {
//        return Root.Instance.StartCoroutine(methodName, value);
//    }
//    public void StopCoroutine(string methodName)
//    {
//        Root.Instance.StopCoroutine(methodName);
//    }
//    public void StopCoroutine(IEnumerator routine)
//    {
//        Root.Instance.StopCoroutine(routine);
//    }
//    public void StopCoroutine(Coroutine routine)
//    {
//        Root.Instance.StopCoroutine(routine);
//    }
//    public void StopAllCoroutine()
//    {
//        Root.Instance.StopAllCoroutines();
//    }

//    public GameObject FindGameObject(string path)
//    {
//        Transform tran = transform.Find(path);
//        if (tran)
//            return tran.gameObject;
//        return null;
//    }
//    public T GetComponent<T>(string path) where T : Component
//    {
//        var tran = transform.Find(path);
//        if (!tran) return null;
//        return tran.GetComponent<T>();
//    }

//    protected void AnchorUIGameObject(GameObject ui)
//    {
//        if (Root.Instance == null || ui == null) return;

//        this.gameObject = ui;
//        this.transform = ui.transform;
//        if (type == UIType.Normal)
//        {
//            ui.transform.SetParent(Root.Instance.normalRoot, false);
//        }
//        else if (type == UIType.Fixed)
//        {
//            ui.transform.SetParent(Root.Instance.fixedRoot, false);
//        }
//        else if (type == UIType.PopUp)
//        {
//            ui.transform.SetParent(Root.Instance.popupRoot, false);
//        }
//        else if (type == UIType.Peak)
//        {
//            ui.transform.SetParent(Root.Instance.peakRoot, false);
//        }
//        ui.transform.localPosition = Vector3.zero;
//        ui.transform.localScale = Vector3.one;
//    }
//    #endregion
//}//Page