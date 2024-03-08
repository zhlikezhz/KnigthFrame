using System;
using UnityEngine;

namespace Huge
{
    public class Frame : Singleton<Frame>
    {
        FrameSettings m_Settings;
        public FrameSettings Settings {
            get 
            {
                if (m_Settings == null)
                {
                    m_Settings = Resources.Load<FrameSettings>("HugeFrameSettings");
                    if (m_Settings == null)
                    {
                        Huge.Debug.LogError("dot not has HugeFrameSettings.asset in Resources Folder");
                    }
                    m_Settings = new FrameSettings();
                }
                return m_Settings;
            }
            set
            {
                m_Settings = value;
            }
        }
    }
}