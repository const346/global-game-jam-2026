using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [SerializeField] private Image _fadeUI;
    [SerializeField] private RoomUI _roomUI;
    [SerializeField] private ProgressUI _progressUI;
    [SerializeField] private GameObject _startUI;
    [SerializeField] private CinemachineVirtualCamera _failCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    
    [SerializeField] private Room _previousRoom;
    [SerializeField] private Door _door;
    [SerializeField] private Transform _playerSpawn;

    [SerializeField] private Transform _hunterA;
    [SerializeField] private Transform _hunterB;

    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    
    [Header("Suspicion")]
    [SerializeField] private float SuspicionIncreaseAmount = 0.2f;
    [SerializeField] private float SuspicionSpeed = 0.01f;
    [SerializeField] private float SuspicionLevel = 0f;

    public UnityEvent OnGenerate;

    [Header("DEBUG ONLY")]
    public Actor CorrectActor;
    public bool IsRoomCompleted;
    public bool IsRoomEnding;

    private PlayerMovement _playerMovement;

    public void OnInteract(Actor actor)
    {
        if (actor == CorrectActor)
        {
            foreach (var act in GetComponentsInChildren<Actor>())
            {
                act.DeactivateInteraction();
            }

            _door.Open();
            SuspicionLevel = 0f;

            _progressUI.SetProgress(SuspicionLevel);
        }
        else
        {
            SuspicionLevel += SuspicionIncreaseAmount;
            SuspicionLevel = Mathf.Clamp01(SuspicionLevel);
        }
    }

    public void SpawnPlayer()
    {
        var characterController = _playerMovement.GetComponent<CharacterController>();
        characterController.enabled = false;

        _playerMovement.transform.position = _playerSpawn.position;
        _playerMovement.transform.rotation = _playerSpawn.rotation;

        characterController.enabled = true;

        var pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.Value = _playerSpawn.rotation.eulerAngles.y;
        pov.m_VerticalAxis.Value = 0;
    }

    private IEnumerator Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();

        OnGenerate?.Invoke();
        _door.OnAutoClosed.AddListener(OnLeaveRoom);

        if (_previousRoom == null)
        {
            var pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
            pov.enabled = false;

            _startUI.gameObject.SetActive(true);

            var playerInput = _playerMovement.GetComponent<PlayerInputController>();
            playerInput.enabled = false;
            playerInput.ResetInput();

            IsRoomEnding = true;

            yield return new WaitUntil(() => !_startUI.activeSelf);

            IsRoomEnding = false;

            pov.enabled = true;

            playerInput.enabled = true;
            SpawnPlayer();
        }
    }

    private void Update()
    {
        if (IsRoomEnding)
        {
            return;
        }

        if (_previousRoom != null && 
            !_previousRoom.IsRoomCompleted)
        {
            return;
        }

        if (IsRoomCompleted || _door.IsOpen)
        {
            return;
        }

        SuspicionLevel += Time.deltaTime * SuspicionSpeed;
        SuspicionLevel = Mathf.Clamp01(SuspicionLevel);

        if (SuspicionLevel >= 1f)
        {
            IsRoomEnding = true;
            StartCoroutine(RoomEnding());
        }

        _progressUI.SetProgress(SuspicionLevel);
    }

    private void OnLeaveRoom()
    {
        IsRoomCompleted = true;
        _roomUI.SetCheck(true);
    }

    private IEnumerator RoomEnding()
    {
        // Disable player control
        var playerInput = _playerMovement.GetComponent<PlayerInputController>();
        playerInput.enabled = false;
        playerInput.ResetInput();

        var dA = Vector3.Distance(_hunterA.transform.position, _failCamera.transform.position);
        var dB = Vector3.Distance(_hunterB.transform.position, _failCamera.transform.position);

        var hunter = dA < dB ? _hunterB : _hunterA;
        _failCamera.LookAt = hunter;
        _failCamera.Priority = 20;

        var hunterAnimator = hunter.GetComponentInChildren<Animator>();
        hunterAnimator.SetTrigger("Shoot");

        foreach (var act in GetComponentsInChildren<Actor>())
        {
            act.DeactivateInteraction();
        }

        yield return new WaitForSeconds(2f);

        // audio
        _audioSource.Play();

        yield return new WaitForSeconds(1f);

        hunterAnimator.Play("Idle", 0);

        _startUI.SetActive(true);

        SpawnPlayer();
        _failCamera.Priority = 5;

        var pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.enabled = false;

        yield return FadeEffect();
        yield return new WaitUntil(() => !_startUI.activeSelf);
        
        pov.enabled = true;

        // Reset room
        SuspicionLevel = 0f;
        OnGenerate?.Invoke();
        playerInput.enabled = true;

        IsRoomEnding = false;
    }

    private IEnumerator FadeEffect()
    {
        float t = 0f;
        var color = _fadeUI.color;
        while (t < 1f)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0f, t / 1f);

            _fadeUI.color = color;

            yield return null;
        }

        color.a = 0f;
        _fadeUI.color = color;
    }
}