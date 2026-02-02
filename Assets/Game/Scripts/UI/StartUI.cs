using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(OnStartGame);
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnStartGame()
    {
        gameObject.SetActive(false);
    }
}
