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
    public TextMeshProUGUI text;
    private bool isSelected;
    public Color normalColor;
    public Color highlightColor;
    public AudioClip onHoverClip;
    public UnityEvent onClick;
    private AudioSource audioSource;
    private Image background;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
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
    public void Deselect()
    {
        if (text == null) Init();
        decoration.enabled = false;
        text.fontSize = normalFontSize;
        text.color = normalColor;

    }

    public void Select()
    {
        if (text == null) Init();
        text.fontSize = highlightFontSize;
        text.color = highlightColor;
        decoration.enabled = true;
    }

    public void SetAsSelection(bool selected)
    {
        isSelected = selected;
        if (selected) { Select(); } else Deselect();
    }
   void Init()
    {
        audioSource = GetComponent<AudioSource>();
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.fontSize = normalFontSize;
        text.color = normalColor;
        if (decoration != null) decoration.enabled = false;
    }
}
