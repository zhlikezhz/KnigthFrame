using UnityEngine;
using UnityEditor;

namespace Huge.Editor.Build
{
    public static class BuildUtility
    {
        public static BuildConfig ParseArgument(string targetBuildFuncName)
        {
            BuildConfig param = new BuildConfig();

            int debugArgsIndx = -1;
            string[] args = System.Environment.GetCommandLineArgs();
            if (args.Length == 0)
            {
                return param;
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], targetBuildFuncName))
                {
                    debugArgsIndx = i + 1;
                    break;
                }
            }

            if (debugArgsIndx >= args.Length || debugArgsIndx == -1)
            {
                return param;
            }
            for (int i = debugArgsIndx; i < args.Length; ++i)
            {
                string[] debugArr = args[i].Split(':');
                if (string.IsNullOrEmpty(debugArr[0]))
                {
                    continue;
                }

                if (string.Equals(debugArr[0], "AppID"))
                {
                    param.AppID = debugArr[1];
                }
                else if (string.Equals(debugArr[0], "AppName"))
                {
                    param.AppName = debugArr[1];
                }
                else if (string.Equals(debugArr[0], "Platform"))
                {
                    param.Platform = debugArr[1];
                }
                else if (string.Equals(debugArr[0], "Version"))
                {
                    param.Version = debugArr[1];
                }
                else if (string.Equals(debugArr[0], "UseIL2cpp"))
                {
                    param.UseIL2CPP = bool.Parse(debugArr[1]);
                }
                else if (string.Equals(debugArr[0], "IsDevelopment"))
                {
                    param.IsDevelopment = bool.Parse(debugArr[1]);
                }
                else if (string.Equals(debugArr[0], "IsForceRebuild"))
                {
                    param.IsForceRebuild = bool.Parse(debugArr[1]);
                }
                else if (string.Equals(debugArr[0], "IsAutoconnectProfiler"))
                {
                    param.IsAutoconnectProfiler = bool.Parse(debugArr[1]);
                }
                else if (string.Equals(debugArr[0], "BuildOutputPath"))
                {
                    param.BuildOutputPath = debugArr[1];
                }
            }
            UnityEngine.Debug.LogFormat(param.ToString());
            return param;
        }
    }
}