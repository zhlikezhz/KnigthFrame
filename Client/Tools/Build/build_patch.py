import os
import sys
import argparse
import datetime
import build_utils as BuildUtils
import build_package_ios as BuildiOS
import build_package_android as BuildAndroid

class Platform:
    iOS     = "iOS"
    Android = "Android"
    Windows = "Windows"

def main():
    try:
        parser = argparse.ArgumentParser(description="build package")
        parser.add_argument("--UnityDir",type=str,required=True,dest="unityDir",help="Unity目录")
        parser.add_argument("--ProjectDir",type=str,required=True,dest="projectDir",help="项目工程目录")
        parser.add_argument("--Platform",type=str,required=True,dest="platform",help="打包平台")
        parser.add_argument("--Version",type=str,required=True,dest="version",help="游戏版本号")
        parser.add_argument("--IsForceRebuild",type=bool,required=False,default=False,dest="isForceRebuild",help="重新构建包")
        args = parser.parse_args()
    except:
        print("error: Unity资源打包参数解析失败")
        return -1

    try:
        logFile = "%s/logs/build_patch_%s_%s.txt" % (args.projectDir, args.platform, datetime.date.today())
        params = "Huge.Editor.Build.BuildPackage.BuildPatch -Platform:%s -Version:%s -IsForceRebuild:%s" % (args.platform, args.version, args.isForceRebuild)
        if subprocess.call([args.unityDir, "-batchmode", "-nographics", "-projectPath", args.projectDir, "-logFile", logFile, "-executeMethod", params, "-quit"]) != 0:
            print("build log file: %s" % (logFile))
            print("error: Unity资源打包失败")
            return -1
        else:
            print("build log file: %s" % (logFile))
            return 0
    except:
        print("error: Unity资源打包执行失败")
        return -1

if __name__ == "__main__":
    main()