using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huge.FSM;
using Huge.Utils;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

namespace Huge.HotFix
{
    public class CheckRemoteState : FSMState
    {
        public async override void OnEnter(FSMContent content)
        {
            try
            {
                string remoteJson = await UpdateVersionFile(content);
                TinkerVersion remoteConfig = new TinkerVersion(remoteJson);
                string cacheJson = await Huge.Utils.FileUtility.LoadPresistentFileByText(TinkerConst.VersionFile);
                TinkerVersion cacheConfig = new TinkerVersion(cacheJson);

                if (remoteConfig.BigVersion != cacheConfig.BigVersion || remoteConfig.ForceUpdateVersion != cacheConfig.ForceUpdateVersion)
                {
                    //强制更新包
                    UpdatePacakge();
                }
                else
                {
                    if (remoteConfig.HotUpdateVersion > cacheConfig.HotUpdateVersion)
                    {
                        //热更新
                        content.ChangeState<PatchGameState>();
                    }
                    else
                    {
                        //进入游戏
                        content.ChangeState<EnterGameState>();
                    }
                }
            }
            catch(Exception ex)
            {
                TinkerManager.LogGameBI(false, TinkerState.CheckRemoteFile, ex.ToString());
                Huge.Debug.LogError(ex.ToString());
            }
        }

        public override void OnLeave(FSMContent content)
        {
            base.OnLeave(content);
        }

        async UniTask<string> UpdateVersionFile(FSMContent content)
        {
            string versionURL = $"{Frame.Instance.Settings.ResCDN}{TinkerConst.VersionFile}";
            var handler = await HTTPUtility.Get(versionURL, 30);
            if (!handler.isError)
            {
                return handler.text;
            }
            else
            {
                string msg = $"download version file fail: {handler.msg}";
                TinkerManager.LogGameBI(false, TinkerState.CheckRemoteFile, msg);
                Huge.Debug.LogError(msg);
            }
            return null;
        }

        void UpdatePacakge()
        {

        }
    }
}