using System.Text.Json;
using System.Text.Json.Serialization;
using static ReaderAPI.Models.RequestClasses;
using static ReaderAPI.Models.ResponseClasses;

namespace ReaderAPI.Utilities
{
    public static class Utility
    {
        public static string FormatJson ( object json, Type? type = null )
        {
            if ( json is string jsonString )
            {
                if ( string.IsNullOrEmpty ( jsonString ) )
                    return string.Empty;

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

    public interface ITypeResolver
    {
        Type GetRequestType ( string path );
        Type GetResponseType ( string path );
    }

    public class TypeResolver : ITypeResolver
    {
        private readonly Dictionary<string, Type> _requestTypeMap = new ( )
        {
           { "/accountuser/login", typeof ( AccountUserLoginPOSTRequest ) },
           { "/accountuser/register", typeof ( AccountUserRegisterPOSTRequest ) }
        };

        private readonly Dictionary<string, Type> _responseTypeMap = new ( )
        {
           { "/accountuser/login", typeof ( AccountUserPOSTResponse ) },
           { "/accountuser/register", typeof ( AccountUserPOSTResponse ) }
        };

        public Type GetRequestType ( string path ) => _requestTypeMap.GetValueOrDefault ( path );
        public Type GetResponseType ( string path ) => _responseTypeMap.GetValueOrDefault ( path );
    }
}
