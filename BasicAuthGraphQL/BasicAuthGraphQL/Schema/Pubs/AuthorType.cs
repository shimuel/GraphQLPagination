using BasicAuthGraphQL.Domain;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Field(x => x.Id).Description("The Id of the Author.");
            Field(x => x.Name).Description("The name of the Author.");
            //Field(x => x.Books).Description("Books");
        }
    }
}
