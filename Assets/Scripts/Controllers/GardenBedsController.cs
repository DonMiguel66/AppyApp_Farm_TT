using System.Collections.Generic;
using Interfaces;
using Views;

namespace Controllers
{
    public class GardenBedsController : BaseController, IExecute
    {
        private List<GardenBedView> _gardenBedViews = new List<GardenBedView>();
        private readonly ListInteractableObjects _listInteractableObject;
        
        public GardenBedsController(ListInteractableObjects listInteractableObject)
        {
            _listInteractableObject = listInteractableObject;
            AddViewsFromObjectList();
        }
        
        private void AddViewsFromObjectList()
        {
            foreach (var o in _listInteractableObject)
            {
                if (o is GardenBedView gardenBedViewView)
                {
                    _gardenBedViews.Add(gardenBedViewView);
                    gardenBedViewView.Init(10);
                }
            }
        }
        public void Execute()
        {
        }
    }
}