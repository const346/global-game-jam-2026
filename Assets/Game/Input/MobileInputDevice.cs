using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
[InputControlLayout(displayName = "Mobile",
stateType = typeof(MobileInputDevice),
description = "Contains on screen controls for mobile devices")]
public class MobileInputDevice : InputDevice
{
    [InputControl(name = "Stick", layout = "stick")]
    public StickControl Stick { get; set; }

    [InputControl(name = "Delta", layout = "vector2")]
    public Vector2Control Delta { get; set; }

    [InputControl(name = "E", layout = "button")]
    public ButtonControl E { get; set; }

    static MobileInputDevice()
    {
        InputSystem.RegisterLayout<MobileInputDevice>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeInPlayer()
    {
        InputSystem.AddDevice<MobileInputDevice>();
    }

    protected override void FinishSetup()
    {
        base.FinishSetup();

        Stick = GetChildControl<StickControl>(nameof(Stick));
        Delta = GetChildControl<Vector2Control>(nameof(Delta));

        E = GetChildControl<ButtonControl>(nameof(E));
    }
}