using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IFineRepository _fineRepository;
        private readonly ICatalogueRepository _catalougeRepository;

        public LoanService(ILoanRepository loanRepository, IFineRepository fineRepository, ICatalogueRepository catalogueRepository)
        {
            _loanRepository = loanRepository;
            _fineRepository = fineRepository;
            _catalougeRepository = catalogueRepository;
        }

        public BookStock ReturnBook(Guid bookStockId)
        {
            var bookStock = _catalougeRepository.GetBookStockById(bookStockId);

            if (bookStock == null || bookStock.OnLoanTo == null)
            {
                return null;
            }

            // Check for overdue fine
            if (bookStock.LoanEndDate.HasValue && bookStock.LoanEndDate.Value.Date < DateTime.UtcNow.Date)
            {
                int overdueDays = (DateTime.UtcNow.Date - bookStock.LoanEndDate.Value.Date).Days;
                decimal fineAmount = overdueDays * 5;

                var fine = new Fine
                {
                    Amount = fineAmount,
                    Borrower = bookStock.OnLoanTo,
                    FineDate = DateTime.UtcNow
                };

                _fineRepository.AddFine(fine);
            }

            _loanRepository.ReturnBook(bookStock);

            return bookStock;
        }
    }
}
