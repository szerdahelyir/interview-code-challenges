using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IBookRepository
    {
        public List<Book> GetBooks();

        public Guid AddBook(Book book);
        public Book GetBookById(Guid bookId);
    }
}
