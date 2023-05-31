using System;
using System.Collections.Generic;
using Controllers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    public class AviaryView : InteractiveZones
    {
        [SerializeField] private Transform _aviary;
        [SerializeField] private Transform _pulsatingArea;
        [SerializeField] private List<NavMeshAgent> _navMeshAgents;
        [SerializeField] private Transform _buildPriceLogo;
        [SerializeField] private Transform _buildPriceParent;
        [SerializeField] private TMP_Text _buildPriceText;
        
        [SerializeField] private Transform _carrotCountLogo;
        [SerializeField] private Transform _carrotCountParent;
        [SerializeField] private TMP_Text _carrotCountText;
        public Transform BuildPriceLogo => _buildPriceLogo;

        private int _buildCost;
        private int _neededPlantNumber;
        private int _currentPlantNumber;
        private int _requiredPlantNumber;
        public int BuildCost => _buildCost;

        private Vector3 _originalScale;
        private float _scaleSpeed = 1f;
        
        public Transform Aviary => _aviary;

        public List<NavMeshAgent> NavMeshAgents => _navMeshAgents;

        public Transform PulsatingArea => _pulsatingArea;

        public Transform CarrotCountLogo => _carrotCountLogo;

        public AviaryState AviaryState
        {
            get => _aviaryState;
            set => _aviaryState = value;
        }

        public TMP_Text CarrotCountText
        {
            get => _carrotCountText;
            set => _carrotCountText = value;
        }

        public event Action<Transform, Transform, int> OnAviaryBuildEnter;
        public event Action<Transform, int> OnAviaryFeedEnter;
        public event Action<bool> OnBuyingStateChange;
        public event Action OnAviaryStay;
        public event Action OnAviaryExit;

        private AviaryState _aviaryState;
        
        public void Init(int buildCost, int neededCarrotNumber)
        {
            _buildCost = buildCost;
            _currentPlantNumber = 0;
            _neededPlantNumber = neededCarrotNumber;
            _requiredPlantNumber = _neededPlantNumber - _currentPlantNumber;
            _buildPriceText.text = BuildCost.ToString();
            _carrotCountText.text = $"{_currentPlantNumber}/{_neededPlantNumber} ";
            Debug.Log("Init");
            _originalScale = PulsatingArea.transform.localScale;
            var sequence = DOTween.Sequence().
                Append(PulsatingArea.transform.DOScale(new Vector3(_originalScale.x + 0.25f, _originalScale.y, _originalScale.z + 0.25f), _scaleSpeed)).
                Append(PulsatingArea.transform.DOScale(_originalScale, _scaleSpeed));
            sequence.SetLoops(-1, LoopType.Restart);
        }

        public void ResetValues()
        {
            _currentPlantNumber = 0;
            _requiredPlantNumber = _neededPlantNumber - _currentPlantNumber;
        }
        
        
        public void SwitchPulsatingAreaColor()
        {
            var go = _pulsatingArea.gameObject;
            go.GetComponent<Renderer>().material.color = Color.green;
            OnAviaryFeedEnter?.Invoke(_carrotCountParent.transform, _requiredPlantNumber);
        }
        
        protected override void EnterInteraction()
        {
            if(_aviaryState == AviaryState.NotBuilt)
            {
                OnBuyingStateChange?.Invoke(true);
                OnAviaryBuildEnter?.Invoke(ContactPlayerView.PlaceToPlants[0].transform, _buildPriceParent.transform, _buildCost);
            }
            if(_aviaryState ==AviaryState.Built)
            {
                OnBuyingStateChange?.Invoke(false);
                OnAviaryFeedEnter?.Invoke(_carrotCountParent.transform, _requiredPlantNumber);
            }
        }

        protected override void StayInteraction()
        {
            /*if(_aviaryState ==AviaryState.Built)
            {
                OnBuyingStateChange?.Invoke(false);
                OnAviaryFeedEnter?.Invoke(_carrotCountParent.transform, _requiredPlantNumber);
            }*/
        }

        protected override void ExitInteraction()
        {
            OnBuyingStateChange?.Invoke(false);
        }
        public void ChangePriceAndUI(int amountOfPaidMoney)
        {
            //Debug.Log($"{BuildCost} |-| {amountOfPaidMoney}");
            _buildCost -= amountOfPaidMoney;
            _buildPriceText.text = BuildCost.ToString();
        }

        public int ChangeCarrotCountAndUI(int amountOfAddedCarrots)
        {
            _currentPlantNumber = amountOfAddedCarrots;
            _requiredPlantNumber = _neededPlantNumber - _currentPlantNumber;
            _carrotCountText.text = $"{_currentPlantNumber}/{_neededPlantNumber} ";
            return _currentPlantNumber;
        }
    }
}