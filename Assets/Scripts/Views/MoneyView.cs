using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
            Debug.Log("Ch");
            transform.DOScale(new Vector3(0f, 0f, 0f), 0.1f).OnComplete(() => Destroy(gameObject));
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

        public async void MoveToAsync(PlayerView playerView)
        {
            await MoveTo(playerView);
        }
        
        private async UniTask MoveTo(PlayerView playerView)
        {
            transform.LookAt(playerView.transform);
            Vector3 newPos = new Vector3(playerView.transform.position.x, playerView.transform.position.y + 10f, playerView.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 10);
            await UniTask.CompletedTask;
        }
    }
}