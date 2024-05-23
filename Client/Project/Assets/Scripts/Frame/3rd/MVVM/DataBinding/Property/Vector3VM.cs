using Unity.VisualScripting;
using UnityEngine;

namespace Huge.MVVM.DataBinding
{
    public class Vector3VM : ObservableObject
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
        float _z;
        public float z
        {
            get { return _z; }
            set { Set(ref _z, value); }
        }

        public Vector3VM(float x, float y, float z)
        {
            _x = x; _y = y; _z = z;
        }

        public static Vector3VM zero { get { return  new Vector3VM(0f, 0f, 0f); }}
        public static Vector3VM one { get { return  new Vector3VM(1f, 1f, 1f); }}
        public static Vector3VM up { get { return  new Vector3VM(0f, 1f, 0f); }}
        public static Vector3VM down { get { return  new Vector3VM(0f, -1f, 0f); }}
        public static Vector3VM left { get { return  new Vector3VM(-1f, 0f, 0f); }}
        public static Vector3VM right { get { return  new Vector3VM(1f, 0f, 0f); }}
        public static Vector3VM forward { get { return  new Vector3VM(0f, 0f, 1f); }}
        public static Vector3VM back { get { return  new Vector3VM(0f, 0f, -1f); }}
        public static implicit operator Vector3(Vector3VM vec) => new Vector3(vec.x, vec.y, vec.z);
        public static implicit operator Vector3VM(Vector3 vec) => new Vector3VM(vec.x, vec.y, vec.z);

        public static Vector3VM operator +(Vector3VM a, Vector3VM b)
        {
            return new Vector3VM(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3VM operator -(Vector3VM a, Vector3VM b)
        {
            return new Vector3VM(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3VM operator -(Vector3VM a)
        {
            return new Vector3VM(0f - a.x, 0f - a.y, 0f - a.z);
        }

        public static Vector3VM operator *(Vector3VM a, float d)
        {
            return new Vector3VM(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3VM operator *(float d, Vector3VM a)
        {
            return new Vector3VM(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3VM operator /(Vector3VM a, float d)
        {
            return new Vector3VM(a.x / d, a.y / d, a.z / d);
        }
    }
}