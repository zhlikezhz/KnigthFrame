using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huge.Utils
{
    // 网络类型定义
    public enum EnumNetworkType
    {
        None = 0,
        Wifi = 1,
        Mobile = 2,
        MobileUnknown = 3,
        MobileEdge = 4,
        Mobile3G = 5,
        Mobile4G = 6
    }

    public static class NativeUtility
    {
        /// <summary>
        /// 网络是否可达
        /// </summary>
        /// <returns></returns>
        public static bool IsInternetReachability()
        {
            if (GetNetworkInfo() == EnumNetworkType.None)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 只返回简单的网络状态
        /// </summary>
        /// <returns>None,Wifi,Mobile</returns>
        public static EnumNetworkType GetNetworkInfo()
        {
            var reachabbility = Application.internetReachability;
            switch (reachabbility)
            {
                case NetworkReachability.NotReachable:
                    return EnumNetworkType.None;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return EnumNetworkType.Wifi;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return EnumNetworkType.Mobile;
                default:
                    return EnumNetworkType.None;
            }
        }
    }
}
