using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform _hint;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector3 _offset = Vector3.up;
    [SerializeField] private float _radius = 2f;

    private Interactable _currentInteractable;

    public Vector3 Look { get; set; }

    private void FixedUpdate()
    {
        var playerPosition = transform.position + _offset;
        var hits = Physics.OverlapSphere(transform.position + _offset, _radius, _layerMask);

        var minDistance = float.MaxValue;
        var closestInteractable = default(Interactable);

        var plane = new Plane(Look, playerPosition);
        

        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<Interactable>();
            if (interactable != null && plane.GetSide(interactable.Face.position) && interactable.IsInteractable(Look))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        _currentInteractable = closestInteractable;
        _currentInteractable?.ApplyFrameInteractive();

        // debug
        if (_currentInteractable != null)
        {
            var c = _currentInteractable.IsInteractable(Look) ? Color.green : Color.red;
            Debug.DrawLine(transform.position + _offset, _currentInteractable.Face.transform.position, c);
        }

        _hint?.gameObject.SetActive(_currentInteractable != null);
    }

    public void Interact()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.OnInteract();
        }
    }
}
