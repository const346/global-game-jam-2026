using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteractor _playerInteractor;

    private Vector2 _inputMove;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerInteractor.Interact();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
    }

    private void Update() // temporary
    {
        var rt = Quaternion.Euler(0f, 0f, -_camera.transform.eulerAngles.y);
        _playerMovement.Input = rt * _inputMove;
        _playerInteractor.Look = _camera.transform.forward;
    }

    public void ResetInput() // temporary
    {
        _playerMovement.Input = Vector2.zero;
        _playerInteractor.Look = Vector2.zero;
    }
}
