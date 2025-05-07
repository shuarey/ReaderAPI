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
    }
}
