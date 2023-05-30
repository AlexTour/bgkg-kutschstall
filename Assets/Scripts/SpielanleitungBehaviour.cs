using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Controls the behaviour of Spielanleitung scene
/// </summary>
public class SpielanleitungBehaviour : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [Range(0.1f, 3)]
    [SerializeField] private float _fadeInDuration;

    void Start()
    {
        _canvasGroup.DOFade(0, 0);
        StartCoroutine(StartFadeInAnimation());
    }

    private IEnumerator StartFadeInAnimation()
    {
        _canvasGroup.DOFade(1, _fadeInDuration);
        yield return new WaitForSeconds(_fadeInDuration);
    }
}
