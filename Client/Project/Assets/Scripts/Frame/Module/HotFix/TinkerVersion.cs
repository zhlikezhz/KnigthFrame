using System;
using UnityEngine;


namespace Huge.HotFix
{
    public class TinkerVersion
    {
        public int BigVersion;
        public int ForceUpdateVersion;
        public int HotUpdateVersion;
        public int SmallVersion;
        public TinkerConfig Config;

        public TinkerVersion()
        {

        }

        public TinkerVersion(string json)
        {
            ParseVersionJson(json);
        }

        public void ParseVersionJson(string json)
        {
            try
            {
                Config = JsonUtility.FromJson<TinkerConfig>(json);
                string[] versions = Config.Version.Split(".");
                if (versions.Length == 4)
                {
                    BigVersion = int.Parse(versions[0]);
                    ForceUpdateVersion = int.Parse(versions[1]);
                    HotUpdateVersion = int.Parse(versions[2]);
                    SmallVersion = int.Parse(versions[3]);
                }
                else
                {
                    throw new Exception("config file format error.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}