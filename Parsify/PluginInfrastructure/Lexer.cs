﻿using Kbg.NppPluginNET.PluginInfrastructure;
using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Parsify.PluginInfrastructure
{
    // Credits: github.com/BdR76

    internal static class Lexer
    {
        public static readonly string Name = "Parsify\0";
        public static readonly string StatusText = "Parsify - Text parser highlighting\0";
        public static List<ParsifyLine> Lines { get; set; } = new List<ParsifyLine>();
        public static bool RefreshLexerState = false;

        static readonly Dictionary<string, bool> SupportedProperties = new Dictionary<string, bool>
        {
            { "nnot09", false }
        };

        static readonly Dictionary<string, string> PropertyDescription = new Dictionary<string, string>
        {
            { "nnot09", "Refreshes scintilla lexer ghetto approach" }
        };
        static readonly Dictionary<string, int> PropertyTypes = new Dictionary<string, int>
        {
            { "nnot09", (int)SciMsg.SC_TYPE_BOOLEAN }
        };

        static List<string> NamedStylesList = new List<string> { "SCE_TEXTFORMAT_DEFAULT" };
        static int NamedStylesListCount = NamedStylesList.Count;
        static List<string> TagsOfStyleList = new List<string> { "default" };
        static List<string> DescriptionOfStyleList = new List<string> { "Default style" };

        #region IDocument
        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentVersion( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void IDocumentSetErrorStatus( IntPtr instance, int status );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr IDocumentLength( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void IDocumentGetCharRange( IntPtr instance, IntPtr buffer, IntPtr position, IntPtr lengthRetrieve );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate char IDocumentStyleAt( IntPtr instance, IntPtr position );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr IDocumentLineFromPosition( IntPtr instance, IntPtr position );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr IDocumentLineStart( IntPtr instance, IntPtr line );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentGetLevel( IntPtr instance, IntPtr line );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentSetLevel( IntPtr instance, IntPtr line, int level );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentGetLineState( IntPtr instance, IntPtr line );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentSetLineState( IntPtr instance, IntPtr line, int state );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void IDocumentStartStyling( IntPtr instance, IntPtr position );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate bool IDocumentSetStyleFor( IntPtr instance, IntPtr length, char style );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate bool IDocumentSetStyles( IntPtr instance, IntPtr length, IntPtr styles );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void IDocumentDecorationSetCurrentIndicator( IntPtr instance, int indicator );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void IDocumentDecorationFillRange( IntPtr instance, IntPtr position, int value, IntPtr fillLength );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void IDocumentChangeLexerState( IntPtr instance, IntPtr start, IntPtr end );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentCodePage( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate bool IDocumentIsDBCSLeadByte( IntPtr instance, char ch );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr IDocumentBufferPointer( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentGetLineIndentation( IntPtr instance, IntPtr line );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr IDocumentLineEnd( IntPtr instance, IntPtr line );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr IDocumentGetRelativePosition( IntPtr instance, IntPtr positionStart, IntPtr characterOffset );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int IDocumentGetCharacterAndWidth( IntPtr instance, IntPtr position, IntPtr pWidth );


        [StructLayout( LayoutKind.Sequential )]
        public struct IDocumentVtable
        {
            public IDocumentVersion Version;
            public IDocumentSetErrorStatus SetErrorStatus;
            public IDocumentLength Length;
            public IDocumentGetCharRange GetCharRange;
            public IDocumentStyleAt StyleAt;
            public IDocumentLineFromPosition LineFromPosition;
            public IDocumentLineStart LineStart;
            public IDocumentGetLevel GetLevel;
            public IDocumentSetLevel SetLevel;
            public IDocumentGetLineState GetLineState;
            public IDocumentSetLineState SetLineState;
            public IDocumentStartStyling StartStyling;
            public IDocumentSetStyleFor SetStyleFor;
            public IDocumentSetStyles SetStyles;
            public IDocumentDecorationSetCurrentIndicator DecorationSetCurrentIndicator;
            public IDocumentDecorationFillRange DecorationFillRange;
            public IDocumentChangeLexerState ChangeLexerState;
            public IDocumentCodePage CodePage;
            public IDocumentIsDBCSLeadByte IsDBCSLeadByte;
            public IDocumentBufferPointer BufferPointer;
            public IDocumentGetLineIndentation GetLineIndentation;
            public IDocumentLineEnd LineEnd;
            public IDocumentGetRelativePosition GetRelativePosition;
            public IDocumentGetCharacterAndWidth GetCharacterAndWidth;
        }

        [StructLayout( LayoutKind.Sequential )]
        public struct IDocument
        {
            public IntPtr VTable;
        }
        #endregion IDocument

        #region ILexer
        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerVersion( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void ILexerRelease( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerPropertyNames( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerPropertyType( IntPtr instance, IntPtr name );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerDescribeProperty( IntPtr instance, IntPtr name );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerPropertySet( IntPtr instance, IntPtr key, IntPtr val );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerDescribeWordListSets( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerWordListSet( IntPtr instance, int kw_list_index, IntPtr key_word_list );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void ILexerLex( IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void ILexerFold( IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerPrivateCall( IntPtr instance, int operation, IntPtr pointer );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerLineEndTypesSupported( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerAllocateSubStyles( IntPtr instance, int style_base, int number_styles );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerSubStylesStart( IntPtr instance, int style_base );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerSubStylesLength( IntPtr instance, int style_base );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerStyleFromSubStyle( IntPtr instance, int sub_style );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerPrimaryStyleFromStyle( IntPtr instance, int style );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void ILexerFreeSubStyles( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate void ILexerSetIdentifiers( IntPtr instance, int style, IntPtr identifiers );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerDistanceToSecondaryStyles( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerGetSubStyleBases( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate int ILexerNamedStyles( IntPtr instance );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerNameOfStyle( IntPtr instance, int style );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerTagsOfStyle( IntPtr instance, int style );

        [UnmanagedFunctionPointer( CallingConvention.StdCall )]
        public delegate IntPtr ILexerDescriptionOfStyle( IntPtr instance, int style );

        [StructLayout( LayoutKind.Sequential )]
        public struct ILexer4
        {
            public ILexerVersion Version;
            public ILexerRelease Release;
            public ILexerPropertyNames PropertyNames;
            public ILexerPropertyType PropertyType;
            public ILexerDescribeProperty DescribeProperty;
            public ILexerPropertySet PropertySet;
            public ILexerDescribeWordListSets DescribeWordListSets;
            public ILexerWordListSet WordListSet;
            public ILexerLex Lex;
            public ILexerFold Fold;
            public ILexerPrivateCall PrivateCall;
            public ILexerLineEndTypesSupported LineEndTypesSupported;
            public ILexerAllocateSubStyles AllocateSubStyles;
            public ILexerSubStylesStart SubStylesStart;
            public ILexerSubStylesLength SubStylesLength;
            public ILexerStyleFromSubStyle StyleFromSubStyle;
            public ILexerPrimaryStyleFromStyle PrimaryStyleFromStyle;
            public ILexerFreeSubStyles FreeSubStyles;
            public ILexerSetIdentifiers SetIdentifiers;
            public ILexerDistanceToSecondaryStyles DistanceToSecondaryStyles;
            public ILexerGetSubStyleBases GetSubStyleBases;
            public ILexerNamedStyles NamedStyles;
            public ILexerNameOfStyle NameOfStyle;
            public ILexerTagsOfStyle TagsOfStyle;
            public ILexerDescriptionOfStyle DescriptionOfStyle;
        }
        #endregion ILexer

        static ILexer4 ilexer4 = new ILexer4 { };
        static IntPtr vtable_pointer = IntPtr.Zero;

        public static IntPtr ILexerImplementation()
        {
            if ( vtable_pointer == IntPtr.Zero )
            {
                // simulate a c++ vtable by creating an array of 25 function pointers
                ilexer4.Version = new ILexerVersion( Version );
                ilexer4.Release = new ILexerRelease( Release );
                ilexer4.PropertyNames = new ILexerPropertyNames( PropertyNames );
                ilexer4.PropertyType = new ILexerPropertyType( PropertyType );
                ilexer4.DescribeProperty = new ILexerDescribeProperty( DescribeProperty );
                ilexer4.PropertySet = new ILexerPropertySet( PropertySet );
                ilexer4.DescribeWordListSets = new ILexerDescribeWordListSets( DescribeWordListSets );
                ilexer4.WordListSet = new ILexerWordListSet( WordListSet );
                ilexer4.Lex = new ILexerLex( Lex );
                ilexer4.Fold = new ILexerFold( Fold );
                ilexer4.PrivateCall = new ILexerPrivateCall( PrivateCall );
                ilexer4.LineEndTypesSupported = new ILexerLineEndTypesSupported( LineEndTypesSupported );
                ilexer4.AllocateSubStyles = new ILexerAllocateSubStyles( AllocateSubStyles );
                ilexer4.SubStylesStart = new ILexerSubStylesStart( SubStylesStart );
                ilexer4.SubStylesLength = new ILexerSubStylesLength( SubStylesLength );
                ilexer4.StyleFromSubStyle = new ILexerStyleFromSubStyle( StyleFromSubStyle );
                ilexer4.PrimaryStyleFromStyle = new ILexerPrimaryStyleFromStyle( PrimaryStyleFromStyle );
                ilexer4.FreeSubStyles = new ILexerFreeSubStyles( FreeSubStyles );
                ilexer4.SetIdentifiers = new ILexerSetIdentifiers( SetIdentifiers );
                ilexer4.DistanceToSecondaryStyles = new ILexerDistanceToSecondaryStyles( DistanceToSecondaryStyles );
                ilexer4.GetSubStyleBases = new ILexerGetSubStyleBases( GetSubStyleBases );
                ilexer4.NamedStyles = new ILexerNamedStyles( NamedStyles );
                ilexer4.NameOfStyle = new ILexerNameOfStyle( NameOfStyle );
                ilexer4.TagsOfStyle = new ILexerTagsOfStyle( TagsOfStyle );
                ilexer4.DescriptionOfStyle = new ILexerDescriptionOfStyle( DescriptionOfStyle );
                IntPtr vtable = Marshal.AllocHGlobal( Marshal.SizeOf( ilexer4 ) );
                Marshal.StructureToPtr( ilexer4, vtable, false );
                vtable_pointer = Marshal.AllocHGlobal( Marshal.SizeOf( vtable ) );
                Marshal.StructureToPtr( vtable, vtable_pointer, false );
            }
            // return the address of the fake vtable
            return vtable_pointer;
        }

        // virtual int SCI_METHOD Version() const = 0
        public static int Version( IntPtr instance )
        {
            /*returns an enumerated value specifying which version of the interface is implemented: 
             * lvRelease5 for ILexer5 and lvRelease4 for ILexer4. 
             * ILexer5 must be provided for Scintilla version 5.0 or later.
             */
            //GC.Collect();  // test to see if the methods do get garbage collected
            return 2;
        }

        // virtual void SCI_METHOD Release() = 0
        public static void Release( IntPtr instance )
        {
            // is called to destroy the lexer object.
        }

        // virtual const char * SCI_METHOD PropertyNames() = 0
        public static IntPtr PropertyNames( IntPtr instance )
        {
            /*  returns a string with all of the valid properties separated by "\n".
             *  If the lexer does not support this call then an empty string is returned. 
             */
            return Marshal.StringToHGlobalAnsi( string.Join( Environment.NewLine, SupportedProperties.Keys ) );
        }

        // virtual int SCI_METHOD PropertyType(const char *name) = 0
        public static int PropertyType( IntPtr instance, IntPtr name )
        {
            // Properties may be boolean (SC_TYPE_BOOLEAN), integer (SC_TYPE_INTEGER), or string (SC_TYPE_STRING) 
            return PropertyTypes[ Marshal.PtrToStringAnsi( name ) ];
        }

        // virtual const char * SCI_METHOD DescribeProperty(const char *name) = 0
        public static IntPtr DescribeProperty( IntPtr instance, IntPtr name )
        {
            // A description of a property in English
            return Marshal.StringToHGlobalAnsi( PropertyDescription[ Marshal.PtrToStringAnsi( name ) ] );
        }

        // virtual i64 SCI_METHOD PropertySet(const char *key, const char *val) = 0
        public static IntPtr PropertySet( IntPtr instance, IntPtr key, IntPtr val )
        {
            /* The return values from PropertySet and WordListSet are used to indicate 
             * whether the change requires performing lexing or folding over any of the document. 
             * It is the position at which to restart lexing and folding or -1 
             * if the change does not require any extra work on the document. 
             * A simple approach is to return 0 if there is any possibility that a change 
             * requires lexing the document again while an optimisation could be to remember 
             * where a setting first affects the document and return that position.
             */
            string name = Marshal.PtrToStringAnsi( key );
            string value = Marshal.PtrToStringAnsi( val );

            if ( !SupportedProperties.ContainsKey( name ) )
                return IntPtr.Zero;

#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] PropertySet: {name} Value: {value}" );
#endif

            SupportedProperties[ name ] = value == "0" ? false : true;

            return IntPtr.Zero;
        }

        // virtual const char * SCI_METHOD DescribeWordListSets() = 0
        public static IntPtr DescribeWordListSets( IntPtr instance )
        {
            // ? A string describing the different keywords, but how to separate is the question
            // return Marshal.StringToHGlobalAnsi( KeywordDescription );
            return Marshal.StringToHGlobalAnsi( "" );
        }

        // virtual i64 SCI_METHOD WordListSet(int n, const char *wl) = 0
        public static IntPtr WordListSet( IntPtr instance, int kw_list_index, IntPtr key_word_list )
        {
            return (IntPtr)kw_list_index;
        }

        // virtual void SCI_METHOD Lex(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Lex( IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] Lex()" );
#endif

            /* main lexing method. 
             * start_pos is always the startposition of a line
             * length_doc is NOT the total length of a document but the size of the text to be styled
             * init_style is the style of last styled byte
             * p_access is the pointer of the IDocument cpp class
             */

            int length = (int)length_doc;
            int start = (int)start_pos;

            // allocate a buffer
            IntPtr buffer_ptr = Marshal.AllocHGlobal( length );
            if ( buffer_ptr == IntPtr.Zero ) { return; }

            // create the IDocument interface (struct) from the provided p_access pointer
            IDocument idoc = (IDocument)Marshal.PtrToStructure( p_access, typeof( IDocument ) );
            // create/simulate the vtable of the IDocument interface
            IDocumentVtable vtable = (IDocumentVtable)Marshal.PtrToStructure( (IntPtr)idoc.VTable, typeof( IDocumentVtable ) );

            // scintilla fills the allocated buffer
            vtable.GetCharRange( p_access, buffer_ptr, (IntPtr)start, (IntPtr)length );
            if ( buffer_ptr == IntPtr.Zero ) { return; }

            // convert the buffer into a managed string
            string content = Marshal.PtrToStringAnsi( buffer_ptr, length );

            int styleId = 2;
            for ( int i = 0; i < length; i++ )
            {
                //if ( content[ i ] == '\n' || content[ i ] == '\r' )
                //{
                //    vtable.StartStyling( p_access, (IntPtr)( i ) );
                //    vtable.SetStyleFor( p_access, (IntPtr)( i + 1 ), (char)0 );
                //    continue;
                //}

                int startPosition = i;
                string indexedContent = content.Substring( i );

                var dataLine = Lines.Where( l => indexedContent.StartsWith( l.StartsWithIdentifier ) ).FirstOrDefault();

                if ( dataLine == null )
                    continue;

                styleId = 2;

                i += dataLine.StartsWithIdentifier.Length;

                int fromDocumentLineStartPosition = start + startPosition;
                int toDocumentLineStartPosition = i - startPosition;

                vtable.StartStyling( p_access, (IntPtr)( fromDocumentLineStartPosition ) );
                vtable.SetStyleFor( p_access, (IntPtr)( toDocumentLineStartPosition ), (char)1 );

                foreach ( var field in dataLine.Fields )
                {
                    int fromDocumentFieldPosition = i;
                    int toDocumentFieldPosition = i + field.Length;

                    vtable.StartStyling( p_access, (IntPtr)( fromDocumentFieldPosition ) );
                    vtable.SetStyleFor( p_access, (IntPtr)( toDocumentFieldPosition ), (char)styleId );

                    i += field.Length;

                    styleId++;
                    if ( styleId > 11 )
                        styleId = 2;
                }

                int remaining = i;
                if ( remaining < content.Length && content[ remaining ] != '\n' )
                {
                    vtable.StartStyling( p_access, (IntPtr)( i ) );
                    for ( ; remaining < length && content[ remaining ] != '\n'; remaining++ ) ;

                    bool hasCarriageReturn = content[ remaining - 1 ] == '\r';

                    int delta = remaining - i;
                    if ( delta > 0 )
                    {
                        int startDefaultIndex = remaining;
                        
                        if ( hasCarriageReturn )
                            startDefaultIndex--;

                        vtable.SetStyleFor( p_access, (IntPtr)( startDefaultIndex ), (char)0 );
                        i += delta;
                    }
                }
            }

            // free allocated buffer
            Marshal.FreeHGlobal( buffer_ptr );
        }

        // virtual void SCI_METHOD Fold(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Fold( IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access )
        {
        }

        // virtual int SCI_METHOD NamedStyles() = 0;
        public static int NamedStyles( IntPtr instance )
        {
            // Retrieve the number of named styles for the lexer.
            return NamedStylesListCount;
        }

        // virtual const char * SCI_METHOD NameOfStyle(int style) = 0;
        public static IntPtr NameOfStyle( IntPtr instance, int style )
        {
            // Retrieve the name of a style.
            return Marshal.StringToHGlobalAnsi( NamedStylesList[ style ] );
        }

        // virtual const char * SCI_METHOD TagsOfStyle(int style) = 0;
        public static IntPtr TagsOfStyle( IntPtr instance, int style )
        {
            /*
             * Retrieve the tags of a style.
             * This is a space-separated set of words like "comment documentation".
             */
            return Marshal.StringToHGlobalAnsi( TagsOfStyleList[ style ] );
        }

        // virtual const char * SCI_METHOD DescriptionOfStyle(int style) = 0;
        public static IntPtr DescriptionOfStyle( IntPtr instance, int style )
        {
            /* 
             * Retrieve an English-language description of a style which may be suitable 
             * for display in a user interface.
             * This looks like "Doc comment: block comments beginning with /** or /*!".
             */
            return Marshal.StringToHGlobalAnsi( DescriptionOfStyleList[ style ] );
        }

        // virtual int SCI_METHOD LineEndTypesSupported() = 0;
        public static int LineEndTypesSupported( IntPtr instance )
        {
            /*
             *  reports the different types of line ends supported by the current lexer. 
             *  This is a bit set although there is currently only a single choice
             *  with either SC_LINE_END_TYPE_DEFAULT (0) or SC_LINE_END_TYPE_UNICODE (1). 
             *  These values are also used by the other messages concerned with Unicode line ends.
             */
            return (int)SciMsg.SC_LINE_END_TYPE_DEFAULT;
        }

        // virtual void * SCI_METHOD PrivateCall(int operation, void *pointer) = 0;
        public static IntPtr PrivateCall( IntPtr instance, int operation, IntPtr pointer )
        {
            /* allows for direct communication between the application and a lexer.
             * An example would be where an application maintains a single large data structure 
             * containing symbolic information about system headers(like Windows.h) 
             * and provides this to the lexer where it can be applied to each document.
             * This avoids the costs of constructing the system header information for each document.
             * This is invoked with the SCI_PRIVATELEXERCALL API.
             */
            return IntPtr.Zero;
        }


        /***********************************************************************************************
         * 
         * NOTE:
         * 
         * THE FOLLOWING METHODS ARE NEEDED ONLY IF THE DOCUMENT IS TO BE LEXED WITH DIFFERENT LEXERS,
         * LIKE THE HTML LEXER DOES, WHICH BESIDES HTML CODE ALSO HAS TO LEX PHP, JS etc.
         * 
         * THE IMPLEMENTATION IS LEFT TO THOSE INTERESTED :)
         * 
         * *********************************************************************************************/


        // virtual int SCI_METHOD AllocateSubStyles(int styleBase, int numberStyles) = 0;
        public static int AllocateSubStyles( IntPtr instance, int style_base, int number_styles )
        {
            /* Allocate some number of substyles for a particular base style, 
             * returning the first substyle number allocated.
             * Substyles are allocated contiguously.
             */
            return -1;
        }

        // virtual int SCI_METHOD SubStylesStart(int styleBase) = 0;
        public static int SubStylesStart( IntPtr instance, int style_base )
        {
            // Return the start of the substyles allocated for a base style.
            return -1;
        }

        // virtual int SCI_METHOD SubStylesLength(int styleBase) = 0;
        public static int SubStylesLength( IntPtr instance, int style_base )
        {
            // Return the length of the substyles allocated for a base style.
            return 0;
        }

        // virtual int SCI_METHOD StyleFromSubStyle(int subStyle) = 0;
        public static int StyleFromSubStyle( IntPtr instance, int sub_style )
        {
            // For a sub style, return the base style, else return the argument.
            return 0;
        }

        // virtual int SCI_METHOD PrimaryStyleFromStyle(int style) = 0;
        public static int PrimaryStyleFromStyle( IntPtr instance, int style )
        {
            // For a secondary style, return the primary style, else return the argument.
            return 0;
        }

        // virtual void SCI_METHOD FreeSubStyles() = 0;
        public static void FreeSubStyles( IntPtr instance )
        {
            // Free all allocated substyles.
        }

        // virtual void SCI_METHOD SetIdentifiers(int style, const char *identifiers) = 0;
        public static void SetIdentifiers( IntPtr instance, int style, IntPtr identifiers )
        {
            /* Similar to SCI_SETKEYWORDS but for substyles. 
             * The prefix feature available with SCI_SETKEYWORDS is not implemented for SCI_SETIDENTIFIERS.
             */
        }

        // virtual int SCI_METHOD DistanceToSecondaryStyles() = 0;
        public static int DistanceToSecondaryStyles( IntPtr instance )
        {
            // Returns the distance between a primary style and its corresponding secondary style.
            return 0;
        }

        // virtual const char * SCI_METHOD GetSubStyleBases() = 0;
        public static IntPtr GetSubStyleBases( IntPtr instance )
        {
            // Fill styles with a byte for each style that can be split into substyles.
            return IntPtr.Zero;
        }
    }
}
