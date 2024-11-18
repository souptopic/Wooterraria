using System.Runtime.InteropServices;

namespace Wooting
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DisconnectedCallback();
}
