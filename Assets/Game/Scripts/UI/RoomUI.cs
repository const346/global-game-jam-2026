using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _off;
    [SerializeField] private Sprite _on;

    public void SetCheck(bool check)
    {
        _image.sprite = check ? _on : _off;
    }
}
