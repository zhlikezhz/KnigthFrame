using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Huge.FSM;
using Huge;

namespace Huge.HotFix
{
    public enum TinkerState
    {
        StartTinker             = 0, //��ʼ����
        PrepareFirstGame        = 1, //�״ν�����Ϸ����
        CheckRemoteFile         = 2, //���CDN
        DownloadPatch           = 3, //����Patch
        UnzipPatch              = 4, //��ѹPatch
        InstallPatch            = 5, //��װPatch
        EndTinker               = 6, //��������
    }

    public class TinkerManager : FSMContent
    {
        static readonly string BI_TINKER_LOG = "BITinker";
        static readonly Huge.SDK.BIData biData = new Huge.SDK.BIData();

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
                Huge.Debug.LogError(errMsg);
                biData.AddData("error", errMsg);
            }
            Huge.SDK.BIManager.Instance.LogGameBI(BI_TINKER_LOG, biData);
        }
    }
}
