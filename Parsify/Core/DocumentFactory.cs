using Parsify.Core;
using Parsify.Models;
using Parsify.Models.Events.EArgs;
using Parsify.XmlModels;
using System;
using System.Diagnostics;

namespace Parsify.Core
{
    public class DocumentFactory
    {
        public event EventHandler<DocumentParsingEventArgs> DocumentParsingEvent;
        public event EventHandler<DocumentParsedEventArgs> DocumentParsedEvent;
        public event EventHandler<DocumentParseFailedEventArgs> DocumentParseFailedEvent;
        public event EventHandler<DocumentParserChangingEventArgs> DocumentParserChangingEvent;
        public event EventHandler<DocumentParserChangedEventArgs> DocumentParserChangedEvent;
        public event EventHandler<DocumentUnloadingEventArgs> DocumentUnloadingEvent;
        public event EventHandler<DocumentUnloadedEventArgs> DocumentUnloadedEvent;
        public event EventHandler<DocumentPreInitializeEventArgs> DocumentPreInitializeEvent;
        public event EventHandler<DocumentInitializedEventArgs> DocumentInitializedEvent;
        public event EventHandler DocumentChanged;

        public Document Active { get; private set; }
        public DocumentParser DocumentReader { get; set; }
        public DocumentParserCache DocumentParserCache { get; set; }
        private readonly Scintilla _scintilla;
        
        public DocumentFactory( Scintilla scintilla )
        {
            _scintilla = scintilla;
            DocumentParserCache = new DocumentParserCache();
        }

        public void UpdateParser( ParsifyModule parser )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] Executing Update" );
#endif

            if ( OnDocumentParserChanging().Cancel )
                return;

            if ( OnDocumentParsing( _scintilla.GetFilePath(), parser ).Cancel )
                return;

            DocumentReader = new DocumentParser( _scintilla, parser );

            if ( !DocumentReader.Success )
            {
                OnDocumentParseFailed( DocumentReader );
                return;
            }

            OnDocumentParsed( DocumentReader.Document );

            DocumentParserCache.AddOrUpdate( parser, DocumentReader.Document );

            var oldDoc = Unload();

            Active = DocumentReader.Document;

            Initialize( Active );

            OnDocumentParserChanged( oldDoc?.Parser );
        }

        private Document Unload()
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] Unload" );
#endif

            if ( Active == null )
                return null;

            if ( OnDocumentUnloading().Cancel )
                return null;

            PluginInfrastructure.Lexer.Lines.Clear();

            var oldDocument = Active;

            Active = null;

            OnDocumentUnloaded( Active );

            return oldDocument;
        }

        private void Initialize( Document document )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] Initialize" );
#endif

            if ( OnDocumentPreInitalize( document ).Cancel )
                return;

            PluginInfrastructure.Lexer.Lines.AddRange( document.Parser.LineDefinitions );
            _scintilla.GatewaySetProperty( "nnot09", "0" );

            OnDocumentInitialized();
        }

        public void OnDocumentChanged()
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentChanged" );
#endif

            DocumentChanged?.Invoke( this, EventArgs.Empty );
        }

        public DocumentPreInitializeEventArgs OnDocumentPreInitalize( Document document )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentPreInitalize" );
#endif

            var args = new DocumentPreInitializeEventArgs()
            {
                Cancel = false,
                Document = document
            };

            DocumentPreInitializeEvent?.Invoke( this, args );

            return args;
        }

        public void OnDocumentInitialized()
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentInitialized" );
#endif

            DocumentInitializedEvent?.Invoke( this, new DocumentInitializedEventArgs() );
        }

        public DocumentParsingEventArgs OnDocumentParsing( string filePath, ParsifyModule parser )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentParsing" );
#endif

            var args = new DocumentParsingEventArgs()
            {
                Cancel = false,
                FilePath = filePath,
                Parser = parser
            };

            DocumentParsingEvent?.Invoke( this, args );

            return args;
        }

        public void OnDocumentParsed( Document newDocument )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentParsed" );
#endif

            DocumentParsedEvent?.Invoke( this, new DocumentParsedEventArgs()
            {
                Document = newDocument,
                Parser = newDocument.Parser,
            } );
        }

        public DocumentParseFailedEventArgs OnDocumentParseFailed( DocumentParser reader )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentParseFailed" );
#endif

            var args = new DocumentParseFailedEventArgs()
            {
                ErrorText = reader.GetErrors(),
                NumberOfErrors = reader.NumberOfErrors,
            };

            DocumentParseFailedEvent?.Invoke( this, args );

            return args;
        }

        public DocumentParserChangingEventArgs OnDocumentParserChanging()
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentParserChanging" );
#endif

            var args = new DocumentParserChangingEventArgs()
            {
                Cancel = false,
                Parser = Active?.Parser,
                Document = Active,
            };

            args.SetIsFirst();

            DocumentParserChangingEvent?.Invoke( this, args );

            return args;
        }

        public void OnDocumentParserChanged( ParsifyModule old )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentParserChanged" );
#endif

            var args = new DocumentParserChangedEventArgs()
            {
                Document = Active,
                NewParser = Active.Parser,
                OldParser = old
            };

            DocumentParserChangedEvent?.Invoke( this, args );

            return;
        }

        public DocumentUnloadingEventArgs OnDocumentUnloading()
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentUnloading" );
#endif

            var args = new DocumentUnloadingEventArgs()
            {
                Cancel = false,
                Document = Active
            };

            DocumentUnloadingEvent?.Invoke( this, args );

            return args;
        }

        public void OnDocumentUnloaded( Document unloadedDocument )
        {
#if DEBUG
            Debug.WriteLine( $"[{DateTime.Now}] OnDocumentUnloaded" );
#endif

            DocumentUnloadedEvent?.Invoke( this, new DocumentUnloadedEventArgs()
            {
                UnloadedDocument = unloadedDocument
            } );
        }
    }
}
