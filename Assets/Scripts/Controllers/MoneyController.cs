using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Tools;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class MoneyController : BaseController, IExecute
    {
        private List<MoneyView> _currentAvailableMoneyViews;
        public event Action OnMoneyCountChange;
        private MoneyHolderView _moneyHolder;

        private int _playerAmountOfMoney;
        private readonly ResourcePath _viewPath = new ResourcePath {PathResource = "Prefabs/Money"};

        public MoneyController(PlayerConfig playerConfig, List<MoneyView> moneyViews)
        {
            _currentAvailableMoneyViews = moneyViews;
            foreach (var moneyView in _currentAvailableMoneyViews)
            {
                moneyView.Init(playerConfig.parOfMoneyInView,playerConfig.moneyFlySpeed);
                moneyView.OnMoneyClaim += ChangeMoneyCount;
            }
            _moneyHolder = Object.FindObjectOfType<MoneyHolderView>();
            _moneyHolder.Init(playerConfig.moneyPickupRadius, _currentAvailableMoneyViews);
        }

        private void AddMoneyView(Transform placeToInstantiate)
        {
            var moneyView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeToInstantiate, false).GetComponent<MoneyView>();
            _currentAvailableMoneyViews.Add(moneyView);
            moneyView.OnMoneyClaim += ChangeMoneyCount;
        }
        
        private void ChangeMoneyCount(int amountOfHandpickedMoney, MoneyView moneyView)
        {
            Debug.Log(amountOfHandpickedMoney);
            _playerAmountOfMoney += amountOfHandpickedMoney;
            _currentAvailableMoneyViews.Remove(moneyView);
            Debug.Log(_playerAmountOfMoney);
            OnMoneyCountChange?.Invoke();
        }
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDispose()
        {
            foreach (var moneyView in _currentAvailableMoneyViews)
            {
                moneyView.OnMoneyClaim -= ChangeMoneyCount;
            }
            base.OnDispose();
        }
    }
}