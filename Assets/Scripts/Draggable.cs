using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void DragEndedDelegate(Draggable draggableObject);
    public DragEndedDelegate dragEndedCallback;

    private bool isDragged;
    private bool firstTime = true;

    [HideInInspector]
    public Vector3 spriteDragStartPosition;
    private Vector2 offset;
    private Sprite mainSprite;
    private bool isMatched = false;
    public bool isExtra; 
    private Material highlightedMat;
    private Image highlightedImage;

    private  Color32 noColor = new Color32(255, 255, 255, 0);
    private Color32 withColor = new Color32(255, 255, 255, 255);
    private  Color32 lightGreen = new Color32(103, 255, 108, 255);
    private  Color32 darkGreen = new Color32(112, 181, 114, 255);
    private Color32 lightRed = new Color32(198, 42, 42, 255);
    private Color32 darkRed = new Color32(255, 0, 0, 255);


    void Start()
    {
        isExtra = this.gameObject.name.Contains("extra");
        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).GetComponent<Image>())
            {
                highlightedImage = transform.GetChild(0).GetComponent<Image>();
                highlightedMat = highlightedImage.material;
            }
            else
            {
                highlightedImage = null;
                highlightedMat = null;
            }
        }
        else if (isExtra)
        {
            highlightedImage = transform.gameObject.GetComponent<Image>();
            highlightedMat = highlightedImage.material;
            highlightedMat.DOFade(1, 0);
        }
    }

    void Update()
    {
        if (isDragged && !isMatched)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset; ;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragged = true;

        if (firstTime)
        {
            spriteDragStartPosition = transform.position;
            firstTime = false;
        }
        
        offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragged = false;
        if (!isMatched)
        dragEndedCallback(this);
    }

    /// <summary>
    /// It sets the sprite of the Draggable
    /// </summary>
    /// <param name="sprite">The sprite to set</param>
    public void SetSprite(Sprite sprite)
    {
        highlightedImage.sprite = sprite;
        mainSprite = sprite;

    }

    /// <summary>
    /// Gets the main sprite of the Draggable
    /// </summary>
    /// <returns></returns>
    public Sprite GetSprite()
    {  
        return mainSprite;
    }

    public IEnumerator OnSuccessAnimation()
    {
        isMatched = true;
        highlightedMat.DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(1.5f);
        highlightedMat.DOFade(0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Color should change!!");  
    }

    public IEnumerator ExtraCardAnimation()
    {
        isMatched = true;
        highlightedMat.DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(1.5f);
        highlightedMat.DOFade(0.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("ExtraCardAnimation");
    }

    public void StopDragMovement(bool state)
    {
        isMatched = state;
    }

}
