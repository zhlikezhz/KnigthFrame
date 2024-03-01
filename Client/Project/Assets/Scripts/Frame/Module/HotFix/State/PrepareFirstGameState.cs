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
        TinkerVersion m_CacheConfig;
        TinkerVersion m_StreamConfig;
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
                    TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, "无法进入游戏");
                }
            }
            catch (Exception ex)
            {
                TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, ex.Message);
                Huge.Debug.LogException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        async UniTask<FirstGameState> CheckFirstGameState(FSMContent content)
        {
            FirstGameState state = FirstGameState.None;
            if (Huge.Utils.FileUtility.IsPresistentFileExists(TinkerConst.VersionFile))
            {
                string localJson = await Huge.Utils.FileUtility.LoadStreamingFileByText(TinkerConst.VersionFile);
                if (localJson != null)
                {
                    m_StreamConfig = new TinkerVersion(localJson);

                    string cacheJson = await Huge.Utils.FileUtility.LoadPresistentFileByText(TinkerConst.VersionFile);
                    m_CacheConfig = new TinkerVersion(cacheJson);

                    //覆盖安装的时候，如果本地版本更大则进入首次安装逻辑
                    if (IsCleanPresistent(m_StreamConfig, m_CacheConfig))
                    {
                        state = FirstGameState.FirstEnterGame;
                    }
                    else
                    {
                        state = FirstGameState.EnterGame;
                    }
                }
                else
                {
                    string message = $"load local config file error: fileName = {TinkerConst.VersionFile}";
                    TinkerManager.LogGameBI(false, TinkerState.PrepareFirstGame, message);
                    Huge.Debug.LogError(message);
                }
            }
            else
            {
                state = FirstGameState.FirstEnterGame;
            }
            return state;
        }

        bool IsCleanPresistent(TinkerVersion streamConfig, TinkerVersion cacheConfig)
        {
            if (cacheConfig.BigVersion >= streamConfig.BigVersion)
            {
                if (cacheConfig.ForceUpdateVersion >= streamConfig.ForceUpdateVersion)
                {
                    if (cacheConfig.HotUpdateVersion >= streamConfig.HotUpdateVersion)
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
            if (m_StreamConfig != null && m_StreamConfig.Config.IsOpenHotFix)
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
