using System.Reactive.Linq;
using BasicAuthGraphQL.Domain;

namespace BasicAuthGraphQL.PubRepo
{
    public class BookRepo
    {
        private readonly List<Book> _books = new() {
            //new Book() { Id = "ISBN1", Name = "Book1" },
            //new Book() { Id ="ISBN2", Name = "Book2" },
            //new Book() { Id ="ISBN3", Name = "Book3" }
        };

        private int _id = 3;

        public IEnumerable<Book> Books
        {
            get
            {
                IEnumerable<Book> books;
                lock (_books)
                    books = _books.ToList();
                return books;
            }
        }

        public Book? this[string id]
        {
            get
            {
                Book? book = null;
                lock (_books)
                    book = _books.FirstOrDefault(c => c.Id == id);
                return book;
            }
        }

        public Book Add(string name)
        {
            var newId = $"ISBN{Interlocked.Increment(ref _id)}";
            var book = new Book() { Id = newId , Name = name};
            lock (_books)
                _books.Add(book);
            return book;
        }

        public Book? Remove(string id)
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
            return book;
        }

        public IObservable<Book> BooksObservable()
            => _books.ToObservable();
    }
}