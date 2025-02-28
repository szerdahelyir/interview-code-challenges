using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IFineRepository _fineRepository;
        private readonly ICatalogueRepository _catalougeRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBorrowerRepository _borrowerRepository;

        public LoanService(ILoanRepository loanRepository, IFineRepository fineRepository, ICatalogueRepository catalogueRepository, IReservationRepository reservationRepository, IBorrowerRepository borrowerRepository)
        {
            _loanRepository = loanRepository;
            _fineRepository = fineRepository;
            _catalougeRepository = catalogueRepository;
            _reservationRepository = reservationRepository;
            _borrowerRepository = borrowerRepository;
        }

        //Extend the "On Loan" end point to allow books on loan to be returned.
        public BookStock ReturnBook(Guid bookStockId)
        {
            using (var context = new LibraryContext())
            {
                var bookStock = _catalougeRepository.GetBookStockById(bookStockId);

                if (bookStock == null || bookStock.OnLoanTo == null)
                {
                    return null;
                }

                // Check for overdue fine
                // If books are returned after their loan end date then a fine should be raised against the borrower (data model for fines and relationships with borrowers are left to the candidate to define)
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

                var nextReservation = _reservationRepository.GetNextReservation(bookStock.Book.Id);
                if (nextReservation != null)
                {
                    // Assign the book to the next borrower in the queue
                    var borrower = _borrowerRepository.GetBorrowerById(nextReservation.BorrowerId);
                    bookStock.OnLoanTo = borrower;
                    bookStock.LoanEndDate = DateTime.UtcNow.AddDays(14); // New loan period

                    _reservationRepository.RemoveReservation(nextReservation.Id);
                }
                else
                {
                    // No reservations, book is available
                    bookStock.OnLoanTo = null;
                    bookStock.LoanEndDate = null;
                }

                context.SaveChanges();
            

                return bookStock;
            }
        }
    }
}
