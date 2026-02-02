using Cinemachine;
using UnityEngine;

public class PlayerStepEffect : MonoBehaviour
{
    [SerializeField] private Transform _ct;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private CharacterController _characterController;

    [Header("Step motion")]
    [SerializeField] private float _amplitude = 0.04f;
    [SerializeField] private float _frequency = 12f;
    [SerializeField] private float _motionTreshold = 0.1f;

    [Header("Step sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepClips;

    private Vector3 _defaultFollowOffset;
    private Vector3 _targetFollowOffset;
    private float _lastStep;
    private CinemachineTransposer _transposer;

    private void Start()
    {
        _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        _defaultFollowOffset = _transposer.m_FollowOffset;
    }

    private void FixedUpdate()
    {
        var velocity = _characterController.velocity.magnitude;

        if (velocity > _motionTreshold)
        {
            _targetFollowOffset.y = _defaultFollowOffset.y + Mathf.Sin(Time.time * _frequency) * 
                _amplitude * (1 - Mathf.Abs(_ct.localRotation.x)) * velocity;

            _targetFollowOffset.x = _defaultFollowOffset.x + Mathf.Cos(Time.time * _frequency / 2f) * 
                (_amplitude / 2f) * velocity;

            TryPlayFootstep();
        }
        else
        {
            _targetFollowOffset = _defaultFollowOffset;
        }
    }

    private void Update()
    {
        _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * 5f);
    }

    private void TryPlayFootstep()
    {
        var step = Mathf.Sin(Time.time * _frequency);
        if (_lastStep < 0f && step >= 0f)
        {
            _audioSource.PlayOneShot(_footstepClips[Random.Range(0, _footstepClips.Length)]);
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.volume = Random.Range(0.8f, 1f);
        }

        _lastStep = step;
    }
}