using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private ActorIK _actorIK;
    [SerializeField] private bool isCorrect;

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

    private void OnInteract()
    {
        var playerSuspicion = FindObjectOfType<PlayerSuspicion>();


        _actorIK.Trigger();

        if (!isCorrect)
        {
            playerSuspicion.IncreaseSuspicion();
            _animator.SetTrigger("leftInteract");
        }
        else
        {
            playerSuspicion.ResetSuspicion();
            _animator.SetTrigger("rightInteract");

            var room = GetComponentInParent<Room>();
            room?.Complete();
        }
    }

    private void UpdateIK() // for test
    {
        var playerSuspicion = FindObjectOfType<PlayerSuspicion>();
        _actorIK.Weight = playerSuspicion.SuspicionLevel;
        _actorIK.LookAtPosition = playerSuspicion.transform.position + Vector3.up * 1.8f;
    }

    public void ApplyCustomize(ActorMask actorMask)
    {

    }

    public void SetCorrect(bool correct)
    {
        isCorrect = correct;
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