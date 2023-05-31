using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;

namespace Controllers
{
    public class PlantsController : BaseController
    {
        private AviaryController _aviaryController;
        private PlayerView _playerView;
        private int _currentPlayerPlantNumber;

        private List<ClaimableObject> _plantsViews = new List<ClaimableObject>();
        private Transform[] _plantHoldPositions;
        private Transform _currFreePos;
        private int _requiredPlantNumber;
        
        public PlantsController(PlayerView playerView, AviaryController aviaryController)
        {
            _aviaryController = aviaryController;
            _playerView = playerView;
            _plantHoldPositions = _playerView.PlaceToPlants;
        }

        public async void AddPlant(ClaimableObject claimableObject)
        {
            _plantsViews.Add(claimableObject);
            await UniTask.Delay(200);
            //CheckFreePos();
            Debug.Log($"Before Pos:{claimableObject.transform.localPosition} Rot:{claimableObject.transform.localRotation}");
            claimableObject.transform.SetParent(_currFreePos.transform, true);
            claimableObject.transform.localPosition = Vector3.zero;
            claimableObject.transform.localRotation = Quaternion.identity;
            claimableObject.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            Debug.Log($"After Pos:{claimableObject.transform.localPosition} Rot:{claimableObject.transform.localRotation}");
            claimableObject.GetComponent<CarrotView>().OnFeeding += _aviaryController.CheckCarrotToFeed;
            claimableObject.OnDestroyingObj += () => _plantsViews.Remove(claimableObject);
        }

        public Transform CheckFreePos()
        {
            foreach (var plantHoldPosition in _plantHoldPositions)
            {
                if(plantHoldPosition.GetComponentInChildren<ClaimableObject>())
                    continue;
                _currFreePos = plantHoldPosition;
            }
            return _currFreePos;
        }
        
        public async void PlantToAviary(Transform toTransform, int currentNeededCountOfPlants)
        {
            Debug.Log(currentNeededCountOfPlants);
            foreach (var claimableObject in _plantsViews.ToList())
            {
                claimableObject.gameObject.transform.SetParent(null);
                claimableObject.isMovable = true;
                claimableObject.TargetToMove = toTransform.gameObject;
                await UniTask.Delay(200);
                if(currentNeededCountOfPlants < _plantsViews.Count)
                    return;
            }
        }
    }
}