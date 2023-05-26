using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class AnimationController : BaseController, IExecute
    {
        private PlayerView _playerView;
        private Animator _animator;
        private Rigidbody _playerRB;
        private static readonly int Speed = Animator.StringToHash("speed");

        public AnimationController(PlayerView playerView)
        {
            _playerView = playerView;
            _playerRB = playerView.PlayerRB;
            _animator = playerView.Animator;
        }

        public void Execute()
        {
            if(_playerRB.velocity.normalized == Vector3.zero)
                _animator.SetFloat(Speed,0);
            else
            {
                _animator.SetFloat(Speed,1f);
            }
            /*if(_playerRB.velocity.x == 0 && _playerRB.velocity.y ==0)
                _animator.SetFloat(Speed,0);
            else if (_playerRB.velocity.x is > 0 and < 2.5f || _playerRB.velocity.y is > 0 and < 2.5f)
            {
                _animator.SetFloat(Speed, 0.5f);
            }
            else if (_playerRB.velocity.x > 2.5f || _playerRB.velocity.y > 2.5f)
            {
                _animator.SetFloat(Speed, 1f);
            }*/
        }
    }
}