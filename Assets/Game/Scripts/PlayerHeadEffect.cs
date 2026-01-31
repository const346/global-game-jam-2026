using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerHeadEffect : MonoBehaviour
{
    [SerializeField] private Transform headHolder;
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float frequencyWalk = 1;
    [SerializeField] private float motionTreshold = 0;

    private Vector3 defaultHeadPosition;
    private Vector3 headPosition;

    public float Velocity { get; set; }

    private void Start()
    {
        defaultHeadPosition = headHolder.localPosition;
    }

    private void FixedUpdate()
    {
        if (Velocity > motionTreshold)
        {
            headPosition.y = defaultHeadPosition.y + Mathf.Sin(Time.time * frequencyWalk) * 
                amplitude * (1 - Mathf.Abs(headHolder.localRotation.x)) * Velocity;

            headPosition.x = defaultHeadPosition.x + Mathf.Cos(Time.time * frequencyWalk / 2f) * 
                (amplitude / 2f) * Velocity;
        }
        else
        {
            headPosition = defaultHeadPosition;
        }

        headHolder.localPosition = Vector3.Lerp(headHolder.localPosition, headPosition, Time.deltaTime * 5f);
    }
}