using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using BasicAuthGraphQL.Schema.Pubs;
using BasicAuthGraphQL.Security;
using GraphQL;
using GraphQL.Types;
using System.Security.Cryptography;
using GraphQL.Relay.Utilities;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsQuery : ObjectGraphType<object>
    {
        public PubsQuery([FromServices] AuthorRepo authorRepo, BookRepo bookRepo)
        {
            //this.AuthorizeWithPolicy(Constants.POLICY_READ2);
            Name = "PubsQuery";

            FieldAsync<AuthorType>(
                "authorById",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    var rtnAuth = await authorRepo.GetAuthorAsync(id);
                    
                    return rtnAuth;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

            Connection<AuthorType>()
                .Name("authors")
                .AuthorizeWithPolicy(Constants.POLICY_READ)
                .Resolve(context =>
                {
                    return context.ToConnection(authorRepo.AuthorsAsync.Result);
                });

            Connection<BookType>()
                .Name("books")
                .AuthorizeWithPolicy(Constants.POLICY_READ)
                .Resolve(context =>
                {
                    return context.ToConnection(bookRepo.BooksAsync.Result);
                });

            FieldAsync<BookType>(
                "BookById",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<string>("id");
                    var bookId = $"ISBN{id}";
                    var rtnBook = await bookRepo.GetBookAsync(bookId);
                    return rtnBook;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

        }

    }
}