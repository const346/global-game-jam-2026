using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenPointer : OnScreenControl, IDragHandler
{
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    private Vector2 _delta;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _delta = eventData.delta;
    }

    private void LateUpdate()
    {
        SendValueToControl(_delta);
        _delta = Vector2.zero;
    }

    private void OnDisable()
    {
        _delta = Vector2.zero;
        SendValueToControl(_delta);
    }
}