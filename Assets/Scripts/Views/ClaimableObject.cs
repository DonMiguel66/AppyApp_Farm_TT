using System;
using Interfaces;
using UnityEngine;

namespace Views
{
    public abstract class ClaimableObject : MonoBehaviour, IExecute
    {
        protected float _objMoveSpeed;
        protected float _objScaleChangeSpeed;
        public event Action OnDestroyingObj;
        
        public bool isMovable;
        [SerializeField]protected GameObject _targetToMove;
        
        private Vector3 _defaultScale;
        
        public Vector3 DefaultScale => _defaultScale;

        protected virtual void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public float ObjMoveSpeed
        {
            get => _objMoveSpeed;
            set => _objMoveSpeed = value;
        }

        public float ObjScaleChangeSpeed
        {
            get => _objScaleChangeSpeed;
            set => _objScaleChangeSpeed = value;
        }
        
        public GameObject TargetToMove
        {
            get => _targetToMove;
            set => _targetToMove = value;
        }
        
        protected bool DistanceToTargetCheck()
        {
            var distance = Vector3.Distance(transform.position, _targetToMove.transform.position);
            return !(distance <= 0.1f);
        }

        protected abstract void Move();

        public void Execute()
        {
            if(!_targetToMove) return;
            Move();
        }
        

        private void OnDestroy()
        {
            OnDestroyingObj?.Invoke();
        }
    }
}