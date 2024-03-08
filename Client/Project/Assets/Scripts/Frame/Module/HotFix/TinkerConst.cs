using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Huge.HotFix
{
    public static class TinkerConst
    {
        public static readonly string PatchDLLFileName = "hotupdate.dll";
        public static readonly string PatchPackageFileName = "res.zip";
        public static readonly string PatchVersionFileName = "version.txt";

        public static readonly string ResDir = "res";
        public static readonly string ScriptDir = "script";
        public static readonly string DownloadDir = "downloads";
        public static readonly string VersionFile = "version.txt";
        public static readonly string AppFootPrintFile = "footprint.txt";
        public static readonly string HybridCLRFile = "hotupdate.dll";
    }
}
