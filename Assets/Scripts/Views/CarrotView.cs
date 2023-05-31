using System;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class CarrotView : ClaimableObject, IExecute
    {
        public event Action<int> OnFeeding;
        public event Action OnPlayerTakePlant;
        public event Action OnDestroyinObj;
        protected override void Awake()
        {
            base.Awake();
            _objMoveSpeed = 8f;
            _objScaleChangeSpeed = 8f;
        }

        protected override void Move()
        {
            if(DistanceToTargetCheck())
            {
                transform.LookAt(_targetToMove.transform);
                var targetTransform = _targetToMove.transform;
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position,
                    _objMoveSpeed * Time.deltaTime);
                if (targetTransform.GetComponent<PlaceToHold>())
                    return;
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f,0.1f,0.1f),
                    Time.deltaTime * (_objScaleChangeSpeed/2));
            }
            else
            {
                isMovable = false;
                if(_targetToMove.GetComponent<PlaceToHold>())
                {
                    OnPlayerTakePlant?.Invoke();
                }
                if (_targetToMove.GetComponentInParent<AviaryView>())
                {
                    OnFeeding?.Invoke(1);
                    Destroy(gameObject);
                }
            }
        }

        public void Execute()
        {
            if (!isMovable) return;
            Move();
        }
    }
}