using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System.Collections;

/// <summary>
/// Controls the Drag n' Snap behaviour
/// </summary>
public class SnapController : MonoBehaviour
{
    [Header("Snap Settings")]
    public float snapRange = 0.5f;

    [Header("Info Panel elements")]
    [SerializeField] private Image _mainImage;
    [SerializeField] private TMP_Text _captionInfoPanel;
    [SerializeField] private TMP_Text _descriptionInfoPanel;
    [SerializeField] private TMP_Text _titleInfoPanel;
    [SerializeField] private TMP_Text _rightWrongTitleInfoPanel;

    [SerializeField] private List<Transform> _snapPoints;
    [SerializeField] private Draggable[] _hasDraggableSnapped;
    [SerializeField] private List<Draggable> _draggableObjects;
    [SerializeField] private GameObject _greyPanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _incorrectPanel;
    [SerializeField] private CanvasGroup _mainGameCanvasGroup;
    [SerializeField] private CanvasGroup _greyCanvasGroup;
    [SerializeField] private CanvasGroup _toastCanvasGroup;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private List<Material> _snapMaterials;

    [SerializeField] private InfoPanelScriptableObject _infoPanelScriptableObject;
    private List<InfoPanelItem> _infoPanelScriptableObjects = new List<InfoPanelItem>();
    
    void Start()
    {
        _mainGameCanvasGroup.DOFade(0, 0);
        StartCoroutine(StartFadeInAnimation());
        _greyCanvasGroup.blocksRaycasts = false;

        // Initialize a list with Info Panel data
        _infoPanelScriptableObjects = _infoPanelScriptableObject.InfoPanelItems;

        foreach (var snapPoint in _snapPoints)
        {
            if (snapPoint.childCount > 0)
            {
                if (snapPoint.GetChild(0).GetComponent<Image>())
                {
                    _snapMaterials.Add(snapPoint.GetChild(0).GetComponent<Image>().material);
                }

            }
        }

        foreach (var _snapMaterial in _snapMaterials)
        {
            _snapMaterial.DOFade(0, 0);
        }
       
        foreach (var draggable in _draggableObjects)
        {
            draggable.dragEndedCallback += onDragEnded;
        }

        _hasDraggableSnapped = new Draggable[_snapPoints.Count];
    }

    private void onDragEnded(Draggable draggable)
    {
        float closestDistance = -1;
        Transform closestSnapPoint = null;
        int index = 0;
        int indexOfClosestSnapPoint = 0;
        foreach (var snapPoint in _snapPoints)
        {
            float currentDistance = Vector2.Distance(draggable.transform.position, snapPoint.position);
            if (closestSnapPoint == null || currentDistance < closestDistance)
            {
                closestSnapPoint = snapPoint;
                closestDistance = currentDistance;
                // get index of closest snap point
                indexOfClosestSnapPoint = index;
            }
            index++;
        }

        // check if we are close to a snap point
        if (closestSnapPoint != null && closestDistance <= snapRange)
        {
            // Check if closest Snap point is already matched
            if (_hasDraggableSnapped[indexOfClosestSnapPoint] != null)
            {
                // return the image in its initial position        
                draggable.transform.DOMove(draggable.spriteDragStartPosition, 1.0f).SetEase(Ease.OutExpo);
            }
            else
            {
                _hasDraggableSnapped[indexOfClosestSnapPoint] = draggable;
                // Save the draggable
                draggable.transform.position = closestSnapPoint.position;
                StartCoroutine(EvaluateSnap(draggable, indexOfClosestSnapPoint));
            }      
        }
        else
        {
            draggable.transform.DOMove(draggable.spriteDragStartPosition, 1.0f).SetEase(Ease.OutExpo);
            //hasDraggableSnapped[indexOfClosestSnapPoint] = null;
        }
    }

