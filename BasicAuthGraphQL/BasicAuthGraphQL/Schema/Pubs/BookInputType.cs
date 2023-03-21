using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookInputType : InputObjectGraphType
    {
        public BookInputType()
        {
            Name = "BookInput";
            Field<StringGraphType>("Id");
            Field<NonNullGraphType<StringGraphType>>("Name");
            Field<IntGraphType>("AuthorId");
        }
    }
}
