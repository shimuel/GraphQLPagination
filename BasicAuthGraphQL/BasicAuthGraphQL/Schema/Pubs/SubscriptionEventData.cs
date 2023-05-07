﻿using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GraphQL;
using GraphQL.Types;

namespace BasicAuthGraphQL.Schema.Pubs
{
    public class SubscriptionEventData
    {
        [Id]
        public string Id { get; set; }

        [Name("MessageType")]
        public string Data { get; set; } = null!;
        public MessageType MessageType { get; set; }
        public DateTime At { get; set; }
    }

    public class SubscriptionEvent : ObjectGraphType<SubscriptionEventData>
    {
        public SubscriptionEvent()
        {
            Field(o => o.Id);
            Field(o => o.MessageType);
            Field(o => o.Data);
            Field(o => o.At);
           
        }
    }

    public enum MessageType
    {
        BookAdded,
        BookRemoved,
        AuthorAdded,
        AuthorRemoved
    }

    //public class NotificationInput
    //{
    //    public string Id { get; set; }
    //    public string Data { get; set; } = null!;
    //    public string MessageType { get; set; }
    //    public DateTime At { get; set; }
    //}

    //public class NotificationEventType : ObjectGraphType<MessageType>
    //{
    //    public NotificationEventType()
    //    {
    //        Field(o => o.Type);
    //        Field(o => o.SubscriptionEventData, false, typeof(NotificationEventType)).Resolve(ResolveNotification);
    //    }

    //    private SubscriptionEventData ResolveNotification(IResolveFieldContext<MessageType> context)
    //    {
    //        var eventType = context.Source;
    //        return eventType.SubscriptionEventData!;
    //    }
    //}

    //public class MessageType
    //{
    //    public MessageType Type { get; set; }
    //    public SubscriptionEventData? SubscriptionEventData { get; set; }
    //}

    //public enum MessageType
    //{
    //    BookAdded,
    //    BookRemoved,
    //    AuthorAdded,
    //    AuthorRemoved
    //}


}