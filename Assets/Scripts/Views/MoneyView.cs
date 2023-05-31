using System;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class MoneyView : ClaimableObject, IMoney, IExecute
    {
        private int _parOfMoney;
        public event Action<int, MoneyView> OnMoneyClaim;
        public event Action<int, GardenBedView> OnMoneyPayForGarden;
        public event Action<int, AviaryView> OnMoneyPayForAviary;

        public event Action OnLostTarget;

        public void Init(int moneyCount, float moneyMoveSpeed,float moneyScaleChangeSpeed)
        {
            _parOfMoney = moneyCount;
            _objMoveSpeed = moneyMoveSpeed;
            _objScaleChangeSpeed = moneyScaleChangeSpeed;
        }

        public void Execute()
        {
            if(!_targetToMove)
            {
                return;
            }
            Move();
        }

        protected override void Move()
        {
            if (!isMovable)
            {
                return;
            }
            if(DistanceToTargetCheck())
            {
                transform.LookAt(_targetToMove.transform);
                var targetTransform = _targetToMove.transform;
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position,
                    _objMoveSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f,0.1f,0.1f),
                    Time.deltaTime * _objScaleChangeSpeed);
            }
            else
            {
                isMovable = false;
                if(_targetToMove.GetComponent<PlaceToHold>())
                {
                    OnMoneyClaim?.Invoke(_parOfMoney, this);
                }
                else
                {
                    OnMoneyClaim?.Invoke(-_parOfMoney, this);
                }
                if (_targetToMove.GetComponentInParent<GardenBedView>())
                {
                    OnMoneyPayForGarden?.Invoke(_parOfMoney, _targetToMove.GetComponentInParent<GardenBedView>());
                }
                if (_targetToMove.GetComponentInParent<AviaryView>())
                {
                    OnMoneyPayForAviary?.Invoke(_parOfMoney, _targetToMove.GetComponentInParent<AviaryView>());
                }
            }
        }

        public void PayForGarden(GardenBedView gardenBedView)
        {
            OnMoneyPayForGarden?.Invoke(_parOfMoney, gardenBedView);
        }
        public void PayForAviary(AviaryView aviaryView)
        {
            OnMoneyPayForAviary?.Invoke(_parOfMoney, aviaryView);
        }
        
        private void OnDestroy()
        {
            isMovable = false;
            _targetToMove = null;
        }
    }
}