using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool interactible;
    public Image decoration;
    public Sprite normalBackground;
    public Sprite hoveredBackground;
    public int normalFontSize;
    public int highlightFontSize;
    public string title;
    private bool isSelected;
    public Color normalColor;
    public Color highlightColor;
    public AudioClip onHoverClip;
    public UnityEvent onClick;
    private AudioSource audioSource;
    private Image background;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.fontSize = normalFontSize;
        text.color = normalColor;
        if(decoration != null) decoration.enabled = false;
        if (title != string.Empty) text.text = title;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();

        if (interactible)
        {
            Select();
            isSelected = true;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
       
        //audioSource.PlayOneShot(onHoverClip);
        Select();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (interactible)
        {
            if (!isSelected)
            {
                Deselect();
            }
        }
        else
        {
            Deselect();
        
        }

    }
    private void Deselect()
    {
        decoration.enabled = false;
        text.fontSize = normalFontSize;
        text.color = normalColor;

    }

    private void Select()
    {
        text.fontSize = highlightFontSize;
        text.color = highlightColor;
        decoration.enabled = true;
    }

    public void SetAsSelection(bool selected)
    {
        isSelected = selected;
        if (selected) { Select(); } else Deselect();
    }
   
}
