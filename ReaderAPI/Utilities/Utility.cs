using System.Text.Json;
using System.Text.Json.Serialization;
namespace ReaderAPI.Utilities
{
    public static class Utility
    {
        public static string FormatJson ( object json, Type? type = null )
        {
            if ( json is string jsonString )
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement> ( jsonString );
                return JsonSerializer.Serialize ( jsonElement, new JsonSerializerOptions
                { 
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault, 
                    WriteIndented = true 
                } );
            }
            else if ( json != null )
            {
                // If the input is an object, serialize it to JSON
                return JsonSerializer.Serialize ( json, type ?? json.GetType ( ), new JsonSerializerOptions 
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                    WriteIndented = true 
                } );
            }

            return string.Empty;
        }
    }
}
