using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _failCamera;

    [SerializeField] private Room _previousRoom;
    [SerializeField] private Door _door;
    [SerializeField] private Transform _playerSpawn;

    [SerializeField] private Transform _hunterA;
    [SerializeField] private Transform _hunterB;

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
    }

    private void OnLeaveRoom()
    {
        IsRoomCompleted = true;
    }

    private IEnumerator RoomEnding()
    {
        // Disable player control
        var playerInput = FindObjectOfType<PlayerInputController>();
        playerInput.enabled = false;

        var dA = Vector3.Distance(_hunterA.transform.position, _failCamera.transform.position);
        var dB = Vector3.Distance(_hunterB.transform.position, _failCamera.transform.position);

        _failCamera.LookAt = dA < dB ? _hunterB : _hunterA;
        _failCamera.Priority = 20;

        yield return new WaitForSeconds(3f);

        playerInput.enabled = true;
        _failCamera.Priority = 5;

        // Reset room
        SuspicionLevel = 0f;
        OnGenerate?.Invoke();
        SpawnPlayer();

        IsRoomEnding = false;
    }
}