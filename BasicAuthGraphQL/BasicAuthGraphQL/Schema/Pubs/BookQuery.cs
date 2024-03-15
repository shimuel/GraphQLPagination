using BasicAuthGraphQL.PubRepo;
using BasicAuthGraphQL.Security;
using GraphQL.Types;
using GraphQL;
using GraphQL.Relay.Utilities;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookQuery : ObjectGraphType
    {
        public BookQuery([FromServices] AuthorRepo authorRepo, BookRepo bookRepo)
        {
            //this.AuthorizeWithPolicy(Constants.POLICY_READ2);
            Name = "bookQueries";
            FieldAsync<BookType>(
                "BookById",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<string>("id");
                    var rtnBook = await bookRepo.GetBookAsync(id);
                    return rtnBook;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

            Connection<BookType>()
                .Name("booksSearch")
                .AuthorizeWithPolicy(Constants.POLICY_READ)
                .Bidirectional()
                .Resolve(context =>
                {
                    return context.ToConnection(bookRepo.BooksAsync.Result);
                });

        }

    }
}

