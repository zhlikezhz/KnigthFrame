using System;
using Huge.Pool;
using Unity.VisualScripting;

namespace Huge.MVVM
{
    public static class DBPools
    {
        public readonly static ObjectPool<DBInt8> DBInt8Pool = new ObjectPool<DBInt8>(null, null, true);
        public readonly static ObjectPool<DBInt16> DBInt16Pool = new ObjectPool<DBInt16>(null, null, true);
        public readonly static ObjectPool<DBInt32> DBInt32Pool = new ObjectPool<DBInt32>(null, null, true);
        public readonly static ObjectPool<DBInt64> DBInt64Pool = new ObjectPool<DBInt64>(null, null, true);
        public readonly static ObjectPool<DBFloat> DBFloatPool = new ObjectPool<DBFloat>(null, null, true);
        public readonly static ObjectPool<DBDouble> DBDoublePool = new ObjectPool<DBDouble>(null, null, true);
        public readonly static ObjectPool<DBString> DBStringPool = new ObjectPool<DBString>(null, null, true);
    }
}