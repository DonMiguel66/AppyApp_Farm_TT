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
        private float _moneyPickupRadius;
        
        private readonly ResourcePath _viewPath = new ResourcePath {PathResource = "Prefabs/Money"};

        public MoneyController(PlayerConfig playerConfig, List<MoneyView> moneyViews)
        {
            _moneyPickupRadius = playerConfig.moneyPickupRadius;
            _currentAvailableMoneyViews = moneyViews;
            foreach (var moneyView in _currentAvailableMoneyViews)
            {
                moneyView.Init(playerConfig.parOfMoneyInView,playerConfig.moneyFlySpeed);
                moneyView.OnMoneyClaim += ChangeMoneyCount;
            }
            _moneyHolder = Object.FindObjectOfType<MoneyHolderView>();
            _moneyHolder.OnMoneyPickup += CheckAvailableMoneyToPickUp;
        }

        private MoneyView AddMoneyView(Transform posToSpawn)
        {
            var moneyView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), posToSpawn, false).GetComponent<MoneyView>();
            _currentAvailableMoneyViews.Add(moneyView);
            moneyView.OnMoneyClaim += ChangeMoneyCount;
            return moneyView;
        }

        public void SpendMoney(Transform posToSpawn, Transform posToMove)
        {
            if(_currentAvailableMoneyViews.Count<=0) return;
            var moneyView = AddMoneyView(posToSpawn);
            moneyView.MoveTo(posToMove);
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
        }

        private void CheckAvailableMoneyToPickUp(PlayerView contactPlayerView)
        {
            foreach (var moneyView in _currentAvailableMoneyViews)
            {
                if ((contactPlayerView.gameObject.transform.position - moneyView.transform.position).magnitude < _moneyPickupRadius)
                {
                    //Debug.Log(contactPlayerView.name);
                    //moneyView.MoveTo(contactPlayerView.transform);
                    moneyView.MoveToAsync(contactPlayerView);
                }
            }
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