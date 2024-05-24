using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joy;

namespace Joy.SDK
{
    public class BIData    
    {
        Dictionary<string, object> Data = new Dictionary<string, object>();

        public void Clear()
        {
            Data.Clear();
        }

        public void AddData(string key, object value)
        {
            Data.Add(key, value);
        }

        public override string ToString()
        {
            return "";
        }
    }

    public class BIManager : Singleton<BIManager>
    {
        public void LogGameBI(string biType, string biJson)
        {
            Joy.Debug.LogError($"[BI][Type = {biType}]: msg = {biJson}");
        }

        public void LogGameBI(string biType, BIData biData)
        {
            LogGameBI(biType, biData.ToString());
        }
    }
}
