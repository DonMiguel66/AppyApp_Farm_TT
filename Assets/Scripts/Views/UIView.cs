using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerMoneyTxt;
        [SerializeField] private Image _transitionImage;

        public TMP_Text PlayerMoneyTxt
        {
            get => _playerMoneyTxt;
            set => _playerMoneyTxt = value;
        }

        public Image TransitionImage
        {
            get => _transitionImage;
            set => _transitionImage = value;
        }

        public void SetMoneyAmount(int moneyAmount)
        {
            _playerMoneyTxt.text = moneyAmount.ToString();
        }
    }
}