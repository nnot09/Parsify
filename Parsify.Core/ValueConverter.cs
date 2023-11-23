using Parsify.Core.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core
{
    public static class ValueConverter
    {
        public static string GetDecimalRepresentation( string value )
        {
            //if ( string.IsNullOrWhiteSpace( value ) )
            //    return "(null)";

            //if ( value.Equals( "decimal", StringComparison.OrdinalIgnoreCase ) )
            //    return value;

            //if ( !value.Equals( "decimal(" ) && value.Last() == ')' ) ;

            return value;
        }
    }
}
