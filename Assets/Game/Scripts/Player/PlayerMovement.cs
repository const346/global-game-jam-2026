using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _speed = 5f;

    public Vector2 Input { get; set; }

    private float velocityY;

    private void Update()
    {
        var movement = new Vector3(Input.x, 0, Input.y) * _speed;
        velocityY += -_gravity * Time.deltaTime;

        movement.y = velocityY;
        var collisionFlags = _controller.Move(movement * Time.deltaTime);

        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            velocityY = 0;
        }

        if ((collisionFlags & CollisionFlags.Above) != 0 && velocityY > 0)
        {
            velocityY = 0;
        }
    }
}
