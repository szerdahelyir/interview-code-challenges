using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class LoanRepository : ILoanRepository
    {
        public LoanRepository()
        {
        }

        //Add an "On Loan" end point with functionality to get/query the details of all borrowers with active loans and the titles of books they have on loan.
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

    }
}
