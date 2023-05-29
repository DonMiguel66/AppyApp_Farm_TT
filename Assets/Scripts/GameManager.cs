using System;
using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Views;

public class GameManager : MonoBehaviour, IDisposable
{
    private ListInteractableObjects _interactableObject;
    private ListExecuteObject _listExecuteObject;
    private InputController _inputController;
    private CameraController _cameraController;
    private AnimationController _playerAnimatorController;
    private NavMeshController _navMeshController;
    private MoneyController _moneyController;
    private GardenBedsController _gardenBedsController;
    private List<MoneyView> _moneyViews = new List<MoneyView>();
    private List<GardenBedView> _gardenBedViews = new List<GardenBedView>();
    
    [SerializeField]private PlayerView _playerView;
    //изменить на отдельный класс кроликов
    [SerializeField] private List<NavMeshAgent> _navMeshAgents;
    [SerializeField] private Transform _patrolzone;
    private PlayerProfile _playerProfile;
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
        _navMeshController = new NavMeshController(_navMeshAgents,_patrolzone);
        _moneyController = new MoneyController(_playerConfig, _listExecuteObject);
        _gardenBedsController = new GardenBedsController(_interactableObject);
        SubscribeInteractiveObjects();
    }
    void Start()
    {
    }

    private void Update()
    {
        _inputController?.Execute();
        _cameraController?.Execute();
        _playerAnimatorController?.Execute();
        _navMeshController?.Execute();
        for (var i = 0; i < _listExecuteObject.Length; i++)
        {
            var executedObject = _listExecuteObject[i];
            executedObject?.Execute();
        }
    }

    /*private void AddCustomInteractiveObjectsToLists()
    {
        foreach (var o in _interactableObject)
        {
            switch (o)
            {
                case MoneyView moneyView:
                    _moneyViews.Add(moneyView);
                    break;
                case GardenBedView gardenBedView:
                    _gardenBedViews.Add(gardenBedView);
                    break;
            }
        }
    }*/

    private void SubscribeInteractiveObjects()
    {
        foreach (var o in _interactableObject)
        {
            switch (o)
            {
                case MoneyView moneyView:
                    break;
                case GardenBedView gardenBedView:
                    //gardenBedView.OnGardenBedStay += _moneyController.SpendMoney;
                    gardenBedView.OnGardenBedStayAsync += (async (transform1, transform2, arg3, arg4) =>
                    {
                        await _moneyController.Test(transform1, transform2, arg3, arg4);
                    });
                    break;
            }
        }
    }
    
    public void Dispose()
    {
        foreach (var o in _interactableObject)
        {
            switch (o)
            {
                case MoneyView moneyView:
                    break;
                case GardenBedView gardenBedView:
                    //gardenBedView.OnGardenBedStay -= _moneyController.SpendMoney;
                    break;
            }
            if (o is InteractiveObject interactiveObject)
            {
                Destroy(interactiveObject.gameObject);
            }
        }
    }
}
