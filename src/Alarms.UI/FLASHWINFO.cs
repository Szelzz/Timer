using System;
using System.Runtime.InteropServices;

namespace Alarms.UI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        private const uint FLASHW_ALL = 3;
        public FLASHWINFO(IntPtr window, uint count)
        {
            hwnd = window;
            dwFlags = FLASHW_ALL;
            uCount = count;
            cbSize = Convert.ToUInt32(Marshal.SizeOf(this));
        }
        public UInt32 cbSize; //The size of the structure in bytes.            
        public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.


        public UInt32 dwFlags; //The Flash Status.            
        public UInt32 uCount; // number of times to flash the window            
        public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.        
    }
}
