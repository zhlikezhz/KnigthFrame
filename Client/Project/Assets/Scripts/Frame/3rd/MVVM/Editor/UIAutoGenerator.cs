using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Huge.MVVM.Editor
{
    public class UIAutoGenerator
    {
        [MenuItem("Assets/UI Prefab Code Generater")]
        public static void GeneratePrefabCode()
        {
            GenerateCode(Selection.activeObject);
            EditorUtility.DisplayDialog("完成", "生成代码成功", "确定");
        }

        public static string s_ScriptDirPath = "Scripts/HotUpdate/Runtime/UI/AutoGenerate";
        static string m_strPrefabPath = "";
        static string m_strScriptName = "";
        static string m_strScriptFullPath = "";
        private static void GenerateCode(UnityEngine.Object obj)
        {
            //对应path
            m_strPrefabPath = AssetDatabase.GetAssetPath(obj);
            m_strScriptName = Path.GetFileNameWithoutExtension(m_strPrefabPath);
            string dirPath = Path.GetDirectoryName(m_strPrefabPath);
            m_strScriptFullPath = $"{s_ScriptDirPath}/{m_strScriptName}Generate.cs";
            m_strScriptFullPath = Path.Combine(Application.dataPath, m_strScriptFullPath);
            
            try
            {
                FileInfo file = new System.IO.FileInfo(m_strScriptFullPath);
                file.Directory.Create(); 
                CheckRoot((obj as GameObject).transform);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"ui generate error: {ex.Message}.");
            }
            AssetDatabase.Refresh();
        }

        static StringBuilder m_DstText;
        static void CheckRoot(Transform root)
        {
            UIAutoTree tree = new UIAutoTree();
            CheckChild("", tree, root);

            m_DstText = new StringBuilder();
            m_DstText.AppendLine("//------------------------------------------------------------------------------");
            m_DstText.AppendLine("//--  <autogenerated>");
            m_DstText.AppendLine("//--      This code was generated by a tool.");
            m_DstText.AppendLine("//--      Changes to this file may cause incorrect behavior and will be lost if");
            m_DstText.AppendLine("//--      the code is regenerated.");
            m_DstText.AppendLine("//--  </autogenerated>");
            m_DstText.AppendLine("//------------------------------------------------------------------------------");
            m_DstText.AppendLine();
            m_DstText.AppendLine("using Huge.MVVM;");
            m_DstText.AppendLine("using Huge.MVVM.DataBinding;");
            m_DstText.AppendLine("using UnityEngine;");
            m_DstText.AppendLine("using UnityEngine.UI;");
            m_DstText.AppendLine();
            m_DstText.AppendLine($"[ViewSettingAttribute(\"{m_strPrefabPath}\")]");
            tree.GenerateView(m_strScriptName, m_DstText, "");
            tree.GenerateViewModel(m_strScriptName, m_DstText, "");
            foreach(var template in tree.TemplateNodeList)
            {
                template.GenerateTemplate(m_DstText);
            }
            foreach(var template in tree.ScrollNodeList)
            {
                template.GenerateScrollItemView(m_DstText);
            }
            File.WriteAllText(m_strScriptFullPath, m_DstText.ToString());
        }

        static void CheckChild(string path, UIAutoTree tree, Transform parent)
        {
            Transform[] children = parent.GetChildren(); 
            foreach(Transform child in children)
            {
                string name = child.gameObject.name;
                if (name.StartsWith(UIAutoNode.s_IngorePrefix))
                {
                    continue;
                }

                UIAutoNode node = new UIAutoNode();
                node.ClassName = m_strScriptName;
                node.Trans = child;
                node.Name = name;
                int index = name.IndexOf(UIAutoNode.s_ObjectPrefix);
                if (0 <= index && index < name.Length)
                {
                    node.PropertyName = name.Substring(index+1);
                }
                else
                {
                    node.PropertyName = name;
                }

                if (path.Length > 0)
                {
                    node.Path = $"{path}/{name}";
                }
                else
                {
                    node.Path = name;
                }

                bool bFound = false;
                foreach(string prefix in UIAutoNode.s_PrefixList)
                {
                    if (name.StartsWith(prefix))
                    {
                        tree.NodeList.Add(node);
                        CheckChild(node.Path, tree, child);
                        bFound = true;
                        break;
                    }
                }

                if (bFound == false)
                {
                    if (name.StartsWith(UIAutoNode.s_BGPrefix))
                    {
                        tree.BGNodeList.Add(node);
                        CheckChild(node.Path, tree, child);
                    }
                    else if (name.StartsWith(UIAutoNode.s_FullPrefix))
                    {
                        tree.FullNodeList.Add(node);
                        CheckChild(node.Path, tree, child);
                    }
                    else if (name.StartsWith(UIAutoNode.s_TemplatePrefix))
                    {
                        node.ClassName = $"{m_strScriptName}{name.Substring(UIAutoNode.s_TemplatePrefix.Length)}";
                        tree.NodeList.Add(node);
                        tree.TemplateNodeList.Add(node);
                        node.Tree = new UIAutoTree();
                        CheckChild("", node.Tree, child);
                    }
                    else if (name.StartsWith(UIAutoNode.s_ScrollPrefix))
                    {
                        tree.NodeList.Add(node);

                        UIAutoNode templateNode = new UIAutoNode();
                        templateNode.Tree = new UIAutoTree();
                        templateNode.ClassName = $"{m_strScriptName}{name.Substring(UIAutoNode.s_ScrollPrefix.Length)}";
                        templateNode.Trans = child.Find("Template");
                        templateNode.Name = name;
                        templateNode.Path = "";
                        tree.ScrollNodeList.Add(templateNode);
                        CheckChild("", templateNode.Tree, templateNode.Trans);
                    }
                    else if (name.StartsWith(UIAutoNode.s_ObjectPrefix))
                    {
                        tree.ObjectNodeList.Add(node);
                        CheckChild(node.Path, tree, child);
                    }
                    else
                    {
                        CheckChild(node.Path, tree, child);
                    }
                }
            }
        }
    }
}