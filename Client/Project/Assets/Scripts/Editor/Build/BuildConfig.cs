using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Huge.Editor.Build
{
    public class BuildConfig
    {
        public string AppID = "Avatar";
        public string AppName = "com.hugesoft.avatar";
        public string Version = "1.0.0";
        public string Platform = "";
        public string BuildOutputPath = "";

        public bool UseIL2CPP = false;
        public bool IsDevelopment = false;
        public bool IsAutoconnectProfiler = false;
        public bool IsForceRebuild = false;

        public override string ToString()
        {
            return "AppID:" + AppID +
                   ", AppName:" + AppName +
                   ", Version:" + Version +
                   ", Platform:" + Platform +
                   ", UseIL2CPP:" + UseIL2CPP +
                   ", IsDevelopment:" + IsDevelopment +
                   ", IsForceRebuild:" + IsForceRebuild +
                   ", BuildOutputPath:" + BuildOutputPath +
                   ", IsAutoconnectProfiler:" + IsAutoconnectProfiler;
        }
    }
}