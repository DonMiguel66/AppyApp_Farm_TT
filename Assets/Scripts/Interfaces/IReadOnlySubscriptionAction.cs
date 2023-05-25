using System;

namespace Interfaces
{
    public interface IReadOnlySubscriptionAction
    {
        void SubscribeOnChange(Action subscriptionAction);
        void UnSubscriptionOnChange(Action unsubscriptionAction);
    }
}