using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Helpers
{
    public class NPADefinitions
    {
        public Dictionary<KeyboardHintType, IEnumerable<string>> Comments { get; set; }

        public Dictionary<Guid, string> ApiEndpoints { get; set; }

        public static NPADefinitions Load()
        {
            var assembly = typeof( App ).GetTypeInfo().Assembly;
            var definitionsPath = assembly.GetManifestResourceNames().FirstOrDefault( r => r.EndsWith( Constants.DefinitionsFileName ) );
            using( var reader = new StreamReader( assembly.GetManifestResourceStream( definitionsPath ) ) )
            {
                return JsonConvert.DeserializeObject<NPADefinitions>( reader.ReadToEnd() );
            }
        }
    }
}
