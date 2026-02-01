using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Animator _animator;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private ActorIK _actorIK;

    [Header("Customize")]
    [SerializeField] private Transform _maskA;
    [SerializeField] private Transform _maskB;
    [SerializeField] private Transform _maskC;
    [Space]
    [SerializeField] private Transform _deerHorn;
    [SerializeField] private Transform _goatHorn;
    [SerializeField] private Transform _ramHorn;
    [Space]
    [SerializeField] private Color _colorWhite = Color.white;
    [SerializeField] private Color _colorBlack = Color.black;
    [SerializeField] private Color _colorRed = Color.red;
    [Space]
    [SerializeField] private Renderer[] _renderers;


    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    public void DeactivateInteraction()
    {
        _interactable.IsDeactivated = true;
    }

    private void Start()
    {
        _interactable.OnInteracted.AddListener(OnInteract);
    }

    private void Update()
    {
        _animator.SetBool("isInteracting", _interactable.WasRecentlyInteractive());

        UpdateIK();
    }

    private void UpdateIK()
    {
        var player = FindObjectOfType<PlayerMovement>();
        //_actorIK.Weight = playerSuspicion.SuspicionLevel;
        _actorIK.LookAtPosition = player.transform.position + Vector3.up * 1.8f;
    }

    private void OnInteract()
    {
        var room = GetComponentInParent<Room>();
        room?.OnInteract(this);

        _actorIK.Trigger();

        var isCorrect = room != null && room.CorrectActor == this;
        _animator.SetTrigger(isCorrect ? "rightInteract" : "leftInteract");
    }

    public void ApplyCustomize(ActorMask actorMask)
    {
        // mask
        _maskA.gameObject.SetActive(actorMask.Type == "TypeA");
        _maskB.gameObject.SetActive(actorMask.Type == "TypeB");
        _maskC.gameObject.SetActive(actorMask.Type == "TypeC");

        // horn
        _deerHorn.gameObject.SetActive(actorMask.Horn == "Deer");
        _goatHorn.gameObject.SetActive(actorMask.Horn == "Goat");
        _ramHorn.gameObject.SetActive(actorMask.Horn == "Ram");

        // color
        if (_mpb == null)
        {
            _mpb = new MaterialPropertyBlock();
        }

        var colors = new Dictionary<string, Color>
        {
            { "White", _colorWhite },
            { "Black", _colorBlack },
            { "Red",_colorRed }
        };

        foreach (var renderer in _renderers)
        {
            renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(ColorId, colors[actorMask.Color]);
            renderer.SetPropertyBlock(_mpb);
        }
    }
}

public struct ActorMask
{
    public string Type;
    public string Color;
    public string Horn;
    public bool HasGold;
    public bool HasPattern;
}

//public struct ActorMask
//{
//    public MaskType Type;
//    public ColorMaskType Color;
//    public HornType Horn;
//    public bool HasGold;
//    public bool HasPattern;
//}

//public enum HornType
//{
//    None,
//    Deer,
//    Goat,
//    Ram
//}

//public enum MaskType
//{
//    MaskA,
//    MaskB,
//    MaskC
//}

//public enum ColorMaskType
//{
//    White,
//    Red,
//    Black
//}