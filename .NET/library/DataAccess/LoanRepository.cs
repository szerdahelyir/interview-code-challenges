using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class LoanRepository : ILoanRepository
    {
        public LoanRepository()
        {
        }

        public List<LoanDetail> GetActiveLoans()
        {
            using (var context = new LibraryContext())
            {
                var activeLoans = context.Catalogue
                    .AsNoTracking()
                    .Where(bs => bs.OnLoanTo != null)
                    .GroupBy(bs => bs.OnLoanTo) 
                    .Select(group => new LoanDetail
                    {
                        BorrowerName = group.Key.Name,
                        BorrowerEmail = group.Key.EmailAddress,
                        BooksOnLoan = group.Select(bs => new LoanedBook
                        {
                            Title = bs.Book.Name,
                            Author = bs.Book.Author.Name,
                            LoanEndDate = bs.LoanEndDate
                        }).ToList()
                    })
                    .ToList();

                return activeLoans;
            }
        }

        public BookStock ReturnBook(Guid bookStockId)
        {
            using (var context = new LibraryContext())
            {
                var bookStock = context.Catalogue
                    .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OnLoanTo).FirstOrDefault(bs => bs.Id == bookStockId);

                if (bookStock == null || bookStock.OnLoanTo == null)
                {
                    return null;
                }

                // Clear the loan details
                bookStock.OnLoanTo = null;
                bookStock.LoanEndDate = null;

                context.SaveChanges();
                return bookStock;
            }
        }

        public List<Fine> GetFinesByBorrower(Guid borrowerId)
        {
            using (var context = new LibraryContext())
            {
                return context.Fines.Include(f=>f.Borrower).Where(f => f.Borrower.Id == borrowerId).ToList();
            }
        }
    }
}
