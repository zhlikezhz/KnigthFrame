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
using System.Collections.ObjectModel;
using UnityEngine;

namespace Huge.MVVM
{
    [System.Serializable]
    public class UIVariableArray
    {
        [SerializeField]
        private List<UIVariable> variables;

        public ReadOnlyCollection<UIVariable> UIVariables
        {
            get { return variables.AsReadOnly(); }
        }

        public UIVariable this[int index]
        {
            get { return variables[index]; }
        }

        public object Get(string name)
        {
            if (this.variables == null || this.variables.Count <= 0)
                return null;
            var variable = this.variables.Find(v => v.Name.Equals(name));
            if (variable == null)
                return null;
            return variable.GetValue();
        }

        public T Get<T>(string name)
        {
            if (this.variables == null || this.variables.Count <= 0)
                return default(T);
            var variable = this.variables.Find(v => v.Name.Equals(name));
            if (variable == null)
                return default(T);
            return variable.GetValue<T>();
        }

        public static implicit operator List<UIVariable>(UIVariableArray array)
        {
            return array.variables;
        }

        public static implicit operator UIVariableArray(List<UIVariable> variables)
        {
            return new UIVariableArray() { variables = variables };
        }
    }
}
