using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using Tools;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Controllers
{
    public enum GardenBedState
    {
        NotBuilt,
        InGrowthProcess,
        Grown 
    }
    
    public class GardenBedsController : BaseController, IExecute
    {
        private List<GardenBedView> _gardenBedViews = new List<GardenBedView>();
        private readonly ListInteractableObjects _listInteractableObject;
        private readonly ListExecuteObject _listExecuteObject;
        private PlayerConfig _playerConfig;
        private PlantsController _plantsController;

        private int _buildCost;
        private int _parOfMoney;
        private int _nextIndexOfGb;
        private int _growthTime;
        private GardenBedState _currentGardenBedState;

        public event Action OnNewPlantSpawned;
        public event Action<ClaimableObject> OnPlantTakenByPlayer;
        private readonly ResourcePath _viewPath = new ResourcePath {PathResource = "Prefabs/CarrotView"};
        public GardenBedsController(PlayerConfig playerConfig, ListInteractableObjects listInteractableObject, ListExecuteObject listExecuteObject, PlantsController plantsController)
        {
            _playerConfig = playerConfig;
            _plantsController = plantsController;
            _buildCost = _playerConfig._plantBuiltCost;
            _growthTime = _playerConfig._plantGrowthTime;
            _parOfMoney = _playerConfig.parOfMoneyInView;
            _listInteractableObject = listInteractableObject;
            _listExecuteObject = listExecuteObject;
            _currentGardenBedState = GardenBedState.NotBuilt;
            _nextIndexOfGb = 0;
            AddViewsFromObjectList();
        }
        
        private void AddViewsFromObjectList()
        {
            foreach (var o in _listInteractableObject)
            {
               // Debug.Log(_listInteractableObject.Current);
                if (o is GardenBedView gardenBedView)
                {
                    _gardenBedViews.Add(gardenBedView);
                    gardenBedView.OnPlantGrowBegan += SetGardenPlant;
                    gardenBedView.OnPlantGrown += TakeGrownPlant;
                }
            }
            foreach (var gardenBedView in _gardenBedViews)
            {
                gardenBedView.gameObject.SetActive(false);
            }
            SetNextActiveGB();
            Debug.Log(_nextIndexOfGb);
        }
        
        
        public void CheckMoneyToBuild(int amountOfMoney, GardenBedView gardenBedView)
        {
            Debug.Log(gardenBedView.BuildCost);
            Debug.Log(amountOfMoney);
            gardenBedView.ChangeCostAndUI(amountOfMoney);
            if (gardenBedView.BuildCost <= 0)
            {
                gardenBedView.PulsatingArea.gameObject.SetActive(false);
                gardenBedView.BuildPriceLogo.gameObject.SetActive(false);
                gardenBedView.GardenGround.gameObject.SetActive(true);
                SetGardenPlant(gardenBedView);
                SetNextActiveGB();
            }
        }

        private void SetNextActiveGB()
        {
            if (_nextIndexOfGb >= _gardenBedViews.Count)
            {
                return;
            }
            Debug.Log("index " + _nextIndexOfGb);
            _gardenBedViews[_nextIndexOfGb].gameObject.SetActive(true);
            _gardenBedViews[_nextIndexOfGb].Init(_buildCost, _parOfMoney);
            _nextIndexOfGb++;
        }

        private async void SetGardenPlant(GardenBedView gardenBedView)
        {
            gardenBedView.BedState = GardenBedState.InGrowthProcess;
            gardenBedView.CarrotStem.gameObject.SetActive(true);
            await CarrotGrowCycle(gardenBedView);
        }

        private void TakeGrownPlant(GardenBedView gardenBedView, CarrotView carrotView, PlayerView playerView)
        {
            if(playerView.CheckForSpaceToCO() && gardenBedView.CarrotView)
            {
                //carrotView.TargetToMove = playerView.PlaceToPlants[0].gameObject;
                carrotView.TargetToMove = _plantsController.CheckFreePos().gameObject;
                carrotView.isMovable = true;
                carrotView.OnFeeding += playerView.ClearSpaceToCO;
                carrotView.OnPlayerTakePlant += () => SetGardenPlant(gardenBedView);
                OnPlantTakenByPlayer?.Invoke(carrotView);
                gardenBedView.CarrotView = null;
            }
        }
        
        private async UniTask CarrotGrowCycle(GardenBedView gardenBedView)
        {
            await UniTask.Delay(_growthTime * 1000);
            gardenBedView.CarrotStem.gameObject.SetActive(false);
            InstantiateGrownObject(_viewPath, gardenBedView);
            gardenBedView.BedState = GardenBedState.Grown;
        }
        private void InstantiateGrownObject(ResourcePath viewPath, GardenBedView gardenBedView)
        {
            var obj = Object.Instantiate(ResourceLoader.LoadPrefab(viewPath));
            obj.SetActive(false);
            obj.transform.position = gardenBedView.transform.position + new Vector3(0f,0.25f, 0f);
            obj.SetActive(true);
            gardenBedView.CarrotView = obj.GetComponent<CarrotView>();
            _listExecuteObject.AddExecuteObject(gardenBedView.CarrotView);
            OnNewPlantSpawned?.Invoke();
        }
        
        public void Execute()
        {
        }
    }
}