using System;
using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;
using Views;

public class GameManager : MonoBehaviour, IDisposable
{
    private ListInteractableObjects _interactableObject;
    private ListExecuteObject _listExecuteObject;
    private InputController _inputController;
    private CameraController _cameraController;
    private AnimationController _playerAnimatorController;
    private AviaryController _aviaryController;
    private MoneyController _moneyController;
    private GardenBedsController _gardenBedsController;
    private PlantsController _plantsController;
    private UIController _uiController;
    private List<MoneyView> _moneyViews = new List<MoneyView>();
    private List<GardenBedView> _gardenBedViews = new List<GardenBedView>();
    
    private PlayerProfile _playerProfile;
    
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private UIView _uiView;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private Transform _mainPivotCamera;

    private void Awake()
    {
        _interactableObject = new ListInteractableObjects();
        _listExecuteObject = new ListExecuteObject();
        _playerProfile = new PlayerProfile(_playerConfig.moveSpeed, _playerConfig.rotateSmoothing);
        _inputController = new InputController(_playerView, _playerProfile);
        _cameraController = new CameraController(_playerView.transform, _mainPivotCamera.transform);
        _playerAnimatorController = new AnimationController(_playerView);
        _aviaryController = new AviaryController(_playerConfig);
        _moneyController = new MoneyController(_playerConfig, _listExecuteObject);
        _plantsController = new PlantsController(_playerView, _aviaryController);
        _gardenBedsController = new GardenBedsController(_playerConfig,_interactableObject, _listExecuteObject,_plantsController);
        
        _uiController = new UIController(_uiView);
    }
    void Start()
    {
        SubscribeInteractiveObjects();
        SubscribeExecutableObjects();
        _moneyController.OnPlayerMoneyCountChange += _uiController.ChangeUI;
        _gardenBedsController.OnPlantTakenByPlayer += _plantsController.AddPlant;
        _uiController.LoadGame();
    }

    private void Update()
    {
        _inputController?.Execute();
        _cameraController?.Execute();
        _playerAnimatorController?.Execute();
        _aviaryController?.Execute();
        for (var i = 0; i < _listExecuteObject.Length; i++)
        {
            var executedObject = _listExecuteObject[i];
            executedObject?.Execute();
        }
    }

    private void SubscribeInteractiveObjects()
    {
        foreach (var o in _interactableObject)
        {
            if (o is GardenBedView gardenBedView)
            {
                gardenBedView.OnBuyingStateChange += _moneyController.IsBuying;
                gardenBedView.OnGardenBedEnter += _moneyController.SpendMoney;
            }
            if (o is AviaryView rabbitAviaryView)
            {
                rabbitAviaryView.OnBuyingStateChange += _moneyController.IsBuying;
                rabbitAviaryView.OnAviaryBuildEnter += _moneyController.SpendMoney;
                rabbitAviaryView.OnAviaryFeedEnter += _plantsController.PlantToAviary;
            }
        }

    }

    private void SubscribeExecutableObjects()
    {
        for (var i = 0; i < _listExecuteObject.Length; i++)
        {
            if (_listExecuteObject[i] is MoneyView moneyView)
            {
                moneyView.OnMoneyPayForGarden += _gardenBedsController.CheckMoneyToBuild;
                moneyView.OnMoneyPayForAviary += _aviaryController.CheckMoneyToBuild;
            }
            if (_listExecuteObject[i] is CarrotView carrotView)
            {
                carrotView.OnFeeding += _aviaryController.CheckCarrotToFeed;
            }
        }
    }
    
    public void Dispose()
    {
        foreach (var o in _interactableObject)
        {
            if (o is InteractiveObject interactiveObject)
            {
                Destroy(interactiveObject.gameObject);
            }
        }
    }
}
