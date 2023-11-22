using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using System.Linq;

namespace ProtoBufToLuaAPI
{
    //用于存贮消息类型的类


    class ProtoBufToLua
    {
        private List<EnumStruct> enumList { get; set; }
        private List<MessageStruct> messageList { get; set; }
        private List<MethodStruct> methodList { get; set; }
        public static Dictionary<string, string> messageIdDic { get; set; }
        public static Dictionary<string, string> entityTypeDic { get; set; }
        public static List<string>pbFileList { get; set; }
        private string filePath { get; set; }
        private string fileName { get; set; }
        public ProtoBufToLua()
        {
            enumList = new List<EnumStruct>();
            messageList = new List<MessageStruct>();
            methodList = new List<MethodStruct>();
        }
        private string ExprotPath = null;
        public void Init(string path, string outFold)
        {
            InitPath(path, outFold);
            GenAPI();
            WriteAllEnum();
            WriteAllMessage();
            WriteAllMethod();
        }
        private void InitPath(string filePath, string outFold)
        {

            this.ExprotPath = outFold;
            this.filePath = filePath;
            this.fileName = Path.GetFileName(filePath);
            //创建导出的文件的地址
            if (Directory.Exists(ExprotPath) == false)
                Directory.CreateDirectory(outFold);
        }

        private void GenAPI()
        {

            Debug.Log("读取文件--->" + filePath);
            var lines = File.ReadAllLines(filePath);
            ReadProto(lines);
            
            string fileName=Path.GetFileNameWithoutExtension(filePath);
            if (!string.IsNullOrEmpty(fileName) && !pbFileList.Contains(fileName))
            {
                pbFileList.Add(fileName);
            }

        }

        private void ReadProto(string[] file)
        {
            for (long i = 0; i < file.Length; ++i)
            {
                string line = file[i].Trim();
                if (line == "")
                    continue;

                if (line.Contains("import") || line.Contains("package com.gy.server.packet;")
                    || line.Contains("option java_package") || line.Contains("option java_outer_classname ")
                    || line.Contains("=") || line.Contains("//"))
                    continue;

                if (Tools.StringIsStruct(line))
                    GetStructLines(i, file);

                if (line.StartsWith(MethodStruct.type))
                {
                    ParseMethodLines(line);
                }
            }
        }
        /// <summary>
        /// 获得一个结构体再文件中的所有的行数
        /// </summary>
        /// <param name="startIndex">结构体起始的行数</param>
        /// <param name="files">结构体所在文件的所有内容</param>
        /// <returns>返回这个结构截止下标</returns>
        private long GetStructLines(long startIndex, string[] files)
        {
            long _startIndex = startIndex;
            List<string> lins = new List<string>();
            lins.Add(files[startIndex++]);//后置++，添加第一行进列表后下标自动+1

            for (long i = startIndex; i < files.Length; ++i)
            {
                string data = files[i];
                if (Tools.StringIsStruct(data))
                {
                    _startIndex = i;
                    i = GetStructLines(i, files);//获取到一个新的消息的所有行,嵌套的消息
                }
                else if (data.Contains("}"))//当前结构体的结尾,在这里将获取到的结构添加
                {
                    lins.Add(data);

                    var comment = _startIndex > 0 ? files[_startIndex - 1] : string.Empty;
                    DistinguishStruct(lins, comment);
                    return i;
                }
                else
                    lins.Add(data);
            }
            return startIndex;
        }

