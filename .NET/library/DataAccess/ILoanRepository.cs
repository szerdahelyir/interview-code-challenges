using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ILoanRepository
    {
        public List<LoanDetail> GetActiveLoans();
        
        public void ReturnBook(BookStock bookStock);
    }
}
