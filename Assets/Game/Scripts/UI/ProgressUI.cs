using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Image _fill;
    
    public void SetProgress(float progress)
    {
        _fill.fillAmount = progress;
    }
}
