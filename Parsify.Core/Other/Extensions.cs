using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsify.Core.Other
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