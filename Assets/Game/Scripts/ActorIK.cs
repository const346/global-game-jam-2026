using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActorIK : MonoBehaviour
{
    [SerializeField] private float weight = 1.0f;
    [SerializeField] private float bodyWeight = 1.0f;
    [SerializeField] private float headWeight = 1.0f;

    public Vector3 LookAtPosition { get; set; }
    public float Weight { get; set; }

    private Animator _animator;

    private float _triggerTime;
    private float _scaleHeadWeight;

    public void Trigger()
    {
        _triggerTime = Time.time;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var w = Mathf.InverseLerp(0, 2, Time.time - _triggerTime);
        _scaleHeadWeight = Mathf.MoveTowards(_scaleHeadWeight, w, 4 * Time.deltaTime);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _animator.SetLookAtWeight(weight, bodyWeight, headWeight * _scaleHeadWeight);
        _animator.SetLookAtPosition(LookAtPosition);
    }
}