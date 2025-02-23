using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ILoanRepository
    {
        public List<LoanDetail> GetActiveLoans();
        public BookStock ReturnBook(Guid bookStockId);
        public List<Fine> GetFinesByBorrower(Guid borrowerId);
    }
}
