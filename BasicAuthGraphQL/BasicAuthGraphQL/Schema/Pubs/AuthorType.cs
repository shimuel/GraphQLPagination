using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using GraphQL;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorType : NodeGraphType<Author>
    {
        private AuthorRepo _authorRepo;
        private BookRepo _bookRepo;
        public AuthorType(AuthorRepo authorRepo, BookRepo bookRepo)
        {
            Name = "author";
            Id(bk => bk.Id);
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;

            Field(x => x.Name).Description("The name of the Author.");
            FieldAsync<ListGraphType<BookType>>("books",
                resolve: async context =>
                {
                    var booksByAuthor = await bookRepo.GetBooksByAuthorAsync(context.Source.Id);
                    return booksByAuthor;
                });
        }

        public override Author GetById(IResolveFieldContext<object> context, string id)
        {
            return _authorRepo.GetAuthorAsync(int.Parse(id)).Result;
        }
    }
}
