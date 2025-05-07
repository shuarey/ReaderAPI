namespace ReaderAPI.Models
{
    public class UserPlan
    {
        public string ID { get; set; }
        public AccountUser User { get; set; }
        public Plan Plan { get; set; }
    }
}
