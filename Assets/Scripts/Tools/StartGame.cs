using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Image _transitionImage;

    private void Awake()
    {
        _startButton.onClick.AddListener(InitializeGame);
        _transitionImage.enabled = false;
    }

    private void InitializeGame()
    {
        _transitionImage.enabled = true;
        _transitionImage.DOFade(1,0.3f).OnComplete(()=>
        {
            SceneManager.LoadScene("MainScene");
        });
    }
}
