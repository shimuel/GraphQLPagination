using GraphQL.Types;
using System.Xml.Linq;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorInputType : InputObjectGraphType
    {
        public AuthorInputType()
        {
            Name = "AuthorInput";
            Field<NonNullGraphType<IntGraphType>>("Id");
            Field<NonNullGraphType<StringGraphType>>("Name");
            //Field<InputObjectGraphType<ListGraphType<BookInputType>>("AuthorBooks");
        }
    }
}
