﻿// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Kbg.NppPluginNET.PluginInfrastructure;
using NppPlugin.DllExport;
using Parsify.Core;
using Parsify.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    class UnmanagedExports
    {
        [DllExport(CallingConvention=CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            PluginBase.nppData = notepadPlusData;
            Main.CommandMenuInit();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Main.PluginName);
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            ScNotification notification = (ScNotification)Marshal.PtrToStructure(notifyCode, typeof(ScNotification));
            if (notification.Header.Code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginBase._funcItems.RefreshItems();
                Main.SetToolBarIcon();
            }
            else if (notification.Header.Code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                Main.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
            else
            {
	            Main.OnNotification(notification);
            }
        }

        #region LEXER specific

        [DllExport( CallingConvention = CallingConvention.StdCall )]
        public static int GetLexerCount()
        {
            // function will be called twice, once by npp and once by scintilla
            return 1;  // this dll contains only one lexer
        }

        [DllExport( CallingConvention = CallingConvention.StdCall )]
        public static IntPtr CreateLexer( IntPtr pName )
        {
            // function will be called by scintilla
            // Required for Notepad++ update from iLexer4 -> iLexer5

            string sName = Marshal.PtrToStringAnsi( pName );

#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] CreateLexer: " + sName );
#endif

            if ( sName == Parsify.PluginInfrastructure.Lexer.Name.Trim( '\0' ) )
            {
                if ( Main.DocumentFactory.Active == null )
                {
                    Main.Scintilla.SetDefaultLanguage();
                    return IntPtr.Zero;
                }

                return Parsify.PluginInfrastructure.Lexer.ILexerImplementation();
            }
            return IntPtr.Zero;
        }

        [DllExport( CallingConvention = CallingConvention.StdCall )]
        public static void GetLexerName( uint index, IntPtr name, int buffer_length )
        {
            // function will be called twice, once by npp and once by scintilla
            // index is always 0 if this dll has only one lexer
            // name is a pointer to memory provided by npp and scintilla InsertMenuA is used, hence byte array
            // buffer_length is the size of the provided memory
            // use zero-terminated string to interface with C++
            byte[] lexer_name = Encoding.ASCII.GetBytes( Parsify.PluginInfrastructure.Lexer.Name );
            Marshal.Copy( lexer_name, 0, name, lexer_name.Length );
        }

        [DllExport( CallingConvention = CallingConvention.StdCall )]
        public static void GetLexerStatusText( uint index, IntPtr name, int buffer_length )
        {
            // function will be called by npp only, fills the first field of the statusbar
            // index is always 0 if this dll has only one lexer
            // name is a pointer to memory provided by npp and scintilla
            // buffer_length is the size of the provided memory
            // use zero-terminated string to interface with C++

            char[] lexer_status_text = Parsify.PluginInfrastructure.Lexer.StatusText.ToCharArray(); // SendMessageW is used, hence ToCharArray as this returns utf16 strings
            Marshal.Copy( lexer_status_text, 0, name, lexer_status_text.Length );
        }

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerImpDelegate();

        //static ILexerImpDelegate ILexerImplementation;

        private static Delegate ilexer_implementation = new ILexerImpDelegate( Parsify.PluginInfrastructure.Lexer.ILexerImplementation );


        [DllExport( CallingConvention = CallingConvention.StdCall )]
        public static Delegate GetLexerFactory( int index )
        {
            // function will be called by scintilla only
            // index is always 0 if this dll has only one lexer
            //ILexerImplementation = new ILexerImpDelegate(ILexer.ILexerImplementation);
            //return ILexerImplementation;
            return ilexer_implementation;
        }
        #endregion
    }
}
