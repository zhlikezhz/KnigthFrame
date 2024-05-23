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
    public enum UIGenType
    {
        MemberDefine,
        FindObject,
        BindObject,
        DataMemberDefine,
        AddMethod,
    }

    public delegate void NodeGenerateFunc(StringBuilder data);
    public class UIAutoNode
    {
        public string Name;
        public string PropertyName;
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
        public static string s_TextPrefix = "_Txt";
        public static string s_ImagePrefix = "_Img";
        public static string s_RawImagePrefix = "_RImg";
        public static string s_SliderPrefix = "_Sld";
        public static string[] s_PrefixList = 
        {
            s_TextPrefix,
            s_ImagePrefix,
            "_Tran",
            "_RTran",
            s_ButtonPrefix,
            s_SliderPrefix,
            "_Ifd", 
            "_Tgl", 
            "_LBtn", 
            s_RawImagePrefix,
        };

        public static Dictionary<string, string> s_Prefix2TypeName = new Dictionary<string, string> {
            {"_Tran", "Transform"},
            {"_RTran", "RectTransform"},
            {"_Ifd", "InputField"},
            {"_Tgl", "Toggle"},
            {"_LBtn", "LongButton"},
        };

        public void GenerateCode(StringBuilder data, UIGenType genType, string prefix)
        {
            foreach(var item in s_Prefix2TypeName)
            {
                if (Name.StartsWith(item.Key))
                {
                    GenerateComponent(data, genType, prefix, item.Value);
                    return;
                }
            }

            if (Name.StartsWith(s_TextPrefix))
            {
                GenerateText(data, genType, prefix);
            }
            else if (Name.StartsWith(s_ImagePrefix))
            {
                GenerateImage(data, genType, prefix);
            }
            else if (Name.StartsWith(s_RawImagePrefix))
            {
                GenerateRawImage(data, genType, prefix);
            }
            else if (Name.StartsWith(s_SliderPrefix))
            {
                GenerateSlider(data, genType, prefix);
            }
            else if (Name.StartsWith(s_ButtonPrefix))
            {
                GenerateButton(data, genType, prefix);
            }
            else if (Name.StartsWith(s_ScrollPrefix))
            {
                GenerateScroll(data, genType, prefix);
            }
            else if (Name.StartsWith(s_LoopScrollPrefix))
            {
                GenerateLoopScroll(data, genType, prefix);
            }
            else if (Name.StartsWith(s_TemplatePrefix))
            {
                GenerateObject(data, genType, prefix);
            }
        }
        
        public void GenerateObject(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public GameObject {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").gameObject;");
            }
        }

        public void GenerateComponent(StringBuilder data, UIGenType genType, string prefix, string compName)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public {compName} {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<{compName}>();");
            }
        }

        public void GenerateText(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public Text {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<Text>();");
            }
            else if (genType == UIGenType.BindObject)
            {
                data.AppendLine($"\t\t{prefix}bindSet.Bind(vm).For(() => vm.{PropertyName}, nameof(vm.{PropertyName})).To(Refresh{Name});");
            }
            else if (genType == UIGenType.DataMemberDefine)
            {
                data.AppendLine($"\t{prefix}string {Name};");
                data.AppendLine($"\t{prefix}public string {PropertyName}");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}get {{ return {Name}; }}");
                data.AppendLine($"\t\t{prefix}set {{ Set(ref {Name}, value); }}");
                data.AppendLine($"\t{prefix}}}");
            }
            else if (genType == UIGenType.AddMethod)
            {
                data.AppendLine($"\t{prefix}protected virtual void Refresh{Name}(string value)");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}{Name}.text = value;");
                data.AppendLine($"\t{prefix}}}");
                data.AppendLine();
            }
        }

        public void GenerateImage(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public Image {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<Image>();");
            }
            else if (genType == UIGenType.BindObject)
            {
                data.AppendLine($"\t\t{prefix}bindSet.Bind(vm).For(() => vm.{PropertyName}, nameof(vm.{PropertyName})).To(Refresh{Name});");
            }
            else if (genType == UIGenType.DataMemberDefine)
            {
                data.AppendLine($"\t{prefix}Sprite {Name};");
                data.AppendLine($"\t{prefix}public Sprite {PropertyName}");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}get {{ return {Name}; }}");
                data.AppendLine($"\t\t{prefix}set {{ Set(ref {Name}, value); }}");
                data.AppendLine($"\t{prefix}}}");
            }
            else if (genType == UIGenType.AddMethod)
            {
                data.AppendLine($"\t{prefix}protected virtual void Refresh{Name}(Sprite value)");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}{Name}.sprite = value;");
                data.AppendLine($"\t{prefix}}}");
                data.AppendLine();
            }
        }

        public void GenerateRawImage(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public RawImage {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<RawImage>();");
            }
            else if (genType == UIGenType.BindObject)
            {
                data.AppendLine($"\t\t{prefix}bindSet.Bind(vm).For(() => vm.{PropertyName}, nameof(vm.{PropertyName})).To(Refresh{Name});");
            }
            else if (genType == UIGenType.DataMemberDefine)
            {
                data.AppendLine($"\t{prefix}Texture {Name};");
                data.AppendLine($"\t{prefix}public Texture {PropertyName}");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}get {{ return {Name}; }}");
                data.AppendLine($"\t\t{prefix}set {{ Set(ref {Name}, value); }}");
                data.AppendLine($"\t{prefix}}}");
            }
            else if (genType == UIGenType.AddMethod)
            {
                data.AppendLine($"\t{prefix}protected virtual void Refresh{Name}(Texture value)");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}{Name}.texture = value;");
                data.AppendLine($"\t{prefix}}}");
                data.AppendLine();
            }
        }

        public void GenerateSlider(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public Slider {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<Slider>();");
            }
            else if (genType == UIGenType.BindObject)
            {
                data.AppendLine($"\t\t{prefix}bindSet.Bind(vm).For(() => vm.{PropertyName}, nameof(vm.{PropertyName})).To(Refresh{Name});");
            }
            else if (genType == UIGenType.DataMemberDefine)
            {
                data.AppendLine($"\t{prefix}float {Name};");
                data.AppendLine($"\t{prefix}public float {PropertyName}");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}get {{ return {Name}; }}");
                data.AppendLine($"\t\t{prefix}set {{ Set(ref {Name}, value); }}");
                data.AppendLine($"\t{prefix}}}");
            }      
            else if (genType == UIGenType.AddMethod)
            {
                data.AppendLine($"\t{prefix}protected virtual void Refresh{Name}(float value)");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}{Name}.value = value;");
                data.AppendLine($"\t{prefix}}}");
                data.AppendLine();
            }
        }

        public void GenerateButton(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public Button {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<Button>();");
            }
            else if (genType == UIGenType.BindObject)
            {
                string name = Name.Substring(UIAutoNode.s_ButtonPrefix.Length);
                data.AppendLine($"\t\t{prefix}bindSet.BindButton({Name}).For({Name}.onClick).To(vm.OnClick{name});");
            }
            else if (genType == UIGenType.DataMemberDefine)
            {
                string name = Name.Substring(UIAutoNode.s_ButtonPrefix.Length);
                data.AppendLine($"\t{prefix}public virtual void OnClick{name}() {{ Huge.Debug.Log(\"click {Name}\"); }}");
            }
        }

        public void GenerateScroll(StringBuilder data, UIGenType genType, string prefix)
        {
            string name = Name.Substring(UIAutoNode.s_ScrollPrefix.Length);
            string itemClassName = $"Item{ClassName}{name}";
            string listClassName = $"List{ClassName}{name}Generate";
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public ScrollRect {Name};");
                data.AppendLine($"\t{prefix}public GameObject {Name}Content;");
                data.AppendLine($"\t{prefix}public GameObject {Name}Template;");
                data.AppendLine($"\t{prefix}public IListView _{name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<ScrollRect>();");
                data.AppendLine($"\t\t{prefix}{Name}Content = {Name}.transform.Find(\"Content\").gameObject;");
                data.AppendLine($"\t\t{prefix}{Name}Template = {Name}.transform.Find(\"Template\").gameObject;");
                data.AppendLine($"\t\t{prefix}_{name} = Create{name}();");
                data.AppendLine($"\t\t{prefix}_{name}.Template = {Name}Template;");
                data.AppendLine($"\t\t{prefix}_{name}.Content = {Name}Content;");
                data.AppendLine($"\t\t{prefix}_{name}.Scroll = {Name};");
                data.AppendLine($"\t\t{prefix}_{name}.Parent = this;");
            }
            else if (genType == UIGenType.BindObject)
            {
                data.AppendLine($"\t\t{prefix}bindSet.BindList(vm).For(() => vm.{name}, nameof(vm.{name})).To(_{name});");
            }
            else if (genType == UIGenType.DataMemberDefine)
            {
                data.AppendLine($"\t{prefix}ObservableList<{itemClassName}ViewModelGenerate> _{name} = new ObservableList<{itemClassName}ViewModelGenerate>();");
                data.AppendLine($"\t{prefix}public ObservableList<{itemClassName}ViewModelGenerate> {name}");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}get {{ return _{name}; }}");
                data.AppendLine($"\t\t{prefix}set {{ Set(ref _{name}, value); }}");
                data.AppendLine($"\t{prefix}}}");
            }
            else if (genType == UIGenType.AddMethod)
            {
                data.AppendLine($"\t{prefix}protected virtual IListView Create{name}()");
                data.AppendLine($"\t{prefix}{{");
                data.AppendLine($"\t\t{prefix}return AddSubView<{listClassName}>({Name}.gameObject, null);");
                data.AppendLine($"\t{prefix}}}");
                data.AppendLine();
            }
        }

        public void GenerateLoopScroll(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public ScrollView {Name};");
                data.AppendLine($"\t{prefix}public GameObject {Name}Content;");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\").GetComponent<ScrollView>();");
                data.AppendLine($"\t\t{prefix}{Name}Content = {Name}.content.gameObject;");
            }
            else if (genType == UIGenType.BindObject)
            {

            }
            else if (genType == UIGenType.DataMemberDefine)
            {

            }
        }

        public void GenerateBG(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public Transform {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\");");
            }
        }

        public void GenerateFull(StringBuilder data, UIGenType genType, string prefix)
        {
            if (genType == UIGenType.MemberDefine)
            {
                data.AppendLine($"\t{prefix}public Transform {Name};");
            }
            else if (genType == UIGenType.FindObject)
            {
                data.AppendLine($"\t\t{prefix}{Name} = transform.Find(\"{Path}\")();");
            }
        }

        public void GenerateTemplate(StringBuilder data)
        {
            Tree.GenerateTemplate(ClassName, data, "");
            Tree.GenerateViewModel(ClassName, data, "");
        }

        public void GenerateScrollItemView(StringBuilder data)
        {
            Tree.GenerateScrollView(ClassName, data, "");
            Tree.GenerateScrollItemView($"Item{ClassName}", data, "");
            Tree.GenerateViewModel($"Item{ClassName}", data, "");
        }
    }

    public class UIAutoTree
    {
        public List<UIAutoNode> NodeList = new List<UIAutoNode>();
        public List<UIAutoNode> BGNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> FullNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> ObjectNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> TemplateNodeList = new List<UIAutoNode>();
        public List<UIAutoNode> ScrollNodeList = new List<UIAutoNode>();

        public void GenerateView(string viewName, StringBuilder data, string prefix)
        {
            data.AppendLine($"{prefix}public class {viewName}Generate : Window");
            data.AppendLine($"{prefix}{{");

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.MemberDefine, prefix);
            }
            data.AppendLine();

            data.AppendLine($"\t{prefix}protected override void OnGenerate()");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.FindObject, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            data.AppendLine($"\t{prefix}public virtual void BindViewModel(BindingSet bindSet, {viewName}ViewModelGenerate vm)");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.BindObject, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.AddMethod, prefix);
            }

            data.AppendLine($"{prefix}}}");
            data.AppendLine();
        }

        public void GenerateViewModel(string viewName, StringBuilder data, string prefix)
        {
            data.AppendLine($"{prefix}public class {viewName}ViewModelGenerate : ViewModel");
            data.AppendLine($"{prefix}{{");

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.DataMemberDefine, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.DataMemberDefine, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.DataMemberDefine, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.DataMemberDefine, prefix);
            }

            data.AppendLine($"{prefix}}}");
            data.AppendLine();
        }

        public void GenerateTemplate(string viewName, StringBuilder data, string prefix)
        {
            data.AppendLine($"{prefix}public class {viewName}Generate : SubView");
            data.AppendLine($"{prefix}{{");

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.MemberDefine, prefix);
            }
            data.AppendLine();

            data.AppendLine($"\t{prefix}protected override void OnGenerate()");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.FindObject, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            data.AppendLine($"\t{prefix}public virtual void BindViewModel(BindingSet bindSet, {viewName}ViewModelGenerate vm)");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.BindObject, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.AddMethod, prefix);
            }

            data.AppendLine($"{prefix}}}");
            data.AppendLine();
        }

        public void GenerateScrollView(string viewName, StringBuilder data, string prefix)
        {
            data.AppendLine($"{prefix}public class List{viewName}Generate : ListView<Item{viewName}Generate, Item{viewName}ViewModelGenerate>");
            data.AppendLine($"{prefix}{{");
            data.AppendLine();
            data.AppendLine($"{prefix}}}");
            data.AppendLine();
        }

        public void GenerateScrollItemView(string viewName, StringBuilder data, string prefix)
        {
            data.AppendLine($"{prefix}public class {viewName}Generate : ItemView<{viewName}ViewModelGenerate>");
            data.AppendLine($"{prefix}{{");

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.MemberDefine, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.MemberDefine, prefix);
            }
            data.AppendLine();

            data.AppendLine($"\t{prefix}protected override void OnGenerate()");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.FindObject, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.FindObject, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            data.AppendLine($"\t{prefix}public override void BindViewModel(BindingSet bindSet, {viewName}ViewModelGenerate vm)");
            data.AppendLine($"\t{prefix}{{");
            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.BindObject, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.BindObject, prefix);
            }
            data.AppendLine($"\t{prefix}}}");
            data.AppendLine();

            foreach(var node in ObjectNodeList)
            {
                node.GenerateObject(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in NodeList)
            {
                node.GenerateCode(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in BGNodeList)
            {
                node.GenerateBG(data, UIGenType.AddMethod, prefix);
            }
            foreach(var node in FullNodeList)
            {
                node.GenerateFull(data, UIGenType.AddMethod, prefix);
            }

            data.AppendLine($"{prefix}}}");
            data.AppendLine();
        }
    }
}