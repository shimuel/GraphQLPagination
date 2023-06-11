using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using GraphQL;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookType : NodeGraphType<Book>
    {
        private AuthorRepo _authorRepo;
        private BookRepo _bookRepo;

        public BookType(AuthorRepo authorRepo, BookRepo bookRepo)
        {
            Name = "book";
            Id(bk => bk.Id);
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
            Field(x => x.Name).Description("The name of the Book.");
            FieldAsync<AuthorType>("bookAuthor",
                resolve: async context =>
                {
                    var author = await _authorRepo.GetAuthorAsync(context.Source.AuthorId);
                    return author;
                });
        }
        public override Book GetById(IResolveFieldContext<object> context, string id)
        {
            return _bookRepo.GetBookAsync(id).Result;
        }
    }
}
