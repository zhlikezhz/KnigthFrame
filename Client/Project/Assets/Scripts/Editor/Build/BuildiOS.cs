using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Huge.Editor.Build
{
    public class BuildiOS : Builder
    {
        protected override string GetBuildLocation(BuildConfig buildConfig)
        {
            return $"{BuildPackage.ProjectRootDir}/xcode";
        }

        public override void BuildProject(BuildConfig buildConfig)
        {
            PlayerSettings.iOS.buildNumber = buildConfig.BundleVersion;
            PlayerSettings.iOS.appleDeveloperTeamID = buildConfig.TeamID;
            PlayerSettings.iOS.appleEnableAutomaticSigning = true;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, buildConfig.BundleID);
            base.BuildProject(buildConfig);
        }
    }
}