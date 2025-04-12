namespace ReaderAPI.Models
{
    public class Login
    {
        public string ID { get; set; }
        public string AcctUserID { get; set; }
        public DateTime LoginTimestamp { get; set; }
        public bool IsSuccessful { get; set; }
        public string IPAddress { get; set; }
    }
}