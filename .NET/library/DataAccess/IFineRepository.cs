using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IFineRepository
    {
        public List<Fine> GetFinesByBorrower(Guid borrowerId);
        public void AddFine(Fine fine);
    }
}
