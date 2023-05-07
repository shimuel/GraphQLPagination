using GraphQL.Types;
using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.Security;
using GraphQL;
using BasicAuthGraphQL.PubRepo;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsMutation : ObjectGraphType
    {
        public PubsMutation(
            [FromServices] AuthorRepo authorRepo, 
            BookRepo bookRepo,
            ISubscriptionService subscriptionService,
            ILogger<PubsMutation> logger)
        {
            Name = "PubsMutation";
            FieldAsync<AuthorType>(
                    "addAuthor",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<AuthorInputType>> { Name = "author" }
                    ),
                    resolve: async context =>
                    {
                        var authorArg = context.GetArgument<Author>("author");
                        var newAuthor = await authorRepo.AddAsync(authorArg.Name);

                        foreach (var bk in authorArg.Books)
                        {
                            var newBook = await bookRepo.AddBookAsync(bk.Name, newAuthor);
                            newAuthor.Books.Add(newBook);
                            //subscriptionService.Notify(new SubscriptionEventData(){Id = bk.Id, MessageType = MessageType.AuthorAdded, Data = bk.Name ,At = DateTime.Now});
                            subscriptionService.Notify(new SubscriptionEventData()
                            {
                                Id = Guid.NewGuid().ToString(), MessageType = MessageType.BookAdded, Data = JsonSerializer.Serialize<Book>(bk,new JsonSerializerOptions()
                                {
                                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                                    MaxDepth = 5,
                                    WriteIndented = true
                                }),
                                At = DateTime.Now
                            });
                        }

                        return newAuthor;
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);

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
                            var newBook = await bookRepo.AddBookAsync(book.Name, author);
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
        }
    }
}