    private IEnumerator StartFadeInAnimation()
    {
        _mainGameCanvasGroup.DOFade(1, 1);
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// Checks if an image is already snapped
    /// </summary>
    /// <param name="draggable"></param>
    private void CheckIfImageWasAlreadySnapped(Draggable draggable)
    {
        for (int i = 0; i < _hasDraggableSnapped.Length; i++)
        {
            if (_hasDraggableSnapped[i] == draggable)
            {
                _hasDraggableSnapped[i] = null;
            }
        }
    }

    /// <summary>
    /// Check if all images are matched
    /// </summary>
    private void CheckIfAllImagesAreMatched()
    {
        if (_hasDraggableSnapped.Contains(null))
        {
            Debug.Log("Images are NOT Matched");
        }
        else
        {
            Debug.Log("All images are Matched");
            _sceneLoader.LoadAdditiveScene(2);
        }
    }

    private IEnumerator EvaluateSnap(Draggable draggable, int index)
    {

       if (_hasDraggableSnapped[index].name == _draggableObjects[index].name)
        {
            _greyCanvasGroup.blocksRaycasts = true;
            _snapPoints[index].gameObject.SetActive(false);
            StartCoroutine(draggable.OnSuccessAnimation());
            yield return new WaitForSeconds(2);
            _greyPanel.SetActive(true);
            SetInfoPanel(index);
            _infoPanel.SetActive(true);
            _greyCanvasGroup.DOFade(1.0f, 0.8f);          
            yield return new WaitForSeconds(0.8f);
            //OpenInfoPanel();
        }
        else
        {  
            if (draggable.isExtra)
            {
                _greyCanvasGroup.blocksRaycasts = true;
                StartCoroutine(OnFailPair(index));
                StartCoroutine(draggable.ExtraCardAnimation());
                yield return new WaitForSeconds(2);
                _greyPanel.SetActive(true);
                int indexDraggable = _draggableObjects.FindIndex(a => a == draggable);
                SetInfoPanel(indexDraggable);
                _infoPanel.SetActive(true);
                _greyCanvasGroup.DOFade(1.0f, 0.8f);            
                draggable.transform.DOMove(draggable.spriteDragStartPosition, 1.0f).SetEase(Ease.OutExpo); // position = draggable.spriteDragStartPosition;
                _hasDraggableSnapped[index] = null;
                draggable.StopDragMovement(true);
                yield return new WaitForSeconds(0.8f);
            }
            else
            {
                _greyCanvasGroup.blocksRaycasts = true;
                StartCoroutine(OnFailPair(index));         
                _incorrectPanel.SetActive(true);
                _toastCanvasGroup.DOFade(1.0f, 0.5f);
                yield return new WaitForSeconds(3);
                draggable.transform.DOMove(draggable.spriteDragStartPosition, 1.0f).SetEase(Ease.OutExpo); // position = draggable.spriteDragStartPosition;
                _hasDraggableSnapped[index] = null;
                _toastCanvasGroup.DOFade(0.0f, 0.5f);
                yield return new WaitForSeconds(1);
                _incorrectPanel.SetActive(false);
                _greyCanvasGroup.blocksRaycasts = false;

            }
        }
    }

    public void StartCloseInfoPanelAnimation()
    {
        if (_greyCanvasGroup.alpha == 1.0f)
        StartCoroutine(CloseInfoPanel());
    }

    private IEnumerator CloseInfoPanel()
    {
        _greyCanvasGroup.DOFade(0, 0.5f);    
        yield return new WaitForSeconds(0.5f);
        _greyCanvasGroup.blocksRaycasts = false;
        CheckIfAllImagesAreMatched();
    }

    private IEnumerator OnFailPair(int index)
    {
        _snapMaterials[index].DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(2.5f);
        _snapMaterials[index].DOFade(0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    private void SetInfoPanel(int index)
    {
        _mainImage.sprite = _infoPanelScriptableObjects[index].MainImage;
        _captionInfoPanel.text = _infoPanelScriptableObjects[index].Captions;
        _titleInfoPanel.text = _infoPanelScriptableObjects[index].Title;
        _rightWrongTitleInfoPanel.text = _infoPanelScriptableObjects[index].RightWrongTitle;
        _descriptionInfoPanel.text = _infoPanelScriptableObjects[index].Description;
    }
}

[Serializable]
public class QuizItem
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public bool isExtra;
}
