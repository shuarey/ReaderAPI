using System.Text.Json;

namespace ReaderAPI.Utilities
{
    public static class Utility
    {
        public static string FormatJson ( string json )
        {
            using var doc = JsonDocument.Parse ( json );
            return JsonSerializer.Serialize ( doc, new JsonSerializerOptions { WriteIndented = true } );
        }

        public static string FormatJson ( object json )
        {
            return JsonSerializer.Serialize ( json, new JsonSerializerOptions { WriteIndented = true } );
        }
    }
}
