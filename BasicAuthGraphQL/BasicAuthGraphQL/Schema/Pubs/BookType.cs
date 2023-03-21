using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType(AuthorRepo repo)
        {

            Field(x => x.Id).Description("The Id of the Book.");
            Field(x => x.Name).Description("The name of the Book.");
            FieldAsync<AuthorType>("author",
                resolve: async context =>
                {
                    var author = await repo.GetAuthorAsync(context.Source.AuthorId);
                    return author;
                });
        }
    }
}
