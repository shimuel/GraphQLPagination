using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorInputType : InputObjectGraphType
    {
        public AuthorInputType()
        {
            Name = "AuthorInput";
            Field<NonNullGraphType<IntGraphType>>("Id");
            Field<NonNullGraphType<StringGraphType>>("Name");
            Field<ListGraphType<BookInputType>>("Books");
            //Field(a => a.Books, type: typeof(ListGraphType<BookInputType>), nullable: true);
        }
    }
}
