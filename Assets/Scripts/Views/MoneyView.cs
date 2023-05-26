using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using static UnityEngine.Random;

namespace Views
{
    public class MoneyView : InteractiveObject, IMoney
    {
        private int _moneyCount;
        private float _moneyFlySpeed;
        private float _lengthFly;
        public event Action<int, MoneyView> OnMoneyClaim;

        private void Awake()
        {
            //_lengthFly = Range(1.0f, 1.5f);
            _lengthFly = 0.75f;
        }

        public void Init(int moneyCount, float moneyFlySpeed)
        {
            _moneyCount = moneyCount;
            _moneyFlySpeed = moneyFlySpeed;
        }
        
        protected override void EnterInteraction()
        {
            IsInteractable = false;
            OnMoneyClaim?.Invoke(_moneyCount, this);
        }

        public override void Execute()
        {
            if(!IsInteractable){return;}
            Fly();
        }

        private void Fly()
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(localPosition.x, Mathf.PingPong(Time.time, _lengthFly),
                localPosition.z);
            transform.localPosition = localPosition;
        }

        public void MoveTo(Transform target)
        {
            transform.LookAt(target);
            transform.Translate((target.position - transform.position)*_moneyFlySpeed* Time.deltaTime, Space.World);
        }
    }
}