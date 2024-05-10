/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Huge.MVVM;

namespace Huge.MVVM.Editor
{
    [CustomPropertyDrawer(typeof(UIVariable))]
    public class UIVariableDrawer : PropertyDrawer
    {
        private const float HORIZONTAL_GAP = 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var name = property.FindPropertyRelative("name");
            var objectValue = property.FindPropertyRelative("objectValue");
            var dataValue = property.FindPropertyRelative("dataValue");
            var variableType = property.FindPropertyRelative("variableType");

            float y = position.y;
            float x = position.x;
            float height = GetPropertyHeight(property, label);
            float width = position.width - HORIZONTAL_GAP * 2;

            Rect nameRect = new Rect(x, y, Mathf.Min(200, width * 0.4f), height);
            Rect typeRect = new Rect(nameRect.xMax + HORIZONTAL_GAP, y, Mathf.Min(120, width * 0.2f), height);
            Rect valueRect = new Rect(typeRect.xMax + HORIZONTAL_GAP, y, position.xMax - typeRect.xMax - HORIZONTAL_GAP, height);

            EditorGUI.PropertyField(nameRect, name, GUIContent.none);

            UIVariableType variableTypeValue = (UIVariableType)variableType.enumValueIndex;

            if (variableTypeValue == UIVariableType.Component)
            {
                int index = 0;
                List<System.Type> types = new List<System.Type>();
                var component = (Component)objectValue.objectReferenceValue;
                if (component != null)
                {
                    GameObject go = component.gameObject;
                    foreach (var c in go.GetComponents<Component>())
                    {
                        if (c == null)
                            continue;

                        if (!types.Contains(c.GetType()))
                            types.Add(c.GetType());
                    }

                    for (int i = 0; i < types.Count; i++)
                    {
                        if (component.GetType().Equals(types[i]))
                        {
                            index = i;
                            break;
                        }
                    }
                }

                if (types.Count <= 0)
                    types.Add(typeof(Transform));

                List<GUIContent> contents = new List<GUIContent>();
                foreach (var t in types)
                {
                    contents.Add(new GUIContent(t.Name, t.FullName));
                }

                EditorGUI.BeginChangeCheck();
                var newIndex = EditorGUI.Popup(typeRect, GUIContent.none, index, contents.ToArray(), EditorStyles.popup);
                if (EditorGUI.EndChangeCheck())
                {
                    if (component != null)
                        objectValue.objectReferenceValue = component.gameObject.GetComponent(types[newIndex]);
                    else
                        objectValue.objectReferenceValue = null;
                }
            }
            else
            {
                EditorGUI.LabelField(typeRect, variableTypeValue.ToString());
            }

            switch (variableTypeValue)
            {
                case UIVariableType.Component:
                    EditorGUI.BeginChangeCheck();
                    objectValue.objectReferenceValue = EditorGUI.ObjectField(valueRect, GUIContent.none, objectValue.objectReferenceValue, typeof(UnityEngine.Component), true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (string.IsNullOrEmpty(name.stringValue) && objectValue.objectReferenceValue != null)
                            name.stringValue = NormalizeName(objectValue.objectReferenceValue.name);
                    }
                    break;
                case UIVariableType.GameObject:
                    EditorGUI.BeginChangeCheck();
                    objectValue.objectReferenceValue = EditorGUI.ObjectField(valueRect, GUIContent.none, objectValue.objectReferenceValue, typeof(UnityEngine.GameObject), true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (string.IsNullOrEmpty(name.stringValue) && objectValue.objectReferenceValue != null)
                            name.stringValue = NormalizeName(objectValue.objectReferenceValue.name);
                    }
                    break;
                case UIVariableType.Object:
                    EditorGUI.BeginChangeCheck();
                    objectValue.objectReferenceValue = EditorGUI.ObjectField(valueRect, GUIContent.none, objectValue.objectReferenceValue, typeof(UnityEngine.Object), true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (string.IsNullOrEmpty(name.stringValue) && objectValue.objectReferenceValue != null)
                            name.stringValue = NormalizeName(objectValue.objectReferenceValue.name);
                    }
                    break;
                case UIVariableType.Color:
                    Color color = UIDataConverter.ToColor(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    color = EditorGUI.ColorField(valueRect, GUIContent.none, color);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(color);
                    }
                    break;
                case UIVariableType.Vector2:
                    Vector2 vector2 = UIDataConverter.ToVector2(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    vector2 = EditorGUI.Vector2Field(valueRect, GUIContent.none, vector2);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(vector2);
                    }
                    break;
                case UIVariableType.Vector3:
                    Vector3 vector3 = UIDataConverter.ToVector3(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    vector3 = EditorGUI.Vector3Field(valueRect, GUIContent.none, vector3);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(vector3);
                    }
                    break;
                case UIVariableType.Vector4:
                    Vector4 vector4 = UIDataConverter.ToVector4(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
#if UNITY_5_6_OR_NEWER
                    vector4 = EditorGUI.Vector4Field(valueRect, GUIContent.none, vector4);
#else
                    var tmpRect = valueRect;
                    tmpRect.y -= height;
                    vector4 = EditorGUI.Vector4Field(tmpRect, "", vector4);
#endif
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(vector4);
                    }
                    break;
                case UIVariableType.Boolean:
                    bool b = UIDataConverter.ToBoolean(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    b = EditorGUI.Toggle(valueRect, GUIContent.none, b);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(b);
                    }
                    break;
                case UIVariableType.Float:
                    float f = UIDataConverter.ToSingle(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    f = EditorGUI.FloatField(valueRect, GUIContent.none, f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(f);
                    }
                    break;
                case UIVariableType.Integer:
                    int i = UIDataConverter.ToInt32(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    i = EditorGUI.IntField(valueRect, GUIContent.none, i);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(i);
                    }
                    break;
                case UIVariableType.String:
                    string s = UIDataConverter.ToString(dataValue.stringValue);
                    EditorGUI.BeginChangeCheck();
                    s = EditorGUI.TextField(valueRect, GUIContent.none, s);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dataValue.stringValue = UIDataConverter.GetString(s);
                    }
                    break;
                default:
                    break;
            }
            EditorGUI.EndProperty();
        }


        protected virtual string NormalizeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "";

            name = name.Replace(" ", "");
            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}