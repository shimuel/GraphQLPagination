using GraphQL.Types;
using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.Security;
using GraphQL;
using BasicAuthGraphQL.PubRepo;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsMutation : ObjectGraphType
    {
        public PubsMutation([FromServices] AuthorRepo authorRepo, BookRepo bookRepo, ILogger<PubsMutation> logger)
        {
            Name = "PubsMutation";
            FieldAsync<AuthorType>(
                    "addAuthor",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<AuthorInputType>> { Name = "author" }
                    ),
                    resolve: async context =>
                    {
                        var authorArg = context.GetArgument<Author>("author");
                        var newAuthor = await authorRepo.AddAsync(authorArg.Name);

                        foreach (var bk in authorArg.Books)
                        {
                            var newBook = await bookRepo.AddBookAsync(bk.Name, newAuthor);
                            newAuthor.Books.Add(newBook);
                        }

                        return newAuthor;
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);

            FieldAsync<BookType>(
                    "addBook",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<BookInputType>> { Name = "book" }
                    ),
                    resolve: async context =>
                    {
                        var book = context.GetArgument<Book>("book");
                        var author = await authorRepo.GetAuthorAsync(book.AuthorId);
                        if (author != null)
                        {
                            var newBook = await bookRepo.AddBookAsync(book.Name, author);
                            return newBook;
                        }

                        throw new ArgumentException("Provide a valid AuthorId");
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);
        }
    }
}
