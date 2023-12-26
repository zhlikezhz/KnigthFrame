using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Huge.Editor.Build
{
    public class BuildiOS : Builder
    {
        protected override string GetBuildLocation()
        {
            return $"{BuildPackage.ProjectRootDir}/xcode";
        }
    }
}