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

        public void ReturnBook(BookStock bookStock)
        {
            using (var context = new LibraryContext())
            {
                var borrower = bookStock.OnLoanTo;

                // Clear the loan details
                bookStock.OnLoanTo = null;
                bookStock.LoanEndDate = null;

                context.SaveChanges();
            }
                
        }


    }
}
