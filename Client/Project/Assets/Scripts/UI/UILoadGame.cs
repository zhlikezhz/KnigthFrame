using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using GameUtils;
using System.Collections.Generic;
using EasyAlphabetArabic;
using XLua;



public class UILoadGame : MonoBehaviour
{
    //加载页面
    Text txt_version;
    Slider slider_progress;
    Text txt_info;
    Text txt_progress;
    Transform rect_slider;
    //提示框
    GameObject tips;
    Button btnOk;
    Button btnCancel;
    Text txt_lab;
    AssetUpdater mgr;
    Transform rect_wait;

    private long startDownloadTime;


    public void Awake()
    {
        txt_version = transform.Find("txt_version").GetComponent<Text>();
        rect_slider = transform.Find("rect_slider");
        slider_progress = rect_slider.Find("slider_progress").GetComponent<Slider>();
        txt_info = transform.Find("txt_info").GetComponent<Text>();
        txt_progress = rect_slider.Find("txt_progress").GetComponent<Text>();
        rect_wait = transform.Find("rect_wait");

        tips = transform.Find("tips").gameObject;
        txt_lab = transform.Find("tips/txt_lab").GetComponent<Text>();
        btnOk = transform.Find("tips/btn_Panel/btn_ok").GetComponent<Button>();
        btnCancel = transform.Find("tips/btn_Panel/btn_cancel").GetComponent<Button>();
#if DebugMod
        txt_progress.text = EasyArabicCore.CorrectString("نتمنى لك يوم سعيد");
#else
txt_progress.text="";
#endif
        rect_wait.gameObject.SetActive(false);
        rect_slider.gameObject.SetActive(false);

    }

    protected void OnDestroy()
    {
        this.txt_version = null;
        this.txt_info = null;
        txt_progress = null;
        tips = null;
        // logoPage = null;
        // startPage = null;
        this.btnCancel = null;
        btnOk = null;
        txt_lab = null;
        slider_progress = null;
        rect_wait = null;
        rect_slider = null;

        if (this.mgr != null)
        {
            this.mgr.Dispose();
            this.mgr = null;
        }
    }


    public void Start()
    {
        Loom.DispatchToMainThread(()=> {
            mgr = new AssetUpdater();
            mgr.StartUpdate(OnCallback);
        });
    }

    private void UpToData()
    {
#if UNITY_IOS
        rect_slider.gameObject.SetActive(false);
        rect_wait.gameObject.SetActive(true);
#else
        rect_slider.gameObject.SetActive(true);
        rect_wait.gameObject.SetActive(false);
#endif

        //更新配置文件
        AppConst.config = AppConst.serverConifg;
        string path = FileUtils.ins.getPresistentPath(true) + ConfigMgr.Config_Name;
        FileUtils.saveObjectToJsonFile(AppConst.serverConifg, path, AppConst.isConfigEncrypt);

        slider_progress.value = 1;
        gameObject.DestroySelf();
        GameController.Instance.StartGame();
    }

    private void ShowDialog(string tip, UnityAction onOk, UnityAction onCancel)
    {
        tips.gameObject.SetActive(true);
        //txt_lab.text = tip;
        EasyArabicCore.CorrectWithLineWrapping(txt_lab, tip);
        btnCancel.onClick.RemoveAllListeners();
        btnOk.onClick.RemoveAllListeners();
        btnOk.onClick.AddListener(onOk);
        if (onCancel == null)
        {
            btnCancel.gameObject.SetActive(false);
        }
        else
        {
            btnCancel.gameObject.SetActive(true);
            btnCancel.onClick.AddListener(onCancel);
        }
    }

    private void OnCallback(ProgressState state, float p)
    {
        if (Loom.CheckIfMainThread())
        {
            OnCallBackArab(state, p);
        }
        else
        {
            Loom.QueueOnMainThread(OnCallBackArab, state, p);
        }
    }

