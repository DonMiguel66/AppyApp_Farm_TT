using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using Views;

public class PlayerView : BaseView
{
    [SerializeField] private PlayerInput _playerInput;
    public Rigidbody PlayerRB => _playerRB;
    public PlayerInput PlayerInput => _playerInput;

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}