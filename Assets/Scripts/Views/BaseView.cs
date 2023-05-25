using Interfaces;
using UnityEditor.Animations;
using UnityEngine;

namespace Views
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] protected Rigidbody _playerRB;
        [SerializeField] protected Collider _playerCollider;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected AnimatorController _animatorController;

        public abstract void Move();
    }
}