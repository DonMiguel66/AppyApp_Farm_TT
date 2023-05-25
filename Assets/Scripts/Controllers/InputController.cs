using Interfaces;
using Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : BaseController, IExecute
    {
        private readonly PlayerView _playerView;
        private PlayerInput _playerInput;
        private PlayerProfile _playerProfile;
        private float _playerSpeed;
        public InputController(PlayerView playerView, PlayerProfile playerProfile)
        {
            _playerView = playerView;
            _playerProfile = playerProfile;
            _playerSpeed = playerProfile.CurrentPlayer.Speed;
            _playerInput = playerView.PlayerInput;
        }
            
        public void Execute()
        {
            Vector2 input = _playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            _playerView.PlayerRB.velocity = move*_playerSpeed;
        }
    }
}