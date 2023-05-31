using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Views;
using Object = UnityEngine.Object;

namespace Controllers
{
    public enum AviaryState
    {
        NotBuilt,
        Built
    }
    public class AviaryController : BaseController, IExecute
    {
        private NavMeshController _navMeshController;
        private AviaryView _aviaryView;
        private AviaryModel _aviaryModel = new AviaryModel();
        private List<NavMeshAgent> _navMeshAgents;

        private bool isBuilded;
        public AviaryController(PlayerConfig playerConfig)
        {
            _aviaryView = Object.FindObjectOfType<AviaryView>();
            _aviaryModel.BuildCost = 60;
            _aviaryModel.NeededCarrotNumber = playerConfig.neededAviaryPlantsCount;
            _navMeshAgents = _aviaryView.NavMeshAgents;
            _aviaryView.Init(_aviaryModel.BuildCost, _aviaryModel.NeededCarrotNumber);
        }

        public void BuildAviary()
        {
            _aviaryView.Aviary.gameObject.SetActive(true);
            foreach (var navMeshAgent in _navMeshAgents)
            {
                navMeshAgent.gameObject.SetActive(true);
            }
            _navMeshController = new NavMeshController(_navMeshAgents,_aviaryView.transform);
            isBuilded = true;
            _aviaryView.AviaryState = AviaryState.Built;
        }
        
        public void CheckMoneyToBuild(int amountOfMoney, AviaryView aviaryView)
        {
            aviaryView.ChangePriceAndUI(amountOfMoney);
            if (aviaryView.BuildCost <= 0)
            {
                aviaryView.SwitchPulsatingAreaColor();
                aviaryView.BuildPriceLogo.gameObject.SetActive(false);
                aviaryView.CarrotCountLogo.gameObject.SetActive(true);
                BuildAviary();
            }
        }

        public void CheckCarrotToFeed(int amountOfCarrot)
        {
            if (_aviaryModel.CurrentCarrotNumber <= _aviaryModel.NeededCarrotNumber)
            {
                _aviaryModel.CurrentCarrotNumber += amountOfCarrot;
                SetNewNumInView(_aviaryModel.CurrentCarrotNumber);
            }
        }

        private async void SetNewNumInView(int amount)
        {
            var currentNum = _aviaryView.ChangeCarrotCountAndUI(amount);
            Debug.Log(currentNum);
            if (currentNum >= _aviaryModel.NeededCarrotNumber)
            {
                await UniTask.Delay(500);
                _aviaryView.CarrotCountLogo.gameObject.SetActive(false);
                _aviaryView.PulsatingArea.gameObject.SetActive(false);
                _aviaryView.IsInteractable = false;
                //RestartFeeding();
            }
        }

        public async void RestartFeeding()
        {
            await UniTask.Delay(7000);
            SetNewNumInView(0);
            _aviaryView.CarrotCountLogo.gameObject.SetActive(true);
            _aviaryView.PulsatingArea.gameObject.SetActive(true);
            _aviaryView.IsInteractable = true;
        }
        public void Execute()
        {
            if(isBuilded)
                _navMeshController.Execute();
        }
    }
}