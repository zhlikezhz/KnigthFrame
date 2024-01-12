#if UNITY_IOS
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;

namespace Huge.Editor.Build
{
    public static class XCodeSetting
    {
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            SetPlistFile("info.plist");
            SetProjectProperty("Unity-iPhone.xcworkspace");
        }

        public static void SetPlistFile(string plistPath)
        {
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            PlistElementDict rootDict = plist.root;

            plist.WriteToFile(plistPath);
        }

        public static void SetProjectProperty(string projectFilePath)
        {
            PBXProject project = new PBXProject();
            project.ReadFromFile(projectFilePath);
            string target = project.GetUnityMainTargetGuid();
            string framework = project.GetUnityFrameworkTargetGuid();
            project.WriteToFile(projectFilePath);
        }

        static Dictionary<string, string> AddFrameworks = new Dictionary<string, string>
        {

        };

        static Dictionary<string, string> ModifyBuildProperty = new Dictionary<string, string>
        {

        };
    }
}
#endif