using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private ActorIK _actorIK;

    [SerializeField] private Door _rightDoor;

    private void Start()
    {
        _interactable.OnInteracted.AddListener(OnInteract);
    }

    private void Update()
    {
        _animator.SetBool("isInteracting", _interactable.WasRecentlyInteractive());

        var playerSuspicion = FindObjectOfType<PlayerSuspicion>();
        _actorIK.Weight = playerSuspicion.SuspicionLevel;
        _actorIK.LookAtPosition = playerSuspicion.transform.position + Vector3.up * 1.8f;
    }

    private void OnInteract()
    {
        var playerSuspicion = FindObjectOfType<PlayerSuspicion>();

        if (Random.value > 0.5f)
        {
            playerSuspicion.IncreaseSuspicion();
            _animator.SetTrigger("leftInteract");
        }
        else
        {
            playerSuspicion.ResetSuspicion();
            _animator.SetTrigger("rightInteract");
            _rightDoor?.Open();
        }
    }
}