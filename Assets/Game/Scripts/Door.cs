using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private Collider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerExit(Collider other)
    {
        var position = other.transform.position;
        var localP = _collider.transform.InverseTransformPoint(position);

        if (localP.z > 0)
        {
            Close();
        }
    }

    public void Close()
    {
        _collider.isTrigger = false;
        _renderer.enabled = true;
    }

    public void Open()
    {
        _collider.isTrigger = true;
        _renderer.enabled = false;
    }
}
