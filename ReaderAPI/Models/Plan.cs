namespace ReaderAPI.Models
{
    public class Plan
    {
        public string ID { get; set; }
        public PlanPart [] PlanParts { get; set; }
        public int PercentComplete { get; set; } //not based on number of plan parts, but on the aggregate of word count in all verses and UserText. Not sure if this is the exact place to store this data. Is it determined at this point? Should it be determined in the Passage level? What about the UserText?
    }

    public class PlanPart
    {
        public string ID { get; set; }
        //PlanPart elements will only be a Passage or a UserText. Is there a better way to construct this object?

        public Passage Passage { get; set; }
        public string UserText { get; set; } // can contain links. How do I do this?
        public int WordCount { get; set; }
        public int Ordinal { get; set; }
    }
}
