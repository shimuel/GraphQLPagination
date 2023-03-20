using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookInputType : InputObjectGraphType
    {
        public BookInputType()
        {
            Name = "BookInput";
            Field<NonNullGraphType<StringGraphType>>("Id");
            Field<NonNullGraphType<StringGraphType>>("Name");
        }
    }
}
