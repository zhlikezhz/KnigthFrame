using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection.Emit;
using System.Drawing.Drawing2D;
using UnityEngine.UI;
using System.IO;

namespace Huge.MVVM
{
    public delegate void NodeGenerateFunc(StringBuilder data);
    public class UIAutoNode
    {
        public string Name;
        public string Path;
        public string ClassName;
        public Transform Trans;
        public UIAutoTree Tree;

        public static string s_ObjectPrefix = "_";
        public static string s_IngorePrefix = "_Igr";
        public static string s_BGPrefix = "_BG";
        public static string s_FullPrefix = "_Full";
        public static string s_TemplatePrefix = "_Stc";
        public static string s_ScrollPrefix = "_Scl";
        public static string s_LoopScrollPrefix = "_LScl";
        public static string s_ButtonPrefix = "_Btn";
        public static string[] s_PrefixList = 
        {
            "_Txt", 
            "_Img", 
            "_Tran",
            "_RTran",
            s_ButtonPrefix,
            "_Sld", 
            "_Ifd", 
            "_Tgl", 
            "_LBtn", 
            "_RImg",
            s_ScrollPrefix,
            s_LoopScrollPrefix,
        };

        public static Dictionary<string, string> s_Prefix2TypeName = new Dictionary<string, string> {
            {"_Txt", "Text"},
            {"_Img", "Image"},
            {"_Tran", "Transform"},
            {"_RTran", "RectTransform"},
            {"_Sld", "Slider"},
            {"_Ifd", "InputField"},
            {"_Tgl", "Toggle"},
            {"_LBtn", "LongButton"},
            {"_RImg", "RawImage"},
        };

        public void GenerateCode(StringBuilder data, bool isInit, string prefix)
        {
            foreach(var item in s_Prefix2TypeName)
            {
                if (Name.StartsWith(item.Key))
                {
                    GenerateComponent(data, isInit, prefix, item.Value);
                    return;
                }
            }

            if (Name.StartsWith(s_ButtonPrefix))
            {
                GenerateButton(data, isInit, prefix);
            }
            if (Name.StartsWith(s_ScrollPrefix))
            {
                GenerateScroll(data, isInit, prefix);
            }
            if (Name.StartsWith(s_LoopScrollPrefix))
            {
                GenerateLoopScroll(data, isInit, prefix);
            }
            if (Name.StartsWith(s_TemplatePrefix))
            {
                GenerateObject(data, isInit, prefix);
            }
        }
        
        public void GenerateObject(StringBuilder data, bool isInit, string prefix)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public GameObject {Name};");
            }
            else
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").gameObject;");
            }
        }

        public void GenerateComponent(StringBuilder data, bool isInit, string prefix, string compName)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public {compName} {Name};");
            }
            else
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<{compName}>();");
            }
        }

        public void GenerateButton(StringBuilder data, bool isInit, string prefix)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public Button {Name};");
            }
            else
            {
                string name = Name.Substring(UIAutoNode.s_ButtonPrefix.Length);
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<Button>();");
                data.AppendLine($"\t\t{prefix}{Name}.onClick.AddListener(OnClick{name});");
            }
        }

        public void GenerateScroll(StringBuilder data, bool isInit, string prefix)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public ScrollRect {Name};");
                data.AppendLine($"\t{prefix}public GameObject {Name}Content;");
            }
            else
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<ScrollRect>();");
                data.AppendLine($"\t\t{prefix}{Name}Content = {Name}.content.gameObject;");
            }
        }

        public void GenerateLoopScroll(StringBuilder data, bool isInit, string prefix)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public ScrollView {Name};");
                data.AppendLine($"\t{prefix}public GameObject {Name}Content;");
            }
            else
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<ScrollView>();");
                data.AppendLine($"\t\t{prefix}{Name}Content = {Name}.content.gameObject;");
            }
        }

        public void GenerateBG(StringBuilder data, bool isInit, string prefix)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public Transform {Name};");
            }
            else
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\");");
            }
        }

        public void GenerateFull(StringBuilder data, bool isInit, string prefix)
        {
            if (isInit)
            {
                data.AppendLine($"\t{prefix}public Transform {Name};");
            }
            else
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\")();");
            }
        }

        public void GenerateTemplate(StringBuilder data)
        {
            string name = Name.Substring(Name.IndexOf("_Stc"));
            data.AppendLine($"\tpublic class {ClassName}{name}Prefab : Prefab");
            data.AppendLine("\t{");
            data.AppendLine($"\t\tpublic {ClassName}{name}Prefab(GameObject gameObj) : base(gameObj) {{}}");
            data.AppendLine();
            Tree.GenerateCode(data, "\t");
            data.AppendLine("\t}");
            data.AppendLine();
            data.AppendLine($"\tpublic {ClassName}{name}Prefab Generate{name}Prefab(GameObject gameObj = null, Transform parent = null)");
            data.AppendLine("\t{");
            data.AppendLine($"\t\tif (gameObj == null) gameObj = UnityEngine.GameObject.Instantiate({Name});");
            data.AppendLine($"\t\tvar inst = new {ClassName}{name}Prefab(gameObj);");
            data.AppendLine("\t\tif (parent != null) inst.transform.SetParent(parent);");
            data.AppendLine("\t\treturn inst;");
            data.AppendLine("\t}");
        }
    }

    public class UIAutoTree
    {
        public List<UIAutoNode> NodeList = new List<UIAutoNode>();
        public List<UIAutoNode> BGNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> FullNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> ObjectNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> TemplateNodeList = new List<UIAutoNode>();

        public void GenerateCode(StringBuilder data, string prefix)
        {
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, true, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, true, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, true, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, true, prefix);
            }
            data.AppendLine();
            data.AppendLine($"\t{prefix}public override void OnCreate()");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, false, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, false, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, false, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, false, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            foreach(var node in NodeList)
            {
                if (node.Name.StartsWith(UIAutoNode.s_ButtonPrefix))
                {
                    string name = node.Name.Substring(UIAutoNode.s_ButtonPrefix.Length);
                    data.AppendLine($"\t{prefix}protected virtual void OnClick{name}() {{}}");
                    data.AppendLine();
                }
            }
        }
    }
}