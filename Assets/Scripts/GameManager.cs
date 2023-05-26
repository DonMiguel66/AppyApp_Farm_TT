using System;
using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Views;

public class GameManager : MonoBehaviour, IDisposable
{
    private ListExecuteObject _interactiveObject;
    private InputController _inputController;
    private CameraController _cameraController;
    private AnimationController _playerAnimatorController;
    private NavMeshController _navMeshController;
    private MoneyController _moneyController;
    private List<MoneyView> _moneyViews = new List<MoneyView>();
    
    [SerializeField]private PlayerView _playerView;
    //изменить на отдельный класс кроликов
    [SerializeField] private List<NavMeshAgent> _navMeshAgents;
    [SerializeField] private Transform _patrolzone;
    private PlayerProfile _playerProfile;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private Transform _mainPivotCamera;

    private void Awake()
    {
        _interactiveObject = new ListExecuteObject();
        AddCustomInteractiveObjectsToLists();
        _playerProfile = new PlayerProfile(_playerConfig.moveSpeed, _playerConfig.rotateSmoothing);
        _inputController = new InputController(_playerView, _playerProfile);
        _cameraController = new CameraController(_playerView.transform, _mainPivotCamera.transform);
        _playerAnimatorController = new AnimationController(_playerView);
        _navMeshController = new NavMeshController(_navMeshAgents,_patrolzone);
        _moneyController = new MoneyController(_playerConfig, _moneyViews);
    }
    void Start()
    {
    }
    void Update()
    {
        _inputController?.Execute();
        _cameraController?.Execute();
        _playerAnimatorController?.Execute();
        _navMeshController?.Execute();
        for (var i = 0; i < _interactiveObject.Length; i++)
        {
            var interactiveObject = _interactiveObject[i];
            interactiveObject?.Execute();
        }
    }

    private void AddCustomInteractiveObjectsToLists()
    {
        foreach (var o in _interactiveObject)
        {
            if (o is MoneyView moneyView)
            {
                _moneyViews.Add(moneyView);
            }
        }
    }
    public void Dispose()
    {
        foreach (var o in _interactiveObject)
        {
            if (o is InteractiveObject interactiveObject)
            {
                Destroy(interactiveObject.gameObject);
            }
        }
    }
}
