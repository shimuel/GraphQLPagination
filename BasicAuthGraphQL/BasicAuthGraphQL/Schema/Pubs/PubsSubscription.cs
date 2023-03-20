using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using GraphQL;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using System.Reactive.Linq;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsSubscription : ObjectGraphType
    {
        public static IObservable<Author> Books([FromServices] AuthorRepo repo) => repo.AuthorObservable();

        public static IObservable<Book> Books([FromServices] BookRepo repo) => repo.BooksObservable();

    }
}
