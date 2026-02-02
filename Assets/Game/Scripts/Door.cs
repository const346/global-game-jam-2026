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
    [SerializeField] private OcclusionPortal _occlusionPortal; 

    private Collider _collider;

    public UnityEvent OnAutoClosed;
    public bool IsOpen => _collider.isTrigger;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        _occlusionPortal.open = false;
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

        StartCoroutine(CloseOcclusionPortal());
    }

    private IEnumerator CloseOcclusionPortal()
    {
        yield return new WaitForSeconds(2.0f);
        _occlusionPortal.open = false;
    }

    public void Open()
    {
        _animator.SetBool("Open", true);

        _audioSource.Play();

        _collider.isTrigger = true;
        _occlusionPortal.open = true;
        //_renderer.enabled = false;
    }
}
