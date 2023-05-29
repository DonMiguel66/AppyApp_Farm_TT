using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Views
{
    public class MoneyView : InteractiveObject, IMoney, IExecute
    {
        private int _moneyCount;
        private float _moneyMoveSpeed;
        private float _moneyScaleChangeSpeed;
        private float _lengthFly;
        [SerializeField]private Vector3 _defaultScale;
        public event Action<int, MoneyView> OnMoneyClaim;

        public bool isMovable;
        [SerializeField]private GameObject _targetToMove;

        public GameObject TargetToMove
        {
            get => _targetToMove;
            set => _targetToMove = value;
        }

        public Vector3 DefaultScale => _defaultScale;

        public float MoneyMoveSpeed
        {
            get => _moneyMoveSpeed;
            set => _moneyMoveSpeed = value;
        }

        public float MoneyScaleChangeSpeed
        {
            get => _moneyScaleChangeSpeed;
            set => _moneyScaleChangeSpeed = value;
        }

        private void Awake()
        {
            //_lengthFly = Range(1.0f, 1.5f);
            _lengthFly = 0.75f;
            _defaultScale = transform.localScale;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
        }

        public void Init(int moneyCount, float moneyMoveSpeed,float moneyScaleChangeSpeed)
        {
            _moneyCount = moneyCount;
            _moneyMoveSpeed = moneyMoveSpeed;
            _moneyScaleChangeSpeed = moneyScaleChangeSpeed;
        }
        
        protected override void EnterInteraction()
        {/*
            IsInteractable = false;
            OnMoneyClaim?.Invoke(_moneyCount, this);
            Debug.Log($"{transform.position}"); */
        }

        public void Execute()
        {
            if(!IsInteractable){return;}
            Fly();
            if(!_targetToMove) return;
            Move();
        }

        private void Fly()
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, Mathf.PingPong(Time.time, _lengthFly),
                localPosition.z);
            transform.localPosition = localPosition;
        }

        private void Move()
        {
            if (!isMovable) return;
            if(DistanceToTargetCheck())
            {
                transform.LookAt(_targetToMove.transform);
                var targetTransform = _targetToMove.transform;
                Vector3 newPos = new Vector3(targetTransform.position.x, targetTransform.position.y + 10f,
                    targetTransform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * _moneyMoveSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero,
                    Time.deltaTime * _moneyScaleChangeSpeed);
            }
            else
            {
                isMovable = false;
                if(_targetToMove.GetComponent<PlayerView>())
                    OnMoneyClaim?.Invoke(_moneyCount, this);
                else
                {
                    Debug.Log("Spend");
                    OnMoneyClaim?.Invoke(-_moneyCount, this);
                }
            }
        }

        private bool DistanceToTargetCheck()
        {
            var distance = Vector3.Distance(transform.position, _targetToMove.transform.position);
            return !(distance <= 0.5f);
        }
        
        /*public void MoveTo(Transform target)
        {
            transform.LookAt(target);
            transform.Translate((target.position - transform.position)*_moneyMoveSpeed* Time.deltaTime, Space.World);
        }*/

        public async void MoveToAsync(Transform target)
        {
            await MoveTo(target);
        }
        
        private async UniTask MoveTo (Transform target)
        {
            transform.LookAt(target.transform);
            var targetTransform = target.transform;
            Vector3 newPos = new Vector3(targetTransform.position.x, targetTransform.position.y + targetTransform.lossyScale.y/2, targetTransform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * _moneyMoveSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * _moneyScaleChangeSpeed);
            await UniTask.CompletedTask;
        }

        private void OnDestroy()
        {
            isMovable = false;
            _targetToMove = null;
        }
    }
}