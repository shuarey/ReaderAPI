namespace ReaderAPI.Models
{
    public class AccountUser
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LockedTimestamp { get; set; }
    }
}