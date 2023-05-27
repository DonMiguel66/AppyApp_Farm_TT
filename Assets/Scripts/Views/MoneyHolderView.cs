using System;

namespace Views
{
    public class MoneyHolderView : InteractiveZones
    {

        public event Action<PlayerView> OnMoneyPickup;
        
        protected override void EnterInteraction()
        {
            OnMoneyPickup?.Invoke(ContactPlayerView);
        }

        protected override void StayInteraction()
        {
            OnMoneyPickup?.Invoke(ContactPlayerView);
        }

        protected override void ExitInteraction()
        {
            OnMoneyPickup?.Invoke(ContactPlayerView);
        }
    }
}