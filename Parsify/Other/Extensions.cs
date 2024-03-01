using Parsify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Other
{
    internal static class Extensions
    {
        public static string GetField( string line, int index, int length )
        {
            if ( string.IsNullOrWhiteSpace( line ) )
                return line;

            if ( line.Length < index )
                return line;

            if ( line.Length < index + length )
                return line.Substring( index );

            return line.Substring( index, length );
        }

        public static byte[] CreateHash( this Document document )
        {
            using ( SHA1 sha1 = SHA1.Create() )
            {
                var content = File.ReadAllBytes( document.FilePath );
                return sha1.ComputeHash( content );
            }
        }

        public static bool CompareHash( this byte[] a, byte[] b )
            => a.SequenceEqual( b );

        public static void UpdateWindowStyles( this Form form )
        {
            foreach ( Control control in form.Controls )
                UpdateControlWindowStyles( control );
        }

        private static void UpdateControlWindowStyles( Control control )
        {
            NativeWindowApi.ModifyStyleEx( control.Handle, 0, NativeWindowApi.WS_EX_CONTROLPARENT );

            foreach ( Control child in control.Controls )
                UpdateControlWindowStyles( child );
        }
    }
}