using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using BasicAuthGraphQL.Schema.Pubs;
using BasicAuthGraphQL.Security;
using GraphQL;
using GraphQL.Types;
using System.Security.Cryptography;
using GraphQL.Relay.Types;
using GraphQL.Relay.Utilities;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsQueries : ObjectGraphType
    {
        public PubsQueries([FromServices] AuthorRepo authorRepo, BookRepo bookRepo)
        {
            //this.AuthorizeWithPolicy(Constants.POLICY_READ2);
            Name = "pubsQueries";

            FieldAsync<AuthorQuery>(
                "authorQuery",
                resolve: async context => new { });
            FieldAsync<BookQuery>(
                "bookQuery",
                resolve: async context => new { });
        }

    }
}