using System.Collections.Generic;

namespace NPAInspectionWriter.Helpers
{
    public static class KeyboardHints
    {
        private static Dictionary<KeyboardHintType, IEnumerable<string>> HintLookup = null;

        public static IEnumerable<string> GetHints( KeyboardHintType lookup )
        {
            if( HintLookup == null ) HintLookup = NPADefinitions.Load().Comments;

            return HintLookup?[ lookup ] ?? new List<string>();
        }
    }
}
