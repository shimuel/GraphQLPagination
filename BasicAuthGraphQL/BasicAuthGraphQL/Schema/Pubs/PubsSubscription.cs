using BasicAuthGraphQL.Domain;
using BasicAuthGraphQL.PubRepo;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class PubsSubscription : ObjectGraphType
    {
        private readonly ISubscriptionService _subscriptionService;
        //public static IObservable<SubscriptionEventData> Notices([FromServices] ISubscriptionService subscriptionService) => subscriptionService.SubscribeAll();
        public PubsSubscription([FromServices] ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;

            AddField(new FieldType
            {
                Name = "subscriptionMessage",
                Type = typeof(SubscriptionEvent),
                StreamResolver = new SourceStreamResolver<SubscriptionEventData>(ResolveStream)
            });
        }

        private IObservable<SubscriptionEventData> ResolveStream(IResolveFieldContext context)
        {
            return _subscriptionService.SubscribeAll();
        }
}

    /// <summary>
    /// 
    /// </summary>
    public interface ISubscriptionService
    {
        IObservable<SubscriptionEventData> SubscribeAll();
        void Notify(SubscriptionEventData notice);
    }

    public class SubscriptionService : ISubscriptionService
    {
        private readonly Subject<SubscriptionEventData> _broadcaster = new();

        public IObservable<SubscriptionEventData> SubscribeAll()
        {
            return _broadcaster.Select(x => { return x; });
        }

        public void Notify(SubscriptionEventData notice)
        {
            _broadcaster.OnNext(notice);
        }
    }
}
