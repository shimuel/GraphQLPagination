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
            Name = "PubsQuery";
            FieldAsync<ListGraphType<AuthorType>>(
                "Authors",
                resolve: async context =>
                {
                    return await authorRepo.AuthorsAsync;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

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


            FieldAsync<ListGraphType<BookType>>(
                "Books",
                resolve: async context =>
                {
                    return await bookRepo.BooksAsync;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

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