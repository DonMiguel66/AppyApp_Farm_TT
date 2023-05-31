using UnityEngine;

namespace Views
{
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField] protected Rigidbody _playerRB;
        [SerializeField] protected Animator _animator;
    }
}