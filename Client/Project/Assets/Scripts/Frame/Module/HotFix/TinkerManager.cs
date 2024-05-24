using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Joy.FSM;
using Joy;
using System;

namespace Joy.HotFix
{
    public enum TinkerState
    {
        StartTinker             = 0, //开始更新
        PrepareFirstGame        = 1, //首次进入游戏处理
        CheckRemoteFile         = 2, //检查CDN
        DownloadPatch           = 3, //下载Patch
        UnzipPatch              = 4, //解压Patch
        InstallPatch            = 5, //安装Patch
        EndTinker               = 6, //结束更新
    }

    public class TinkerManager : FSMContent
    {
        static readonly string BI_TINKER_LOG = "BITinker";
        static readonly Joy.SDK.BIData biData = new Joy.SDK.BIData();

        float m_fProgress = 0.0f;
        public float Progress
        {
            get { return m_fProgress; }
            set { m_fProgress = value; }
        }

        string m_strMessage;
        public string Message
        {
            get { return m_strMessage; }
            set { m_strMessage = value; }
        }

        bool m_bIsCompleted = false;
        public bool IsCompleted 
        { 
            get { return m_bIsCompleted; } 
            set { m_bIsCompleted = value; }
        }

        public async void HotFix()
        {
            ChangeState<PrepareFirstGameState>();
            await UniTask.WaitUntil(() => 
            { 
                return m_bIsCompleted; 
            });
        }

        static public void LogGameBI(bool isError, TinkerState state, string errMsg = "")
        {
            biData.Clear();
            biData.AddData("status", isError);
            biData.AddData("startup", state);
            biData.AddData("time", System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            if (isError)
            {
                Joy.Debug.LogError(errMsg);
                biData.AddData("error", errMsg);
            }
            Joy.SDK.BIManager.Instance.LogGameBI(BI_TINKER_LOG, biData);
        }

        static public void ShowMessagePopup(string title, string content, string confirmTxt, string cancelTxt, System.Action confirm, System.Action cancel = null, System.Action close = null)
        {

        }
    }
}
