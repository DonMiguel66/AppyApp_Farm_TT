using UnityEngine;

internal class SafeareaDetection : MonoBehaviour
{
    public delegate void SafeAreaChanged(Rect safeArea);

    public static event SafeAreaChanged OnSafeAreaChanged;

    private Rect _safeArea;
    void Awake()
    {
        _safeArea = Screen.safeArea;
        if (_safeArea != Screen.safeArea)
        {
            _safeArea = Screen.safeArea;
            OnSafeAreaChanged?.Invoke(_safeArea);
        }
    }
    
    void Update()
    {
        if (_safeArea != Screen.safeArea)
        {
            _safeArea = Screen.safeArea;
            
            OnSafeAreaChanged?.Invoke(_safeArea);
        }
    }
}
