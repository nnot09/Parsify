using Parsify.Models;
using Parsify.Other;
using Parsify.XmlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Parsify.Core
{
    public class DocumentParserCache
    {
        // Structure goes by <ParsifyModule-Collection of Document Hashes>
        private Dictionary<ParsifyModule, List<byte[]>> _cache;
        public DocumentParserCache()
        {
            _cache = new Dictionary<ParsifyModule, List<byte[]>>();
        }

        public ParsifyModule GetCachedModuleOrNull( byte[] hash )
        {
            if ( hash == null )
                return null;

            foreach ( var entry in _cache )
            {
                var docCached = entry.Value.Any( d => hash.CompareHash( d ) );
                if ( docCached )
                    return entry.Key;
            }

            return null;
        }

        public void AddOrUpdate( ParsifyModule parser, Document doc )
        {
            // Reference should be consistent same since this some serialized object
            if ( !_cache.ContainsKey( parser ) )
            {
                _cache.Add( parser, new List<byte[]>() );
            }

            if ( !_cache[ parser ].Any( d => doc.Hash.CompareHash( d ) ) )
            {
                _cache[ parser ].Add( doc.Hash );
            }
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}
