using BasicAuthGraphQL.PubRepo;
using BasicAuthGraphQL.Security;
using GraphQL.Types;
using GraphQL;
using GraphQL.Relay.Utilities;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorQuery : ObjectGraphType
    {
        public AuthorQuery([FromServices] AuthorRepo authorRepo, BookRepo bookRepo)
        {
            //this.AuthorizeWithPolicy(Constants.POLICY_READ2);
            Name = "authorQueries";

            FieldAsync<AuthorType>(
                "getAuthor",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    var rtnAuth = await authorRepo.GetAuthorAsync(id);

                    return rtnAuth;
                }
            ).AuthorizeWithPolicy(Constants.POLICY_READ);

            Connection<AuthorType>()
                .Name("searchAuthor")
                .AuthorizeWithPolicy(Constants.POLICY_READ)
                .Resolve(context =>
                {
                    //ConnectionUtils.ToConnection(context.Source.Friends, context);
                    return context.ToConnection(authorRepo.AuthorsAsync.Result);
                });

        }

    }
}

