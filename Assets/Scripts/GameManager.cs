using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ListExecuteObject _interactiveObject;
    private InputController _inputController;
    [SerializeField]private PlayerView _playerView;
    private PlayerProfile _playerProfile;
    [SerializeField] private PlayerConfig _playerConfig;

    private void Awake()
    {
        _interactiveObject = new ListExecuteObject();
        _playerProfile = new PlayerProfile(_playerConfig.moveSpeed);
        _inputController = new InputController(_playerView, _playerProfile);
    }
    void Start()
    {
        
    }
    void Update()
    {
        _inputController?.Execute();
        /*for (var i = 0; i < _interactiveObject.Length; i++)
        {
            var interactiveObject = _interactiveObject[i];
            interactiveObject?.Execute();
        }*/
    }
}
