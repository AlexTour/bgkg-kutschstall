using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles the winning sequence
/// </summary>
public class WinBehaviour : MonoBehaviour
{
    [Header("Animation Settings")]
    [Range(0.5f, 10.0f)]
    [SerializeField] private float _secondsToWait = 8.0f;

    [Header("Trumbet Transforms")]
    [SerializeField] private Transform _left1;
    [SerializeField] private Transform _left2;
    [SerializeField] private Transform _left3;
    [SerializeField] private Transform _right1;
    [SerializeField] private Transform _right2;
    [SerializeField] private Transform _right3;

    [SerializeField] private ToggleUIWaveShader _image1, _image2, _image3, _image4;

    [Header("Other UI Elements")]
    [Space(20)]    
    [SerializeField] private CanvasGroup _CanvasGroup;
    [SerializeField] private CanvasGroup _imageMatCanvasGroup;
    [SerializeField] private SceneLoader _sceneLoader;

   // Start is called before the first frame update
   void Start()
    {
        // Init values
        _CanvasGroup.DOFade(0, 0);
        _imageMatCanvasGroup.DOFade(0, 0);

        StartCoroutine(StartWinningAnimation());
    }

    private IEnumerator StartWinningAnimation()
    {
        _CanvasGroup.DOFade(1, 2);
        yield return new WaitForSeconds(2f);


        StartCoroutine(PlayAnimation(_left3));
        StartCoroutine(PlayAnimation(_right3, isRight: true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayAnimation(_left2));
        StartCoroutine(PlayAnimation(_right2, isRight: true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayAnimation(_left1));
        StartCoroutine(PlayAnimation(_right1, isRight: true));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1.2f);

        _left2.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f);
        _right2.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        _left2.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        _right2.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);

        _left3.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f);
        _right3.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        _left3.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        _right3.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);

        _imageMatCanvasGroup.DOFade(1.0f, 2.0f);
        _image1.SetWavyMaterial();
        _image2.SetWavyMaterial();
        _image3.SetWavyMaterial();
        _image4.SetWavyMaterial();
        yield return new WaitForSeconds(_secondsToWait);
        _sceneLoader.LoadScene(0);
    }

    private IEnumerator PlayAnimation(Transform trumbetTransform, bool isRight = false)
    {
        if (isRight)
        {
            trumbetTransform.DOLocalMoveX(trumbetTransform.localPosition.x - 500, 1.5f);
        }
        else
        {
            trumbetTransform.DOLocalMoveX(trumbetTransform.localPosition.x + 500, 1.5f);
        }
        yield return new WaitForSeconds(1.2f);
        trumbetTransform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        trumbetTransform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        trumbetTransform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        trumbetTransform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
    }
}
