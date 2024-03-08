using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using YooAsset;
using Huge.Utils;
using Huge.FSM;

namespace Huge.HotFix
{
    public class PatchGameState : FSMState
    {
        public override void OnEnter(FSMContent content)
        {
            StartUpdate(content).Forget();
        }

        public override void OnLeave(FSMContent content)
        {
            base.OnLeave(content);
        }

        async UniTask StartUpdate(FSMContent content)
        {
            try
            {
                await UpdateResources();
                await UpdateScripts();
                TinkerManager.LogGameBI(false, TinkerState.StartTinker, "success");
                content.ChangeState<EnterGameState>();
            }
            catch (Exception ex)
            {
                TinkerManager.LogGameBI(false, TinkerState.StartTinker, ex.Message);
                Huge.Debug.LogError($"update game resource error: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新YooAsset资源包
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="timeout"></param>
        /// <exception cref="Exception"></exception> <summary>
        async UniTask UpdateResources(IProgress<float> progress = null, int timeout = 60)
        {
            YooAssets.Initialize();
            ResourcePackage package = YooAssets.TryGetPackage(Frame.Instance.Settings.ResPackageName);
            if (package == null) package = YooAssets.CreatePackage(Frame.Instance.Settings.ResPackageName);
            YooAssets.SetDefaultPackage(package);

            var versionOperation = package.UpdatePackageVersionAsync(true, timeout);
            await versionOperation.ToUniTask();
            if (versionOperation.Status != EOperationStatus.Succeed)
            {
                throw new Exception(versionOperation.Error);
            }

            var manifestOperation = package.UpdatePackageManifestAsync(versionOperation.PackageVersion, true, timeout);
            await manifestOperation.ToUniTask();
            if (manifestOperation.Status != EOperationStatus.Succeed)
            {
                throw new Exception(manifestOperation.Error);
            }

            int failedTryAgain = 3;
            int downloadingMaxNum = 10;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain, timeout);
            if (downloader.TotalDownloadCount > 0)
            {
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                string totalDownloadSize = MathUtility.ByteToMemorySize(totalDownloadBytes);

                downloader.BeginDownload();
                progress = Progress.Create<float>(f => {UnityEngine.Debug.Log($"update resources progress: {f} ");});
                await downloader.ToUniTask(progress, PlayerLoopTiming.Update);
                if (downloader.Status != EOperationStatus.Succeed)
                {
                    throw new Exception(downloader.Error);
                }
            }
            progress?.Report(1.0f);
        }

        async UniTask UpdateScripts()
        {
            string url = Frame.Instance.Settings.ResCDN;
            string fileName = System.IO.Path.Combine(TinkerConst.ScriptDir, TinkerConst.HybridCLRFile);
            string fullPath = PathUtility.GetPresistentDataFullPath(fileName);
            await DownloadUtility.Download(url, fullPath, 60);
        }
    }
}
