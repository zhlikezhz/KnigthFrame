using Unity.VisualScripting;
using UnityEngine;

namespace Huge.MVVM.DataBinding
{
    public class ColorVM : ObservableObject
    {
        float r;
        public float R
        {
            get { return r; }
            set { Set(ref r, value); }
        }

        float g;
        public float G
        {
            get { return g; }
            set { Set(ref g, value); }
        }

        float b;
        public float B
        {
            get { return b; }
            set { Set(ref b, value); }
        }

        float a;
        public float A
        {
            get { return a; }
            set { Set(ref a, value); }
        }

        public ColorVM(float r, float g, float b, float a = 1f)
        {
            this.r = r; 
            this.g = g; 
            this.b = b; 
            this.a = a;
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
        public static explicit operator ColorVM(Color color) => new ColorVM(color.r, color.g, color.b, color.a);

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

        public void Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            R = a.r + (b.r - a.r) * t;
            G = a.g + (b.g - a.g) * t;
            B = a.b + (b.b - a.b) * t;
            A = a.a + (b.a - a.a) * t;
        }

        public void Lerp(ColorVM a, ColorVM b, float t)
        {
            t = Mathf.Clamp01(t);
            R = a.r + (b.r - a.r) * t;
            G = a.g + (b.g - a.g) * t;
            B = a.b + (b.b - a.b) * t;
            A = a.a + (b.a - a.a) * t;
        }
    }
}