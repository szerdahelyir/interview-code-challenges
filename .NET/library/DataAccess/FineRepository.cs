using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class FineRepository : IFineRepository
    {
        public FineRepository()
        {
        }

        public List<Fine> GetFinesByBorrower(Guid borrowerId)
        {
            using (var context = new LibraryContext())
            {
                return context.Fines.Include(f=>f.Borrower).Where(f => f.Borrower.Id == borrowerId).ToList();
            }
        }

        public void AddFine(Fine fine)
        {
            using (var context = new LibraryContext())
            {
                context.Fines.Add(fine);
                context.SaveChanges();
            }
        }
    }
}
