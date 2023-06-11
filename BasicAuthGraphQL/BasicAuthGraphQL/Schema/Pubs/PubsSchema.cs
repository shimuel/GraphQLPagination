using GraphQL.Instrumentation;
using GraphQL.Types;
using System;
using System.Reactive;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsSchema : GraphQL.Types.Schema
    {
        //https://stackoverflow.com/questions/64729712/how-does-one-organize-more-than-a-few-mutations-in-graphql-net-graphtype-first
        public PubsSchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            //Query = (PubsQueries)serviceProvider.GetService(typeof(PubsQueries)) ?? throw new InvalidOperationException();
            //Mutation = (PubsMutations)serviceProvider.GetService(typeof(PubsMutations)) ?? throw new InvalidOperationException();
            Query = serviceProvider.GetService<PubsQueries>()!;
            Mutation = serviceProvider.GetService<PubsMutations>()!;
            Subscription = serviceProvider.GetService<PubsSubscription>();
            FieldMiddleware.Use(new InstrumentFieldsMiddleware());
        }

    }
}
