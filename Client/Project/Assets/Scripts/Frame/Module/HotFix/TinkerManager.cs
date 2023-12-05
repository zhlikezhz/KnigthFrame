using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Huge.FSM;
using Huge;

namespace Huge.HotFix
{
    public class TinkerManager : FSMContent
    {
        float m_fProgress = 0.0f;
        public float Progress
        {
            get { return m_fProgress; }
            set { m_fProgress = value; }
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
    }
}
