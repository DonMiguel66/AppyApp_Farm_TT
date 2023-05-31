using System;
using Controllers;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Views
{
    public class GardenBedView : InteractiveZones
    {
        [SerializeField] private Transform _pulsatingArea;
        [SerializeField] private Transform _gardenGround;
        [SerializeField] private Transform _carrotStem;
        [SerializeField] private Transform _buildPriceLogo;
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private CarrotView _carrotView;

        public event Action<Transform, Transform, int> OnGardenBedEnter;
        public event Action<bool> OnBuyingStateChange;
        public event Action<Transform, Transform, int> OnGardenBedStay;
        public event Action<Transform, Transform> OnGardenBedExit;
        public event Action<GardenBedView> OnPlantGrowBegan;
        public event Action<GardenBedView, CarrotView, PlayerView> OnPlantGrown;

        private Vector3 _originalScale;
        private float _scaleSpeed = 1f;

        private int _buildCost;
        private int _parOfMoney;

        public Transform CarrotStem => _carrotStem;

        public Transform GardenGround => _gardenGround;

        public Transform BuildPriceLogo => _buildPriceLogo;

        public Transform PulsatingArea => _pulsatingArea;

        public int BuildCost => _buildCost;

        public GardenBedState BedState
        {
            get => _gardenBedState;
            set => _gardenBedState = value;
        }

        public CarrotView CarrotView
        {
            get => _carrotView;
            set => _carrotView = value;
        }

        private GardenBedState _gardenBedState;

        public void Init(int buildCost, int parOfMoney)
        {
            _parOfMoney = parOfMoney;
            _gardenBedState = GardenBedState.NotBuilt;
            _buildCost = buildCost;
            _tmpText.text = BuildCost.ToString();
            _originalScale = PulsatingArea.transform.localScale;
            var sequence = DOTween.Sequence().
                Append(PulsatingArea.transform.DOScale(new Vector3(_originalScale.x + 0.25f, _originalScale.y, _originalScale.z + 0.25f), _scaleSpeed)).
                Append(PulsatingArea.transform.DOScale(_originalScale, _scaleSpeed));
            sequence.SetLoops(-1, LoopType.Restart);
        }
        protected override void EnterInteraction()
        {
            Debug.Log($"Enter {_gardenBedState}");
            if(_gardenBedState == GardenBedState.NotBuilt&& !_carrotView)
            {
                OnBuyingStateChange?.Invoke(true);
                OnGardenBedEnter?.Invoke(ContactPlayerView.PlaceToPlants[0].transform, BuildPriceLogo.transform, _buildCost);
            }
            else if(_gardenBedState == GardenBedState.InGrowthProcess&& !_carrotView)
            {
                OnBuyingStateChange?.Invoke(false);
            }
            else if(_gardenBedState == GardenBedState.Grown&& _carrotView)
            {
                OnBuyingStateChange?.Invoke(false);
                OnPlantGrown?.Invoke(this,_carrotView, ContactPlayerView);
            }
        }
        protected override void StayInteraction()
        {
            if(_gardenBedState == GardenBedState.Grown && _carrotView)
            {
                OnBuyingStateChange?.Invoke(false);
                OnPlantGrown?.Invoke(this,_carrotView, ContactPlayerView);
            }
        }
 
        protected override void ExitInteraction()
        {
            OnBuyingStateChange?.Invoke(false);
        }

        private void ChangeCost(int amountOfPaidMoney)
        {
            _buildCost -= amountOfPaidMoney;
        }
        public void ChangeUI()
        {
            //Debug.Log($"{BuildCost} |-| {amountOfPaidMoney}");
            _tmpText.text = BuildCost.ToString();
        }

        public void ChangeCostAndUI(int amountOfPaidMoney)
        {
            _buildCost -= amountOfPaidMoney;
            _tmpText.text = BuildCost.ToString();
        }
        
    }
}