﻿using System.Linq;

namespace Parsify.Lexer
{
    internal class ColorPalette
    {
        public string[] BackgroundColors { get; }
        public string[] ForegroundColors { get; }
        public string BoldFlag { get; }

        public ColorPalette( string[] bgColors, string[] fgColors, string boldFlag )
        {
            this.BackgroundColors = bgColors;
            this.ForegroundColors = fgColors;
            this.BoldFlag = boldFlag;
        }
    }

    internal static class HighlightingConfiguration
    {
        public const int NUMBER_OF_COLORS_PER_SET = 12;

        public static ColorPalette GetColorPalette( AppHighlightingMode mode )
        {
            switch ( mode )
            {
                case AppHighlightingMode.Background:
                    return new ColorPalette( BackgroundModePalette.Select( c => c.ToString( "x6" ) ).ToArray(), Black, "0" );

                case AppHighlightingMode.Foreground:
                    return new ColorPalette( White, ForegroundModePalette.Select( c => c.ToString( "x6" ) ).ToArray(), "1" );

                case AppHighlightingMode.None:
                default:
                    return null;
            }
        }

        private static string[] White
             => Enumerable.Repeat( "FFFFFF", NUMBER_OF_COLORS_PER_SET ).ToArray();

        private static string[] Black
             => Enumerable.Repeat( "000000", NUMBER_OF_COLORS_PER_SET ).ToArray();

        private static int[] ForegroundModePalette
            => new int[]
            {
               0x000000,
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
                0xFFFFFF,
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
