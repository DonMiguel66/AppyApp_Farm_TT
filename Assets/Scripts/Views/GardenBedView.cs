using System;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class GardenBedView : InteractiveZones
    {
        [SerializeField] private Transform _pulsatingArea;
        [SerializeField] private Transform _gardenGround;
        [SerializeField] private Transform _carrotStem;
        [SerializeField] private Transform _buildPriceLogo;

        public event Action<Transform, Transform> OnGardenBedEnter;
        public event Action<Transform, Transform> OnGardenBedStay;
        public event Action<Transform, Transform> OnGardenBedExit;

        private Vector3 _originalScale;
        private float _scaleSpeed = 1f;

        public void Init()
        {
            _originalScale = _pulsatingArea.transform.localScale;
            var sequence = DOTween.Sequence().
                Append(_pulsatingArea.transform.DOScale(new Vector3(_originalScale.x + 0.25f, _originalScale.y, _originalScale.z + 0.25f), _scaleSpeed)).
                Append(_pulsatingArea.transform.DOScale(_originalScale, _scaleSpeed));
            sequence.SetLoops(-1, LoopType.Restart);
        }
        protected override void EnterInteraction()
        {
            OnGardenBedEnter?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform);
            //_pulsatingArea.transform.gameObject.SetActive(false);
            //_gardenGround.transform.gameObject.SetActive(true);
            //_carrotStem.transform.gameObject.SetActive(true);
        }

        protected override void StayInteraction()
        {
            OnGardenBedStay?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform);
        }

        protected override void ExitInteraction()
        {
            OnGardenBedExit?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform);
        }

        private void Pulsate()
        {

        }
    }
}