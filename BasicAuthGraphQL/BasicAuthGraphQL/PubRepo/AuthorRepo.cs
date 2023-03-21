﻿using System.Reactive.Linq;
using BasicAuthGraphQL.Domain;

namespace BasicAuthGraphQL.PubRepo
{
    public class AuthorRepo
    {
        private readonly List<Author> _authors = new List<Author>();
        private int _id = 1;

        public AuthorRepo(BookRepo bookRepo)
        {
            Author a = new Author() { Id = Interlocked.Increment(ref _id), Name = "Alex", Books = new List<Book>() };
            a.Books.Add(new Book(){Id = $"ISBN{Interlocked.Increment(ref _id)}", Name = "Hardy Boys", AuthorId = a.Id, Author = a});

            Author b = new Author() { Id = Interlocked.Increment(ref _id), Name = "Alex", Books = new List<Book>() };
            b.Books.Add(new Book() { Id = $"ISBN{Interlocked.Increment(ref _id)}", Name = "Nancy Drew", AuthorId = b.Id, Author = b });

            _authors.Add(a);
            _authors.Add(b);

            a.Books.ForEach(bk => bookRepo.AddBookAsync(bk));
            b.Books.ForEach(bk => bookRepo.AddBookAsync(bk));
        }

        public Task<IEnumerable<Author>> AuthorsAsync
        {
            get
            {
                IEnumerable<Author> authors;
                lock (_authors)
                    authors = _authors.ToList();
                return Task.FromResult(authors);
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

        public Task<Author> AddAsync(string name)
        {
            var author = new Author(){Id= Interlocked.Increment(ref _id), Name = name};
            lock (_authors)
                _authors.Add(author);
            return Task.FromResult(author);
        }

        public Task<Author?> RemoveAsync(int id)
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

        public IObservable<Author> AuthorObservable()
            => _authors.ToObservable();
    }
}
