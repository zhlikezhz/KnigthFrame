using Unity.VisualScripting;
using UnityEngine;

namespace Huge.MVVM.DataBinding
{
    public class QuaternionVM : ObservableObject
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
        float _w;
        public float w
        {
            get { return _w; }
            set { Set(ref _w, value); }
        }

        public QuaternionVM(float x, float y, float z, float w = 1f)
        {
            _x = x; _y = y; _z = z; _w = w;
        }

        public static implicit operator Quaternion(QuaternionVM vec) => new Quaternion(vec.x, vec.y, vec.z, vec.w);
        public static implicit operator QuaternionVM(Quaternion vec) => new QuaternionVM(vec.x, vec.y, vec.z, vec.w);
    }
}