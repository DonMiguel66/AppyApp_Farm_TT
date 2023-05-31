using UnityEngine;
using UnityEngine.InputSystem;
using Views;

public class PlayerView : BaseView
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Transform[] _placeToPlants;
    public Rigidbody PlayerRB => _playerRB;
    public Animator Animator => _animator;
    public PlayerInput PlayerInput => _playerInput;

    public int CountOfClaimableObject
    {
        get => _countOfClaimableObject;
        set => _countOfClaimableObject = value;
    }

    public Transform[] PlaceToPlants => _placeToPlants;

    private int _countOfClaimableObject;

    public bool CheckForSpaceToCO()
    {
        if (_countOfClaimableObject < 2)
        {
            _countOfClaimableObject++;
            return true;
        }
        return false;
    }

    public void ClearSpaceToCO(int count)
    {
        _countOfClaimableObject -= count;
    }

}
