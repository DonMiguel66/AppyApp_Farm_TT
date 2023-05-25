using System;

namespace Interfaces
{
    public interface IReadonlySubscriptionProperty<T>
    {
        T Value { get; }
        void SubscribeOnChange(Action<T> subscriptionAction);
        void UnSubscriptionOnChange(Action<T> unsubscriptionAction);
    }
}