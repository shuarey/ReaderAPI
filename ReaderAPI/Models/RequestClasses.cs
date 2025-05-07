using Newtonsoft.Json;

namespace ReaderAPI.Models
{
    public class RequestClasses
    {
        public class AccountUserLoginPOSTRequest
        {
            public string email { get; set; }
            [JsonIgnore]
            public string password { get; set; }
        }

        public class AccountUserRegisterPOSTRequest
        {
            public string email { get; set; }
            [JsonIgnore]
            public string password { get; set; }
        }
    }
}