        private void ParseMethodLines(string line)
        {
            Debug.Log("parse:" + line);
            // string str = @"^\s*rpc\s*(\w+)\(([^]]+)\)\s*returns\s*\(([^]]+)\)\s*\{\s*\}?$";
            string str = @"^\s*rpc\s*(?<method>[^]]+)\s*(\((?<input>[^]]+)\))\s*returns\s*(\((?<output>[^]]+)\))\s*\{\s*\}?$";
            Regex regex = new Regex(str);
            var match = regex.Match(line);
            // Debug.Log("match:" + match.Success);
            if (match.Success)
            {
                var method = match.Groups["method"].Value;
                var input = match.Groups["input"].Value;
                var output = match.Groups["output"].Value;
                //var method = match.Groups[1].Value;
                //var input = match.Groups[2].Value;
                //var output = match.Groups[3].Value;
                Debug.Log($"method：{method},input:{input},output:{output}");

                MethodStruct methodStruct = new MethodStruct()
                {
                    name = method,
                    inputName = input,
                    outputName = output

                };
                methodList.Add(methodStruct);
            }
        }
        /// <summary>
        /// 区分结构，将结构添加进数据中
        /// </summary>
        /// <param name="file">结构的所有字符串</param>
        /// <param name="comments">结构的注释</param>
        private void DistinguishStruct(List<string> file, string comments)
        {
            if (file.Count == 0)
                return;
            var title = file[0];

            if (StructIsAdd(title) || title.Contains("="))
                return;

            if (title.Contains(MessageStruct.type))
            {
                var data = new MessageStruct(file, comments);
                this.messageList.Add(data);
            }

            if (title.Contains(EnumStruct.type))
            {
                var data = new EnumStruct(file, comments);
                this.enumList.Add(data);
            }
        }
        private bool StructIsAdd(string title)
        {
            var className = Tools.GetClassName(title);
            foreach (var data in this.messageList)
            {
                if (data.name == className)
                    return true;
            }

            foreach (var data in this.enumList)
            {
                if (data.name == className)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 将读取到的结构体写到同一个文件中
        /// </summary>
        private void WriteAllEnum()
        {
            if (this.enumList.Count == 0)
            {
                return;
            }
            string fold = System.Environment.CurrentDirectory + "\\lua\\hall\\proto\\enum";
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }
            string filePath = fold + "\\" + this.fileName.Replace(".proto", "_enum") + ".lua";

            List<string> files = new List<string>();
            for (int i = 0; i < this.enumList.Count; ++i)
            {
                var enumData = this.enumList[i];

                // string className = string.Empty;
                //  className = "---@class " + enumData.name + " " + (string.IsNullOrEmpty(enumData.comments) ? " " : enumData.comments);
                if (!string.IsNullOrEmpty(enumData.comments))
                {
                    var comments = "---" + enumData.comments;
                    files.Add(comments);
                }




                files.Add(enumData.name + " = {");
          

                for (int j = 0; j < enumData.fields.Count; ++j)
                {
                    //ErrNone
                    var field = enumData.fields[j];


                    if (field.Trim().StartsWith("ErrNone")) continue;

                    files.Add(field);
                }


                files.Add("}");
            }
            Console.WriteLine("正在写文件MessageEnum");
            File.WriteAllLines(filePath, files.ToArray());
            Console.WriteLine("枚举类型处理完毕");
        }
        /// <summary>
        /// 将读取到的message写到同一个文件中
        /// </summary>
        private void WriteAllMessage()
        {
            //string typeName = "Message";
            string fileName = this.ExprotPath + "\\" + this.fileName + ".lua";

            List<string> files = new List<string>();

            for (int i = 0; i < this.messageList.Count; ++i)
            {
                var messageData = this.messageList[i];

                string className = string.Empty;
                if (!string.IsNullOrEmpty(messageData.comments))
                {

                    className = "--- " + messageData.comments + "\n";
                }
                className = className + "---@class " + messageData.name;


                files.Add(className);

                for (int j = 0; j < messageData.fields.Count; ++j)
                {
                    files.Add(string.Format("---@field {0} {1}{2} {3}",
                        messageData.fields[j].name,
                        messageData.fields[j].type,
                        messageData.fields[j].isArry ? "[]" : "",
                        messageData.fields[j].comments));
                }

                if (!string.IsNullOrEmpty(messageData.msgid))
                {
                    messageIdDic.Add(messageData.name, messageData.msgid);
                }
                //if (!string.IsNullOrEmpty(messageData.entityType))
                //{
                //    entityTypeDic.Add(messageData.name, messageData.entityType);
                //}

                files.Add(string.Format("{0} = ", messageData.name) + "{ }");
                files.Add("");
            }


            File.WriteAllLines(fileName, files.ToArray());

        }

        private void WriteAllMethod()
        {
            if (this.methodList.Count == 0)
            {
                return;
            }
            string fold = System.Environment.CurrentDirectory + "\\lua\\hall\\proto\\method";
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }
            string fileName = this.fileName.Replace(".proto", "_method");
            string filePath = fold + "\\" + fileName + ".lua";



            StringBuilder sb = new StringBuilder();
            sb.AppendLine(fileName + " =");
            sb.AppendLine("{");

            foreach (var method in this.methodList)
            {
                sb.Append(method.name).Append("=").Append("{").Append($"method=\"{method.name}\",inputType=\"{method.inputName}\",outputType=\"{method.outputName}\"").Append("},");
                sb.AppendLine();
            }

            sb.AppendLine("}");
            File.WriteAllText(filePath, sb.ToString());
        }

        public static void WriteAllMessageIdDic()
        {
            if (messageIdDic.Count == 0)
            {
                return;
            }
            string fold = System.Environment.CurrentDirectory + "\\lua\\hall\\proto\\method";
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }
            string fileName = "MessageID";
            string filePath = fold + "\\MessageID" + ".lua";

            var dic = messageIdDic.OrderBy(x => x.Value);


            StringBuilder sb = new StringBuilder();
            sb.AppendLine(fileName + " = ");
            sb.AppendLine("{");

            foreach (var message in dic)
            {
                sb.Append(message.Key);
                sb.Append("= ");
                sb.Append(message.Value);
                sb.Append(",");
                sb.AppendLine();
            }

            sb.AppendLine("}");
            File.WriteAllText(filePath, sb.ToString());
        }

        public static void WriteAllProtoFiles()
        {
            if (pbFileList.Count == 0)
            {
                return;
            }
            string fold = System.Environment.CurrentDirectory + "\\lua\\hall\\proto\\method";
            if (!Directory.Exists(fold))
            {
                Directory.CreateDirectory(fold);
            }
            string fileName = "ProtoFiles";
            string filePath = $"{fold}\\{fileName}.lua";

            pbFileList.Sort();


            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"local {fileName} = ");
            sb.AppendLine("{");

            for(int i = 0;i < pbFileList.Count; i++) 
            {
                var item = pbFileList[i];
                sb.AppendLine($"[{i+1}]=\"{item}\",");
            }

            sb.AppendLine("}");
            sb.AppendLine($" return {fileName} ");
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
