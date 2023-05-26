using UnityEngine;

namespace Views
{
    public abstract class InteractiveZones: MonoBehaviour
    {
        private bool _isInteractable;
        private Transform _playerTransform;
        protected bool IsInteractable
        {
            get { return _isInteractable; }
            set
            {
                _isInteractable = value;
                GetComponent<Collider>().enabled = _isInteractable;
            }
        }

        public Transform PlayerTransform => _playerTransform;

        private void OnTriggerEnter(Collider other)
        {
            //if (!IsInteractable || !other.CompareTag("Player"))
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            _playerTransform = other.transform;
            EnterInteraction();
            //IsInteractable = false;
        }
        
        private void OnTriggerStay(Collider other)
        {
            //if (!IsInteractable || !other.CompareTag("Player"))
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            _playerTransform = other.transform;
            StayInteraction();
        }
        protected abstract void EnterInteraction();
        protected abstract void StayInteraction();

        private void Start()
        {
            IsInteractable = true;
        }
    }
}