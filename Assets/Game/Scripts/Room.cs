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
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.transform.position = _playerSpawn.position;
            player.transform.rotation = _playerSpawn.rotation;
        }

        var pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.Value = _playerSpawn.rotation.eulerAngles.y;
    }

    private void Start()
    {
        OnGenerate?.Invoke();
        _door.OnAutoClosed.AddListener(OnLeaveRoom);
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
        var playerInput = FindObjectOfType<PlayerInputController>();
        playerInput.enabled = false;
        playerInput.ResetInput();

        var dA = Vector3.Distance(_hunterA.transform.position, _failCamera.transform.position);
        var dB = Vector3.Distance(_hunterB.transform.position, _failCamera.transform.position);

        var hunter = dA < dB ? _hunterB : _hunterA;
        _failCamera.LookAt = hunter;
        _failCamera.Priority = 20;

        var hunterAnimator = hunter.GetComponentInChildren<Animator>();
        hunterAnimator.SetTrigger("Shoot");

        var hunterIK = hunterAnimator.GetComponent<OverseerIK>();
        hunterIK.LookAtPosition = playerInput.transform.position + Vector3.up * 1.8f; 

        foreach (var act in GetComponentsInChildren<Actor>())
        {
            act.DeactivateInteraction();
        }

        yield return new WaitForSeconds(2f);

        // audio
        _audioSource.Play();

        yield return new WaitForSeconds(1f);

        _startUI.SetActive(true);


  

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

        yield return new WaitUntil(() => !_startUI.activeSelf);

        // xxxx
        _failCamera.Priority = 5;
        playerInput.enabled = true;

        // Reset room
        SuspicionLevel = 0f;
        OnGenerate?.Invoke();
        SpawnPlayer();

        IsRoomEnding = false;
    }
}