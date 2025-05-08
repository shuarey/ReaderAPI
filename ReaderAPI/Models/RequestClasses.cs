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

        public class AccountUserPUTRequest
        {
            public string ID { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }// this should eventually be locked behind some kind of validation
            public string password { get; set; }//changing the password will be a link inside the accountuser page
        }
    }
}
