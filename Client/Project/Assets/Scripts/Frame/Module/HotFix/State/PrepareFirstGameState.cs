using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Huge.FSM;

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
                FirstGameState state = await CheckFirstGameState();
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
                    TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, "无法进入游戏");
                }
            }
            catch (Exception ex)
            {
                TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        async UniTask<FirstGameState> CheckFirstGameState()
        {
            FirstGameState state = FirstGameState.None;
            if (Huge.Utils.FileUtility.IsPresistentFileExists(TinkerConst.VersionFile))
            {
                string localJson = await Huge.Utils.FileUtility.LoadStreamingFileByText(TinkerConst.VersionFile);
                string remoteJson = await Huge.Utils.FileUtility.LoadPresistentFileByText(TinkerConst.VersionFile);
                if (localJson != null)
                {
                    TinkerConfig localConfig = JsonUtility.FromJson<TinkerConfig>(localJson);
                    TinkerConfig remoteConfig = JsonUtility.FromJson<TinkerConfig>(remoteJson);
                    string[] localVersions = localConfig.Version.Split(".");
                    string[] remoteVersions = remoteConfig.Version.Split(".");
                    if (localVersions.Length == 4 && remoteVersions.Length == 4)
                    {
                        try
                        {
                            if (IsCleanPresistent(localVersions, remoteVersions))
                            {
                                state = FirstGameState.FirstEnterGame;
                            }
                            else
                            {
                                state = FirstGameState.EnterGame;
                            }
                        }
                        catch
                        {
                            string message = $"config parse error: localConfig = {localJson}, remoteConfig = {remoteJson}";
                            TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, message);
                        }
                    }
                    else
                    {
                        string message = $"config parse error: localConfig = {localJson}, remoteConfig = {remoteJson}";
                        TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, message);
                    }
                }
                else
                {
                    string message = $"load local config file error: fileName = {TinkerConst.VersionFile}";
                    TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, message);
                }
            }
            else
            {
                state = FirstGameState.FirstEnterGame;
            }
            return state;
        }

        bool IsCleanPresistent(string[] localVersions, string[] remoteVersions)
        {
            int localBigVersion = int.Parse(localVersions[0]);
            int localForceUpdateVersion = int.Parse(localVersions[1]);
            int localHotUpdateVersion = int.Parse(localVersions[2]);
            int localSmallVersion = int.Parse(localVersions[3]);

            int remoteBigVersion = int.Parse(remoteVersions[0]);
            int remoteForceUpdateVersion = int.Parse(remoteVersions[1]);
            int remoteHotUpdateVersion = int.Parse(remoteVersions[2]);
            int remoteSmallVersion = int.Parse(remoteVersions[3]);

            if (remoteBigVersion >= localBigVersion)
            {
                if (remoteForceUpdateVersion >= localForceUpdateVersion)
                {
                    if (remoteHotUpdateVersion >= localHotUpdateVersion)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        void EnterGame(FSMContent content)
        {
            TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, "success");
            content.ChangeState<CheckRemoteState>();
        }

        async UniTask FirstEnterGame(FSMContent content)
        {
            //首次进入游戏清理缓存
            ClearPresistentCache();

            //首次进入游戏解包
            await CopyStreamingAssetsToPresistentCache();
        }

        void ClearPresistentCache()
        {
            string resDir = Huge.Utils.PathUtility.GetPresistentDataFullPath(TinkerConst.ResDir);
            Huge.Utils.FileUtility.DeleteExistDirectory(resDir);
            string downloadDir = Huge.Utils.PathUtility.GetPresistentDataFullPath(TinkerConst.DownloadDir);
            Huge.Utils.FileUtility.DeleteExistDirectory(downloadDir);
            string versionFullPath = Huge.Utils.PathUtility.GetStreamingDataFullPath(TinkerConst.VersionFile);
            Huge.Utils.FileUtility.DeleteExistDirectory(versionFullPath);

            Huge.Utils.FileUtility.CreateDirectoryIfNotExist(resDir);
            Huge.Utils.FileUtility.CreateDirectoryIfNotExist(downloadDir);
        }

        async UniTask CopyStreamingAssetsToPresistentCache()
        {
            await Huge.Utils.FileUtility.CopyStreamingFileToPresistentDir(TinkerConst.VersionFile, TinkerConst.VersionFile);
        }
    }
}
