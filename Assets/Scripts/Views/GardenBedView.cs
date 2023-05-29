using System;
using System.Threading;
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
        public event Action<Transform, Transform, int> OnGardenBedStay;
        public event Action<Transform, Transform, int,CancellationToken> OnGardenBedStayAsync;
        public event Action<Transform, Transform> OnGardenBedExit;

        private Vector3 _originalScale;
        private float _scaleSpeed = 1f;

        private int _buildCost;
        private int _currentCountClaimedOfMoney;

        private CancellationTokenSource ctr = new CancellationTokenSource();
        public void Init(int buildCost)
        {
            _buildCost = buildCost;
            Debug.Log("Init");
            _originalScale = _pulsatingArea.transform.localScale;
            var sequence = DOTween.Sequence().
                Append(_pulsatingArea.transform.DOScale(new Vector3(_originalScale.x + 0.25f, _originalScale.y, _originalScale.z + 0.25f), _scaleSpeed)).
                Append(_pulsatingArea.transform.DOScale(_originalScale, _scaleSpeed));
            sequence.SetLoops(-1, LoopType.Restart);
        }
        protected override void EnterInteraction()
        {
            OnGardenBedEnter?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform);
            //OnGardenBedStayAsync?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform, ctr.Token);
            //_pulsatingArea.transform.gameObject.SetActive(false);
            //_gardenGround.transform.gameObject.SetActive(true);
            //_carrotStem.transform.gameObject.SetActive(true);
        }
        protected override void StayInteraction()
        {
            OnGardenBedStayAsync?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform, _buildCost, ctr.Token);
        }

        protected override void ExitInteraction()
        {
            ctr.Cancel();
            OnGardenBedExit?.Invoke(ContactPlayerView.transform,_buildPriceLogo.transform);
        }

        private void Pulsate()
        {

        }
    }
}