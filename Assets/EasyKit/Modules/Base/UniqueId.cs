using System;
using System.Collections.Generic;

namespace EasyKit
{
    public enum UniqueIdType
    {
        Entity = 0,
        Task,
        Timer,
    }
    
    public class UniqueId
    {
        internal class ScopedUniqueID
        {
            private long currentId;

            public void Init()
            {
                currentId = 0L;
            }

            public void Reset()
            {
                currentId = 0L;
            }

            public long NewId()
            {
                bool flag = currentId < 9223372036854775807L;
                if (flag)
                {
                    currentId += 1L;
                }
                else
                {
                    currentId = 1L;
                }
                return currentId;
            }

        }


        private static Dictionary<int, ScopedUniqueID> sUniquePool = new Dictionary<int, ScopedUniqueID>();

        public static long NewId(UniqueIdType type)
        {
            var scope = (int)type;
            ScopedUniqueID scopeUniqueID = null;
            bool flag = !sUniquePool.TryGetValue(scope, out scopeUniqueID);
            if (flag)
            {
                scopeUniqueID = new ScopedUniqueID();
                scopeUniqueID.Init();
                sUniquePool[scope] = scopeUniqueID;
            }
            return scopeUniqueID.NewId();
        }
    }
}
