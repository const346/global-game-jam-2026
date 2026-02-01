using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OverseerIK : MonoBehaviour
{
    [SerializeField] private float weight = 1.0f;
    [SerializeField] private float bodyWeight = 1.0f;
    [SerializeField] private float headWeight = 1.0f;

    [SerializeField] private Transform lookAtTarget;
    //public Vector3 LookAtPosition { get; set; }
    public float Weight { get; set; }

    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _animator.SetLookAtWeight(weight, bodyWeight, headWeight * headWeight);
        _animator.SetLookAtPosition(lookAtTarget.position);
    }
}
