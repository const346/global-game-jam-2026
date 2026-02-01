using Cinemachine;
using UnityEngine;

public class PlayerHeadEffect : MonoBehaviour
{
    [SerializeField] private Transform _ct;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float frequencyWalk = 1;
    [SerializeField] private float motionTreshold = 0;

    private Vector3 defaultHeadPosition;
    private Vector3 headPosition;

    private void Start()
    {
        var ct = _camera.GetCinemachineComponent<CinemachineTransposer>();
        defaultHeadPosition = ct.m_FollowOffset;
    }

    private void FixedUpdate()
    {
        var velocity = _characterController.velocity.magnitude;

        if (velocity > motionTreshold)
        {
            headPosition.y = defaultHeadPosition.y + Mathf.Sin(Time.time * frequencyWalk) * 
                amplitude * (1 - Mathf.Abs(_ct.localRotation.x)) * velocity;

            headPosition.x = defaultHeadPosition.x + Mathf.Cos(Time.time * frequencyWalk / 2f) * 
                (amplitude / 2f) * velocity;
        }
        else
        {
            headPosition = defaultHeadPosition;
        }

        var ct = _camera.GetCinemachineComponent<CinemachineTransposer>();
        ct.m_FollowOffset = Vector3.Lerp(ct.m_FollowOffset, headPosition, Time.deltaTime * 5f);
    }
}