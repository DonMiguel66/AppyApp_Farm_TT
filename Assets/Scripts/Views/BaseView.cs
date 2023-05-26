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

    }
}