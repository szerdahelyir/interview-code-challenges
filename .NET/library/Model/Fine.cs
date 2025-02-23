namespace OneBeyondApi.Model
{
    public class Fine
    {
        public Guid Id { get; set; }
        // How much the borrower has to pay in dollars.
        public decimal Amount { get; set; }
        // The date the borrower got the fine.
        public DateTime FineDate { get; set; }
        public Borrower Borrower { get; set; }
    }
}
