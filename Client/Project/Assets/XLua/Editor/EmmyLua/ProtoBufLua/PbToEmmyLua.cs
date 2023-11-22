using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace ProtoBufToLuaAPI
{
    public class PbToEmmyLua : Editor
    {

        public static string PB_PROTOBUFF_FOLDER_PATH_KEY = "PB_PROTOBUFF_FOLDER_PATH_KEY";

        private static bool isCurEnum = false;
        private static List<string> curClsName = new List<string>();
        //protobuf文件夹的存放路径，根据需求来更改
        private static string defaultPbPath = Application.dataPath + "/../baloot_hall_proto/proto/";

        private static string emPbDirecPath = Application.dataPath + "/../~luaTip/API_Proto";

        [MenuItem("XLua/PB生成lua", false, 200)]
        public static void UpdatePBFromSVN()
        {
            string inPath = EditorUserSettings.GetConfigValue(PB_PROTOBUFF_FOLDER_PATH_KEY);
            Debug.Log(" save protobuf Path：" + inPath);
            var exitsFold = Directory.Exists(inPath);
            Debug.Log("exitsFold:"+exitsFold);
            if (string.IsNullOrEmpty(inPath) || !exitsFold)
            {
                inPath = SetProtoBufPath();
            }
          //  SetProtoBufPath();

            
            var files = Directory.GetFiles(inPath, "*.proto", SearchOption.AllDirectories);
            ProtoBufToLua.messageIdDic = new Dictionary<string, string>();
            ProtoBufToLua.pbFileList = new List<string>();
            foreach (var item in files)
            {
                ProtoBufToLua pb = new ProtoBufToLua();
                pb.Init(item, emPbDirecPath);
            }
            ProtoBufToLua.WriteAllMessageIdDic();
            ProtoBufToLua.WriteAllProtoFiles();
        }

        //记录当前"程序文件/protobuf"的地址
        public static string SetProtoBufPath()
        {

            string path = "";
            //如果默认地址存在，则自动保存默认地址为protobuf地址
            if (Directory.Exists(defaultPbPath))
            {
                path = defaultPbPath;
            }
            else
            {
                path = EditorUtility.OpenFolderPanel("protobuf文件夹所在的本地目录", defaultPbPath, "");
            }
            if (!string.IsNullOrEmpty(path))
            {
                EditorUserSettings.SetConfigValue(PB_PROTOBUFF_FOLDER_PATH_KEY, path);
                Debug.Log("protobuf Path： " + path);
            }
            return path;
        }

        [MenuItem("XLua/设置pb文件夹", false, 300)]
        private static void resetProtoBufPath()
        {
            string path = "";
            path = EditorUtility.OpenFolderPanel("protobuf文件夹所在的本地目录", defaultPbPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                EditorUserSettings.SetConfigValue(PB_PROTOBUFF_FOLDER_PATH_KEY, path);
                Debug.Log("protobuf Path： " + path);
            }

        }

    }
}