namespace ReaderAPI.Models
{
    public class Passage
    {
        public string ID { get; set; }
        public string BookID { get; set; }
        public int ChapterStart { get; set; }
        public int ChapterEnd { get; set; }
        public int VerseStart { get; set; }
        public int VerseEnd { get; set; }
        public string VersionID { get; set; }
        public string CurrentPosition { get; set; } // ex. 33:44, 1:4
    }
}
