 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TransparentTaskbar
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT;
            accent.AccentFlags = 2;
            accent.GradientColor = 0x000000;
            accent.AnimationId = 0;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;
            IntPtr taskbar = (IntPtr)0;
            while (true) { 
                taskbar = FindWindow("Shell_TrayWnd", null);
                SetWindowCompositionAttribute(taskbar, ref data);

                taskbar = FindWindow("Shell_SecondaryTrayWnd", null);
                SetWindowCompositionAttribute(taskbar, ref data);
                Thread.Sleep(2000);
            }
           Marshal.FreeHGlobal(accentPtr);
             
        }

        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("User32.dll", EntryPoint = "GetWindow")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

    }
}
