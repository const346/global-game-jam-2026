using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private static readonly float _maxAngle = 45;
    private static readonly float _threshold = Mathf.Cos(_maxAngle * Mathf.Deg2Rad);

    [SerializeField] private Transform _face;

    public UnityEvent OnInteracted;
    public Transform Face => _face;
    public bool IsDeactivated {  get; set; }


    private int frameIndex = -1000;

    public void OnInteract()
    {
        OnInteracted?.Invoke();
    }

    public bool IsInteractable(Vector3 look)
    {
        return !IsDeactivated && Vector3.Dot(_face.forward, look.normalized) < _threshold * -1;
    }

    private void OnDrawGizmosSelected()
    {
        if (_face != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_face.position, _face.position + _face.forward * 2f);
        }
    }

    public void ApplyFrameInteractive()
    {
        frameIndex = Time.frameCount;
    }

    public bool WasRecentlyInteractive()
    {
        return Time.frameCount - frameIndex < 10;
    }
}
