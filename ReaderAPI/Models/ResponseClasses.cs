using static ReaderAPI.Models.BaseClasses;

namespace ReaderAPI.Models
{
    public class ResponseClasses
    {
        public class AccountUserPOSTResponse : BaseResponse
        {
            public string id { get; set; }
            public string user_name { get; set; }
        }

        public class AccountUserGETResponse : BaseResponse
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
        }
    }
}
