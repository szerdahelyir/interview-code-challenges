namespace OneBeyondApi.Model
{
    public class LoanedBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? LoanEndDate { get; set; }
    }
}
