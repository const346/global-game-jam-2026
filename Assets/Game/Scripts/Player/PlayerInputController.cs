using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteractor _playerInteractor;

    private InputSystem_Actions _actions;

    private void OnEnable()
    {
        _actions = new InputSystem_Actions();
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    private void Update()
    {
        // cursor lock
        if (_actions.Player.Crouch.WasPressedThisFrame())
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        // movement
        var moveInput = _actions.Player.Move.ReadValue<Vector2>();
        var cameraYaw = _camera.transform.eulerAngles.y;

        _playerMovement.Input = Quaternion.Euler(0f, 0f, -cameraYaw) * moveInput;

        // interaction
        _playerInteractor.Look = _camera.transform.forward;

        if (_actions.Player.Interact.WasPressedThisFrame())
        {
            _playerInteractor.Interact();
        }
    }

    public void ResetInput()
    {
        _playerMovement.Input = Vector2.zero;
        _playerInteractor.Look = Vector2.zero;
    }
}
