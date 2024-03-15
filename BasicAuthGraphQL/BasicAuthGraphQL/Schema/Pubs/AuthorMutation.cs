using GraphQL.Types;
using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.Security;
using GraphQL;
using BasicAuthGraphQL.PubRepo;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class AuthorMutation : ObjectGraphType
    {
        public AuthorMutation(
            [FromServices] AuthorRepo authorRepo,
            BookRepo bookRepo,
            ISubscriptionService subscriptionService,
            IGraphQLTextSerializer serializer,
            ILogger<AuthorMutation> logger)
        {
            Name = "AuthorMutations";
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
                            var newBook = await bookRepo.AddBookAsync(bk.Name, bk.Genre, bk.Published, newAuthor);
                            newAuthor.Books.Add(newBook);
                            //subscriptionService.Notify(new SubscriptionEventData(){Id = bk.Id, MessageType = MessageType.AuthorAdded, Data = bk.Name ,At = DateTime.Now});
                        }
                        subscriptionService.Notify(new SubscriptionEventData()
                        {
                            Id = Guid.NewGuid().ToString(),
                            MessageType = MessageType.AuthorAdded,
                            Data = JsonSerializer.Serialize<Author>(newAuthor, new JsonSerializerOptions()
                            {
                                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                                MaxDepth = 5,
                                WriteIndented = true
                            }),
                            At = DateTime.Now
                        });
                        return newAuthor;
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);

            FieldAsync<AuthorType>(
                    "updateAuthor",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<AuthorInputType>> { Name = "author" }
                    ),
                    resolve: async context =>
                    {
                        return new Author(){Id = 000, Name = "Updated"};
                    })
                .AuthorizeWithPolicy(Constants.POLICY_UPDATE);
        }
    }
}
