using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Animator _animator;

    private Collider _collider;

    public UnityEvent OnAutoClosed;
    public bool IsOpen => _collider.isTrigger;

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
            OnAutoClosed?.Invoke();
        }
    }

    public void Close()
    {
        _animator.SetBool("Open", false);

        _audioSource.Play();

        _collider.isTrigger = false;
        //_renderer.enabled = true;
    }

    public void Open()
    {
        _animator.SetBool("Open", true);

        _audioSource.Play();

        _collider.isTrigger = true;
        //_renderer.enabled = false;
    }
}
