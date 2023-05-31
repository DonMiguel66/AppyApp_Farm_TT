using Interfaces;
using UnityEngine;

namespace Views
{
    public abstract class InteractiveObject: MonoBehaviour
    {
        private bool _isInteractable;
        public bool IsInteractable
        {
            get => _isInteractable;
            set
            {
                _isInteractable = value;
                GetComponent<Collider>().enabled = _isInteractable;
            }
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!_isInteractable)
            {
                return;
            }
            EnterInteraction();
        }
        
        protected abstract void EnterInteraction();

        protected virtual void Start()
        {
            _isInteractable = true;
        }
    }
}