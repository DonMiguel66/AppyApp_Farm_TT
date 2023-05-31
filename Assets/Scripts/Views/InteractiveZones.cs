using Interfaces;
using UnityEngine;

namespace Views
{
    public abstract class InteractiveZones: InteractiveObject, IInteractable
    {
        private PlayerView _contactPlayerTransform;

        protected PlayerView ContactPlayerView => _contactPlayerTransform;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            _contactPlayerTransform = other.transform.GetComponent<PlayerView>();
            EnterInteraction();
            //IsInteractable = false;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            _contactPlayerTransform = other.transform.GetComponent<PlayerView>();
            StayInteraction();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            _contactPlayerTransform = other.transform.GetComponent<PlayerView>();
            ExitInteraction();
        }

        protected abstract void StayInteraction();
        protected abstract void ExitInteraction();

    }
}