using System.Collections.Generic;
using Interfaces;
using Views;

namespace Controllers
{
    public class GardenBedsController : BaseController, IExecute
    {
        private List<GardenBedView> _gardenBedViews;
        
        public GardenBedsController(List<GardenBedView> gardenBedViews)
        {
            _gardenBedViews = gardenBedViews;
            foreach (var gardenBedView in _gardenBedViews)
            {
                gardenBedView.Init();
            }
        }
        public void Execute()
        {
        }
    }
}