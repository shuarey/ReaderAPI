using ReaderAPI.Models;

namespace ReaderAPI.Services
{
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
