using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType(BookRepo repo)
        {
            Field(x => x.Id).Description("The Id of the Author.");
            Field(x => x.Name).Description("The name of the Author.");
            FieldAsync<ListGraphType<BookType>>("books",
                resolve: async context =>
                {
                    var booksByAuthor = await repo.GetBooksByAuthorAsync(context.Source.Id);
                    return booksByAuthor;
                });
        }
    }
}
