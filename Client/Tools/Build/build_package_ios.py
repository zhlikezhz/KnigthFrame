import os
import sys
import datetime
import argparse
import subprocess
import build_utils as BuildUtils

def main():
    try:
        parser = argparse.ArgumentParser(description="build package")
        parser.add_argument("--UnityDir",type=str,required=True,dest="UnityDir",help="Unity目录")
        parser.add_argument("--ProjectDir",type=str,required=True,dest="ProjectDir",help="项目工程目录")
        parser.add_argument("--PackageName",type=str,required=True,default="None",dest="PackageName",help="应用名")
        parser.add_argument("--Version",type=str,required=True,dest="Version",help="游戏版本号")
        parser.add_argument("--TeamID",type=str,required=True,default="None",dest="TeamID",help="TeamID")
        parser.add_argument("--BundleID",type=str,required=True,default="None",dest="BundleID",help="com.xxx.xxx")
        parser.add_argument("--BundleVersion",type=str,required=False,default="0",dest="BundleVersion",help="苹果TestFlight测试编号")
        parser.add_argument("--UseIL2cpp",type=str,required=False,default="true",dest="UseIL2cpp",help="使用IL2cpp否则使用mono")
        parser.add_argument("--IsDevelopment",type=str,required=False,default="false",dest="IsDevelopment",help="Debug模式")
        parser.add_argument("--IsAutoconnectProfiler",type=str,required=False,default="false",dest="IsAutoconnectProfiler",help="AutoconnectProfiler")
        parser.add_argument("--IsForceRebuild",type=str,required=False,default="true",dest="IsForceRebuild",help="重新构建包")
        args = parser.parse_args()
    except Exception as ex:
        print("error: Unity打包参数解析失败, %s" % ex)
        exit(1)

    try:
        xcodePath = "%s/xcode" % (args.ProjectDir)

        cmdShell = "rm -rf %s" % (xcodePath)
        print(cmdShell)
        subprocess.run(cmdShell, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, universal_newlines=True)

        logFile = "%s/logs/build_%s_%s.txt" % (args.ProjectDir, "iOS", datetime.date.today())
        params = "Platform:%s Version:%s PackageName:'%s' BundleID:%s TeamID:%s BundleVersion:%s UseIL2cpp:%s IsDevelopment:%s IsAutoconnectProfiler:%s IsForceRebuild:%s" % ("iOS", args.Version, args.PackageName, args.BundleID, args.TeamID, args.BundleVersion, args.UseIL2cpp, args.IsDevelopment, args.IsAutoconnectProfiler, args.IsForceRebuild)
        cmdShell = "%s -quit -batchmode -nographics -projectPath %s -logFile %s -executeMethod Huge.Editor.Build.BuildPackage.Build %s" % (args.UnityDir, args.ProjectDir, logFile, params)
        print(cmdShell)
        result = subprocess.run(cmdShell, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, universal_newlines=True)
        if result.returncode != 0:
            print(f"input: ", result.stdout)
            print(f"error: ", result.stderr)
            print("error: Unity打包失败")
            print("build log file: %s" % (logFile))
            cmdShell = ("cat %s") % (logFile)
            subprocess.run(cmdShell, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, universal_newlines=True)
            exit(1)

        if os.path.isdir(xcodePath) == False:
            print("error: Unity打包失败")
            print("build log file: %s" % (logFile))
            cmdShell = ("cat %s") % (logFile)
            subprocess.run(cmdShell, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, universal_newlines=True)
            exit(1)
        
        exit(0)

    except Exception as ex:
        print("error: %s" % ex)
        exit(1)

if __name__ == "__main__":
    main()