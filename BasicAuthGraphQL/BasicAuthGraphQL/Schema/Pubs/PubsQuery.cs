using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using BasicAuthGraphQL.Schema.Pubs;
using BasicAuthGraphQL.Security;
using GraphQL;
using GraphQL.Types;
using System.Security.Cryptography;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsQuery : ObjectGraphType<object>
    {
        public PubsQuery([FromServices] AuthorRepo authorRepo, BookRepo bookRepo)
        {
            //this.AuthorizeWithPolicy(Constants.POLICY_READ2);

            Field<ListGraphType<AuthorType>>(
                "Authors",
                resolve: context =>
                {
                    return authorRepo.Authors;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

            Field<ListGraphType<BookType>>(
                "Books",
                resolve: context =>
                {
                    return bookRepo.Books;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

            Field<AuthorType>(
                "Author",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var rtnAuth = authorRepo[id];
                    return rtnAuth;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

            Field<BookType>(
                "Book",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("id");
                    var bookId = $"ISBN{id}";
                    var rtnBook = bookRepo[bookId];
                    return rtnBook;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

        }

    }
}