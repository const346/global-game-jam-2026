using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] private Room _previousRoom;
    [SerializeField] private Door _door;
    [SerializeField] private Transform _playerSpawn;

    [Header("Suspicion")]
    [SerializeField] private float SuspicionIncreaseAmount = 0.2f;
    [SerializeField] private float SuspicionSpeed = 0.01f;
    [SerializeField] private float SuspicionLevel = 0f;

    public UnityEvent OnGenerate;

    public Actor CorrectActor;
    public bool RoomCompleted;

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
        if (_previousRoom != null && 
            !_previousRoom.RoomCompleted)
        {
            return;
        }

        if (RoomCompleted || _door.IsOpen)
        {
            return;
        }

        SuspicionLevel += Time.deltaTime * SuspicionSpeed;
        SuspicionLevel = Mathf.Clamp01(SuspicionLevel);

        if (SuspicionLevel >= 1f)
        {
            OnEndRoom();
        }
    }

    private void OnLeaveRoom()
    {
        RoomCompleted = true;
    }

    private void OnEndRoom()
    {
        SuspicionLevel = 0f;
        OnGenerate?.Invoke();

        SpawnPlayer();
    }
}