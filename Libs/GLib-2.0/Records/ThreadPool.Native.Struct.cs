using System;
using System.Runtime.InteropServices;

#nullable enable

namespace GLib
{
    // AUTOGENERATED FILE - DO NOT MODIFY
    public partial record ThreadPool
    {
        //TODO: Native is only temporary public. Managed Callbacks reference this class, which is not correct. 
        public partial class Native
        {

            #region Methods
            
            public partial class Methods
            {
                public static void Free(ThreadPoolSafeHandle pool)
                    => Free(pool, true, true); //TODO clarify which parameters are usefull
            }
            #endregion
        }
    }
}