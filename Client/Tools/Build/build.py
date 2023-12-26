import os
import sys
import argparse
import build_ios as BuildiOS
import build_android as BuildAndroid
import build_utils as BuildUtils

class Platform:
    iOS     = "iOS"
    Android = "Android"
    Windows = "Windows"

def main():
    parser = argparse.ArgumentParser(description="build package")
    parser.add_argument("--ProjectDir",type=str,required=True,dest="projectDir",help="项目工程目录")
    parser.add_argument("--Platform",type=str,required=True,dest="platform",help="打包平台")
    parser.add_argument("--Version",type=str,required=False,dest="version",help="游戏版本号")
    parser.add_argument("--AppName",type=str,required=False,dest="appName",help="App Name")
    parser.add_argument("--AppID",type=str,required=False,dest="appID",help="App ID")
    parser.add_argument("--IsForceRebuild",type=bool,required=False,default=False,dest="IsForceRebuild",help="重新构建包")
    parser.add_argument("--IsDevelopment",type=bool,required=False,default=True,dest="IsDevelopment",help="Development打包模式")
    parser.add_argument("--UseIL2cpp",type=bool,required=False,default=False,dest="UseIL2cpp",help="使用IL2cpp")
    args = parser.parse_args()

    pass

if __name__ == "__main__":
    main()