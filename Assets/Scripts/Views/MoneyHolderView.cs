using System;
using System.Collections.Generic;
using UnityEngine;

namespace Views
{
    public class MoneyHolderView : InteractiveZones
    {
        private List<MoneyView> _currentMoneyViewsInHolder;
        private float _moneyPickupRadius;
        
        public void Init(float moneyPickupRadius, List<MoneyView> currentMoneyViewsInHolder)
        {
            _moneyPickupRadius = moneyPickupRadius;
            _currentMoneyViewsInHolder = currentMoneyViewsInHolder;
        }
        protected override void EnterInteraction()
        {
           Debug.Log("Enter");
        }

        protected override void StayInteraction()
        {
            foreach (var moneyView in _currentMoneyViewsInHolder)
            {
                if ((PlayerTransform.position - moneyView.transform.position).magnitude < _moneyPickupRadius)
                {
                    Debug.Log(PlayerTransform.name);
                    moneyView.MoveTo(PlayerTransform);
                }
            }
        }

        /*private void OnTriggerStay(Collider other)
        {
            //if (!IsInteractable || !other.CompareTag("Player"))
            if (!IsInteractable || !other.GetComponent<PlayerView>())
            {
                return;
            }
            StayInteraction();
        }*/
    }
}