using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private RectTransform _stickConteiner;

    public void SetProgress(float progress)
    {
        _fill.fillAmount = progress;

        _stickConteiner.anchorMax = new Vector2(1, progress);
    }
}
