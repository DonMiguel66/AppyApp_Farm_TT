using System;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Views
{
    public abstract class InteractiveZones: InteractiveObject, IInteractable
    {
        //private bool _isInteractable;
        private PlayerView _contactPlayerTransform;
        /*protected bool IsInteractable
        {
            get { return _isInteractable; }
            set
            {
                _isInteractable = value;
                GetComponent<Collider>().enabled = _isInteractable;
            }
        }*/

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

        //protected abstract void EnterInteraction();
        protected abstract void StayInteraction();
        protected abstract void ExitInteraction();

        /*
        private void Start()
        {
            IsInteractable = true;
        }*/
    }
}