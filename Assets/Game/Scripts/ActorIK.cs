using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActorIK : MonoBehaviour
{
    public Vector3 LookAtPosition { get; set; }
    public float Weight { get; set; }

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("ActorIK OnAnimatorIK called");

        _animator.SetLookAtWeight(1, 1, 1);
        _animator.SetLookAtPosition(LookAtPosition);
    }
}