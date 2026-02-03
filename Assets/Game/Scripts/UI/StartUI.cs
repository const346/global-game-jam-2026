using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button _button;

    // temporary
    [SerializeField] private RectTransform _controls;
    [SerializeField] private PlayerInput _playerInput;

    private void Start()
    {
        _button.onClick.AddListener(OnStartGame);
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        _controls.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_playerInput.currentControlScheme == "Touch")
        {
            _controls.gameObject.SetActive(true);
        }
        else
        {
            _controls.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnStartGame()
    {
        gameObject.SetActive(false);
    }
}
