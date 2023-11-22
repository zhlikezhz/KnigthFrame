using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameUtils;

/// <summary>
/// Init The UI Root
/// 
/// Root
/// -Canvas
/// --FixedRoot
/// --NormalRoot
/// --PopupRoot
/// -Camera
/// </summary>
public class Root : MonoBehaviour
{
    private static Root m_Instance = null;
    public static Root Instance
    {
        get
        {
            return m_Instance;
        }
    }

    public EventSystem Event;
    public Transform startRoot;
    public Transform normalRoot;
    public Transform subNormalRoot;
    public Transform fixedRoot;
    public Transform popupRoot;
    public Transform peakRoot;
    public CanvasScaler canvasScaler;
    public Camera mainCamera;
    public static int designWidth = 1080;
    public static int designHeight = 1920;
    public static float designMatch = 0;
    public Camera bgCamera;

    private void Awake()
    {
        m_Instance = this;
        InitRoot();
    }   

    void InitRoot()
    {
        float scale = (float)Screen.width / Screen.height;
        float designScale = (float)designWidth / (float)designHeight;
        if (scale > designScale)
        {
            GameDebug.LogYellow("Is 宽屏 ");
            designMatch = 1f;
        }
        else
        {
            GameDebug.Log("Is 长屏");
            designMatch = 0f;
        }

        //GameObject go = new GameObject("Root");
        //go.layer = LayerMask.NameToLayer("UI");
        //go.AddComponent<RectTransform>();
        //m_Instance = .ga.AddComponent<Root>();

        ////add camera
        //GameObject camObj = new GameObject("UICamera");
        //camObj.layer = LayerMask.NameToLayer("UI");
        //camObj.transform.parent = go.transform;
        //camObj.transform.localPosition = new Vector3(0, 0, -100f);
        //camObj.tag = "MainCamera";
        //Camera cam = camObj.AddComponent<Camera>();
        //cam.clearFlags = CameraClearFlags.Depth;
        //cam.orthographic = true;
        //cam.farClipPlane = 200f;
        //cam.cullingMask = 1 << 5;
        //cam.nearClipPlane = -50f;
        //cam.farClipPlane = 50f;
        //camObj.AddComponent<AudioListener>();
        //m_Instance.mainCamera = cam;

        ////add type
        //Canvas can = go.AddComponent<Canvas>();
        ////can.renderMode = RenderMode.ScreenSpaceOverlay;
        //can.renderMode = RenderMode.ScreenSpaceCamera;
        //can.pixelPerfect = false;
        //can.worldCamera = cam;
        CanvasScaler cs = this.gameObject.GetComponent<CanvasScaler>();
        //cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //cs.referenceResolution = new Vector2(designWidth, designHeight);
        //cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        cs.matchWidthOrHeight = designMatch;
        //m_Instance.canvasScaler = cs;
        ////add root
        //GameObject subRoot = CreateSubCanvasForRoot(go.transform, 100);
        //subRoot.name = "NormalRoot";
        //var img = subRoot.AddComponent<Image>();
        //img.color = Color.blue;
        //m_Instance.normalRoot = subRoot.transform;
        //subRoot = CreateSubCanvasForRoot(go.transform, 300);
        //subRoot.name = "FixedRoot";
        //m_Instance.fixedRoot = subRoot.transform;
        //subRoot = CreateSubCanvasForRoot(go.transform, 500);
        //subRoot.name = "PopupRoot";
        //m_Instance.popupRoot = subRoot.transform;
        //subRoot = CreateSubCanvasForRoot(go.transform, 1000);
        //subRoot.name = "PeakRoot";
        //m_Instance.peakRoot = subRoot.transform;

        ////add Event System
        //GameObject eventObj = new GameObject("EventSystem");
        //eventObj.layer = LayerMask.NameToLayer("UI");
        //eventObj.transform.SetParent(go.transform);
        //m_Instance.Event = eventObj.AddComponent<EventSystem>();
        //eventObj.AddComponent<StandaloneInputModule>();
    }

    static GameObject CreateSubCanvasForRoot(Transform root, int sort)
    {
        GameObject go = new GameObject("canvas");
        go.transform.parent = root;
        go.layer = LayerMask.NameToLayer("UI");

        Canvas can = go.AddComponent<Canvas>();
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        can.transform.localScale = Vector2.one;//new Vector3(size,1,1);
        can.overrideSorting = true;
        can.sortingOrder = sort;

        go.AddComponent<YHGraphicRaycaster>();
        return go;
    }

    public static void SetScreenOrientation_Protrait()
    {
        m_Instance.canvasScaler.matchWidthOrHeight = 0;
        m_Instance.canvasScaler.referenceResolution = new Vector2(1080, 1920);
    }

    public static void SetScreenOrientation_Left()
    {
        m_Instance.canvasScaler.matchWidthOrHeight = 1;
        m_Instance.canvasScaler.referenceResolution = new Vector2(1920, 1080);
    }

    public void SetBGTexture(RawImage image)
    {
        if (bgCamera != null && image)
        {
            image.texture = bgCamera.targetTexture;
        }
    }
    void OnDestroy()
    {
        m_Instance = null;
    }
}