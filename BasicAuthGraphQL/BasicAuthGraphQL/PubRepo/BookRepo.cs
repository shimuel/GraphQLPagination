using BasicAuthGraphQL.Domain;

namespace BasicAuthGraphQL.PubRepo
{
    public class BookRepo
    {
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

        public Task<Book> AddBookAsync(string bookName, string genre, bool published, Author author)
        {
            lock (_books)
            {
                var id = $"ISBN{Interlocked.Increment(ref _id)}";
                _books.Add(new Book() { Id = id, Name = bookName, AuthorId = author.Id, Published = published, Author = author });
                return GetBookAsync(id);
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
    }
}