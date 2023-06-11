namespace BasicAuthGraphQL.Schema.Pubs
{


    using GraphQL.Types;

    //https://stackoverflow.com/questions/64729712/how-does-one-organize-more-than-a-few-mutations-in-graphql-net-graphtype-first
    public class PubsMutations : ObjectGraphType
    {
        public PubsMutations()
        {
            Name = "pubsMutations";
            Field<AuthorMutation>(
                "authorMutate",
                resolve: context => new { });
            Field<BookMutation>(
                "bookMutate",
                resolve: context => new { });
        }
    }
}
