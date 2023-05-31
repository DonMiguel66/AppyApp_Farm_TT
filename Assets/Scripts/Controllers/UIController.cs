using DG.Tweening;
using Views;

namespace Controllers
{
    public class UIController : BaseController
    {
        private UIView _uiView;

        public UIController(UIView uiView)
        {
            _uiView = uiView;
            _uiView.SetMoneyAmount(0);
            _uiView.TransitionImage.enabled = true;
        }

        public void ChangeUI(int moneyAmount)
        {
            _uiView.SetMoneyAmount(moneyAmount);
        }

        public void LoadGame()
        {
            _uiView.TransitionImage.DOFade(0,0.5f).OnComplete(()=>
            {
                _uiView.TransitionImage.enabled = false;
            });
        }
    }
}