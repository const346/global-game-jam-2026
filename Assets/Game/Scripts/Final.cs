using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Image _fadeUI;
    [SerializeField] private GameObject _finalUI;

    private void Start()
    {
        _door.OnAutoClosed.AddListener(() =>
        {
            _finalUI.SetActive(true);
            StartCoroutine(ShowFinal());
        });
    }

    private IEnumerator ShowFinal()
    {
        float t = 0f;
        var color = _fadeUI.color;
        while (t < 1f)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0f, t / 1f);

            _fadeUI.color = color;

            yield return null;
        }

        color.a = 0f;
        _fadeUI.color = color;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
