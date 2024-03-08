using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Huge.FSM;
using System.IO;

namespace Huge.HotFix
{
    internal enum FirstGameState
    {
        None            = 0,
        EnterGame       = 1,
        FirstEnterGame  = 2,
    }

    internal class PrepareFirstGameState : FSMState
    {
        public override void OnEnter(FSMContent content)
        {
            ResetGame(content).Forget();
        }

        public override void OnLeave(FSMContent content)
        {
            base.OnLeave(content);
        }

        async UniTask ResetGame(FSMContent content)
        {
            try
            {
                FirstGameState state = await CheckFirstGameState(content);
                if (state == FirstGameState.EnterGame)
                {
                    EnterGame(content);
                }
                else if (state == FirstGameState.FirstEnterGame)
                {
                    await FirstEnterGame(content);
                }
                else
                {
                    TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, "enter game error");
                }
            }
            catch (Exception ex)
            {
                TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, ex.Message);
                Huge.Debug.LogError(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        async UniTask<FirstGameState> CheckFirstGameState(FSMContent content)
        {
            FirstGameState state = FirstGameState.None;
            if (Huge.Utils.FileUtility.IsPresistentFileExists(TinkerConst.AppFootPrintFile))
            {
                string footPrint = await Huge.Utils.FileUtility.LoadPresistentFileByText(TinkerConst.AppFootPrintFile);
                if (Application.buildGUID == footPrint)
                {
                    state = FirstGameState.EnterGame;
                }
                else
                {
                    state = FirstGameState.FirstEnterGame;
                }
            }
            else
            {
                state = FirstGameState.FirstEnterGame;
            }
            return state;
        }

        void EnterGame(FSMContent content)
        {
            TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, "success");
            if (Frame.Instance.Settings.IsHotUpdatePackage)
            {
                content.ChangeState<CheckRemoteState>();
            }
            else
            {
                content.ChangeState<EnterGameState>();
            }
        }

        async UniTask FirstEnterGame(FSMContent content)
        {
            //写入footprint
            await Huge.Utils.FileUtility.SavePresistentFileAsync(TinkerConst.AppFootPrintFile, Application.buildGUID);
            
            //首次进入游戏清理缓存
            ClearPresistentCache();

            //首次进入游戏解包
            await CopyStreamingAssetsToPresistentCache();

            EnterGame(content);
        }

        void ClearPresistentCache()
        {
            string resDir = Huge.Utils.PathUtility.GetPresistentDataFullPath(TinkerConst.ResDir);
            Huge.Utils.FileUtility.DeleteExistDirectory(resDir);
            string scriptDir = Huge.Utils.PathUtility.GetPresistentDataFullPath(TinkerConst.ScriptDir);
            Huge.Utils.FileUtility.DeleteExistDirectory(scriptDir);
            string downloadDir = Huge.Utils.PathUtility.GetPresistentDataFullPath(TinkerConst.DownloadDir);
            Huge.Utils.FileUtility.DeleteExistDirectory(downloadDir);
            string versionFullPath = Huge.Utils.PathUtility.GetStreamingDataFullPath(TinkerConst.VersionFile);
            Huge.Utils.FileUtility.DeleteExistDirectory(versionFullPath);

            Huge.Utils.FileUtility.CreateDirectoryIfNotExist(resDir);
            Huge.Utils.FileUtility.CreateDirectoryIfNotExist(scriptDir);
            Huge.Utils.FileUtility.CreateDirectoryIfNotExist(downloadDir);
        }

        async UniTask CopyStreamingAssetsToPresistentCache()
        {
            await Huge.Utils.FileUtility.CopyStreamingFileToPresistentDir(TinkerConst.VersionFile, TinkerConst.VersionFile);
            string hybridCLRFile = Path.Combine(TinkerConst.ScriptDir, TinkerConst.HybridCLRFile);
            await Huge.Utils.FileUtility.CopyStreamingFileToPresistentDir(TinkerConst.HybridCLRFile, hybridCLRFile);
        }
    }
}
