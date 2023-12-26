using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

namespace Huge.Editor.Build
{
    public static class BuildPackage
    {
        public static string ProjectRootDir = Application.dataPath.Replace("\\", "/").Replace("/Assets", "");

        [MenuItem("Build/BuildiOS", false, 1)]
        public static void BuildiOSAssetBundle()
        {
            BuildConfig buildConfig = new BuildConfig();
            buildConfig.IsDevelopment = true;
            buildConfig.IsForceRebuild = true;
            buildConfig.UseIL2CPP = false;
            buildConfig.Platform = "iOS";
            BuildPackagetTarget(BuildTarget.iOS, buildConfig);
        }

        [MenuItem("Build/BuildAndroid", false, 2)]
        public static void BuildAndroidAssetBunlde()
        {
            BuildConfig buildConfig = new BuildConfig();
            buildConfig.IsDevelopment = true;
            buildConfig.IsForceRebuild = true;
            buildConfig.UseIL2CPP = false;
            buildConfig.Platform = "Android";
            BuildPackagetTarget(BuildTarget.iOS, buildConfig);
        }

        public static void Build()
        {
            BuildConfig buildConfig = BuildUtility.ParseArgument("Huge.Editor.Build.BuildPackage.Build");
            if (buildConfig.Platform == "Android")
            {
                SwitchEnviroment(BuildTargetGroup.Android, BuildTarget.Android);
                BuildPackagetTarget(BuildTarget.Android, buildConfig);
            }
            else if (buildConfig.Platform == "iOS")
            {
                SwitchEnviroment(BuildTargetGroup.iOS, BuildTarget.iOS);
                BuildPackagetTarget(BuildTarget.iOS, buildConfig);
            }
            else
            {
                UnityEngine.Debug.LogError($"error:  invaild platform [{buildConfig.Platform}]");
            }
        }

        public static void BuildPackagetTarget(BuildTarget target, BuildConfig buildConfig)
        {
            try
            {
                BuildAssetBundle(target, buildConfig);
                BuildProject(target, buildConfig);
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogError($"error: build package fail {ex.Message}");
            }
            finally
            {
                ClearCache();
            }
        }

        public static void SwitchEnviroment(BuildTargetGroup group, BuildTarget target)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
        }

        public static void BuildAssetBundle(BuildTarget target, BuildConfig buildConfig)
        {
            BuildParameters buildParam = new BuildParameters();
            buildParam.BuildOutputRoot = Path.Combine(ProjectRootDir, BuildConst.OutputPath);
            buildParam.StreamingAssetsRoot = $"{Application.dataPath}/StreamingAssets/{YooAsset.YooAssetSettings.DefaultYooFolderName}/";
            buildParam.BuildTarget = target;
            //https://www.yooasset.com/docs/api/YooAsset.Editor/EBuildPipeline
            buildParam.BuildPipeline = BuildConst.IsBuildinPipline ? EBuildPipeline.BuiltinBuildPipeline : EBuildPipeline.ScriptableBuildPipeline;
            //https://www.yooasset.com/docs/api/YooAsset.Editor/EBuildMode
            buildParam.BuildMode = buildConfig.IsForceRebuild ? EBuildMode.ForceRebuild : EBuildMode.IncrementalBuild;
            buildParam.PackageName = BuildConst.AssetPackageName;
            DateTime now = DateTime.Now;
            buildParam.PackageVersion = now.ToString("yyyy_MM_dd_HH_mm_ss"); //buildConfig.Version;
            buildParam.VerifyBuildingResult = true;
            buildParam.SharedPackRule = new ZeroRedundancySharedPackRule();
            buildParam.CompressOption = ECompressOption.LZ4;
            //https://www.yooasset.com/docs/api/YooAsset.Editor/EOutputNameStyle
            buildParam.OutputNameStyle = EOutputNameStyle.HashName;
            buildParam.CopyBuildinFileOption = ECopyBuildinFileOption.ClearAndCopyAll;

            AssetBundleBuilder builder = new AssetBundleBuilder();
            var buildResult = builder.Run(buildParam);
            if (buildResult.Success)
            {
                UnityEngine.Debug.Log($"build package success {buildResult.OutputPackageDirectory}");
            }
            else
            {
                throw new Exception($"error: build package fail {buildResult.ErrorInfo}");
            }
        }

        public static void BuildProject(BuildTarget target, BuildConfig buildConfig)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    BuildAndroid android = new BuildAndroid();
                    android.BuildProject(buildConfig);
                    break;
                case BuildTarget.iOS:
                    BuildiOS ios = new BuildiOS();
                    ios.BuildProject(buildConfig);
                    break;
                default:
                    throw new Exception($"error: invaild platform {target}");
            }
        }

        public static void ClearCache()
        {

        }
    }
}