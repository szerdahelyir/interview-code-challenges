using OneBeyondApi.Model;

namespace OneBeyondApi.Services
{
    public interface ILoanService
    {
        public BookStock ReturnBook(Guid bookStockId);
    }
}
