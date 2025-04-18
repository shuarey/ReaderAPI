﻿using Newtonsoft.Json;
using static ReaderAPI.Models.BaseClasses;

namespace ReaderAPI.Models
{
    #region Data classes
    public class AccountUser
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LockedTimestamp { get; set; }
    }
    #endregion

    #region Request / Response classes
    public class AccountUserRequestBase
    {
        public string email { get; set; }
        [JsonIgnore]
        public string password { get; set; }
    }
    public class AccountUserLoginPOSTRequest : AccountUserRequestBase { }

    public class AccountUserRegisterPOSTRequest : AccountUserRequestBase
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class AccountUserPOSTResponse : BaseResponse
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
    #endregion
}