using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Joy.Editor.Build
{
    public static class BuildChannel
    {
        public readonly static string Dev = "Dev";
        public readonly static string AppleStore = "AppStore";
        public readonly static string GooglePlay = "GooglePlay";
    }

    public class BuildConfig
    {
        public string CompanyName = "hugesoft";
        public string PackageName = "Anime Avatar";
        public string Version = "1.0.0";
        public string BundleVersion = "1";
        public string TeamID = "DFURNZYWTV";
        public string BundleID = "com.hugesoft.avatar";
        public string Platform = "";
        public string BuildOutputPath = "";
        public string ChannelID = BuildChannel.Dev;

        public bool UseIL2CPP = false;
        public bool IsDevelopment = false;
        public bool IsAutoconnectProfiler = false;
        public bool IsForceRebuild = false;
        public bool IsUploadBundle = false;

        public override string ToString()
        {
            return "CompanyName:" + CompanyName + 
                   ", PackageName:" + PackageName +
                   ", Version:" + Version +
                   ", BundleVersion:" + BundleVersion +
                   ", TeamID:" + TeamID +
                   ", BundleID:" + BundleID +
                   ", Platform:" + Platform +
                   ", UseIL2CPP:" + UseIL2CPP +
                   ", ChannelID:" + ChannelID +
                   ", IsDevelopment:" + IsDevelopment +
                   ", IsForceRebuild:" + IsForceRebuild +
                   ", IsUploadBundle:" + IsUploadBundle +
                   ", BuildOutputPath:" + BuildOutputPath +
                   ", IsAutoconnectProfiler:" + IsAutoconnectProfiler;
        }
    }
}