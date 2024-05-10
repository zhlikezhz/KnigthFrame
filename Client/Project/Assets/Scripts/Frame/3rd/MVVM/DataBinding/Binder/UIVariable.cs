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

using UnityEngine;

namespace Huge.MVVM
{
    [System.Serializable]
    public enum UIVariableType 
    {
        Object,
        GameObject,
        Component,
        Boolean,
        Integer,
        Float,
        String,
        Color,
        Vector2,
        Vector3,
        Vector4
    }

    [System.Serializable]
    public class UIVariable
    {
        [SerializeField]
        protected string name = "";

        [SerializeField]
        protected UnityEngine.Object objectValue;

        [SerializeField]
        protected string dataValue;

        [SerializeField]
        protected UIVariableType variableType;

        public virtual string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public virtual UIVariableType UIVariableType
        {
            get { return this.variableType; }
        }

        public virtual System.Type ValueType
        {
            get
            {
                switch (this.variableType)
                {
                    case UIVariableType.Boolean:
                        return typeof(bool);
                    case UIVariableType.Float:
                        return typeof(float);
                    case UIVariableType.Integer:
                        return typeof(int);
                    case UIVariableType.String:
                        return typeof(string);
                    case UIVariableType.Color:
                        return typeof(Color);
                    case UIVariableType.Vector2:
                        return typeof(Vector2);
                    case UIVariableType.Vector3:
                        return typeof(Vector3);
                    case UIVariableType.Vector4:
                        return typeof(Vector4);
                    case UIVariableType.Object:
                        return this.objectValue == null ? typeof(UnityEngine.Object) : this.objectValue.GetType();
                    case UIVariableType.GameObject:
                        return this.objectValue == null ? typeof(GameObject) : this.objectValue.GetType();
                    case UIVariableType.Component:
                        return this.objectValue == null ? typeof(Component) : this.objectValue.GetType();
                    default:
                        throw new System.NotSupportedException();
                }
            }
        }

        public virtual void SetValue<T>(T value)
        {
            this.SetValue(value);
        }

        public virtual T GetValue<T>()
        {
            return (T)GetValue();
        }

        public virtual void SetValue(object value)
        {
            switch (this.variableType)
            {
                case UIVariableType.Boolean:
                    this.dataValue = UIDataConverter.GetString((bool)value);
                    break;
                case UIVariableType.Float:
                    this.dataValue = UIDataConverter.GetString((float)value);
                    break;
                case UIVariableType.Integer:
                    this.dataValue = UIDataConverter.GetString((int)value);
                    break;
                case UIVariableType.String:
                    this.dataValue = UIDataConverter.GetString((string)value);
                    break;
                case UIVariableType.Color:
                    this.dataValue = UIDataConverter.GetString((Color)value);
                    break;
                case UIVariableType.Vector2:
                    this.dataValue = UIDataConverter.GetString((Vector2)value);
                    break;
                case UIVariableType.Vector3:
                    this.dataValue = UIDataConverter.GetString((Vector3)value);
                    break;
                case UIVariableType.Vector4:
                    this.dataValue = UIDataConverter.GetString((Vector4)value);
                    break;
                case UIVariableType.Object:
                    this.objectValue = (UnityEngine.Object)value;
                    break;
                case UIVariableType.GameObject:
                    this.objectValue = (GameObject)value;
                    break;
                case UIVariableType.Component:
                    this.objectValue = (Component)value;
                    break;
                default:
                    throw new System.NotSupportedException();
            }
        }
        public virtual object GetValue()
        {
            switch (this.variableType)
            {
                case UIVariableType.Boolean:
                    return UIDataConverter.ToBoolean(this.dataValue);
                case UIVariableType.Float:
                    return UIDataConverter.ToSingle(this.dataValue);
                case UIVariableType.Integer:
                    return UIDataConverter.ToInt32(this.dataValue);
                case UIVariableType.String:
                    return UIDataConverter.ToString(this.dataValue);
                case UIVariableType.Color:
                    return UIDataConverter.ToColor(this.dataValue);
                case UIVariableType.Vector2:
                    return UIDataConverter.ToVector2(this.dataValue);
                case UIVariableType.Vector3:
                    return UIDataConverter.ToVector3(this.dataValue);
                case UIVariableType.Vector4:
                    return UIDataConverter.ToVector4(this.dataValue);
                case UIVariableType.Object:
                    return this.objectValue;
                case UIVariableType.GameObject:
                    return this.objectValue;
                case UIVariableType.Component:
                    return this.objectValue;
                default:
                    throw new System.NotSupportedException();
            }
        }
    }
}
