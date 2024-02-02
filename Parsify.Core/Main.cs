using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.Core.Core;
using System.Collections.Generic;
using System.Linq;
using Parsify.Core.Forms;
using Parsify.Core.Config;

namespace Kbg.NppPluginNET
{
    class Main : IDisposable
    {
        internal const string PluginName = "Parsify";
        static string iniFilePath = null;
        static bool toggleParsify = false;
        static frmCoreWindow coreWindow = null;
        static frmConfig configWindow = null;
        static int parsifyId = -1;
        static Bitmap tbBmp = Parsify.Core.Properties.Resources.star;
        static Bitmap tbBmp_tbTab = Parsify.Core.Properties.Resources.star_bmp;
        static Icon tbIcon = null;
        public static AppConfig Configuration { get; set; }

        public static void OnNotification( ScNotification notification )
        {
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder( Win32.MAX_PATH );
            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath );
            iniFilePath = sbIniFilePath.ToString();

            if ( !Directory.Exists( iniFilePath ) )
                Directory.CreateDirectory( iniFilePath );

            Configuration = AppConfig.LoadOrCreate();

            iniFilePath = Path.Combine( iniFilePath, PluginName + ".ini" );
            toggleParsify = ( Win32.GetPrivateProfileInt( "Parsify", "Toggle", 0, iniFilePath ) != 0 );

            PluginBase.SetCommand( 0, "Open", StartCoreWindow ); parsifyId = 0;
            PluginBase.SetCommand( 0, "Settings", StartConfigWindowFloatDiag ); 
        }

        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal( Marshal.SizeOf( tbIcons ) );
            Marshal.StructureToPtr( tbIcons, pTbIcons, false );
            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[ parsifyId ]._cmdID, pTbIcons );
            Marshal.FreeHGlobal( pTbIcons );
        }

        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString( "Parsify", "Toggle", toggleParsify ? "1" : "0", iniFilePath );
        }

        internal static void StartCoreWindow()
        {
            if ( coreWindow == null )
            {
                coreWindow = new frmCoreWindow();

                using ( Bitmap newBmp = new Bitmap( 16, 16 ) )
                {
                    Graphics g = Graphics.FromImage( newBmp );
                    ColorMap[] colorMap = new ColorMap[ 1 ];
                    colorMap[ 0 ] = new ColorMap();
                    colorMap[ 0 ].OldColor = Color.Fuchsia;
                    colorMap[ 0 ].NewColor = Color.FromKnownColor( KnownColor.ButtonFace );
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable( colorMap );
                    g.DrawImage( tbBmp_tbTab, new Rectangle( 0, 0, 16, 16 ), 0, 0, 16, 16, GraphicsUnit.Pixel, attr );
                    tbIcon = Icon.FromHandle( newBmp.GetHicon() );
                }

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = coreWindow.Handle;
                _nppTbData.pszName = coreWindow.Text;
                _nppTbData.dlgID = parsifyId;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_LEFT | NppTbMsg.DWS_ICONBAR | NppTbMsg.DWS_ICONTAB;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;

                IntPtr _ptrNppTbData = Marshal.AllocHGlobal( Marshal.SizeOf( _nppTbData ) );
                Marshal.StructureToPtr( _nppTbData, _ptrNppTbData, false );

                Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData );
            }
            else
            {
                // Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, instance.Handle );
                Show( coreWindow );
            }
        }

        internal static void StartConfigWindowFloatDiag()
        {
            using ( frmConfig config = new frmConfig() )
            {
                if ( config.ShowDialog() == DialogResult.OK )
                {
                    Configuration.Save();
                }
            }
        }

        internal static void Show( Form instance )
        {
            if ( instance == null )
                return;

            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, instance.Handle );
        }

        internal static void Hide( Form instance )
        {
            if ( instance == null )
                return;

            Win32.SendMessage( PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMHIDE, 0, instance.Handle );
        }

        public void Dispose()
        {
            if ( coreWindow != null ) coreWindow.Dispose();
            if ( configWindow != null ) configWindow.Dispose();
        }
    }
}