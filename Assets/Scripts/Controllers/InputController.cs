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
        private float _playerRotateSmoothing;

        private Vector3 move;
        private Vector2 rotate;
        public InputController(PlayerView playerView, PlayerProfile playerProfile)
        {
            _playerView = playerView;
            _playerProfile = playerProfile;
            _playerSpeed = playerProfile.CurrentPlayer.Speed;
            _playerRotateSmoothing = playerProfile.CurrentPlayer.RotateSmoothing;
            _playerInput = playerView.PlayerInput;
        }
            
        public void Execute()
        {
            Vector2 input = _playerInput.actions["Move"].ReadValue<Vector2>();
            rotate = _playerInput.actions["Rotate"].ReadValue<Vector2>();
            move = new Vector3(input.x, 0, input.y);
            _playerView.PlayerRB.velocity = move * _playerSpeed;
            HandleRotation();
        }

        private void HandleRotation()
        {
            Vector3 dir = Vector3.right*rotate.x + Vector3.forward*rotate.y ;
            if (dir.sqrMagnitude > 0.0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(dir, Vector3.up);
                _playerView.transform.rotation = Quaternion.RotateTowards(_playerView.transform.rotation, newRotation,
                    _playerRotateSmoothing * Time.deltaTime);
            }
        }
    }
}