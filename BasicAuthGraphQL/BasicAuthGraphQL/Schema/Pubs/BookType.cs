using BasicAuthGraphQL.Domain;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(x => x.Id).Description("The Id of the Book.");
            Field(x => x.Name).Description("The name of the Book.");
            //Field(x => x.Author).Description("Author");
        }
    }
}
