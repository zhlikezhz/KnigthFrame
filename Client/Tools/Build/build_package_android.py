import os
import sys
import argparse
import build_utils as BuildUtils

def main():
    try:
        parser = argparse.ArgumentParser(description="build package")
        parser.add_argument("--ProjectDir",type=str,required=True,dest="projectDir",help="项目工程目录")
        parser.add_argument("--Version",type=str,required=False,dest="version",help="游戏版本号")
        parser.add_argument("--AppName",type=str,required=False,dest="appName",help="App Name")
        parser.add_argument("--AppID",type=str,required=False,dest="appID",help="App ID")
        parser.add_argument("--IsDevelopment",type=bool,required=False,default=True,dest="IsDevelopment",help="Development打包模式")
        parser.add_argument("--UseIL2cpp",type=bool,required=False,default=False,dest="UseIL2cpp",help="使用IL2cpp")
        args = parser.parse_args()
    except:
        print("error: 打包参数解析失败")
        return -1

    pass

if __name__ == "__main__":
    main()