using GraphQL.Instrumentation;
using GraphQL.Types;
using System;
using System.Reactive;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsSchema : GraphQL.Types.Schema
    {
        public PubsSchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            //Query = (PubsQuery)serviceProvider.GetService(typeof(PubsQuery)) ?? throw new InvalidOperationException();
            //Mutation = (PubsMutation)serviceProvider.GetService(typeof(PubsMutation)) ?? throw new InvalidOperationException();
            Query = serviceProvider.GetService<PubsQuery>()!;
            Mutation = serviceProvider.GetService<PubsMutation>()!;
            Subscription = serviceProvider.GetService<PubsSubscription>();
            FieldMiddleware.Use(new InstrumentFieldsMiddleware());
        }

    }
}
