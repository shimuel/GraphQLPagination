using BasicAuthGraphQL.Domain;

namespace BasicAuthGraphQL.PubRepo
{
    public class AuthorRepo
    {
        private readonly List<Author> _authors = new List<Author>();
        private int _id = 1;

        public AuthorRepo(BookRepo bookRepo)
        {
            Author author1 = new Author()
                { Id = Interlocked.Increment(ref _id), Name = "Alex", Books = new List<Book>() };
            Author author2 = new Author()
                { Id = Interlocked.Increment(ref _id), Name = "Alex", Books = new List<Book>() };

            _authors.Add(author1);
            _authors.Add(author2);

            author1.Books.ForEach(bk =>
            {
                var book = bookRepo.AddBookAsync(bookName: "Hardy Boys", author1).Result;
                author1.Books.Add(book);
            });
            author2.Books.ForEach(bk =>
            {
                var book = bookRepo.AddBookAsync(bookName: "Nancy Drew", author2).Result;
                author2.Books.Add(book);
            });
        }

        public Task<IEnumerable<Author>> AuthorsAsync
        {
            get
            {
                IEnumerable<Author> authors;
                authors = _authors.ToList();
                return Task.FromResult(authors);
            }
        }

        public Author? this[int id]
        {
            get
            {
                Author? author = null;
                author = _authors.FirstOrDefault(c => c.Id == id);
                return author;
            }
        }

        public Task<Author> AddAsync(string name)
        {
            var author = new Author(){Id= Interlocked.Increment(ref _id), Name = name, Books = new List<Book>()};
            lock (_authors)
            {
                _authors.Add(author);
            }
            return Task.FromResult(author);
        }

        public Task<Author?> RemoveAsync(int id)
        {
            Author? author = null;
            for (int i = 0; i < _authors.Count; i++)
            {
                if (_authors[i].Id == id)
                {
                    author = _authors[i];
                    lock (_authors)
                    {
                        _authors.RemoveAt(i);
                    }

                    break;
                }
            }
            return Task.FromResult(author);
        }

        public Task<Author?> GetAuthorAsync(int id)
        {
            Author? author = null;
            lock (_authors)
            {
                for (int i = 0; i < _authors.Count; i++)
                {
                    if (_authors[i].Id == id)
                    {
                       author = _authors[i];
                       break;
                    }
                }
            }
            return Task.FromResult(author);
        }
    }
}
