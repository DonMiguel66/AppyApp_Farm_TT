using UnityEngine;

internal class SafeareaPanel : MonoBehaviour
{
    [SerializeField]private RectTransform _rectTransform;

    private void Awake()
    {
        RefreshPanel(Screen.safeArea);
    }

    private void OnEnable()
    {
        SafeareaDetection.OnSafeAreaChanged += RefreshPanel;
    }

    private void OnDisable()
    {
        SafeareaDetection.OnSafeAreaChanged -= RefreshPanel;
    }

    private void RefreshPanel(Rect safearea)
    {
        Vector2 anchorMin = safearea.position;
        Vector2 anchorMax = safearea.position + safearea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }

    
}