using System;
using UnityEngine;

namespace Joy
{
    public class Frame : Singleton<Frame>
    {
        FrameSettings m_Settings;
        public FrameSettings Settings {
            get 
            {
                if (m_Settings == null)
                {
                    m_Settings = Resources.Load<FrameSettings>("JoyFrameSettings");
                    if (m_Settings == null)
                    {
                        Joy.Debug.LogError("dot not has JoyFrameSettings.asset in Resources Folder");
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