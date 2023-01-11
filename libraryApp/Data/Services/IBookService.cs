using libraryApp.Data.Models;

namespace libraryApp.Data.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();

        Book Add(Book newBook);

        Book GetById(Guid id);

        void Delete(Guid id);
    }
}
