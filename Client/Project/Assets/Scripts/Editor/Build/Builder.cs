using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Huge.Editor.Build
{
    public class Builder
    {
        protected BuildConfig m_BuildConfig;

        protected virtual bool IsDevelopmentBuild()
        {
            return m_BuildConfig.IsDevelopment;
        }

        protected virtual bool IsAutoConnectProfiler()
        {
            return m_BuildConfig.IsAutoconnectProfiler;
        }

        protected virtual string[] GetScenePaths()
        {
            return new string[]
            {
                "Assets/Scenes/Main.unity",
            };
        }

        protected virtual string GetBuildLocation()
        {
            string packagePath = Path.Combine(BuildPackage.ProjectRootDir, BuildConst.OutputPath);
            return $"{packagePath}/{m_BuildConfig.AppName}.apk";
        }

        protected virtual BuildOptions GetBuildOptions()
        {
            BuildOptions buildOptions = BuildOptions.StrictMode;

            if (IsDevelopmentBuild())
            {
                buildOptions = buildOptions | BuildOptions.Development;
                if (IsAutoConnectProfiler())
                {
                    buildOptions = buildOptions | BuildOptions.ConnectWithProfiler;
                }
            }
            return buildOptions;
        }

        protected virtual string[] GetScriptDefineSymbols()
        {
            return new string[]
            {
                "UNITY_VISUAL_SCRIPTING",
                "OOTII_MD",
                "hq_ads_max",
                "yoo_asset",
                "hq_ads_ta_event",
                "hq_iap_v2",
                "hq_ta_event",
            };
        }

        public void BuildProject(BuildConfig buildConfig)
        {
            m_BuildConfig = buildConfig;

            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (buildConfig.UseIL2CPP)
            {
                PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.IL2CPP);
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7 | AndroidArchitecture.X86;
            }
            else
            {
                PlayerSettings.SetScriptingBackend(buildTargetGroup, ScriptingImplementation.Mono2x);
            }

            string buildLocation = GetBuildLocation();
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildReport buildReport = BuildPipeline.BuildPlayer(GetScenePaths(), buildLocation, buildTarget, GetBuildOptions());

            if (buildReport.summary.result != BuildResult.Succeeded)
            {
                string errorInfo = ParseBuildReportErrorMessage(buildReport);
                throw new Exception($"Build Failed: {errorInfo}");
            }
        }

        string ParseBuildReportErrorMessage(BuildReport report)
        {
            var sb = new StringBuilder();

            foreach (var step in report.steps)
            {
                string name = step.name;
                BuildStepMessage[] messages = step.messages;

                if (messages != null && messages.Length > 0)
                {
                    sb.Append(name).Append(" --> ");

                    foreach (var message in messages)
                    {
                        sb.Append(message.content);
                    }

                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }

        void SetScriptingSymbols()
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defineSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            UnityEngine.Debug.Log("symbol: " + defineSymbol);

            var symbols = new List<string>(defineSymbol.Split(";"));
            var addSymbols = GetScriptDefineSymbols();

            foreach(string addSymbol in addSymbols)
            {
                bool isAddSymbol = true;
                foreach(string symbol in symbols)
                {
                    if (addSymbol == symbol)
                    {
                        isAddSymbol = false;
                        break;
                    }
                }
                if (isAddSymbol)
                {
                    symbols.Add(addSymbol);
                }
            }
            defineSymbol = string.Join(";", symbols);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbol);
        }
    }
}
