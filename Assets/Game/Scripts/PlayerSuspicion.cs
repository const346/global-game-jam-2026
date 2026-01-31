using UnityEngine;

public class PlayerSuspicion : MonoBehaviour
{
    public float SuspicionIncreaseAmount = 0.2f;
    public float SuspicionSpeed = 0.01f;
    public float SuspicionLevel = 0f;

    private void Update()
    {
        SuspicionLevel += Time.deltaTime * SuspicionSpeed;
        SuspicionLevel = Mathf.Clamp01(SuspicionLevel);
    }

    public void IncreaseSuspicion()
    {
        SuspicionLevel += SuspicionIncreaseAmount;
        SuspicionLevel = Mathf.Clamp01(SuspicionLevel);
    }

    public void ResetSuspicion()
    {
        SuspicionLevel = 0f;
    }
}
