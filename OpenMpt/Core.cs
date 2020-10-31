using System;

namespace OpenMpt
{
    public class Core
    {
        

        public static UInt32 GetCoreVersion()
        {
            return Native.GetCoreVersion();
        }

        public static UInt32 GetLibraryVersion()
        {
            return Native.GetLibraryVersion();
        }

    }
}