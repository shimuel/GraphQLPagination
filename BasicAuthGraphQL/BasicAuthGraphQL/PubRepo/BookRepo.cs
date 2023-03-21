using System.Reactive.Linq;
using BasicAuthGraphQL.Domain;
using static System.Reflection.Metadata.BlobBuilder;

namespace BasicAuthGraphQL.PubRepo
{
    public class BookRepo
    {
        //private readonly List<Book> _books = new() {
        //    new Book() { Id = "ISBN1", Name = "Book1", AuthorId = 1},
        //    new Book() { Id ="ISBN2", Name = "Book2" , AuthorId = 1},
        //    new Book() { Id ="ISBN3", Name = "Book3", AuthorId = 2 }
        //};
        private readonly List<Book> _books = new();
        private int _id = 3;

        public Task<IEnumerable<Book>> BooksAsync
        {
            get
            {
                IEnumerable<Book> books;
                lock (_books)
                    books = _books.ToList();
                return Task.FromResult(books);
            }
        }

        public Book?this[string id]
        {
            get
            {
                Book? book = null;
                lock (_books)
                    book = _books.FirstOrDefault(c => c.Id == id);
                return book;
            }
        }

        public Task<Book> AddAsync(string name)
        {
            var newId = $"ISBN{Interlocked.Increment(ref _id)}";
            var book = new Book() { Id = newId , Name = name};
            lock (_books)
                _books.Add(book);
            return Task.FromResult(book);
        }

        public Task AddBookAsync(Book book)
        {
            lock (_books)
            {
                _books.Add(book);
                return Task.FromResult(book);
            }
        }

        public Task<Book?> RemoveAsync(string id)
        {
            Book? book = null;
            lock (_books)
            {
                for (int i = 0; i < _books.Count; i++)
                {
                    if (_books[i].Id == id)
                    {
                        book = _books[i];
                        _books.RemoveAt(i);
                        break;
                    }
                }
            }
            return Task.FromResult(book);
        }

        public Task<IEnumerable<Book>> GetBooksByAuthorAsync(int id)
        {
            List<Book> authorsBooks = new();

            for (int i = 0; i < _books.Count; i++)
            {
                if (_books[i].AuthorId == id)
                {
                    authorsBooks.Add(_books[i]);
                }
            }

            return Task.FromResult(authorsBooks.AsEnumerable());
        }

        public Task<Book?> GetBookAsync(string id)
        {
            Book? book = null;
            lock (_books)
            {
                for (int i = 0; i < _books.Count; i++)
                {
                    if (_books[i].Id == id)
                    {
                        book = _books[i];
                        break;
                    }
                }
            }
            return Task.FromResult(book);
        }

        public IObservable<Book> BooksObservable()
            => _books.ToObservable();
    }
}