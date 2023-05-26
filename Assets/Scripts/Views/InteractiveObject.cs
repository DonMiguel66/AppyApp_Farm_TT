using System;
using Interfaces;
using UnityEngine;

namespace Views
{
    public abstract class InteractiveObject: MonoBehaviour, IExecute
    {
        private bool _isInteractable;
        protected bool IsInteractable
        {
            get { return _isInteractable; }
            set
            {
                _isInteractable = value;
                GetComponent<Renderer>().enabled = _isInteractable;
                GetComponent<Collider>().enabled = _isInteractable;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            //if (!IsInteractable || !other.CompareTag("Player"))
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            EnterInteraction();
            //IsInteractable = false;
        }
        protected abstract void EnterInteraction();
        public abstract void Execute();

        private void Start()
        {
            IsInteractable = true;
        }
    }
}