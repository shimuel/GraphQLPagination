using System.Reactive.Linq;
using BasicAuthGraphQL.Domain;

namespace BasicAuthGraphQL.PubRepo
{
    public class AuthorRepo
    {
        private readonly List<Author> _authors = new() {
            new Author() { Id = 1, Name = "Alex" },
            new Author() { Id = 2, Name = "Sam" }
        };

        private int _id = 3;

        public IEnumerable<Author> Authors
        {
            get
            {
                IEnumerable<Author> authors;
                lock (_authors)
                    authors = _authors.ToList();
                return authors;
            }
        }

        public Author? this[int id]
        {
            get
            {
                Author? author = null;
                lock (_authors)
                    author = _authors.FirstOrDefault(c => c.Id == id);
                return author;
            }
        }

        public Author Add(string name)
        {
            var author = new Author(){Id= Interlocked.Increment(ref _id), Name = name};
            lock (_authors)
                _authors.Add(author);
            return author;
        }

        public Author? Remove(int id)
        {
            Author? author = null;
            lock (_authors)
            {
                for (int i = 0; i < _authors.Count; i++)
                {
                    if (_authors[i].Id == id)
                    {
                        author = _authors[i];
                        _authors.RemoveAt(i);
                        break;
                    }
                }
            }
            return author;
        }

        public IObservable<Author> AuthorObservable()
            => _authors.ToObservable();
    }
}
