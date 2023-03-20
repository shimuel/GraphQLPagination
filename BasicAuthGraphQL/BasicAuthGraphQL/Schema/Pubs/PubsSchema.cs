using GraphQL.Instrumentation;

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
            //Subscription = serviceProvider.GetService<PubsSubscription>()!;
            Description = "PubsSchema";
            FieldMiddleware.Use(new InstrumentFieldsMiddleware());
        }

    }
}
