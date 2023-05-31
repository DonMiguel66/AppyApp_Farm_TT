using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using Tools;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class MoneyController : BaseController, IExecute
    {
        private List<MoneyView> _currentAvailableMoneyViews = new List<MoneyView>();
        private ObjectPool _objectPool;
        private readonly ListExecuteObject _listExecuteObject;
        //public event Action OnMoneyCountChange;
        private MoneyHolderView _moneyHolder;
        private PlayerConfig _playerConfig;
        
        private int _playerAmountOfMoney;
        private float _moneyPickupRadius;
        private bool _isBuying = true;
        private int _counter = 0;
        private readonly ResourcePath _viewPath = new ResourcePath {PathResource = "Prefabs/MoneyView"};

        public event Action<int> OnPlayerMoneyCountChange;
        
        public MoneyController(PlayerConfig playerConfig, ListExecuteObject listExecuteObject)
        {
            _listExecuteObject = listExecuteObject;
            _objectPool = new ObjectPool(_viewPath);
            _playerConfig = playerConfig;
            AddViewsFromObjectList();
            _moneyPickupRadius = playerConfig.moneyPickupRadius;
            _moneyHolder = Object.FindObjectOfType<MoneyHolderView>();
            _moneyHolder.OnMoneyPickup += CheckAvailableMoneyToPickUp;
        }

        private void AddViewsFromObjectList()
        {
            foreach (var o in _listExecuteObject)
            {
                if (o is MoneyView moneyView)
                {
                    _currentAvailableMoneyViews.Add(moneyView);
                    moneyView.Init(_playerConfig.parOfMoneyInView,_playerConfig.moneyMoveSpeed, _playerConfig.moneyScaleChangeSpeed);
                    _objectPool.InitialPush(moneyView.gameObject);
                    moneyView.OnMoneyClaim += ChangeMoneyCount;
                }
            }
        }
        
        private MoneyView AddMoneyView(Transform posToSpawn)
        {
            var moneyView = _objectPool.Pop().GetComponent<MoneyView>();
            _listExecuteObject.AddExecuteObject(moneyView);
            moneyView.OnLostTarget += () => _objectPool.Push(moneyView.gameObject);
            _currentAvailableMoneyViews.Add(moneyView);
            moneyView.Init(_playerConfig.parOfMoneyInView,_playerConfig.moneyMoveSpeed, _playerConfig.moneyScaleChangeSpeed);
            moneyView.gameObject.transform.position = posToSpawn.position;
            moneyView.transform.localScale = moneyView.DefaultScale;
            moneyView.ObjMoveSpeed = 2f;
            moneyView.ObjScaleChangeSpeed = 2f;
            moneyView.OnMoneyClaim += ChangeMoneyCount;
            return moneyView;
        }

        public void IsBuying(bool status)
        {
            _isBuying = status;
        }

        public async void SpendMoney(Transform posToSpawn, Transform posToMove, int buildCost)
        { 
            Debug.Log($"Spend Money {_playerAmountOfMoney}");
            if(_playerAmountOfMoney<=0) return;
            await UniTask.Delay(450);
            if (!_isBuying)
                return;
            Debug.Log(buildCost);
            for (var i = 0; i < buildCost/10; i++)
            {
                Debug.Log(_isBuying);
                if(_playerAmountOfMoney<=0) return;
                if(!_isBuying)
                    return;
                var moneyView = AddMoneyView(posToSpawn);
                moneyView.isMovable = true;
                moneyView.TargetToMove = posToMove.gameObject;
                await UniTask.Delay(300);
            }
        }

        private void ChangeMoneyCount(int amountOfHandpickedMoney, MoneyView moneyView)
        {
            if (_playerAmountOfMoney <= 0 && amountOfHandpickedMoney<0)
            {
                _playerAmountOfMoney = 0;
                return;
            }
            _playerAmountOfMoney += amountOfHandpickedMoney;
            _currentAvailableMoneyViews.Remove(moneyView);
            _objectPool.Push(moneyView.gameObject);
            moneyView.OnMoneyClaim -= ChangeMoneyCount;
            OnPlayerMoneyCountChange?.Invoke(_playerAmountOfMoney);
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
                    moneyView.isMovable = true;
                    moneyView.TargetToMove = contactPlayerView.PlaceToPlants[0].gameObject;
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