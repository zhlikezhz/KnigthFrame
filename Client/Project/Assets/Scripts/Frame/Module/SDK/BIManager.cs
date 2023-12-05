using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huge;

namespace SDK
{
    public class BIData
    {
        Dictionary<string, string> Data = new Dictionary<string, string>();
    }

    public class BIManager : Singleton<BIManager>
    {
        public void LogGameBI(string biType, string biJson)
        {

        }

        public void LogGameBI(string biType, BIData biData)
        {

        }
    }
}
