using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class BookRepository : IBookRepository
    {
        public BookRepository()
        {
        }
        public List<Book> GetBooks()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Books
                    .ToList();
                return list;
            }
        }

        public Guid AddBook(Book book)
        {
            using (var context = new LibraryContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
                return book.Id;
            }
        }

        public Book GetBookById(Guid bookId)
        {
            using (var context = new LibraryContext())
            {
                var book = context.Books.Where(b => b.Id == bookId).FirstOrDefault();
                return book;
            }
        }
    }
}
