using GraphQL.Types;
using System.Xml.Linq;
using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.Security;
using GraphQL;
using BasicAuthGraphQL.PubRepo;
using static System.Net.Mime.MediaTypeNames;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsMutation : ObjectGraphType
    {
        public PubsMutation([FromServices] AuthorRepo authorRepo, BookRepo bookRepo)
        {
            Name = "PubsMutation";
            FieldAsync<AuthorType>(
                    "addAuthor",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<AuthorInputType>> { Name = "author" }
                    ),
                    resolve: async context =>
                    {
                        var author = context.GetArgument<Author>("author");
                        var x = author.Name;
                        var y = author.Id;
                        var newAuthor =  await authorRepo.AddAsync(author.Name);
                        //foreach (var bk in author.Books)
                        //{
                        //   var newBook = bookRepo.Add(bk.Name);
                        //    newAuthor.Books.Add(newBook);
                        //    newBook.Author = newAuthor;
                        //}
                        return newAuthor;
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);
            ;
            FieldAsync<BookType>(
                    "addBook",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<BookInputType>> { Name = "book" }
                    ),
                    resolve: async context =>
                    {
                        var book = context.GetArgument<Book>("book");
                        return bookRepo.AddAsync(book.Name);
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);
            ;
        }
    }
}
