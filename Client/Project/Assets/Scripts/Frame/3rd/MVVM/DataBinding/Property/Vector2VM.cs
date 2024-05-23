using Unity.VisualScripting;
using UnityEngine;

namespace Huge.MVVM.DataBinding
{
    public class Vector2VM : ObservableObject
    {
        float _x;
        public float x
        {
            get { return _x; }
            set { Set(ref _x, value); }
        }
        float _y;
        public float y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }
        public Vector2VM(float x, float y)
        {
            _x = x; _y = y;
        }

        public static Vector2VM zero { get { return  new Vector2VM(0f, 0f); }}
        public static Vector2VM one { get { return  new Vector2VM(1f, 1f); }}
        public static Vector2VM up { get { return  new Vector2VM(0f, 1f); }}
        public static Vector2VM down { get { return  new Vector2VM(0f, -1f); }}
        public static Vector2VM left { get { return  new Vector2VM(-1f, 0f); }}
        public static Vector2VM right { get { return  new Vector2VM(1f, 0f); }}
        public static implicit operator Vector2(Vector2VM vec) => new Vector2(vec.x, vec.y);
        public static implicit operator Vector2VM(Vector2 vec) => new Vector2VM(vec.x, vec.y);

        public static Vector2VM operator +(Vector2VM a, Vector2VM b)
        {
            return new Vector2VM(a.x + b.x, a.y + b.y);
        }

        public static Vector2VM operator -(Vector2VM a, Vector2VM b)
        {
            return new Vector2VM(a.x - b.x, a.y - b.y);
        }

        public static Vector2VM operator -(Vector2VM a)
        {
            return new Vector2VM(0f - a.x, 0f - a.y);
        }

        public static Vector2VM operator *(Vector2VM a, float d)
        {
            return new Vector2VM(a.x * d, a.y * d);
        }

        public static Vector2VM operator *(float d, Vector2VM a)
        {
            return new Vector2VM(a.x * d, a.y * d);
        }

        public static Vector2VM operator /(Vector2VM a, float d)
        {
            return new Vector2VM(a.x / d, a.y / d);
        }
    }
}