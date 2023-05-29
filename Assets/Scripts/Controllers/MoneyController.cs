using System;
using System.Collections.Generic;
using System.Threading;
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
        public event Action OnMoneyCountChange;
        private MoneyHolderView _moneyHolder;
        private PlayerConfig _playerConfig;
        
        private int _playerAmountOfMoney;
        private float _moneyPickupRadius;
        private bool _isReadyToMoveMoney = true;
        private int _counter = 0;
        private readonly ResourcePath _viewPath = new ResourcePath {PathResource = "Prefabs/MoneyView"};

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
            //var moneyView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), posToSpawn, true).GetComponent<MoneyView>();
            var moneyView = _objectPool.Pop().GetComponent<MoneyView>();
            _listExecuteObject.AddExecuteObject(moneyView);
            _currentAvailableMoneyViews.Add(moneyView);
            moneyView.Init(_playerConfig.parOfMoneyInView,_playerConfig.moneyMoveSpeed, _playerConfig.moneyScaleChangeSpeed);
            moneyView.gameObject.transform.position = posToSpawn.position;
            moneyView.transform.localScale = moneyView.DefaultScale;
            moneyView.MoneyMoveSpeed = 1f;
            moneyView.MoneyScaleChangeSpeed = 1f;
            moneyView.OnMoneyClaim += ChangeMoneyCount;
            return moneyView;
        }

        public async UniTask Test(Transform posToSpawn, Transform posToMove, int buildCost,CancellationToken cts)
        {
            if(_playerAmountOfMoney<=0) return;
            if(_isReadyToMoveMoney)
            {
                _isReadyToMoveMoney = false;
                Debug.Log($"Spend Money {_playerAmountOfMoney}");
                _playerAmountOfMoney -= _playerConfig.parOfMoneyInView;
                var moneyView = AddMoneyView(posToSpawn);
                moneyView.isMovable = true;
                moneyView.TargetToMove = posToMove.gameObject;
                await UniTask.Delay(1000, cancellationToken: cts);
                _isReadyToMoveMoney = true;
                //await UniTask.WaitUntilValueChanged(moneyView, x => !x.isActiveAndEnabled, cancellationToken: cts);
            }
        }

        public void SpendMoney(Transform posToSpawn, Transform posToMove)
        {
            if(_playerAmountOfMoney<=0) return;
            Debug.Log($"Spend Money {_playerAmountOfMoney}");
            _playerAmountOfMoney -= _playerConfig.parOfMoneyInView;
            var moneyView = AddMoneyView(posToSpawn);
            moneyView.isMovable = true;
            moneyView.TargetToMove = posToMove.gameObject;
        }
        
        private void ChangeMoneyCount(int amountOfHandpickedMoney, MoneyView moneyView)
        {
            Debug.Log($"Длина {_listExecuteObject.Length}");
            _playerAmountOfMoney += amountOfHandpickedMoney;
            //_listExecuteObject.RemoveExecuteObject(moneyView);
            _currentAvailableMoneyViews.Remove(moneyView);
            _objectPool.Push(moneyView.gameObject);
            Debug.Log($"Change Money: {_playerAmountOfMoney}");
            moneyView.OnMoneyClaim -= ChangeMoneyCount;
            OnMoneyCountChange?.Invoke();
            Debug.Log($"{_counter++}");
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
                    moneyView.isMovable = true;
                    moneyView.TargetToMove = contactPlayerView.gameObject;
                    //moneyView.MoveToAsync(contactPlayerView.transform);
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