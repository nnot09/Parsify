using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Parsify.Core.UDL
{
    internal static class HighlightingConfiguration
    {
        public const int NUMBER_OF_COLORS_PER_SET = 12;

        public static (string[] Background, string[] Foreground, string BoldFlag) GetColorPalette( AppHighlightingMode mode )
        {
            switch ( mode )
            {
                case AppHighlightingMode.Background:
                    return (BackgroundModePalette.Select( c => c.ToString( "x6" ) ).ToArray(), Black, "0");

                case AppHighlightingMode.Foreground:
                    return (White, ForegroundModePalette.Select( c => c.ToString( "x6" ) ).ToArray(), "1");

                case AppHighlightingMode.None:
                default:
                    return (null, null, null);
            }
        }

        private static string[] White
             => Enumerable.Repeat<string>( "FFFFFF", NUMBER_OF_COLORS_PER_SET ).ToArray();

        private static string[] Black
             => Enumerable.Repeat<string>( "000000", NUMBER_OF_COLORS_PER_SET ).ToArray();

        private static int[] ForegroundModePalette
            => new int[]
            {
               0x186CC0,
               0x186CC0,
               0x909000,
               0x6C18C0,
               0x18C018,
               0xC0186C,
               0x00A0A0,
               0xC06C18,
               0x1818C0,
               0x6CC018,
               0xC018C0,
               0x10A060,
               0xC01818,
            };

        private static int[] BackgroundModePalette
            => new int[]
            {
                0xC0EFFF,
                0xFFFFC0,
                0xFFE0FF,
                0xA0FFA0,
                0xFFC0E0,
                0xA0FFFF,
                0xFFE0C0,
                0xD0D0FF,
                0xCFFFA0,
                0xFFACFF,
                0x80FFBF,
                0xFFC0C0
            };
    }
}
