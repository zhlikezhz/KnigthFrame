using Unity.VisualScripting;
using UnityEngine;

namespace Huge.MVVM.DataBinding
{
    public class ColorVM : ObservableObject
    {
        float _r;
        public float r
        {
            get { return _r; }
            set { Set(ref _r, value); }
        }

        float _g;
        public float g
        {
            get { return _g; }
            set { Set(ref _g, value); }
        }

        float _b;
        public float b
        {
            get { return _b; }
            set { Set(ref _b, value); }
        }

        float _a;
        public float a
        {
            get { return _a; }
            set { Set(ref _a, value); }
        }

        public ColorVM(float r, float g, float b, float a = 1f)
        {
            _r = r; _g = g; 
            _b = b; _a = a;
        }

        public static ColorVM red { get { return new ColorVM(1f,0f,0f,1f); } }
        public static ColorVM green { get { return new ColorVM(0f,1f,0f,1f); } }
        public static ColorVM blue { get { return new ColorVM(0f,0f,1f,1f); } }
        public static ColorVM white { get { return new ColorVM(1f,1f,1f,1f); } }
        public static ColorVM black { get { return new ColorVM(0f,0f,0f,1f); } }
        public static ColorVM yellow { get { return new ColorVM(1f,47f/51f,0.015686275f,1f); } }
        public static ColorVM cyan { get { return new ColorVM(0f,1f,1f,1f); } }
        public static ColorVM magenta { get { return new ColorVM(1f,0f,1f,1f); } }
        public static ColorVM gray { get { return new ColorVM(0.5f,0.5f,0.5f,1f); } }
        public static ColorVM grey { get { return new ColorVM(0.5f,0.5f,0.5f,1f); } }
        public static ColorVM clear { get { return new ColorVM(0f,0f,0f,0f); } }
        public static implicit operator Color(ColorVM color) => new Color(color.r, color.g, color.b, color.a);
        public static implicit operator ColorVM(Color color) => new ColorVM(color.r, color.g, color.b, color.a);

        public static ColorVM operator +(ColorVM a, ColorVM b)
        {
            return new ColorVM(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        }

        public static ColorVM operator -(ColorVM a, ColorVM b)
        {
            return new ColorVM(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        }

        public static ColorVM operator *(ColorVM a, ColorVM b)
        {
            return new ColorVM(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
        }

        public static ColorVM operator *(ColorVM a, float b)
        {
            return new ColorVM(a.r * b, a.g * b, a.b * b, a.a * b);
        }

        public static ColorVM operator *(float b, ColorVM a)
        {
            return new ColorVM(a.r * b, a.g * b, a.b * b, a.a * b);
        }

        public static ColorVM operator /(ColorVM a, float b)
        {
            return new ColorVM(a.r / b, a.g / b, a.b / b, a.a / b);
        }
    }
}