    private float totalSize = 0;
    private string strTotal = "";
    private void OnCallBackArab(ProgressState state, float p)
    {
        switch (state)
        {
            case ProgressState.Virtual:
                {
                    slider_progress.value = p / totalSize + 0.04f;
                    var numStr = string.Format("{0:f1}", p / totalSize * 100);
                    var txt = numStr + "%:" + EasyArabicCore.CorrectString(@"جاري التحميل");
                    txt_info.text = txt;

                    txt_progress.text = "";
                    txt_info.text = EasyArabicCore.CorrectString("جاري تحميل...");
                }
                break;
            case ProgressState.Checking:
                {
                    txt_info.text = EasyArabicCore.CorrectString("جاري فحص الإصدار...");
                }
                break;
            case ProgressState.Checked:
                {
                    //  txt_version.text = "version " + AppConst.config.version;
                    txt_version.text = AppConst.config.version + ":" + EasyArabicCore.CorrectString("الإصدار");
                    totalSize = p;
                    strTotal = SizeFormat(p);
                    GameDebug.Log("总大小: " + strTotal);
                    rect_slider.gameObject.SetActive(true);
                    rect_wait.gameObject.SetActive(false);
                }
                break;
            case ProgressState.CheckError:
                {
                    ShowDialog("خطأ في الفحص،\nرجاء المحاولة مرة ثانية",
                    () => { tips.gameObject.SetActive(false); mgr.StartDownload(); },
                    () => { Application.Quit(); }
                 );
                }
                break;
            case ProgressState.Updating:
                {
                    slider_progress.value = p / totalSize + 0.04f;
                    var numStr = string.Format("{0:f1}", p / totalSize * 100);
                    var txt = numStr + "%:" + EasyArabicCore.CorrectString(@"جاري التحميل");
                    txt_info.text = txt;
#if DebugMod
                    txt_progress.text = string.Format("{0} / {1}", SizeFormat(p), strTotal);
#endif
                }
                break;
            case ProgressState.Updated:
                {
                    UpToData();
                }
                break;
            case ProgressState.UpdateError:
                {
                    //SdkMgr.Instance.ThinkingAnalyticsTrack("resource_fail");
                    ShowDialog("خطأ في الشبكة،\n رجاء المحاولة مرة ثانية",
                    () => { tips.gameObject.SetActive(false); mgr.StartDownload(); },
                    () => { Application.Quit(); }
                 );
                }
                break;
            case ProgressState.BigVersion:
                {
                    ShowDialog("تم نشر الإصدار الجديد في المتجر\n، رجاء تحديثه لأفضل التجربة!",
                     () => { tips.gameObject.SetActive(false); DownLoadApp(); },
                     () => { Application.Quit(); }
                 );
                }
                break;
            case ProgressState.BigUpdating:
                {
                    slider_progress.value = p + 0.04f;
                    var numStr = string.Format("{0:f1}", p / totalSize * 100);
                    var txt = numStr + "%:" + EasyArabicCore.CorrectString(@"جاري التحميل");
                    txt_info.text = txt;
                }
                break;
            case ProgressState.BigUpdated:
                InstallApp();
                break;
            case ProgressState.BigError:
                {
                    ShowDialog("خطأ في الشبكة،\n رجاء المحاولة مرة ثانية",
                     () => { tips.gameObject.SetActive(false); DownLoadApp(); },
                     () => { Application.Quit(); }
                  );
                }
                break;
            default:
                break;
        }

    }

    private void OnCallBackEn(ProgressState state, float p)
    {
        switch (state)
        {
            case ProgressState.Virtual:
                slider_progress.value = p / totalSize + 0.04f;
                txt_info.text = string.Format("Downloading:  {0:f1}% ", p / totalSize * 100);
                txt_progress.text = "";
                txt_info.text = "Resources Loading...";
                break;
            case ProgressState.Checking:
                txt_info.text = "Version Checking...";
                break;
            case ProgressState.Checked:
                txt_version.text = "version " + AppConst.config.version;
                totalSize = p;
                strTotal = SizeFormat(p);
                GameDebug.Log("总大小: " + strTotal);
                break;
            case ProgressState.CheckError:
                ShowDialog("Checking error,please try again later!",
                   () => { mgr.StartDownload(); tips.gameObject.SetActive(false); },
                   () => { Application.Quit(); }
                );
                break;
            case ProgressState.Updating:
                slider_progress.value = p / totalSize + 0.04f;
                txt_info.text = string.Format("Downloading :  {0:f1}% ", p / totalSize * 100);
#if DebugMod
                txt_progress.text = string.Format("{0} / {1}", SizeFormat(p), strTotal);
#endif
                break;
            case ProgressState.Updated:
                UpToData();
                break;
            case ProgressState.UpdateError:
                ShowDialog("Network error,please try again later!",
                   () => { mgr.StartDownload(); tips.gameObject.SetActive(false); },
                   () => { Application.Quit(); }
                );
                break;
            case ProgressState.BigVersion:
                ShowDialog("New app found,Download now?",
                    () => { DownLoadApp(); tips.gameObject.SetActive(false); },
                    () => { Application.Quit(); }
                );
                break;
            case ProgressState.BigUpdating:
                slider_progress.value = p + 0.04f;
                txt_info.text = string.Format("Downloading :  {0:f1}% ", (p * 100).ToString("F2"));
                break;
            case ProgressState.BigUpdated:
                InstallApp();
                break;
            case ProgressState.BigError:
                ShowDialog("Network error,please try again later!",
                   () => { DownLoadApp(); tips.gameObject.SetActive(false); },
                   () => { Application.Quit(); }
                );
                break;
            default:
                break;
        }

    }

    private string SizeFormat(float p)
    {
        string[] tag = new string[] { "B", "K", "M", "G" };
        int i = 0;
        float f = 0;
        while (p / 1024 > 1)
        {
            i++;
            f = p % 1024;
            p = p / 1024;
        }

        if (i == 0) return p.ToString("0.0") + tag[i];
        f = f / 1024f;
        return (p + f).ToString("0.0") + tag[i];
    }

    private void DownLoadApp()
    {
        //#if UNITY_EDITOR || UNITY_IOS
        //        Application.OpenURL(AppConst.config.appUrl);
        //        Application.Quit();
        //#elif UNITY_ANDROID
        //        mgr.StartDownloadApk();
        //#endif
    }

    private void InstallApp()
    {
        string path = FileUtils.ins.getPresistentPath(true) + AssetUpdater.Config_Name;
        FileUtils.ins.removeFile(path);

        path = FileUtils.ins.getPresistentPath(true) + AssetUpdater.Apk_Name;
        SdkMgr.Instance.InstallApp(path);
    }
}
