﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.Other
{
    public static class NativeWindowApi
    {
        [DllImport( "User32.dll", CharSet = CharSet.Ansi )]
        extern static int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );

        [DllImport( "User32.dll", CharSet = CharSet.Ansi )]
        extern static int GetWindowLong( IntPtr hWnd, int nIndex );

        const int GWL_EXSTYLE = -20;

        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_WINDOWEDGE = 0x00000100;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_OVERLAPPEDWINDOW = 0x00000100 | 0x00000200;
        public const int WS_EX_PALETTEWINDOW = 0x00000100 | 0x00000080 | 0x00000008;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000;
        public const int WS_EX_LAYOUTRTL = 0x00400000;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public static void ModifyStyleEx( IntPtr hWnd, int remove, int add )
        {
            int res = GetWindowLong( hWnd, GWL_EXSTYLE );

            res &= ~remove;
            res |= add;

            SetWindowLong( hWnd, GWL_EXSTYLE, res );
        }
    }
}