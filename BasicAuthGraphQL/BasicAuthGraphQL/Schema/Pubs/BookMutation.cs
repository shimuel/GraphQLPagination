using GraphQL.Types;
using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.Security;
using GraphQL;
using BasicAuthGraphQL.PubRepo;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class BookMutation : ObjectGraphType
    {
        public BookMutation(
            [FromServices] AuthorRepo authorRepo, 
            BookRepo bookRepo,
            ISubscriptionService subscriptionService,
            ILogger<BookMutation> logger)
        {
            Name = "BookMutation";
            FieldAsync<BookType>(
                    "addBook",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<BookInputType>> { Name = "book" }
                    ),
                    resolve: async context =>
                    {
                        var book = context.GetArgument<Book>("book");
                        var author = await authorRepo.GetAuthorAsync(book.AuthorId);
                        if (author != null)
                        {
                            var newBook = await bookRepo.AddBookAsync(book.Name, book.Genre, book.Published, author);
                            //subscriptionService.Notify(new SubscriptionEventData() { Id = author.Id.ToString(), MessageType = MessageType.BookAdded, Data = author.Name, At = DateTime.Now });
                            subscriptionService.Notify(new SubscriptionEventData()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MessageType = MessageType.AuthorAdded,
                                Data = JsonSerializer.Serialize<Author>(author, new JsonSerializerOptions()
                                {
                                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                                    MaxDepth = 5,
                                    WriteIndented = true
                                }),
                                At = DateTime.Now
                            });
                            return newBook;
                        }

                        throw new ArgumentException("Provide a valid AuthorId");
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);


            FieldAsync<BookType>(
                    "updateBook",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<BookInputType>> { Name = "book" }
                    ),
                    resolve: async context =>
                    {
                        var authorArg = context.GetArgument<Book>("book");
                      return new Book() { Id = "000", Name = "Updated" };
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);
        }
    }
}
